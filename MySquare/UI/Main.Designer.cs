﻿namespace MySquare.UI
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
            this.picAd = new System.Windows.Forms.PictureBox();
            this.friends1 = new MySquare.UI.Friends.Friends();
            this.userDetail1 = new MySquare.UI.Friends.UserDetail();
            this.createVenue1 = new MySquare.UI.Places.Create.CreateVenue();
            this.venueDetails1 = new MySquare.UI.Places.VenueDetails();
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
            this.picAd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.picAd.Location = new System.Drawing.Point(0, 218);
            this.picAd.Name = "picAd";
            this.picAd.Size = new System.Drawing.Size(240, 50);
            this.picAd.Visible = false;
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
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(240, 268);
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
            this.ResumeLayout(false);

        }

        #endregion

        private Tenor.Mobile.UI.HeaderStrip header;
        internal MySquare.UI.Places.Places places1;
        internal System.Windows.Forms.MenuItem mnuLeft;
        internal System.Windows.Forms.MenuItem mnuRight;
        internal MySquare.UI.Settings.Settings settings1;
        internal System.Windows.Forms.Label lblError;
        private Microsoft.WindowsCE.Forms.InputPanel inputPanel;
        private System.Windows.Forms.PictureBox picAd;
        internal MySquare.UI.Friends.Friends friends1;
        internal MySquare.UI.Friends.UserDetail userDetail1;
        internal MySquare.UI.Places.Create.CreateVenue createVenue1;
        internal MySquare.UI.Places.VenueDetails venueDetails1;
    }
}

