namespace RetrospectClientNotificationReceiver
{
    partial class NotificationDialogBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationDialogBox));
            this.body = new System.Windows.Forms.Label();
            this.dismissButton = new System.Windows.Forms.Button();
            this.icon = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // body
            // 
            this.body.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.body.AutoSize = true;
            this.body.Location = new System.Drawing.Point(3, 0);
            this.body.Margin = new System.Windows.Forms.Padding(3, 0, 3, 17);
            this.body.MaximumSize = new System.Drawing.Size(300, 0);
            this.body.MinimumSize = new System.Drawing.Size(300, 0);
            this.body.Name = "body";
            this.body.Size = new System.Drawing.Size(300, 13);
            this.body.TabIndex = 0;
            this.body.Text = "label1";
            // 
            // dismissButton
            // 
            this.dismissButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dismissButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.dismissButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dismissButton.Location = new System.Drawing.Point(228, 33);
            this.dismissButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.dismissButton.Name = "dismissButton";
            this.dismissButton.Size = new System.Drawing.Size(75, 23);
            this.dismissButton.TabIndex = 1;
            this.dismissButton.Text = "OK";
            this.dismissButton.UseVisualStyleBackColor = true;
            this.dismissButton.Click += new System.EventHandler(this.dismissButton_Click);
            // 
            // icon
            // 
            this.icon.BackColor = System.Drawing.Color.Transparent;
            this.icon.Location = new System.Drawing.Point(17, 18);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(32, 32);
            this.icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.icon.TabIndex = 2;
            this.icon.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.body);
            this.flowLayoutPanel1.Controls.Add(this.dismissButton);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(65, 18);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(306, 66);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // NotificationDialogBox
            // 
            this.AcceptButton = this.dismissButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.dismissButton;
            this.ClientSize = new System.Drawing.Size(383, 97);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.icon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NotificationDialogBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Retrospect";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label body;
        private System.Windows.Forms.Button dismissButton;
        private System.Windows.Forms.PictureBox icon;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}