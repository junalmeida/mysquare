namespace MySquare.UI.Places.Create
{
    partial class CreateVenue
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label7;
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtCross = new Tenor.Mobile.UI.TextControl();
            this.txtName = new Tenor.Mobile.UI.TextControl();
            this.txtPhone = new Tenor.Mobile.UI.TextControl();
            this.txtAddress = new Tenor.Mobile.UI.TextControl();
            this.txtZip = new Tenor.Mobile.UI.TextControl();
            this.txtState = new Tenor.Mobile.UI.TextControl();
            this.txtCity = new Tenor.Mobile.UI.TextControl();
            this.picMap = new System.Windows.Forms.PictureBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.ForeColor = System.Drawing.Color.White;
            label1.Location = new System.Drawing.Point(0, 10);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(49, 20);
            label1.Text = "Name:";
            // 
            // label2
            // 
            label2.ForeColor = System.Drawing.Color.White;
            label2.Location = new System.Drawing.Point(0, 52);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(59, 20);
            label2.Text = "Address:";
            // 
            // label3
            // 
            label3.ForeColor = System.Drawing.Color.White;
            label3.Location = new System.Drawing.Point(0, 237);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(81, 20);
            label3.Text = "Cross Street:";
            // 
            // label4
            // 
            label4.ForeColor = System.Drawing.Color.White;
            label4.Location = new System.Drawing.Point(0, 279);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(33, 20);
            label4.Text = "City:";
            // 
            // label5
            // 
            label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label5.ForeColor = System.Drawing.Color.White;
            label5.Location = new System.Drawing.Point(138, 279);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(39, 20);
            label5.Text = "State:";
            // 
            // label6
            // 
            label6.ForeColor = System.Drawing.Color.White;
            label6.Location = new System.Drawing.Point(0, 321);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(33, 20);
            label6.Text = "Zip:";
            // 
            // label7
            // 
            label7.ForeColor = System.Drawing.Color.White;
            label7.Location = new System.Drawing.Point(87, 321);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(46, 20);
            label7.Text = "Phone:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.txtCross);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.txtPhone);
            this.panel1.Controls.Add(this.txtAddress);
            this.panel1.Controls.Add(this.txtZip);
            this.panel1.Controls.Add(this.txtState);
            this.panel1.Controls.Add(this.txtCity);
            this.panel1.Controls.Add(label1);
            this.panel1.Controls.Add(this.picMap);
            this.panel1.Controls.Add(label2);
            this.panel1.Controls.Add(label7);
            this.panel1.Controls.Add(label3);
            this.panel1.Controls.Add(label6);
            this.panel1.Controls.Add(label4);
            this.panel1.Controls.Add(label5);
            this.panel1.Location = new System.Drawing.Point(3, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(194, 363);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CreateVenue_MouseMove);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CreateVenue_MouseDown);
            // 
            // txtCross
            // 
            this.txtCross.AcceptsReturn = false;
            this.txtCross.AcceptsTab = false;
            this.txtCross.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCross.HideSelection = true;
            this.txtCross.InputMode = Microsoft.WindowsCE.Forms.InputMode.Default;
            this.txtCross.Location = new System.Drawing.Point(0, 254);
            this.txtCross.MaxLength = 32767;
            this.txtCross.Modified = false;
            this.txtCross.Multiline = false;
            this.txtCross.Name = "txtCross";
            this.txtCross.PasswordChar = '\0';
            this.txtCross.ReadOnly = false;
            this.txtCross.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCross.SelectedText = "";
            this.txtCross.SelectionLength = 0;
            this.txtCross.SelectionStart = 0;
            this.txtCross.Size = new System.Drawing.Size(189, 21);
            this.txtCross.TabIndex = 6;
            this.txtCross.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtCross.WordWrap = true;
            // 
            // txtName
            // 
            this.txtName.AcceptsReturn = false;
            this.txtName.AcceptsTab = false;
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.HideSelection = true;
            this.txtName.InputMode = Microsoft.WindowsCE.Forms.InputMode.Default;
            this.txtName.Location = new System.Drawing.Point(0, 28);
            this.txtName.MaxLength = 32767;
            this.txtName.Modified = false;
            this.txtName.Multiline = false;
            this.txtName.Name = "txtName";
            this.txtName.PasswordChar = '\0';
            this.txtName.ReadOnly = false;
            this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtName.SelectedText = "";
            this.txtName.SelectionLength = 0;
            this.txtName.SelectionStart = 0;
            this.txtName.Size = new System.Drawing.Size(189, 21);
            this.txtName.TabIndex = 1;
            this.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtName.WordWrap = true;
            // 
            // txtPhone
            // 
            this.txtPhone.AcceptsReturn = false;
            this.txtPhone.AcceptsTab = false;
            this.txtPhone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPhone.HideSelection = true;
            this.txtPhone.InputMode = Microsoft.WindowsCE.Forms.InputMode.Default;
            this.txtPhone.Location = new System.Drawing.Point(87, 337);
            this.txtPhone.MaxLength = 32767;
            this.txtPhone.Modified = false;
            this.txtPhone.Multiline = false;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.PasswordChar = '\0';
            this.txtPhone.ReadOnly = false;
            this.txtPhone.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPhone.SelectedText = "";
            this.txtPhone.SelectionLength = 0;
            this.txtPhone.SelectionStart = 0;
            this.txtPhone.Size = new System.Drawing.Size(102, 21);
            this.txtPhone.TabIndex = 18;
            this.txtPhone.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtPhone.WordWrap = true;
            // 
            // txtAddress
            // 
            this.txtAddress.AcceptsReturn = false;
            this.txtAddress.AcceptsTab = false;
            this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress.HideSelection = true;
            this.txtAddress.InputMode = Microsoft.WindowsCE.Forms.InputMode.Default;
            this.txtAddress.Location = new System.Drawing.Point(0, 69);
            this.txtAddress.MaxLength = 32767;
            this.txtAddress.Modified = false;
            this.txtAddress.Multiline = false;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.PasswordChar = '\0';
            this.txtAddress.ReadOnly = false;
            this.txtAddress.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtAddress.SelectedText = "";
            this.txtAddress.SelectionLength = 0;
            this.txtAddress.SelectionStart = 0;
            this.txtAddress.Size = new System.Drawing.Size(189, 21);
            this.txtAddress.TabIndex = 3;
            this.txtAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtAddress.WordWrap = true;
            // 
            // txtZip
            // 
            this.txtZip.AcceptsReturn = false;
            this.txtZip.AcceptsTab = false;
            this.txtZip.HideSelection = true;
            this.txtZip.InputMode = Microsoft.WindowsCE.Forms.InputMode.Default;
            this.txtZip.Location = new System.Drawing.Point(0, 337);
            this.txtZip.MaxLength = 32767;
            this.txtZip.Modified = false;
            this.txtZip.Multiline = false;
            this.txtZip.Name = "txtZip";
            this.txtZip.PasswordChar = '\0';
            this.txtZip.ReadOnly = false;
            this.txtZip.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtZip.SelectedText = "";
            this.txtZip.SelectionLength = 0;
            this.txtZip.SelectionStart = 0;
            this.txtZip.Size = new System.Drawing.Size(80, 21);
            this.txtZip.TabIndex = 15;
            this.txtZip.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtZip.WordWrap = true;
            // 
            // txtState
            // 
            this.txtState.AcceptsReturn = false;
            this.txtState.AcceptsTab = false;
            this.txtState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtState.HideSelection = true;
            this.txtState.InputMode = Microsoft.WindowsCE.Forms.InputMode.Default;
            this.txtState.Location = new System.Drawing.Point(138, 295);
            this.txtState.MaxLength = 32767;
            this.txtState.Modified = false;
            this.txtState.Multiline = false;
            this.txtState.Name = "txtState";
            this.txtState.PasswordChar = '\0';
            this.txtState.ReadOnly = false;
            this.txtState.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtState.SelectedText = "";
            this.txtState.SelectionLength = 0;
            this.txtState.SelectionStart = 0;
            this.txtState.Size = new System.Drawing.Size(51, 21);
            this.txtState.TabIndex = 12;
            this.txtState.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtState.WordWrap = true;
            // 
            // txtCity
            // 
            this.txtCity.AcceptsReturn = false;
            this.txtCity.AcceptsTab = false;
            this.txtCity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCity.HideSelection = true;
            this.txtCity.InputMode = Microsoft.WindowsCE.Forms.InputMode.Default;
            this.txtCity.Location = new System.Drawing.Point(0, 295);
            this.txtCity.MaxLength = 32767;
            this.txtCity.Modified = false;
            this.txtCity.Multiline = false;
            this.txtCity.Name = "txtCity";
            this.txtCity.PasswordChar = '\0';
            this.txtCity.ReadOnly = false;
            this.txtCity.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCity.SelectedText = "";
            this.txtCity.SelectionLength = 0;
            this.txtCity.SelectionStart = 0;
            this.txtCity.Size = new System.Drawing.Size(132, 21);
            this.txtCity.TabIndex = 9;
            this.txtCity.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtCity.WordWrap = true;
            // 
            // picMap
            // 
            this.picMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.picMap.Location = new System.Drawing.Point(0, 96);
            this.picMap.Name = "picMap";
            this.picMap.Size = new System.Drawing.Size(189, 137);
            this.picMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picMap_MouseDown);
            this.picMap.Paint += new System.Windows.Forms.PaintEventHandler(this.picMap_Paint);
            // 
            // CreateVenue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.panel1);
            this.Name = "CreateVenue";
            this.Size = new System.Drawing.Size(202, 383);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CreateVenue_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CreateVenue_MouseDown);
            this.Resize += new System.EventHandler(this.CreateVenue_Resize);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal Tenor.Mobile.UI.TextControl txtName;
        internal Tenor.Mobile.UI.TextControl txtAddress;
        internal Tenor.Mobile.UI.TextControl txtCross;
        internal Tenor.Mobile.UI.TextControl txtCity;
        internal Tenor.Mobile.UI.TextControl txtState;
        internal Tenor.Mobile.UI.TextControl txtZip;
        internal Tenor.Mobile.UI.TextControl txtPhone;
        internal System.Windows.Forms.PictureBox picMap;
        private System.Windows.Forms.Panel panel1;
    }
}
