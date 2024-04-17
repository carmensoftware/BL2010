namespace ReportViewer
{
    partial class Main
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbb_BuCode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbl_Username = new System.Windows.Forms.Label();
            this.btn_Login = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_OpenReport = new System.Windows.Forms.Button();
            this.grd_Report = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd_Report)).BeginInit();
            this.SuspendLayout();
            // 
            // cbb_BuCode
            // 
            this.cbb_BuCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_BuCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbb_BuCode.FormattingEnabled = true;
            this.cbb_BuCode.Location = new System.Drawing.Point(149, 43);
            this.cbb_BuCode.Name = "cbb_BuCode";
            this.cbb_BuCode.Size = new System.Drawing.Size(486, 21);
            this.cbb_BuCode.TabIndex = 2;
            this.cbb_BuCode.SelectedIndexChanged += new System.EventHandler(this.cbb_BuCode_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(149, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Business Unit";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(10, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(120, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lbl_Username
            // 
            this.lbl_Username.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_Username.Location = new System.Drawing.Point(0, 0);
            this.lbl_Username.Name = "lbl_Username";
            this.lbl_Username.Size = new System.Drawing.Size(784, 18);
            this.lbl_Username.TabIndex = 1;
            this.lbl_Username.Text = "Username";
            this.lbl_Username.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_Login
            // 
            this.btn_Login.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Login.Location = new System.Drawing.Point(706, 22);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(75, 23);
            this.btn_Login.TabIndex = 0;
            this.btn_Login.Text = "Login";
            this.btn_Login.UseVisualStyleBackColor = true;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.btn_OpenReport);
            this.panel2.Controls.Add(this.cbb_BuCode);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lbl_Username);
            this.panel2.Controls.Add(this.btn_Login);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(784, 120);
            this.panel2.TabIndex = 5;
            // 
            // btn_OpenReport
            // 
            this.btn_OpenReport.Location = new System.Drawing.Point(147, 87);
            this.btn_OpenReport.Name = "btn_OpenReport";
            this.btn_OpenReport.Size = new System.Drawing.Size(75, 23);
            this.btn_OpenReport.TabIndex = 3;
            this.btn_OpenReport.Text = "Open";
            this.btn_OpenReport.UseVisualStyleBackColor = true;
            this.btn_OpenReport.Click += new System.EventHandler(this.btn_OpenReport_Click);
            // 
            // grd_Report
            // 
            this.grd_Report.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grd_Report.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd_Report.Location = new System.Drawing.Point(0, 120);
            this.grd_Report.Name = "grd_Report";
            this.grd_Report.ReadOnly = true;
            this.grd_Report.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grd_Report.Size = new System.Drawing.Size(784, 442);
            this.grd_Report.TabIndex = 4;
            this.grd_Report.DoubleClick += new System.EventHandler(this.grd_Report_DoubleClick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.grd_Report);
            this.Controls.Add(this.panel2);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blueledgers Report Viewer";
            this.Load += new System.EventHandler(this.Main_Load);
            this.Shown += new System.EventHandler(this.Main_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd_Report)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Login;
        private System.Windows.Forms.Label lbl_Username;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView grd_Report;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.ComboBox cbb_BuCode;
        private System.Windows.Forms.Button btn_OpenReport;
    }
}

