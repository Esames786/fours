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
    public partial class product_kitchen_view : Form
    {


        public product_kitchen_view(Object obj, String dtFrom, String dtTo)
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

            pfield.Add(pdtFrom);
            pfield.Add(Dates);
            crystalReportViewer1.ParameterFieldInfo = pfield;
            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();
        }

      

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
