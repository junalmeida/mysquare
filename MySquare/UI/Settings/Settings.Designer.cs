namespace MySquare.UI.Settings
{
    partial class Settings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label8;
            System.Windows.Forms.Label label9;
            this.label1 = new System.Windows.Forms.Label();
            this.txtEmail = new Tenor.Mobile.UI.TextControl();
            this.txtPassword = new Tenor.Mobile.UI.TextControl();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlPremium = new System.Windows.Forms.Panel();
            this.chkShowAds = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkNotifications = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.chkUseGps = new System.Windows.Forms.CheckBox();
            this.cboMapType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            this.pnlPremium.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(10, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.Text = "E-mail:";
            // 
            // txtEmail
            // 
            this.txtEmail.AcceptsReturn = false;
            this.txtEmail.AcceptsTab = false;
            this.txtEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtEmail.HideSelection = true;
            this.txtEmail.InputMode = Microsoft.WindowsCE.Forms.InputMode.Default;
            this.txtEmail.Location = new System.Drawing.Point(10, 40);
            this.txtEmail.MaxLength = 32767;
            this.txtEmail.Modified = false;
            this.txtEmail.Multiline = false;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.PasswordChar = '\0';
            this.txtEmail.ReadOnly = false;
            this.txtEmail.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtEmail.SelectedText = "";
            this.txtEmail.SelectionLength = 0;
            this.txtEmail.SelectionStart = 0;
            this.txtEmail.Size = new System.Drawing.Size(214, 21);
            this.txtEmail.TabIndex = 1;
            this.txtEmail.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtEmail.WordWrap = true;
            // 
            // txtPassword
            // 
            this.txtPassword.AcceptsReturn = false;
            this.txtPassword.AcceptsTab = false;
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPassword.HideSelection = true;
            this.txtPassword.InputMode = Microsoft.WindowsCE.Forms.InputMode.Default;
            this.txtPassword.Location = new System.Drawing.Point(10, 81);
            this.txtPassword.MaxLength = 32767;
            this.txtPassword.Modified = false;
            this.txtPassword.Multiline = false;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.ReadOnly = false;
            this.txtPassword.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPassword.SelectedText = "";
            this.txtPassword.SelectionLength = 0;
            this.txtPassword.SelectionStart = 0;
            this.txtPassword.Size = new System.Drawing.Size(214, 21);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtPassword.WordWrap = true;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(10, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 15);
            this.label2.Text = "Password:";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(2, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 15);
            this.label3.Text = "Account:";
            // 
            // pnlPremium
            // 
            this.pnlPremium.BackColor = System.Drawing.Color.Black;
            this.pnlPremium.Controls.Add(label8);
            this.pnlPremium.Controls.Add(label7);
            this.pnlPremium.Controls.Add(this.chkShowAds);
            this.pnlPremium.Controls.Add(this.label4);
            this.pnlPremium.Controls.Add(this.chkNotifications);
            this.pnlPremium.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPremium.Location = new System.Drawing.Point(0, 115);
            this.pnlPremium.Name = "pnlPremium";
            this.pnlPremium.Size = new System.Drawing.Size(239, 156);
            this.pnlPremium.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // chkShowAds
            // 
            this.chkShowAds.ForeColor = System.Drawing.Color.White;
            this.chkShowAds.Location = new System.Drawing.Point(15, 28);
            this.chkShowAds.Name = "chkShowAds";
            this.chkShowAds.Size = new System.Drawing.Size(100, 20);
            this.chkShowAds.TabIndex = 11;
            this.chkShowAds.Text = "Show Ads";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(2, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 15);
            this.label4.Text = "Premium Options:";
            // 
            // chkNotifications
            // 
            this.chkNotifications.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkNotifications.ForeColor = System.Drawing.Color.White;
            this.chkNotifications.Location = new System.Drawing.Point(2, 74);
            this.chkNotifications.Name = "chkNotifications";
            this.chkNotifications.Size = new System.Drawing.Size(100, 20);
            this.chkNotifications.TabIndex = 13;
            this.chkNotifications.Text = "Enable Pings";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(label9);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.chkUseGps);
            this.panel1.Controls.Add(this.cboMapType);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 271);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(239, 125);
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(12, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 15);
            this.label6.Text = "Map Type:";
            // 
            // chkUseGps
            // 
            this.chkUseGps.ForeColor = System.Drawing.Color.White;
            this.chkUseGps.Location = new System.Drawing.Point(14, 69);
            this.chkUseGps.Name = "chkUseGps";
            this.chkUseGps.Size = new System.Drawing.Size(141, 20);
            this.chkUseGps.TabIndex = 11;
            this.chkUseGps.Text = "Use Gps if available";
            // 
            // cboMapType
            // 
            this.cboMapType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboMapType.BackColor = System.Drawing.Color.White;
            this.cboMapType.ForeColor = System.Drawing.Color.Black;
            this.cboMapType.Items.Add("Roadmap");
            this.cboMapType.Items.Add("Satellite");
            this.cboMapType.Items.Add("Hybrid");
            this.cboMapType.Items.Add("Terrain");
            this.cboMapType.Location = new System.Drawing.Point(13, 43);
            this.cboMapType.Name = "cboMapType";
            this.cboMapType.Size = new System.Drawing.Size(214, 22);
            this.cboMapType.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(2, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 15);
            this.label5.Text = "Localization && Maps:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtEmail);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtPassword);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(239, 115);
            // 
            // label7
            // 
            label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            label7.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            label7.ForeColor = System.Drawing.Color.Gainsboro;
            label7.Location = new System.Drawing.Point(21, 48);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(215, 20);
            label7.Text = "Uncheck to disable the bottom bar of ads.";
            // 
            // label8
            // 
            label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            label8.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            label8.ForeColor = System.Drawing.Color.Gainsboro;
            label8.Location = new System.Drawing.Point(21, 94);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(215, 59);
            label8.Text = "This will show pings from your friends, every 15 min.\r\nGo to your account on four" +
                "square.com to disable ping for specific friends.";
            // 
            // label9
            // 
            label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            label9.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            label9.ForeColor = System.Drawing.Color.Gainsboro;
            label9.Location = new System.Drawing.Point(21, 91);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(215, 34);
            label9.Text = "Gps will be disabled after 5 min. of inactivity.";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlPremium);
            this.Controls.Add(this.panel2);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "Settings";
            this.Size = new System.Drawing.Size(239, 382);
            this.pnlPremium.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        internal Tenor.Mobile.UI.TextControl txtEmail;
        internal Tenor.Mobile.UI.TextControl txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        internal System.Windows.Forms.CheckBox chkShowAds;
        internal System.Windows.Forms.Panel pnlPremium;
        internal System.Windows.Forms.CheckBox chkUseGps;
        internal System.Windows.Forms.CheckBox chkNotifications;
        internal System.Windows.Forms.ComboBox cboMapType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel2;
    }
}
