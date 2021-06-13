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
    public partial class UpdFurnView : Form
    {
        public UpdFurnView(String dtFrom, String dtTo, Object obj)
        {
            InitializeComponent();
            ParameterFields pfield = new ParameterFields();
       

            ParameterField pdtFrom = new ParameterField();
            ParameterDiscreteValue dtFromValue = new ParameterDiscreteValue();
            pdtFrom.ParameterFieldName = "From";
            dtFromValue.Value = dtFrom;
            pdtFrom.CurrentValues.Add(dtFromValue);

            ParameterField Dates = new ParameterField();
            ParameterDiscreteValue dtToValue = new ParameterDiscreteValue();
            Dates.ParameterFieldName = "To";
            dtToValue.Value = dtTo;
            Dates.CurrentValues.Add(dtToValue);

            pfield.Add(pdtFrom);
            pfield.Add(Dates);

             crystalReportViewer1.ParameterFieldInfo = pfield;

            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();
        }
    }
}
