using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reddot_Express_Inventory
{
    public partial class CcInvoiceView : Form
    {
        public CcInvoiceView()
        {
            InitializeComponent();
            CrystalDecisions.CrystalReports.Engine.Database crDatabase;
            TableLogOnInfo crTableLogonInfo;
            Tables crTables;
            ConnectionInfo crConnectionInfo;
            ReportDocument cryRpt = new ReportDocument();
            string Apppath = Path.GetDirectoryName(Application.ExecutablePath);
            Apppath = Path.Combine(Apppath, "Report\\CreditCardInvoice.rpt");
            cryRpt.Load(Apppath);
            crConnectionInfo = new ConnectionInfo();
            string[] con = File.ReadAllLines("ConnectionSetup.txt");
            crConnectionInfo.ServerName = con[0];
            crConnectionInfo.DatabaseName = con[1];
            crConnectionInfo.UserID = con[2];
            crConnectionInfo.Password = con[3];
            crDatabase = cryRpt.Database;
            crTables = crDatabase.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
            {
                crTableLogonInfo = crTable.LogOnInfo;
                crTableLogonInfo.ConnectionInfo = crConnectionInfo;
                crTable.ApplyLogOnInfo(crTableLogonInfo);
            }
            crystalReportViewer1.ReportSource = cryRpt;

            crystalReportViewer1.Refresh();
        }
    }
}
