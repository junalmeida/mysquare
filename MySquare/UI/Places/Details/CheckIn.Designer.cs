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
            this.txtShout = new Tenor.Mobile.UI.TextControl();
            this.pnlShout = new System.Windows.Forms.Panel();
            this.pnlCheckInResult = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.pnlShout.SuspendLayout();
            this.pnlCheckInResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkFacebook
            // 
            this.chkFacebook.ThreeState = true;
            this.chkFacebook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFacebook.Checked = true;
            this.chkFacebook.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chkFacebook.ForeColor = System.Drawing.Color.Black;
            this.chkFacebook.Location = new System.Drawing.Point(99, 92);
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
            this.chkTellFriends.CheckStateChanged += new System.EventHandler(this.chkTellFriends_CheckStateChanged);
            // 
            // txtShout
            // 
            this.txtShout.AcceptsReturn = false;
            this.txtShout.AcceptsTab = false;
            this.txtShout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtShout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShout.HideSelection = true;
            this.txtShout.InputMode = Microsoft.WindowsCE.Forms.InputMode.Default;
            this.txtShout.Location = new System.Drawing.Point(3, 3);
            this.txtShout.MaxLength = 140;
            this.txtShout.Modified = false;
            this.txtShout.Multiline = true;
            this.txtShout.Name = "txtShout";
            this.txtShout.PasswordChar = '\0';
            this.txtShout.ReadOnly = false;
            this.txtShout.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtShout.SelectedText = "";
            this.txtShout.SelectionLength = 0;
            this.txtShout.SelectionStart = 0;
            this.txtShout.Size = new System.Drawing.Size(220, 58);
            this.txtShout.TabIndex = 8;
            this.txtShout.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtShout.WordWrap = true;
            this.txtShout.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtShout_KeyDown);
            // 
            // pnlShout
            // 
            this.pnlShout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(204)))), ((int)(((byte)(212)))));
            this.pnlShout.Controls.Add(this.txtShout);
            this.pnlShout.Controls.Add(this.chkFacebook);
            this.pnlShout.Controls.Add(this.chkTellFriends);
            this.pnlShout.Controls.Add(this.chkTwitter);
            this.pnlShout.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlShout.Location = new System.Drawing.Point(0, 0);
            this.pnlShout.Name = "pnlShout";
            this.pnlShout.Size = new System.Drawing.Size(226, 117);
            // 
            // pnlCheckInResult
            // 
            this.pnlCheckInResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(204)))), ((int)(((byte)(212)))));
            this.pnlCheckInResult.Controls.Add(this.lblMessage);
            this.pnlCheckInResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCheckInResult.Location = new System.Drawing.Point(0, 117);
            this.pnlCheckInResult.Name = "pnlCheckInResult";
            this.pnlCheckInResult.Size = new System.Drawing.Size(226, 170);
            this.pnlCheckInResult.Visible = false;
            this.pnlCheckInResult.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCheckInResult_Paint);
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.BackColor = System.Drawing.Color.White;
            this.lblMessage.ForeColor = System.Drawing.Color.Black;
            this.lblMessage.Location = new System.Drawing.Point(12, 11);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(201, 147);
            this.lblMessage.Text = "OK! We\\\'ve got you @ Restaurante Quinta da Boa Vista. This is your 8th checkin he" +
                "re!";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // CheckIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(204)))), ((int)(((byte)(212)))));
            this.Controls.Add(this.pnlCheckInResult);
            this.Controls.Add(this.pnlShout);
            this.Name = "CheckIn";
            this.Size = new System.Drawing.Size(226, 287);
            this.pnlShout.ResumeLayout(false);
            this.pnlCheckInResult.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.CheckBox chkFacebook;
        internal System.Windows.Forms.CheckBox chkTwitter;
        internal System.Windows.Forms.CheckBox chkTellFriends;
        internal Tenor.Mobile.UI.TextControl txtShout;
        internal System.Windows.Forms.Panel pnlShout;
        internal System.Windows.Forms.Panel pnlCheckInResult;
        internal System.Windows.Forms.Label lblMessage;

    }
}
