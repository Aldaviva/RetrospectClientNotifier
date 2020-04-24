#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using DataSizeUnits;

namespace RetrospectClientNotificationReceiver.Events {

    public struct EndSourceEvent: RetrospectEvent {

        public const string EVENT_NAME = "EndSource";
        private const string DATE_FORMAT = "M/d/yyyy H:mm";

        public string scriptName { get; }
        public string sourceName { get; }
        public string sourcePath { get; }
        public string clientName { get; }
        public DataSize sizeBackedUp { get; }
        public long filesBackedUp { get; }
        public TimeSpan duration { get; }
        public DateTime sourceStart { get; }
        public DateTime sourceEnd { get; }
        public DateTime scriptStart { get; }
        public string backupSet { get; }
        public string backupAction { get; }
        public string parentVolume { get; }
        public int errorCount { get; }
        public int fatalErrorCode { get; }
        public string fatalErrorMessage { get; }
        public bool unused { get; }

        public EndSourceEvent(IReadOnlyList<string> args) {
            scriptName = args[0];
            sourceName = args[1];
            sourcePath = args[2];
            clientName = args[3];
            sizeBackedUp = new DataSize(long.Parse(args[4]), Unit.Kilobyte);
            filesBackedUp = long.Parse(args[5]);
            duration = TimeSpan.FromSeconds(long.Parse(args[6]));
            sourceStart = parseDateTime(args[7]);
            sourceEnd = parseDateTime(args[8]);
            scriptStart = parseDateTime(args[9]);
            backupSet = args[10];
            backupAction = args[11];
            parentVolume = args[12];
            errorCount = int.Parse(args[13]);
            fatalErrorCode = int.Parse(args[14]);
            fatalErrorMessage = args[15];
            unused = bool.Parse(args[16]);
        }

        public EndSourceEvent(string scriptName, DataSize sizeBackedUp, long filesBackedUp, TimeSpan duration, DateTime sourceEnd,
            string fatalErrorMessage): this() {
            this.scriptName = scriptName;
            this.sizeBackedUp = sizeBackedUp;
            this.filesBackedUp = filesBackedUp;
            this.duration = duration;
            this.sourceEnd = sourceEnd;
            this.fatalErrorMessage = fatalErrorMessage;
        }

        private static DateTime parseDateTime(string dateTimeString) {
            return DateTime.ParseExact(dateTimeString, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

    }

}