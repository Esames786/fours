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
using System.Data.SqlClient;

namespace GrayLark
{
    public partial class Purchase_Order_Preview : Form
    {
        public Purchase_Order_Preview(Object obj)
        {
            InitializeComponent();
            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();
        }

        private string InvoiceID;
        public string passInvoiceID
        {
            get { return InvoiceID; }
            set { InvoiceID = value; }
        }
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        private void Purchase_Order_Preview_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    Database crDatabase;
            //    TableLogOnInfo crTableLogonInfo;
            //    Tables crTables;
            //    ConnectionInfo crConnectionInfo;
            //    ReportDocument cryRpt = new ReportDocument();
            //    string Apppath = Path.GetDirectoryName(Application.ExecutablePath);
            //    Apppath = Path.Combine(Apppath, "Report\\PurchaseOrder.rpt");
            //    //cryRpt.Load(@"D:\My Project\Reddot Express Inventory\Reddot Express Inventory\PurchaseOrder.rpt");
            //    cryRpt.Load(Apppath);
            //    crConnectionInfo = new ConnectionInfo();
            //    string[] readConnectionConfig = File.ReadAllLines("ConnectionSetup.txt");
            //    crConnectionInfo.ServerName = readConnectionConfig[0];
            //    crConnectionInfo.DatabaseName = readConnectionConfig[1];
            //    crConnectionInfo.UserID = readConnectionConfig[2];
            //    crConnectionInfo.Password = readConnectionConfig[3];
            //    crDatabase = cryRpt.Database;
            //    crTables = crDatabase.Tables;
            //    foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
            //    {
            //        crTableLogonInfo = crTable.LogOnInfo;
            //        crTableLogonInfo.ConnectionInfo = crConnectionInfo;
            //        crTable.ApplyLogOnInfo(crTableLogonInfo);
            //    }
            //    ParameterFieldDefinitions crParameterFieldDefinitions;
            //    ParameterFieldDefinition crParameterFieldDefinition;
            //    ParameterValues crParameterValues = new ParameterValues();
            //    ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

            //    crParameterDiscreteValue.Value = InvoiceID;
            //    crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            //    crParameterFieldDefinition = crParameterFieldDefinitions["@P_ID"];
            //    crParameterValues = crParameterFieldDefinition.CurrentValues;
            //    crParameterValues.Clear();
            //    crParameterValues.Add(crParameterDiscreteValue);
            //    crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
            //    crystalReportViewer1.ReportSource = cryRpt;
            //    crystalReportViewer1.Refresh();

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}  
        }
    }
}
