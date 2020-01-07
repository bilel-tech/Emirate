using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using Newtonsoft.Json.Linq;
using EmirateHMBot.Models;
using OfficeOpenXml;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;

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
        public ChromeDriver Driver;
        public bool LoggedInToMohre = false;
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

        public bool SelectNextCell(DataGridView x)
        {
            int row = x.CurrentCell.RowIndex;
            int column = x.CurrentCell.ColumnIndex;
            DataGridViewCell startingCell = x.CurrentCell;

            do
            {
                column++;
                if (column == x.Columns.Count)
                {
                    column = 0;
                    row++;
                }
                if (row == x.Rows.Count)
                    row = 0;
            } while (x.Rows[row].Cells[column].ReadOnly == true && x.Rows[row].Cells[column] != startingCell);

            if (x.Rows[row].Cells[column] == startingCell)
                return false;
            x.CurrentCell = x.Rows[row].Cells[column];
            return true;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            ServicePointManager.DefaultConnectionLimit = 65000;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Directory.CreateDirectory("data");
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Utility.CreateDb();
            Utility.LoadConfig();
            Utility.InitCntrl(this);
            //allow other threads to modify UI as long as its one thread only
            CheckForIllegalCrossThreadCalls = false;
            //start the navigator on a separate task to gain some time
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
                process.Kill();
            }
            foreach (var process in Process.GetProcessesByName("chrome"))
            {
                process.CloseMainWindow();
            }
            _ = Task.Run(LoginToMohre);

            PermitDGV.ColumnCount = 2;

            PermitDGV.Columns[0].Width = 250;
            PermitDGV.Columns[1].Width = 634;

            PermitDGV.RowTemplate.Height = 25;

            PermitDGV.Rows.Add(20);

            for (int i = 0; i < PermitDGV.Rows.Count; i++)
            {
                PermitDGV.Rows[i].Cells[0].ReadOnly = true;
            }

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

            foreach (DataGridViewColumn column in PermitDGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            EID2DGV.ColumnCount = 2;
            EID2DGV.Columns[0].Width = 250;
            EID2DGV.Columns[1].Width = 649;
            EID2DGV.RowTemplate.Height = 25;

            EID2DGV.Rows.Add(20);

            for (int i = 0; i < EID2DGV.Rows.Count; i++)
            {
                PermitDGV.Rows[i].Cells[0].ReadOnly = true;
            }

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
            foreach (DataGridViewColumn column in EID2DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            PermitMOHAPDGV.ColumnCount = 2;
            PermitMOHAPDGV.Columns[0].Width = 250;
            PermitMOHAPDGV.Columns[1].Width = 635;
            PermitMOHAPDGV.RowTemplate.Height = 28;
            PermitMOHAPDGV.Rows.Add(18);
            for (int i = 0; i < PermitMOHAPDGV.Rows.Count; i++)
            {
                PermitMOHAPDGV.Rows[i].Cells[0].ReadOnly = true;
            }

            PermitMOHAPDGV.Rows[0].Cells[0].Value = "Company Name";
            PermitMOHAPDGV.Rows[1].Cells[0].Value = "Work Phone";
            PermitMOHAPDGV.Rows[2].Cells[0].Value = "Name Arabic";
            PermitMOHAPDGV.Rows[3].Cells[0].Value = "Name English";
            PermitMOHAPDGV.Rows[4].Cells[0].Value = "EID Number";
            PermitMOHAPDGV.Rows[5].Cells[0].Value = "UID";
            PermitMOHAPDGV.Rows[6].Cells[0].Value = "Residency File Number";
            PermitMOHAPDGV.Rows[7].Cells[0].Value = "Residence Issue Date";
            PermitMOHAPDGV.Rows[8].Cells[0].Value = "Residence Expiry Date";
            PermitMOHAPDGV.Rows[9].Cells[0].Value = "Passport Number";
            PermitMOHAPDGV.Rows[10].Cells[0].Value = "Passport Issue Place";
            PermitMOHAPDGV.Rows[11].Cells[0].Value = "Passport Issue Date";
            PermitMOHAPDGV.Rows[12].Cells[0].Value = "Passport Expiry Date";
            PermitMOHAPDGV.Rows[13].Cells[0].Value = "Nationality";
            PermitMOHAPDGV.Rows[14].Cells[0].Value = "Gender";
            PermitMOHAPDGV.Rows[15].Cells[0].Value = "Birth Date";
            PermitMOHAPDGV.Rows[16].Cells[0].Value = "Profession";
            PermitMOHAPDGV.Rows[17].Cells[0].Value = "Mobile Number";

            foreach (DataGridViewColumn col in PermitMOHAPDGV.Columns)
            {
                col.DefaultCellStyle.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point);
            }
            foreach (DataGridViewColumn column in PermitMOHAPDGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //EChannel DGVS
            EChannelDGV.ColumnCount = 2;
            EChannelDGV.Columns[0].Width = 250;
            EChannelDGV.Columns[1].Width = 525;

            EChannelDGV.RowTemplate.Height = 25;

            EChannelDGV.Rows.Add(20);

            for (int i = 0; i < EChannelDGV.Rows.Count; i++)
            {
                EChannelDGV.Rows[i].Cells[0].ReadOnly = true;
            }

            EChannelDGV.Rows[0].Cells[0].Value = "Emirates ID Number";
            EChannelDGV.Rows[1].Cells[0].Value = "Nationality";
            EChannelDGV.Rows[2].Cells[0].Value = "Gender";
            EChannelDGV.Rows[3].Cells[0].Value = "Name Arabic 1";
            EChannelDGV.Rows[4].Cells[0].Value = "Name English 1";
            EChannelDGV.Rows[5].Cells[0].Value = "Mother Name EN";
            EChannelDGV.Rows[6].Cells[0].Value = "Mother Name AR";
            EChannelDGV.Rows[7].Cells[0].Value = "Place Of Birth Country";
            EChannelDGV.Rows[8].Cells[0].Value = "Place Of Birth City";
            EChannelDGV.Rows[9].Cells[0].Value = "Date of Birth";
            EChannelDGV.Rows[10].Cells[0].Value = "Passport Number";
            EChannelDGV.Rows[11].Cells[0].Value = "Passport Issue Date";
            EChannelDGV.Rows[12].Cells[0].Value = "Passport Expiry Date";
            EChannelDGV.Rows[13].Cells[0].Value = "Unified ID";
            EChannelDGV.Rows[14].Cells[0].Value = "Residency Number";
            EChannelDGV.Rows[15].Cells[0].Value = "Residence Issue Date";
            EChannelDGV.Rows[16].Cells[0].Value = "Residence Expiry Date";
            EChannelDGV.Rows[17].Cells[0].Value = "Company Name Arabic";
            EChannelDGV.Rows[18].Cells[0].Value = "Mobile Number";
            EChannelDGV.Rows[19].Cells[0].Value = "Profession";
            foreach (DataGridViewColumn col in EChannelDGV.Columns)
            {
                col.DefaultCellStyle.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point);
            }
            foreach (DataGridViewColumn column in EChannelDGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            EChnEIDDgview.ColumnCount = 2;

            EChnEIDDgview.Columns[0].Width = 250;
            EChnEIDDgview.Columns[1].Width = 653;
            EChnEIDDgview.RowTemplate.Height = 25;

            EChnEIDDgview.Rows.Add(20);

            for (int i = 0; i < EChnEIDDgview.Rows.Count; i++)
            {
                PermitDGV.Rows[i].Cells[0].ReadOnly = true;
            }

            EChnEIDDgview.Rows[0].Cells[0].Value = "EID Number";
            EChnEIDDgview.Rows[1].Cells[0].Value = "Nationality";
            EChnEIDDgview.Rows[2].Cells[0].Value = "Gender";
            EChnEIDDgview.Rows[3].Cells[0].Value = "Name Arabic";
            EChnEIDDgview.Rows[4].Cells[0].Value = "Mother Name Arabic";
            EChnEIDDgview.Rows[5].Cells[0].Value = "Name English";
            EChnEIDDgview.Rows[6].Cells[0].Value = "Mother Name English";
            EChnEIDDgview.Rows[7].Cells[0].Value = "Place of Birth";
            EChnEIDDgview.Rows[8].Cells[0].Value = "Date of Birth";
            EChnEIDDgview.Rows[9].Cells[0].Value = "Passport Number";
            EChnEIDDgview.Rows[10].Cells[0].Value = "Date of Issue Passport";
            EChnEIDDgview.Rows[11].Cells[0].Value = "Date of Expiry Passport";
            EChnEIDDgview.Rows[12].Cells[0].Value = "UID";
            EChnEIDDgview.Rows[13].Cells[0].Value = "File Number";
            EChnEIDDgview.Rows[14].Cells[0].Value = "Residence Issue Date";
            EChnEIDDgview.Rows[15].Cells[0].Value = "Residence Expiry Date";
            EChnEIDDgview.Rows[16].Cells[0].Value = "Mobile Number";
            EChnEIDDgview.Rows[17].Cells[0].Value = "Abroad Location";
            EChnEIDDgview.Rows[18].Cells[0].Value = "Company Name Arabic";
            EChnEIDDgview.Rows[19].Cells[0].Value = "Profession";

            foreach (DataGridViewColumn col in EChnEIDDgview.Columns)
            {
                col.DefaultCellStyle.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point);
            }
            foreach (DataGridViewColumn column in EChnEIDDgview.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            EchanMohreDgview.ColumnCount = 2;

            EchanMohreDgview.Columns[0].Width = 250;
            EchanMohreDgview.Columns[1].Width = 639;
            EchanMohreDgview.RowTemplate.Height = 28;

            EchanMohreDgview.Rows.Add(18);

            for (int i = 0; i < EchanMohreDgview.Rows.Count; i++)
            {
                EchanMohreDgview.Rows[i].Cells[0].ReadOnly = true;
            }

            EchanMohreDgview.Rows[0].Cells[0].Value = "Company Name";
            EchanMohreDgview.Rows[1].Cells[0].Value = "Work Phone";
            EchanMohreDgview.Rows[2].Cells[0].Value = "Name Arabic";
            EchanMohreDgview.Rows[3].Cells[0].Value = "Name English";
            EchanMohreDgview.Rows[4].Cells[0].Value = "EID Number";
            EchanMohreDgview.Rows[5].Cells[0].Value = "UID";
            EchanMohreDgview.Rows[6].Cells[0].Value = "Residency File Number";
            EchanMohreDgview.Rows[7].Cells[0].Value = "Residence Issue Date";
            EchanMohreDgview.Rows[8].Cells[0].Value = "Residence Expiry Date";
            EchanMohreDgview.Rows[9].Cells[0].Value = "Passport Number";
            EchanMohreDgview.Rows[10].Cells[0].Value = "Passport Issue Place";
            EchanMohreDgview.Rows[11].Cells[0].Value = "Passport Issue Date";
            EchanMohreDgview.Rows[12].Cells[0].Value = "Passport Expiry Date";
            EchanMohreDgview.Rows[13].Cells[0].Value = "Nationality";
            EchanMohreDgview.Rows[14].Cells[0].Value = "Gender";
            EchanMohreDgview.Rows[15].Cells[0].Value = "Birth Date";
            EchanMohreDgview.Rows[16].Cells[0].Value = "Profession";
            EchanMohreDgview.Rows[17].Cells[0].Value = "Mobile Number";

            foreach (DataGridViewColumn col in EchanMohreDgview.Columns)
            {
                col.DefaultCellStyle.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point);
            }
            foreach (DataGridViewColumn column in EchanMohreDgview.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            MohreDGV.ColumnCount = 2;

            MohreDGV.Columns[0].Width = 250;
            MohreDGV.Columns[1].Width = 583;

            MohreDGV.RowTemplate.Height = 25;

            MohreDGV.Rows.Add(20);

            for (int i = 0; i < MohreDGV.Rows.Count; i++)
            {
                MohreDGV.Rows[i].Cells[0].ReadOnly = true;
            }

            MohreDGV.Rows[0].Cells[0].Value = "Emirates ID Number";
            MohreDGV.Rows[1].Cells[0].Value = "Nationality";
            MohreDGV.Rows[2].Cells[0].Value = "Gender";
            MohreDGV.Rows[3].Cells[0].Value = "Name Arabic 1";
            MohreDGV.Rows[4].Cells[0].Value = "Name English 1";
            MohreDGV.Rows[5].Cells[0].Value = "Mother Name EN";
            MohreDGV.Rows[6].Cells[0].Value = "Mother Name AR";
            MohreDGV.Rows[7].Cells[0].Value = "Place Of Birth (AR)";
            MohreDGV.Rows[8].Cells[0].Value = "Place Of Birth (EN)";
            MohreDGV.Rows[9].Cells[0].Value = "Date of Birth";
            MohreDGV.Rows[10].Cells[0].Value = "Passport Number";
            MohreDGV.Rows[11].Cells[0].Value = "Passport Issue Date";
            MohreDGV.Rows[12].Cells[0].Value = "Passport Expiry Date";
            MohreDGV.Rows[13].Cells[0].Value = "Unified ID";
            MohreDGV.Rows[14].Cells[0].Value = "Residency Number";
            MohreDGV.Rows[15].Cells[0].Value = "Residence Issue Date";
            MohreDGV.Rows[16].Cells[0].Value = "Residence Expiry Date";
            MohreDGV.Rows[17].Cells[0].Value = "Company Name Arabic";
            MohreDGV.Rows[18].Cells[0].Value = "Mobile Number";
            MohreDGV.Rows[19].Cells[0].Value = "Profession";
            foreach (DataGridViewColumn col in MohreDGV.Columns)
            {
                col.DefaultCellStyle.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point);
            }

            foreach (DataGridViewColumn column in MohreDGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            MohreEidDGV.ColumnCount = 2;
            MohreEidDGV.Columns[0].Width = 250;
            MohreEidDGV.Columns[1].Width = 647;
            MohreEidDGV.RowTemplate.Height = 24;
            MohreEidDGV.Rows.Add(21);
            for (int i = 0; i < MohreEidDGV.Rows.Count; i++)
            {
                MohreEidDGV.Rows[i].Cells[0].ReadOnly = true;
            }

            MohreEidDGV.Rows[0].Cells[0].Value = "EID Number";
            MohreEidDGV.Rows[1].Cells[0].Value = "Nationality";
            MohreEidDGV.Rows[2].Cells[0].Value = "Gender";
            MohreEidDGV.Rows[3].Cells[0].Value = "Name Arabic";
            MohreEidDGV.Rows[4].Cells[0].Value = "Mother Name Arabic";
            MohreEidDGV.Rows[5].Cells[0].Value = "Name English";
            MohreEidDGV.Rows[6].Cells[0].Value = "Mother Name English";
            MohreEidDGV.Rows[7].Cells[0].Value = "Place of Birth (AR)";
            MohreEidDGV.Rows[8].Cells[0].Value = "Place of Birth (EN)";
            MohreEidDGV.Rows[9].Cells[0].Value = "Date of Birth";
            MohreEidDGV.Rows[10].Cells[0].Value = "Passport Number";
            MohreEidDGV.Rows[11].Cells[0].Value = "Date of Issue Passport";
            MohreEidDGV.Rows[12].Cells[0].Value = "Date of Expiry Passport";
            MohreEidDGV.Rows[13].Cells[0].Value = "UID";
            MohreEidDGV.Rows[14].Cells[0].Value = "File Number";
            MohreEidDGV.Rows[15].Cells[0].Value = "Residence Issue Date";
            MohreEidDGV.Rows[16].Cells[0].Value = "Residence Expiry Date";
            MohreEidDGV.Rows[17].Cells[0].Value = "Mobile Number";
            MohreEidDGV.Rows[18].Cells[0].Value = "Abroad Location";
            MohreEidDGV.Rows[19].Cells[0].Value = "Company Name Arabic";
            MohreEidDGV.Rows[20].Cells[0].Value = "Profession";

            foreach (DataGridViewColumn col in MohreEidDGV.Columns)
            {
                col.DefaultCellStyle.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point);
            }
            foreach (DataGridViewColumn column in MohreEidDGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            MohreMohreDGV.ColumnCount = 2;
            MohreMohreDGV.Columns[0].Width = 250;
            MohreMohreDGV.Columns[1].Width = 647;
            MohreMohreDGV.RowTemplate.Height = 28;
            MohreMohreDGV.Rows.Add(18);
            for (int i = 0; i < MohreMohreDGV.Rows.Count; i++)
            {
                MohreMohreDGV.Rows[i].Cells[0].ReadOnly = true;
            }

            MohreMohreDGV.Rows[0].Cells[0].Value = "Company Name";
            MohreMohreDGV.Rows[1].Cells[0].Value = "Work Phone";
            MohreMohreDGV.Rows[2].Cells[0].Value = "Name Arabic";
            MohreMohreDGV.Rows[3].Cells[0].Value = "Name English";
            MohreMohreDGV.Rows[4].Cells[0].Value = "EID Number";
            MohreMohreDGV.Rows[5].Cells[0].Value = "UID";
            MohreMohreDGV.Rows[6].Cells[0].Value = "Residency File Number";
            MohreMohreDGV.Rows[7].Cells[0].Value = "Residence Issue Date";
            MohreMohreDGV.Rows[8].Cells[0].Value = "Residence Expiry Date";
            MohreMohreDGV.Rows[9].Cells[0].Value = "Passport Number";
            MohreMohreDGV.Rows[10].Cells[0].Value = "Passport Issue Place";
            MohreMohreDGV.Rows[11].Cells[0].Value = "Passport Issue Date";
            MohreMohreDGV.Rows[12].Cells[0].Value = "Passport Expiry Date";
            MohreMohreDGV.Rows[13].Cells[0].Value = "Nationality";
            MohreMohreDGV.Rows[14].Cells[0].Value = "Gender";
            MohreMohreDGV.Rows[15].Cells[0].Value = "Birth Date";
            MohreMohreDGV.Rows[16].Cells[0].Value = "Profession";
            MohreMohreDGV.Rows[17].Cells[0].Value = "Mobile Number";

            foreach (DataGridViewColumn col in MohreMohreDGV.Columns)
            {
                col.DefaultCellStyle.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point);
            }
            foreach (DataGridViewColumn column in MohreMohreDGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            metroTabControl1.SelectTab(0);
            metroTabControl2.SelectTab(0);
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
            Driver?.Quit();
            Application.Exit();
        }


        private void CleanPermitDataGridViews()
        {


            for (int i = 0; i < PermitDGV.Rows.Count; i++)
            {
                PermitDGV.Rows[i].Cells[1].Value = "";
            }

            for (int i = 0; i < EID2DGV.Rows.Count; i++)
            {
                EID2DGV.Rows[i].Cells[1].Value = "";
            }

            for (int i = 0; i < PermitMOHAPDGV.Rows.Count; i++)
            {
                PermitMOHAPDGV.Rows[i].Cells[1].Value = "";
            }
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
        private void PermitDGV_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.GetHashCode().Equals(589833))
            {
                SelectNextCell(PermitDGV);
                PermitDGV.BeginEdit(true);
            }
        }

        private void EID2DGV_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.GetHashCode().Equals(589833))
            {
                SelectNextCell(EID2DGV);
                EID2DGV.BeginEdit(true);
            }
        }
        private void MOHAPDGV_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.GetHashCode().Equals(589833))
            {
                SelectNextCell(PermitMOHAPDGV);
                PermitMOHAPDGV.BeginEdit(true);
            }
        }

        async Task SetCell(DataGridView x)
        {
            await Task.Delay(1);
            x.CurrentCell = x.Rows[x.CurrentCell.RowIndex].Cells[1];
        }
        private async void EID2DGV_CellEndEditAsync(object sender, DataGridViewCellEventArgs e)
        {
            await SetCell(EID2DGV);
            EID2DGV.BeginEdit(true);
        }

        private async void PermitDGV_CellEndEditAsync(object sender, DataGridViewCellEventArgs e)
        {
            await SetCell(PermitDGV);
            // PermitDGV.BeginEdit(true);
        }
        private async void MOHAPDGV_CellEndEditAsync(object sender, DataGridViewCellEventArgs e)
        {
            await SetCell(PermitMOHAPDGV);
            PermitMOHAPDGV.BeginEdit(true);
        }
        //Console.WriteLine("companyName:"+companyName);
        //Console.WriteLine("personalNameArabic: " + personalNameArabic);
        //Console.WriteLine("personalNameEnglish: " + personalNameEnglish);
        //Console.WriteLine("nationality: " + nationality);
        //Console.WriteLine("gender: " + gender);
        //Console.WriteLine("birthDate: "+birthDate);
        //Console.WriteLine("birthPlaceArabic: " + birthPlaceArabic);
        //Console.WriteLine("birthPlaceEnglish: " + birthPlaceEnglish);
        //Console.WriteLine("passportNumber: " + passportNumber);
        //Console.WriteLine("passportIssueDate: " + passportIssueDate);
        //Console.WriteLine("passportExpiryDate: " + passportExpiryDate);
        private void FillFormsPermitB_Click(object sender, EventArgs e)
        {

            if (PermitDGV?.Rows[18]?.Cells[1]?.Value?.ToString()?.Length > 4)
            {
                var firstpartNbr = PermitDGV.Rows[18].Cells[1].Value.ToString().Substring(0, 3);
                var secondepartNbr = PermitDGV.Rows[18].Cells[1].Value.ToString().Substring(3);
                EID2DGV.Rows[16].Cells[1].Value = firstpartNbr + "-" + secondepartNbr;
                /// add"-"after the third digit in phone number       
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
            var x = PermitDGV.Rows[9].Cells[1].Value;
            Console.WriteLine(x);
            var dateOfBirth = "";
            if ((PermitDGV.Rows[9].Cells[1].Value + "").Length > 1)
            {
                try
                {
                    DateTime dateOfBirthResult = DateTime.ParseExact(PermitDGV.Rows[9].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    dateOfBirth = dateOfBirthResult.ToString("yyyy/MM/dd");
                    PermitMOHAPDGV.Rows[15].Cells[1].Value = dateOfBirth;
                }
                catch (Exception)
                {
                    try
                    {
                        DateTime dateOfBirthResult = DateTime.ParseExact(PermitDGV.Rows[9].Cells[1].Value + "", @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        dateOfBirth = dateOfBirthResult.ToString("yyyy/MM/dd");
                        PermitMOHAPDGV.Rows[15].Cells[1].Value = dateOfBirth;
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                        return;
                    }


                }
            }
            else
            {
                PermitMOHAPDGV.Rows[15].Cells[1].Value = dateOfBirth;  //dateOfBirth;
            }
            var passportIssueDate = "";
            if ((PermitDGV.Rows[11].Cells[1].Value + "").Length > 1)
            {
                try
                {
                    DateTime passportIssueDateresult = DateTime.ParseExact(PermitDGV.Rows[11].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    passportIssueDate = passportIssueDateresult.ToString("yyyy/MM/dd");
                    PermitMOHAPDGV.Rows[11].Cells[1].Value = passportIssueDate;//passportIssueDate
                }
                catch (Exception)
                {
                    try
                    {
                        DateTime passportIssueDateresult = DateTime.ParseExact(PermitDGV.Rows[11].Cells[1].Value + "", @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        passportIssueDate = passportIssueDateresult.ToString("yyyy/MM/dd");
                        PermitMOHAPDGV.Rows[11].Cells[1].Value = passportIssueDate;//passportIssueDate
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                        return;
                    }

                }
            }
            else
            {
                PermitMOHAPDGV.Rows[11].Cells[1].Value = passportIssueDate;//passportIssueDate
            }
            var passportExpiryDate = "";
            if ((PermitDGV.Rows[12].Cells[1].Value + "").Length > 1)
            {
                try
                {
                    DateTime passportExpiryDateResult = DateTime.ParseExact(PermitDGV.Rows[12].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    passportExpiryDate = passportExpiryDateResult.ToString("yyyy/MM/dd");
                    PermitMOHAPDGV.Rows[12].Cells[1].Value = passportExpiryDate;//passportExpiryDate
                }
                catch (Exception)
                {
                    try
                    {
                        DateTime passportExpiryDateResult = DateTime.ParseExact(PermitDGV.Rows[12].Cells[1].Value + "", @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        passportExpiryDate = passportExpiryDateResult.ToString("yyyy/MM/dd");
                        PermitMOHAPDGV.Rows[12].Cells[1].Value = passportExpiryDate;//passportExpiryDate
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                        return;
                    }

                }
            }
            else
            {
                PermitMOHAPDGV.Rows[12].Cells[1].Value = passportExpiryDate;//passportExpiryDate
            }
            var residencIssueDate = "";
            if ((PermitDGV.Rows[15].Cells[1].Value + "").Length > 1)
            {
                try
                {
                    DateTime residencIssueDateResult = DateTime.ParseExact(PermitDGV.Rows[15].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    residencIssueDate = residencIssueDateResult.ToString("yyyy/MM/dd");
                    PermitMOHAPDGV.Rows[7].Cells[1].Value = residencIssueDate;//residencIssueDate
                }
                catch (Exception)
                {
                    try
                    {
                        DateTime residencIssueDateResult = DateTime.ParseExact(PermitDGV.Rows[15].Cells[1].Value + "", @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        residencIssueDate = residencIssueDateResult.ToString("yyyy/MM/dd");
                        PermitMOHAPDGV.Rows[7].Cells[1].Value = residencIssueDate;//residencIssueDate
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                        return;
                    }

                }
            }
            else
            {
                PermitMOHAPDGV.Rows[7].Cells[1].Value = residencIssueDate;//residencIssueDate
            }
            var residencExpiryDate = "";
            if ((PermitDGV.Rows[16].Cells[1].Value + "").Length > 1)
            {
                try
                {
                    DateTime residencExpiryDateResult = DateTime.ParseExact(PermitDGV.Rows[16].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    residencExpiryDate = residencExpiryDateResult.ToString("yyyy/MM/dd");
                    PermitMOHAPDGV.Rows[8].Cells[1].Value = residencExpiryDate;//residencExpiryDate
                }
                catch (Exception)
                {
                    try
                    {
                        DateTime residencExpiryDateResult = DateTime.ParseExact(PermitDGV.Rows[16].Cells[1].Value + "", @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        residencExpiryDate = residencExpiryDateResult.ToString("yyyy/MM/dd");
                        PermitMOHAPDGV.Rows[8].Cells[1].Value = residencExpiryDate;//residencExpiryDate
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                        return;
                    }

                }
            }
            else
            {
                PermitMOHAPDGV.Rows[8].Cells[1].Value = residencExpiryDate;//residencExpiryDate
            }
            var worPhone = "";
            if (PermitDGV.Rows[18]?.Cells[1]?.Value?.ToString()?.Length >= 10)
            {
                worPhone = PermitDGV.Rows[18].Cells[1].Value + "";
                worPhone = worPhone.Substring(3);
            }
            else
                worPhone = "";

            PermitMOHAPDGV.Rows[0].Cells[1].Value = PermitDGV.Rows[17].Cells[1].Value;
            PermitMOHAPDGV.Rows[1].Cells[1].Value = worPhone;
            PermitMOHAPDGV.Rows[2].Cells[1].Value = PermitDGV.Rows[3].Cells[1].Value;
            PermitMOHAPDGV.Rows[3].Cells[1].Value = PermitDGV.Rows[4].Cells[1].Value;
            PermitMOHAPDGV.Rows[4].Cells[1].Value = PermitDGV.Rows[0].Cells[1].Value;
            PermitMOHAPDGV.Rows[5].Cells[1].Value = PermitDGV.Rows[13].Cells[1].Value;
            PermitMOHAPDGV.Rows[6].Cells[1].Value = PermitDGV.Rows[14].Cells[1].Value;
            PermitMOHAPDGV.Rows[9].Cells[1].Value = PermitDGV.Rows[10].Cells[1].Value;
            PermitMOHAPDGV.Rows[10].Cells[1].Value = PermitDGV.Rows[10].Cells[1].Value;
            PermitMOHAPDGV.Rows[13].Cells[1].Value = PermitDGV.Rows[1].Cells[1].Value;
            PermitMOHAPDGV.Rows[14].Cells[1].Value = PermitDGV.Rows[2].Cells[1].Value;
            PermitMOHAPDGV.Rows[16].Cells[1].Value = PermitDGV.Rows[19].Cells[1].Value;
            PermitMOHAPDGV.Rows[17].Cells[1].Value = PermitDGV.Rows[18].Cells[1].Value;
            PermitMOHAPDGV.Rows[10].Cells[1].Value = PermitMOHAPDGV.Rows[13].Cells[1].Value;
        }

        async Task LoginToMohre()
        {
            //var vrifie = false;
            //do
            //{
            //    if (UserNameMohreTI.Text != "" && PassWordMohreTI.Text != "")
            //        vrifie = true;
            //    if (vrifie)
            //        break;
            //} while (true);
            LoggedInToMohre = false;
            var chromeOptions = new ChromeOptions();
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            //chromeOptions.AddArguments("headless");
            Driver = new ChromeDriver(chromeDriverService, chromeOptions);
            Driver.Navigate().GoToUrl(" https://eservices.mohre.gov.ae/SmartTasheel/home/index?lang=en-gb#");
            Driver.ExecuteScript("CloseMessagePopUp();");
            Driver.ExecuteScript("OpenLogin('');");
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            //todo Make the user/pass on config file/db 
            Driver.FindElementById("txtLoginUserName").SendKeys(UserNameMohreTI.Text);
            await Task.Delay(500);
            Driver.FindElementById("txtLoginPassword").SendKeys(PassWordMohreTI.Text);
            Driver.FindElementById("AcceptTerms").Click();
            Driver.ExecuteScript("GetSecurityQuestions();");
            await Task.Delay(1000);
            var question = "0";

            #region Wait for question id

            var tries = 0;
            do
            {
                try
                {
                    question = Driver.FindElementById("txtQuestion").GetAttribute("data-question");
                    break;
                }
                catch (Exception)
                {
                    tries++;
                    await Task.Delay(500);
                }
            } while (tries < 10);

            #endregion

            Driver.FindElementById("txtAnswer").SendKeys(question.Equals("1")
                ? "dubai"
                : "green");
            Driver.ExecuteScript("Login();");
            if (Driver.FindElementByXPath("//a[text()='Logout ']") != null)
                LoggedInToMohre = true;
            await Task.Delay(2000);
        }

        async Task ScrapeMohre()
        {
            if (!LoggedInToMohre)
            {
                //this when the user start to scrape Mohre , while the task to login we lunched on form load didn't finish yet
                MessageBox.Show("Not logged to Mohre site yet");
                return;
            }
            try
            {
                Driver.Navigate().GoToUrl("https://eservices.mohre.gov.ae/MOLForms/services.aspx?groupid=12");
                try
                {
                    Driver.ExecuteScript("popUp('companyEntryLC.aspx?fCode=72','72')");
                }
                catch (Exception e)
                {
                    //it mean we need to login again
                    Console.WriteLine(e);
                    await LoginToMohre();
                    Driver.Navigate().GoToUrl("https://eservices.mohre.gov.ae/MOLForms/services.aspx?groupid=12");
                    Driver.ExecuteScript("popUp('companyEntryLC.aspx?fCode=72','72')");
                }
                var mainWindowHandler = Driver.CurrentWindowHandle;
                var popup = Driver.WindowHandles[1];
                Driver.SwitchTo().Window(popup);
                Driver.FindElementById("ctrlNationality_txtCode").Click();
                //todo make the 3 params on UI
                Driver.FindElementById("ctrlNationality_txtCode").SendKeys(NationalityTI.Text);
                Driver.FindElementById("txtCompanyNumber").Click();
                Driver.FindElementById("txtCompanyNumber").SendKeys(CompanieCodeTI.Text);
                await Task.Delay(2000);
                Driver.FindElementById("txtCardNo").SendKeys(PersonCodeTI.Text);
                Driver.FindElementById("btnGo").Click();
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);//562889//407
                var personalNameArabic = FindElementByXPath("//input[@id='txtPER_NAME_ARB']")?.GetAttribute("value");
                var personalNameEnglish = FindElementByXPath("//input[@id='txtPER_NAME_ENG']")?.GetAttribute("value");
                var nationality = Driver.FindElementById("ctrlNationality_txtDescription").GetAttribute("value");
                var gender = FindElementByXPath("//select[@id='drpGENDER']/option[@selected]")?.Text;
                var birthDate = FindElementByXPath("//input[@id='txtBIRTH_DATE']")?.GetAttribute("value");
                var birthPlaceArabic = FindElementByXPath("//input[@id='txBIRTH_PLACE_ARB']")?.GetAttribute("value");
                var birthPlaceEnglish = FindElementByXPath("//input[@id='txtBIRTH_PLACE_ENG']")?.GetAttribute("value");
                var passportNumber = FindElementByXPath("//input[@id='txtPASSPORT_NO']")?.GetAttribute("value");
                var passportIssueDate = FindElementByXPath("//input[@id='txtPASSPORT_ISS']")?.GetAttribute("value");
                var passportExpiryDate = FindElementByXPath("//input[@id='txtPASSPORT_EXP']")?.GetAttribute("value");
                MohreDGV.Rows[1].Cells[1].Value = nationality;
                MohreDGV.Rows[2].Cells[1].Value = gender;
                MohreDGV.Rows[3].Cells[1].Value = personalNameArabic;
                MohreDGV.Rows[4].Cells[1].Value = personalNameEnglish;
                MohreDGV.Rows[7].Cells[1].Value = birthPlaceArabic;
                MohreDGV.Rows[8].Cells[1].Value = birthPlaceEnglish;
                MohreDGV.Rows[9].Cells[1].Value = birthDate;
                MohreDGV.Rows[10].Cells[1].Value = passportNumber;
                MohreDGV.Rows[11].Cells[1].Value = passportIssueDate;
                MohreDGV.Rows[12].Cells[1].Value = passportExpiryDate;
                try
                {
                    Driver.Navigate().GoToUrl("https://eservices.mohre.gov.ae/MOLForms/arabic/services.aspx?groupid=12");
                    try
                    {
                        Driver.ExecuteScript("popUp('companyEntryLC.aspx?fCode=72','72')");
                    }
                    catch (Exception e)
                    {
                        //it mean we need to login again
                        Console.WriteLine(e);
                        await LoginToMohre();
                        Driver.Navigate().GoToUrl("https://eservices.mohre.gov.ae/MOLForms/arabic/services.aspx?groupid=12");
                        Driver.ExecuteScript("popUp('companyEntryLC.aspx?fCode=72','72')");
                    }
                    Driver.FindElementById("ctrlNationality_txtCode").SendKeys(NationalityTI.Text);
                    Driver.FindElementById("txtCompanyNumber").Click();
                    Driver.FindElementById("txtCompanyNumber").SendKeys(CompanieCodeTI.Text);
                    Driver.FindElementById("txtCardNo").SendKeys(PersonCodeTI.Text);
                    await Task.Delay(500);
                    Driver.FindElementById("btnGo").Click();
                    Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                    var companyName = Driver.FindElement(By.XPath("//span[@id='ctrlComInfo_lblCompanyNameArabic']")).Text;
                    MohreDGV.Rows[17].Cells[1].Value = companyName;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                //todo Put the data to the UI
                Driver.Close();
                Driver.SwitchTo().Window(mainWindowHandler);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show(e.ToString(), "Error");
            }
        }
        private void FillFormsMohreB_Click(object sender, EventArgs e)
        {
            #region Fill date data
            if ((MohreDGV.Rows[15].Cells[1].Value + "").ToString().Length > 1)
            {

                try
                {
                    DateTime dateOfBirthResult = DateTime.ParseExact(MohreDGV.Rows[15].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    MohreEidDGV.Rows[15].Cells[1].Value = dateOfBirthResult.ToString("dd/MM/yyy");
                }
                catch (Exception)
                {

                    MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                    return;
                }

            }
            else
            {
                MohreEidDGV.Rows[15].Cells[1].Value = "";
            }
            if ((MohreDGV.Rows[16].Cells[1].Value + "").ToString().Length > 1)
            {
                Console.WriteLine("hi");
                try
                {
                    DateTime expDateResult = DateTime.ParseExact(MohreDGV.Rows[16].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    var expDate = expDateResult.ToString("dd/MM/yyyy");
                    MohreEidDGV.Rows[16].Cells[1].Value = expDate;
                }
                catch (Exception)
                {

                    MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                    return;
                }
            }
            else
            {
                MohreEidDGV.Rows[16].Cells[1].Value = "";
            }
            if ((MohreDGV.Rows[15].Cells[1].Value + "").ToString().Length > 1)
            {

                try
                {
                    DateTime residenceIssueDate = DateTime.ParseExact(MohreDGV.Rows[15].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    var issuDate = residenceIssueDate.ToString("yyyy/MM/dd");//done
                    MohreMohreDGV.Rows[7].Cells[1].Value = issuDate;//residenceIssuedate
                }
                catch (Exception)
                {

                    MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                    return;
                }
            }
            else
            {
                MohreMohreDGV.Rows[7].Cells[1].Value = "";
            }
            if ((MohreDGV.Rows[16].Cells[1].Value + "").ToString().Length > 1)
            {

                try
                {
                    DateTime EXPDate = DateTime.ParseExact(MohreDGV.Rows[16].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    var expDate = EXPDate.ToString("yyyy/MM/dd");//done
                    MohreMohreDGV.Rows[8].Cells[1].Value = expDate;//residenceIssueDate
                }
                catch (Exception)
                {

                    MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                    return;
                }
            }
            else
            {
                MohreMohreDGV.Rows[8].Cells[1].Value = "";
            }
            if ((MohreDGV.Rows[9].Cells[1].Value + "").ToString().Length > 1)
            {

                try
                {
                    DateTime dateOfBirthResult = DateTime.ParseExact(MohreDGV.Rows[9].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    var dateOfB = dateOfBirthResult.ToString("yyyy/MM/dd");
                    MohreMohreDGV.Rows[15].Cells[1].Value = dateOfB;
                }
                catch (Exception)
                {

                    MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                    return;
                }
            }
            else
            {
                MohreMohreDGV.Rows[15].Cells[1].Value = "";
            }
            if ((MohreDGV.Rows[11].Cells[1].Value + "").ToString().Length > 1)
            {

                try
                {
                    DateTime passportIssueDate = DateTime.ParseExact(MohreDGV.Rows[11].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    var passpoetIssueD = passportIssueDate.ToString("yyyy/MM/dd");
                    MohreMohreDGV.Rows[11].Cells[1].Value = passpoetIssueD;
                }
                catch (Exception)
                {

                    MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                    return;
                }
            }
            else
            {
                MohreMohreDGV.Rows[11].Cells[1].Value = "";
            }
            if ((MohreDGV.Rows[12].Cells[1].Value + "").ToString().Length > 1)
            {

                try
                {
                    DateTime passportIssueDate = DateTime.ParseExact(MohreDGV.Rows[12].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    var passpoetIssueD = passportIssueDate.ToString("yyyy/MM/dd");
                    MohreMohreDGV.Rows[12].Cells[1].Value = passpoetIssueD;
                }
                catch (Exception)
                {

                    MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                    return;
                }
            }
            else
            {
                MohreMohreDGV.Rows[12].Cells[1].Value = "";
            }
            #endregion
            MohreEidDGV.Rows[0].Cells[1].Value = MohreDGV.Rows[0].Cells[1].Value;//EID
            MohreEidDGV.Rows[1].Cells[1].Value = MohreDGV.Rows[1].Cells[1].Value;//nationality;
            MohreEidDGV.Rows[2].Cells[1].Value = MohreDGV.Rows[2].Cells[1].Value;//gender;
            MohreEidDGV.Rows[3].Cells[1].Value = MohreDGV.Rows[3].Cells[1].Value;//personalNameArabic;
            MohreEidDGV.Rows[5].Cells[1].Value = MohreDGV.Rows[4].Cells[1].Value;//personalNameEnglish
            MohreEidDGV.Rows[7].Cells[1].Value = MohreDGV.Rows[7].Cells[1].Value;//birthPlaceArabic
            MohreEidDGV.Rows[8].Cells[1].Value = MohreDGV.Rows[8].Cells[1].Value;//birthPlaceEnglish
            MohreEidDGV.Rows[9].Cells[1].Value = MohreDGV.Rows[9].Cells[1].Value;//birthDate
            MohreEidDGV.Rows[10].Cells[1].Value = MohreDGV.Rows[10].Cells[1].Value;//passportNumber
            MohreEidDGV.Rows[11].Cells[1].Value = MohreDGV.Rows[11].Cells[1].Value;//passportIssueDate
            MohreEidDGV.Rows[12].Cells[1].Value = MohreDGV.Rows[12].Cells[1].Value;//passportExpiryDate
            MohreEidDGV.Rows[13].Cells[1].Value = MohreDGV.Rows[13].Cells[1].Value;//UID
            MohreEidDGV.Rows[14].Cells[1].Value = MohreDGV.Rows[14].Cells[1].Value;//file nbr
            MohreEidDGV.Rows[19].Cells[1].Value = MohreDGV.Rows[17].Cells[1].Value;//companie name ARB
            MohreEidDGV.Rows[17].Cells[1].Value = MohreDGV.Rows[18].Cells[1].Value;//mobile nbr
            MohreEidDGV.Rows[20].Cells[1].Value = MohreDGV.Rows[19].Cells[1].Value;//profession 
            if (MohreDGV.Rows[18].Cells[1].Value?.ToString()?.Length > 4)
            {
                var firstpartNbr = MohreDGV.Rows[18].Cells[1].Value.ToString().Substring(0, 3);
                var secondepartNbr = MohreDGV.Rows[18].Cells[1].Value.ToString().Substring(3);
                MohreEidDGV.Rows[17].Cells[1].Value = firstpartNbr + "-" + secondepartNbr;
                /// add"-"after the third digit in phone number       
            }
            else
                MohreEidDGV.Rows[17].Cells[1].Value = MohreDGV.Rows[18].Cells[1].Value;


            var worPhone = "";
            if (MohreDGV.Rows[18]?.Cells[1]?.Value?.ToString()?.Length >= 10)
            {
                worPhone = MohreDGV.Rows[18].Cells[1].Value + "";
                worPhone = worPhone.Substring(3);
            }
            else
                worPhone = "";
            MohreMohreDGV.Rows[1].Cells[1].Value = worPhone;
            MohreMohreDGV.Rows[13].Cells[1].Value = MohreDGV.Rows[1].Cells[1].Value;
            MohreMohreDGV.Rows[14].Cells[1].Value = MohreDGV.Rows[2].Cells[1].Value;
            MohreMohreDGV.Rows[2].Cells[1].Value = MohreDGV.Rows[3].Cells[1].Value;
            MohreMohreDGV.Rows[3].Cells[1].Value = MohreDGV.Rows[4].Cells[1].Value;
            MohreMohreDGV.Rows[4].Cells[1].Value = MohreDGV.Rows[0].Cells[1].Value;
            MohreMohreDGV.Rows[5].Cells[1].Value = MohreDGV.Rows[13].Cells[1].Value;
            MohreMohreDGV.Rows[6].Cells[1].Value = MohreDGV.Rows[14].Cells[1].Value;
            MohreMohreDGV.Rows[9].Cells[1].Value = MohreDGV.Rows[10].Cells[1].Value;
            //MohreMohreDGV.Rows[11].Cells[1].Value = MohreDGV.Rows[11].Cells[1].Value;
            //MohreMohreDGV.Rows[12].Cells[1].Value = MohreDGV.Rows[12].Cells[1].Value;
            //MohreMohreDGV.Rows[15].Cells[1].Value = MohreDGV.Rows[9].Cells[1].Value;
            MohreMohreDGV.Rows[16].Cells[1].Value = MohreDGV.Rows[7].Cells[1].Value;
            MohreMohreDGV.Rows[17].Cells[1].Value = MohreDGV.Rows[8].Cells[1].Value;
            MohreMohreDGV.Rows[16].Cells[1].Value = MohreDGV.Rows[19].Cells[1].Value;
            MohreMohreDGV.Rows[17].Cells[1].Value = MohreDGV.Rows[18].Cells[1].Value;
            MohreMohreDGV.Rows[10].Cells[1].Value = MohreMohreDGV.Rows[13].Cells[1].Value;
            MohreMohreDGV.Rows[0].Cells[1].Value = MohreDGV.Rows[17].Cells[1].Value;

        }

        IWebElement FindElementByXPath(string xpath)
        {
            try
            {
                return Driver.FindElementByXPath(xpath);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private async void ScrapeEChannelB_ClickAsync(object sender, EventArgs e)
        {

            var normalOrbetaRB = "";
            var ResidencyviewOrVisaviewRB = "";
            if (NormalRadioB.Checked)
            {
                normalOrbetaRB = "echannels";
            }
            if (BetaRadioB.Checked)
            {
                normalOrbetaRB = "beta.echannels";
            }
            if (ResidencyviewRadioB.Checked)
            {
                ResidencyviewOrVisaviewRB = "residency";
            }
            if (VisaviewRadioB.Checked)
            {
                ResidencyviewOrVisaviewRB = "visa";
            }
            if ((normalOrbetaRB == "" || ResidencyviewOrVisaviewRB == "") || (normalOrbetaRB == "" && ResidencyviewOrVisaviewRB == ""))
            {
                MessageBox.Show("Please tick the necessary parameters");
                return;
            }
            //Console.WriteLine(normalOrbetaRB);
            //Console.WriteLine(ResidencyviewOrVisaviewRB);
            //Console.WriteLine($"https://{normalOrbetaRB}.moi.gov.ae/echannels/api/api/establishment/{ResidencyviewOrVisaviewRB}/{CodeEChannelT.Text}");
            //return;
            var Object = new JObject();
            // CodeEChannelT.Text = "4012019020046239";
            CleanECannelDataGridViews();
            if (EChannelUsernameTI.Text == "" || EChannelPasswTI.Text == "")
            {
                MessageBox.Show("username and/or password are missed ");
                return;
            }
            if (CodeEChannelTI.Text == "")
            {
                MessageBox.Show("Please put the code wich you will scrape data with");
                return;
            }
            Console.WriteLine($"https://{normalOrbetaRB}.moi.gov.ae/echannels/api/api/establishment/{ResidencyviewOrVisaviewRB}/{CodeEChannelTI.Text}");

            Object = JObject.Parse(File.ReadAllText("ECHannel headers.txt"));
            var refreshToken = (string)Object.SelectToken("RefreshToken");
            var userToken = (string)Object.SelectToken("UserToken");
            var EchannelData = await HttpCaller.GetEchannelHtml($"https://{normalOrbetaRB}.moi.gov.ae/echannels/api/api/establishment/{ResidencyviewOrVisaviewRB}/{CodeEChannelTI.Text}", refreshToken, userToken);
            if (EchannelData.error != null)
            {
                MessageBox.Show(EchannelData.error);
                return;
            }
            if (EchannelData.html == "" || EchannelData.html == "null")
            {
                Console.WriteLine("{\"userName\":" + "\"" + EChannelUsernameTI.Text + "\"" + ",\"password\":" + "\"" + EChannelPasswTI.Text + "\"}");
                var logInResponse = await HttpCaller.PostJson("https://echannels.moi.gov.ae/echannels/api/api/user/login", "{\"userName\":" + "\"" + EChannelUsernameTI.Text + "\"" + ",\"password\":" + "\"" + EChannelPasswTI.Text + "\"}");
                if (logInResponse.error != null)
                {
                    MessageBox.Show(logInResponse.error);
                    return;
                }
                try
                {
                    var jArray = JArray.Parse(logInResponse.json);
                    Object = JObject.Parse(jArray[0].ToString());
                    Console.WriteLine(Object.SelectToken("messageCode"));
                    //NAME_OR_PASSWORD_IS_NOT_CORRECT
                    var errorMsg = Object.SelectToken("messageCode");
                    if (errorMsg.Contains("USER_NAME_OR_PASSWORD_IS_NOT_CORRECT"))
                    {
                        MessageBox.Show("USERNAME OR PASSWORD IS NOT CORRECT PLEASE VERIFY");
                        return;
                    }


                }
                catch (Exception)
                {
                    var headers = new ECHannelHeaders();
                    Object = JObject.Parse(logInResponse.json);
                    headers.UserToken = (string)Object.SelectToken("userToken");
                    headers.RefreshToken = (string)Object.SelectToken("refreshToken");
                    var jsonHeaders = JsonConvert.SerializeObject(headers, Formatting.Indented);
                    File.WriteAllText("ECHannel headers.txt", jsonHeaders);
                    EchannelData = await HttpCaller.GetEchannelHtml($"https://{normalOrbetaRB}.moi.gov.ae/echannels/api/api/establishment/{ResidencyviewOrVisaviewRB}/{CodeEChannelTI.Text}", headers.RefreshToken, headers.UserToken);
                    if (EchannelData.error != null)
                    {
                        MessageBox.Show(EchannelData.error);
                        return;
                    }
                }
            }
            try
            {
                Object = JObject.Parse(EchannelData.html);
            }
            catch (Exception)
            {
                MessageBox.Show("This code is not available");
                return;
            }
            var visafilenbr = (string)Object?.SelectToken("departmentCode") + "/" + (string)Object?.SelectToken("serviceYear") + "/" + (string)Object?.SelectToken("serviceCode") + "/" + (string)Object?.SelectToken("sequenceNumber") ?? "";
            //visafilenbr=(departmentCode+serviceYear+maritalStatusId+sequenceNumber)
            var UnifiedNo = ((string)Object?.SelectToken("personUnifiedNumber"))?.Trim() ?? "";
            //UnifiedNo= personUnifiedNumber
            var FullENgname = (string)Object.SelectToken("englishName");
            //FullENgname= englishName
            var currentnationality = (string)Object.SelectToken("currentNationality.text");
            //currentnationality currentNationality/text
            var gender = ((string)Object.SelectToken("gender.text")).Trim() ?? "";
            if (gender == "انثى")
                gender = "FEMALE";
            else
                gender = "MALE";
            //gender =gender/text
            var motherEnglishName = ((string)Object?.SelectToken("motherEnglishName"))?.Trim() ?? "";
            //motherEnglish Name=motherEnglishName
            var FullNameArb = ((string)Object.SelectToken("arabicName")).Trim() ?? "";
            //FullNameArb =arabicName
            var dateOfBirth = ((string)Object.SelectToken("dateOfBirth")).Trim() ?? "";
            dateOfBirth = dateOfBirth.Substring(0, 10);
            DateTime dateOfBirthResult = DateTime.ParseExact(dateOfBirth, @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            dateOfBirth = dateOfBirthResult.ToString("dd/MM/yyy");
            //dateOfBirth=dateOfBirth
            var motherArabicName = ((string)Object.SelectToken("motherArabicName"))?.Trim() ?? "";
            //motherArabicName motherArabicName
            var passportIssueDate = ((string)Object?.SelectToken("passportIssueDate"))?.Trim() ?? "";
            passportIssueDate = passportIssueDate.Substring(0, 10);
            DateTime passportIssueDateResult = DateTime.ParseExact(passportIssueDate, @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            passportIssueDate = passportIssueDateResult.ToString("dd/MM/yyyy");
            //passportIssueDate= passportIssueDate
            var passportExpiryDate = ((string)Object?.SelectToken("passportExpiryDate"))?.Trim() ?? "";
            passportExpiryDate = passportExpiryDate.Substring(0, 10);
            DateTime passportExpiryDateResult = DateTime.ParseExact(passportExpiryDate, @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            passportExpiryDate = passportExpiryDateResult.ToString("dd/MM/yyyy");
            //passportExpiryDate=passportExpiryDate
            var passportNumber = ((string)Object.SelectToken("passportNumber")).Trim() ?? "";
            //passportNumber= passportNumber
            Console.WriteLine(passportIssueDate);
            if (ResidencyviewRadioB.Checked)
            {//foreignResidenceExpiryDate residencyExpireDate
                var residencyIssueDate = ((string)Object.SelectToken("foreignResidenceIssueDate"))?.Trim() ?? "";
                residencyIssueDate = residencyIssueDate.Substring(0, 10);
                DateTime residencyIssueDateResult = DateTime.ParseExact(residencyIssueDate, @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                residencyIssueDate = residencyIssueDateResult.ToString("dd/MM/yyyy");

                var residencyExpireDate = ((string)Object.SelectToken("foreignResidenceExpiryDate")).Trim() ?? "";
                residencyExpireDate = residencyExpireDate.Substring(0, 10);
                DateTime residencyExpireDateResult = DateTime.ParseExact(residencyExpireDate, @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                residencyExpireDate = residencyExpireDateResult.ToString("dd/MM/yyyy");
                EChannelDGV.Rows[15].Cells[1].Value = residencyIssueDate;
                EChannelDGV.Rows[16].Cells[1].Value = residencyExpireDate;
                Console.WriteLine(residencyIssueDate);
                Console.WriteLine(residencyExpireDate);
            }

            EChannelDGV.Rows[13].Cells[1].Value = UnifiedNo;
            EChannelDGV.Rows[1].Cells[1].Value = currentnationality;
            EChannelDGV.Rows[2].Cells[1].Value = gender;
            EChannelDGV.Rows[3].Cells[1].Value = FullNameArb;
            EChannelDGV.Rows[4].Cells[1].Value = FullENgname;
            EChannelDGV.Rows[5].Cells[1].Value = motherEnglishName;
            EChannelDGV.Rows[6].Cells[1].Value = motherArabicName;
            EChannelDGV.Rows[9].Cells[1].Value = dateOfBirth;
            EChannelDGV.Rows[10].Cells[1].Value = passportNumber;
            EChannelDGV.Rows[11].Cells[1].Value = passportIssueDate;
            EChannelDGV.Rows[12].Cells[1].Value = passportExpiryDate;
            EChannelDGV.Rows[14].Cells[1].Value = visafilenbr;
        }

        private void CleanECannelDataGridViews()
        {
            for (int i = 0; i < EChannelDGV.Rows.Count; i++)
            {
                EChannelDGV.Rows[i].Cells[1].Value = "";
            }
            for (int i = 0; i < EChnEIDDgview.Rows.Count; i++)
            {
                EChnEIDDgview.Rows[i].Cells[1].Value = "";
            }
            for (int i = 0; i < EchanMohreDgview.Rows.Count; i++)
            {
                EchanMohreDgview.Rows[i].Cells[1].Value = "";
            }
        }

        private void FillFormEChannelB_Click(object sender, EventArgs e)
        {
            if (EChannelDGV?.Rows[18]?.Cells[1]?.Value?.ToString()?.Length > 4)
            {
                var firstpartNbr = EChannelDGV.Rows[18].Cells[1].Value.ToString().Substring(0, 3);
                var secondepartNbr = EChannelDGV.Rows[18].Cells[1].Value.ToString().Substring(3);
                EChnEIDDgview.Rows[16].Cells[1].Value = firstpartNbr + "-" + secondepartNbr;
                /// add "-" after the third digit in phone number       
            }
            else
                EChnEIDDgview.Rows[16].Cells[1].Value = EChannelDGV.Rows[18].Cells[1].Value;

            EChnEIDDgview.Rows[0].Cells[1].Value = EChannelDGV.Rows[0].Cells[1].Value;
            EChnEIDDgview.Rows[1].Cells[1].Value = EChannelDGV.Rows[1].Cells[1].Value;
            EChnEIDDgview.Rows[2].Cells[1].Value = EChannelDGV.Rows[2].Cells[1].Value;
            EChnEIDDgview.Rows[3].Cells[1].Value = EChannelDGV.Rows[3].Cells[1].Value;
            EChnEIDDgview.Rows[4].Cells[1].Value = EChannelDGV.Rows[6].Cells[1].Value;
            EChnEIDDgview.Rows[5].Cells[1].Value = EChannelDGV.Rows[4].Cells[1].Value;
            EChnEIDDgview.Rows[6].Cells[1].Value = EChannelDGV.Rows[5].Cells[1].Value;
            EChnEIDDgview.Rows[7].Cells[1].Value = EChannelDGV.Rows[7].Cells[1].Value;
            EChnEIDDgview.Rows[8].Cells[1].Value = EChannelDGV.Rows[9].Cells[1].Value;
            EChnEIDDgview.Rows[9].Cells[1].Value = EChannelDGV.Rows[10].Cells[1].Value;
            EChnEIDDgview.Rows[10].Cells[1].Value = EChannelDGV.Rows[11].Cells[1].Value;
            EChnEIDDgview.Rows[11].Cells[1].Value = EChannelDGV.Rows[12].Cells[1].Value;
            EChnEIDDgview.Rows[12].Cells[1].Value = EChannelDGV.Rows[13].Cells[1].Value;
            EChnEIDDgview.Rows[13].Cells[1].Value = EChannelDGV.Rows[14].Cells[1].Value;
            EChnEIDDgview.Rows[18].Cells[1].Value = EChannelDGV.Rows[17].Cells[1].Value;
            EChnEIDDgview.Rows[19].Cells[1].Value = EChannelDGV.Rows[19].Cells[1].Value;
            EChnEIDDgview.Rows[15].Cells[1].Value = EChannelDGV.Rows[16].Cells[1].Value;
            if (ResidencyviewRadioB.Checked)
            {
                var residencyIssueDate = "";
                var residencyExpireDate = "";
                if ((EChannelDGV.Rows[15].Cells[1].Value + "").Length > 1)
                {
                    try
                    {
                        DateTime dateOfBirthResult = DateTime.ParseExact(EChannelDGV.Rows[15].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        residencyIssueDate = dateOfBirthResult.ToString("dd/MM/yyyy");
                        EChnEIDDgview.Rows[14].Cells[1].Value = residencyIssueDate;
                    }
                    catch (Exception)
                    {
                        try
                        {
                            DateTime dateOfBirthResult = DateTime.ParseExact(EChannelDGV.Rows[15].Cells[1].Value + "", @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            residencyIssueDate = dateOfBirthResult.ToString("dd/MM/yyyy");
                            EChnEIDDgview.Rows[14].Cells[1].Value = residencyIssueDate;
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                            return;
                        }

                    }
                }
                else
                {
                    EChnEIDDgview.Rows[14].Cells[1].Value = residencyIssueDate;  //dateOfBirth;
                }
                if ((EChannelDGV.Rows[16].Cells[1].Value + "").Length > 1)
                {
                    try
                    {
                        DateTime residencExpiryDateResult = DateTime.ParseExact(EChannelDGV.Rows[16].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        residencyExpireDate = residencExpiryDateResult.ToString("yyyy/MM/dd");
                        EchanMohreDgview.Rows[15].Cells[1].Value = residencyExpireDate;//residencExpiryDate
                    }
                    catch (Exception)
                    {
                        try
                        {
                            DateTime residencExpiryDateResult = DateTime.ParseExact(EChannelDGV.Rows[16].Cells[1].Value + "", @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            residencyExpireDate = residencExpiryDateResult.ToString("yyyy/MM/dd");
                            EchanMohreDgview.Rows[15].Cells[1].Value = residencyExpireDate;//residencExpiryDate
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                            return;
                        }
                    }
                }
                else
                {
                    EChnEIDDgview.Rows[15].Cells[1].Value = residencyExpireDate;  //dateOfBirth;
                }
            }
            var dateOfBirth = "";
            if ((EChannelDGV.Rows[9].Cells[1].Value + "").Length > 1)
            {
                try
                {
                    DateTime dateOfBirthResult = DateTime.ParseExact(EChannelDGV.Rows[9].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    dateOfBirth = dateOfBirthResult.ToString("yyyy/MM/dd");
                    EchanMohreDgview.Rows[15].Cells[1].Value = dateOfBirth;
                }
                catch (Exception)
                {
                    try
                    {
                        DateTime dateOfBirthResult = DateTime.ParseExact(EChannelDGV.Rows[9].Cells[1].Value + "", @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        dateOfBirth = dateOfBirthResult.ToString("yyyy/MM/dd");
                        EchanMohreDgview.Rows[15].Cells[1].Value = dateOfBirth;
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                        return;
                    }

                }
            }
            else
            {
                EchanMohreDgview.Rows[15].Cells[1].Value = dateOfBirth;  //dateOfBirth;
            }
            var passportIssueDate = "";
            if ((EChannelDGV.Rows[11].Cells[1].Value + "").Length > 1)
            {
                try
                {
                    DateTime passportIssueDateresult = DateTime.ParseExact(EChannelDGV.Rows[11].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    passportIssueDate = passportIssueDateresult.ToString("yyyy/MM/dd");
                    EchanMohreDgview.Rows[11].Cells[1].Value = passportIssueDate;//passportIssueDate
                }
                catch (Exception)
                {
                    try
                    {
                        DateTime passportIssueDateresult = DateTime.ParseExact(EChannelDGV.Rows[11].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        passportIssueDate = passportIssueDateresult.ToString("yyyy/MM/dd");
                        EchanMohreDgview.Rows[11].Cells[1].Value = passportIssueDate;//passportIssueDate
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                        return;
                    }

                }
            }
            else
            {
                EchanMohreDgview.Rows[11].Cells[1].Value = passportIssueDate;//passportIssueDate
            }
            var passportExpiryDate = "";
            if ((EChannelDGV.Rows[12].Cells[1].Value + "").Length > 1)
            {
                try
                {
                    DateTime passportExpiryDateResult = DateTime.ParseExact(EChannelDGV.Rows[12].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    passportExpiryDate = passportExpiryDateResult.ToString("yyyy/MM/dd");
                    EchanMohreDgview.Rows[12].Cells[1].Value = passportExpiryDate;//passportExpiryDate
                }
                catch (Exception)
                {


                    try
                    {
                        DateTime passportExpiryDateResult = DateTime.ParseExact(EChannelDGV.Rows[12].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        passportExpiryDate = passportExpiryDateResult.ToString("yyyy/MM/dd");
                        EchanMohreDgview.Rows[12].Cells[1].Value = passportExpiryDate;//passportExpiryDate
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                        return;
                    }

                }
            }
            else
            {
                EchanMohreDgview.Rows[12].Cells[1].Value = passportExpiryDate;//passportExpiryDate
            }
            if (EChannelDGV?.Rows[15]?.Cells[1]?.Value?.ToString().Length > 1)
            {
                var residencIssueDate = "";
                if ((EChannelDGV.Rows[15].Cells[1].Value + "").Length > 1)
                {
                    try
                    {
                        DateTime residencIssueDateResult = DateTime.ParseExact(EChannelDGV.Rows[15].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        residencIssueDate = residencIssueDateResult.ToString("yyyy/MM/dd");
                        EchanMohreDgview.Rows[7].Cells[1].Value = residencIssueDate;//residencIssueDate
                    }
                    catch (Exception)
                    {
                        try
                        {
                            DateTime residencIssueDateResult = DateTime.ParseExact(EChannelDGV.Rows[15].Cells[1].Value + "", @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            residencIssueDate = residencIssueDateResult.ToString("yyyy/MM/dd");
                            EchanMohreDgview.Rows[7].Cells[1].Value = residencIssueDate;//residencIssueDate
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                            return;
                        }
                    }
                }
                else
                {
                    EchanMohreDgview.Rows[7].Cells[1].Value = residencIssueDate;//residencIssueDate
                }
            }
            else
            {
                EchanMohreDgview.Rows[7].Cells[1].Value = EChannelDGV.Rows[15].Cells[1].Value;
            }
            var residencExpiryDate = "";
            if ((EChannelDGV.Rows[16].Cells[1].Value + "").Length > 1)
            {
                try
                {
                    DateTime residencExpiryDateResult = DateTime.ParseExact(EChannelDGV.Rows[16].Cells[1].Value + "", @"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    residencExpiryDate = residencExpiryDateResult.ToString("yyyy/MM/dd");
                    EchanMohreDgview.Rows[8].Cells[1].Value = residencExpiryDate;//residencExpiryDate
                }
                catch (Exception)
                {
                    try
                    {
                        DateTime residencExpiryDateResult = DateTime.ParseExact(EChannelDGV.Rows[16].Cells[1].Value + "", @"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        residencExpiryDate = residencExpiryDateResult.ToString("yyyy/MM/dd");
                        EchanMohreDgview.Rows[8].Cells[1].Value = residencExpiryDate;//residencExpiryDate
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("the input date format is not valid the format should be: \"dd / MM / yyyy\"");
                        return;
                    }
                }
            }
            else
            {
                EchanMohreDgview.Rows[8].Cells[1].Value = residencExpiryDate;//residencExpiryDate
            }

            var worPhone = "";
            if (EChannelDGV.Rows[18]?.Cells[1]?.Value?.ToString()?.Length >= 10)
            {
                worPhone = EChannelDGV.Rows[18].Cells[1].Value + "";
                worPhone = worPhone.Substring(3);
            }
            else
                worPhone = "";
            EchanMohreDgview.Rows[0].Cells[1].Value = EChannelDGV.Rows[17].Cells[1].Value;
            EchanMohreDgview.Rows[1].Cells[1].Value = worPhone;
            EchanMohreDgview.Rows[2].Cells[1].Value = EChannelDGV.Rows[3].Cells[1].Value;
            EchanMohreDgview.Rows[3].Cells[1].Value = EChannelDGV.Rows[4].Cells[1].Value;
            EchanMohreDgview.Rows[4].Cells[1].Value = EChannelDGV.Rows[0].Cells[1].Value;
            EchanMohreDgview.Rows[5].Cells[1].Value = EChannelDGV.Rows[13].Cells[1].Value;
            EchanMohreDgview.Rows[6].Cells[1].Value = EChannelDGV.Rows[14].Cells[1].Value;
            EchanMohreDgview.Rows[9].Cells[1].Value = EChannelDGV.Rows[10].Cells[1].Value;
            EchanMohreDgview.Rows[13].Cells[1].Value = EChannelDGV.Rows[1].Cells[1].Value;
            EchanMohreDgview.Rows[14].Cells[1].Value = EChannelDGV.Rows[2].Cells[1].Value;
            EchanMohreDgview.Rows[16].Cells[1].Value = EChannelDGV.Rows[19].Cells[1].Value;
            EchanMohreDgview.Rows[17].Cells[1].Value = EChannelDGV.Rows[18].Cells[1].Value;
            EchanMohreDgview.Rows[10].Cells[1].Value = EchanMohreDgview.Rows[13].Cells[1].Value;
        }

        private void EChannelDGV_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.GetHashCode().Equals(589833))
            {
                SelectNextCell(EChannelDGV);
                EChannelDGV.BeginEdit(true);
            }
        }

        private void EChnEIDDgview_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.GetHashCode().Equals(589833))
            {
                SelectNextCell(EChnEIDDgview);
                EChnEIDDgview.BeginEdit(true);
            }
        }

        private void EchanMohreDgview_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.GetHashCode().Equals(589833))
            {
                SelectNextCell(EchanMohreDgview);
                EchanMohreDgview.BeginEdit(true);
            }
        }

        private async void EChannelDGV_CellEndEditAsync(object sender, DataGridViewCellEventArgs e)
        {
            await SetCell(EChannelDGV);
        }

        private async void ScrapeMohreB_Click(object sender, EventArgs e)
        {
            ScrapeMohreB.Enabled = false;
            if (UserNameMohreTI.Text == "" || PassWordMohreTI.Text == "")
            {
                MessageBox.Show("username and/or password are missed ");
                ScrapeMohreB.Enabled = true;
                return;
            }
            if (NationalityTI.Text == "")
            {
                MessageBox.Show("Please fill Nationality field ");
                ScrapeMohreB.Enabled = true;
                return;
            }
            if (CompanieCodeTI.Text == "")
            {
                MessageBox.Show("Please fill Companie code field ");
                ScrapeMohreB.Enabled = true;
                return;
            }
            if (PersonCodeTI.Text == "")
            {
                MessageBox.Show("Please fill Person code field ");
                ScrapeMohreB.Enabled = true;
                return;
            }
            CleanMohreDataGridViews();
            await Task.Run(ScrapeMohre);
            ScrapeMohreB.Enabled = true;
        }
        private void CleanMohreDataGridViews()
        {
            for (int i = 0; i < MohreDGV.Rows.Count; i++)
            {
                MohreDGV.Rows[i].Cells[1].Value = "";
            }
            for (int i = 0; i < MohreEidDGV.Rows.Count; i++)
            {
                MohreEidDGV.Rows[i].Cells[1].Value = "";
            }
            for (int i = 0; i < MohreMohreDGV.Rows.Count; i++)
            {
                MohreMohreDGV.Rows[i].Cells[1].Value = "";
            }
        }

        private async void MohreDGV_CellEndEditAsync(object sender, DataGridViewCellEventArgs e)
        {
            await SetCell(MohreDGV);
        }

        private void MohreDGV_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.GetHashCode().Equals(589833))
            {
                SelectNextCell(MohreDGV);
                MohreDGV.BeginEdit(true);
            }
        }

        

        private void UploadImgEChannelEIDB_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog { Filter = @"png|*.png", InitialDirectory = _path };
            if (o.ShowDialog() == DialogResult.OK)
            {
                ImgPathForEChannelEIDTextBoxI.Text = o.FileName;
               
            }
        }

        private void UploadImgEChannelMohapB_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog { Filter = @"|*", InitialDirectory = _path };
            if (o.ShowDialog() == DialogResult.OK)
            {//|*.jpg;*.jpeg;*.png
                ImgPathForEChannelMohapTextBoxI.Text = o.FileName;
               
            }
        }

        private void SaveFromEChannelMOHAPB_Click(object sender, EventArgs e)
        {

            if (EchanMohreDgview.Rows[2].Cells[1].Value + "" == "")
            {
                MessageBox.Show("please fill the MOHAP format before saving data");
                return;
            }
            if (ImgPathForEChannelMohapTextBoxI.Text == "")
            {
                MessageBox.Show("please add the required image");
                return;
            }
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            var op = new ChromeOptions();
            op.AddArguments("--user-data-dir=" + Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/AppData/Local/Google/Chrome/User Data");//C://Users/Riadh/
            op.AddArguments("--no-sandbox");
            op.AddArguments("--disable-dev-shm-usage");
            op.AddArguments("--disable-setuid-sandbox");
            var driver = new ChromeDriver(chromeDriverService, op, TimeSpan.FromSeconds(120));

            driver.Navigate().GoToUrl("https://smartform.mohap.gov.ae/MOHOnlinePortal/FitnessDetail.aspx");


            driver.FindElement(By.XPath("//input[@id='txtNameAr']")).SendKeys(EchanMohreDgview.Rows[2].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[@id='txtNameEn']")).SendKeys(EchanMohreDgview.Rows[3].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_UnifiedNumber')]")).SendKeys(EchanMohreDgview.Rows[5].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_ResidenceNumber')] ")).SendKeys(EchanMohreDgview.Rows[6].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_ResidenceIssueDate')] ")).SendKeys(EchanMohreDgview.Rows[7].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_ResidenceExpiryDate')]")).SendKeys(EchanMohreDgview.Rows[8].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_PassportNumber')] ")).SendKeys(EchanMohreDgview.Rows[9].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_PassportIssuePlace')]")).SendKeys(EchanMohreDgview.Rows[10].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_PassportIssueDate')]")).SendKeys(EchanMohreDgview.Rows[11].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_PassportExpiryDate')]")).SendKeys(EchanMohreDgview.Rows[12].Cells[1].Value + "");
            if (EchanMohreDgview.Rows[14].Cells[1].Value + "" == "FEMALE")
                driver.FindElement(By.XPath("//select[contains(@id,'txtGender')]")).SendKeys("أنثى");
            else
                driver.FindElement(By.XPath("//select[contains(@id,'txtGender')]")).SendKeys("ذكر");
            driver.FindElement(By.XPath("//input[contains(@id,'_BirthDate')]")).SendKeys(EchanMohreDgview.Rows[15].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_MobileNumber')]")).SendKeys(EchanMohreDgview.Rows[1].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_block_wtColumn4_wt224')]")).Click();
        }

        private void SaveFromEID_Click(object sender, EventArgs e)
        {

        }

        private void UploadImgMohreMohapB_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog { Filter = @"|*", InitialDirectory = _path };
            if (o.ShowDialog() == DialogResult.OK)
            {
                ImgPathForMohreMohapTextBoxI.Text = o.FileName;

            }
        }

        private void SaveFromMohrelMOHAPB_Click(object sender, EventArgs e)
        {
            if (MohreMohreDGV.Rows[2].Cells[1].Value + "" == "")
            {
                MessageBox.Show("please fill the MOHAP format before saving data");
                return;
            }
            if (ImgPathForMohreMohapTextBoxI.Text == "")
            {
                MessageBox.Show("please add the required image");
                return;
            }
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            var op = new ChromeOptions();
            op.AddArguments("--user-data-dir=" + Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/AppData/Local/Google/Chrome/User Data");//C://Users/Riadh/
            op.AddArguments("--no-sandbox");
            op.AddArguments("--disable-dev-shm-usage");
            op.AddArguments("--disable-setuid-sandbox");
            var driver = new ChromeDriver(chromeDriverService, op, TimeSpan.FromSeconds(120));

            driver.Navigate().GoToUrl("https://smartform.mohap.gov.ae/MOHOnlinePortal/FitnessDetail.aspx");


            driver.FindElement(By.XPath("//input[@id='txtNameAr']")).SendKeys(MohreMohreDGV.Rows[2].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[@id='txtNameEn']")).SendKeys(MohreMohreDGV.Rows[3].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_UnifiedNumber')]")).SendKeys(MohreMohreDGV.Rows[5].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_ResidenceNumber')] ")).SendKeys(MohreMohreDGV.Rows[6].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_ResidenceIssueDate')] ")).SendKeys(MohreMohreDGV.Rows[7].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_ResidenceExpiryDate')]")).SendKeys(MohreMohreDGV.Rows[8].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_PassportNumber')] ")).SendKeys(MohreMohreDGV.Rows[9].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_PassportIssuePlace')]")).SendKeys(MohreMohreDGV.Rows[10].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_PassportIssueDate')]")).SendKeys(MohreMohreDGV.Rows[11].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_PassportExpiryDate')]")).SendKeys(MohreMohreDGV.Rows[12].Cells[1].Value + "");
            if (MohreMohreDGV.Rows[14].Cells[1].Value + "" == "FEMALE")
                driver.FindElement(By.XPath("//select[contains(@id,'txtGender')]")).SendKeys("أنثى");
            else
                driver.FindElement(By.XPath("//select[contains(@id,'txtGender')]")).SendKeys("ذكر");
            driver.FindElement(By.XPath("//input[contains(@id,'_BirthDate')]")).SendKeys(MohreMohreDGV.Rows[15].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_MobileNumber')]")).SendKeys(MohreMohreDGV.Rows[1].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_block_wtColumn4_wt224')]")).Click();
        }

        private void UploadImgPermitMohapB_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog { Filter = @"|*", InitialDirectory = _path };
            if (o.ShowDialog() == DialogResult.OK)
            {
                ImgPathForPermitMohapTextBoxI.Text = o.FileName;

            }
        }

        private void SaveFromPermitMOHAPB_Click(object sender, EventArgs e)
        {
            if (PermitMOHAPDGV.Rows[2].Cells[1].Value + "" == "")
            {
                MessageBox.Show("please fill the MOHAP format before saving data");
                return;
            }
            if (ImgPathForPermitMohapTextBoxI.Text=="")
            {
                MessageBox.Show("please add the required image");
                return;
            }
          
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            var op = new ChromeOptions();
            op.AddArguments("--user-data-dir=" + Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/AppData/Local/Google/Chrome/User Data");//C://Users/Riadh/
            op.AddArguments("--no-sandbox");
            op.AddArguments("--disable-dev-shm-usage");
            op.AddArguments("--disable-setuid-sandbox");
            var driver = new ChromeDriver(chromeDriverService, op, TimeSpan.FromSeconds(120));


            driver.Navigate().GoToUrl("https://smartform.mohap.gov.ae/MOHOnlinePortal/FitnessDetail.aspx");


            driver.FindElement(By.XPath("//input[@id='txtNameAr']")).SendKeys(PermitMOHAPDGV.Rows[2].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[@id='txtNameEn']")).SendKeys(PermitMOHAPDGV.Rows[3].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_UnifiedNumber')]")).SendKeys(PermitMOHAPDGV.Rows[5].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_ResidenceNumber')] ")).SendKeys(PermitMOHAPDGV.Rows[6].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_ResidenceIssueDate')] ")).SendKeys(PermitMOHAPDGV.Rows[7].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_ResidenceExpiryDate')]")).SendKeys(PermitMOHAPDGV.Rows[8].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_PassportNumber')] ")).SendKeys(PermitMOHAPDGV.Rows[9].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_PassportIssuePlace')]")).SendKeys(PermitMOHAPDGV.Rows[10].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_PassportIssueDate')]")).SendKeys(PermitMOHAPDGV.Rows[11].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_PassportExpiryDate')]")).SendKeys(PermitMOHAPDGV.Rows[12].Cells[1].Value + "");
            if (MohreMohreDGV.Rows[14].Cells[1].Value + "" == "FEMALE")
                driver.FindElement(By.XPath("//select[contains(@id,'txtGender')]")).SendKeys("أنثى");
            else
                driver.FindElement(By.XPath("//select[contains(@id,'txtGender')]")).SendKeys("ذكر");
            driver.FindElement(By.XPath("//input[contains(@id,'_BirthDate')]")).SendKeys(PermitMOHAPDGV.Rows[15].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_MobileNumber')]")).SendKeys(PermitMOHAPDGV.Rows[1].Cells[1].Value + "");
            driver.FindElement(By.XPath("//input[contains(@id,'_block_wtColumn4_wt224')]")).Click();
        }

        private async void ScrapePermitB_ClickAsync(object sender, EventArgs e)
        {
            if (CodeT.Text == "")
            {
                MessageBox.Show("Please put the code wich you will scrape data with");
                return;
            }
            Display("");
            CleanPermitDataGridViews();
            var datas = new Dictionary<string, string>();
            var res = await HttpCaller.GetDoc($"http://eservices.mohre.gov.ae/NewMolGateway/english/Services/wpStatusMolMoi.aspx?Code={CodeT.Text}");
            if (res.error != null)
            {
                //ErrorLog(res.error);
                return;
            }
            var validityCode = res.doc.DocumentNode?.SelectSingleNode("//span[@id='lblMsg']").InnerText;
            if (validityCode.Contains("Not available"))
            {
                MessageBox.Show("This code is not available");
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

        public class KeyValue
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

    }
}
