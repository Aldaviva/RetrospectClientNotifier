#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using DataSizeUnits;

namespace RetrospectClientNotifier.Events {

    public readonly struct EndSourceEvent: RetrospectEvent {

        public const  string EVENT_NAME  = "EndSource";
        private const string DATE_FORMAT = "M/d/yyyy H:mm";

        public string scriptName { get; }     // "My Backup Script"
        public string sourceName { get; }     // "Backup Clients/Aegir/Games (G:)"
        public string sourcePath { get; }     // "G:\"
        public string clientName { get; }     // "Aegir"
        public DataSize sizeBackedUp { get; } //number in KB
        public long filesBackedUp { get; }
        public TimeSpan duration { get; }    // "0"
        public DateTime sourceStart { get; } // "6/29/2021 1:34"
        public DateTime sourceEnd { get; }
        public DateTime scriptStart { get; }
        public string backupSet { get; }    // "My Backup Set"
        public string backupAction { get; } // "Normal"
        public string parentVolume { get; } // "Aegir"
        public int errorCount { get; }      //0

        /// <summary>
        /// Negative number for the error code, or <c>0</c> if there were no fatal errors
        /// </summary>
        public int fatalErrorCode { get; }

        /// <summary>
        /// The error message, or <c>successful</c> if there were no fatal errors
        /// </summary>
        public string fatalErrorMessage { get; }

        public bool unused { get; } //"true"

        public EndSourceEvent(IReadOnlyList<string> args) {
            scriptName        = args[0];
            sourceName        = args[1];
            sourcePath        = args[2];
            clientName        = args[3];
            sizeBackedUp      = new DataSize(long.Parse(args[4]), Unit.Kilobyte);
            filesBackedUp     = long.Parse(args[5]);
            duration          = TimeSpan.FromSeconds(long.Parse(args[6]));
            sourceStart       = parseDateTime(args[7]);
            sourceEnd         = parseDateTime(args[8]);
            scriptStart       = parseDateTime(args[9]);
            backupSet         = args[10];
            backupAction      = args[11];
            parentVolume      = args[12];
            errorCount        = int.Parse(args[13]);
            fatalErrorCode    = int.Parse(args[14]);
            fatalErrorMessage = args[15];
            unused            = bool.Parse(args[16]);
        }

        private static DateTime parseDateTime(string dateTimeString) {
            return DateTime.ParseExact(dateTimeString, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

    }

}