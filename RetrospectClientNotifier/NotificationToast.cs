using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.Toolkit.Uwp.Notifications;

#nullable enable

namespace RetrospectClientNotifier {

    public static class NotificationToast {

        private static readonly string LOGO_PATH      = Path.Combine(Path.GetTempPath(), "RetrospectClientNotifier", "retrospect.png");
        private static readonly string LOGO_DIRECTORY = Path.GetDirectoryName(LOGO_PATH)!;

        public static void showNotification(RetrospectNotification notification) {
            if (notification.title is null && notification.body is null) return;

            Uri logoUri = saveLogo();

            ToastContentBuilder builder = new ToastContentBuilder()
                .SetToastDuration(notification.longDuration ? ToastDuration.Long : ToastDuration.Short)
                .AddAppLogoOverride(logoUri, ToastGenericAppLogoCrop.None, "Retrospect");

            if (notification.title is not null) {
                builder.AddText(notification.title, AdaptiveTextStyle.Header, false, 1, 1, null, "en-US");
            }

            if (notification.body is not null) {
                builder.AddText(notification.body, AdaptiveTextStyle.Caption, true, 1, 1, null, "en-US");
            }

            builder.Show();

            Thread.Sleep(400); //Give Windows time to read the image before we delete it
            try {
                File.Delete(LOGO_PATH);
                Directory.Delete(LOGO_DIRECTORY);
            } catch (IOException) {
                //directory may not have been empty, ignore
            }
        }

        private static Uri saveLogo() {
            Uri logoUri = new(new Uri("file://"), LOGO_PATH);

            Directory.CreateDirectory(LOGO_DIRECTORY);
            using Stream     logoReadStream  = Assembly.GetExecutingAssembly().GetManifestResourceStream("RetrospectClientNotifier.Resources.retrospect48.png")!;
            using FileStream logoWriteStream = new(LOGO_PATH, FileMode.Create);
            logoReadStream.CopyTo(logoWriteStream);

            return logoUri;
        }

        public static void hideNotifications() {
            ToastNotificationManagerCompat.History.Clear();
        }

    }

}