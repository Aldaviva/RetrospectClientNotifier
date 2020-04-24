#nullable enable

using System.Drawing;
using System.Windows.Forms;
using RetrospectClientNotificationReceiver.Properties;

namespace RetrospectClientNotificationReceiver {

    public partial class NotificationDialogBox: Form {

        public NotificationDialogBox(string bodyText) {
            InitializeComponent();
            body.Text = bodyText;
            icon.Image = Bitmap.FromHicon(new Icon(Resources.retroico, 32, 32).Handle);
        }

        private void dismissButton_Click(object sender, System.EventArgs e) {
            Close();
            Dispose();
        }

    }

}