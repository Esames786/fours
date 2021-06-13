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
 
using System.IO;

namespace GrayLark
{
    public partial class Return_Order_Preview : Form
    {
        public Return_Order_Preview(Object obj)
        {
            InitializeComponent();
            
            crystalReportViewer1.ReportSource = obj;

            crystalReportViewer1.Refresh();
        }

        private string RetSellID;
        public string passRetSellID
        {
            get { return RetSellID; }
            set { RetSellID = value; }
        }

        private void Return_Order_Preview_Load(object sender, EventArgs e)
        {
            try
            {
                //Database crDatabase;
                //TableLogOnInfo crTableLogonInfo;
                //Tables crTables;
                //ConnectionInfo crConnectionInfo;
                //ReportDocument cryRpt = new ReportDocument();
                //string Apppath = Path.GetDirectoryName(Application.ExecutablePath);
                //Apppath = Path.Combine(Apppath, "Report\\ReturnOrder.rpt");
                ////cryRpt.Load(@"D:\My Project\Reddot Express Inventory\Reddot Express Inventory\PurchaseOrder.rpt");
                //cryRpt.Load(Apppath);
                //crConnectionInfo = new ConnectionInfo();
                //string[] con = File.ReadAllLines("ConnectionSetup.txt");

                //crConnectionInfo.ServerName = con[0];
                //crConnectionInfo.DatabaseName = con[1];
                //crConnectionInfo.UserID = con[2];
                //crConnectionInfo.Password = con[3];
                //crDatabase = cryRpt.Database;
                //crTables = crDatabase.Tables;
                //foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
                //{
                //    crTableLogonInfo = crTable.LogOnInfo;
                //    crTableLogonInfo.ConnectionInfo = crConnectionInfo;
                //    crTable.ApplyLogOnInfo(crTableLogonInfo);
                //}
                //ParameterFieldDefinitions crParameterFieldDefinitions;
                //ParameterFieldDefinition crParameterFieldDefinition;
                //ParameterValues crParameterValues = new ParameterValues();
                //ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

                //crParameterDiscreteValue.Value = RetSellID;
                //crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
                //crParameterFieldDefinition = crParameterFieldDefinitions["@SaleID"];
                //crParameterValues = crParameterFieldDefinition.CurrentValues;

                //crParameterValues.Clear();
                //crParameterValues.Add(crParameterDiscreteValue);
                //crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

          

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }
    }
}
