using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataSizeUnits;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using murrayju.ProcessExtensions;
using RetrospectClientNotifier.Events;

#nullable enable

namespace RetrospectClientNotifier {

    internal static class Notifier {

        private const string PIPE_NAME    = "RetrospectClientNotifier";
        private const string COMMAND_EXIT = "exit";

        private static readonly (string key, string value) ALLOW_TRAY_ICON_REGISTRY = (Registry.CurrentUser.Name + @"\SOFTWARE\Ben Hutchison\RetrospectClientNotifier", "AllowTrayIcon");

        public static bool allowTrayIcon {
            get => Convert.ToBoolean(Registry.GetValue(ALLOW_TRAY_ICON_REGISTRY.key, ALLOW_TRAY_ICON_REGISTRY.value, null) ?? true);
            set => Registry.SetValue(ALLOW_TRAY_ICON_REGISTRY.key, ALLOW_TRAY_ICON_REGISTRY.value, value, RegistryValueKind.DWord);
        }

        [STAThread]
        private static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (ToastNotificationManagerCompat.WasCurrentProcessToastActivated()) {
                //user clicked a toast, do nothing and exit
            } else if (!Environment.UserInteractive) {
                //if running as LocalSystem, we can't show UI, so relaunch in the console user's session
                ProcessExtensions.StartProcessAsCurrentUser(Application.ExecutablePath, Environment.CommandLine);
            } else if (Environment.GetCommandLineArgs().Length == 1) {
                MessageBox.Show("Copy this executable into the Retrospect installation directory (C:\\Program Files (x86)\\Retrospect\\Retrospect Client by default) and rename it to " +
                    "retroeventhandler.exe so that Retrospect can call it when a backup starts or finishes.", "Cannot be run manually", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } else {
                closeOtherInstances();

                IReadOnlyList<string> args = null!;
                try {
                    args = getCommandLineArguments();
                } catch (Win32Exception) {
                    MessageBox.Show("Unable to parse arguments string " + Environment.CommandLine, "Retrospect Client Notifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                RetrospectEvent retrospectEvent;
                try {
                    retrospectEvent = RetrospectEventParser.parseEvent(args);
                } catch (ArgumentOutOfRangeException e) {
                    MessageBox.Show(e.Message + "\n\n" + Environment.CommandLine, "RetrospectClientNotifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                RetrospectNotification notification = formatNotification(retrospectEvent);

                TrayIcon? trayIcon    = null;
                bool      keepRunning = false;

                if (retrospectEvent is StartSourceEvent) {
                    keepRunning = true;
                    if (allowTrayIcon) {
                        trayIcon = new TrayIcon($"Retrospect backup started at {DateTime.Now:t}");
                    }
                }

                NotificationToast.hideNotifications();
                NotificationToast.showNotification(notification);

                if (keepRunning) {
                    listenForExitMessages(trayIcon);
                    Application.Run();
                }
            }
        }

        internal static IReadOnlyList<string> getCommandLineArguments(string? commandLine = null) {
            commandLine ??= Environment.CommandLine;

            //pcpds.exe doesn't properly escape paths ending with \ in argument string, so turn "G:\" into "G:\\" and then manually parse
            string commandLineEscaped = commandLine.Replace(@"\"" ", @"\\"" ");
            return CommandLine.commandLineToArgs(commandLineEscaped).Skip(1).ToArray();
        }

        private static void listenForExitMessages(Form? trayIcon) {
            NamedPipeServerStream pipeServer = new(PIPE_NAME, PipeDirection.In, 1, PipeTransmissionMode.Message);
            pipeServer.WaitForConnectionAsync().ContinueWith(async _ => {
                using StreamReader reader = new(pipeServer, Encoding.UTF8);

                string message = await reader.ReadToEndAsync();
                if (message == COMMAND_EXIT) {
                    trayIcon?.Close(); //needed to quickly hide the tray icon instead of waiting for Explorer to eventually realize our process exited
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

        private static RetrospectNotification formatNotification(RetrospectEvent retrospectEvent) {
            //EndSourceEvent.duration from Retrospect is way too short, it probably only covers copying files and not scanning, building snapshot, or verification
            DateTime now = DateTime.Now;
            TimeSpan getCorrectDuration(EndSourceEvent endEvent) => now - endEvent.scriptStart;

            return retrospectEvent switch {
                StartSourceEvent => new RetrospectNotification("Starting backup", "Always back up, never back down.", false),

                EndSourceEvent { fatalErrorCode: 0 } notification => new RetrospectNotification("Backup complete", string.Format(new DataSizeFormatter(),
                    "Copied {0:N0} {2} ({1:A1}) in {3}.", notification.filesBackedUp, notification.sizeBackedUp, notification.filesBackedUp != 1 ? "files" : "file",
                    getCorrectDuration(notification).ToString(getCorrectDuration(notification) >= TimeSpan.FromHours(1) ? @"h\h\ mm\m" : @"m\m")), false),

                EndSourceEvent notification => new RetrospectNotification("Backup failed", $"{notification.fatalErrorMessage}\n" +
                    $"Code {notification.fatalErrorCode:N0} ({notification.errorCount:N0} errors)", true),

                _ => new RetrospectNotification("Unknown event", retrospectEvent.GetType().Name, true)
            };
        }

    }

}