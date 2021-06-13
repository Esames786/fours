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
    public partial class LedgerView : Form
    {
        public LedgerView(String dtFrom, String dtTo,Object obj)
        {
            InitializeComponent();
            ParameterFields pfield = new ParameterFields();
            ParameterField ptitle = new ParameterField();
            ParameterDiscreteValue pvalue = new ParameterDiscreteValue();
            ptitle.ParameterFieldName = "UserName";
            pvalue.Value = Login.UserID;
            ptitle.CurrentValues.Add(pvalue);

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

            pfield.Add(ptitle);
            crystalReportViewer1.ParameterFieldInfo = pfield;

            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();
        }
    }
}
