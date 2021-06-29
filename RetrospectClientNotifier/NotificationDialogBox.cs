#nullable enable

using System;
using System.Drawing;
using System.Windows.Forms;
using RetrospectClientNotifier.Properties;

namespace RetrospectClientNotifier {

    public partial class NotificationDialogBox: Form {

        public NotificationDialogBox(string bodyText) {
            InitializeComponent();
            body.Text  = bodyText;
            icon.Image = Bitmap.FromHicon(new Icon(Resources.retroico, 32, 32).Handle);
        }

        private void dismissButton_Click(object sender, EventArgs e) {
            Close();
            Dispose();
        }

    }

}