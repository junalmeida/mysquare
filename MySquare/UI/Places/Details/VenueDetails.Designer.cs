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
            this.panelTabs = new System.Windows.Forms.Panel();
            this.checkIn1 = new MySquare.UI.Places.Details.CheckIn();
            this.tabStrip1 = new MySquare.UI.TabStrip();
            this.venueInfo1 = new MySquare.UI.Places.Details.VenueInfo();
            this.venueMap1 = new MySquare.UI.Places.Details.VenueMap();
            this.venueTips1 = new MySquare.UI.Places.Details.VenueTips();
            this.panelTabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAddress
            // 
            this.lblAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAddress.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblAddress.ForeColor = System.Drawing.Color.White;
            this.lblAddress.Location = new System.Drawing.Point(0, 20);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(269, 13);
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
            this.lblVenueName.Size = new System.Drawing.Size(269, 19);
            this.lblVenueName.Text = "Venue Name";
            // 
            // panelTabs
            // 
            this.panelTabs.BackColor = System.Drawing.Color.Black;
            this.panelTabs.Controls.Add(this.lblVenueName);
            this.panelTabs.Controls.Add(this.lblAddress);
            this.panelTabs.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTabs.Location = new System.Drawing.Point(0, 0);
            this.panelTabs.Name = "panelTabs";
            this.panelTabs.Size = new System.Drawing.Size(269, 37);
            // 
            // checkIn1
            // 
            this.checkIn1.Location = new System.Drawing.Point(32, 141);
            this.checkIn1.Name = "checkIn1";
            this.checkIn1.Size = new System.Drawing.Size(119, 57);
            this.checkIn1.TabIndex = 2;
            this.checkIn1.Visible = false;
            // 
            // tabStrip1
            // 
            this.tabStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabStrip1.Location = new System.Drawing.Point(0, 37);
            this.tabStrip1.Name = "tabStrip1";
            this.tabStrip1.Size = new System.Drawing.Size(269, 25);
            this.tabStrip1.TabIndex = 4;
            this.tabStrip1.Text = "tabStrip1";
            this.tabStrip1.SelectedIndexChanged += new System.EventHandler(this.tabStrip1_SelectedIndexChanged);
            // 
            // venueInfo1
            // 
            this.venueInfo1.Location = new System.Drawing.Point(102, 86);
            this.venueInfo1.Name = "venueInfo1";
            this.venueInfo1.Size = new System.Drawing.Size(150, 150);
            this.venueInfo1.TabIndex = 6;
            this.venueInfo1.Visible = false;
            // 
            // venueMap1
            // 
            this.venueMap1.Location = new System.Drawing.Point(45, 68);
            this.venueMap1.Name = "venueMap1";
            this.venueMap1.Size = new System.Drawing.Size(150, 150);
            this.venueMap1.TabIndex = 8;
            this.venueMap1.Visible = false;
            // 
            // venueTips1
            // 
            this.venueTips1.Location = new System.Drawing.Point(90, 84);
            this.venueTips1.Name = "venueTips1";
            this.venueTips1.Size = new System.Drawing.Size(150, 150);
            this.venueTips1.TabIndex = 9;
            this.venueTips1.Visible = false;
            // 
            // VenueDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.venueTips1);
            this.Controls.Add(this.venueMap1);
            this.Controls.Add(this.venueInfo1);
            this.Controls.Add(this.checkIn1);
            this.Controls.Add(this.tabStrip1);
            this.Controls.Add(this.panelTabs);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "VenueDetails";
            this.Size = new System.Drawing.Size(269, 239);
            this.panelTabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblVenueName;
        private MySquare.UI.Places.Details.CheckIn checkIn1;
        private System.Windows.Forms.Panel panelTabs;
        private TabStrip tabStrip1;
        private MySquare.UI.Places.Details.VenueInfo venueInfo1;
        private MySquare.UI.Places.Details.VenueMap venueMap1;
        private MySquare.UI.Places.Details.VenueTips venueTips1;

    }
}
