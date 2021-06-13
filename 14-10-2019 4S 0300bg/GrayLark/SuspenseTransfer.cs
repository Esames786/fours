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
using Reddot_Express_Inventory.bin.Debug.Report;

namespace Reddot_Express_Inventory
{
    public partial class SuspenseTransfer : Form
    {
        String sign = "";
        String UID = Login.UserID;
        public string GreenSignal = "";
        String AccountName = "";
        public string FormID = "";
        double ck = 0;
        decimal old = 0;
        String CashChange = "false"; String OLDDATE = "";
        String Code = "";
        public SuspenseTransfer()
        {
            InitializeComponent();
            clsGeneral.SetAutoCompleteTextBox(txtparty, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 12 Order BY ID DESC"));
            String a1 = "select PartyName from tbl_SuspenseTransfer";
            DataTable d1 = clsDataLayer.RetreiveQuery(a1);
            if (d1.Rows.Count > 0)
            { clsGeneral.SetAutoCompleteTextBox(txt_pname, d1); }

            String a2 = "select Amount from tbl_SuspenseTransfer";
            DataTable d2 = clsDataLayer.RetreiveQuery(a2);
            if (d2.Rows.Count > 0)
            { clsGeneral.SetAutoCompleteTextBox(txt_pamount, d2); }

            String a3 = "select TransId from tbl_SuspenseTransfer";
            DataTable d3 = clsDataLayer.RetreiveQuery(a3);
            if (d3.Rows.Count > 0)
            { clsGeneral.SetAutoCompleteTextBox(txt_bcode, d3); }

            String a4 = "select Remarks from tbl_SuspenseTransfer";
            DataTable d4 = clsDataLayer.RetreiveQuery(a4);
            if (d4.Rows.Count > 0)
            { clsGeneral.SetAutoCompleteTextBox(txt_remarks, d4); } Disable();
     }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
  
        public void UserNametesting()
        {
            string query = "  select * from user_access where USENAME='" + UID + "' AND FORM_NAME='" + FormID + "'";
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
            con.Open();
            SqlCommand sc = new SqlCommand(query, con);
            SqlDataReader dr = sc.ExecuteReader();
            dr.Read();

            if (dr.HasRows == true)
            {
                if (dr["ACCESS"].ToString() == "Yes")
                {
                    GreenSignal = "YES";
                }
                else
                {
                    GreenSignal = "NO"; 
                }
            }
        }

        private void Courier_Transaction_Load(object sender, EventArgs e)
        {   ShowAll();    }

        private void ShowAll()
        {
            try
            {
  SqlDataAdapter sda = new SqlDataAdapter("select TransId,Dates,PartyName,AvailableBalance,Amount,RemainingBalance,Remarks,UserName,Nature from tbl_SuspenseTransfer", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
     
                dataGridView1.DataSource = dt; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        { 
            FormID = "Add";
            UserNametesting();
            if (GreenSignal == "YES")
            {
            enable();
            this.txtparty.Focus();
            try
            {
                Clears();
                txtInvoice.Text = clsGeneral.getMAXCode("tbl_SuspenseTransfer", "TransId", "TS");
                btnEdit.Enabled = false;
                btnupdate.Enabled = false;
                btnSave.Enabled = true;
                btnNew.Enabled = false;
                txtnature.Enabled = true;
                txt_bcode.Text = "-";
                txt_pname.Text = "-";
                txt_pamount.Text = "-";
                txt_remarks.Text = "-";
                Enable();
                btnEdit.Enabled = false; btnupdate.Enabled = false;
            }
            catch 
            { }
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void enable()
        {
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            txtInvoice.Enabled = true;
            //date1.Enabled = true;
            //date2.Enabled = true; 
            txtparty.Enabled = true;
            txtRemarks.Enabled = true;
        } 

        private void txtCashRelease_TextChanged(object sender, EventArgs e)
        {
            CashRelease();
            CashChange = "true";
        }

        private void txtAvailable_TextChanged(object sender, EventArgs e)
        {
            CashRelease();
        }

        private void CashRelease()
        {
            try
            {
                double r = Convert.ToDouble(txtCashRelease.Text);
                double final = 0;
                //if (r > ck)
                //{
                    final = r - ck;
                    double a = Convert.ToDouble(txtAvailable.Text);
                    double na = a - r;
                    String ans = na.ToString();
                    //if (ans.StartsWith("-"))
                    //{
                    //    txtNewAmount.Text = "0";
                    //    txtCashRelease.Text = "0";
                    //}
                    //else
                    //{
                        txtNewAmount.Text = na.ToString();
                   // }
               // }
                //else if (r < ck)
                //{
                //    final = ck - r;
                //    double a = Convert.ToDouble(txtAvailable.Text);
                //    double na = a + final;
                //    String ans = na.ToString();
                //    //if (ans.StartsWith("-"))
                //    //{
                //    //    txtNewAmount.Text = "0";
                //    //    txtCashRelease.Text = "0";
                //    //}
                //    //else
                //    //{
                //        txtNewAmount.Text = na.ToString();
                //    //}
                //}
                 if(r == ck)
                {
                    txtNewAmount.Text = txtAvailable.Text;
                }
             
            }
            catch (Exception ex)
            {
                string name = ex.Message;
            }
        }

        private bool CheckAllFields()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel3.Controls)
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
                    {
                        if (((ComboBox)c).Text == "")
                        {
                            flag = true;
                            break;
                        }
                    }
                }
            }
            return flag;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckAllFields())
            {
                String Condition = "false";
                FormID = "Save"; UserNametesting();
                if (GreenSignal == "YES")
                {
                    decimal available = Convert.ToDecimal(txtAvailable.Text);
                    decimal paid = Convert.ToDecimal(txtNewAmount.Text);
                    if (!CheckAllFields())
                    {
                        String getname = "select * from Accounts where ActTitle='" + txtparty.Text + "'";
                        DataTable de = clsDataLayer.RetreiveQuery(getname);
                        if (de.Rows.Count < 1)
                        {
                            MessageBox.Show("UserName dont have a chart Account");
                        }
                        else
                        {
                            String ins = "insert into tbl_SuspenseTransfer(TransId,Dates,Nature,AvailableBalance,Amount,RemainingBalance,PartyName,Remarks,UserName)values('" + txtInvoice.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + txtnature.Text + "'," + txtAvailable.Text + "," + txtCashRelease.Text + "," + txtNewAmount.Text + ",'" + txtparty.Text + "','" + txtRemarks.Text + "','" + Login.UserID + "')";
                            clsDataLayer.ExecuteQuery(ins);
                            PartyCode();
                            decimal a = 0;
                            if (txtnature.Text.Equals("Paid"))
                            { a = PartyBalance(); }
                            else { a = PartyBalances(); }

                            //String h = "select HeaderActCode from Accounts where ActTitle = '" + txtparty.Text + "'";
                            //DataTable dq = clsDataLayer.RetreiveQuery(h);
                            decimal c = 0;
                            //bool req = false;
                            //if (dq.Rows.Count > 0)
                            //{
                            //    int head = Convert.ToInt32(dq.Rows[0][0]);
                            //    if (head == 20101 || head == 10101) { req = true; }
                            //    else
                            //    {
                            //        if (a > 0) { req = true; }
                            //        else
                            //        {
                            //            if (a < 0)
                            //            {
                            //                req = true;
                            //            }
                            //            else
                            //            {
                            //                req = false;
                            //            }
                            //        }
                            //    }

                            //}

                            decimal b = Convert.ToDecimal(txtCashRelease.Text);

                            String h3 = "select HeaderActCode from Accounts where ActTitle = '" + txtparty.Text + "'";
                            DataTable dq3 = clsDataLayer.RetreiveQuery(h3);
                            if (dq3.Rows.Count > 0)
                            {
                                int head = Convert.ToInt32(dq3.Rows[0][0]);
                                if (head == 20101 || head == 10101)
                                {
                                    c = a - b;
                                    sign = "minus";
                                }
                                else
                                {
                                    if (a > 0)
                                    {
                                        c = a + b;
                                        sign = "plus";
                                    }
                                    else
                                    {
                                        if (a < 0)
                                        {
                                            c = a + b; sign = "plus";
                                        }
                                        else { c = 0; }
                                    }
                                }

                            }

                            if (txtnature.Text.Equals("Paid"))
                            {
                                String ins4 = @"insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                                " values('" + txtInvoice.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtInvoice.Text + "','" + txtparty.Text + "'," + txtCashRelease.Text + ",0.00,'OnPayabale'," + c + ",'Reddot Technologies') insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                                " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtInvoice.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','528','" + Code + "','" + txtInvoice.Text + "','Suspense'," + txtCashRelease.Text + ",0.00,'OnPayabale' ," + c + ",'Reddot Technologies')";
                                if (clsDataLayer.ExecuteQuery(ins4) > 0)
                                { Condition = "true"; PayRec(); }
                            }
                            else
                            {
                                String asd = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                                " values('" + txtInvoice.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','528','" + Code + "','" + txtInvoice.Text + "','Suspense'," + txtCashRelease.Text + ",0.00,'OnCredit'," + c + ",'Reddot Technologies') insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                                " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtInvoice.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtInvoice.Text + "','" + txtparty.Text + "'," + txtCashRelease.Text + ",0.00,'OnCredit' ," + c + ",'Reddot Technologies')";
                                if (clsDataLayer.ExecuteQuery(asd) > 0) { Condition = "true"; PayRec(); }
                            }

                        }

                        if (Condition.Equals("true"))
                        {
                            SuspenseLedger();
                            MessageBox.Show("Suspense Account Transfer Successfully!");
                            txtparty.Clear();
                            disable();
                            ShowAll();
                            btnEdit.Enabled = true;
                            txtnature.Enabled = false;
                            txt_bcode.Text = "-";
                            txt_pname.Text = "-";
                            txt_pamount.Text = "-";
                            txt_remarks.Text = "-";
                        }
                        else
                        {
                            MessageBox.Show("Not Inserted!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Fill All Fields!");
                    }
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                MessageBox.Show("Please Fill All Fields!");
            }
        }

        private void PartyCode()
        {
            String sel = "select ActCode from Accounts where ActTitle = '" + txtparty.Text + "'";
            DataTable dc = clsDataLayer.RetreiveQuery(sel);
            if (dc.Rows.Count > 0)
            {
                Code = dc.Rows[0][0].ToString();
            }
        }
        private void disable()
        {
            btnNew.Enabled = true;
            btnSave.Enabled = false;
            
            txtInvoice.Enabled = false;
     
             txtAvailable.Enabled = false;
            txtCashRelease.Enabled = false;

           

            txtNewAmount.Enabled = false;
            txtRemarks.Enabled = false;
             txtInvoice.Clear();
     
            txtAvailable.Clear();
             txtCashRelease.Clear(); 
            txtNewAmount.Clear();
            txtRemarks.Clear();
        }

        public void SuspenseLedger()
        {
            decimal transfer = Convert.ToDecimal(txtCashRelease.Text); decimal available = Convert.ToDecimal(txtAvailable.Text);
            decimal Paid = rp + transfer; decimal DueFinal = available - transfer;
        if (txtnature.Text.Equals("Paid"))
        {
        
        String ins4 = @"insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
        " values('" + txtInvoice.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','528','528','" + txtInvoice.Text + "','" + txtparty.Text + "'," + txtCashRelease.Text + ",0.00,'OnPaid'," + DueFinal + ",'Reddot Technologies') insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
        " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtInvoice.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','528','" + txtInvoice.Text + "','Suspense'," + txtCashRelease.Text + ",0.00,'OnPaid' ," + DueFinal + ",'Reddot Technologies')";
        if (clsDataLayer.ExecuteQuery(ins4) > 0)
        {SuspenseIncDec(); }
        }
        else
        {
        String asd = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
        " values('" + txtInvoice.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','528','528','" + txtInvoice.Text + "','Suspense'," + txtCashRelease.Text + ",0.00,'OnCredit'," + DueFinal + ",'Reddot Technologies') insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
        " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtInvoice.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','528','" + txtInvoice.Text + "','" + txtparty.Text + "'," + txtCashRelease.Text + ",0.00,'OnCredit' ," + DueFinal + ",'Reddot Technologies')";
        if (clsDataLayer.ExecuteQuery(asd) > 0) { SuspenseIncDec(); }
        }
        }

        private void txtCashRelease_Leave(object sender, EventArgs e)
        {
            if(txtCashRelease.Text.Equals(""))
            {
                txtNewAmount.Text = txtAvailable.Text;
                txtCashRelease.Text = "0";
            }
            //else
            //{
            //    double r = Convert.ToDouble(txtCashRelease.Text);
            //    if (ck > r)
            //    {
            //        txtNewAmount.Text = txtAvailable.Text;
            //        txtCashRelease.Text = "0";
            //    }
            //}
        }

        private void txtCashRelease_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtparty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }

            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
        }

        private void txtbname_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (Char.IsDigit(e.KeyChar) || e.KeyChar == '\b' || e.KeyChar == '-' || e.KeyChar == '=' || e.KeyChar == '.' || e.KeyChar == (char)Keys.Space || e.KeyChar == '+')
            //{
            //    e.Handled = true;
            //}
            //else
            //{
            //    e.Handled = false;
            //}

            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (!char.IsControl(e.KeyChar)&& !char.IsLetter(e.KeyChar) && (e.KeyChar != '-') &&(e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void txtRemarks_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Escape)
            //{
            //    this.Close();
            //}

            //e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || !(char.IsDigit(e.KeyChar) ));
        }

        private void txtcheque_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (!char.IsControl(e.KeyChar) && char.IsLetter(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }
        }

        private void comboMode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void txtbano_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (Char.IsDigit(e.KeyChar) || e.KeyChar == '\b' || e.KeyChar == '-' || e.KeyChar == '=' || e.KeyChar == '.' || e.KeyChar == (char)Keys.Space || e.KeyChar == '+')
            //{
            //    e.Handled = true;
            //}
            //else
            //{
            //    e.Handled = false;
            //}

            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar) && (e.KeyChar != '-') && (e.KeyChar == (char)Keys.Space) && (e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

    
        decimal balance = 0;
        public decimal PartyBalance()
        {
        try
        {
        String h = "select * from Accounts where ActTitle = '" + txtparty.Text + "' and HeaderActCode='02'";
           DataTable dq = clsDataLayer.RetreiveQuery(h);
           if (dq.Rows.Count < 1)
           {
               String get = "select Party_Balance from LedgerPayment where RefCode='" + Code + "' order by ID desc";
               DataTable dfr = clsDataLayer.RetreiveQuery(get);
               if (dfr.Rows.Count > 0)
               {
                   balance = decimal.Parse(dfr.Rows[0]["Party_Balance"].ToString());
               }
               else{balance = 0;}
           }
           else
           {
               balance = 0;
           }
            }
            catch { }
            return balance;

        }

        decimal balances = 0;
        public decimal PartyBalances()
        {
            try
            {
                String h = "select * from Accounts where ActTitle = '" + txtparty.Text + "' and HeaderActCode='02'";
                DataTable dq = clsDataLayer.RetreiveQuery(h);
                if (dq.Rows.Count < 1)
                {
                    String get = "select Party_Balance from LedgerReceived where RefCode='" + Code + "' order by ID desc";
                    DataTable dfr = clsDataLayer.RetreiveQuery(get);
                    if (dfr.Rows.Count > 0)
                    {
                        balances = decimal.Parse(dfr.Rows[0]["Party_Balance"].ToString());
                    }
                    else { balances = 0; }
                }
                else
                {
                    balances = 0;
                }
            }
            catch { }
            return balances;

        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
              
            }
            catch { }
        }

        private void btnFind_Click(object sender, EventArgs e)
        { 
            Find find = new Find("Transactions","Payment_voucher");
            find.Show();
            this.Hide();
        }

        private void txtInvoice_TextChanged(object sender, EventArgs e)
        {
            search();
        } 

        private bool Enable()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel3.Controls)
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
            foreach (Control c in this.tableLayoutPanel3.Controls)
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
        private void Clears()
        {
            foreach (Control c in this.tableLayoutPanel3.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Text != "")
                    {
                        ((TextBox)c).Text = "";
                    }
                }
                else if (c is ComboBox)
                {
                    {
                        if (((ComboBox)c).Text != "")
                        {
                            ((ComboBox)c).Text = "";
                           
                        }
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            txtnature.Enabled = false;
          String search = "select TransId from Transactions";
            DataTable dt = clsDataLayer.RetreiveQuery(search);
            if (dt.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txtInvoice, dt);
            } 
            txtInvoice.Enabled = true;
            txtInvoice.ReadOnly = false;
            btnupdate.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = false;
            btnEdit.Enabled = false;
            txt_bcode.Text = "-";
            txt_pname.Text = "-";
            txt_pamount.Text = "-";
            txt_remarks.Text = "-";
            dataGridView1.Enabled = false;
            ck = Convert.ToDouble(txtCashRelease.Text);
 
            enable();
            Enable();

            txtparty.Enabled = false;
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            FormID = "Update";
             UserNametesting();
            if (GreenSignal == "YES")
            {
                 
                    if (!CheckAllFields())
                    {
                   try
                   {
                   //PartyCode();
                   //String querys = "Delete From Transactions Where TransId = '" + txtInvoice.Text + "' Delete From CashRelease Where CashRel = '" + txtInvoice.Text + "' Delete From BankCashRelease Where BankRel = '" + txtInvoice.Text + "'  ";
                   //clsDataLayer.ExecuteQuery(querys);

                   //String q1 = "delete from BankTransaction where BankCode='" + txtInvoice.Text + "' delete from	CashTransaction where CashCode='" + txtInvoice.Text + "'";
                   //clsDataLayer.ExecuteQuery(q1);

                   //string query = "";
                   //query = "insert into Transactions (TransId,Dates,BankName,AccountNo,ChequeNo,RemainingBalance,PartyName,CashGiven,Remarks,UserName,Mode,Company_Name)values ('" + txtInvoice.Text + "','" + OLDDATE + "','" + txtbname.Text + "','" + txtbano.Text + "','" + txtcheque.Text + "','" + txtNewAmount.Text + "','" + txtparty.Text + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + UID + "','" + comboMode.Text + "','" + txtnature.Text + "')";
                   //clsDataLayer.ExecuteQuery(query);

                     
                   }
                   catch
                   {   }

                  

                        //
                        PartyCode();
                        decimal a = PartyBalance();

                        String h = "select HeaderActCode from Accounts where ActTitle = '" + txtparty.Text + "'";
                        DataTable dq = clsDataLayer.RetreiveQuery(h);
                        decimal c = 0;
                        bool req = false;
                        if (dq.Rows.Count > 0)
                        {
                            int head = Convert.ToInt32(dq.Rows[0][0]);
                            if (head == 20101 || head == 10101)
                            {
                                req = true;
                            }
                            else
                            {
                                if (a > 0)
                                {
                                    req = true;
                                }
                                else
                                {
                                    if (a < 0)
                                    {
                                        req = true;
                                    }
                                    else
                                    {
                                        req = false;
                                    }
                                }
                            }

                        }

                        //
                        String state = "false";
                        //
                        if (req == true)
                        {
                            String h2 = "select HeaderActCode from Accounts where ActTitle = '" + txtparty.Text + "'";
                            DataTable dqs = clsDataLayer.RetreiveQuery(h2);

                            if (dqs.Rows.Count > 1)
                            {
                                int head = Convert.ToInt32(dqs.Rows[0][0]);
                                if (head == 020101 || head == 010101)
                                {
                                    String seldues = "Select * from PaymentDue where PartyCode='" + Code + "'";
                                    DataTable dxs = clsDataLayer.RetreiveQuery(seldues);
                                    if (dxs.Rows.Count > 0)
                                    {
                                        state = "true";
                                        decimal totals = Convert.ToDecimal(dxs.Rows[0]["TotalAmount"].ToString());
                                        decimal dues = Convert.ToDecimal(dxs.Rows[0]["DueAmount"].ToString());
                                        decimal paids = Convert.ToDecimal(dxs.Rows[0]["PaidAmount"].ToString());
                                        decimal pays = Convert.ToDecimal(txtCashRelease.Text);

                                        dues = (dues + old) - pays;
                                        paids = (paids - old) + pays;
                                        String qs = "update PaymentDue set DueAmount=" + dues + ",PaidAmount=" + paids + " where PartyCode='" + Code + "'";
                                        clsDataLayer.ExecuteQuery(qs);

                                    }
                                }
                            }
                        }
                        decimal a2 = 0;
                        a2 = Convert.ToDecimal(txtCashRelease.Text);
                        int index = GetRowIndex(txtInvoice.Text);
                        DataTable dt = SetDT();
                        ShowDt(dt);
                        Console.WriteLine("Index=" + index);
                        DeleteRecord();
                        dt.Rows[index][7] = a2;
                        dt.Rows[index + 1][8] = a2;
                        dt = SomeOperation(dt);
                        InsertRecord(dt);

                        MessageBox.Show("Payment Update Successfully!");
                        // } 
                        Disable();
                        disable();
                        btnEdit.Enabled = true;
                        btnNew.Enabled = true;
                        btnupdate.Enabled = false;
                        btnNew.Enabled = true;
                        txt_bcode.Text = "-";
                        txt_pname.Text = "-";
                        txt_pamount.Text = "-";
                        txt_remarks.Text = "-";
                        dataGridView1.Enabled = true;
                        CashChange = "false"; 
                    }
                    else
                    {
                        MessageBox.Show("Please Fill All Fields!");
                    }
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void txtInvoice_Leave(object sender, EventArgs e)
        {
            search();
           }


        public void search()
        {
            try
            {
                String search = "select * from tbl_SuspenseTransfer where TransId = '" + txtInvoice.Text + "' ";
                DataTable dt = clsDataLayer.RetreiveQuery(search);
                if (dt.Rows.Count > 0)
                {
                    txtInvoice.Enabled = false;
                    txtInvoice.ReadOnly = true;
                    OLDDATE = dt.Rows[0]["Dates"].ToString();
                    txtparty.Text = dt.Rows[0]["PartyName"].ToString();
                    txtCashRelease.Text = dt.Rows[0]["Amount"].ToString(); 
                    txtNewAmount.Text = dt.Rows[0]["RemainingBalance"].ToString();
                    txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                    txtnature.Text = dt.Rows[0]["Nature"].ToString();
                   decimal aa = Convert.ToDecimal(dt.Rows[0]["Amount"].ToString());
                    decimal bb = Convert.ToDecimal(dt.Rows[0]["RemainingBalance"].ToString());
                    decimal cc = aa + bb;
                    txtAvailable.Enabled = true;

                    txtAvailable.Text = cc.ToString();
                    Disable();
                    btnSave.Enabled = false;
                    old = Convert.ToDecimal(txtCashRelease.Text);
                }
            }
            catch { }

        }



        //Ledger Update
        #region LedgerUpdate
        private int GetRowIndex(string input)
        {
            int index = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT V_No FROM LedgerPayment where RefCode = '" + Code + "' order by ID asc", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetValue(0).ToString() == input)
                    {
                        break;
                    }
                    index++;
                }
                con.Close();
            }
            catch { }
            return index;
        }
        private void DeleteRecord()
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM LedgerPayment where RefCode = '" + Code + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch { }
        }
        private DataTable SetDT()
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlDataAdapter cmd = new SqlDataAdapter("select V_No, Datetime, AccountCode, RefCode, PaymentType, InvoiceNo, Particulars, Debit, Credit, Party_Balance,Company_Name from LedgerPayment where RefCode = '" + Code + "' order by ID asc", con);
                cmd.Fill(dt);
                con.Close();
            }
            catch { }
            return dt;
        }
        private DataTable SomeOperation(DataTable dt)
        {
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Console.WriteLine("Payment Type" + dt.Rows[i].ItemArray[4]);
                    String type = "";
                    type = dt.Rows[i].ItemArray[4].ToString();
                    if (type.Equals("OnCredit"))
                    {
                        Console.WriteLine("True");
                    }
                    else
                    {
                        Console.WriteLine("False");
                    }
                    if (i == 0)
                    {
                        dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[7]);
                    }
                    else if (i == 1)
                    {
                        dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[8]);
                    }
                    else
                    {
                        ///
                            String h2 = "select HeaderActCode from Accounts where ActTitle = '" + txtparty.Text + "'";
                        DataTable dqs = clsDataLayer.RetreiveQuery(h2);

                        if (dqs.Rows.Count > 1)
                        {
                            int head = Convert.ToInt32(dqs.Rows[0][0]);
                            if (head == 020101 || head == 010101)
                            {
                                if (type.Equals("OnPayabale"))
                                {
                                    dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[7]) + Convert.ToDecimal(dt.Rows[i - 1].ItemArray[9]);
                                }
                                else
                                {
                                    dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i - 1].ItemArray[9]) - Convert.ToDecimal(dt.Rows[i].ItemArray[7]);
                                }
                            }
                            else
                            {
                                dt.Rows[i][9] = 0;
                            }
                        }

                      
                   
                    }
                }
            }
            catch { }
            return dt;
        }
        private void InsertRecord(DataTable dt)
        {
            try
            {
                con.Open();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].ItemArray[8].ToString().Equals(""))
                    {
                        dt.Rows[i].ItemArray[8] = "0.00";
                    }
                    else
                    {
                        dt.Rows[i].ItemArray[8].ToString();
                    }
                    SqlCommand cmd = new SqlCommand("INSERT INTO LedgerPayment VALUES('" + dt.Rows[i].ItemArray[0].ToString() + "','" + dt.Rows[i].ItemArray[1].ToString() + "','" + dt.Rows[i].ItemArray[2].ToString() + "','" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','" + dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "'," + dt.Rows[i].ItemArray[7].ToString() + "," + dt.Rows[i].ItemArray[8].ToString() + "," + dt.Rows[i].ItemArray[9].ToString() + "," + dt.Rows[i].ItemArray[10].ToString() + ")", con);
                    Console.WriteLine("Update Ledger " + cmd.CommandText);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch { }
        }
        private void ShowDt(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Console.Write("  " + dt.Rows[i].ItemArray[j].ToString());
                }
                Console.WriteLine();
            }
        }
        #endregion LedgerUpdate
        //LedgerUpdate Closed 

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtInvoice.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            btnNew.Enabled = true;
        }
         
      
        private void txt_remarks_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String query5 = "select * from tbl_SuspenseTransfer where Remarks like '%" + txt_remarks.Text + "%'";
                DataTable df = clsDataLayer.RetreiveQuery(query5);
                if (df.Rows.Count > 0)
                {
                    dataGridView1.DataSource = df;
                }
                else
                {
                    String query6 = "select * from tbl_SuspenseTransfer";
                    DataTable df6 = clsDataLayer.RetreiveQuery(query6);
                    if (df6.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = df6;
                    }
                }
            }
            catch
            {
            }
        }
         
        decimal rp = 0;
        private void txtnature_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
//                Paid
//Received
            if(txtnature.Text.Equals("Paid"))
            {
                AccountName = "Suspense";
            String a1 = "select DueAmount,PaidAmount from PaymentDue where PartyCode='528'"; DataTable dt1 = clsDataLayer.RetreiveQuery(a1); if (dt1.Rows.Count > 0) { txtAvailable.Text = dt1.Rows[0][0].ToString(); rp =Convert.ToDecimal(dt1.Rows[0][1].ToString()); }
            }
            else { AccountName = "Suspense"; String a1 = "select DueAmount,ReceivedAmount from ReceiveDue where PartyCode='528'"; ; DataTable dt1 = clsDataLayer.RetreiveQuery(a1); if (dt1.Rows.Count > 0) { txtAvailable.Text = dt1.Rows[0][0].ToString(); rp = Convert.ToDecimal(dt1.Rows[0][1].ToString()); } }
            }
            catch{}
        }

        //PartyAccount Balance Increase
        public void PayRec()
        {
            try
            {
            //                Paid
            //Received
            decimal transfer = Convert.ToDecimal(txtCashRelease.Text);   decimal available = Convert.ToDecimal(txtAvailable.Text);
            decimal Paid = 0;

            if (sign.Equals("plus"))
            {
                if (txtnature.Text.Equals("Paid"))
                {
                    String sel = "select TotalAmount,DueAmount from PaymentDue where PartyCode='" + Code + "'"; DataTable de = clsDataLayer.RetreiveQuery(sel);
                    if (de.Rows.Count > 0)
                    {
                        decimal Total0 = Convert.ToDecimal(de.Rows[0][0].ToString());
                        decimal CDue = Convert.ToDecimal(de.Rows[0][1].ToString()); decimal DueFinal = CDue + transfer; decimal DueFinal1 = Total0 + transfer;
                        Paid = rp + transfer;
                        String a1 = "update PaymentDue set TotalAmount=" + DueFinal1 + ", DueAmount=" + DueFinal + " where PartyCode='" + Code + "'";
                        clsDataLayer.ExecuteQuery(a1);
                    }
                }
                else
                {
                    String sel = "select TotalAmount,DueAmount from ReceiveDue where PartyCode='" + Code + "'"; DataTable de = clsDataLayer.RetreiveQuery(sel);
                    if (de.Rows.Count > 0)
                    {
                        decimal Total1 = Convert.ToDecimal(de.Rows[0][0].ToString()); decimal DueFinal2 = Total1 + transfer;
                        decimal CDue = Convert.ToDecimal(de.Rows[0][1].ToString()); decimal DueFinal = CDue + transfer; Paid = rp + transfer;
                        String a1 = "update ReceiveDue set TotalAmount=" + DueFinal2 + ",DueAmount=" + DueFinal + " where PartyCode='" + Code + "'"; clsDataLayer.ExecuteQuery(a1);
                    }
                }
            }
            else
            {
            decimal paydue = 0; decimal payRec = 0;
            String sel = "select DueAmount,PaidAmount from PaymentDue where PartyCode='" + Code + "'"; DataTable de = clsDataLayer.RetreiveQuery(sel);
            if (de.Rows.Count > 0)
            {
                paydue = Convert.ToDecimal(de.Rows[0][0].ToString());
                payRec = Convert.ToDecimal(de.Rows[0][1].ToString());
            }

                decimal recdue = 0; decimal rec = 0;
                String sel1 = "select DueAmount,ReceivedAmount from ReceiveDue where PartyCode='" + Code + "'"; DataTable de1 = clsDataLayer.RetreiveQuery(sel1);
            if (de1.Rows.Count > 0)
            {
                recdue = Convert.ToDecimal(de1.Rows[0][0].ToString());
                rec = Convert.ToDecimal(de1.Rows[0][1].ToString());
            }
                

            decimal transfer1 = Convert.ToDecimal(txtCashRelease.Text); decimal available1 = Convert.ToDecimal(txtAvailable.Text);
            decimal Paid1 = payRec + transfer; decimal DueFinal = paydue - transfer;

            decimal Rec4 = rec + transfer; decimal RecDue5 = recdue - transfer;
            if (txtnature.Text.Equals("Paid"))
            { String a1 = "update PaymentDue set DueAmount=" + DueFinal + ",PaidAmount=" + Paid + " where PartyCode='"+Code+"'"; clsDataLayer.ExecuteQuery(a1);}
            else { String a1 = "update ReceiveDue set DueAmount=" + RecDue5 + ",ReceivedAmount=" + Rec4 + " where PartyCode='" + Code + "'"; clsDataLayer.ExecuteQuery(a1); }
            }  
            }
            catch { }
        }

        //SuspenseAccount Decrease
        public void SuspenseIncDec()
        {
            try
            {
            //                Paid
            //Received
            decimal transfer = Convert.ToDecimal(txtCashRelease.Text); decimal available = Convert.ToDecimal(txtAvailable.Text);
            decimal Paid = rp + transfer; decimal DueFinal = available - transfer;
            if (txtnature.Text.Equals("Paid"))
            { String a1 = "update PaymentDue set DueAmount=" + DueFinal + ",PaidAmount=" + Paid + " where PartyCode='528'"; clsDataLayer.ExecuteQuery(a1);
            }else
            {String a1 = "update ReceiveDue set DueAmount=" + DueFinal + ",ReceivedAmount=" + Paid + " where PartyCode='528'"; clsDataLayer.ExecuteQuery(a1);  }
            }
            catch { }
        }

        private void txt_bcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String query5 = "select * from tbl_SuspenseTransfer where TransId like '%" + txt_bcode.Text + "'";
                DataTable df = clsDataLayer.RetreiveQuery(query5);
                if (df.Rows.Count > 0)
                {
                    dataGridView1.DataSource = df;
                }
                else
                {
                    String query6 = "select * from tbl_SuspenseTransfer";
                    DataTable df6 = clsDataLayer.RetreiveQuery(query6);
                    if (df6.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = df6;
                    }
                }
            }
            catch { }
        }

        private void txt_pname_TextChanged(object sender, EventArgs e)
        {
        try
        {
            String query5 = " select * from tbl_SuspenseTransfer where PartyName like '%" + txt_pname.Text + "'";
        DataTable df = clsDataLayer.RetreiveQuery(query5);
        if (df.Rows.Count > 0){  dataGridView1.DataSource = df;}
        else{
            String query6 = "select * from tbl_SuspenseTransfer";
        DataTable df6 = clsDataLayer.RetreiveQuery(query6);
        if (df6.Rows.Count > 0)
        {
            dataGridView1.DataSource = df6;
        }
        }
        }
        catch { }
        }

        private void txt_pamount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String query5 = " select * from tbl_SuspenseTransfer where Amount like '%" + txt_pamount.Text + "'";
                DataTable df = clsDataLayer.RetreiveQuery(query5);
                if (df.Rows.Count > 0)
                { dataGridView1.DataSource = df; }
                else
                {
                    String query6 = "select * from tbl_SuspenseTransfer";
                    DataTable df6 = clsDataLayer.RetreiveQuery(query6);
                    if (df6.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = df6;
                    }
                }
            }
            catch { }
        }
  
       

    }
}
