using GrayLark.bin.Debug.Report;
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
    public partial class ExpenseForm : Form
    {
        String q1 = "";
        public ExpenseForm(String q)
        {
            InitializeComponent();
            q1 = q; Show(q1);        
        }

        private void Show(String qs)
        {
            String query = qs; DataTable ds = clsDataLayer.RetreiveQuery(query);
            if (ds.Rows.Count > 0)
            {
                grid2.DataSource = ds;
            }
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
        try
        {
        String pr = q1; DataTable ds = clsDataLayer.RetreiveQuery(pr);
        if (ds.Rows.Count > 0)
        {
            rptPaymentTr rpt = new rptPaymentTr(); rpt.SetDataSource(ds); PaymentView pv = new PaymentView(rpt); pv.Show();
        }
        else { MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        } catch { }
        }

    }
}
