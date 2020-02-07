using EmirateHMBot.Models;
using EmirateHMBot.Services;
using IronPdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            var employees = JsonConvert.DeserializeObject<List<Employee>>(File.ReadAllText("employes.txt"));
            var companyInfo = new CompanyInfo()
            {
                CompanyCategory = "D",
                CompanyCode= "352128",
                CompanyName= "الامارات للرخام",
                DateOfImprimate= "00:31:28 02/02/202",
                NbrOfEmployees= "292"
            };
            EservicesMohreService.SaveBrutHtml(employees, companyInfo);
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
