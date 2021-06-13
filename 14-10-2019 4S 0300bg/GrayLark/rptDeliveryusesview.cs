using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrayLark
{
    public partial class rptDeliveryusesview : Form
    {
        public rptDeliveryusesview(Object obj,String qm)
        {
            InitializeComponent();
            SendEmail(obj,qm); 
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        } 
        private void SendEmail(Object obj,String q)
        { 
                CrystalDecisions.CrystalReports.Engine.Database crDatabase;
                TableLogOnInfo crTableLogonInfo;
                Tables crTables;
                ConnectionInfo crConnectionInfo;
                ReportDocument cryRpt = new ReportDocument();
                string Apppath = Path.GetDirectoryName(Application.ExecutablePath);
                Apppath = Path.Combine(Apppath, "Report\\DeliveryUsesReport.rpt");
                cryRpt.Load(Apppath); String v4 = "";
                String m = "select * from tbl_DeliveryUses where DCode ='" + q + "'";
                DataTable dr = clsDataLayer.RetreiveQuery(m);
                if (dr.Rows.Count > 0) { cryRpt.SetDataSource(dr); } 

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
                string path = Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf("Debug"));

                String name = "Delivery Uses " + DateTime.Now.ToString("dd-MM-yyyy") + q + DateTime.Now.ToString("MM_ss");
                path = Path.Combine(path, @"Debug\ " + name + ".pdf");
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                bool find = path.Contains(name); 
                cryRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);

         }
         


    }
}
