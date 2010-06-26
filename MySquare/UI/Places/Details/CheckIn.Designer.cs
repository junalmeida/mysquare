namespace MySquare.UI.Places.Details
{
    partial class CheckIn
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
            this.chkFacebook = new System.Windows.Forms.CheckBox();
            this.chkTwitter = new System.Windows.Forms.CheckBox();
            this.chkTellFriends = new System.Windows.Forms.CheckBox();
            this.txtShout = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // chkFacebook
            // 
            this.chkFacebook.ThreeState = true;
            this.chkFacebook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFacebook.Checked = true;
            this.chkFacebook.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chkFacebook.ForeColor = System.Drawing.Color.Black;
            this.chkFacebook.Location = new System.Drawing.Point(134, 92);
            this.chkFacebook.Name = "chkFacebook";
            this.chkFacebook.Size = new System.Drawing.Size(124, 20);
            this.chkFacebook.TabIndex = 10;
            this.chkFacebook.Text = "Update Facebook";
            // 
            // chkTwitter
            // 
            this.chkTwitter.ThreeState = true;
            this.chkTwitter.Checked = true;
            this.chkTwitter.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chkTwitter.ForeColor = System.Drawing.Color.Black;
            this.chkTwitter.Location = new System.Drawing.Point(3, 92);
            this.chkTwitter.Name = "chkTwitter";
            this.chkTwitter.Size = new System.Drawing.Size(118, 20);
            this.chkTwitter.TabIndex = 11;
            this.chkTwitter.Text = "Update Twitter";
            // 
            // chkTellFriends
            // 
            this.chkTellFriends.Checked = true;
            this.chkTellFriends.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTellFriends.ForeColor = System.Drawing.Color.Black;
            this.chkTellFriends.Location = new System.Drawing.Point(3, 66);
            this.chkTellFriends.Name = "chkTellFriends";
            this.chkTellFriends.Size = new System.Drawing.Size(141, 20);
            this.chkTellFriends.TabIndex = 9;
            this.chkTellFriends.Text = "Tell my friends";
            // 
            // txtShout
            // 
            this.txtShout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtShout.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtShout.Location = new System.Drawing.Point(3, 3);
            this.txtShout.MaxLength = 140;
            this.txtShout.Multiline = true;
            this.txtShout.Name = "txtShout";
            this.txtShout.Size = new System.Drawing.Size(255, 58);
            this.txtShout.TabIndex = 8;
            this.txtShout.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtShout_KeyDown);
            // 
            // CheckIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(204)))), ((int)(((byte)(212)))));
            this.Controls.Add(this.chkFacebook);
            this.Controls.Add(this.chkTwitter);
            this.Controls.Add(this.chkTellFriends);
            this.Controls.Add(this.txtShout);
            this.Name = "CheckIn";
            this.Size = new System.Drawing.Size(261, 289);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkFacebook;
        private System.Windows.Forms.CheckBox chkTwitter;
        private System.Windows.Forms.CheckBox chkTellFriends;
        private System.Windows.Forms.TextBox txtShout;
    }
}
