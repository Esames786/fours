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
    public partial class BalanceSheetViewer : Form
    {
        public BalanceSheetViewer(Object obj, decimal cash = 0, decimal ReceiveDue = 0, decimal PaymentDue = 0, decimal Inventory = 0, decimal Salary = 0, decimal Capital = 0, decimal Drawing = 0, decimal pnl = 0)
        {
       InitializeComponent();
       ParameterFields pfield = new ParameterFields();
       ParameterField ptitle = new ParameterField();
       ParameterDiscreteValue pvalue = new ParameterDiscreteValue();
       ptitle.ParameterFieldName = "CashInHand";
       pvalue.Value = cash;
       ptitle.CurrentValues.Add(pvalue);

       ParameterField pdtFrom = new ParameterField();
       ParameterDiscreteValue dtFromValue = new ParameterDiscreteValue();
       pdtFrom.ParameterFieldName = "ACReceiveable";
       dtFromValue.Value = ReceiveDue;
       pdtFrom.CurrentValues.Add(dtFromValue);

       ParameterField Dates = new ParameterField();
       ParameterDiscreteValue dtToValue = new ParameterDiscreteValue();
       Dates.ParameterFieldName = "ACPayable";
       dtToValue.Value = PaymentDue;
       Dates.CurrentValues.Add(dtToValue);

       ParameterField inventory = new ParameterField();
       ParameterDiscreteValue dtToValue2 = new ParameterDiscreteValue();
       inventory.ParameterFieldName = "Inventory";
       dtToValue2.Value = Inventory;
       inventory.CurrentValues.Add(dtToValue2);

            //
       ParameterField sp = new ParameterField();
       ParameterDiscreteValue dtToValue3 = new ParameterDiscreteValue();
       sp.ParameterFieldName = "SalaryPayable";
       dtToValue3.Value = Salary;
       sp.CurrentValues.Add(dtToValue3);

       ParameterField Capitals = new ParameterField();
       ParameterDiscreteValue dtToValue4 = new ParameterDiscreteValue();
       Capitals.ParameterFieldName = "PCapital";
       dtToValue4.Value = Capital;
       Capitals.CurrentValues.Add(dtToValue4);

       ParameterField c2 = new ParameterField();
       ParameterDiscreteValue s2 = new ParameterDiscreteValue();
       c2.ParameterFieldName = "Drawing";
       s2.Value = Drawing;
       c2.CurrentValues.Add(s2);

       ParameterField c4 = new ParameterField();
       ParameterDiscreteValue s4 = new ParameterDiscreteValue();
       c4.ParameterFieldName = "pnl";
       s4.Value = pnl;
       c4.CurrentValues.Add(s4);

            //
       pfield.Add(c2); pfield.Add(c4);
       pfield.Add(ptitle); pfield.Add(pdtFrom);
       pfield.Add(Dates);  pfield.Add(inventory);
       pfield.Add(Capitals);   pfield.Add(sp);
             
       crystalReportViewer1.ParameterFieldInfo = pfield;
       crystalReportViewer1.ReportSource = obj;
       crystalReportViewer1.Refresh();
        }

        private void BalanceSheetViewer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
