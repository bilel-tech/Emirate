namespace EmirateHMBot
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel3 = new System.Windows.Forms.Panel();
            this.ProgressB = new System.Windows.Forms.ProgressBar();
            this.displayT = new System.Windows.Forms.Label();
            this.metroTabControl1 = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.metroTabControl2 = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPage4 = new MetroFramework.Controls.MetroTabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.FillFormsPermitB = new MetroFramework.Controls.MetroButton();
            this.ScrapePermitB = new MetroFramework.Controls.MetroButton();
            this.CodeT = new MetroFramework.Controls.MetroTextBox();
            this.PermitDGV = new System.Windows.Forms.DataGridView();
            this.metroTabPage5 = new MetroFramework.Controls.MetroTabPage();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.EID2DGV = new System.Windows.Forms.DataGridView();
            this.metroTabPage6 = new MetroFramework.Controls.MetroTabPage();
            this.MOHAPDGV = new System.Windows.Forms.DataGridView();
            this.metroTabPage2 = new MetroFramework.Controls.MetroTabPage();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.metroTabPage3 = new MetroFramework.Controls.MetroTabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel3.SuspendLayout();
            this.metroTabControl1.SuspendLayout();
            this.metroTabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.metroTabControl2.SuspendLayout();
            this.metroTabPage4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PermitDGV)).BeginInit();
            this.metroTabPage5.SuspendLayout();
            this.metroPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EID2DGV)).BeginInit();
            this.metroTabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MOHAPDGV)).BeginInit();
            this.metroTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel3.Controls.Add(this.ProgressB);
            this.panel3.Controls.Add(this.displayT);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(20, 676);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1033, 57);
            this.panel3.TabIndex = 15;
            // 
            // ProgressB
            // 
            this.ProgressB.Location = new System.Drawing.Point(4, 35);
            this.ProgressB.Name = "ProgressB";
            this.ProgressB.Size = new System.Drawing.Size(933, 14);
            this.ProgressB.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.ProgressB.TabIndex = 4;
            // 
            // displayT
            // 
            this.displayT.AutoSize = true;
            this.displayT.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayT.ForeColor = System.Drawing.Color.Black;
            this.displayT.Location = new System.Drawing.Point(22, 10);
            this.displayT.Name = "displayT";
            this.displayT.Size = new System.Drawing.Size(91, 16);
            this.displayT.TabIndex = 2;
            this.displayT.Text = "Bot Started";
            // 
            // metroTabControl1
            // 
            this.metroTabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.metroTabControl1.Controls.Add(this.metroTabPage1);
            this.metroTabControl1.Controls.Add(this.metroTabPage2);
            this.metroTabControl1.Controls.Add(this.metroTabPage3);
            this.metroTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabControl1.Location = new System.Drawing.Point(20, 60);
            this.metroTabControl1.Name = "metroTabControl1";
            this.metroTabControl1.SelectedIndex = 0;
            this.metroTabControl1.Size = new System.Drawing.Size(1033, 616);
            this.metroTabControl1.Style = MetroFramework.MetroColorStyle.Orange;
            this.metroTabControl1.TabIndex = 16;
            this.metroTabControl1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTabControl1.UseSelectable = true;
            this.metroTabControl1.UseStyleColors = true;
            // 
            // metroTabPage1
            // 
            this.metroTabPage1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.metroTabPage1.Controls.Add(this.panel2);
            this.metroTabPage1.ForeColor = System.Drawing.Color.Black;
            this.metroTabPage1.HorizontalScrollbarBarColor = true;
            this.metroTabPage1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.HorizontalScrollbarSize = 0;
            this.metroTabPage1.Location = new System.Drawing.Point(4, 41);
            this.metroTabPage1.Name = "metroTabPage1";
            this.metroTabPage1.Size = new System.Drawing.Size(1025, 571);
            this.metroTabPage1.TabIndex = 0;
            this.metroTabPage1.Text = "Permit";
            this.metroTabPage1.VerticalScrollbarBarColor = true;
            this.metroTabPage1.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.VerticalScrollbarSize = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel2.Controls.Add(this.metroTabControl2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1025, 571);
            this.panel2.TabIndex = 14;
            // 
            // metroTabControl2
            // 
            this.metroTabControl2.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.metroTabControl2.Controls.Add(this.metroTabPage4);
            this.metroTabControl2.Controls.Add(this.metroTabPage5);
            this.metroTabControl2.Controls.Add(this.metroTabPage6);
            this.metroTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabControl2.Location = new System.Drawing.Point(0, 0);
            this.metroTabControl2.Name = "metroTabControl2";
            this.metroTabControl2.SelectedIndex = 2;
            this.metroTabControl2.Size = new System.Drawing.Size(1025, 571);
            this.metroTabControl2.Style = MetroFramework.MetroColorStyle.Orange;
            this.metroTabControl2.TabIndex = 17;
            this.metroTabControl2.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTabControl2.UseSelectable = true;
            this.metroTabControl2.UseStyleColors = true;
            // 
            // metroTabPage4
            // 
            this.metroTabPage4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.metroTabPage4.Controls.Add(this.panel1);
            this.metroTabPage4.ForeColor = System.Drawing.Color.Black;
            this.metroTabPage4.HorizontalScrollbarBarColor = true;
            this.metroTabPage4.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage4.HorizontalScrollbarSize = 0;
            this.metroTabPage4.Location = new System.Drawing.Point(4, 41);
            this.metroTabPage4.Name = "metroTabPage4";
            this.metroTabPage4.Size = new System.Drawing.Size(1017, 526);
            this.metroTabPage4.TabIndex = 0;
            this.metroTabPage4.Text = "Scraped data";
            this.metroTabPage4.VerticalScrollbarBarColor = true;
            this.metroTabPage4.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage4.VerticalScrollbarSize = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.FillFormsPermitB);
            this.panel1.Controls.Add(this.ScrapePermitB);
            this.panel1.Controls.Add(this.CodeT);
            this.panel1.Controls.Add(this.PermitDGV);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1017, 526);
            this.panel1.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(635, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 18);
            this.label1.TabIndex = 26;
            this.label1.Text = "Code:";
            // 
            // FillFormsPermitB
            // 
            this.FillFormsPermitB.Location = new System.Drawing.Point(696, 405);
            this.FillFormsPermitB.Name = "FillFormsPermitB";
            this.FillFormsPermitB.Size = new System.Drawing.Size(122, 63);
            this.FillFormsPermitB.Style = MetroFramework.MetroColorStyle.Orange;
            this.FillFormsPermitB.TabIndex = 25;
            this.FillFormsPermitB.Text = "Fill forms";
            this.FillFormsPermitB.UseSelectable = true;
            this.FillFormsPermitB.UseStyleColors = true;
            this.FillFormsPermitB.Click += new System.EventHandler(this.FillFormsPermitB_Click);
            // 
            // ScrapePermitB
            // 
            this.ScrapePermitB.Location = new System.Drawing.Point(849, 405);
            this.ScrapePermitB.Name = "ScrapePermitB";
            this.ScrapePermitB.Size = new System.Drawing.Size(122, 63);
            this.ScrapePermitB.Style = MetroFramework.MetroColorStyle.Orange;
            this.ScrapePermitB.TabIndex = 24;
            this.ScrapePermitB.Text = "Scrape";
            this.ScrapePermitB.UseSelectable = true;
            this.ScrapePermitB.UseStyleColors = true;
            this.ScrapePermitB.Click += new System.EventHandler(this.ScrapePermitB_ClickAsync);
            // 
            // CodeT
            // 
            // 
            // 
            // 
            this.CodeT.CustomButton.Image = null;
            this.CodeT.CustomButton.Location = new System.Drawing.Point(177, 1);
            this.CodeT.CustomButton.Name = "";
            this.CodeT.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.CodeT.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.CodeT.CustomButton.TabIndex = 1;
            this.CodeT.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.CodeT.CustomButton.UseSelectable = true;
            this.CodeT.CustomButton.Visible = false;
            this.CodeT.Lines = new string[0];
            this.CodeT.Location = new System.Drawing.Point(708, 83);
            this.CodeT.MaxLength = 32767;
            this.CodeT.Name = "CodeT";
            this.CodeT.PasswordChar = '\0';
            this.CodeT.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.CodeT.SelectedText = "";
            this.CodeT.SelectionLength = 0;
            this.CodeT.SelectionStart = 0;
            this.CodeT.ShortcutsEnabled = true;
            this.CodeT.Size = new System.Drawing.Size(199, 23);
            this.CodeT.TabIndex = 8;
            this.CodeT.UseSelectable = true;
            this.CodeT.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.CodeT.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // PermitDGV
            // 
            this.PermitDGV.AllowUserToAddRows = false;
            this.PermitDGV.AllowUserToDeleteRows = false;
            this.PermitDGV.AllowUserToResizeColumns = false;
            this.PermitDGV.AllowUserToResizeRows = false;
            this.PermitDGV.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.PermitDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PermitDGV.Location = new System.Drawing.Point(17, 15);
            this.PermitDGV.Name = "PermitDGV";
            this.PermitDGV.RowHeadersVisible = false;
            this.PermitDGV.Size = new System.Drawing.Size(510, 490);
            this.PermitDGV.TabIndex = 0;
            this.PermitDGV.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.PermitDGV_CellEndEditAsync);
            this.PermitDGV.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PermitDGV_KeyPress);
            this.PermitDGV.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PermitDGV_KeyUp);
            // 
            // metroTabPage5
            // 
            this.metroTabPage5.Controls.Add(this.metroPanel1);
            this.metroTabPage5.HorizontalScrollbarBarColor = false;
            this.metroTabPage5.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage5.HorizontalScrollbarSize = 0;
            this.metroTabPage5.Location = new System.Drawing.Point(4, 41);
            this.metroTabPage5.Name = "metroTabPage5";
            this.metroTabPage5.Size = new System.Drawing.Size(1017, 526);
            this.metroTabPage5.TabIndex = 1;
            this.metroTabPage5.Text = "EID 2";
            this.metroTabPage5.VerticalScrollbarBarColor = false;
            this.metroTabPage5.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage5.VerticalScrollbarSize = 0;
            // 
            // metroPanel1
            // 
            this.metroPanel1.Controls.Add(this.EID2DGV);
            this.metroPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(0, 0);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(1017, 526);
            this.metroPanel1.TabIndex = 2;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // EID2DGV
            // 
            this.EID2DGV.AllowUserToAddRows = false;
            this.EID2DGV.AllowUserToDeleteRows = false;
            this.EID2DGV.AllowUserToResizeColumns = false;
            this.EID2DGV.AllowUserToResizeRows = false;
            this.EID2DGV.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.EID2DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EID2DGV.Location = new System.Drawing.Point(32, 15);
            this.EID2DGV.Name = "EID2DGV";
            this.EID2DGV.RowHeadersVisible = false;
            this.EID2DGV.Size = new System.Drawing.Size(510, 490);
            this.EID2DGV.TabIndex = 2;
            this.EID2DGV.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.EID2DGV_CellEndEdit);
            this.EID2DGV.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EID2DGV_KeyPress);
            // 
            // metroTabPage6
            // 
            this.metroTabPage6.Controls.Add(this.MOHAPDGV);
            this.metroTabPage6.HorizontalScrollbarBarColor = true;
            this.metroTabPage6.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage6.HorizontalScrollbarSize = 10;
            this.metroTabPage6.Location = new System.Drawing.Point(4, 41);
            this.metroTabPage6.Name = "metroTabPage6";
            this.metroTabPage6.Size = new System.Drawing.Size(1017, 526);
            this.metroTabPage6.TabIndex = 2;
            this.metroTabPage6.Text = "MOHRE";
            this.metroTabPage6.VerticalScrollbarBarColor = true;
            this.metroTabPage6.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage6.VerticalScrollbarSize = 10;
            // 
            // MOHAPDGV
            // 
            this.MOHAPDGV.AllowUserToAddRows = false;
            this.MOHAPDGV.AllowUserToDeleteRows = false;
            this.MOHAPDGV.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.MOHAPDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MOHAPDGV.Location = new System.Drawing.Point(17, 21);
            this.MOHAPDGV.Name = "MOHAPDGV";
            this.MOHAPDGV.RowHeadersVisible = false;
            this.MOHAPDGV.Size = new System.Drawing.Size(510, 490);
            this.MOHAPDGV.TabIndex = 3;
            this.MOHAPDGV.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.MOHAPDGV_CellEndEditAsync);
            this.MOHAPDGV.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MOHAPDGV_KeyPress);
            // 
            // metroTabPage2
            // 
            this.metroTabPage2.Controls.Add(this.metroPanel2);
            this.metroTabPage2.HorizontalScrollbarBarColor = false;
            this.metroTabPage2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.HorizontalScrollbarSize = 0;
            this.metroTabPage2.Location = new System.Drawing.Point(4, 41);
            this.metroTabPage2.Name = "metroTabPage2";
            this.metroTabPage2.Size = new System.Drawing.Size(1025, 571);
            this.metroTabPage2.TabIndex = 1;
            this.metroTabPage2.Text = "MOHRE";
            this.metroTabPage2.VerticalScrollbarBarColor = false;
            this.metroTabPage2.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.VerticalScrollbarSize = 0;
            // 
            // metroPanel2
            // 
            this.metroPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(0, 0);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(1025, 571);
            this.metroPanel2.TabIndex = 2;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            // 
            // metroTabPage3
            // 
            this.metroTabPage3.HorizontalScrollbarBarColor = true;
            this.metroTabPage3.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage3.HorizontalScrollbarSize = 10;
            this.metroTabPage3.Location = new System.Drawing.Point(4, 41);
            this.metroTabPage3.Name = "metroTabPage3";
            this.metroTabPage3.Size = new System.Drawing.Size(1025, 571);
            this.metroTabPage3.TabIndex = 2;
            this.metroTabPage3.Text = "EChannels";
            this.metroTabPage3.VerticalScrollbarBarColor = true;
            this.metroTabPage3.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage3.VerticalScrollbarSize = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BackgroundImage = global::EmirateHMBot.Properties.Resources.clipart196740;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(20, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 42);
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 753);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.metroTabControl1);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Style = MetroFramework.MetroColorStyle.Orange;
            this.Text = "         merchantwords.com 1.00";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.metroTabControl1.ResumeLayout(false);
            this.metroTabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.metroTabControl2.ResumeLayout(false);
            this.metroTabPage4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PermitDGV)).EndInit();
            this.metroTabPage5.ResumeLayout(false);
            this.metroPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.EID2DGV)).EndInit();
            this.metroTabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MOHAPDGV)).EndInit();
            this.metroTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ProgressBar ProgressB;
        private System.Windows.Forms.Label displayT;
        private MetroFramework.Controls.MetroTabControl metroTabControl1;
        private MetroFramework.Controls.MetroTabPage metroTabPage1;
        private System.Windows.Forms.Panel panel2;
        private MetroFramework.Controls.MetroTabPage metroTabPage2;
        private MetroFramework.Controls.MetroPanel metroPanel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private MetroFramework.Controls.MetroTabControl metroTabControl2;
        private MetroFramework.Controls.MetroTabPage metroTabPage4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView PermitDGV;
        private MetroFramework.Controls.MetroTabPage metroTabPage5;
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroTabPage metroTabPage6;
        private MetroFramework.Controls.MetroTabPage metroTabPage3;
        private System.Windows.Forms.DataGridView EID2DGV;
        private System.Windows.Forms.DataGridView MOHAPDGV;
        private MetroFramework.Controls.MetroTextBox CodeT;
        private MetroFramework.Controls.MetroButton FillFormsPermitB;
        private MetroFramework.Controls.MetroButton ScrapePermitB;
        private System.Windows.Forms.Label label1;
    }
}

