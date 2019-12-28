using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        
        private void CredentialForm_Load(object sender, EventArgs e)
        {
            

        }

        private void ScrapePermitB_Click(object sender, EventArgs e)
        {
            if (UsernameT.Text==""||PasswordT.Text=="")
            {
                MessageBox.Show("username and/or password is missed");
                return;
            }
            if (UsernameT.Text!=userName|| PasswordT.Text!=passWord)
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
