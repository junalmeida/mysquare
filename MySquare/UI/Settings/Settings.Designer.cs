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
            this.label3 = new System.Windows.Forms.Label();
            this.pnlPremium = new System.Windows.Forms.Panel();
            this.chkShowAds = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkNotifications = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboDoubleTap = new System.Windows.Forms.CheckBox();
            this.cboAutoUpdate = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkUseGps = new System.Windows.Forms.CheckBox();
            this.cboMapType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblLogin = new System.Windows.Forms.Label();
            this.lnkOAuth = new System.Windows.Forms.LinkLabel();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            this.pnlPremium.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            label7.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            label7.ForeColor = System.Drawing.Color.Gainsboro;
            label7.Location = new System.Drawing.Point(21, 48);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(215, 26);
            label7.Text = "Uncheck to disable the bottom bar of ads.";
            // 
            // label8
            // 
            label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            label8.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            label8.ForeColor = System.Drawing.Color.Gainsboro;
            label8.Location = new System.Drawing.Point(21, 100);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(215, 59);
            label8.Text = "This will show pings from your friends, for every 15 min.\r\nGo to your account on " +
                "foursquare.com to disable ping for specific friends.";
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
            label9.Text = "Gps will be disabled after 5 minutes of inactivity.";
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
            this.pnlPremium.Controls.Add(this.chkShowAds);
            this.pnlPremium.Controls.Add(this.label4);
            this.pnlPremium.Controls.Add(this.chkNotifications);
            this.pnlPremium.Controls.Add(label7);
            this.pnlPremium.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPremium.Location = new System.Drawing.Point(0, 73);
            this.pnlPremium.Name = "pnlPremium";
            this.pnlPremium.Size = new System.Drawing.Size(239, 165);
            this.pnlPremium.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.pnlPremium.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            this.pnlPremium.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
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
            this.chkNotifications.ForeColor = System.Drawing.Color.White;
            this.chkNotifications.Location = new System.Drawing.Point(15, 77);
            this.chkNotifications.Name = "chkNotifications";
            this.chkNotifications.Size = new System.Drawing.Size(108, 20);
            this.chkNotifications.TabIndex = 13;
            this.chkNotifications.Text = "Enable Pings";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.cboDoubleTap);
            this.panel1.Controls.Add(this.cboAutoUpdate);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(label9);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.chkUseGps);
            this.panel1.Controls.Add(this.cboMapType);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 238);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(239, 205);
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            // 
            // cboDoubleTap
            // 
            this.cboDoubleTap.ForeColor = System.Drawing.Color.White;
            this.cboDoubleTap.Location = new System.Drawing.Point(15, 177);
            this.cboDoubleTap.Name = "cboDoubleTap";
            this.cboDoubleTap.Size = new System.Drawing.Size(209, 20);
            this.cboDoubleTap.TabIndex = 17;
            this.cboDoubleTap.Text = "Double tap on lists";
            // 
            // cboAutoUpdate
            // 
            this.cboAutoUpdate.ForeColor = System.Drawing.Color.White;
            this.cboAutoUpdate.Location = new System.Drawing.Point(15, 151);
            this.cboAutoUpdate.Name = "cboAutoUpdate";
            this.cboAutoUpdate.Size = new System.Drawing.Size(212, 20);
            this.cboAutoUpdate.TabIndex = 16;
            this.cboAutoUpdate.Text = "Check for &updates at startup";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(3, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(137, 15);
            this.label10.Text = "Other options:";
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
            this.panel2.Controls.Add(this.lblLogin);
            this.panel2.Controls.Add(this.lnkOAuth);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(239, 73);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form_MouseMove);
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            // 
            // lblLogin
            // 
            this.lblLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLogin.ForeColor = System.Drawing.Color.White;
            this.lblLogin.Location = new System.Drawing.Point(58, 6);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(174, 20);
            this.lblLogin.Text = "login@email.com";
            // 
            // lnkOAuth
            // 
            this.lnkOAuth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkOAuth.ForeColor = System.Drawing.Color.Cyan;
            this.lnkOAuth.Location = new System.Drawing.Point(13, 34);
            this.lnkOAuth.Name = "lnkOAuth";
            this.lnkOAuth.Size = new System.Drawing.Size(211, 20);
            this.lnkOAuth.TabIndex = 1;
            this.lnkOAuth.Text = "Authenticate";
            this.lnkOAuth.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lnkOAuth.Click += new System.EventHandler(this.lnkOAuth_Click);
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
            this.Size = new System.Drawing.Size(239, 449);
            this.pnlPremium.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

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
        internal System.Windows.Forms.CheckBox cboDoubleTap;
        internal System.Windows.Forms.CheckBox cboAutoUpdate;
        private System.Windows.Forms.Label label10;
        internal System.Windows.Forms.LinkLabel lnkOAuth;
        internal System.Windows.Forms.Label lblLogin;
    }
}
