#nullable enable

using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using DataSizeUnits;
using RetrospectClientNotificationReceiver.Events;

namespace RetrospectClientNotificationReceiver {

    internal static class Receiver {

        private static Dispatcher? DISPATCHER;

        [STAThread]
        private static void Main() {
            if (!isOnlyInstanceOfProgramIsRunning()) {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DISPATCHER = Dispatcher.CurrentDispatcher;

            Task.Run(() => {
                while (true) {
                    try {
                        using var pipeServer = new NamedPipeServerStream("RetrospectClientNotifier", PipeDirection.In);
                        pipeServer.WaitForConnection();

                        using var pipeReader = new StreamReader(pipeServer);
                        string rawNotification = pipeReader.ReadToEnd();

                        string[] notificationArgs = rawNotification.Split('\t');
                        string[] argsTail = notificationArgs.Skip(1).ToArray();
                        RetrospectEvent retrospectEvent;

                        switch (notificationArgs[0]) {
                            case StartSourceEvent.EVENT_NAME:
                                retrospectEvent = new StartSourceEvent(argsTail);
                                break;
                            case EndSourceEvent.EVENT_NAME:
                                retrospectEvent = new EndSourceEvent(argsTail);
                                break;
                            default:
                                continue;
                        }

                        Task.Run(() => OnNotificationReceived(retrospectEvent));
                    } catch (Exception e) when (!(e is OutOfMemoryException)) {
                        Console.WriteLine(
                            $"Got exception {e.GetType().Name} while listening for incoming notifications, listening again after 10 seconds...");
                        Console.WriteLine(e.Message);
                        Thread.Sleep(TimeSpan.FromSeconds(10));
                    }
                }

                // ReSharper disable once FunctionNeverReturns - it's supposed to run forever
            });

            Application.Run();
        }

        private static bool isOnlyInstanceOfProgramIsRunning() {
            string lockName = Assembly.GetExecutingAssembly().GetCustomAttribute<GuidAttribute>().Value;
            // ReSharper disable once ObjectCreationAsStatement
            new Mutex(true, lockName, out bool createdNew);
            return createdNew;
        }

        private static void OnNotificationReceived(RetrospectEvent notification) {
            DISPATCHER?.Invoke(() => {
                string body = formatBody(notification);
                var dialog = new NotificationDialogBox(body);
                dialog.Show();
                dialog.Activate();
                dialog.Focus();
            });
        }

        private static string formatBody(RetrospectEvent retrospectEvent) {
            switch (retrospectEvent) {
                case EndSourceEvent notification:
                    return string.Format(new DataSizeFormatter(),
                        "Backup of {0} finished on {1:D} at {1:t}.\r\n\r\n" +
                        "Status: {2}\r\n\r\n" +
                        "Backed up {3:N0} files ({4:A1}) in {5:h\\ \\h\\ mm\\ \\m}.",
                        notification.scriptName, notification.sourceEnd, notification.fatalErrorMessage, notification.filesBackedUp,
                        notification.sizeBackedUp, notification.duration);
                case StartSourceEvent notification:
                    DateTime now = DateTime.Now;
                    return string.Format("Backup of {0} started on {1:D} at {1:t}.",
                        notification.scriptName, now);
                default:
                    return "Unknown Retrospect event";
            }
        }

    }

}