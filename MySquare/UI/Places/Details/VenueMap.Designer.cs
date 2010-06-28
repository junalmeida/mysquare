namespace MySquare.UI.Places.Details
{
    partial class VenueMap
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
            this.picMap = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // picMap
            // 
            this.picMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.picMap.Location = new System.Drawing.Point(3, 3);
            this.picMap.Name = "picMap";
            this.picMap.Size = new System.Drawing.Size(225, 188);
            this.picMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            // 
            // VenueMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.picMap);
            this.Name = "VenueMap";
            this.Size = new System.Drawing.Size(231, 194);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.PictureBox picMap;

    }
}
