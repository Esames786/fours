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
    public partial class PayableandReceiveable : Form
    {
        String Code = ""; String h0 = ""; decimal Remaining = 0; bool ck = false;
        public PayableandReceiveable()
        {
            InitializeComponent();
            Disable(); btnsubmit.Enabled = false; 
            clsGeneral.SetAutoCompleteTextBox(txt_party, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 14 Order BY ID DESC"));
            this.KeyPreview = true;
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

        private void PaymentDue(decimal AmountFinal)
        {
            PartyCode();
            String rec = "select * from PaymentDue where PartyCode = '" + Code + "'";
            DataTable d = clsDataLayer.RetreiveQuery(rec);
            if (d.Rows.Count > 0)
            {
                decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
                decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
                decimal received = decimal.Parse(d.Rows[0]["PaidAmount"].ToString());
                decimal bill =  AmountFinal ;
                total = bill;
                due   = bill;

                string updateblnc = "update PaymentDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                clsDataLayer.ExecuteQuery(updateblnc);
            }
            else
            {
                String ii = @"insert into PaymentDue (PayCode,PartyName,PartyCode,TotalAmount,DueAmount,PaidAmount,Company_Name) values('" + txt_code.Text + "','" + txt_party.Text + "','" + Code + "'," + txt_amount.Text + "," + txt_amount.Text + ",0,'Reddot Technologies')";
                clsDataLayer.ExecuteQuery(ii);
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

                total += b;
                   due += b;
                string updateblnc = "update ReceiveDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                clsDataLayer.ExecuteQuery(updateblnc);
            }
            else
            {
                decimal b = Convert.ToDecimal(txt_amount.Text); 
                String ii = @"insert into ReceiveDue (RemCode,PartyName,PartyCode,TotalAmount,DueAmount,ReceivedAmount,Company_Name)
                values('" + txt_code.Text + "','" + txt_party.Text + "','" + Code + "'," + b + "," + b + ",0,'"+cb_company.Text+"')";
                clsDataLayer.ExecuteQuery(ii);
            }
        }

        decimal balance = 0;
        String Status = "";
        public decimal PartyBalance()
        {
        try
        {
        String get = ""; 
        if (cmb.Text.Equals("OnPayabale"))
        {
            get = "select Party_Balance,PaymentType from LedgerPayment where RefCode='" + Code + "' order by ID desc";
        }
        else
        {
            get = "select Party_Balance from LedgerReceived where RefCode='" + Code + "' order by ID desc";
        }
        DataTable dfr = clsDataLayer.RetreiveQuery(get);
        if (dfr.Rows.Count > 0)
        {
            balance = decimal.Parse(dfr.Rows[0]["Party_Balance"].ToString()); Status = dfr.Rows[0]["PaymentType"].ToString();
        }   else  { balance = 0;}

        }
        catch { }
        return balance;
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
          

        private void btnsubmit_Click(object sender, EventArgs e)
        {
       if (!CheckAllFields())
       {
           txt_code.Text = clsGeneral.getMAXCode("tbl_payandreceive", "PR_CODE", "PR"); btnsubmit.Enabled = false;
      String query = "insert into tbl_payandreceive(PR_CODE,PartyName,PartyCode,Reason,Amount,UserName)values('" + txt_code.Text + "','" + txt_party.Text + "','" + Code + "','" + txt_reason.Text + "'," + txt_amount.Text + ",'" + Login.UserID + "')";
      clsDataLayer.ExecuteQuery(query);
        
      PartyCode();
      decimal a = PartyBalance();
      decimal b = Convert.ToDecimal(txt_amount.Text);
      decimal c = 0;

      if (cmb.Text.Equals("OnPayabale"))
      { 
          String getname = "select RemainingLimit,OpeningBalance from Accounts where ActTitle='" + txt_party.Text + "'";
          DataTable de = clsDataLayer.RetreiveQuery(getname);
          if (de.Rows.Count < 1)
          {
              MessageBox.Show("UserName dont have a chart Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
          else
          {
              String Opb = de.Rows[0][1].ToString();
              if (Opb.Equals("NILL"))
              { 
              }
              else
              {
                  decimal limit = Convert.ToDecimal(de.Rows[0][1].ToString());
                  decimal Avlimit = Convert.ToDecimal(de.Rows[0][0].ToString());
                  if (Avlimit != 0)
                  {
                      decimal LimitUse = Avlimit;
                      decimal CashRe = Convert.ToDecimal(txt_amount.Text);
                      if (CashRe > LimitUse)
                      {
                          MessageBox.Show("Balance Limit : " + limit + "  " + "Remaining Limit : " + Avlimit, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                      }
                      else
                      {
                          decimal final = Avlimit - CashRe;
                          String upd = "update Accounts set RemainingLimit =" + final + " where ActCode='" + Code + "'"; clsDataLayer.ExecuteQuery(upd);
                      }
                  }
                  else
                  {
                      MessageBox.Show("Balance Limit : " + limit + "  " + "Remaining Limit : " + Avlimit, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  }
              }
          }
          // 
          c=  a + b; 
          PaymentDue(c);
          String ins = @"insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                          " Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) values('" + txt_reason.Text + "','" + txt_code.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','01010209 ','" + Code + "','" + txt_code.Text + "','" + txt_reason.Text + "'," + txt_amount.Text + ",0.00,'" + cmb.Text + "'," + c + ",'" + cb_company.Text + "')  insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                          " values('" + txt_reason.Text + "','" + txt_code.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txt_code.Text + "','" + txt_party.Text + "',0.00," + txt_amount.Text + ",'" + cmb.Text + "'," + c + ",'" + cb_company.Text + "') ";
          clsDataLayer.ExecuteQuery(ins);
      }
      else if (cmb.Text.Equals("OnReceiveable"))
      {
          PartyCode();
          ReceiveDue(); 
          c = a + b;
          String ins = @"insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
         " values('" + txt_reason.Text + "','" + txt_code.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txt_code.Text + "','" + txt_party.Text + "'," + b + ",0.00,'OnCredit'," + c + ",'" + cb_company.Text + "') insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
         " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txt_reason.Text + "','" + txt_code.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','01010207','" + Code + "','" + txt_code.Text + "','" + txt_reason.Text + "'," + b + ",0.00,'OnCredit' ," + c + ",'" + cb_company.Text + "')";
          clsDataLayer.ExecuteQuery(ins);
      }
      MessageBox.Show("Record Save Successfully!","Success",MessageBoxButtons.OK,MessageBoxIcon.Information); Disable(); btnsubmit.Enabled = false; btn_Add.Enabled = true; 
      }
       else
       {
           MessageBox.Show("Please Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
       }
      
       }

        private void btn_Add_Click(object sender, EventArgs e)
        {
        try
        {
        Enable(); Clears();
        txt_code.Text = clsGeneral.getMAXCode("tbl_payandreceive", "PR_CODE", "PR"); ck = false; cb_company.Text = "GrayLark"; btnsubmit.Enabled = true;
        btn_Add.Enabled = false; btnsubmit.Enabled = true; txt_party.Text = ""; txt_reason.Text = ""; txt_amount.Text = ""; txt_party.Focus();
        } catch{  }
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

        private void txtcno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
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

        private void PayableandReceiveable_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.A)
            {
                btn_Add.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.S)
            {
                btnsubmit.PerformClick();
            }
        }
       
    }
}
