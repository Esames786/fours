using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System.IO; 

namespace GrayLark
{
    public partial class Sale_Preview : Form
    {
        public Sale_Preview(Object obj)
        {
            InitializeComponent();
            ConnectionInfo crConnectionInfo;
            ReportDocument cryRpt = new ReportDocument();
            string Apppath = Path.GetDirectoryName(Application.ExecutablePath);
            Apppath = Path.Combine(Apppath, "Report\\DeliveryReport.rpt");
            cryRpt.Load(Apppath);
            crConnectionInfo = new ConnectionInfo();
            string[] readConnectionConfig = File.ReadAllLines("ConnectionSetup.txt");
            crConnectionInfo.ServerName = readConnectionConfig[0];
            crConnectionInfo.DatabaseName = readConnectionConfig[1];
            crConnectionInfo.UserID = readConnectionConfig[2];
            crConnectionInfo.Password = readConnectionConfig[3];
            crystalReportViewer1.ReportSource = obj;

            crystalReportViewer1.Refresh();

        }

        private string SellID;
        public string passSellID
        {
            get { return SellID; }
            set { SellID = value; }
        }

        private void Sale_Preview_Load(object sender, EventArgs e)
        { 
        }
    }
}
