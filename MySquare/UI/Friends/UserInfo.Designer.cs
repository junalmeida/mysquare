﻿namespace MySquare.UI.Friends
{
    partial class UserInfo
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
            this.lblFacebook = new System.Windows.Forms.LinkLabel();
            this.lblTwitter = new System.Windows.Forms.LinkLabel();
            this.lblEmail = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblShout = new System.Windows.Forms.Label();
            this.lblShoutT = new System.Windows.Forms.Label();
            this.lblLastSeenT = new System.Windows.Forms.Label();
            this.lblLastSeen = new System.Windows.Forms.LinkLabel();
            this.lnkFoursquare = new System.Windows.Forms.LinkLabel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFacebook
            // 
            this.lblFacebook.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFacebook.Location = new System.Drawing.Point(21, 90);
            this.lblFacebook.Name = "lblFacebook";
            this.lblFacebook.Size = new System.Drawing.Size(165, 20);
            this.lblFacebook.TabIndex = 8;
            this.lblFacebook.Text = "loading...";
            this.lblFacebook.Click += new System.EventHandler(this.lblFacebook_Click);
            // 
            // lblTwitter
            // 
            this.lblTwitter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTwitter.Location = new System.Drawing.Point(21, 62);
            this.lblTwitter.Name = "lblTwitter";
            this.lblTwitter.Size = new System.Drawing.Size(165, 20);
            this.lblTwitter.TabIndex = 9;
            this.lblTwitter.Text = "loading...";
            this.lblTwitter.Click += new System.EventHandler(this.lblTwitter_Click);
            // 
            // lblEmail
            // 
            this.lblEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEmail.Location = new System.Drawing.Point(21, 34);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(165, 20);
            this.lblEmail.TabIndex = 10;
            this.lblEmail.Text = "loading...";
            this.lblEmail.Click += new System.EventHandler(this.lblEmail_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(204)))), ((int)(((byte)(212)))));
            this.panel1.Controls.Add(this.lnkFoursquare);
            this.panel1.Controls.Add(this.lblEmail);
            this.panel1.Controls.Add(this.lblFacebook);
            this.panel1.Controls.Add(this.lblTwitter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(208, 111);
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.UserInfo_Paint);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VenueInfo_MouseMove);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VenueInfo_MouseDown);
            // 
            // lblShout
            // 
            this.lblShout.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblShout.ForeColor = System.Drawing.Color.Black;
            this.lblShout.Location = new System.Drawing.Point(0, 127);
            this.lblShout.Name = "lblShout";
            this.lblShout.Size = new System.Drawing.Size(208, 25);
            this.lblShout.Text = "label2";
            this.lblShout.Visible = false;
            this.lblShout.TextChanged += new System.EventHandler(this.lblShout_TextChanged);
            // 
            // lblShoutT
            // 
            this.lblShoutT.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblShoutT.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblShoutT.ForeColor = System.Drawing.Color.Black;
            this.lblShoutT.Location = new System.Drawing.Point(0, 111);
            this.lblShoutT.Name = "lblShoutT";
            this.lblShoutT.Size = new System.Drawing.Size(208, 16);
            this.lblShoutT.Text = "Last shout:";
            this.lblShoutT.Visible = false;
            // 
            // lblLastSeenT
            // 
            this.lblLastSeenT.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLastSeenT.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblLastSeenT.ForeColor = System.Drawing.Color.Black;
            this.lblLastSeenT.Location = new System.Drawing.Point(0, 152);
            this.lblLastSeenT.Name = "lblLastSeenT";
            this.lblLastSeenT.Size = new System.Drawing.Size(208, 16);
            this.lblLastSeenT.Text = "Last seen:";
            this.lblLastSeenT.Visible = false;
            // 
            // lblLastSeen
            // 
            this.lblLastSeen.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLastSeen.Location = new System.Drawing.Point(0, 168);
            this.lblLastSeen.Name = "lblLastSeen";
            this.lblLastSeen.Size = new System.Drawing.Size(208, 25);
            this.lblLastSeen.TabIndex = 22;
            this.lblLastSeen.Text = "linkLabel1";
            this.lblLastSeen.Visible = false;
            this.lblLastSeen.TextChanged += new System.EventHandler(this.lblLastSeen_TextChanged);
            this.lblLastSeen.Click += new System.EventHandler(this.lblLastSeen_Click);
            // 
            // lnkFoursquare
            // 
            this.lnkFoursquare.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkFoursquare.Location = new System.Drawing.Point(21, 6);
            this.lnkFoursquare.Name = "lnkFoursquare";
            this.lnkFoursquare.Size = new System.Drawing.Size(165, 20);
            this.lnkFoursquare.TabIndex = 11;
            this.lnkFoursquare.Text = "loading...";
            this.lnkFoursquare.Click += new System.EventHandler(this.lnkFoursquare_Click);
            // 
            // UserInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(204)))), ((int)(((byte)(212)))));
            this.Controls.Add(this.lblLastSeen);
            this.Controls.Add(this.lblLastSeenT);
            this.Controls.Add(this.lblShout);
            this.Controls.Add(this.lblShoutT);
            this.Controls.Add(this.panel1);
            this.Name = "UserInfo";
            this.Size = new System.Drawing.Size(208, 339);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VenueInfo_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VenueInfo_MouseDown);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.LinkLabel lblFacebook;
        internal System.Windows.Forms.LinkLabel lblTwitter;
        internal System.Windows.Forms.LinkLabel lblEmail;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblShoutT;
        internal System.Windows.Forms.Label lblShout;
        private System.Windows.Forms.Label lblLastSeenT;
        internal System.Windows.Forms.LinkLabel lblLastSeen;
        internal System.Windows.Forms.LinkLabel lnkFoursquare;
    }
}
