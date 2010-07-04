namespace MySquare.UI.Friends
{
    partial class Friends
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
            this.listBox.DefaultItemHeight = 38;
            this.listBox.DefaultItemWidth = 80;
            this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox.Layout = Tenor.Mobile.UI.KListLayout.Vertical;
            this.listBox.Location = new System.Drawing.Point(0, 0);
            this.listBox.Name = "listBox";
            this.listBox.SeparatorColor = System.Drawing.SystemColors.InactiveBorder;
            this.listBox.Size = new System.Drawing.Size(150, 150);
            this.listBox.TabIndex = 1;
            this.listBox.SelectedItemClicked += new System.EventHandler(this.listBox_SelectedItemClicked_1);
            // 
            // Friends
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.listBox);
            this.Name = "Friends";
            this.ResumeLayout(false);

        }

        #endregion

        internal Tenor.Mobile.UI.KListControl listBox;
    }
}
