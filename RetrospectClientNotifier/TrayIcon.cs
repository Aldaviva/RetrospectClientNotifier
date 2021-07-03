using System;
using System.Windows.Forms;

#nullable enable

namespace RetrospectClientNotifier {

    public partial class TrayIcon: Form {

        public TrayIcon(string tooltip) {
            InitializeComponent();
            notifyIcon.Text = tooltip;
        }

        private void onClickHideIcon(object? sender = null, EventArgs? e = null) {
            Close();
        }

        private void onClickAlwaysHideIcon(object sender, EventArgs e) {
            Notifier.allowTrayIcon = false;
            onClickHideIcon();
        }

    }

}