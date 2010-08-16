namespace MySquare.UI.Friends
{
    partial class UserDetail
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
            this.picAvatar = new System.Windows.Forms.PictureBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblFriendStatus = new System.Windows.Forms.Label();
            this.tabStrip = new MySquare.UI.TabStrip();
            this.userInfo1 = new MySquare.UI.Friends.UserInfo();
            this.userFriends1 = new MySquare.UI.Friends.UserFriends();
            this.userBadges1 = new MySquare.UI.Friends.UserBadges();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picAvatar
            // 
            this.picAvatar.Location = new System.Drawing.Point(4, 4);
            this.picAvatar.Name = "picAvatar";
            this.picAvatar.Size = new System.Drawing.Size(48, 48);
            this.picAvatar.Paint += new System.Windows.Forms.PaintEventHandler(this.picAvatar_Paint);
            // 
            // lblUserName
            // 
            this.lblUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblUserName.ForeColor = System.Drawing.Color.White;
            this.lblUserName.Location = new System.Drawing.Point(58, 9);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(191, 16);
            this.lblUserName.Text = "label1";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.lblFriendStatus);
            this.panel1.Controls.Add(this.tabStrip);
            this.panel1.Controls.Add(this.lblUserName);
            this.panel1.Controls.Add(this.picAvatar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(253, 80);
            // 
            // lblFriendStatus
            // 
            this.lblFriendStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFriendStatus.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblFriendStatus.ForeColor = System.Drawing.Color.White;
            this.lblFriendStatus.Location = new System.Drawing.Point(58, 26);
            this.lblFriendStatus.Name = "lblFriendStatus";
            this.lblFriendStatus.Size = new System.Drawing.Size(191, 16);
            this.lblFriendStatus.Text = "label1";
            // 
            // tabStrip
            // 
            this.tabStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabStrip.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.tabStrip.Location = new System.Drawing.Point(0, 55);
            this.tabStrip.Name = "tabStrip";
            this.tabStrip.Size = new System.Drawing.Size(253, 25);
            this.tabStrip.TabIndex = 5;
            this.tabStrip.Text = "tabStrip1";
            this.tabStrip.SelectedIndexChanged += new System.EventHandler(this.tabStrip_SelectedIndexChanged);
            // 
            // userInfo1
            // 
            this.userInfo1.Location = new System.Drawing.Point(32, 86);
            this.userInfo1.Name = "userInfo1";
            this.userInfo1.Size = new System.Drawing.Size(137, 115);
            this.userInfo1.TabIndex = 1;
            this.userInfo1.Visible = false;
            // 
            // userFriends1
            // 
            this.userFriends1.Location = new System.Drawing.Point(80, 68);
            this.userFriends1.Name = "userFriends1";
            this.userFriends1.Size = new System.Drawing.Size(150, 150);
            this.userFriends1.TabIndex = 3;
            this.userFriends1.Visible = false;
            // 
            // userBadges1
            // 
            this.userBadges1.Location = new System.Drawing.Point(99, 86);
            this.userBadges1.Name = "userBadges1";
            this.userBadges1.Size = new System.Drawing.Size(150, 150);
            this.userBadges1.TabIndex = 5;
            this.userBadges1.Visible = false;
            // 
            // UserDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.userBadges1);
            this.Controls.Add(this.userFriends1);
            this.Controls.Add(this.userInfo1);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "UserDetail";
            this.Size = new System.Drawing.Size(253, 243);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        internal System.Windows.Forms.PictureBox picAvatar;
        private System.Windows.Forms.Panel panel1;
        internal TabStrip tabStrip;
        internal System.Windows.Forms.Label lblUserName;
        internal UserInfo userInfo1;
        internal System.Windows.Forms.Label lblFriendStatus;
        internal UserFriends userFriends1;
        internal UserBadges userBadges1;
    }
}
