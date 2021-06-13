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
    public partial class CashTransView : Form
    {
        public CashTransView(String dtFrom, String dtTo, Object obj,String Cashin,String CashRel)
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

            // 
            ParameterField pdtFrom4 = new ParameterField();
            ParameterDiscreteValue dtFromValue4 = new ParameterDiscreteValue();
            pdtFrom4.ParameterFieldName = "TCash";
            dtFromValue4.Value = Cashin;
            pdtFrom4.CurrentValues.Add(dtFromValue4);

            ParameterField Dates4 = new ParameterField();
            ParameterDiscreteValue dtToValue4 = new ParameterDiscreteValue();
            Dates4.ParameterFieldName = "Tcashrelease";
            dtToValue4.Value = CashRel;
            Dates4.CurrentValues.Add(dtToValue4);

            //

            pfield.Add(ptitle);
            pfield.Add(pdtFrom);
            pfield.Add(Dates);

            pfield.Add(pdtFrom4);
            pfield.Add(Dates4);

            crystalReportViewer1.ParameterFieldInfo = pfield;

            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();
        }
    }
}
