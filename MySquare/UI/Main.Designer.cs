namespace MySquare.UI
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu;

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
            this.components = new System.ComponentModel.Container();
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.mnuLeft = new System.Windows.Forms.MenuItem();
            this.mnuRight = new System.Windows.Forms.MenuItem();
            this.header = new Tenor.Mobile.UI.HeaderStrip();
            this.lblError = new System.Windows.Forms.Label();
            this.settings1 = new MySquare.UI.Settings.Settings();
            this.places1 = new MySquare.UI.Places.Places();
            this.inputPanel = new Microsoft.WindowsCE.Forms.InputPanel(this.components);
            this.picAd = new System.Windows.Forms.Panel();
            this.lnkTextLink = new System.Windows.Forms.LinkLabel();
            this.friends1 = new MySquare.UI.Friends.Friends();
            this.userDetail1 = new MySquare.UI.Friends.UserDetail();
            this.createVenue1 = new MySquare.UI.Places.Create.CreateVenue();
            this.venueDetails1 = new MySquare.UI.Places.VenueDetails();
            this.help1 = new MySquare.UI.Help();
            this.contextHelp1 = new MySquare.UI.ContextHelp();
            this.timerTutorial = new System.Windows.Forms.Timer();
            this.timerAds = new System.Windows.Forms.Timer();
            this.picGps = new System.Windows.Forms.PictureBox();
            this.moreActions1 = new MySquare.UI.More.MoreActions();
            this.leaderboard1 = new MySquare.UI.More.Leaderboard();
            this.shout1 = new MySquare.UI.More.Shout();
            this.picAd.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.Add(this.mnuLeft);
            this.mainMenu.MenuItems.Add(this.mnuRight);
            // 
            // mnuLeft
            // 
            this.mnuLeft.Text = "&Left";
            // 
            // mnuRight
            // 
            this.mnuRight.Text = "&Right";
            // 
            // header
            // 
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(240, 49);
            this.header.TabIndex = 0;
            this.header.SelectedTabChanged += new System.EventHandler(this.header_SelectedTabChanged);
            // 
            // lblError
            // 
            this.lblError.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblError.BackColor = System.Drawing.Color.Black;
            this.lblError.ForeColor = System.Drawing.Color.White;
            this.lblError.Location = new System.Drawing.Point(12, 126);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(216, 29);
            this.lblError.Text = "Cannot list nearby venues.";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblError.Visible = false;
            // 
            // settings1
            // 
            this.settings1.Location = new System.Drawing.Point(51, 72);
            this.settings1.Name = "settings1";
            this.settings1.Size = new System.Drawing.Size(150, 150);
            this.settings1.TabIndex = 2;
            this.settings1.Visible = false;
            // 
            // places1
            // 
            this.places1.Location = new System.Drawing.Point(43, 64);
            this.places1.Name = "places1";
            this.places1.Size = new System.Drawing.Size(150, 150);
            this.places1.TabIndex = 1;
            this.places1.Visible = false;
            // 
            // inputPanel
            // 
            this.inputPanel.EnabledChanged += new System.EventHandler(this.inputPanel_EnabledChanged);
            // 
            // picAd
            // 
            this.picAd.BackColor = System.Drawing.Color.Black;
            this.picAd.Controls.Add(this.lnkTextLink);
            this.picAd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.picAd.Location = new System.Drawing.Point(0, 218);
            this.picAd.Name = "picAd";
            this.picAd.Size = new System.Drawing.Size(240, 50);
            this.picAd.Visible = false;
            this.picAd.Paint += new System.Windows.Forms.PaintEventHandler(this.picAd_Paint);
            // 
            // lnkTextLink
            // 
            this.lnkTextLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkTextLink.Location = new System.Drawing.Point(0, 18);
            this.lnkTextLink.Name = "lnkTextLink";
            this.lnkTextLink.Size = new System.Drawing.Size(240, 20);
            this.lnkTextLink.TabIndex = 0;
            this.lnkTextLink.Text = "linkLabel1";
            this.lnkTextLink.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lnkTextLink.Click += new System.EventHandler(this.lnkTextLink_Click);
            // 
            // friends1
            // 
            this.friends1.Location = new System.Drawing.Point(78, 55);
            this.friends1.Name = "friends1";
            this.friends1.Size = new System.Drawing.Size(150, 150);
            this.friends1.TabIndex = 4;
            this.friends1.Visible = false;
            // 
            // userDetail1
            // 
            this.userDetail1.Location = new System.Drawing.Point(12, 62);
            this.userDetail1.Name = "userDetail1";
            this.userDetail1.Size = new System.Drawing.Size(150, 150);
            this.userDetail1.TabIndex = 7;
            this.userDetail1.Visible = false;
            // 
            // createVenue1
            // 
            this.createVenue1.Location = new System.Drawing.Point(45, 59);
            this.createVenue1.Name = "createVenue1";
            this.createVenue1.Size = new System.Drawing.Size(150, 150);
            this.createVenue1.TabIndex = 10;
            this.createVenue1.Visible = false;
            // 
            // venueDetails1
            // 
            this.venueDetails1.Location = new System.Drawing.Point(53, 67);
            this.venueDetails1.Name = "venueDetails1";
            this.venueDetails1.Size = new System.Drawing.Size(150, 150);
            this.venueDetails1.TabIndex = 11;
            this.venueDetails1.Visible = false;
            // 
            // help1
            // 
            this.help1.Location = new System.Drawing.Point(53, 55);
            this.help1.Name = "help1";
            this.help1.Size = new System.Drawing.Size(150, 150);
            this.help1.TabIndex = 14;
            this.help1.Visible = false;
            // 
            // contextHelp1
            // 
            this.contextHelp1.Location = new System.Drawing.Point(43, 55);
            this.contextHelp1.Name = "contextHelp1";
            this.contextHelp1.Size = new System.Drawing.Size(134, 136);
            this.contextHelp1.TabIndex = 17;
            this.contextHelp1.Text = "contextHelp1";
            this.contextHelp1.Visible = false;
            this.contextHelp1.Click += new System.EventHandler(this.contextHelp1_Click);
            // 
            // timerTutorial
            // 
            this.timerTutorial.Interval = 4000;
            this.timerTutorial.Tick += new System.EventHandler(this.timerTutorial_Tick);
            // 
            // timerAds
            // 
            this.timerAds.Interval = 10000;
            this.timerAds.Tick += new System.EventHandler(this.timerAds_Tick);
            // 
            // picGps
            // 
            this.picGps.BackColor = System.Drawing.Color.Black;
            this.picGps.Location = new System.Drawing.Point(191, 24);
            this.picGps.Name = "picGps";
            this.picGps.Size = new System.Drawing.Size(46, 25);
            this.picGps.Paint += new System.Windows.Forms.PaintEventHandler(this.picGps_Paint);
            // 
            // moreActions1
            // 
            this.moreActions1.Location = new System.Drawing.Point(78, 55);
            this.moreActions1.Name = "moreActions1";
            this.moreActions1.Size = new System.Drawing.Size(150, 150);
            this.moreActions1.TabIndex = 20;
            this.moreActions1.Visible = false;
            // 
            // leaderboard1
            // 
            this.leaderboard1.Location = new System.Drawing.Point(63, 55);
            this.leaderboard1.Name = "leaderboard1";
            this.leaderboard1.Size = new System.Drawing.Size(150, 150);
            this.leaderboard1.TabIndex = 24;
            this.leaderboard1.Visible = false;
            // 
            // shout1
            // 
            this.shout1.Location = new System.Drawing.Point(63, 55);
            this.shout1.Name = "shout1";
            this.shout1.Size = new System.Drawing.Size(150, 150);
            this.shout1.TabIndex = 28;
            this.shout1.Visible = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.shout1);
            this.Controls.Add(this.leaderboard1);
            this.Controls.Add(this.moreActions1);
            this.Controls.Add(this.picGps);
            this.Controls.Add(this.contextHelp1);
            this.Controls.Add(this.help1);
            this.Controls.Add(this.venueDetails1);
            this.Controls.Add(this.createVenue1);
            this.Controls.Add(this.userDetail1);
            this.Controls.Add(this.friends1);
            this.Controls.Add(this.picAd);
            this.Controls.Add(this.settings1);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.places1);
            this.Controls.Add(this.header);
            this.Menu = this.mainMenu;
            this.Name = "Main";
            this.Text = "MySquare";
            this.Load += new System.EventHandler(this.Main_Load);
            this.picAd.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal MySquare.UI.Places.Places places1;
        internal System.Windows.Forms.MenuItem mnuLeft;
        internal System.Windows.Forms.MenuItem mnuRight;
        internal MySquare.UI.Settings.Settings settings1;
        internal System.Windows.Forms.Label lblError;
        internal MySquare.UI.Friends.Friends friends1;
        internal MySquare.UI.Friends.UserDetail userDetail1;
        internal MySquare.UI.Places.Create.CreateVenue createVenue1;
        internal MySquare.UI.Places.VenueDetails venueDetails1;
        internal Tenor.Mobile.UI.HeaderStrip header;
        private ContextHelp contextHelp1;
        private System.Windows.Forms.Timer timerTutorial;
        private System.Windows.Forms.Timer timerAds;
        internal Microsoft.WindowsCE.Forms.InputPanel inputPanel;
        internal System.Windows.Forms.Panel picAd;
        private System.Windows.Forms.LinkLabel lnkTextLink;
        internal Help help1;
        private System.Windows.Forms.PictureBox picGps;
        internal MySquare.UI.More.MoreActions moreActions1;
        internal MySquare.UI.More.Leaderboard leaderboard1;
        internal MySquare.UI.More.Shout shout1;
    }
}

