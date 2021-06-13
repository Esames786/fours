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
using CrystalDecisions.Windows.Forms;
using System.IO;

namespace Reddot_Express_Inventory
{
    public partial class Replace_Return_Preview : Form
    {
        public Replace_Return_Preview()
        {
            InitializeComponent();
        }

        private string RepID;
        public string passRepID
        {
            get { return RepID; }
            set { RepID = value; }
        }

        private void Replace_Return_Preview_Load(object sender, EventArgs e)
        {
            try
            {
                Database crDatabase;
                TableLogOnInfo crTableLogonInfo;
                Tables crTables;
                ConnectionInfo crConnectionInfo;
                ReportDocument cryRpt = new ReportDocument();
                string Apppath = Path.GetDirectoryName(Application.ExecutablePath);
                Apppath = Path.Combine(Apppath, "Report\\VenderReplace.rpt");
                //cryRpt.Load(@"D:\My Project\Reddot Express Inventory\Reddot Express Inventory\PurchaseOrder.rpt");
                cryRpt.Load(Apppath);
                crConnectionInfo = new ConnectionInfo();
                string[] readConnectionConfig = File.ReadAllLines("ConnectionSetup.txt");
                crConnectionInfo.ServerName = readConnectionConfig[0];
                crConnectionInfo.DatabaseName = readConnectionConfig[1];
                crConnectionInfo.UserID = readConnectionConfig[2];
                crConnectionInfo.Password = readConnectionConfig[3];
                foreach (string o in readConnectionConfig)
                {
                    Console.WriteLine(o);
                }
                crDatabase = cryRpt.Database;
                crTables = crDatabase.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
                {
                    crTableLogonInfo = crTable.LogOnInfo;
                    crTableLogonInfo.ConnectionInfo = crConnectionInfo;
                    crTable.ApplyLogOnInfo(crTableLogonInfo);
                }
                ParameterFieldDefinitions crParameterFieldDefinitions;
                ParameterFieldDefinition crParameterFieldDefinition;
                ParameterValues crParameterValues = new ParameterValues();
                ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

                crParameterDiscreteValue.Value = RepID;
                crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
                crParameterFieldDefinition = crParameterFieldDefinitions["@OrderNo"];
                crParameterValues = crParameterFieldDefinition.CurrentValues;

                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

                crystalReportViewer1.ReportSource = cryRpt;

                crystalReportViewer1.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }
    }
}
