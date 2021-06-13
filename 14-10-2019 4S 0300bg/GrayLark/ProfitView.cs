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
    public partial class ProfitView : Form
    {
        public ProfitView(String PNL, Object obj,String From,String To)
        {
            InitializeComponent();
            ParameterFields pfield = new ParameterFields();
            ParameterField ptitle = new ParameterField();
            ParameterDiscreteValue pvalue = new ParameterDiscreteValue();
            ptitle.ParameterFieldName = "PNL";
            pvalue.Value = PNL;
            ptitle.CurrentValues.Add(pvalue);


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
             
            pfield.Add(ptitle); 
            crystalReportViewer1.ParameterFieldInfo = pfield;

            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();
        }
    }
}
