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
            this.SuspendLayout();
            // 
            // list1
            // 
            this.list1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list1.Location = new System.Drawing.Point(0, 0);
            this.list1.Name = "list1";
            this.list1.Size = new System.Drawing.Size(204, 207);
            this.list1.TabIndex = 0;
            // 
            // Places
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.list1);
            this.Name = "Places";
            this.Size = new System.Drawing.Size(204, 207);
            this.ResumeLayout(false);

        }

        #endregion

        private List list1;
    }
}
