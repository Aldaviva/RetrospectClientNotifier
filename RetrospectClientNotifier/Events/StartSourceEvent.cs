#nullable enable

using System.Collections.Generic;

namespace RetrospectClientNotifier.Events {

    public readonly struct StartSourceEvent: RetrospectEvent {

        public const string EVENT_NAME = "StartSource";

        public string scriptName { get; }
        public string sourceName { get; }
        public string sourcePath { get; }
        public string clientName { get; }
        public string interventionFile { get; }

        public StartSourceEvent(IReadOnlyList<string> args) {
            scriptName       = args[0];
            sourceName       = args[1];
            sourcePath       = args[2];
            clientName       = args[3];
            interventionFile = args[4];
        }

        public StartSourceEvent(string scriptName, string sourceName, string sourcePath, string clientName, string interventionFile) {
            this.scriptName       = scriptName;
            this.sourceName       = sourceName;
            this.sourcePath       = sourcePath;
            this.clientName       = clientName;
            this.interventionFile = interventionFile;
        }

    }

}