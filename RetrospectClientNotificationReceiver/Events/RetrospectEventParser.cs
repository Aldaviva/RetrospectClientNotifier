using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace RetrospectClientNotificationReceiver.Events {

    internal static class RetrospectEventParser {

        public static RetrospectEvent parseEvent(IReadOnlyList<string> notificationArgs) {
            string   eventName = notificationArgs[0];
            string[] argsTail  = notificationArgs.Skip(1).ToArray();

            // ReSharper disable once NotResolvedInText
            RetrospectEvent retrospectEvent = eventName switch {
                StartSourceEvent.EVENT_NAME => new StartSourceEvent(argsTail),
                EndSourceEvent.EVENT_NAME   => new EndSourceEvent(argsTail),
                _ => throw new ArgumentOutOfRangeException("eventName", eventName,
                    "Unknown Retrospect event name, known names are " + string.Join(", ", StartSourceEvent.EVENT_NAME, EndSourceEvent.EVENT_NAME))
            };

            return retrospectEvent;
        }

    }

}