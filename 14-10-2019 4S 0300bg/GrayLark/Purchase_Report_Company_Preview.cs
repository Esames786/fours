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
    public partial class Purchase_Report_Company_Preview : Form
    {
        public Purchase_Report_Company_Preview(String vender, String FROM, String TO, Object obj)
        {
            InitializeComponent();
            try
              {

                  ConnectionInfo crConnectionInfo;
                  ReportDocument cryRpt = new ReportDocument();
                  string Apppath = Path.GetDirectoryName(Application.ExecutablePath);
                  Apppath = Path.Combine(Apppath, "Report\\PurchaseSale.rpt");
                  cryRpt.Load(Apppath);

                  crConnectionInfo = new ConnectionInfo();
                  string[] readConnectionConfig = File.ReadAllLines("ConnectionSetup.txt");
                  crConnectionInfo.ServerName = readConnectionConfig[0];
                  crConnectionInfo.DatabaseName = readConnectionConfig[1];
                  crConnectionInfo.UserID = readConnectionConfig[2];
                  crConnectionInfo.Password = readConnectionConfig[3];
                  //  crConnectionInfo.IntegratedSecurity = false;

                ParameterFields pfield = new ParameterFields();


                ParameterField Vender = new ParameterField();
                ParameterDiscreteValue v = new ParameterDiscreteValue();
                Vender.ParameterFieldName = "Vender";
                v.Value = vender;
                Vender.CurrentValues.Add(v);

                ParameterField from = new ParameterField();
                ParameterDiscreteValue f = new ParameterDiscreteValue();
                from.ParameterFieldName = "From";
                f.Value = FROM;
                from.CurrentValues.Add(f);


                ParameterField to = new ParameterField();
                ParameterDiscreteValue t = new ParameterDiscreteValue();
                to.ParameterFieldName = "To";
                t.Value = TO;
                to.CurrentValues.Add(t);

                pfield.Add(Vender);
                pfield.Add(from);
                pfield.Add(to);

                crystalReportViewer1.ParameterFieldInfo = pfield;
                crystalReportViewer1.ReportSource = obj;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Purchase_Report_Company_Preview_Load(object sender, EventArgs e)
        {
           
        }

        private void Purchase_Report_Company_Preview_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
