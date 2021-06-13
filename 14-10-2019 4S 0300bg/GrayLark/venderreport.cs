using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Reddot_Express_Inventory.bin.Debug.Report;
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
    public partial class venderreport : Form
    {
        public venderreport(Allvender obj)
        {
            InitializeComponent();
             
            try
            { 
                ConnectionInfo crConnectionInfo;
                ReportDocument cryRpt = new ReportDocument();
                string Apppath = Path.GetDirectoryName(Application.ExecutablePath);
                Apppath = Path.Combine(Apppath, "Report\\Allvender.rpt");
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void venderreport_Load(object sender, EventArgs e)
        {

        }
    }
}
