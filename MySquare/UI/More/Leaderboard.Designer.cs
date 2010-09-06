﻿namespace MySquare.UI.More
{
    partial class Leaderboard
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
            this.panel = new Tenor.Mobile.UI.KListControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblFriendStatus = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.tabStrip = new MySquare.UI.TabStrip();
            this.picAvatar = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(204)))), ((int)(((byte)(212)))));
            this.panel.DefaultItemHeight = 38;
            this.panel.DefaultItemWidth = 80;
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Layout = Tenor.Mobile.UI.KListLayout.Vertical;
            this.panel.Location = new System.Drawing.Point(0, 71);
            this.panel.Name = "panel";
            this.panel.SeparatorColor = System.Drawing.SystemColors.InactiveBorder;
            this.panel.Size = new System.Drawing.Size(350, 189);
            this.panel.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.lblFriendStatus);
            this.panel1.Controls.Add(this.lblUserName);
            this.panel1.Controls.Add(this.tabStrip);
            this.panel1.Controls.Add(this.picAvatar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(350, 71);
            // 
            // lblFriendStatus
            // 
            this.lblFriendStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFriendStatus.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblFriendStatus.ForeColor = System.Drawing.Color.White;
            this.lblFriendStatus.Location = new System.Drawing.Point(33, 24);
            this.lblFriendStatus.Name = "lblFriendStatus";
            this.lblFriendStatus.Size = new System.Drawing.Size(311, 16);
            this.lblFriendStatus.Text = "Leaderboard resets every Sunday night";
            // 
            // lblUserName
            // 
            this.lblUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblUserName.ForeColor = System.Drawing.Color.White;
            this.lblUserName.Location = new System.Drawing.Point(33, 7);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(311, 16);
            this.lblUserName.Text = "The Leaderboard";
            // 
            // tabStrip
            // 
            this.tabStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabStrip.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.tabStrip.Location = new System.Drawing.Point(0, 46);
            this.tabStrip.Name = "tabStrip";
            this.tabStrip.Size = new System.Drawing.Size(350, 25);
            this.tabStrip.TabIndex = 5;
            this.tabStrip.Text = "tabStrip1";
            // 
            // picAvatar
            // 
            this.picAvatar.Location = new System.Drawing.Point(-1, 4);
            this.picAvatar.Name = "picAvatar";
            this.picAvatar.Size = new System.Drawing.Size(36, 36);
            this.picAvatar.Paint += new System.Windows.Forms.PaintEventHandler(this.picAvatar_Paint);
            // 
            // Leaderboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.panel);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "Leaderboard";
            this.Size = new System.Drawing.Size(350, 260);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label lblFriendStatus;
        internal TabStrip tabStrip;
        internal System.Windows.Forms.Label lblUserName;
        internal System.Windows.Forms.PictureBox picAvatar;
        private Tenor.Mobile.UI.KListControl panel;

    }
}
