namespace MySquare.UI.Places
{
    partial class Places
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
            this.list1 = new MySquare.UI.Places.List();
            this.venueDetails1 = new MySquare.UI.Places.VenueDetails();
            this.SuspendLayout();
            // 
            // list1
            // 
            this.list1.Location = new System.Drawing.Point(0, 0);
            this.list1.Name = "list1";
            this.list1.Size = new System.Drawing.Size(135, 137);
            this.list1.TabIndex = 0;
            this.list1.Visible = false;
            this.list1.ItemSelected += new System.EventHandler(this.list1_ItemSelected);
            // 
            // venueDetails1
            // 
            this.venueDetails1.Location = new System.Drawing.Point(41, 54);
            this.venueDetails1.Name = "venueDetails1";
            this.venueDetails1.Size = new System.Drawing.Size(150, 150);
            this.venueDetails1.TabIndex = 1;
            this.venueDetails1.Visible = false;
            // 
            // Places
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.venueDetails1);
            this.Controls.Add(this.list1);
            this.Name = "Places";
            this.Size = new System.Drawing.Size(204, 207);
            this.ResumeLayout(false);

        }

        #endregion

        internal List list1;
        internal VenueDetails venueDetails1;

    }
}
