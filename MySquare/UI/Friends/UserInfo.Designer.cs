namespace MySquare.UI.Friends
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
            this.SuspendLayout();
            // 
            // lblFacebook
            // 
            this.lblFacebook.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFacebook.Location = new System.Drawing.Point(24, 69);
            this.lblFacebook.Name = "lblFacebook";
            this.lblFacebook.Size = new System.Drawing.Size(181, 20);
            this.lblFacebook.TabIndex = 8;
            this.lblFacebook.Text = "linkLabel1";
            this.lblFacebook.Click += new System.EventHandler(this.lblFacebook_Click);
            // 
            // lblTwitter
            // 
            this.lblTwitter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTwitter.Location = new System.Drawing.Point(24, 39);
            this.lblTwitter.Name = "lblTwitter";
            this.lblTwitter.Size = new System.Drawing.Size(181, 20);
            this.lblTwitter.TabIndex = 9;
            this.lblTwitter.Text = "linkLabel1";
            this.lblTwitter.Click += new System.EventHandler(this.lblTwitter_Click);
            // 
            // lblEmail
            // 
            this.lblEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEmail.Location = new System.Drawing.Point(24, 9);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(181, 20);
            this.lblEmail.TabIndex = 10;
            this.lblEmail.Text = "linkLabel1";
            this.lblEmail.Click += new System.EventHandler(this.lblEmail_Click);
            // 
            // UserInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(204)))), ((int)(((byte)(212)))));
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblTwitter);
            this.Controls.Add(this.lblFacebook);
            this.Name = "UserInfo";
            this.Size = new System.Drawing.Size(208, 236);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UserInfo_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.LinkLabel lblFacebook;
        internal System.Windows.Forms.LinkLabel lblTwitter;
        internal System.Windows.Forms.LinkLabel lblEmail;
    }
}
