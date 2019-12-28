namespace EmirateHMBot
{
    partial class CredentialForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.UsernameT = new System.Windows.Forms.TextBox();
            this.PasswordT = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ScrapePermitB = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(180, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // UsernameT
            // 
            this.UsernameT.Location = new System.Drawing.Point(278, 96);
            this.UsernameT.Name = "UsernameT";
            this.UsernameT.Size = new System.Drawing.Size(155, 20);
            this.UsernameT.TabIndex = 1;
            // 
            // PasswordT
            // 
            this.PasswordT.Location = new System.Drawing.Point(278, 139);
            this.PasswordT.Name = "PasswordT";
            this.PasswordT.Size = new System.Drawing.Size(155, 20);
            this.PasswordT.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(180, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password:";
            // 
            // ScrapePermitB
            // 
            this.ScrapePermitB.Location = new System.Drawing.Point(278, 217);
            this.ScrapePermitB.Name = "ScrapePermitB";
            this.ScrapePermitB.Size = new System.Drawing.Size(155, 38);
            this.ScrapePermitB.Style = MetroFramework.MetroColorStyle.Black;
            this.ScrapePermitB.TabIndex = 25;
            this.ScrapePermitB.Text = "log in";
            this.ScrapePermitB.UseSelectable = true;
            this.ScrapePermitB.UseStyleColors = true;
            this.ScrapePermitB.Click += new System.EventHandler(this.ScrapePermitB_Click);
            // 
            // CredentialForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 295);
            this.Controls.Add(this.ScrapePermitB);
            this.Controls.Add(this.PasswordT);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.UsernameT);
            this.Controls.Add(this.label1);
            this.Name = "CredentialForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log in";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CredentialForm_FormClosing);
            this.Load += new System.EventHandler(this.CredentialForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox UsernameT;
        private System.Windows.Forms.TextBox PasswordT;
        private System.Windows.Forms.Label label2;
        private MetroFramework.Controls.MetroButton ScrapePermitB;
    }
}