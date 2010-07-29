﻿namespace MySquare.UI
{
    partial class Help
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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.lblHelpText = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox.Location = new System.Drawing.Point(43, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(151, 67);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Help_MouseMove);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Help_MouseDown);
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // lblHelpText
            // 
            this.lblHelpText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHelpText.ForeColor = System.Drawing.Color.White;
            this.lblHelpText.Location = new System.Drawing.Point(20, 80);
            this.lblHelpText.Name = "lblHelpText";
            this.lblHelpText.Size = new System.Drawing.Size(200, 150);
            this.lblHelpText.Text = "MySquare for Windows Phone\r\n\r\nVersion {0}.\r\nRising Mobility Software\r\n\r\nThi" +
                "s program is distributed as-is without warranty.\t\r\n\r\nPortions based on NewtonSof" +
                "t Json.net\r\n";
            this.lblHelpText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.linkLabel1.Location = new System.Drawing.Point(10, 234);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(112, 20);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.Text = "FourSquare Terms";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.linkLabel1.Click += new System.EventHandler(this.linkLabel1_Click);
            // 
            // linkLabel2
            // 
            this.linkLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.linkLabel2.Location = new System.Drawing.Point(132, 234);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(92, 20);
            this.linkLabel2.TabIndex = 5;
            this.linkLabel2.Text = "MySquare Web";
            this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.linkLabel2.Click += new System.EventHandler(this.linkLabel2_Click);
            // 
            // Help
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.lblHelpText);
            this.Controls.Add(this.pictureBox);
            this.Name = "Help";
            this.Size = new System.Drawing.Size(239, 294);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Help_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Help_MouseDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label lblHelpText;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
    }
}
