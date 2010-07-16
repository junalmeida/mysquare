namespace MySquare.UI.Places.Details
{
    partial class VenueInfo
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.LinkLabel();
            this.imgMayor = new System.Windows.Forms.PictureBox();
            this.imgCategory = new System.Windows.Forms.PictureBox();
            this.lblStats = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.mnuCopy = new System.Windows.Forms.MenuItem();
            this.mnuDial = new System.Windows.Forms.MenuItem();
            this.lblSpecials = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblMayor = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.Text = "Category:";
            // 
            // lblCategory
            // 
            this.lblCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCategory.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblCategory.ForeColor = System.Drawing.Color.Black;
            this.lblCategory.Location = new System.Drawing.Point(3, 21);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(165, 25);
            this.lblCategory.Text = "Category";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(3, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 15);
            this.label3.Text = "Phone:";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(3, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 15);
            this.label4.Text = "Mayor:";
            // 
            // lblPhone
            // 
            this.lblPhone.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Underline);
            this.lblPhone.Location = new System.Drawing.Point(3, 64);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(211, 17);
            this.lblPhone.TabIndex = 11;
            this.lblPhone.Text = "phone";
            this.lblPhone.Click += new System.EventHandler(this.lblPhone_Click);
            // 
            // imgMayor
            // 
            this.imgMayor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imgMayor.Location = new System.Drawing.Point(176, 84);
            this.imgMayor.Name = "imgMayor";
            this.imgMayor.Size = new System.Drawing.Size(34, 34);
            this.imgMayor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgMayor.Paint += new System.Windows.Forms.PaintEventHandler(this.Img_Paint);
            // 
            // imgCategory
            // 
            this.imgCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imgCategory.Location = new System.Drawing.Point(176, 4);
            this.imgCategory.Name = "imgCategory";
            this.imgCategory.Size = new System.Drawing.Size(34, 34);
            this.imgCategory.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgCategory.Paint += new System.Windows.Forms.PaintEventHandler(this.Img_Paint);
            // 
            // lblStats
            // 
            this.lblStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStats.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblStats.ForeColor = System.Drawing.Color.Black;
            this.lblStats.Location = new System.Drawing.Point(3, 139);
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(165, 15);
            this.lblStats.Text = "-";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(3, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 15);
            this.label5.Text = "Stats:";
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.Add(this.mnuCopy);
            this.contextMenu.MenuItems.Add(this.mnuDial);
            // 
            // mnuCopy
            // 
            this.mnuCopy.Text = "&Copy";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // mnuDial
            // 
            this.mnuDial.Text = "&Dial";
            this.mnuDial.Click += new System.EventHandler(this.mnuDial_Click);
            // 
            // lblSpecials
            // 
            this.lblSpecials.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSpecials.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.lblSpecials.ForeColor = System.Drawing.Color.Black;
            this.lblSpecials.Location = new System.Drawing.Point(3, 175);
            this.lblSpecials.Name = "lblSpecials";
            this.lblSpecials.Size = new System.Drawing.Size(207, 17);
            this.lblSpecials.Text = "-";
            this.lblSpecials.TextChanged += new System.EventHandler(this.lblSpecials_TextChanged);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(3, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 15);
            this.label6.Text = "Specials:";
            // 
            // lblMayor
            // 
            this.lblMayor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMayor.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Underline);
            this.lblMayor.Location = new System.Drawing.Point(3, 101);
            this.lblMayor.Name = "lblMayor";
            this.lblMayor.Size = new System.Drawing.Size(167, 17);
            this.lblMayor.TabIndex = 17;
            this.lblMayor.Text = "user";
            this.lblMayor.Click += new System.EventHandler(this.lblMayor_Click);
            // 
            // VenueInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.Controls.Add(this.lblMayor);
            this.Controls.Add(this.lblSpecials);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblStats);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.imgCategory);
            this.Controls.Add(this.imgMayor);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.label1);
            this.Name = "VenueInfo";
            this.Size = new System.Drawing.Size(214, 247);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VenueInfo_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VenueInfo_MouseDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        internal System.Windows.Forms.LinkLabel lblPhone;
        internal System.Windows.Forms.PictureBox imgMayor;
        internal System.Windows.Forms.PictureBox imgCategory;
        internal System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem mnuCopy;
        private System.Windows.Forms.MenuItem mnuDial;
        internal System.Windows.Forms.Label lblSpecials;
        private System.Windows.Forms.Label label6;
        internal System.Windows.Forms.LinkLabel lblMayor;
    }
}
