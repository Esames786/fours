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
    public partial class RedProfit : Form
    {
        public RedProfit(String Sales, String cp, String fbadd, String cexp, Object obj,String Salary)
        {
            InitializeComponent();
            ParameterFields pfield = new ParameterFields();
            ParameterField ptitle = new ParameterField();
            ParameterDiscreteValue pvalue = new ParameterDiscreteValue();
            ptitle.ParameterFieldName = "SALES";
            pvalue.Value = Sales;
            ptitle.CurrentValues.Add(pvalue);

            //
            ParameterField pdtFrom5 = new ParameterField();
            ParameterDiscreteValue dtFromValue5 = new ParameterDiscreteValue();
            pdtFrom5.ParameterFieldName = "SE";
            dtFromValue5.Value = cp;
            pdtFrom5.CurrentValues.Add(dtFromValue5);
            //

            ParameterField pdtFrom = new ParameterField();
            ParameterDiscreteValue dtFromValue = new ParameterDiscreteValue();
            pdtFrom.ParameterFieldName = "cp";
            dtFromValue.Value = cp;
            pdtFrom.CurrentValues.Add(dtFromValue);

            ParameterField Dates = new ParameterField();
            ParameterDiscreteValue dtToValue = new ParameterDiscreteValue();
            Dates.ParameterFieldName = "FbAdd";
            dtToValue.Value = fbadd;
            Dates.CurrentValues.Add(dtToValue);

            ParameterField Dates5 = new ParameterField();
            ParameterDiscreteValue dtToValue5 = new ParameterDiscreteValue();
            Dates5.ParameterFieldName = "CExp";
            dtToValue5.Value = cexp;
            Dates5.CurrentValues.Add(dtToValue5);
             
            pfield.Add(ptitle);
            pfield.Add(pdtFrom5);
            pfield.Add(pdtFrom);
            pfield.Add(Dates);
            pfield.Add(Dates5);

            crystalReportViewer1.ParameterFieldInfo = pfield;

            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();
        }
    }
}
