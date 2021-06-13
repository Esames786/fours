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
using GrayLark.bin.Debug.Report;

namespace GrayLark
{
    public partial class Receipt_voucher  : Form
    {
        String UID = Login.UserID;
        String CashChange = "false";
        double ck = 0; decimal ck1 = 0; bool SaveCk = false;
        String State = "False"; String olddate = "";
        public string GreenSignal = "";
        decimal creditvalue = 0;
        public string FormID = "";
        String Code = "";
        decimal upd1 = 0;
        decimal fcash = 0; decimal fcash1 = 0;
        public Receipt_voucher()
        {
            InitializeComponent();
            clsGeneral.SetAutoCompleteTextBox(txtparty, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 14 and Status = 1 Order BY ID DESC"));
 
            btnSave.Enabled = false;

            try
            {
//                select PartyName from Transactionss
//select CashReceived from Transactionss
//select TransId from Transactionss
//select Remarks from Transactionss

                String a1 = "select PartyName from Transactionss";
                DataTable d1 = clsDataLayer.RetreiveQuery(a1);
                if(d1.Rows.Count > 0)
                { clsGeneral.SetAutoCompleteTextBox(tname, d1); }

                String a2 = "select CashReceived from Transactionss";
                DataTable d2 = clsDataLayer.RetreiveQuery(a2);
                if (d2.Rows.Count > 0)
                { clsGeneral.SetAutoCompleteTextBox(tamount, d2); }

                String a3 = "select TransId from Transactionss";
                DataTable d3 = clsDataLayer.RetreiveQuery(a3);
                if (d3.Rows.Count > 0)
                { clsGeneral.SetAutoCompleteTextBox(tcode, d3); }

                String a4 = "select Remarks from Transactionss";
                DataTable d4 = clsDataLayer.RetreiveQuery(a4);
                if (d4.Rows.Count > 0)
                { clsGeneral.SetAutoCompleteTextBox(tremarks, d4); }

                Disables(); this.KeyPreview = true;
            }
            catch { }
        }
        private bool Disables()
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
                    
                        if (((ComboBox)c).Enabled == true)
                        {
                            ((ComboBox)c).Enabled = false;
                            flag = true;

                        }
                    
                }
            }
            return flag;
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
        {
            ShowAll();
        }

        private void ShowAll()
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT top 20 TransId,Dates,PartyName,CashReceived,Remarks,BankName,AccountNo,ChequeNo,RemainingBalance,UserName,Mode FROM Transactionss order By TransId desc", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
              dataGridView1.DataSource = dt; dataGridView1.PerformLayout(); 
                }
            }
            catch 
            {}
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
                        if (((ComboBox)c).Text != "")
                        {
                            ((ComboBox)c).Text = ""; 
                        } 
                }
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            txt_company.Text = "GrayLark";
            FormID = "Add";
            UserNametesting();
            if (GreenSignal == "YES")
            {
            enable();
            txtAvailable.Text = "";
            txtCashRelease.Text = "";
            txtNewAmount.Text = ""; 
            this.txtparty.Focus();
            btnedit.Enabled = false;
            btnupdate.Enabled = false;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            txtparty.Enabled = true;
            txt_company.Enabled = true;
            tcode.Text = "-";
            tname.Text = "-";
            tamount.Text = "-";
            tremarks.Text = "-";
            try
            {
            Enable(); Clears(); Enable1();
            txtInvoice.Text = clsGeneral.getMAXCode("Transactionss", "TransId", "TR");
            clsGeneral.SetAutoCompleteTextBox(txtparty, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 14 Order BY ID DESC"));
            txtbname.Enabled = false; txtbano.Enabled = false; txtcheque.Enabled = false;
            txtbname.Text = "-"; txtbano.Text = "-"; txtcheque.Text = "-";
            }catch{}
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void enable()
        {   btnNew.Enabled = false; btnSave.Enabled = true;  txtInvoice.Enabled = true;
            comboMode.Enabled = true;   txtparty.Enabled = true;txtRemarks.Enabled = true; }

        private void comboMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            String search = "select * from Transactionss where TransId = '" + txtInvoice.Text + "' ";
            DataTable dt4 = clsDataLayer.RetreiveQuery(search);
            if (dt4.Rows.Count < 1)
            {
                if (txt_company.Text.Equals(""))
                {
                    MessageBox.Show("Please Select Company");
                }
                else
                {
                    if (comboMode.Text == "Cash")
                    {
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tech_cash where CompanyName='" + txt_company.Text + "'", con);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            txtAvailable.Text = dt.Rows[0][0].ToString();
                            txtAvailable.Enabled = true; 

                            txtCashRelease.Enabled = true;
                            txtNewAmount.Enabled = true;
                            txtcheque.Enabled = false;
                            txtbname.Enabled = false;
                            txtbano.Enabled = false; 

                            txtbname.Text = "-";
                            txtbano.Text = "-";
                            txtcheque.Text = "-";
                         }
                        else
                        {
                            txtAvailable.Text = "0";
                            txtAvailable.Enabled = true;
                            txtCashRelease.Enabled = true;
                            txtNewAmount.Enabled = true;
                            txtcheque.Enabled = false;
                            txtbname.Enabled = false;
                            txtbano.Enabled = false; 

                            txtbname.Text = "-";
                            txtbano.Text = "-";
                            txtcheque.Text = "-"; 
                         }
                    }

                    if (comboMode.Text == "Credit Card")
                    {
                   // clsGeneral.SetAutoCompleteTextBox(txtcreditparty, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 12 Order BY ID DESC"));
                     txtcheque.Enabled = true;
                    txtbname.Enabled = true;
                    txtbano.Enabled = true;
                    txtbname.Text = "";
                    txtbano.Text = "";
                    txtcheque.Text="";
                    txtCashRelease.Enabled = false; 

                    String query = "select BankName from BankDetail ";
                    DataTable dq = clsDataLayer.RetreiveQuery(query);
                    if (dq.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtbname, dq);
                    }
                    else { } 

                    }
                    if (comboMode.Text == "Emergency")
                    {
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tech_cash where CompanyName='Emergency'", con);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            txtAvailable.Enabled = true;
                            txtCashRelease.Enabled = true;
                              txtNewAmount.Enabled = true;
                            txtcheque.Enabled = false;
                            txtbname.Enabled = false;
                            txtbano.Enabled = false;
                            txtbname.Text = "-";
                            txtbano.Text = "-";
                            txtcheque.Text = "-";
                            txtAvailable.Text = dt.Rows[0][0].ToString();
                         
                        }
                        else
                        {
                            txtAvailable.Text = "0";
                            txtAvailable.Enabled = true;
                              txtCashRelease.Enabled = true;
                            txtNewAmount.Enabled = true;
                            txtcheque.Enabled = false;
                            txtbname.Enabled = false;
                            txtbano.Enabled = false;
                            txtbname.Text = "-";
                            txtbano.Text = "-";
                            txtcheque.Text = "-"; 
                        }
                    }
                    //
                    if (comboMode.Text == "BKHOME")
                    {
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tech_cash where CompanyName='BKHOME'", con);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            txtAvailable.Enabled = true;
 
                            txtCashRelease.Enabled = true;
                            txtNewAmount.Enabled = true;
                            txtcheque.Enabled = false;
                            txtbname.Enabled = false;
                            txtbano.Enabled = false;
                            txtbname.Text = "-";
                            txtbano.Text = "-";
                            txtcheque.Text = "-";
                            txtAvailable.Text = dt.Rows[0][0].ToString();
                         
                        }
                        else
                        {
                            txtAvailable.Text = "0";
                            txtAvailable.Enabled = true;
 
                            txtCashRelease.Enabled = true;
                            txtNewAmount.Enabled = true;
                            txtcheque.Enabled = false;
                            txtbname.Enabled = false;
                            txtbano.Enabled = false;
                            txtbname.Text = "-";
                            txtbano.Text = "-";
                            txtcheque.Text = "-";
                             
                        }
                    }
                     
                    if (comboMode.Text == "Karachi Delivery")
                    {
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tech_cash where CompanyName='Karachi Delivery'", con);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            txtAvailable.Enabled = true;
                            txtCashRelease.Enabled = true;
                            txtNewAmount.Enabled = true;
                            txtcheque.Enabled = false;
                            txtbname.Enabled = false;
                            txtbano.Enabled = false;
                            txtbname.Text = "-";
 
                            txtbano.Text = "-";
                            txtcheque.Text = "-";
                            txtAvailable.Text = dt.Rows[0][0].ToString();
                          
                        }
                        else
                        {
                            txtAvailable.Text = "0";
                            txtAvailable.Enabled = true;
                            txtCashRelease.Enabled = true;
                            txtNewAmount.Enabled = true;
                            txtcheque.Enabled = false;
                            txtbname.Enabled = false;
                            txtbano.Enabled = false;
                            txtbname.Text = "-"; 

                            txtbano.Text = "-";
                            txtcheque.Text = "-"; 
                        }
                    }

                    if (comboMode.Text == "US")
                    {
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tech_cash where CompanyName='US'", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                    txtAvailable.Enabled = true;
                    txtCashRelease.Enabled = true;
                    txtNewAmount.Enabled = true;
                    txtcheque.Enabled = false;
                    txtbname.Enabled = false;

                     txtbano.Enabled = false;
                    txtbname.Text = "-";
                    txtbano.Text = "-";
                    txtcheque.Text = "-"; 
                    txtAvailable.Text = dt.Rows[0][0].ToString();
                    }
                    else
                    {
                    txtAvailable.Text = "0";
                    txtAvailable.Enabled = true;
                    txtCashRelease.Enabled = true;
                    txtNewAmount.Enabled = true;
                    txtcheque.Enabled = false; 

                        txtbname.Enabled = false;
                    txtbano.Enabled = false;
                    txtbname.Text = "-";
                    txtbano.Text = "-";
                    txtcheque.Text = "-"; 
                    }
                    }

                    //
                    if (comboMode.Text == "Bank")
                    {
                        txtAvailable.Text = "";
                        txtNewAmount.Text = "";
                         
                        txtAvailable.Enabled = true;
                        txtCashRelease.Enabled = true;
                        txtNewAmount.Enabled = true;
                        txtcheque.Enabled = true;
                        txtcheque.Text = "";
                        txtbano.Text = "";
                        txtbname.Text = "";
                        txtbname.Enabled = true;
                        txtbano.Enabled = true; 
                        String query = "select BankName from BankDetail ";
                        DataTable dq = clsDataLayer.RetreiveQuery(query);
                        if (dq.Rows.Count > 0)
                        {
                            clsGeneral.SetAutoCompleteTextBox(txtbname, dq);
                        }else {  } 
                    }
                }
            }
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
                double r = 0;
                r=Convert.ToDouble(txtCashRelease.Text);
                double final = 0;
                //if(r > ck)
                //{
                    final =Math.Abs(r - ck);
                    double a = 0;
                a=Convert.ToDouble(txtAvailable.Text);
                    double na = a + final;
                    //String ans = na.ToString();
                    //if (ans.StartsWith("-"))
                    //{
                    //    txtNewAmount.Text = "0";
                    //    txtCashRelease.Text = "0";
                    //}
                 //   else
                   // {
                        txtNewAmount.Text = na.ToString();
                    //}
                //}
               // else if (r < ck)
               // {
               //     final = ck - r;
               //     double a = Convert.ToDouble(txtAvailable.Text);
               //     double na = a - final;
               //     //String ans = na.ToString();
               //     //if (ans.StartsWith("-"))
               //     //{
               //     //    txtNewAmount.Text = "0";
               //     //    txtCashRelease.Text = "0";
               //     ////}
               //    // else
               //    // {
               //         txtNewAmount.Text = na.ToString();
               //   //  }
               //// }
                 if (r == ck)
                {
                    txtNewAmount.Text = txtAvailable.Text;
                }
             
            }
            catch (Exception ex)
            {
                string name = ex.Message;
            }
        }

        private void CreditCashRelease()
        {
            try
            {
                //double r = Convert.ToDouble(txtccashin.Text);
                //double final = 0;
                ////if(r > ck
                ////{
                //final = Math.Abs(r + ck);
                //double a = Convert.ToDouble(txt_cavailable.Text);
                //double na = a - final;
                //String ans = na.ToString();
                //if (ans.StartsWith("-"))
                //{
                //    txtcremain.Text = txt_cavailable.Text;
                //    txtccashin.Text = "0";
                //}
                //else
                //{
                //    txtcremain.Text = na.ToString();

                //    if (r == ck)
                //    {
                //        txtcremain.Text = txt_cavailable.Text;
                //    }
                //}
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
                         if (((ComboBox)c).Text == "")
                        {
                            flag = true;
                            break;
                        }
                     
                }
            }
            return flag;
        }

        private void PartyCode(String title)
        {
            String sel = "select ActCode from Accounts where ActTitle = '" + title + "'";
            DataTable dc = clsDataLayer.RetreiveQuery(sel);
            if (dc.Rows.Count > 0)
            {
                Code = dc.Rows[0][0].ToString();
            }
        }

        private void ReceiveUpdateDue()
        {
            //
            String h = "select HeaderActCode from Accounts where ActTitle = '" + txtparty.Text + "'";
            DataTable dq = clsDataLayer.RetreiveQuery(h);
            decimal c = 0;
            bool req = false;
            PartyCode(txtparty.Text);
            decimal avv = PartyBalance();
            if (dq.Rows.Count > 0)
            {
                int head = Convert.ToInt32(dq.Rows[0][0]);
                if (head == 20101 || head == 10101)
                {
                    req = true;
                   
                }
                else
                {
                    if (avv > 0)
                    {
                        req = true;
                    }
                    else
                    {
                        if (avv > -0)
                        {
                            req = true;

                        }
                        else
                        {
                            req = false;
                        }
                    }
                }
                if (req == true)
                {
                    String seldues = "Select * from ReceiveDue where PartyCode='" + Code + "'";
                    DataTable dxs = clsDataLayer.RetreiveQuery(seldues);
                    if (dxs.Rows.Count > 0)
                    {
                        State = "True";
                        decimal totals = Convert.ToDecimal(dxs.Rows[0]["TotalAmount"].ToString());
                        decimal dues = Convert.ToDecimal(dxs.Rows[0]["DueAmount"].ToString());
                        decimal paids = Convert.ToDecimal(dxs.Rows[0]["ReceivedAmount"].ToString());
                        decimal pays = Convert.ToDecimal(txtCashRelease.Text);
                        if (totals <= paids)
                        {
                            MessageBox.Show("No Amount Is Due To Pay!");
                        }
                        else
                        { 
                            paids = (paids - upd1) + pays;
                            dues = totals - paids;
                            String qs = "update ReceiveDue set DueAmount=" + dues + ",ReceivedAmount=" + paids + " where PartyCode='" + Code + "'";
                            clsDataLayer.ExecuteQuery(qs);
                        }
                    }
                }
            }
          
        }

        private void ReceiveDue()
        {
            //
            decimal c = 0;
            PartyCode(txtparty.Text);
            decimal avv = PartyBalance();
            bool req = false;

            if (!Code.Equals("528"))
            {
                String h = "select HeaderActCode from Accounts where ActTitle = '" + txtparty.Text + "'";
                DataTable dq = clsDataLayer.RetreiveQuery(h);
                if (dq.Rows.Count > 0)
                {
                    int head = Convert.ToInt32(dq.Rows[0][0]);
                    if (head == 20101 || head == 10101)
                    {
                        req = true;
                    }
                    else
                    {
                        if (avv > 0)
                        {
                            req = true;
                        }
                        else
                        {
                            if (avv < 0)
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
            }
            else { req = true; }

                if (req == true)
                {
                    String seldues = "Select * from ReceiveDue where PartyCode='" + Code + "'";
                    DataTable dxs = clsDataLayer.RetreiveQuery(seldues);
                    if (dxs.Rows.Count > 0)
                    {
                        decimal totals = Convert.ToDecimal(dxs.Rows[0]["TotalAmount"].ToString());
                        decimal dues = Convert.ToDecimal(dxs.Rows[0]["DueAmount"].ToString());
                        decimal paids = Convert.ToDecimal(dxs.Rows[0]["ReceivedAmount"].ToString());
                        decimal pays = Convert.ToDecimal(txtCashRelease.Text);
                        
                         
                            if (Code.Equals("528"))
                            {
                                dues += pays;
                                totals += pays;
                                String qs = "update ReceiveDue set DueAmount=" + dues + ",TotalAmount=" + totals + " where PartyCode='" + Code + "'";
                                clsDataLayer.ExecuteQuery(qs);
                            }
                            else
                            {
                                dues -= pays;
                                paids += pays;
                                String qs = "update ReceiveDue set DueAmount=" + dues + ",ReceivedAmount=" + paids + " where PartyCode='" + Code + "'";
                                clsDataLayer.ExecuteQuery(qs);
                            }
                        
                    }
                    else
                    {
                    if (Code.Equals("528"))
                    {
                        String ii = @"insert into ReceiveDue (RemCode,PartyName,PartyCode,TotalAmount,DueAmount,ReceivedAmount,Company_Name)
                values('" + txtInvoice.Text + "','" + txtparty.Text + "','" + Code + "'," + txtCashRelease.Text + "," + txtCashRelease.Text + ",0,'Reddot Technologies')";
                        clsDataLayer.ExecuteQuery(ii);
                    }
                    }
                }

        }

        public void paydue()
        {
          //  PartyCode(txtcreditparty.Text);
            String seldues = "Select * from PaymentDue where PartyCode='" + Code + "'";
            DataTable dxs = clsDataLayer.RetreiveQuery(seldues);
            if (dxs.Rows.Count > 0)
            {
                decimal totals = Convert.ToDecimal(dxs.Rows[0]["TotalAmount"].ToString());
                decimal dues = Convert.ToDecimal(dxs.Rows[0]["DueAmount"].ToString());
                decimal paids = Convert.ToDecimal(dxs.Rows[0]["PaidAmount"].ToString());
                decimal pays = Convert.ToDecimal(txtCashRelease.Text);

                
                    dues += pays;
                    totals += pays;
                    String qs = "update PaymentDue set DueAmount=" + dues + ",TotalAmount=" + totals + " where PartyCode='" + Code + "'";
                    clsDataLayer.ExecuteQuery(qs);
          
            }
            else
            {
                if (comboMode.Text == "Credit Card")
                {
                    String ii = @"insert into PaymentDue (PayCode,PartyName,PartyCode,TotalAmount,DueAmount,PaidAmount,Company_Name)
                            values('" + txtInvoice.Text + "','" + txtparty.Text + "','" + Code + "'," + txtCashRelease.Text + "," + txtCashRelease.Text + ",0,'graylark')";
                    clsDataLayer.ExecuteQuery(ii);
                }
            }
        }

        decimal balance = 0;
       public decimal PartyBalance()
              {
                  try
                  {
                      String get = "select Party_Balance from LedgerReceived where RefCode='" + Code + "' order by ID desc";
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

        private void ledger(){
        PartyCode(txtparty.Text);
        decimal a = PartyBalance();
        decimal b = Convert.ToDecimal(txtCashRelease.Text);
        decimal c = 0; String Status="";
        if (!Code.Equals("528"))
        {
            String h = "select HeaderActCode from Accounts where ActTitle = '" + txtparty.Text + "'";
            DataTable dq = clsDataLayer.RetreiveQuery(h);
            if (dq.Rows.Count > 0)
            {
                int head = Convert.ToInt32(dq.Rows[0][0]);
                if (head == 20101 || head == 10101)
                {
                    c = a - b;
                }
                else
                {
                    if (a > 0)
                    {
                        c = a - b;
                    }
                    else
                    {
                        if (a < 0)
                        {
                            c = a - b;
                        }
                        else
                        {
                            c = 0;
                        }
                    }
                }
            }
            Status = "OnReceived";
        }
        else { c = a + b; Status = "OnCredit";}

        String ins = @"insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
          " values('" + txtRemarks.Text + "','" + txtInvoice.Text + "','" + vdate.Value.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtInvoice.Text + "','Cash'," + txtCashRelease.Text + ",0.00,'OnReceived'," + c + ",'" + txt_company.Text + "') insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
          " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtRemarks.Text + "','" + txtInvoice.Text + "','" + vdate.Value.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtInvoice.Text + "','" + txtparty.Text + "'," + txtCashRelease.Text + ",0.00,'OnReceived' ," + c + ",'" + txt_company.Text + "')";
          if(clsDataLayer.ExecuteQuery(ins) > 0)
          {
           MessageBox.Show("Payment Received Successfully!","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
          }
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        { 
            if (!CheckAllFields())
            { 
            //if (txtNewAmount.Text.StartsWith("-"))
            //{
            //    MessageBox.Show("You Can't Cross Your Credit Card Limit!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            //}
            //else { 
                save();
            //}
             
            }
            else { MessageBox.Show("Fill All Fields!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error); }
         }

        private void save()
        {
            FormID = "Save";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                if (!CheckAllFields())
                {
      String getname = "select * from Accounts where ActTitle='" + txtparty.Text + "'";
      DataTable de = clsDataLayer.RetreiveQuery(getname);
      if (de.Rows.Count < 1)
      {
          MessageBox.Show("UserName dont have a chart Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      else
      {
      try
      {
          txtInvoice.Text = clsGeneral.getMAXCode("Transactionss", "TransId", "TR"); btnSave.Enabled = false;
          PartyCode(txtparty.Text);
          string query = "";
          query = "insert into Transactionss (TransId,Dates,BankName,AccountNo,ChequeNo,RemainingBalance,PartyName,CashReceived,Remarks,UserName,Mode,Company_Name,VoucherStatus)values ('" + txtInvoice.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtbname.Text + "','" + txtbano.Text + "','" + txtcheque.Text + "','" + txtNewAmount.Text + "','" + txtparty.Text + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + UID + "','" + comboMode.Text + "','" + txt_company.Text + "','" + cbmstatus.Text + "')";
          clsDataLayer.ExecuteQuery(query);
          String sl = "select RemainingAmount,ReceiveAmount from tblInvoice where InCode='" + txtpo.Text + "'"; DataTable d9 = clsDataLayer.RetreiveQuery(sl);
          if (d9.Rows.Count > 0)
          {
              decimal remain = Convert.ToDecimal(d9.Rows[0][0].ToString()); decimal paid = Convert.ToDecimal(d9.Rows[0][1].ToString()); decimal remainrel = Convert.ToDecimal(txtCashRelease.Text);
              decimal finalrel = remain - remainrel; decimal fpaid = paid + remainrel;
              String pstatus = "";
              if (finalrel == 0)
              {
                  pstatus = "Received";
              }
              else
              {
                  pstatus = "Invoice";
              }


              String upd1 = "update tblInvoice set status='"+pstatus+"' , RemainingAmount=" + finalrel + ",ReceiveAmount=" + fpaid + " where InCode='" + txtpo.Text + "'"; clsDataLayer.ExecuteQuery(upd1);
          
          }
      }
      catch (Exception e) { MessageBox.Show(e.Message); }
      
          if (comboMode.Text == "Cash")
          {
              try
              {
                  String cashget = "insert into CashIn (VDate,CashCode,PartyName,Dates,CashIn,Remarks,UserName,Company_Name) values ('" + vdate.Value.ToString("dd-MMMM-yyyy") + "','" + txtInvoice.Text + "','" + txtparty.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + Login.UserID + "','" + txt_company.Text + "')";
                  clsDataLayer.ExecuteQuery(cashget);
                  String name = "";
                    name = txt_company.Text;
                    String ist = "insert into CashTransaction(CashCode,PartyName,Dates,Cash,CurrentCash,Remarks,Status,UserName) values('" + txtInvoice.Text + "','" + txtparty.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtNewAmount.Text + "','" + txtRemarks.Text + "','Received','" + UID + "')";
                  clsDataLayer.ExecuteQuery(ist);
                  string query = "";
                  query = "UPDATE tech_cash SET CASH = '" + txtNewAmount.Text + "' where CompanyName = '" + name + "'";
                  clsDataLayer.ExecuteQuery(query);
                  ReceiveDue();
                  ledger();
              }
              catch (Exception e) { MessageBox.Show(e.Message); } MessageBox.Show("Rs." + txtCashRelease.Text + " Release to " + txtparty.Text + " And New Amount in " + comboMode.Text + " is Rs." + txtNewAmount.Text + "!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtparty.Clear();
                    disable();
                    ShowAll();
                }
                else if (comboMode.Text == "Bank" || comboMode.Text == "Credit Card")
                {
                    try
                    {
                        //
                    String AcNo = ""; 
                    AcNo = txtbano.Text; 
                      
                     string query3 = "update AccountBalance set RemainingBalance=" + txtNewAmount.Text + " where AccountNo='" + txtbano.Text + "'";
                       clsDataLayer.ExecuteQuery(query3);

                       String q1s = "insert into BankCashIn(VDate,BankCode,PartyName,BankName,AccountNo,ChequeNo,Dates,CashIn,Remarks,UserName,Company_Name) values ('" + vdate.Value.ToString("dd-MMMM-yyyy") + "','" + txtInvoice.Text + "','" + txtparty.Text + "','" + txtbname.Text + "','" + AcNo + "','" + txtcheque.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + Login.UserID + "','" + txt_company.Text + "')";
                    clsDataLayer.ExecuteQuery(q1s);

                    String inst = "insert into BankTransaction(BankCode,PartyName,BankName,AccountNo,ChequeNo,Dates,BankCash,CurrentCash,Remarks,Status,UserName)values('" + txtInvoice.Text + "','" + txtparty.Text + "','" + txtbname.Text + "','" + AcNo + "','" + txtcheque.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtNewAmount.Text + "','" + txtRemarks.Text + "','Received','" + UID + "')";
                    clsDataLayer.ExecuteQuery(inst);
                    ReceiveDue();
                    ledger();               
                 }
                    catch (Exception e) { MessageBox.Show(e.Message); }
                    //if (!txtInvoice.Text.Equals(""))
                    //{
                    //    if (comboMode.Text.Equals("Bank"))
                    //    {
                    //        string q = "select * from BankCashIn where BankCode='" + txtInvoice.Text + "'";
                    //        DataTable purchase = clsDataLayer.RetreiveQuery(q);
                    //        if (purchase.Rows.Count > 0)
                    //        {
                    //            rptReceiptBank rpt = new rptReceiptBank();
                    //            rpt.SetDataSource(purchase);
                    //            PaymentView pop = new PaymentView(rpt);
                    //            pop.Show();
                    //        }
                    //        else
                    //        {
                    //            MessageBox.Show("No Record Found!");
                    //        }
                    //    }
                    //    else
                    //    {
            
                    //        string q = "select * from CashIn where CashCode='" + txtInvoice.Text + "'";
                    //        DataTable purchase = clsDataLayer.RetreiveQuery(q);
                    //        if (purchase.Rows.Count > 0)
                    //        {
                    //            rptReceiptCash rpt = new rptReceiptCash();
                    //            rpt.SetDataSource(purchase);
                    //            PaymentView pop = new PaymentView(rpt);
                    //            pop.Show();
                    //        }
                    //        else
                    //        {
                    //            MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Please Fill Pay Id!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                    MessageBox.Show("Rs." + txtCashRelease.Text + " Receive for " + txtparty.Text + " And New Amount in " + comboMode.Text + " is Rs." + txtNewAmount.Text + "!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtparty.Clear();
                    disable();
                    ShowAll();
            
                }
                //  }
                txtparty.Text = "";
                btnedit.Enabled = true;
                btnupdate.Enabled = true;
                btnNew.Enabled = true;
                btnSave.Enabled = false;
                tcode.Text = "-";
                tname.Text = "-";
                tamount.Text = "-";
                tremarks.Text = "-";
                this.Hide();
                Receipt_voucher pv = new Receipt_voucher(); pv.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Please Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void disable()
        {
            btnNew.Enabled = true;
            btnSave.Enabled = false;
            txtInvoice.Enabled = false;
     
            comboMode.Enabled = false;
            txtAvailable.Enabled = false;
            txtCashRelease.Enabled = false;

            txtbname.Enabled = false;
            txtbano.Enabled = false; 
            txtcheque.Enabled = false; 

            txtNewAmount.Enabled = false;
            txtRemarks.Enabled = false;
            txtInvoice.Clear();
    
            comboMode.SelectedIndex = -1;
            txtAvailable.Clear();
            txtCashRelease.Clear();
       
            txtNewAmount.Clear();
            txtRemarks.Clear();
        }

         

        private void txtbano_TextChanged(object sender, EventArgs e)
        {
            if (comboMode.Text != "Cash")
            {
                String query = "select RemainingBalance from AccountBalance where AccountNo = '" + txtbano.Text + "'";
                DataTable dc = clsDataLayer.RetreiveQuery(query);
                if (dc.Rows.Count > 0)
                {
                    txtAvailable.Text = dc.Rows[0][0].ToString();
                }
                else
                {
                    txtAvailable.Text = "Account Number Incorrect";
                }
            }
        }

        private void txtCashRelease_Leave(object sender, EventArgs e)
        {
        if(txtCashRelease.Text.Equals(""))
        {
            txtNewAmount.Text = txtAvailable.Text;
            txtCashRelease.Text = "0";
        }
        txtRemarks.Focus();
        }
          
        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                FormID = "Print";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    if (!txtInvoice.Text.Equals(""))
                    {
                        if (comboMode.SelectedIndex == 0)
                        {
                            string q = "select * from CashIn where CashCode='" + txtInvoice.Text + "'";
                            DataTable purchase = clsDataLayer.RetreiveQuery(q);
                            if (purchase.Rows.Count > 0)
                            {
                                rptPaymentCash rpt = new rptPaymentCash();
                                rpt.SetDataSource(purchase);
                                PaymentView pop = new PaymentView(rpt);
                                pop.Show();
                            }
                            else
                            {
                                MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            string q = "select * from BankCashIn where BankCode='" + txtInvoice.Text + "'";
                            DataTable purchase = clsDataLayer.RetreiveQuery(q);
                            if (purchase.Rows.Count > 0)
                            {
                                rptPayment rpt = new rptPayment();
                                rpt.SetDataSource(purchase);
                                PaymentView pop = new PaymentView(rpt);
                                pop.Show();
                            }
                            else
                            {
                                MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Fill Receipt Id!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            FormID = "Edit";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                String search = "select TransId from Transactionss";
                DataTable dt = clsDataLayer.RetreiveQuery(search);
                if (dt.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtInvoice, dt);
                }
                dataGridView1.Enabled = false;
                txtInvoice.ReadOnly = false;
                txtInvoice.Enabled = true;
                
                btnedit.Enabled = false;
                btnupdate.Enabled = true;
                btnNew.Enabled = false;
                btnSave.Enabled = false;
                tcode.Text = "-";
                tname.Text = "-";
                tamount.Text = "-";
                tremarks.Text = "-";

                if (comboMode.Text == "Cash")
                {
                    String name = "";
                      name = txt_company.Text;
                     decimal ab = Convert.ToDecimal(txtCashRelease.Text);
                    decimal abc = Convert.ToDecimal(txtAvailable.Text);
                    decimal a1 = Convert.ToDecimal(abc + ab);
                    txtAvailable.Text = a1.ToString();
                    decimal abcd = Convert.ToDecimal(txtAvailable.Text);
                    decimal final = abcd + ab;
                   txtNewAmount.Text = txtAvailable.Text;
                }
                else if (comboMode.Text == "Bank")
                {
                      fcash = Convert.ToDecimal(txtCashRelease.Text);  
                    String quers = "select RemainingBalance from AccountBalance where BankName='" + txtbname.Text + "' and AccountNo='" + txtbano.Text + "'";
                    DataTable dte = clsDataLayer.RetreiveQuery(quers);
                    if (dte.Rows.Count > 0)
                    {
                        String csh = dte.Rows[0][0].ToString();
                        txtAvailable.Text = csh;
                    //    CashRelease();
                     }

                    //Changes
                    decimal ab = Convert.ToDecimal(txtCashRelease.Text);
                    //decimal abc = Convert.ToDecimal(txtAvailable.Text);
                    //decimal a1 = Convert.ToDecimal(abc + ab);
                    //txtAvailable.Text = a1.ToString();
                    
                    decimal abcd = Convert.ToDecimal(txtAvailable.Text);
                    decimal final = abcd + ab;
                    //  txtNewAmount.Text = final.ToString();
                    txtNewAmount.Text = txtAvailable.Text;
                }
                ck = Convert.ToDouble(txtCashRelease.Text); ck1 = Convert.ToDecimal(txtCashRelease.Text);
                txtparty.Enabled = false;
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
             txtCashRelease.Enabled = true; txtRemarks.Enabled = true;
            txtparty.Enabled = false;
            dataGridView1.Enabled = false;
            txtInvoice.ReadOnly = false;
            txtInvoice.Enabled = false;      btnedit.Enabled = false;
            btnupdate.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = false;
            tcode.Text = "-";
            tname.Text = "-";
            tamount.Text = "-";
            tremarks.Text = "-"; cbmstatus.Enabled = true;

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
            if (((ComboBox)c).Enabled == false)
            {
                ((ComboBox)c).Enabled = true;
                flag = true; 
            } 
            } 
            }
            return flag;
        }
        private bool Enable1()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel4.Controls)
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


        private void BankTransUpdate()
        {
            String Code = txtInvoice.Text; decimal amount = Convert.ToDecimal(txtCashRelease.Text);
            String upd1 = "update BankTransaction set BankCash=" + amount + " where BankCode='" + Code + "'"; clsDataLayer.ExecuteQuery(upd1);
            String BID = ""; String bno = txtbano.Text;
            decimal oldc = 0;
            String q0 = "select BankIn from BankTransaction where AccountNo='" + bno + "' and BankCode='" + Code + "' Order by BankIn desc"; DataTable d0 = clsDataLayer.RetreiveQuery(q0); if (d0.Rows.Count > 0) { BID = d0.Rows[0][0].ToString(); }
            String sf = "select BankIn,CurrentCash from BankTransaction where AccountNo='" + bno + "' and BankIn < " + BID + " Order by BankIn desc"; DataTable df = clsDataLayer.RetreiveQuery(sf);
            if (df.Rows.Count > 0) { oldc = Convert.ToDecimal(df.Rows[0][1].ToString()); }

            String q1 = "select CurrentCash from BankTransaction where AccountNo='" + bno + "' and BankIn < " + BID + " Order by BankIn desc"; DataTable d1 = clsDataLayer.RetreiveQuery(q1);
            if (d1.Rows.Count > 0)
            {
                String SID = ""; decimal oldb = 0; String Status = ""; decimal rel = 0; String BCode = "";
                String s1 = "select BankIn from BankTransaction where BankCode='" + Code + "'"; DataTable s2 = clsDataLayer.RetreiveQuery(s1); if (s2.Rows.Count > 0) { SID = s2.Rows[0][0].ToString(); }
                String q2 = "select BankIn,BankCode,Status,BankCash,CurrentCash from BankTransaction where AccountNo='" + bno + "' and BankIn >= " + SID + " Order by BankIn asc"; DataTable d2 = clsDataLayer.RetreiveQuery(q2); if (d2.Rows.Count > 0)
                {
                    for (int b = 0; b < d2.Rows.Count; b++)
                    {
                        BCode = d2.Rows[b][1].ToString(); Status = d2.Rows[b][2].ToString(); rel = Convert.ToDecimal(d2.Rows[b][3].ToString());
                        if (b == 0)
                        {
                            if (Status.Equals("Paid"))
                            {
                                oldb = oldc - rel;
                            }
                            else
                            {
                                oldb = oldc + rel;
                            }
                        }
                        else
                        {
                            if (Status.Equals("Paid"))
                            {
                                oldb = oldb - rel;
                            }
                            else
                            {
                                oldb = oldb + rel;
                            }
                        }
                        String upd = "update BankTransaction set CurrentCash=" + oldb + " where BankCode='" + BCode + "'"; clsDataLayer.ExecuteQuery(upd);
                    }
                    String query30 = "update AccountBalance set RemainingBalance=" + oldb + " where AccountNo='" + txtbano.Text + "'";
                    clsDataLayer.ExecuteQuery(query30);
                }
            }
        }

        private void CashTransUpdate()
        {
            String Code = txtInvoice.Text; decimal amount = Convert.ToDecimal(txtCashRelease.Text);
            String upd1 = "update CashTransaction set Cash=" + amount + " where CashCode='" + Code + "'"; clsDataLayer.ExecuteQuery(upd1);
            String BID = ""; String bno = txtbano.Text;
            decimal oldc = 0;
            String q0 = "select CashIn from CashTransaction where CashCode='" + Code + "' Order by CashIn desc"; DataTable d0 = clsDataLayer.RetreiveQuery(q0); if (d0.Rows.Count > 0) { BID = d0.Rows[0][0].ToString(); }
            String sf = "select CashIn,CurrentCash from CashTransaction where CashIn < " + BID + " Order by CashIn desc"; DataTable df = clsDataLayer.RetreiveQuery(sf);
            if (df.Rows.Count > 0) { oldc = Convert.ToDecimal(df.Rows[0][1].ToString()); }

            //String q1 = "select CurrentCash from CashTransaction where CashIn < " + BID + " Order by CashIn desc"; DataTable d1 = clsDataLayer.RetreiveQuery(q1);
            //if (d1.Rows.Count > 0)
            //{
                String SID = ""; decimal oldb = 0; String Status = ""; decimal rel = 0; String BCode = "";
                String s1 = "select CashIn from CashTransaction where CashCode='" + Code + "'"; DataTable s2 = clsDataLayer.RetreiveQuery(s1); if (s2.Rows.Count > 0) { SID = s2.Rows[0][0].ToString(); }
                String q2 = "select CashIn,CashCode,Status,Cash,CurrentCash from CashTransaction where CashIn >= " + SID + " Order by CashIn asc"; DataTable d2 = clsDataLayer.RetreiveQuery(q2); if (d2.Rows.Count > 0)
                { 
                    for (int b = 0; b < d2.Rows.Count; b++)
                    {
                        BCode = d2.Rows[b][1].ToString(); Status = d2.Rows[b][2].ToString(); rel = Convert.ToDecimal(d2.Rows[b][3].ToString());
                        if (b == 0)
                        {
                            if (Status.Equals("Paid"))
                            {
                                oldb = oldc - rel;
                            }
                            else
                            {
                                oldb = oldc + rel;
                            }
                        }
                        else
                        {
                            if (Status.Equals("Paid"))
                            {
                                oldb = oldb - rel;
                            }
                            else
                            {
                                oldb = oldb + rel;
                            }
                        }
                        String upd = "update CashTransaction set CurrentCash=" + oldb + " where CashCode='" + BCode + "'"; clsDataLayer.ExecuteQuery(upd);
                    }
                    string query1 = "";
                    query1 = "UPDATE tech_cash SET CASH = '" + oldb + "' where CompanyName = 'graylark' ";
                    clsDataLayer.ExecuteQuery(query1);
                }
             
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            FormID = "Update";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                try
                {
                    String getname = "select * from Accounts where ActTitle='" + txtparty.Text + "'";
                    DataTable de = clsDataLayer.RetreiveQuery(getname);
                    if (de.Rows.Count < 1)
                    {
                        MessageBox.Show("UserName dont have a chart Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (!CheckAllFields())
                        {
                           
                  comboMode.Enabled = false;
                  this.txtparty.Focus();
                  String query = "Delete From Transactionss Where TransId = '" + txtInvoice.Text + "' Delete From CashIn Where CashCode = '" + txtInvoice.Text + "' Delete From BankCashIn Where BankCode = '" + txtInvoice.Text + "'  ";
                  clsDataLayer.ExecuteQuery(query);

                  //String q1 = "delete from BankTransaction where BankCode='" + txtInvoice.Text + "'";
                  //clsDataLayer.ExecuteQuery(q1);
                    try
                      {
                          PartyCode(txtparty.Text); 
                          query = "insert into Transactionss (TransId,Dates,BankName,AccountNo,ChequeNo,RemainingBalance,PartyName,CashReceived,Remarks,UserName,Mode,Company_Name,VoucherStatus)values ('" + txtInvoice.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtbname.Text + "','" + txtbano.Text + "','" + txtcheque.Text + "','" + txtNewAmount.Text + "','" + txtparty.Text + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + UID + "','" + comboMode.Text + "','" + txt_company.Text + "','" + cbmstatus.Text + "')";
                          clsDataLayer.ExecuteQuery(query);
                          String sl = "select RemainingAmount,ReceiveAmount from tblInvoice where InCode='" + txtpo.Text + "'"; DataTable d9 = clsDataLayer.RetreiveQuery(sl);
                          if (d9.Rows.Count > 0)
                          {
                              decimal remain = Convert.ToDecimal(d9.Rows[0][0].ToString()); decimal paid = Convert.ToDecimal(d9.Rows[0][1].ToString()); decimal remainrel = Convert.ToDecimal(txtCashRelease.Text);
                              decimal finalrel = remain + ck1 - remainrel; decimal fpaid = paid  - ck1 + remainrel;
                              String pstatus = "";
                              if (finalrel == 0)
                              {
                                  pstatus = "Received";
                              }
                              else
                              {
                                  pstatus = "Invoice";
                              }


                              String upd1 = "update tblInvoice set status='" + pstatus + "' , RemainingAmount=" + finalrel + ",ReceiveAmount=" + fpaid + " where InCode='" + txtpo.Text + "'"; clsDataLayer.ExecuteQuery(upd1);
          
                            }
                      } catch {  }

                    if (comboMode.Text == "Cash" )
                    {
                    try
                    {
                        String cashget = "insert into CashIn (VDate,CashCode,PartyName,Dates,CashIn,Remarks,UserName,Company_Name) values ('" + vdate.Value.ToString("dd-MMMM-yyyy") + "','" + txtInvoice.Text + "','" + txtparty.Text + "','" + olddate + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + Login.UserID + "','" + txt_company.Text + "')";
                    
                    clsDataLayer.ExecuteQuery(cashget);
                    String name = "";
                  name = txt_company.Text;
                    if (CashChange.Equals("true"))
                    {
                        CashTransUpdate();
                    }
                    ReceiveUpdateDue();
                    //String ist = "insert into CashTransaction(CashCode,PartyName,Dates,Cash,CurrentCash,Remarks,Status,UserName) values('" + txtInvoice.Text + "','" + txtparty.Text + "','" + olddate + "','" + txtCashRelease.Text + "','" + txtNewAmount.Text + "','" + txtRemarks.Text + "','Received','" + UID + "')";
                    //                    clsDataLayer.ExecuteQuery(ist);
                                      //  ledger();
                                    }
                                    catch  {  }
                                    MessageBox.Show("Rs." + txtCashRelease.Text + " Release to " + txtparty.Text + " And New Amount in " + comboMode.Text + " is Rs." + txtNewAmount.Text + "!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //txtparty.Clear();
                                }

                                else if (comboMode.Text == "Bank")
                                {
                                    try
                                    {
                                        String q1s = "insert into BankCashIn(VDate,BankCode,PartyName,BankName,AccountNo,ChequeNo,Dates,CashIn,Remarks,UserName,Company_Name) values ('" + vdate.Value.ToString("dd-MMMM-yyyy") + "','" + txtInvoice.Text + "','" + txtparty.Text + "','" + txtbname.Text + "','" + txtbano.Text + "','" + txtcheque.Text + "','" + olddate + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + Login.UserID + "','" + txt_company.Text + "')";

                                        clsDataLayer.ExecuteQuery(q1s);
                                        if (CashChange.Equals("true"))
                                        { 
                                            BankTransUpdate();
                                         }
                                        ReceiveUpdateDue(); 
                                     //   ledger();
                                    }
                                    catch
                                    {

                                    }
                                    //if (!txtInvoice.Text.Equals(""))
                                    //{
                                    //    if (comboMode.Text.Equals("Bank"))
                                    //    {
                                    //        string q = "select * from BankCashIn where BankCode='" + txtInvoice.Text + "'";
                                    //        DataTable purchase = clsDataLayer.RetreiveQuery(q);
                                    //        if (purchase.Rows.Count > 0)
                                    //        {
                                    //            rptReceiptBank rpt = new rptReceiptBank();
                                    //            rpt.SetDataSource(purchase);
                                    //            PaymentView pop = new PaymentView(rpt);
                                    //            pop.Show();
                                    //        }
                                    //        else
                                    //        {
                                    //            MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //        }
                                    //    }
                                    //    else
                                    //    {

                                    //        string q = "select * from CashIn where CashCode='" + txtInvoice.Text + "'";
                                    //        DataTable purchase = clsDataLayer.RetreiveQuery(q);
                                    //        if (purchase.Rows.Count > 0)
                                    //        {
                                    //            rptReceiptCash rpt = new rptReceiptCash();
                                    //            rpt.SetDataSource(purchase);
                                    //            PaymentView pop = new PaymentView(rpt);
                                    //            pop.Show();
                                    //        }
                                    //        else
                                    //        {
                                    //            MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //        }
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    MessageBox.Show("Please Fill Pay Id!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //}
                                    MessageBox.Show("Rs." + txtCashRelease.Text + " Receive for " + txtparty.Text + " And New Amount in " + comboMode.Text + " is Rs." + txtNewAmount.Text + "!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                 
                             

                                }
                           // }

                    decimal a2 = 0;
                    if (State.Equals("False"))
                    {
                        decimal a1 = Convert.ToDecimal(txtCashRelease.Text);
                        a2 = 0 - a1;
                    }
                    else
                    {
                        a2 = Convert.ToDecimal(txtCashRelease.Text);
                    }

                    if (comboMode.Text == "Bank")
                    {
                        fcash1 = Convert.ToDecimal(txtCashRelease.Text);

                    }

                    //if (fcash != fcash1)
                    //{
                        int index = GetRowIndex(txtInvoice.Text);
                        DataTable dt = SetDT();
                        ShowDt(dt);
                        DeleteRecord();
                        dt.Rows[index][7] = Convert.ToDecimal(txtCashRelease.Text);
                        dt.Rows[index + 1][8] = Convert.ToDecimal(txtCashRelease.Text);
                        dt = SomeOperation(dt);
                        InsertRecord(dt);
                  //  }
                            if (comboMode.Text == "Credit Card"){ PaymentUpdateDue();}
                            txtparty.Clear();
                            disable();
                            ShowAll();
                            MessageBox.Show("Payment Update Received Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnedit.Enabled = true;
                            btnupdate.Enabled = false;
                            btnNew.Enabled = true;
                            tcode.Text = "-";
                            tname.Text = "-";
                            tamount.Text = "-";
                            tremarks.Text = "-";
                            dataGridView1.Enabled = true;
                            CashChange = "false";
                             }
                        else
                        {
                            MessageBox.Show("Please Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }  
                    }
                }
                catch
                {

                }
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public void PaymentUpdateDue()
        {
           // PartyCode(txtcreditparty.Text);
             decimal c = 0;
             //
            String state = "false";
            // 
        String seldues = "Select * from PaymentDue where PartyCode='" + Code + "'";
        DataTable dxs = clsDataLayer.RetreiveQuery(seldues);
        if (dxs.Rows.Count > 0)
        {
            state = "true";
            decimal totals = Convert.ToDecimal(dxs.Rows[0]["TotalAmount"].ToString());
            decimal dues = Convert.ToDecimal(dxs.Rows[0]["DueAmount"].ToString());
            decimal paids = Convert.ToDecimal(dxs.Rows[0]["PaidAmount"].ToString());
            decimal pays = Convert.ToDecimal(txtCashRelease.Text);

            dues = (dues - creditvalue) + pays;
            totals = (totals - creditvalue) + pays;

            String qs = "update PaymentDue set DueAmount=" + dues + ",TotalAmount=" + paids + " where PartyCode='" + Code + "'";
            clsDataLayer.ExecuteQuery(qs);
        }
               
        }


        public void search()
        {
            try
            {
                String search = "select * from Transactionss where TransId = '" + txtInvoice.Text + "' ";
                DataTable dt = clsDataLayer.RetreiveQuery(search);
                if (dt.Rows.Count > 0)
                { 
           txtInvoice.Enabled = false;
           txtInvoice.ReadOnly = true;
           enable();
           olddate = dt.Rows[0]["Dates"].ToString();
           vdate.Text = dt.Rows[0]["Dates"].ToString();
           txtparty.Text = dt.Rows[0]["PartyName"].ToString();
           txtbname.Text = dt.Rows[0]["BankName"].ToString();
           txtbano.Text = dt.Rows[0]["AccountNo"].ToString();
           txtCashRelease.Text = dt.Rows[0]["CashReceived"].ToString();
           txtcheque.Text = dt.Rows[0]["ChequeNo"].ToString();
           txtNewAmount.Text = dt.Rows[0]["RemainingBalance"].ToString();
           txt_company.Text = dt.Rows[0]["Company_Name"].ToString();
           cbmstatus.Text = dt.Rows[0]["VoucherStatus"].ToString();
           txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
           decimal aa = Convert.ToDecimal(dt.Rows[0]["CashReceived"].ToString());
           decimal bb = Convert.ToDecimal(dt.Rows[0]["RemainingBalance"].ToString());
           decimal cc = bb - aa;
           txtAvailable.Text = cc.ToString();
           if (!txtbname.Text.Equals("-"))
           {
               comboMode.Text = dt.Rows[0]["Mode"].ToString();
               txtAvailable.Enabled = true;
               txtCashRelease.Enabled = true;
               txtNewAmount.Enabled = true;
               txtcheque.Enabled = true;
               txtbname.Enabled = true;
               txtbano.Enabled = true;
           }
           else
           {
               comboMode.Text = dt.Rows[0]["Mode"].ToString();
               txtAvailable.Enabled = true;
               txtCashRelease.Enabled = true;
               txtNewAmount.Enabled = true;
               txtcheque.Enabled = false;
               txtbname.Enabled = false;
               txtbano.Enabled = false;
               txtbname.Text = "-";
               txtbano.Text = "-";
               txtcheque.Text = "-";
           }
           enable();
           btnSave.Enabled = false;
           btnNew.Enabled = true;
           btnedit.Enabled = true;
           btnupdate.Enabled = false;
           upd1 = Convert.ToDecimal(txtCashRelease.Text);
           Disables();
           //  Enable();
          }
          }
            catch { }
        }

        private void Searching(object sender, EventArgs e)
        {
            search();
        }

        //Ledger Update
        #region LedgerUpdate
        private int GetRowIndex(string input)
        {
            int index = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT V_No FROM LedgerReceived  where RefCode = '" + Code + "' order by ID asc", con);
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
                SqlCommand cmd = new SqlCommand("DELETE FROM LedgerReceived  where RefCode = '" + Code + "'", con);
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
                SqlDataAdapter cmd = new SqlDataAdapter("select V_No, Datetime, AccountCode, RefCode, PaymentType, InvoiceNo, Particulars, Debit, Credit, Party_Balance,Company_Name,Description from LedgerReceived  where RefCode = '" + Code + "' order by ID asc", con);
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
                        if (type.Equals("OnCredit"))
                        {
                            dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[7]);
                        }
                        else
                        {
                            dt.Rows[i][9] = 0 - Convert.ToDecimal(dt.Rows[i].ItemArray[7]);
                        }
                    }
                    else if (i == 1)
                    {
                        if (type.Equals("OnCredit"))
                        {
                            dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[8]);
                        }
                        else
                        {
                            dt.Rows[i][9] = 0 - Convert.ToDecimal(dt.Rows[i].ItemArray[8]);
                        }
                    } 
                    else
                    {
                        //
                        String h2 = "select HeaderActCode from Accounts where ActTitle = '" + txtparty.Text + "'";
                        DataTable dqs = clsDataLayer.RetreiveQuery(h2);

                        if (dqs.Rows.Count > 0)
                        {
                            int head = Convert.ToInt32(dqs.Rows[0][0]);
                            if (head == 20101 || head == 10101)
                            {
                                if (type.Equals("OnCredit"))
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

                    SqlCommand cmd = new SqlCommand("INSERT INTO LedgerReceived  VALUES('" + dt.Rows[i].ItemArray[0].ToString() + "','" + dt.Rows[i].ItemArray[1].ToString() + "','" + dt.Rows[i].ItemArray[2].ToString() + "','" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','" + dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "'," + dt.Rows[i].ItemArray[7].ToString() + "," + dt.Rows[i].ItemArray[8].ToString() + "," + dt.Rows[i].ItemArray[9].ToString() + ",'" + dt.Rows[i].ItemArray[10].ToString() + "','" + dt.Rows[i].ItemArray[11].ToString() + "')", con);
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


        private void txtbname_Leave(object sender, EventArgs e)
        {
            try
            {
                String query = "select AccountNo from BankDetail where BankName = '" + txtbname.Text + "'";
                DataTable dq = clsDataLayer.RetreiveQuery(query);
                if (dq.Rows.Count > 0)
                { 
                //    clsGeneral.SetAutoCompleteTextBox(txtbano, dq);
                    txtbano.Text = dq.Rows[0][0].ToString();
                }
                else
                {
                    MessageBox.Show("Bank Detail Not Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                }
                txtbano.Focus();
            }
            catch { }
        }

        private void btnprint_Click(object sender, EventArgs e)
        {   try
            {
                FormID = "Print";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    if (!txtInvoice.Text.Equals(""))
                    {
                        if (comboMode.Text.Equals("Bank"))
                        {
                            string q = "select * from BankCashIn where BankCode='" + txtInvoice.Text + "'";
                            DataTable purchase = clsDataLayer.RetreiveQuery(q);
                            if (purchase.Rows.Count > 0)
                            {
                                rptReceiptBank rpt = new rptReceiptBank();
                                rpt.SetDataSource(purchase);
                                PaymentView pop = new PaymentView(rpt);
                                pop.Show();
                            }
                            else
                            {
                                MessageBox.Show("No Record Found!");
                            }
                        }
                        else
                        {

                            string q = "select * from CashIn where CashCode='" + txtInvoice.Text + "'";
                            DataTable purchase = clsDataLayer.RetreiveQuery(q);
                            if (purchase.Rows.Count > 0)
                            {
                                rptReceiptCash rpt = new rptReceiptCash();
                                rpt.SetDataSource(purchase);
                                PaymentView pop = new PaymentView(rpt);
                                pop.Show();
                            }
                            else
                            {
                                MessageBox.Show("No Record Found!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Fill Pay Id!");
                    }
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            Clears();
        } 

        private void txtInvoice_TextChanged(object sender, EventArgs e)
        {
            search();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        { 
          txtInvoice.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }


        private void total(String column, String value)
        {
            String query5 = " select sum(CashReceived) from Transactionss where " + column + " = '" + value + "'";
            DataTable dt = clsDataLayer.RetreiveQuery(query5);
            if (dt.Rows.Count > 0)
            {
                decimal cg = Convert.ToDecimal(dt.Rows[0][0].ToString());
                lblamount.Text = cg.ToString();
            }
        }

        private void tcode_TextChanged(object sender, EventArgs e)
        {
            try
            { 
                String query5 = " select * from Transactionss where TransId like '%" + tcode.Text + "'";
               
                DataTable df = clsDataLayer.RetreiveQuery(query5);
                if (df.Rows.Count > 0)
                {
                    total("TransId", tcode.Text);

                    dataGridView1.DataSource = df;
                }
                else
                {
                    String query6 = "select * from Transactionss";
                    DataTable df6 = clsDataLayer.RetreiveQuery(query6);
                    if (df6.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = df6;
                    }
                }
            }
            catch{  }
        }

        private void tname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String query5 = " select * from Transactionss where PartyName like '%" + tname.Text + "'";
                DataTable df = clsDataLayer.RetreiveQuery(query5);
                if (df.Rows.Count > 0)
                {
                    total("PartyName", tname.Text);
                     dataGridView1.DataSource = df;
                }
                else
                {
                    String query6 = "select * from Transactionss";
                    DataTable df6 = clsDataLayer.RetreiveQuery(query6);
                    if (df6.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = df6;
                    }
                }
            }catch{  }
        }

        private void tamount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String query5 = " select * from Transactionss where CashReceived like '%" + tamount.Text + "'";
                DataTable df = clsDataLayer.RetreiveQuery(query5);
                if (df.Rows.Count > 0)
                {
                    total("CashReceived", tamount.Text);
                     dataGridView1.DataSource = df;}
                else
                {
                    String query6 = "select * from Transactionss";
                    DataTable df6 = clsDataLayer.RetreiveQuery(query6);
                    if (df6.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = df6;
                    }
                }
            }catch{}
        }

        private void tremarks_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String query5 = " select * from Transactionss where Remarks like '%" + tremarks.Text + "%'";
                DataTable df = clsDataLayer.RetreiveQuery(query5);
                if (df.Rows.Count > 0)
                {
                 //   total("Remarks", tremarks.Text);

                    dataGridView1.DataSource = df;
                }
                else
                {
                    String query6 = "select * from Transactionss";
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

        private void txtcheque_TextChanged(object sender, EventArgs e)
        {

        }
         

        private void txtccashin_TextChanged(object sender, EventArgs e)
        {
            CreditCashRelease();
          //  txtCashRelease.Text = txtccashin.Text;
        }

        private void txtparty_Leave(object sender, EventArgs e)
        {
            PartyCode(txtparty.Text); 
           String balance ="";
            balance="select DueAmount from ReceiveDue where PartyCode='"+Code+"'";
            DataTable dr = clsDataLayer.RetreiveQuery(balance);
           if(dr.Rows.Count > 0)
           {
               lblamount.Text = Convert.ToDecimal(dr.Rows[0][0]).ToString();
           } 
        }

        private void btntop_Click(object sender, EventArgs e)
        {
            try
            { 
                String hm = "SELECT top 20 TransId,Dates,PartyName,CashReceived,Remarks,BankName,AccountNo,ChequeNo,RemainingBalance,UserName,Mode FROM Transactionss";
                DataTable dt = clsDataLayer.RetreiveQuery(hm);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
            }
            catch
            { }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            try
            { 
                String hm = "SELECT top 20 TransId,Dates,PartyName,CashReceived,Remarks,BankName,AccountNo,ChequeNo,RemainingBalance,UserName,Mode FROM Transactionss Order By TransId desc";
                DataTable dt = clsDataLayer.RetreiveQuery(hm);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
            }
            catch
            { }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            try
            {
            String hm = "SELECT TransId,Dates,PartyName,CashReceived,Remarks,BankName,AccountNo,ChequeNo,RemainingBalance,UserName,Mode FROM Transactionss";
            DataTable dt = clsDataLayer.RetreiveQuery(hm);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }
            }
            catch
            { }
        }

        private void Receipt_voucher_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void txtCashRelease_KeyPress_1(object sender, KeyPressEventArgs e)
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

        private void Receipt_voucher_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.A)
            {
                btnNew.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.S)
            {
                btnSave.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.E)
            {
                btnedit.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.U)
            {
                btnupdate.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.P)
            {
                btnprint.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        private void comboMode_Leave(object sender, EventArgs e)
        {
            try { txtbname.Focus(); }
            catch { } 
        }

        private void txtbano_Leave(object sender, EventArgs e)
        { 
            try { txtcheque.Focus(); }
            catch { } 
        }

        private void txtcheque_Leave(object sender, EventArgs e)
        {
            try { txtCashRelease.Focus(); }
            catch { } 
           
        }

        private void txtRemarks_Leave(object sender, EventArgs e)
        {
            try { cbmstatus.Focus(); }
            catch { } 
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DCInvoiceSearch dc = new DCInvoiceSearch(this,txtparty.Text); dc.Show();
        }
 
    
         //LedgerUpdate Closed

    }
}
