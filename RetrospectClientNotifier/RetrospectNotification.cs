#nullable enable

namespace RetrospectClientNotifier {

    public readonly struct RetrospectNotification {

        public string? title { get; }
        public string? body { get; }
        public bool longDuration { get; }

        public RetrospectNotification(string? title, string? body, bool longDuration) {
            this.title        = title;
            this.body         = body;
            this.longDuration = longDuration;
        }

    }

}