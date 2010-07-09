namespace MySquare.UI.Places.Details
{
    partial class VenueTips
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
            this.lblError = new System.Windows.Forms.Label();
            this.txtComment = new Tenor.Mobile.UI.TextControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listBox = new Tenor.Mobile.UI.KListControl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblError
            // 
            this.lblError.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblError.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblError.ForeColor = System.Drawing.Color.Black;
            this.lblError.Location = new System.Drawing.Point(93, 123);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(121, 15);
            this.lblError.Text = "No comments yet.";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblError.Visible = false;
            // 
            // txtComment
            // 
            this.txtComment.AcceptsReturn = false;
            this.txtComment.AcceptsTab = false;
            this.txtComment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtComment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtComment.HideSelection = true;
            this.txtComment.InputMode = Microsoft.WindowsCE.Forms.InputMode.Default;
            this.txtComment.Location = new System.Drawing.Point(3, 3);
            this.txtComment.MaxLength = 32767;
            this.txtComment.Modified = false;
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.PasswordChar = '\0';
            this.txtComment.ReadOnly = false;
            this.txtComment.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtComment.SelectedText = "";
            this.txtComment.SelectionLength = 0;
            this.txtComment.SelectionStart = 0;
            this.txtComment.Size = new System.Drawing.Size(300, 33);
            this.txtComment.TabIndex = 0;
            this.txtComment.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtComment.WordWrap = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtComment);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 250);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(306, 39);
            // 
            // listBox
            // 
            this.listBox.DefaultItemHeight = 38;
            this.listBox.DefaultItemWidth = 80;
            this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox.Layout = Tenor.Mobile.UI.KListLayout.Vertical;
            this.listBox.Location = new System.Drawing.Point(0, 0);
            this.listBox.Name = "listBox";
            this.listBox.SeparatorColor = System.Drawing.SystemColors.InactiveBorder;
            this.listBox.Size = new System.Drawing.Size(306, 250);
            this.listBox.Skinnable = false;
            this.listBox.TabIndex = 4;
            this.listBox.SelectedItemChanged += new System.EventHandler(this.listBox_SelectedItemChanged);
            this.listBox.DrawItem += new Tenor.Mobile.UI.DrawItemEventHandler(this.listBox_DrawItem);
            this.listBox.SelectedItemClicked += new System.EventHandler(this.listBox_SelectedItemClicked);
            // 
            // VenueTips
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.panel1);
            this.Name = "VenueTips";
            this.Size = new System.Drawing.Size(306, 289);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal Tenor.Mobile.UI.TextControl txtComment;
        internal System.Windows.Forms.Label lblError;
        internal System.Windows.Forms.Panel panel1;
        internal Tenor.Mobile.UI.KListControl listBox;

    }
}
