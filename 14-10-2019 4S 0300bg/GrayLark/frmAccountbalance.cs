using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GrayLark
{
    public partial class frmAccountbalance : Form
    {
        public frmAccountbalance(Object objRpt)
        {
            InitializeComponent();
            ParameterFields pfield = new ParameterFields();

            String gets = "select CASH from tech_cash where CompanyName='Delizia'";
            DataTable dd = clsDataLayer.RetreiveQuery(gets);

            decimal a = 0; decimal b = 0; decimal c = 0; decimal d = 0; decimal e = 0;


            if (dd.Rows.Count > 0)
            {
                a = Convert.ToDecimal(dd.Rows[0]["CASH"].ToString());

            }
            else
            {
                a = 0; b = 0; c = 0; d = 0; e = 0;
            }

            ParameterField ptitles = new ParameterField();
            ParameterDiscreteValue pvalues = new ParameterDiscreteValue();
            ptitles.ParameterFieldName = "techcash";
            pvalues.Value = a.ToString();
            ptitles.CurrentValues.Add(pvalues);
            pfield.Add(ptitles);
             
            crystalReportViewer1.ParameterFieldInfo = pfield;
            crystalReportViewer1.ReportSource = objRpt;
            crystalReportViewer1.Refresh();
        }

        private void frmAccountbalance_Load(object sender, EventArgs e)
        {

        }
    }
}