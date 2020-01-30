using EmirateHMBot.Models;
using IronPdf;
using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using TuesPechkin;

namespace EmirateHMBot
{
    public partial class CredentialForm : Form
    {
        string userName = "bilel";
        string passWord = "2305";
        public CredentialForm()
        {
            InitializeComponent();
        }

        private async  void CredentialForm_Load(object sender, EventArgs e)
        {
            //var htmlPath = Path.GetFullPath("y.html");
            //var pdfPath = Path.GetFullPath("151518.pdf");
            //var exePath = Path.GetFullPath("wkhtmltopdf.exe");
            ////MessageBox.Show(htmlPath);
            //await Utility.WritePDF(htmlPath, pdfPath, exePath);
            //Process.Start(pdfPath);

            //var companyCode = 1;
            //var htmlPath = Path.GetFullPath("y.html");
            //var pdfPath = Path.GetFullPath(companyCode + ".pdf");
            //var exePath = Path.GetFullPath("wkhtmltopdf.exe");
            //MessageBox.Show(exePath);
            //await Utility.WritePDF(htmlPath, pdfPath, exePath);
            //Process.Start(pdfPath);
        }

        private void ScrapePermitB_Click(object sender, EventArgs e)
        {
            if (UsernameT.Text == "" || PasswordT.Text == "")
            {
                MessageBox.Show("username and/or password is missed");
                return;
            }
            if (UsernameT.Text != userName || PasswordT.Text != passWord)
            {
                MessageBox.Show("username or passsword is not valide");
                return;
            }
            else
            {
                Hide();
                MainForm mainfrom = new MainForm();
                mainfrom.Show();
            }
        }

        private void CredentialForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
