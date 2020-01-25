#nullable enable

using System;
using System.IO;
using System.IO.Pipes;

namespace RetrospectClientNotificationSender {

    internal static class Sender {

        private static void Main(string[] args) {
            using var pipeClient = new NamedPipeClientStream(".", "RetrospectClientNotifier", PipeDirection.Out);

            try {
                pipeClient.Connect(500);
                using var pipeWriter = new StreamWriter(pipeClient);
                pipeWriter.WriteLine(string.Join("\t", args));

            } catch (TimeoutException) {
                Console.WriteLine("Notification Receiver is not running, skipping sending notification to it.");
            }
        }

    }

}