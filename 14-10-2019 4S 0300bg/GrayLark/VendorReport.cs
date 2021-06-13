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
    public partial class VendorReport : Form
    {
        String Code = "";
        public VendorReport()
        {
            InitializeComponent();
            Disable();
            date1.Enabled = false;
            date2.Enabled = false;
            String query = "select ActTitle from Accounts where ID > 24 and Status=1";
            DataTable d = clsDataLayer.RetreiveQuery(query);
            if(d.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txtcustomer, d);
            } Enable(); txtreports.Text = "LedgerPayment"; date1.Focus();
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
                        if (((ComboBox)c).Enabled == false)
                        {
                            ((ComboBox)c).Enabled = true;
                            flag = true;

                        }
                 }
                else if (c is MaskedTextBox)
                {
                     
                 if (((MaskedTextBox)c).Enabled == false)
                 {
                     ((MaskedTextBox)c).Enabled = true;
                     flag = true;

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
                     
                        if (((ComboBox)c).Enabled == true)
                        {
                            ((ComboBox)c).Enabled = false;
                            flag = true;

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
                     
                        if (((ComboBox)c).Text != "")
                        {
                            ((ComboBox)c).Text = "";
                            flag = true;
                            break;
                        }
                    
                }
            }
            return flag;
        }
        private void PartyCode()
        {
            String sel = "select ActCode from Accounts where ActTitle = '" + txtcustomer.Text + "'";
            DataTable dc = clsDataLayer.RetreiveQuery(sel);
            if (dc.Rows.Count > 0)
            {
                Code = dc.Rows[0][0].ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Enable();
        }

        private void btnSave_Click(object sender, EventArgs e)
        { 
         PartyCode();
            
             if (txtreports.SelectedIndex == 0)
            {
                string q = "";
                q = "select * from LedgerPayment where Datetime between '" + date1.Text + "' and '" + date2.Text + "' and RefCode='" + Code + "'  Order By ID ASC";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    rptLedger rpt = new rptLedger();
                    rpt.SetDataSource(purchase);
                    LedgerView pop = new LedgerView(date1.Text, date2.Text, rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }            else if (txtreports.SelectedIndex == 1)
            {
                string q = "select * from Purchase where Date between '" + date1.Text + "' and '" + date2.Text + "'";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    dailypurchasereport rpt = new dailypurchasereport();
                    rpt.SetDataSource(purchase);
                    Daily_Sale_Report pop = new Daily_Sale_Report(date1.Text, date2.Text, rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else if (txtreports.SelectedIndex == 2)
            {
                string q = "select * from Purchase where Date between '" + date1.Text + "' and '" + date2.Text + "' and VendorName='" + txtcustomer.Text + "'";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    dailypurchasereport rpt = new dailypurchasereport();
                    rpt.SetDataSource(purchase);
                    Daily_Sale_Report pop = new Daily_Sale_Report(date1.Text, date2.Text, rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }   
        } 
        private void txtreports_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(txtreports.SelectedIndex == 0)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true; 
            }
            else if (txtreports.SelectedIndex == 1)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true; 
            }
            else if (txtreports.SelectedIndex == 2)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true; 
            } 
        }

        private void cmbstatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
         


    }
}
