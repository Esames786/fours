using CrystalDecisions.Shared;
using Reddot_Express_Inventory.bin.Debug.Report;
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
    public partial class InvoiceView : Form
    {

        public InvoiceView(Object obj, String name)
        {
            InitializeComponent();

             name = Login.UserID;

            ParameterFields pfield = new ParameterFields();
            ParameterField ptitle = new ParameterField();
            ParameterDiscreteValue pvalue = new ParameterDiscreteValue();
            ptitle.ParameterFieldName = "User";
            pvalue.Value = Login.UserID;
            ptitle.CurrentValues.Add(pvalue);

            pfield.Add(ptitle);
            crystalReportViewer1.ParameterFieldInfo = pfield;
            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();

         
        }
    }
}
