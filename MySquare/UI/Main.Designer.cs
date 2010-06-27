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
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.mnuLeft = new System.Windows.Forms.MenuItem();
            this.mnuRight = new System.Windows.Forms.MenuItem();
            this.header = new Tenor.Mobile.UI.HeaderStrip();
            this.lblError = new System.Windows.Forms.Label();
            this.settings1 = new MySquare.UI.Settings.Settings();
            this.places1 = new MySquare.UI.Places.Places();
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
            this.mnuRight.Click += new System.EventHandler(this.mnuRight_Click);
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
            this.lblError.Location = new System.Drawing.Point(3, 126);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(234, 29);
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
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.settings1);
            this.Controls.Add(this.places1);
            this.Controls.Add(this.header);
            this.Menu = this.mainMenu;
            this.Name = "Main";
            this.Text = "MySquare";
            this.ResumeLayout(false);

        }

        #endregion

        private Tenor.Mobile.UI.HeaderStrip header;
        private MySquare.UI.Places.Places places1;
        private System.Windows.Forms.MenuItem mnuLeft;
        private System.Windows.Forms.MenuItem mnuRight;
        private MySquare.UI.Settings.Settings settings1;
        private System.Windows.Forms.Label lblError;
    }
}

