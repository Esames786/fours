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
using System.IO;
using GrayLark.bin.Debug.Report;

namespace GrayLark
{
    public partial class Daily_Sale_Report : Form
    {
        public Daily_Sale_Report(String dtFrom, String dtTo, Object obj)
        {
            InitializeComponent();
            ParameterFields pfield = new ParameterFields();
            ParameterField ptitle = new ParameterField();
            ParameterDiscreteValue pvalue = new ParameterDiscreteValue();
            ptitle.ParameterFieldName = "username";
            pvalue.Value = Login.UserID;
            ptitle.CurrentValues.Add(pvalue);

            ParameterField pdtFrom = new ParameterField();
            ParameterDiscreteValue dtFromValue = new ParameterDiscreteValue();
            pdtFrom.ParameterFieldName = "fromdate";
            dtFromValue.Value = dtFrom;
            pdtFrom.CurrentValues.Add(dtFromValue);

            //
         
            //

            ParameterField Dates = new ParameterField();
            ParameterDiscreteValue dtToValue = new ParameterDiscreteValue();
            Dates.ParameterFieldName = "todate";
            dtToValue.Value = dtTo;
            Dates.CurrentValues.Add(dtToValue);

            pfield.Add(ptitle);
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
