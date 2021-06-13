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
    public partial class Find : Form
    {
        String table = "";
        String fname = "";
        String colname = "";
        public Find(String tblName,String form)
        {
            InitializeComponent();
            table = tblName;
            fname = form;
            Disable();
            txtsearch.Enabled = true;
        }

        private bool Enable()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Enabled == false)
                    {
                        ((TextBox)c).Enabled = true;
                        flag = true;

                    }
                }
                else if (c is ComboBox)
                {
                    {
                        if (((ComboBox)c).Enabled == false)
                        {
                            ((ComboBox)c).Enabled = true;
                            flag = true;

                        }
                    }
                }
                else if (c is MaskedTextBox)
                {
                    {
                        if (((MaskedTextBox)c).Enabled == false)
                        {
                            ((MaskedTextBox)c).Enabled = true;
                            flag = true;

                        }
                    }
                }
            }
            return flag;
        }
        private bool Disable()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Enabled == true)
                    {
                        ((TextBox)c).Enabled = false;
                        flag = true;

                    }
                }
                else if (c is ComboBox)
                {
                    {
                        if (((ComboBox)c).Enabled == true)
                        {
                            ((ComboBox)c).Enabled = false;
                            flag = true;

                        }
                    }
                }
            }
            return flag;
        }
        private bool Clears()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Text != "")
                    {
                        ((TextBox)c).Text = "";
                        flag = true;
                        break;
                    }
                }
                else if (c is ComboBox)
                {
                    {
                        if (((ComboBox)c).Text != "")
                        {
                            ((ComboBox)c).Text = "";
                            flag = true;
                            break;
                        }
                    }
                }
            }
            return flag;
        }


        private void txtsearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(txtsearch.SelectedIndex == 0)
            {
                //VoucherNo
                Disable();
                txtsearch.Enabled = true;
                txtvno.Enabled = true;
                String search = "";
                if (table.Equals("Transactions"))
                {
                    search = "select TransId from Transactions";
                    colname = "TransId";
                }
                else if (table.Equals("Transactionss"))
                {
                    search = "select TransId from Transactionss";
                    colname = "TransId";
                }
                else if (table.Equals("Transactions"))
                {

                }

                DataTable dt = clsDataLayer.RetreiveQuery(search);
                if (dt.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtvno, dt);
                }
            }
            else if (txtsearch.SelectedIndex == 1)
            {
                //PartyName
                Disable();
                txtsearch.Enabled = true;
                txtname.Enabled = true;
                String search = "";
                if (table.Equals("Transactions"))
                {
                    search = "select PartyName from Transactions";
                    colname = "PartyName";
                }
                else if (table.Equals("Transactionss"))
                {
                    search = "select PartyName from Transactionss";
                    colname = "PartyName";
                }
                DataTable dt = clsDataLayer.RetreiveQuery(search);
                if (dt.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtname, dt);
                }
            }
            else if (txtsearch.SelectedIndex == 2)
            {
                //Amount
                Disable();
                txtsearch.Enabled = true;
                txtamount.Enabled = true;
                String search = "";
                if (table.Equals("Transactions"))
                {
                    search = "select CashGiven from Transactions";
                    colname = "CashGiven";
                }
                else if (table.Equals("Transactionss"))
                {
                    search = "select CashGiven from Transactionss";
                    colname = "CashGiven";
                }
                DataTable dt = clsDataLayer.RetreiveQuery(search);
                if (dt.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtamount, dt);
                }
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            Clears();
        }

        String search = "";
        private void btnsearch_Click(object sender, EventArgs e)
        {
            if (txtsearch.SelectedIndex == 0)
            {
                //VoucherNo
                if (table.Equals("Transactions"))
                {
                    search = "  select * from Transactions where "+colname+" = '"+txtvno.Text+"'  ";
                }
                else if (table.Equals("Transactionss"))
                {
                    search = "  select * from Transactionss where " + colname + " = '" + txtvno.Text + "'  ";
                }
                else if (table.Equals("Transactions"))
                {

                }
           
            }
            else if (txtsearch.SelectedIndex == 1)
            {
                //PartyName
               
                if (table.Equals("Transactions"))
                {
                    search = "  select * from Transactions where " + colname + " = '" + txtname.Text + "'  ";
                }
                else if (table.Equals("Transactionss"))
                {
                    search = "  select * from Transactionss where " + colname + " = '" + txtname.Text + "'  ";
                }
                else if (table.Equals("Transactions"))
                {

                }
            }
            else if (txtsearch.SelectedIndex == 2)
            {
                //Amount
                if (table.Equals("Transactions"))
                {
                    search = "  select * from Transactions where " + colname + " = " + txtamount.Text + " ";
                }
                else if (table.Equals("Transactionss"))
                {
                    search = "  select * from Transactionss where " + colname + " = " + txtamount.Text + "  ";
                }
                else if (table.Equals("Transactions"))
                {

                }
            }

            DataTable dt = clsDataLayer.RetreiveQuery(search);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
          String Id =  dataGridView1.CurrentRow.Cells[0].Value.ToString();
          if (fname.Equals("Payment_voucher"))
          {
              this.Hide();
              Payment_voucher pay = new Payment_voucher();
              pay.txtInvoice.Text = Id;
              pay.Show();
         }

        }

       

      
    }
}
