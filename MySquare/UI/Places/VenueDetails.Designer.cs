namespace MySquare.UI.Places
{
    partial class VenueDetails
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
            this.lblVenueName = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtShout = new System.Windows.Forms.TextBox();
            this.chkTellFriends = new System.Windows.Forms.CheckBox();
            this.chkFacebook = new System.Windows.Forms.CheckBox();
            this.chkTwitter = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblVenueName
            // 
            this.lblVenueName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVenueName.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblVenueName.ForeColor = System.Drawing.Color.White;
            this.lblVenueName.Location = new System.Drawing.Point(3, 4);
            this.lblVenueName.Name = "lblVenueName";
            this.lblVenueName.Size = new System.Drawing.Size(242, 19);
            this.lblVenueName.Text = "Venue Name";
            // 
            // lblAddress
            // 
            this.lblAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAddress.ForeColor = System.Drawing.Color.White;
            this.lblAddress.Location = new System.Drawing.Point(3, 24);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(242, 15);
            this.lblAddress.Text = "addresspç";
            // 
            // txtShout
            // 
            this.txtShout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtShout.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtShout.Location = new System.Drawing.Point(3, 46);
            this.txtShout.Multiline = true;
            this.txtShout.Name = "txtShout";
            this.txtShout.Size = new System.Drawing.Size(239, 75);
            this.txtShout.TabIndex = 2;
            // 
            // chkTellFriends
            // 
            this.chkTellFriends.Checked = true;
            this.chkTellFriends.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTellFriends.ForeColor = System.Drawing.Color.White;
            this.chkTellFriends.Location = new System.Drawing.Point(3, 135);
            this.chkTellFriends.Name = "chkTellFriends";
            this.chkTellFriends.Size = new System.Drawing.Size(141, 20);
            this.chkTellFriends.TabIndex = 3;
            this.chkTellFriends.Text = "Tell my friends";
            // 
            // chkFacebook
            // 
            this.chkFacebook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFacebook.ForeColor = System.Drawing.Color.White;
            this.chkFacebook.Location = new System.Drawing.Point(118, 161);
            this.chkFacebook.Name = "chkFacebook";
            this.chkFacebook.Size = new System.Drawing.Size(124, 20);
            this.chkFacebook.TabIndex = 4;
            this.chkFacebook.Text = "Update Facebook";
            // 
            // chkTwitter
            // 
            this.chkTwitter.ForeColor = System.Drawing.Color.White;
            this.chkTwitter.Location = new System.Drawing.Point(3, 161);
            this.chkTwitter.Name = "chkTwitter";
            this.chkTwitter.Size = new System.Drawing.Size(118, 20);
            this.chkTwitter.TabIndex = 5;
            this.chkTwitter.Text = "Update Twitter";
            // 
            // VenueDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.chkFacebook);
            this.Controls.Add(this.chkTwitter);
            this.Controls.Add(this.chkTellFriends);
            this.Controls.Add(this.txtShout);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.lblVenueName);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "VenueDetails";
            this.Size = new System.Drawing.Size(245, 232);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblVenueName;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtShout;
        private System.Windows.Forms.CheckBox chkTellFriends;
        private System.Windows.Forms.CheckBox chkFacebook;
        private System.Windows.Forms.CheckBox chkTwitter;
    }
}
