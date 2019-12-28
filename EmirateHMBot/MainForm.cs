using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Forms;
using Newtonsoft.Json.Linq;
using EmirateHMBot.Models;
using OfficeOpenXml;

namespace EmirateHMBot
{
    public partial class MainForm : MetroForm
    {
        public bool LogToUi = true;
        public bool LogToFile = true;
        ExcelPackage _package;
        ExcelWorksheet _worksheet;
        int _r;
        Random rnd = new Random();
        private readonly string _path = Application.StartupPath;
        private int _nbr;
        private int _total;
        private int _maxConcurrency;
        public HttpCaller HttpCaller = new HttpCaller();
        public MainForm()
        {
            InitializeComponent();
        }

        int delayMin, delayMax;
        string user, pass;

        private async Task MainWork()
        {
            await Task.Delay(3000);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ServicePointManager.DefaultConnectionLimit = 65000;
            Directory.CreateDirectory("data");
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Utility.CreateDb();
            Utility.LoadConfig();
            Utility.InitCntrl(this);

            //startB.Visible = false;
            //FillFormsB.Visible = false;
            //PermitDGV.Visible = false;
            //EID2DGV.Visible = false;
            //MOHAPDGV.Visible = false;
            //codeLb.Visible = false;
            //CodeT.Visible = false;


            PermitDGV.ColumnCount = 2;

            PermitDGV.Columns[0].Width = 250;
            PermitDGV.Columns[1].Width = 250;

            PermitDGV.Rows.Add(20);
            PermitDGV.Rows[0].Cells[0].ReadOnly = true;
            PermitDGV.Rows[1].Cells[0].ReadOnly = true;
            PermitDGV.Rows[2].Cells[0].ReadOnly = true;
            PermitDGV.Rows[3].Cells[0].ReadOnly = true;
            PermitDGV.Rows[4].Cells[0].ReadOnly = true;
            PermitDGV.Rows[5].Cells[0].ReadOnly = true;
            PermitDGV.Rows[6].Cells[0].ReadOnly = true;
            PermitDGV.Rows[7].Cells[0].ReadOnly = true;
            PermitDGV.Rows[8].Cells[0].ReadOnly = true;
            PermitDGV.Rows[9].Cells[0].ReadOnly = true;
            PermitDGV.Rows[10].Cells[0].ReadOnly = true;
            PermitDGV.Rows[11].Cells[0].ReadOnly = true;
            PermitDGV.Rows[12].Cells[0].ReadOnly = true;
            PermitDGV.Rows[13].Cells[0].ReadOnly = true;
            PermitDGV.Rows[14].Cells[0].ReadOnly = true;
            PermitDGV.Rows[15].Cells[0].ReadOnly = true;
            PermitDGV.Rows[16].Cells[0].ReadOnly = true;
            PermitDGV.Rows[17].Cells[0].ReadOnly = true;
            PermitDGV.Rows[18].Cells[0].ReadOnly = true;
            PermitDGV.Rows[19].Cells[0].ReadOnly = true;

            PermitDGV.Rows[0].Cells[0].Value = "Emirates ID Number";
            PermitDGV.Rows[1].Cells[0].Value = "Nationality";
            PermitDGV.Rows[2].Cells[0].Value = "Gender";
            PermitDGV.Rows[3].Cells[0].Value = "Name Arabic 1";
            PermitDGV.Rows[4].Cells[0].Value = "Name English 1";
            PermitDGV.Rows[5].Cells[0].Value = "Mother Name EN";
            PermitDGV.Rows[6].Cells[0].Value = "Mother Name AR";
            PermitDGV.Rows[7].Cells[0].Value = "Place Of Birth Country";
            PermitDGV.Rows[8].Cells[0].Value = "Place Of Birth City";
            PermitDGV.Rows[9].Cells[0].Value = "Date of Birth";
            PermitDGV.Rows[10].Cells[0].Value = "Passport Number";
            PermitDGV.Rows[11].Cells[0].Value = "Passport Issue Date";
            PermitDGV.Rows[12].Cells[0].Value = "Passport Expiry Date";
            PermitDGV.Rows[13].Cells[0].Value = "Unified ID";
            PermitDGV.Rows[14].Cells[0].Value = "Residency Number";
            PermitDGV.Rows[15].Cells[0].Value = "Residence Issue Date";
            PermitDGV.Rows[16].Cells[0].Value = "Residence Expiry Date";
            PermitDGV.Rows[17].Cells[0].Value = "Company Name Arabic";
            PermitDGV.Rows[18].Cells[0].Value = "Mobile Number";
            PermitDGV.Rows[19].Cells[0].Value = "Profession";

            foreach (DataGridViewColumn col in PermitDGV.Columns)
            {
                col.DefaultCellStyle.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point);
            }



            EID2DGV.ColumnCount = 2;

            EID2DGV.Columns[0].Width = 250;
            EID2DGV.Columns[1].Width = 250;

            EID2DGV.Rows.Add(20);

            EID2DGV.Rows[0].Cells[0].ReadOnly = true;
            EID2DGV.Rows[1].Cells[0].ReadOnly = true;
            EID2DGV.Rows[2].Cells[0].ReadOnly = true;
            EID2DGV.Rows[3].Cells[0].ReadOnly = true;
            EID2DGV.Rows[4].Cells[0].ReadOnly = true;
            EID2DGV.Rows[5].Cells[0].ReadOnly = true;
            EID2DGV.Rows[6].Cells[0].ReadOnly = true;
            EID2DGV.Rows[7].Cells[0].ReadOnly = true;
            EID2DGV.Rows[8].Cells[0].ReadOnly = true;
            EID2DGV.Rows[9].Cells[0].ReadOnly = true;
            EID2DGV.Rows[10].Cells[0].ReadOnly = true;
            EID2DGV.Rows[11].Cells[0].ReadOnly = true;
            EID2DGV.Rows[12].Cells[0].ReadOnly = true;
            EID2DGV.Rows[13].Cells[0].ReadOnly = true;
            EID2DGV.Rows[14].Cells[0].ReadOnly = true;
            EID2DGV.Rows[15].Cells[0].ReadOnly = true;
            EID2DGV.Rows[16].Cells[0].ReadOnly = true;
            EID2DGV.Rows[17].Cells[0].ReadOnly = true;
            EID2DGV.Rows[18].Cells[0].ReadOnly = true;
            EID2DGV.Rows[19].Cells[0].ReadOnly = true;

            EID2DGV.Rows[0].Cells[0].Value = "EID Number";
            EID2DGV.Rows[1].Cells[0].Value = "Nationality";
            EID2DGV.Rows[2].Cells[0].Value = "Gender";
            EID2DGV.Rows[3].Cells[0].Value = "Name Arabic";
            EID2DGV.Rows[4].Cells[0].Value = "Mother Name Arabic";
            EID2DGV.Rows[5].Cells[0].Value = "Name English";
            EID2DGV.Rows[6].Cells[0].Value = "Mother Name English";
            EID2DGV.Rows[7].Cells[0].Value = "Place of Birth";
            EID2DGV.Rows[8].Cells[0].Value = "Date of Birth";
            EID2DGV.Rows[9].Cells[0].Value = "Passport Number";
            EID2DGV.Rows[10].Cells[0].Value = "Date of Issue Passport";
            EID2DGV.Rows[11].Cells[0].Value = "Date of Expiry Passport";
            EID2DGV.Rows[12].Cells[0].Value = "UID";
            EID2DGV.Rows[13].Cells[0].Value = "File Number";
            EID2DGV.Rows[14].Cells[0].Value = "Residence Issue Date";
            EID2DGV.Rows[15].Cells[0].Value = "Residence Expiry Date";
            EID2DGV.Rows[16].Cells[0].Value = "Mobile Number";
            EID2DGV.Rows[17].Cells[0].Value = "Abroad Location";
            EID2DGV.Rows[18].Cells[0].Value = "Company Name Arabic";
            EID2DGV.Rows[19].Cells[0].Value = "Profession";

            foreach (DataGridViewColumn col in EID2DGV.Columns)
            {
                col.DefaultCellStyle.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point);
            }


            MOHAPDGV.ColumnCount = 2;

            MOHAPDGV.Columns[0].Width = 250;
            MOHAPDGV.Columns[1].Width = 250;

            MOHAPDGV.Rows.Add(17);

            MOHAPDGV.Rows[0].Cells[0].ReadOnly = true;
            MOHAPDGV.Rows[1].Cells[0].ReadOnly = true;
            MOHAPDGV.Rows[2].Cells[0].ReadOnly = true;
            MOHAPDGV.Rows[3].Cells[0].ReadOnly = true;
            MOHAPDGV.Rows[4].Cells[0].ReadOnly = true;
            MOHAPDGV.Rows[5].Cells[0].ReadOnly = true;
            MOHAPDGV.Rows[6].Cells[0].ReadOnly = true;
            MOHAPDGV.Rows[7].Cells[0].ReadOnly = true;
            MOHAPDGV.Rows[8].Cells[0].ReadOnly = true;
            MOHAPDGV.Rows[9].Cells[0].ReadOnly = true;
            MOHAPDGV.Rows[10].Cells[0].ReadOnly = true;
            MOHAPDGV.Rows[11].Cells[0].ReadOnly = true;
            MOHAPDGV.Rows[12].Cells[0].ReadOnly = true;
            MOHAPDGV.Rows[13].Cells[0].ReadOnly = true;
            MOHAPDGV.Rows[14].Cells[0].ReadOnly = true;
            MOHAPDGV.Rows[15].Cells[0].ReadOnly = true;
            MOHAPDGV.Rows[16].Cells[0].ReadOnly = true;

            MOHAPDGV.Rows[0].Cells[0].Value = "Company Name";
            MOHAPDGV.Rows[1].Cells[0].Value = "Work Phone";
            MOHAPDGV.Rows[2].Cells[0].Value = "Name Arabic";
            MOHAPDGV.Rows[3].Cells[0].Value = "Name English";
            MOHAPDGV.Rows[4].Cells[0].Value = "EID Number";
            MOHAPDGV.Rows[5].Cells[0].Value = "UID";
            MOHAPDGV.Rows[6].Cells[0].Value = "Residency File Number";
            MOHAPDGV.Rows[7].Cells[0].Value = "Residence Issue Date";
            MOHAPDGV.Rows[8].Cells[0].Value = "Residence Expiry Date";
            MOHAPDGV.Rows[9].Cells[0].Value = "Passport Number";
            MOHAPDGV.Rows[10].Cells[0].Value = "Passport Issue Date";
            MOHAPDGV.Rows[11].Cells[0].Value = "Passport Expiry Date";
            MOHAPDGV.Rows[12].Cells[0].Value = "Nationality";
            MOHAPDGV.Rows[13].Cells[0].Value = "Gender";
            MOHAPDGV.Rows[14].Cells[0].Value = "Birth Date";
            MOHAPDGV.Rows[15].Cells[0].Value = "Profession";
            MOHAPDGV.Rows[16].Cells[0].Value = "Mobile Number";

            foreach (DataGridViewColumn col in MOHAPDGV.Columns)
            {
                col.DefaultCellStyle.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point);
            }

            metroTabControl2.SelectedTab = metroTabPage3;
        }
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString(), @"Unhandled Thread Exception");
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show((e.ExceptionObject as Exception)?.ToString(), @"Unhandled UI Exception");
        }
        #region UIFunctions

        public delegate void SetProgressD(int x);
        public void SetProgress(int x)
        {
            if (InvokeRequired)
            {
                Invoke(new SetProgressD(SetProgress), x);
                return;
            }
            if ((x <= 100))
            {
                ProgressB.Value = x;
            }
        }
        public delegate void DisplayD(string s);
        public void Display(string s)
        {
            if (InvokeRequired)
            {
                Invoke(new DisplayD(Display), s);
                return;
            }
            displayT.Text = s;
        }

        #endregion
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Utility.Config = new Dictionary<string, string>();
            Utility.SaveCntrl(this);
            Utility.SaveConfig();
        }

        private async void startB_Click(object sender, EventArgs e)
        {
            //CodePermit.Text = "84920767";
            Display("");
            CleanDataGridViews();
            var datas = new Dictionary<string, string>();
            //if (CodeT.Text == "")
            //{
            //    Display("Please put the code wich you will scrape data with");
            //    return;
            //}
            //try
            //{
            //    int.Parse(CodeT.Text);
            //}
            //catch (Exception)
            //{

            //    Display("the input should be a number");

            //    startB.Enabled = true;
            //    return;
            //}

            var res = await HttpCaller.GetDoc($"http://eservices.mohre.gov.ae/NewMolGateway/english/Services/wpStatusMolMoi.aspx?Code={CodeT.Text}");
            if (res.error != null)
            {
                //ErrorLog(res.error);
                return;
            }
            var validityCode = res.doc.DocumentNode?.SelectSingleNode("//span[@id='lblMsg']").InnerText;
            if (validityCode.Contains("Not available"))
            {
                Display("This code is not available");
                datas = new Dictionary<string, string>();
                return;
            }
            var trs = res.doc.DocumentNode.SelectNodes("//table[@id='tblWp']//table[@id]//tr");
            if (trs == null)
                return;

            foreach (var tr in trs)
            {
                var tds = tr.SelectNodes("./td");
                if (tds.Count < 2)
                    continue;
                var keyValue = new KeyValue();
                int index = 1;
                foreach (var td in tds)
                {
                    if (index % 2 != 0)
                        keyValue.Key = td.InnerText.Trim();
                    else
                    {
                        keyValue.Value = td.InnerText.Trim();
                        datas.Add(keyValue.Key, keyValue.Value);
                        keyValue = new KeyValue();
                    }
                    index++;
                }
            }
            Console.WriteLine(datas["Passport Issue Date"]);
            //return;
            PermitDGV.Rows[1].Cells[1].Value = datas["Current Nationality"];
            PermitDGV.Rows[2].Cells[1].Value = datas["Gender"];
            PermitDGV.Rows[3].Cells[1].Value = datas["Person Name (Arabic)"];
            PermitDGV.Rows[4].Cells[1].Value = datas["Person Name (Eng)"];
            PermitDGV.Rows[5].Cells[1].Value = datas["Mother Name (Eng)"];
            PermitDGV.Rows[6].Cells[1].Value = datas["Mother Name (Arabic)"];
            PermitDGV.Rows[7].Cells[1].Value = datas["Birth Place(Arabic)"];
            PermitDGV.Rows[9].Cells[1].Value = datas["Date of Birth"];
            PermitDGV.Rows[10].Cells[1].Value = datas["Passport Number"];
            PermitDGV.Rows[11].Cells[1].Value = datas["Passport Issue Date"];
            PermitDGV.Rows[12].Cells[1].Value = datas["Passport Expiry Date"];
            PermitDGV.Rows[17].Cells[1].Value = datas["Sponsor Name (Arabic)"];

            datas = new Dictionary<string, string>();
        }

        private void CleanDataGridViews()
        {
            PermitDGV.Rows[0].Cells[1].Value = "";
            PermitDGV.Rows[1].Cells[1].Value = "";
            PermitDGV.Rows[2].Cells[1].Value = "";
            PermitDGV.Rows[3].Cells[1].Value = "";
            PermitDGV.Rows[4].Cells[1].Value = "";
            PermitDGV.Rows[5].Cells[1].Value = "";
            PermitDGV.Rows[6].Cells[1].Value = "";
            PermitDGV.Rows[7].Cells[1].Value = "";
            PermitDGV.Rows[9].Cells[1].Value = "";
            PermitDGV.Rows[10].Cells[1].Value = "";
            PermitDGV.Rows[11].Cells[1].Value = "";
            PermitDGV.Rows[12].Cells[1].Value = "";
            PermitDGV.Rows[13].Cells[1].Value = "";
            PermitDGV.Rows[14].Cells[1].Value = "";
            PermitDGV.Rows[15].Cells[1].Value = "";
            PermitDGV.Rows[16].Cells[1].Value = "";
            PermitDGV.Rows[17].Cells[1].Value = "";
            PermitDGV.Rows[18].Cells[1].Value = "";
            PermitDGV.Rows[19].Cells[1].Value = "";


            EID2DGV.Rows[0].Cells[1].Value = "";
            EID2DGV.Rows[1].Cells[1].Value = "";
            EID2DGV.Rows[2].Cells[1].Value = "";
            EID2DGV.Rows[3].Cells[1].Value = "";
            EID2DGV.Rows[4].Cells[1].Value = "";
            EID2DGV.Rows[5].Cells[1].Value = "";
            EID2DGV.Rows[6].Cells[1].Value = "";
            EID2DGV.Rows[7].Cells[1].Value = "";
            EID2DGV.Rows[8].Cells[1].Value = "";
            EID2DGV.Rows[9].Cells[1].Value = "";
            EID2DGV.Rows[10].Cells[1].Value = "";
            EID2DGV.Rows[11].Cells[1].Value = "";
            EID2DGV.Rows[12].Cells[1].Value = "";
            EID2DGV.Rows[13].Cells[1].Value = "";
            EID2DGV.Rows[14].Cells[1].Value = "";
            EID2DGV.Rows[15].Cells[1].Value = "";
            EID2DGV.Rows[16].Cells[1].Value = "";
            EID2DGV.Rows[18].Cells[1].Value = "";
            EID2DGV.Rows[19].Cells[1].Value = "";

            MOHAPDGV.Rows[0].Cells[1].Value = "";
            MOHAPDGV.Rows[1].Cells[1].Value = "";
            MOHAPDGV.Rows[2].Cells[1].Value = "";
            MOHAPDGV.Rows[3].Cells[1].Value = "";
            MOHAPDGV.Rows[4].Cells[1].Value = "";
            MOHAPDGV.Rows[5].Cells[1].Value = "";
            MOHAPDGV.Rows[6].Cells[1].Value = "";
            MOHAPDGV.Rows[7].Cells[1].Value = "";
            MOHAPDGV.Rows[8].Cells[1].Value = "";
            MOHAPDGV.Rows[9].Cells[1].Value = "";
            MOHAPDGV.Rows[10].Cells[1].Value = "";
            MOHAPDGV.Rows[11].Cells[1].Value = "";
            MOHAPDGV.Rows[12].Cells[1].Value = "";
            MOHAPDGV.Rows[13].Cells[1].Value = "";
            MOHAPDGV.Rows[14].Cells[1].Value = "";
            MOHAPDGV.Rows[15].Cells[1].Value = "";
            MOHAPDGV.Rows[16].Cells[1].Value = "";
        }

        async Task<string> Login()
        {
            Display("logging in to www.merchantwords.com...");
            List<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("email",user),
                new KeyValuePair<string, string>("password",pass),
            };
            var response = await HttpCaller.PostFormData("https://www.merchantwords.com/login", formData);
            if (response.error != null)
            {
                return response.error;
            }
            if (response.html.Contains("Email and passwords don't match"))
            {
                return ("Email and passwords don't match");
            }
            return null;//we are cool
        }

        private void FillFormsB_Click(object sender, EventArgs e)
        {
            if (PermitDGV?.Rows[18]?.Cells[1]?.Value?.ToString()?.Length > 4)
            {
                var firstpartNbr = PermitDGV.Rows[18].Cells[1].Value.ToString().Substring(0, 3);
                var secondepartNbr = PermitDGV.Rows[18].Cells[1].Value.ToString().Substring(3);
                EID2DGV.Rows[16].Cells[1].Value = firstpartNbr + "-" + secondepartNbr;
            }
            else
                EID2DGV.Rows[16].Cells[1].Value = PermitDGV.Rows[18].Cells[1].Value;

            EID2DGV.Rows[0].Cells[1].Value = PermitDGV.Rows[0].Cells[1].Value;
            EID2DGV.Rows[1].Cells[1].Value = PermitDGV.Rows[1].Cells[1].Value;
            EID2DGV.Rows[2].Cells[1].Value = PermitDGV.Rows[2].Cells[1].Value;
            EID2DGV.Rows[3].Cells[1].Value = PermitDGV.Rows[3].Cells[1].Value;
            EID2DGV.Rows[4].Cells[1].Value = PermitDGV.Rows[6].Cells[1].Value;
            EID2DGV.Rows[5].Cells[1].Value = PermitDGV.Rows[4].Cells[1].Value;
            EID2DGV.Rows[6].Cells[1].Value = PermitDGV.Rows[5].Cells[1].Value;
            EID2DGV.Rows[7].Cells[1].Value = PermitDGV.Rows[7].Cells[1].Value;
            EID2DGV.Rows[8].Cells[1].Value = PermitDGV.Rows[9].Cells[1].Value;
            EID2DGV.Rows[9].Cells[1].Value = PermitDGV.Rows[10].Cells[1].Value;
            EID2DGV.Rows[10].Cells[1].Value = PermitDGV.Rows[11].Cells[1].Value;
            EID2DGV.Rows[11].Cells[1].Value = PermitDGV.Rows[12].Cells[1].Value;
            EID2DGV.Rows[12].Cells[1].Value = PermitDGV.Rows[13].Cells[1].Value;
            EID2DGV.Rows[13].Cells[1].Value = PermitDGV.Rows[14].Cells[1].Value;
            EID2DGV.Rows[14].Cells[1].Value = PermitDGV.Rows[15].Cells[1].Value;
            EID2DGV.Rows[15].Cells[1].Value = PermitDGV.Rows[16].Cells[1].Value;
            EID2DGV.Rows[18].Cells[1].Value = PermitDGV.Rows[17].Cells[1].Value;
            EID2DGV.Rows[19].Cells[1].Value = PermitDGV.Rows[19].Cells[1].Value;

            MOHAPDGV.Rows[0].Cells[1].Value = PermitDGV.Rows[17].Cells[1].Value;
            MOHAPDGV.Rows[1].Cells[1].Value = PermitDGV.Rows[18].Cells[1].Value;
            MOHAPDGV.Rows[2].Cells[1].Value = PermitDGV.Rows[3].Cells[1].Value;
            MOHAPDGV.Rows[3].Cells[1].Value = PermitDGV.Rows[4].Cells[1].Value;
            MOHAPDGV.Rows[4].Cells[1].Value = PermitDGV.Rows[0].Cells[1].Value;
            MOHAPDGV.Rows[5].Cells[1].Value = PermitDGV.Rows[13].Cells[1].Value;
            MOHAPDGV.Rows[6].Cells[1].Value = PermitDGV.Rows[14].Cells[1].Value;
            MOHAPDGV.Rows[7].Cells[1].Value = PermitDGV.Rows[15].Cells[1].Value;
            MOHAPDGV.Rows[8].Cells[1].Value = PermitDGV.Rows[16].Cells[1].Value;
            MOHAPDGV.Rows[9].Cells[1].Value = PermitDGV.Rows[10].Cells[1].Value;
            MOHAPDGV.Rows[10].Cells[1].Value = PermitDGV.Rows[11].Cells[1].Value;
            MOHAPDGV.Rows[11].Cells[1].Value = PermitDGV.Rows[12].Cells[1].Value;
            MOHAPDGV.Rows[12].Cells[1].Value = PermitDGV.Rows[1].Cells[1].Value;
            MOHAPDGV.Rows[13].Cells[1].Value = PermitDGV.Rows[2].Cells[1].Value;
            MOHAPDGV.Rows[14].Cells[1].Value = PermitDGV.Rows[9].Cells[1].Value;
            MOHAPDGV.Rows[15].Cells[1].Value = PermitDGV.Rows[18].Cells[1].Value;
            MOHAPDGV.Rows[16].Cells[1].Value = PermitDGV.Rows[19].Cells[1].Value;
        }

        private void startB_Click_1(object sender, EventArgs e)
        {

        }
        public class KeyValue
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

    }
}
