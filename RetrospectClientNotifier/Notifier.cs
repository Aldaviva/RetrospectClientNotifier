using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataSizeUnits;
using Microsoft.Toolkit.Uwp.Notifications;
using murrayju.ProcessExtensions;
using RetrospectClientNotifier.Events;

#nullable enable

namespace RetrospectClientNotifier {

    internal static class Notifier {

        private const string PIPE_NAME    = "RetrospectClientNotifier";
        private const string COMMAND_EXIT = "exit";

        [STAThread]
        private static void Main(string[] malformedArgs) {
            if (ToastNotificationManagerCompat.WasCurrentProcessToastActivated()) {
                //user clicked a toast, do nothing and exit
            } else if (!Environment.UserInteractive) {
                //if running as LocalSystem, we can't show UI, so relaunch in the console user's session
                ProcessExtensions.StartProcessAsCurrentUser(Application.ExecutablePath, Environment.CommandLine);
            } else {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                closeOtherInstances();

                //pcpds.exe doesn't properly escape paths ending with \ in argument string, so turn "G:\" into "G:\\" and then manually parse
                string   commandLineEscaped = Environment.CommandLine.Replace(@"\"" ", @"\\"" ");
                string[] args               = CommandLine.commandLineToArgs(commandLineEscaped).Skip(1).ToArray();

                RetrospectEvent retrospectEvent = RetrospectEventParser.parseEvent(args);

                RetrospectNotification notification = formatNotification(retrospectEvent);
                TrayIcon?              trayIcon     = null;

                if (retrospectEvent is StartSourceEvent) {
                    trayIcon = new TrayIcon($"Retrospect backup started at {DateTime.Now:t}");
                }

                NotificationToast.hideNotifications();
                NotificationToast.showNotification(notification);

                if (trayIcon is not null) {
                    listenForExitMessages(trayIcon);
                    Application.Run();
                }
            }
        }

        private static void listenForExitMessages(Form trayIcon) {
            NamedPipeServerStream pipeServer = new(PIPE_NAME, PipeDirection.In, 1, PipeTransmissionMode.Message);
            pipeServer.WaitForConnectionAsync().ContinueWith(async _ => {
                using StreamReader reader = new(pipeServer, Encoding.UTF8);

                string message = await reader.ReadToEndAsync();
                if (message == COMMAND_EXIT) {
                    trayIcon.Close(); //needed to quickly hide the tray icon instead of waiting
                    pipeServer.Close();
                    Application.Exit();
                }
            });
        }

        private static void closeOtherInstances() {
            using NamedPipeClientStream pipeClient = new(".", PIPE_NAME, PipeDirection.Out);
            try {
                pipeClient.Connect(200); //this does slow down the UI from appearing when an existing process is not already running

                using StreamWriter writer = new(pipeClient, Encoding.UTF8);
                writer.Write(COMMAND_EXIT);
            } catch (TimeoutException) {
                Console.WriteLine("Notifier is not running, skipping sending notification to it.");
            }
        }

        private static RetrospectNotification formatNotification(RetrospectEvent retrospectEvent) =>
            retrospectEvent switch {
                StartSourceEvent => new RetrospectNotification(null, "Starting backup", false),

                EndSourceEvent { fatalErrorCode: 0 } notification => new RetrospectNotification("Backup complete", string.Format(new DataSizeFormatter(),
                    "Copied {0:N0} files ({1:A1}) in {2:h\\ \\h\\ mm\\ \\m}.", notification.filesBackedUp, notification.sizeBackedUp, notification.duration), true),

                EndSourceEvent notification => new RetrospectNotification("Backup failed",
                    $"{notification.fatalErrorMessage}\nCode {notification.fatalErrorCode:N0} ({notification.errorCount:N0} errors)", true),
                _ => new RetrospectNotification("Unknown event", retrospectEvent.GetType().Name, true)
            };

    }

}