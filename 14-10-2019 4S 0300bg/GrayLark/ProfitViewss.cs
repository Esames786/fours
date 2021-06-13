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
    public partial class ProfitViewss : Form
    {
        public ProfitViewss(String sal, String pur,String From, String To, Object obj)
        {
            InitializeComponent(); 
            ParameterFields pfield = new ParameterFields();
            ParameterField ptitle = new ParameterField();
            ParameterDiscreteValue pvalue = new ParameterDiscreteValue();
            ptitle.ParameterFieldName = "SALES";
            pvalue.Value = sal;
            ptitle.CurrentValues.Add(pvalue);

            ParameterField pdtFrom = new ParameterField();
            ParameterDiscreteValue dtFromValue = new ParameterDiscreteValue();
            pdtFrom.ParameterFieldName = "Purchase";
            dtFromValue.Value = pur;
            pdtFrom.CurrentValues.Add(dtFromValue); 
            // 

            ParameterField a3 = new ParameterField();
            ParameterDiscreteValue b3 = new ParameterDiscreteValue();
            a3.ParameterFieldName = "From";
            b3.Value = From;
            a3.CurrentValues.Add(b3);

            ParameterField a2 = new ParameterField();
            ParameterDiscreteValue d4 = new ParameterDiscreteValue();
            a2.ParameterFieldName = "To";
            d4.Value = To;
            a2.CurrentValues.Add(d4);

            //
            pfield.Add(a3);
            pfield.Add(a2);

            pfield.Add(ptitle);
            pfield.Add(pdtFrom); 
            crystalReportViewer1.ParameterFieldInfo = pfield;

            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();
        }

        private void ProfitView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
