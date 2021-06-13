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
    public partial class Form3 : Form
    {
        public Form3(Object obj,String fromDate,String ToDate,String Total,String Dis,String CExp ,String FAdd)
        {
            InitializeComponent();

            ParameterFields pfield = new ParameterFields(); 

            //

            ParameterField pdtFrom5 = new ParameterField();
            ParameterDiscreteValue dtFromValue5 = new ParameterDiscreteValue();
            pdtFrom5.ParameterFieldName = "CExp";
            dtFromValue5.Value = CExp;
            pdtFrom5.CurrentValues.Add(dtFromValue5);



            //
            ParameterField pdtFrom7 = new ParameterField();
            ParameterDiscreteValue dtFromValue7 = new ParameterDiscreteValue();
            pdtFrom7.ParameterFieldName = "FbAdd";
            dtFromValue7.Value = FAdd;
            pdtFrom7.CurrentValues.Add(dtFromValue7);
            //

            //




            ParameterField pdtFrom = new ParameterField();
            ParameterDiscreteValue dtFromValue = new ParameterDiscreteValue();
            pdtFrom.ParameterFieldName = "DateFrom";
            dtFromValue.Value = fromDate;
            pdtFrom.CurrentValues.Add(dtFromValue);

        
              
            //
            ParameterField pdtFrom3 = new ParameterField();
            ParameterDiscreteValue dtFromValue3 = new ParameterDiscreteValue();
            pdtFrom3.ParameterFieldName = "Discount";
            dtFromValue3.Value = Dis;
            pdtFrom3.CurrentValues.Add(dtFromValue3);
            //


            ParameterField Dates = new ParameterField();
            ParameterDiscreteValue dtToValue = new ParameterDiscreteValue();
            Dates.ParameterFieldName = "DateTo";
            dtToValue.Value = ToDate;
            Dates.CurrentValues.Add(dtToValue);

            ParameterField Dates3 = new ParameterField();
            ParameterDiscreteValue dtToValue3 = new ParameterDiscreteValue();
            Dates3.ParameterFieldName = "TotalPNL";
            dtToValue3.Value = Total;
            Dates3.CurrentValues.Add(dtToValue3);


            pfield.Add(pdtFrom5);
            pfield.Add(pdtFrom7);

            pfield.Add(pdtFrom3);
            pfield.Add(pdtFrom);
            pfield.Add(Dates);
            pfield.Add(Dates3);

            crystalReportViewer1.ParameterFieldInfo = pfield;
             

            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
