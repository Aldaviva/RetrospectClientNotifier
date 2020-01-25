using System;
using System.Drawing;
using System.Windows.Forms;
using DataSizeUnits;
using RetrospectClientNotificationReceiver.Properties;

namespace RetrospectClientNotificationReceiver {

    public partial class NotificationDialogBox: Form {

        public NotificationDialogBox(EndSourceEvent notification) {
            InitializeComponent();
            body.Text = formatBody(notification);
            icon.Image = Bitmap.FromHicon(new Icon(Resources.retroico, 32, 32).Handle);
        }

        private static string formatBody(EndSourceEvent notification) {
            return string.Format(new DataSizeFormatter(),
                "Backup of {0} finished on {1:D} at {1:t}.\r\n\r\n" +
                "Status: {2}\r\n\r\n" +
                "Backed up {3:N0} files ({4:A1}) in {5:h\\ \\h\\ mm\\ \\m}.",
                notification.scriptName, notification.sourceEnd, notification.fatalErrorMessage, notification.filesBackedUp,
                notification.sizeBackedUp, notification.duration);
        }

        private void dismissButton_Click(object sender, System.EventArgs e) {
            Close();
        }

    }

}