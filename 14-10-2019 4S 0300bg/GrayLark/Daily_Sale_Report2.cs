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
using Reddot_Express_Inventory.bin.Debug.Report;

namespace Reddot_Express_Inventory
{
    public partial class Daily_Sale_Report2 : Form
    {
        public Daily_Sale_Report2(String dtFrom, String dtTo, Object obj,decimal Paid)
        {
            InitializeComponent();
            ParameterFields pfield = new ParameterFields();
 

            ParameterField pdtFrom = new ParameterField();
            ParameterDiscreteValue dtFromValue = new ParameterDiscreteValue();
            pdtFrom.ParameterFieldName = "FromDate";
            dtFromValue.Value = dtFrom;
            pdtFrom.CurrentValues.Add(dtFromValue);

            ParameterField Dates = new ParameterField();
            ParameterDiscreteValue dtToValue = new ParameterDiscreteValue();
            Dates.ParameterFieldName = "ToDate";
            dtToValue.Value = dtTo;
            Dates.CurrentValues.Add(dtToValue);

            ParameterField c1 = new ParameterField();
            ParameterDiscreteValue f1 = new ParameterDiscreteValue();
            c1.ParameterFieldName = "Piad";
            f1.Value = Paid;
            c1.CurrentValues.Add(f1);

            pfield.Add(c1);
            pfield.Add(pdtFrom);
            pfield.Add(Dates);
            crystalReportViewer1.ParameterFieldInfo = pfield;
            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();
        }

        private void Daily_Sale_Report_Load(object sender, EventArgs e)
        { 
        }
    }
}
