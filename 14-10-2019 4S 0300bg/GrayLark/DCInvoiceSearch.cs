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
    public partial class DCInvoiceSearch : Form
    {
        Receipt_voucher pv; String p1;
        public DCInvoiceSearch(Receipt_voucher pv1, String Party)
        {
            InitializeComponent();
            pv = pv1; p1 = Party;
            load();
        }

        private void load()
        {
            String tbl2 = "";
            if (checkBox1.Checked)
            {
                tbl2 = "tblnInvoice";
            }
            else
            {
                tbl2 = "tblInvoice";
            }
            String query = "select distinct InstituteName from "+tbl2+" where status='Invoice' and RemainingAmount!=0";
        DataTable dtt = clsDataLayer.RetreiveQuery(query);
        if (dtt.Rows.Count > 0)
        {
            clsGeneral.SetAutoCompleteTextBox(txtvendor, dtt);
        }
        try
        {
            String a1 = "select InCode,InstituteName,InvoiceDate,InvoiceAmount,ReceiveAmount,RemainingAmount from " + tbl2 + " where RemainingAmount!=0 and InstituteName='" + p1 + "'";
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
            String tbl2 = "";
            if (checkBox1.Checked)
            {
                tbl2 = "tblnInvoice";
            }
            else
            {
                tbl2 = "tblInvoice";
            }
            String a1 = "select InCode,InstituteName,InvoiceDate,InvoiceAmount,ReceiveAmount,RemainingAmount from "+tbl2+" where RemainingAmount!=0 and InstituteName='" + txtvendor.Text + "'";
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
