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
            this.panelDock = new System.Windows.Forms.Panel();
            this.lblError = new System.Windows.Forms.Label();
            this.listBox = new Tenor.Mobile.UI.KListControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.panelDock.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelDock
            // 
            this.panelDock.Controls.Add(this.lblError);
            this.panelDock.Controls.Add(this.listBox);
            this.panelDock.Controls.Add(this.panel1);
            this.panelDock.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDock.Location = new System.Drawing.Point(0, 0);
            this.panelDock.Name = "panelDock";
            this.panelDock.Size = new System.Drawing.Size(306, 100);
            // 
            // lblError
            // 
            this.lblError.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblError.Location = new System.Drawing.Point(93, 16);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(121, 20);
            this.lblError.Text = "No comments yet.";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblError.Visible = false;
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
            this.listBox.Size = new System.Drawing.Size(306, 61);
            this.listBox.Skinnable = false;
            this.listBox.TabIndex = 3;
            this.listBox.SelectedItemChanged += new System.EventHandler(this.listBox_SelectedItemChanged);
            this.listBox.DrawItem += new Tenor.Mobile.UI.DrawItemEventHandler(this.listBox_DrawItem);
            this.listBox.SelectedItemClicked += new System.EventHandler(this.listBox_SelectedItemClicked);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtComment);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 61);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(306, 39);
            // 
            // txtComment
            // 
            this.txtComment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtComment.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtComment.Location = new System.Drawing.Point(3, 3);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(300, 33);
            this.txtComment.TabIndex = 0;
            // 
            // VenueTips
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.panelDock);
            this.Name = "VenueTips";
            this.Size = new System.Drawing.Size(306, 289);
            this.panelDock.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelDock;
        internal Tenor.Mobile.UI.KListControl listBox;
        internal System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.TextBox txtComment;
        internal System.Windows.Forms.Label lblError;

    }
}
