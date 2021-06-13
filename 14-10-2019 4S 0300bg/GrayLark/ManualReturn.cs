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
    public partial class ManualReturn : Form
    {
        String Code = "";
        public ManualReturn()
        {
            InitializeComponent();
            clsGeneral.SetAutoCompleteTextBox(txt_party, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 14 Order BY ID DESC "));
            this.KeyPreview = true;
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
        private bool CheckAllFields()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Text == "")
                    {
                        flag = true;
                        break;
                    }
                }
                else if (c is ComboBox)
                {
                    if (((ComboBox)c).Text == "")
                    {
                        flag = true;
                        break;
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
        decimal balance = 0;
        public decimal PartyBalance()
        {
            try
            {
                 //0Paid 1 Received
                String get = "";
                if (cb_trans.SelectedIndex == 1)
                {
                    get = "select Party_Balance from LedgerReceived  where RefCode='" + Code + "' order by ID desc";
                }
           else {
               get = "select Party_Balance from LedgerPayment where RefCode='" + Code + "' order by ID desc";
           }
                DataTable dfr = clsDataLayer.RetreiveQuery(get);
                if (dfr.Rows.Count > 0)
                {
                    balance = decimal.Parse(dfr.Rows[0]["Party_Balance"].ToString());
                }
                else
                {
                    balance = 0;
                }

            }
            catch { }
            return balance;

        }
        private void PartyCode()
        {
            String sel = "select ActCode from Accounts where ActTitle = '" + txt_party.Text + "'";
            DataTable dc = clsDataLayer.RetreiveQuery(sel);
            if (dc.Rows.Count > 0)
            {
                Code = dc.Rows[0][0].ToString();
            }
        }
        private void ReceiveDue()
        {
            PartyCode();
            String rec = "select * from ReceiveDue where PartyCode = '" + Code + "'";
            DataTable d = clsDataLayer.RetreiveQuery(rec);
            if (d.Rows.Count > 0)
            {
                decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
                decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
                decimal received = decimal.Parse(d.Rows[0]["ReceivedAmount"].ToString());
                decimal b = Convert.ToDecimal(txt_amount.Text);
             
                due -= b;
                received += b;
                string updateblnc = "update ReceiveDue set DueAmount=" + due.ToString() + ",ReceivedAmount="+received.ToString()+" where PartyCode='" + Code + "' ";
                clsDataLayer.ExecuteQuery(updateblnc);
            }
        }
        private void PaymentDue()
        {
            PartyCode();
            String rec = "select * from PaymentDue where PartyCode = '" + Code + "'";
            DataTable d = clsDataLayer.RetreiveQuery(rec);
            if (d.Rows.Count > 0)
            {
                decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
                decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
                decimal received = decimal.Parse(d.Rows[0]["PaidAmount"].ToString());
                decimal bill = decimal.Parse(txt_amount.Text);
                received += bill;
                due -= bill;

                string updateblnc = "update PaymentDue set PaidAmount=" + received.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                clsDataLayer.ExecuteQuery(updateblnc);
            }
          
        }
        private void btn_submit_Click(object sender, EventArgs e)
        {
            try
            {
                if(!CheckAllFields())
                { 
                    PartyCode();
                    txt_code.Enabled = false; txt_code.Text = clsGeneral.getMAXCode("tbl_Manual", "MNCODE", "MN");
                    String ins = "INSERT INTO tbl_Manual(MNCODE,Party_Name,Reason,Amount,MODE)values('"+txt_code.Text+"','"+txt_party.Text+"','"+txt_Reason.Text+"','"+txt_amount.Text+"','"+cb_trans.Text+"')";
                    if(clsDataLayer.ExecuteQuery(ins) > 0)
                    {
                        MessageBox.Show("Record Save Successfully!","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        btn_submit.Enabled = false; btnadd.Enabled = true;
                   }
                    //0Paid 1 Received
                    if(cb_trans.SelectedIndex == 1)
                    {
                        decimal a = PartyBalance();
                        decimal b = Convert.ToDecimal(txt_amount.Text);
                        decimal c = a - b;
                        String ascc = @"insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                             " values('"+txt_Reason.Text+"','" + txt_code.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101080','" + Code + "','" + txt_code.Text + "','" + txt_Reason.Text + "'," + b + ",0.00,'OnReceived'," + c + ",'Delizia') insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                             " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txt_Reason.Text + "','" + txt_code.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txt_code.Text + "','" + txt_party.Text + "'," + b + ",0.00,'OnReceived' ," + c + ",'Delizia')";
                     clsDataLayer.ExecuteQuery(ascc);
                     ReceiveDue();
                    }
                    else
                    {
                        decimal a = PartyBalance();
                        decimal b = Convert.ToDecimal(txt_amount.Text);
                        decimal c = a - b;
                        String ascc = @"insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                        " values('" + txt_Reason.Text + "','" + txt_code.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101080','" + Code + "','" + txt_code.Text + "','" + txt_Reason.Text + "'," + b + ",0.00,'OnPaid'," + c + ",'Delizia') insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                        " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txt_Reason.Text + "','" + txt_code.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txt_code.Text + "','" + txt_party.Text + "'," + b + ",0.00,'OnPaid' ," + c + ",'Delizia')";
                        clsDataLayer.ExecuteQuery(ascc);
                        PaymentDue();
                    }
                }
                else
                {
                    MessageBox.Show("Please Fill All Fields!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                //String ascc = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                //        " values('SR-00001','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101080','" + Code + "','" + txtreturn.Text + "','Sale Return'," + b + ",0.00,'OnReceived'," + c + ",'Reddot Express') insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                //        " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txt_sent.Text + "','" + txtCName.Text + "'," + b + ",0.00,'OnReceived' ," + c + ",'Reddot Express')";
                //clsDataLayer.ExecuteQuery(ascc);
         
            }
            catch { }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            Clears();  Enable();
            txt_code.Enabled = false; txt_code.Text = clsGeneral.getMAXCode("tbl_Manual", "MNCODE", "MN"); txt_party.Text = ""; txt_Reason.Text = "";
        }

        private void txt_amount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void close(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void ManualReturn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.A)
            {
                btnadd.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.S)
            {
                btn_submit.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }
    }
}
