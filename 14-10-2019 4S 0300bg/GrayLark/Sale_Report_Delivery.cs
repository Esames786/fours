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
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
using System.IO;

namespace Reddot_Express_Inventory
{
    public partial class Sale_Report_Delivery : Form
    {
        public Sale_Report_Delivery()
        {
            InitializeComponent();
        }

        private void Sale_Report_Delivery_Load(object sender, EventArgs e)
        {
            try
            {
                Database crDatabase;
                TableLogOnInfo crTableLogonInfo;
                Tables crTables;
                ConnectionInfo crConnectionInfo;
                ReportDocument cryRpt = new ReportDocument();
                string Apppath = Path.GetDirectoryName(Application.ExecutablePath);
                Apppath = Path.Combine(Apppath, "Report\\DeliverySaleReport.rpt");
                cryRpt.Load(Apppath);
                crystalReportViewer1.ReportSource = cryRpt;
                crConnectionInfo = new ConnectionInfo();
                crConnectionInfo.ServerName = "REDDOTGROUP\\REDDOT";
                crConnectionInfo.DatabaseName = "Reddot_Inventory";
                crConnectionInfo.UserID = "sa";
                crConnectionInfo.Password = "Red123";
                //crConnectionInfo.IntegratedSecurity = false;
                crDatabase = cryRpt.Database;
                crTables = crDatabase.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
                {
                    crTableLogonInfo = crTable.LogOnInfo;
                    crTableLogonInfo.ConnectionInfo = crConnectionInfo;
                    crTable.ApplyLogOnInfo(crTableLogonInfo);
                }
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
