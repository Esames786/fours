using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 
using System.Data.SqlClient;

namespace GrayLark
{
    public partial class InvoiceSearch : Form
    {
        Payment_voucher pv; String p1;
        public InvoiceSearch(Payment_voucher pv1,String Party)
        {
            InitializeComponent();
            pv = pv1; p1 = Party;
            load();
        }

        private void load()
        {
            String query = "select distinct VendorName from Purchase";
        DataTable dtt = clsDataLayer.RetreiveQuery(query);
        if (dtt.Rows.Count > 0)
        {
            clsGeneral.SetAutoCompleteTextBox(txtvendor, dtt);
        }
        try
        {
            String a1 = "select PurCode,VendorName,Date,TotalCPTCost,PaidAmount,RemainingAmount from Purchase where RemainingAmount!=0 and VendorName='" + p1 + "'";
            DataTable d1 = clsDataLayer.RetreiveQuery(a1); if (d1.Rows.Count > 0)
            {
                dataGridView1.DataSource = d1;
            }
        }
        catch { }
        }

        private void txtvendor_TextChanged(object sender, EventArgs e)
        {
        try
        {
            String a1 = "select PurCode,VendorName,Date,TotalCPTCost,PaidAmount,RemainingAmount from Purchase where RemainingAmount!=0 and VendorName='" + txtvendor.Text + "'";
        DataTable d1 = clsDataLayer.RetreiveQuery(a1); if (d1.Rows.Count > 0)
        {
            dataGridView1.DataSource = d1;
        }
        }
        catch{}
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        try
        {
            pv.txtpo.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); pv.txtCashRelease.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            this.Close();
        } catch { }
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space)
            {
                pv.txtpo.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); pv.txtCashRelease.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                this.Close();
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
      
        }

        private void txtvendor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

    }
}
