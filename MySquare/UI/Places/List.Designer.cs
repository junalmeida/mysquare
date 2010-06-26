namespace MySquare.UI.Places
{
    partial class List
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
            this.listBox = new Tenor.Mobile.UI.KListControl();
            this.imgLoading = new System.Windows.Forms.PictureBox();
            this.lblError = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            this.listBox.Size = new System.Drawing.Size(299, 244);
            this.listBox.TabIndex = 0;
            this.listBox.Visible = false;
            // 
            // imgLoading
            // 
            this.imgLoading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.imgLoading.Location = new System.Drawing.Point(138, 114);
            this.imgLoading.Name = "imgLoading";
            this.imgLoading.Size = new System.Drawing.Size(22, 16);
            this.imgLoading.Visible = false;
            // 
            // lblError
            // 
            this.lblError.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblError.Location = new System.Drawing.Point(70, 112);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(158, 20);
            this.lblError.Text = "Cannot list nearby venues.";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblError.Visible = false;
            // 
            // List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.imgLoading);
            this.Controls.Add(this.listBox);
            this.Name = "List";
            this.Size = new System.Drawing.Size(299, 244);
            this.ResumeLayout(false);

        }

        #endregion

        private Tenor.Mobile.UI.KListControl listBox;
        private System.Windows.Forms.PictureBox imgLoading;
        private System.Windows.Forms.Label lblError;
    }
}
