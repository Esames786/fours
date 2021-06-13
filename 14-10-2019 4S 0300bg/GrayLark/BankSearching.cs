
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
    public partial class BankSearching : Form
    {
        String Code = "";
        public BankSearching()
        {
            InitializeComponent();
            Disable();
            date1.Enabled = false;
            date2.Enabled = false;
            String query = "select BankName from BankDetail";
            DataTable d = clsDataLayer.RetreiveQuery(query);
            if(d.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txtbank, d);
            }
            Enable();
            date1.Enabled = true; date2.Enabled = true;
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
            String sel = "select ActCode from Accounts where ActTitle = '" + txtbank.Text + "'";
            DataTable dc = clsDataLayer.RetreiveQuery(sel);
            if (dc.Rows.Count > 0)
            {
                Code = dc.Rows[0][0].ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
         Enable();  Clears();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {  
            PartyCode();
            if (txtreports.SelectedIndex == 0)
            {
                string q = "";
                q = "select * from BankCashIn where Dates  between '" + date1.Text + "' and '" + date2.Text + "' and BankName='" + txtbank.Text + "' and AccountNo='" + txtaccountno.Text + "' Order by BankIn asc";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    rptBankCashIn rpt = new rptBankCashIn();
                    rpt.SetDataSource(purchase);
                    LedgerView pop = new LedgerView(date1.Text, date2.Text, rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!");
                }
            }
            else if (txtreports.SelectedIndex == 1)
            {
                string q = "";
                q = "select * from CashIn where Dates between '" + date1.Text + "' and '" + date2.Text + "' Order by CashId asc";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    rptCashBook rpt = new rptCashBook();
                    rpt.SetDataSource(purchase);
                    LedgerView pop = new LedgerView(date1.Text, date2.Text, rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!");
                }
            } 
            else if (txtreports.SelectedIndex == 2)
            {
                string q = "select * from BankCashRelease where Dates between '" + date1.Text + "' and '" + date2.Text + "' and BankName='" + txtbank.Text + "' and AccountNo='" + txtaccountno.Text + "' Order by BankRel asc";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    rptBankCashRelease rpt = new rptBankCashRelease();
                    rpt.SetDataSource(purchase);
                    LedgerView pop = new LedgerView(date1.Text, date2.Text, rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!");
                }

            }

            else if (txtreports.SelectedIndex == 3)
            {
                string q = "select * from CashRelease where Dates between '" + date1.Text + "' and '" + date2.Text + "' Order by CashRel asc";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    rptCashRelease rpt = new rptCashRelease();
                    rpt.SetDataSource(purchase);
                    LedgerView pop = new LedgerView(date1.Text, date2.Text, rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!");
                }
            }
            else if (txtreports.SelectedIndex == 5)
            {
                string q = "select * from BankTransaction where Dates between '" + date1.Text + "' and '" + date2.Text + "' and BankName='" + txtbank.Text + "' and AccountNo='" + txtaccountno.Text + "' Order by BankIn asc";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    //rptBankInorOut rpt = new rptBankInorOut();
                    //rpt.SetDataSource(purchase);
                    //LedgerView pop = new LedgerView(date1.Text, date2.Text, rpt);
                    //pop.Show();
                    //
                    String CashRel = "";
                    string q5 = "select sum(BankCash) from BankTransaction where Dates between '" + date1.Text + "' and '" + date2.Text + "' and BankName='" + txtbank.Text + "' and AccountNo='" + txtaccountno.Text + "' and Status='Paid'";
                    DataTable purchase5 = clsDataLayer.RetreiveQuery(q5);
                    if (purchase5.Rows.Count > 0)
                    {
                        CashRel = purchase5.Rows[0][0].ToString();
                        if (CashRel.Equals("")) { CashRel = "0"; }
                    }
                    else { CashRel = "0"; }
                    String CashRel2 = "";
                    string q6 = "select sum(BankCash) from BankTransaction where Dates between '" + date1.Text + "' and '" + date2.Text + "' and BankName='" + txtbank.Text + "' and AccountNo='" + txtaccountno.Text + "' and Status='Received'";
                    DataTable purchase6 = clsDataLayer.RetreiveQuery(q6);
                    if (purchase6.Rows.Count > 0)
                    {
                        CashRel2 = purchase6.Rows[0][0].ToString();
                        if (CashRel2.Equals("")) { CashRel2 = "0"; }
                    }
                    else
                    {
                        CashRel2 = "0";
                    }
                    rptBankInorOut rpt = new rptBankInorOut();
                    rpt.SetDataSource(purchase);
                    CashTransView pop = new CashTransView(date1.Text, date2.Text, rpt, CashRel2, CashRel);
                    pop.Show();
                    //
                }
                else
                {
                    MessageBox.Show("No Record Found!");
                }
            }
            else if (txtreports.SelectedIndex == 4)
            {
                string q = "select * from CashTransaction where Dates between '" + date1.Text + "' and '" + date2.Text + "' Order by CashIn asc";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    //
                    String CashRel = "";
                    string q5 = "select sum(Cash) from CashTransaction where Dates between '" + date1.Text + "' and '" + date2.Text + "' and Status='Paid'";
                DataTable purchase5 = clsDataLayer.RetreiveQuery(q5);
                if (purchase5.Rows.Count > 0)
                {
                    CashRel=purchase5.Rows[0][0].ToString();
                    if (CashRel.Equals("")) { CashRel = "0"; }
                }else{CashRel="0";}
                        String CashRel2 = "";
                    string q6 = "select sum(Cash) from CashTransaction where Dates between '" + date1.Text + "' and '" + date2.Text + "' and Status='Received'";
                    DataTable purchase6 = clsDataLayer.RetreiveQuery(q6);
                if (purchase6.Rows.Count > 0)
                { 
                    CashRel2 = purchase6.Rows[0][0].ToString();
                    if (CashRel2.Equals("")) { CashRel2 = "0"; }
                }
                else
                {
                    CashRel2 = "0";
                }
                    rptCashTransaction rpt = new rptCashTransaction();
                    rpt.SetDataSource(purchase);
                    CashTransView pop = new CashTransView(date1.Text, date2.Text, rpt, CashRel2, CashRel);
                    pop.Show();
                }
                else { MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
          
        }

        private void txtreports_SelectedIndexChanged(object sender, EventArgs e)
        { 
            if(txtreports.SelectedIndex == 0)
            {
                Enable();

                date1.Enabled = true;
                date2.Enabled = true;
                txtbank.Enabled=true;
                txtaccountno.Enabled=true;
            }
            else if (txtreports.SelectedIndex == 1)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                txtbank.Enabled=false;
                txtaccountno.Enabled=false;
            }
            else if (txtreports.SelectedIndex == 2)
            {
                Enable();
                 date1.Enabled = true;
                date2.Enabled = true;
                txtbank.Enabled=true;
                txtaccountno.Enabled=true;
            }
            else if (txtreports.SelectedIndex == 3)
            {
                  Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                txtbank.Enabled=false;
                txtaccountno.Enabled=false;
            }  else if (txtreports.SelectedIndex == 5)
            {
                Enable();
                 date1.Enabled = true;
                date2.Enabled = true;
                txtbank.Enabled=true;
                txtaccountno.Enabled=true;
            } else if (txtreports.SelectedIndex == 4)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                txtbank.Enabled=false;
                txtaccountno.Enabled=false;
            }
          
        }

        private void txtbank_Leave(object sender, EventArgs e)
        {
            try
            {
                String query = "select AccountNo from BankDetail WHERE BankName='"+txtbank.Text+"'";
                DataTable d = clsDataLayer.RetreiveQuery(query);
                if (d.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtaccountno, d);
                }
                else
                {
                    String querys = "select AccountNo from BankDetail";
                    DataTable d5 = clsDataLayer.RetreiveQuery(querys);
                    if (d5.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtaccountno, d5);
                    }
                }

            }
            catch
            {
            }
        }

        private void txtaccountno_Click(object sender, EventArgs e)
        {
            try
            {
                String query = "select AccountNo from BankDetail WHERE BankName='" + txtbank.Text + "'";
                DataTable d = clsDataLayer.RetreiveQuery(query);
                if (d.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtaccountno, d);
                }
                else
                {
                    String querys = "select AccountNo from BankDetail";
                    DataTable d5 = clsDataLayer.RetreiveQuery(querys);
                    if (d5.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtaccountno, d5);
                    }
                }

            }
            catch
            {
            }
        }

        private void date1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void date2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtbank_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtaccountno_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbstatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void close(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
         

    }
}
