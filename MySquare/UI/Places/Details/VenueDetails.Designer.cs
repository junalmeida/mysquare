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
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblVenueName = new System.Windows.Forms.Label();
            this.checkIn1 = new MySquare.UI.Places.Details.CheckIn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAddress
            // 
            this.lblAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAddress.ForeColor = System.Drawing.Color.White;
            this.lblAddress.Location = new System.Drawing.Point(0, 20);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(435, 15);
            this.lblAddress.Text = "addresspç";
            // 
            // lblVenueName
            // 
            this.lblVenueName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVenueName.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblVenueName.ForeColor = System.Drawing.Color.White;
            this.lblVenueName.Location = new System.Drawing.Point(0, 0);
            this.lblVenueName.Name = "lblVenueName";
            this.lblVenueName.Size = new System.Drawing.Size(435, 19);
            this.lblVenueName.Text = "Venue Name";
            // 
            // checkIn1
            // 
            this.checkIn1.Location = new System.Drawing.Point(3, 41);
            this.checkIn1.Name = "checkIn1";
            this.checkIn1.Size = new System.Drawing.Size(266, 150);
            this.checkIn1.TabIndex = 2;
            this.checkIn1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.lblVenueName);
            this.panel1.Controls.Add(this.lblAddress);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(269, 37);
            // 
            // VenueDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.checkIn1);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "VenueDetails";
            this.Size = new System.Drawing.Size(269, 239);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblVenueName;
        private MySquare.UI.Places.Details.CheckIn checkIn1;
        private System.Windows.Forms.Panel panel1;

    }
}
