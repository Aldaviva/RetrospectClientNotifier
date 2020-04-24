namespace RetrospectClientNotificationReceiver.Events {

    public interface RetrospectEvent {

        string scriptName { get; }
        string sourceName { get; }
        string sourcePath { get; }
        string clientName { get; }

    }

}