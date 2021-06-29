using System;
using System.Windows.Forms;

#nullable enable

namespace RetrospectClientNotifier {

    public partial class TrayIcon: Form {

        public TrayIcon(string tooltip) {
            InitializeComponent();
            notifyIcon.Text = tooltip;
        }

        private void onClickClose(object sender, EventArgs e) {
            Close();
            Application.Exit();
        }

    }

}