namespace MySquare.UI.More
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
            this.lstAll = new Tenor.Mobile.UI.KListControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblRefreshTime = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.picAvatar = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstAll
            // 
            this.lstAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(204)))), ((int)(((byte)(212)))));
            this.lstAll.DefaultItemHeight = 30;
            this.lstAll.DefaultItemWidth = 80;
            this.lstAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAll.Layout = Tenor.Mobile.UI.KListLayout.Vertical;
            this.lstAll.Location = new System.Drawing.Point(0, 44);
            this.lstAll.Name = "lstAll";
            this.lstAll.SeparatorColor = System.Drawing.SystemColors.InactiveBorder;
            this.lstAll.Size = new System.Drawing.Size(350, 216);
            this.lstAll.Skinnable = false;
            this.lstAll.TabIndex = 0;
            this.lstAll.DrawItem += new Tenor.Mobile.UI.DrawItemEventHandler(this.lstAll_DrawItem);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.lblRefreshTime);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.picAvatar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(350, 44);
            // 
            // lblRefreshTime
            // 
            this.lblRefreshTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRefreshTime.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular);
            this.lblRefreshTime.ForeColor = System.Drawing.Color.White;
            this.lblRefreshTime.Location = new System.Drawing.Point(31, 22);
            this.lblRefreshTime.Name = "lblRefreshTime";
            this.lblRefreshTime.Size = new System.Drawing.Size(320, 16);
            this.lblRefreshTime.Text = "Leaderboard resets every Sunday night";
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(31, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(311, 16);
            this.lblTitle.Text = "The Leaderboard";
            // 
            // picAvatar
            // 
            this.picAvatar.Location = new System.Drawing.Point(0, 4);
            this.picAvatar.Name = "picAvatar";
            this.picAvatar.Size = new System.Drawing.Size(32, 32);
            this.picAvatar.Paint += new System.Windows.Forms.PaintEventHandler(this.picAvatar_Paint);
            // 
            // Leaderboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(204)))), ((int)(((byte)(212)))));
            this.Controls.Add(this.lstAll);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "Leaderboard";
            this.Size = new System.Drawing.Size(350, 260);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label lblRefreshTime;
        private System.Windows.Forms.Label lblTitle;
        internal System.Windows.Forms.PictureBox picAvatar;
        internal Tenor.Mobile.UI.KListControl lstAll;

    }
}
