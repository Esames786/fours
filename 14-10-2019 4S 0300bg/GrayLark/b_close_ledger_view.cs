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

namespace Reddot_Express_Inventory
{
    public partial class b_close_ledger_view : Form
    {
        public b_close_ledger_view(Object rpt, String To, String From)
        {
            InitializeComponent();


            ParameterFields pfield = new ParameterFields();


            ParameterField pdtFrom = new ParameterField();
            ParameterDiscreteValue dtFromValue = new ParameterDiscreteValue();
            pdtFrom.ParameterFieldName = "From";
            dtFromValue.Value = From;
            pdtFrom.CurrentValues.Add(dtFromValue);

            ParameterField Dates = new ParameterField();
            ParameterDiscreteValue dtToValue = new ParameterDiscreteValue();
            Dates.ParameterFieldName = "To";
            dtToValue.Value = To;
            Dates.CurrentValues.Add(dtToValue);

            pfield.Add(pdtFrom);
            pfield.Add(Dates);
            crystalReportViewer1.ParameterFieldInfo = pfield;


            crystalReportViewer1.ReportSource = rpt;
            crystalReportViewer1.Refresh();
        }

        private void b_close_ledger_view_Load(object sender, EventArgs e)
        {

        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
