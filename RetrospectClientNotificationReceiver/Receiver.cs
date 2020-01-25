#nullable enable

using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace RetrospectClientNotificationReceiver {

    internal static class Receiver {

        private static Dispatcher? DISPATCHER;

        [STAThread]
        private static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DISPATCHER = Dispatcher.CurrentDispatcher;

            Task.Run(() => {
                while (true) {
                    using var pipeServer = new NamedPipeServerStream("RetrospectClientNotifier", PipeDirection.In);
                    pipeServer.WaitForConnection();

                    using var pipeReader = new StreamReader(pipeServer);
                    string rawNotification = pipeReader.ReadToEnd();

                    string[] notificationArgs = rawNotification.Split('\t');
                    switch (notificationArgs[0]) {
                        case EndSourceEvent.EVENT_NAME:
                            Task.Run(() => OnNotificationReceived(new EndSourceEvent(notificationArgs.Skip(1).ToArray())));
                            break;
                        default:
                            break;
                    }

                }

                // ReSharper disable once FunctionNeverReturns - it's supposed to run forever
            });

            Application.Run();
        }

        private static void OnNotificationReceived(EndSourceEvent notification) {
            DISPATCHER?.Invoke(() => {
                var dialog = new NotificationDialogBox(notification);
                dialog.Show();
                dialog.Activate();
                dialog.Focus();
            });
        }

    }

}