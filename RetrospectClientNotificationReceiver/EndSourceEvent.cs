using System;
using System.Collections.Generic;
using System.Globalization;
using DataSizeUnits;

namespace RetrospectClientNotificationReceiver {

    public struct EndSourceEvent {

        public const string EVENT_NAME = "EndSource";
        private const string DATE_FORMAT = "M/d/yyyy H:mm";

        public readonly string scriptName;
        public readonly string sourceName;
        public readonly string sourcePath;
        public readonly string clientName;
        public readonly DataSize sizeBackedUp;
        public readonly long filesBackedUp;
        public readonly TimeSpan duration;
        public readonly DateTime sourceStart;
        public readonly DateTime sourceEnd;
        public readonly DateTime scriptStart;
        public readonly string backupSet;
        public readonly string backupAction;
        public readonly string parentVolume;
        public readonly int errorCount;
        public readonly int fatalErrorCode;
        public readonly string fatalErrorMessage;
        public readonly bool unused;

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

        public EndSourceEvent(string scriptName, DataSize sizeBackedUp, long filesBackedUp, TimeSpan duration, DateTime sourceEnd, string fatalErrorMessage): this() {
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