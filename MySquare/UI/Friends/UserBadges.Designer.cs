namespace MySquare.UI.Friends
{
    partial class UserBadges
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
            this.SuspendLayout();
            // 
            // listBox
            // 
            this.listBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(204)))), ((int)(((byte)(212)))));
            this.listBox.DefaultItemHeight = 38;
            this.listBox.DefaultItemWidth = 80;
            this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox.Layout = Tenor.Mobile.UI.KListLayout.Vertical;
            this.listBox.Location = new System.Drawing.Point(0, 0);
            this.listBox.Name = "listBox";
            this.listBox.SeparatorColor = System.Drawing.SystemColors.InactiveBorder;
            this.listBox.Size = new System.Drawing.Size(209, 185);
            this.listBox.Skinnable = false;
            this.listBox.TabIndex = 6;
            this.listBox.DrawItem += new Tenor.Mobile.UI.DrawItemEventHandler(this.listBox_DrawItem);
            // 
            // UserBadges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(204)))), ((int)(((byte)(212)))));
            this.Controls.Add(this.listBox);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "UserBadges";
            this.Size = new System.Drawing.Size(209, 185);
            this.ResumeLayout(false);

        }

        #endregion

        internal Tenor.Mobile.UI.KListControl listBox;

    }
}
