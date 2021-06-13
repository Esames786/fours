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
using System.Net;
using System.Net.Mail;

namespace GrayLark
{
    public partial class Payment_voucher : Form
    {
        String UID = Login.UserID;
        public string GreenSignal = "";
        public string FormID = "";
        decimal ck = 0; String Stb = "";
       decimal old = 0; String HCode = "";
        decimal fcash = 0; decimal fcash1 = 0;
        String ProjCode = "";

        String CashChange = "false"; String OLDDATE = "";
        String Code = ""; String PartyNames = ""; String Mode = "";
        
        public Payment_voucher()
        {
        InitializeComponent();
        try
        {
        clsGeneral.SetAutoCompleteTextBox(txtparty, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 14 and Status = 1 Order BY ID DESC"));
        String a1 = "select PartyName from Transactions"; DataTable d1 = clsDataLayer.RetreiveQuery(a1);
        if (d1.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txt_pname, d1); }
        String a2 = "select CashGiven from Transactions"; DataTable d2 = clsDataLayer.RetreiveQuery(a2);
        if (d2.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txt_pamount, d2); }
        
        String a3 = "select TransId from Transactions";
        DataTable d3 = clsDataLayer.RetreiveQuery(a3);
        if (d3.Rows.Count > 0)
        { clsGeneral.SetAutoCompleteTextBox(txt_bcode, d3); }
        
        String a4 = "select Remarks from Transactions";
        DataTable d4 = clsDataLayer.RetreiveQuery(a4);
        if (d4.Rows.Count > 0)
        { clsGeneral.SetAutoCompleteTextBox(txt_remarks, d4); }
        
        String a12 = "select ChequeNo from Transactions";
        DataTable d12 = clsDataLayer.RetreiveQuery(a12);
        if (d12.Rows.Count > 0)
        { clsGeneral.SetAutoCompleteTextBox(txtcno, d12); }
        Disable();
        this.KeyPreview = true;
        }catch { }
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
                String hm = "SELECT top 20 TransId,Dates,PartyName,CashGiven,Remarks,BankName,AccountNo,ChequeNo,RemainingBalance,UserName,Mode FROM Transactions order by TransId desc";
                DataTable dt = clsDataLayer.RetreiveQuery(hm);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.PerformLayout();
                }
                else
                {
                    dataGridView1.DataSource = null; dataGridView1.PerformLayout();
                }
            }
            catch { }
        }

        private void OnNew()
        {
            txt_company.Text = "GrayLark";
            FormID = "Add"; 

            UserNametesting();
            if (GreenSignal == "YES")
            {
                enable();
                this.txtparty.Focus();
                try
                {
                    Clears();
                    txtInvoice.Text = clsGeneral.getMAXCode("Transactions", "TransId", "TP");
                    btnEdit.Enabled = false;      btnupdate.Enabled = false;
                    btnSave.Enabled = true;  btnNew.Enabled = false;    txt_company.Enabled = true;
                    txt_bcode.Text = "-"; txt_pname.Text = "-";  txt_pamount.Text = "-"; txt_remarks.Text = "-";  Enable();
                    txtbname.Enabled = false; txtbano.Enabled = false; txtcheque.Enabled = false;
                    txtbname.Text = "-"; txtbano.Text = "-"; txtcheque.Text = "-"; btnpost.Enabled = false;
                    txtagainst.Text = "-"; 
                    String vno = "select TransId from Transactions";
                    DataTable d22 = clsDataLayer.RetreiveQuery(vno); if (d22.Rows.Count > 0)
                    { clsGeneral.SetAutoCompleteTextBox(txtagainst, d22); }
                    cmbStatus.Text = "Pending"; txtAvailable.Enabled = false; txtparty.Enabled = true; txtpo.Text = "-"; btnpo.Enabled = true;
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
        try { OnNew(); } catch { }
        }

        private void enable()
        {
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            txtInvoice.Enabled = true;
            //date1.Enabled = true;
            //date2.Enabled = true; 
            comboMode.Enabled = true;
            txtparty.Enabled = true;
            txtRemarks.Enabled = true;
        }

        private void comboMode_SelectedIndexChanged(object sender, EventArgs e)
        {
        try
        {
        String search = "select * from Transactions where TransId = '" + txtInvoice.Text + "' ";
        DataTable dtr = clsDataLayer.RetreiveQuery(search);
        if (dtr.Rows.Count < 1)
        { 
        if (txt_company.Text.Equals(""))
        {
            MessageBox.Show("Please Select Company");
        }
        else
        {
         if (comboMode.Text == "Cash")
         {
         String h = "SELECT * FROM tech_cash where CompanyName='" + txt_company.Text + "'";
         DataTable dt = clsDataLayer.RetreiveQuery(h);

         if (dt.Rows.Count > 0)
         {
             txtAvailable.Text = dt.Rows[0][0].ToString(); 
             cmbStatus.Text = "Paid";
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
             MessageBox.Show("No Cash Available!");
         }
         }
         if (comboMode.Text == "Emergency")
         {
             String h = "SELECT * FROM tech_cash where CompanyName='Emergency'";
             DataTable dt = clsDataLayer.RetreiveQuery(h);

             if (dt.Rows.Count > 0)
             { 
             cmbStatus.Text = "Paid";
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
                 MessageBox.Show("No Cash Available!");
             }
         }
         if (comboMode.Text == "BKHOME")
         {
             String h = "SELECT * FROM tech_cash where CompanyName='BKHOME'";
             DataTable dt = clsDataLayer.RetreiveQuery(h);

             if (dt.Rows.Count > 0)
             { 
             cmbStatus.Text = "Paid";
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
                 MessageBox.Show("No Cash Available!");
             }
         }

         if (comboMode.Text == "Karachi Delivery")
         {
             String h = "SELECT * FROM tech_cash where CompanyName='Karachi Delivery'";
             DataTable dt = clsDataLayer.RetreiveQuery(h);

             if (dt.Rows.Count > 0)
             { 
             cmbStatus.Text = "Paid";
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
                 MessageBox.Show("No Cash Available!");
             }
         }
         if (comboMode.Text == "US")
         {
             SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tech_cash where CompanyName='US'", con);
             DataTable dt = new DataTable();
             sda.Fill(dt);
             if (dt.Rows.Count > 0)
             { 
             cmbStatus.Text = "Paid";
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


         if (comboMode.Text == "Bank")
         { 
             cmbStatus.Text = "Pending";
             txtbname.Text = "";
             txtbano.Text = "";
             txtcheque.Text = "";
             txtAvailable.Text = "";
             txtNewAmount.Text = "";
             txtAvailable.Enabled = true;
             txtCashRelease.Enabled = true;
             txtNewAmount.Enabled = true;
             txtcheque.Enabled = true;
             txtbname.Enabled = true;
             txtbano.Enabled = true;

             String query = "select BankName from BankDetail ";
             DataTable dq = clsDataLayer.RetreiveQuery(query);
             if (dq.Rows.Count > 0)
             {
                 clsGeneral.SetAutoCompleteTextBox(txtbname, dq);
             }
             else
             {
                 MessageBox.Show("Bank Detail Not Found!");
             }
         }
         //  }
         }
         }
         }
        catch { }
        
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
                decimal r = 0;
                r=Convert.ToDecimal(txtCashRelease.Text);
                decimal final = 0;
                //if (r > ck)
                //{
                    final = r - ck;
                    decimal a = 0;
                a=Convert.ToDecimal(txtAvailable.Text);
                    decimal na = a - r;
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
                     
                        if (((ComboBox)c).Text == "")
                        {
                            flag = true;
                            break;
                        }
                     
                }
            }
            return flag;
        }

        private void ReceiveDue()
        {
            try
            {
                PartyCode(txtparty.Text);
                String rec = "select * from ReceiveDue where PartyCode = '" + Code + "'";
                DataTable d = clsDataLayer.RetreiveQuery(rec);
                if (d.Rows.Count > 0)
                {
                    decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
                    decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
                    decimal received = decimal.Parse(d.Rows[0]["ReceivedAmount"].ToString());
                    decimal final = Convert.ToDecimal(txtCashRelease.Text);
                    total = total - ck + final;
                    due = due - ck + final;
                    string updateblnc = "update ReceiveDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                    clsDataLayer.ExecuteQuery(updateblnc);
                }
                else
                {
                    decimal b = Convert.ToDecimal(txtCashRelease.Text);
                    String ii = @"insert into ReceiveDue (RemCode,PartyName,PartyCode,TotalAmount,DueAmount,ReceivedAmount,Company_Name)
                values('" + txtInvoice.Text + "','" + txtparty.Text + "','" + Code + "'," + b + "," + b + ",0,'graylark')";
                    clsDataLayer.ExecuteQuery(ii);
                }
            }
            catch { }
        }

        decimal balance3 = 0;
        public decimal PartyBalance3()
        {
        try
        {
        String get = "select Party_Balance from LedgerReceived where RefCode='" + Code + "' order by ID desc";
        DataTable dfr = clsDataLayer.RetreiveQuery(get);
        if (dfr.Rows.Count > 0)
        {
        balance3 = decimal.Parse(dfr.Rows[0]["Party_Balance"].ToString());
        }  else  {  balance3 = 0;  }
       
        }
        catch { } return balance;
        }

        private void ReceiveLedger()
        {
        ReceiveDue();
        decimal a2 = PartyBalance3();
        decimal b2 = Convert.ToDecimal(txtCashRelease.Text);
        decimal c2 = a2 + b2;
        String VoucherStatus = "OnCredit";
        String ins = @"insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
        " values('" + txtRemarks.Text + "','" + txtInvoice.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtInvoice.Text + "','" + txtparty.Text + "'," + b2 + ",0.00,'" + VoucherStatus + "'," + c2 + ",'graylark') insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
        " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtRemarks.Text + "','" + txtInvoice.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','01010205','" + Code + "','" + txtInvoice.Text + "','Receiveable'," + b2 + ",0.00,'" + VoucherStatus + "' ," + c2 + ",'graylark')";
        clsDataLayer.ExecuteQuery(ins); 
        }

        private void SaveClick()
        {
            FormID = "Save";
            UserNametesting();

            if (GreenSignal == "YES")
            {
                decimal available = Convert.ToDecimal(txtAvailable.Text);
                decimal paid = Convert.ToDecimal(txtNewAmount.Text);
                if (paid < available)
                {
                    if (!CheckAllFields())
                    {
                        String getname = "select RemainingLimit,OpeningBalance from Accounts where ActTitle='" + txtparty.Text + "'";
                        DataTable de = clsDataLayer.RetreiveQuery(getname);
                        if (de.Rows.Count < 1)
                        {
                            MessageBox.Show("UserName dont have a chart Account","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        }
                        else
                        {
                            //
                            save();
                            //
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Paid Amount Can not be greater than Available Amount!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
      
  }

        private void dsa()
        {
            try
            {
                // Credentials
                var credentials = new NetworkCredential("salmanidrees012@gmail.com", "salman@dosani123");

                // Mail message
                var mail = new System.Net.Mail.MailMessage()
                {
                    From = new MailAddress("salmanidrees012@gmail.com"),
                    Subject = "Pharmacy Software Make Payment Voucher Company : "   +  txt_company.Text +   "   "  +    DateTime.Now.ToString("dd-MM-yyyy HH:MM"),
                    Body = "Check this Attachement"
                };
                mail.To.Add(new MailAddress("salmanidrees012@gmail.com"));

                // Smtp client
                var client = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = credentials
                };

                // Send it...         
                client.Send(mail);
            }
            catch
            { }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckAllFields())
                {
                    SaveClick(); 
                }
                else { MessageBox.Show("Fill All Fields"); }
            }
            catch { }
         }
         
        private void save()
        {
        try
        {
            String Status = "";
            String second = ""; String scode = "";
            int chelength = txtcheque.Text.Length;
            if (chelength > 2)
            {
                Status = "UNPOST";
            }
            else
            {
               txtInvoice.Text = clsGeneral.getMAXCode("Transactions", "TransId", "TP");
       Status = "NILL";
       if (comboMode.Text.Equals("Bank"))
       {
           String query9 = ""; query9 = "update AccountBalance set RemainingBalance=" + txtNewAmount.Text + " where AccountNo='" + txtbano.Text + "'"; clsDataLayer.ExecuteQuery(query9);
       }
            }
            PartyCode(txtparty.Text);

            btnNew.Enabled = true; btnSave.Enabled = false;
            string query = "";
            txtInvoice.Text = clsGeneral.getMAXCode("Transactions", "TransId", "TP"); btnSave.Enabled = false;

            query = "insert into Transactions (PInvoice,HeadActCode,VDate,PDate,TransId,Dates,BankName,AccountNo,ChequeNo,RemainingBalance,PartyName,CashGiven,Remarks,UserName,Mode,Company_Name,Status,PostedDate,VoucherStatus,AgainstVoucher)values ('"+txtpo.Text+"','" + HCode + "','" + vdate.Value.ToString("dd-MMMM-yyyy") + "','" + dt9.Value.ToString("dd-MMMM-yyyy") + "','" + txtInvoice.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtbname.Text + "','" + txtbano.Text + "','" + txtcheque.Text + "','" + txtNewAmount.Text + "','" + txtparty.Text + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + UID + "','" + comboMode.Text + "','" + txt_company.Text + "','" + Status + "','" + dt9.Value.ToString("yyyy-MM-dd") + "','" + cmbStatus.Text + "','" + txtagainst.Text + "')";
            clsDataLayer.ExecuteQuery(query); 
             //
            String sl = "select RemainingAmount,PaidAmount from Purchase where PurCode='" + txtpo.Text + "'"; DataTable d9 = clsDataLayer.RetreiveQuery(sl);
            if (d9.Rows.Count > 0)
            {
                decimal remain = Convert.ToDecimal(d9.Rows[0][0].ToString()); decimal paid = Convert.ToDecimal(d9.Rows[0][1].ToString()); decimal remainrel = Convert.ToDecimal(txtCashRelease.Text);
                decimal finalrel = remain - remainrel; decimal fpaid = paid + remainrel;
                String upd1 = "update Purchase set RemainingAmount=" + finalrel + ",PaidAmount=" + fpaid + " where PurCode='" + txtpo.Text + "'"; clsDataLayer.ExecuteQuery(upd1);
            }
            //

            if (comboMode.Text == "Cash")
            {
            try
            {
                second = "Cash"; scode = "01010204";
                string query3 = "insert into CashRelease(VDate,CashRel,PartyName,Dates,CashGiven,Remarks,UserName,Company_Name) values ('" + vdate.Value.ToString("dd-MMMM-yyyy") + "','" + txtInvoice.Text + "','" + txtparty.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + UID + "','" + txt_company.Text + "')";
            clsDataLayer.ExecuteQuery(query3);
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
            try
            { 
                      String name = txt_company.Text;
                      String ist = "insert into CashTransaction(CashCode,PartyName,Dates,Cash,CurrentCash,Remarks,Status,UserName) values('" + txtInvoice.Text + "','" + txtparty.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtNewAmount.Text + "','" + txtRemarks.Text + "','Paid','" + UID + "')";
                    clsDataLayer.ExecuteQuery(ist);
                    string querya = "";

                    querya = "UPDATE tech_cash SET CASH = '" + txtNewAmount.Text + "' where CompanyName = '" + name + "'";
                    clsDataLayer.ExecuteQuery(querya);
                }
            catch (Exception e) { MessageBox.Show(e.Message); } MessageBox.Show("Rs." + txtCashRelease.Text + " Release to " + txtparty.Text + " And New Amount in " + comboMode.Text + " is Rs." + txtNewAmount.Text + "!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else if (comboMode.Text == "Bank")
            {
                try
                {
                    second = "Bank"; scode = "01010208";
                    if (Status.Equals("UNPOST"))
                    {
                        string bcv = ""; bcv = "insert into BCR(VDate,BankRel,PartyName,BankName,AccountNo,ChequeNo,Dates,CashGiven,Remarks,UserName,Company_Name) VALUES('" + vdate.Value.ToString("dd-MMMM-yyyy") + "','" + txtInvoice.Text + "','" + txtparty.Text + "','" + txtbname.Text + "','" + txtbano.Text + "','" + txtcheque.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + UID + "','" + txt_company.Text + "')";
                        clsDataLayer.ExecuteQuery(bcv);
                    }
                    else
                    {
                        string bcv = ""; bcv = "insert into BCR(VDate,BankRel,PartyName,BankName,AccountNo,ChequeNo,Dates,CashGiven,Remarks,UserName,Company_Name) VALUES('" + vdate.Value.ToString("dd-MMMM-yyyy") + "','" + txtInvoice.Text + "','" + txtparty.Text + "','" + txtbname.Text + "','" + txtbano.Text + "','" + txtcheque.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + UID + "','" + txt_company.Text + "')";
                        clsDataLayer.ExecuteQuery(bcv);
                        string query9 = "";
                        query9 = "insert into BankCashRelease(VDate,BankRel,PartyName,BankName,AccountNo,ChequeNo,Dates,CashGiven,Remarks,UserName,Company_Name) VALUES('" + vdate.Value.ToString("dd-MMMM-yyyy") + "','" + txtInvoice.Text + "','" + txtparty.Text + "','" + txtbname.Text + "','" + txtbano.Text + "','" + txtcheque.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + UID + "','" + txt_company.Text + "')";
                        clsDataLayer.ExecuteQuery(query9);
                        String inst = "insert into BankTransaction(BankCode,PartyName,BankName,AccountNo,ChequeNo,Dates,BankCash,CurrentCash,Remarks,Status,UserName)values('" + txtInvoice.Text + "','" + txtparty.Text + "','" + txtbname.Text + "','" + txtbano.Text + "','" + txtcheque.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtNewAmount.Text + "','" + txtRemarks.Text + "','Paid','" + UID + "')";
                        clsDataLayer.ExecuteQuery(inst);
                      
                    }
                }
                catch (Exception e) { MessageBox.Show(e.Message); }
                MessageBox.Show("Rs." + txtCashRelease.Text + " Release for " + txtparty.Text + " And New Amount in " + comboMode.Text + " is Rs." + txtNewAmount.Text + "!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            PartyCode(txtparty.Text);
            if (HCode.Equals("2"))
            {
                ReceiveLedger();
            }
            decimal a = PartyBalance();
            decimal c = 0;
            int head = 0;
            bool req = false;
            if (!Code.Equals("528"))
            {
                String h = "select HeaderActCode from Accounts where ActTitle = '" + txtparty.Text + "'";
                DataTable dq = clsDataLayer.RetreiveQuery(h);

                if (dq.Rows.Count > 0)
                {
                    head = Convert.ToInt32(dq.Rows[0][0]);
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
            }
            else { req = true; }

            if (req == true)
            {
                String seldues = "Select * from PaymentDue where PartyCode='" + Code + "'";
                DataTable dxs = clsDataLayer.RetreiveQuery(seldues);
                if (dxs.Rows.Count > 0)
                {
                    decimal totals = Convert.ToDecimal(dxs.Rows[0]["TotalAmount"].ToString());
                    decimal dues = Convert.ToDecimal(dxs.Rows[0]["DueAmount"].ToString());
                    decimal paids = Convert.ToDecimal(dxs.Rows[0]["PaidAmount"].ToString());
                    decimal pays = Convert.ToDecimal(txtCashRelease.Text);
                    if (head == 10101)
                    {
                        totals = 0; dues = 0; paids = 0; pays = 0;
                    }
                    if (Code.Equals("528"))
                    {
                        dues += pays;
                        totals += pays;
                        String qs = "update PaymentDue set DueAmount=" + dues + ",TotalAmount=" + totals + " where PartyCode='" + Code + "'";
                        clsDataLayer.ExecuteQuery(qs);
                    }
                    else
                    {
                          int cklen = txtcheque.Text.Length;
                          if (cklen < 4)
                          {
                              dues -= pays;
                              paids += pays;
                              String qs = "update PaymentDue set DueAmount=" + dues + ",PaidAmount=" + paids + " where PartyCode='" + Code + "'";
                              clsDataLayer.ExecuteQuery(qs);
                          }
                    }

                }
                else
                {
                    if (Code.Equals("528"))
                    {
                        String ii = @"insert into PaymentDue (PayCode,PartyName,PartyCode,TotalAmount,DueAmount,PaidAmount,Company_Name)
                            values('" + txtInvoice.Text + "','" + txtparty.Text + "','" + Code + "'," + txtCashRelease.Text + "," + txtCashRelease.Text + ",0,'Reddot Technologies')";
                        clsDataLayer.ExecuteQuery(ii);
                    }
                }

            } 
            //     } 
            //} 
            String status = "";
            decimal b = Convert.ToDecimal(txtCashRelease.Text);
            if (!Code.Equals("528"))
            {
                String h3 = "select HeaderActCode from Accounts where ActTitle = '" + txtparty.Text + "'";
                DataTable dq3 = clsDataLayer.RetreiveQuery(h3);
                if (dq3.Rows.Count > 0)
                {
                    head = Convert.ToInt32(dq3.Rows[0][0]);
                    if (head == 20101 || head == 10101)
                    { if (head == 10101) { c = 0; } else { c = a - b; } }
                    else
                    {
                        if (a > 0)
                        {
                            c = a + b;
                        }
                        else
                        {
                            if (a < 0)
                            {
                                c = a + b;
                            }
                            else { c = a+b; }
                        }
                    }
                }
                status = "OnPaid";
            }
            else { c = a + b; status = "OnPayabale"; }

            int cklen2 = txtcheque.Text.Length;
            if (cklen2 < 4)
            {
                String insr = @"insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
            " values('" + txtRemarks.Text + "','" + txtInvoice.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtInvoice.Text + "','" + txtparty.Text + "'," + txtCashRelease.Text + ",0.00,'OnPaid'," + c + ",'" + txt_company.Text + "') insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
            " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtRemarks.Text + "','" + txtInvoice.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + scode + "','" + Code + "','" + txtInvoice.Text + "','" + second + "'," + txtCashRelease.Text + ",0.00,'OnPaid' ," + c + ",'" + txt_company.Text + "')";
            if (clsDataLayer.ExecuteQuery(insr) > 0){ }
            }
                MessageBox.Show("Payment Paid Successfully!","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                //
                //if (!txtInvoice.Text.Equals(""))
                //{
                //    if (!comboMode.Text.Equals("Bank"))
                //    {
                //        string q = "select * from CashRelease where CashRel='" + txtInvoice.Text + "'";
                //        DataTable purchase = clsDataLayer.RetreiveQuery(q);
                //        if (purchase.Rows.Count > 0)
                //        {
                //            rptPaymentCash rpt = new rptPaymentCash();
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
                //        string q = "select * from BCR where BankRel='" + txtInvoice.Text + "'";
                //        DataTable purchase = clsDataLayer.RetreiveQuery(q);
                //        if (purchase.Rows.Count > 0)
                //        {
                //            rptPayment rpt = new rptPayment();
                //            rpt.SetDataSource(purchase);
                //            PaymentView pop = new PaymentView(rpt);
                //            pop.Show();
                //        }
                //        else
                //        {
                //            MessageBox.Show("No Record Found!");
                //        }
                //    }
                //}
                //else
                //{
                //    MessageBox.Show("Please Fill Pay Id!");
                //}
                //// 
                Disables();
                btnpost.Enabled = true;  btnNew.Enabled = true;
                btnSave.Enabled = false;   btnEdit.Enabled = true;
                btnupdate.Enabled = false;    ShowAll();
                btnEdit.Enabled = true;  txt_company.Enabled = false;
                txt_bcode.Text = "-";  txt_pname.Text = "-";  txt_pamount.Text = "-";
                txt_remarks.Text = "-";
                this.Hide();
                Payment_voucher pv = new Payment_voucher(); pv.Show();
        }
        catch (Exception e) { MessageBox.Show(e.Message); }
        }

        private void PartyCode(String title)
        {
        String sel = "select ActCode,HeaderActCode from Accounts where ActTitle = '" + title + "'";
        DataTable dc = clsDataLayer.RetreiveQuery(sel);
        if (dc.Rows.Count > 0)
        {
            Code = dc.Rows[0][0].ToString(); HCode = dc.Rows[0][1].ToString();
        }
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
            txtbname.Clear();
            txtbano.Clear();
            txtcheque.Clear();
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
        }

        private void txtparty_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtbname_KeyPress(object sender, KeyPressEventArgs e)
        {

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

        }

        private void comboMode_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtbano_KeyPress(object sender, KeyPressEventArgs e)
        {

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
                            else
                            {
                                balance = 0;
                            }
                        }
                        else
                        {
                            balance = 0;
                        }
            }
            catch { }
            return balance;

        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                dsa();
                FormID = "Print";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    if (!txtInvoice.Text.Equals(""))
                    {
                        if (comboMode.SelectedIndex == 1)
                        {
                            string q = "select * from BCR where BankRel='" + txtInvoice.Text + "'";
                         
                            //   string q = "select * from BankCashRelease where BankRel='" + txtInvoice.Text + "'";
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
                                string qq = "select * from BankCashRelease where BankRel='" + txtInvoice.Text + "'";

                                //   string q = "select * from BankCashRelease where BankRel='" + txtInvoice.Text + "'";
                                DataTable purchasew = clsDataLayer.RetreiveQuery(qq);
                                if (purchasew.Rows.Count > 0)
                                {
                                    rptPayment rpt = new rptPayment();
                                    rpt.SetDataSource(purchasew);
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
                            string q = "select * from CashRelease where CashRel = '" + txtInvoice.Text + "'";

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
                                MessageBox.Show("No Record Found!","Erro",MessageBoxButtons.OK,MessageBoxIcon.Stop);
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

        private void btnFind_Click(object sender, EventArgs e)
        {
          
        }

        private void txtInvoice_TextChanged(object sender, EventArgs e)
        {
            search(); PartyNames = txtparty.Text;
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                FormID = "Edit";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    fcash = Convert.ToDecimal(txtCashRelease.Text);
                    txt_company.Enabled = false;
                    String search = "select TransId from Transactions";
                    DataTable dt = clsDataLayer.RetreiveQuery(search);
                    if (dt.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtInvoice, dt);
                    }
                    ck = Convert.ToDecimal(txtCashRelease.Text);
                    if (comboMode.Text == "Cash")
                    {
                        String name = "";
                        name = txt_company.Text;
                        String quers = "select CASH from tech_cash where CompanyName = '" + name + "'";
                        DataTable dte = clsDataLayer.RetreiveQuery(quers);
                        if (dte.Rows.Count > 0)
                        {
                            String csh = dte.Rows[0][0].ToString();
                            txtAvailable.Text = csh;
                            //  //  CashRelease();
                        }
                        decimal ab = Convert.ToDecimal(txtCashRelease.Text);
                        decimal abc = Convert.ToDecimal(txtAvailable.Text);
                        decimal a1 = Convert.ToDecimal(ab + abc);
                        txtAvailable.Text = a1.ToString();
                        decimal abcd = Convert.ToDecimal(txtAvailable.Text);
                        decimal final = abcd - ab;
                        txtNewAmount.Text = final.ToString();
                    }
                    else if (comboMode.Text == "Bank")
                    {
                        string quers = "select remainingbalance from accountbalance where bankname='" + txtbname.Text + "' and accountno='" + txtbano.Text + "'";
                        DataTable dte = clsDataLayer.RetreiveQuery(quers);
                        if (dte.Rows.Count > 0)
                        {
                            string csh = dte.Rows[0][0].ToString();
                            txtAvailable.Text = csh;
                            //    CashRelease();
                        }
                        decimal ab = Convert.ToDecimal(txtCashRelease.Text);
                        decimal abc = Convert.ToDecimal(txtAvailable.Text);
                        decimal a1 = Convert.ToDecimal(ab + abc);
                        txtAvailable.Text = a1.ToString();
                        decimal abcd = Convert.ToDecimal(txtAvailable.Text);
                        decimal final = abcd - ab;
                        txtNewAmount.Text = final.ToString();
                    }
                    //enable();
                    //Enable();
                    txtCashRelease.Enabled = true; txtRemarks.Enabled = true;
                    btnpost.Enabled = false;

                    txtInvoice.Enabled = true;
                    txtInvoice.ReadOnly = false;
                    btnupdate.Enabled = true;
                    btnSave.Enabled = false;
                    txtparty.Enabled = false;
                    btnEdit.Enabled = false;
                    txt_bcode.Text = "-";
                    txt_pname.Text = "-";
                    txt_pamount.Text = "-";
                    txt_remarks.Text = "-";
                    dataGridView1.Enabled = false;
                    txtparty.Enabled = false; txtagainst.Enabled = true; cmbStatus.Enabled = true; btnpo.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void Bank()
        {
            decimal oldbl = 0; String status = ""; decimal cashout = 0; decimal f = 0; String Code = "";
            //
            String hi = "select BankCode from BankTransaction where AccountNo='" + txtbano.Text + "' and BankCode < '" + txtInvoice.Text+ "' Order By BankCode desc";
            DataTable d20 = clsDataLayer.RetreiveQuery(hi);
            if (d20.Rows.Count > 0)
            {
                Code = d20.Rows[0][0].ToString();
            }
            //

            String q1 = "select CurrentCash from BankTransaction where AccountNo='" + txtbano.Text + "' and BankCode < '" + txtInvoice.Text + "'";
            DataTable d1 = clsDataLayer.RetreiveQuery(q1);
            if (d1.Rows.Count > 0)
            {
                oldbl = Convert.ToDecimal(d1.Rows[0][0].ToString());
            }
            String q2 = "select BankCash,CurrentCash,Status from BankTransaction where AccountNo='" + txtbano.Text + "' and BankCode = '" + txtInvoice.Text + "'";
            DataTable d2 = clsDataLayer.RetreiveQuery(q2);
            if (d2.Rows.Count > 0)
            {
                cashout = Convert.ToDecimal(d2.Rows[0][0].ToString()); status = d2.Rows[0][2].ToString();
                cashout = 5000;
                if (status.Equals("Paid"))
                {
                    f = oldbl - cashout;
                }
                else
                {
                    f = oldbl + cashout;
                }
                oldbl = f;

                String upd = "update BankTransaction set BankCash=" + cashout + ",CurrentCash=" + f + " where AccountNo='" + txtbano.Text + "' and BankCode = '" + txtInvoice.Text + "'"; clsDataLayer.ExecuteQuery(upd);
            }

            String q3 = "select BankCode,BankCash,CurrentCash,Status from BankTransaction where AccountNo='" + txtbano.Text + "' and BankCode > '" + txtInvoice.Text + "'";
            DataTable d3 = clsDataLayer.RetreiveQuery(q3);
            if (d3.Rows.Count > 0)
            {
                for (int a = 0; a < d3.Rows.Count; a++)
                {
                    cashout = Convert.ToDecimal(d3.Rows[a][1].ToString()); status = d3.Rows[a][3].ToString(); Code = d3.Rows[a][0].ToString();
                    if (status.Equals("Paid"))
                    {
                        f = oldbl - cashout;
                    }
                    else
                    {
                        f = oldbl + cashout;
                    }
                    oldbl = f;
                    String upd = "update BankTransaction set BankCash=" + cashout + ",CurrentCash=" + f + " where AccountNo='" + txtbano.Text + "' and BankCode = '" + Code + "'"; clsDataLayer.ExecuteQuery(upd);

                }
                MessageBox.Show("Updated!");
            }
        }

        private void OnUpdate()
        {
            try
            {
                FormID = "Update";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    if (!CheckAllFields())
                    {
                        //
                        String getname = "select RemainingLimit,OpeningBalance from Accounts where ActTitle='" + txtparty.Text + "'";
                        DataTable de = clsDataLayer.RetreiveQuery(getname);
                        if (de.Rows.Count < 1)
                        {
                            MessageBox.Show("UserName dont have a chart Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            PaymentUpdate();
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
            catch { }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                OnUpdate();
            }
            catch { }
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


        String Status = "";
        private void PaymentUpdate()
        {
        try
        {
        String dte = "";
        String q3 = "select Dates from Transactions where TransId='" + txtInvoice.Text + "'"; DataTable drt = clsDataLayer.RetreiveQuery(q3);if(drt.Rows.Count > 0)
        {
            dte = drt.Rows[0][0].ToString();
        }
        PartyCode(txtparty.Text);
       
        //String q1 = "delete from BankTransaction where BankCode='" + txtInvoice.Text + "' ";
        //clsDataLayer.ExecuteQuery(q1);
             
        int chelength = txtcheque.Text.Length;
        if (chelength > 2)
        {
            Status = "UNPOST";
        }
        else
        {
        Status = "NILL";
        if (comboMode.Text.Equals("Bank"))
        {
            BankTransUpdate();
        }
        } 
        PartyCode(txtparty.Text);
        string query = "";
          
        String querys = "Delete From Transactions Where TransId = '" + txtInvoice.Text + "' Delete From BankCashRelease Where BankRel = '" + txtInvoice.Text + "'  ";
        clsDataLayer.ExecuteQuery(querys);
        String gm = "Delete From CashRelease Where CashRel = '" + txtInvoice.Text + "'"; clsDataLayer.ExecuteQuery(gm);
        

        query = "insert into Transactions (PInvoice,HeadActCode,VDate,PDate,TransId,Dates,BankName,AccountNo,ChequeNo,RemainingBalance,PartyName,CashGiven,Remarks,UserName,Mode,Company_Name,Status,PostedDate,VoucherStatus,AgainstVoucher)values ('" + txtpo.Text + "','" + HCode + "','" + vdate.Value.ToString("dd-MMMM-yyyy") + "','" + dt9.Value.ToString("dd-MMMM-yyyy") + "','" + txtInvoice.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtbname.Text + "','" + txtbano.Text + "','" + txtcheque.Text + "','" + txtNewAmount.Text + "','" + txtparty.Text + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + UID + "','" + comboMode.Text + "','" + txt_company.Text + "','" + Status + "','" + dt9.Value.ToString("yyyy-MM-dd") + "','" + cmbStatus.Text + "','" + txtagainst.Text + "')";
        clsDataLayer.ExecuteQuery(query); 
       
        }
        catch { }
       
            //
        String sl = "select RemainingAmount,PaidAmount from Purchase where PurCode='" + txtpo.Text + "'"; DataTable d9 = clsDataLayer.RetreiveQuery(sl);
        if (d9.Rows.Count > 0)
        {
        decimal remain = Convert.ToDecimal(d9.Rows[0][0].ToString()); decimal paid = Convert.ToDecimal(d9.Rows[0][1].ToString()); decimal remainrel = Convert.ToDecimal(txtCashRelease.Text);
        decimal finalrel = remain + ck - remainrel; decimal fpaid = paid -ck + remainrel;
        String upd1 = "update Purchase set RemainingAmount=" + finalrel + ",PaidAmount="+fpaid+" where PurCode='" + txtpo.Text + "'"; clsDataLayer.ExecuteQuery(upd1);
        }
            //
        if (comboMode.Text == "Cash")
        {
            try
            {
                string query = "insert into CashRelease(VDate,CashRel,PartyName,Dates,CashGiven,Remarks,UserName,Company_Name) values ('" + vdate.Value.ToString("dd-MMMM-yyyy") + "','" + txtInvoice.Text + "','" + txtparty.Text + "','" + OLDDATE + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + UID + "','" + txt_company.Text + "')";
                clsDataLayer.ExecuteQuery(query);
                //String ist = "insert into CashTransaction(CashCode,PartyName,Dates,Cash,CurrentCash,Remarks,Status,UserName) values('" + txtInvoice.Text + "','" + txtparty.Text + "','" + OLDDATE + "','" + txtCashRelease.Text + "','" + txtNewAmount.Text + "','" + txtRemarks.Text + "','Paid','" + UID + "')";
                //clsDataLayer.ExecuteQuery(ist);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            try
            {
                String name = "";
                name = txt_company.Text;  
                if (CashChange.Equals("true"))
                {
               //string query = ""; query = "UPDATE tech_cash SET CASH = '" + txtNewAmount.Text + "' where CompanyName = '" + name + "'";
               //clsDataLayer.ExecuteQuery(query);
                    CashTransUpdate();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            MessageBox.Show("Rs." + txtCashRelease.Text + " Release to " + txtparty.Text + " And New Amount in " + comboMode.Text + " is Rs." + txtNewAmount.Text + "!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
       
            ShowAll();
        }
       
        else if (comboMode.Text == "Bank")
        {
            try
            {
                if (Status.Equals("UNPOST"))
                {
                    String u1 = "delete from BCR where BankRel='" + txtInvoice.Text+ "'"; clsDataLayer.ExecuteQuery(u1);
                    string bcv = ""; bcv = "insert into BCR(VDate,BankRel,PartyName,BankName,AccountNo,ChequeNo,Dates,CashGiven,Remarks,UserName,Company_Name) VALUES('" + vdate.Value.ToString("dd-MMMM-yyyy") + "','" + txtInvoice.Text + "','" + txtparty.Text + "','" + txtbname.Text + "','" + txtbano.Text + "','" + txtcheque.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + UID + "','" + txt_company.Text + "')";
                    clsDataLayer.ExecuteQuery(bcv);
                }
                else
                {
                    String u1 = "delete from BCR where BankRel='" + txtInvoice.Text + "'"; clsDataLayer.ExecuteQuery(u1);
                    string bcv = ""; bcv = "insert into BCR(VDate,BankRel,PartyName,BankName,AccountNo,ChequeNo,Dates,CashGiven,Remarks,UserName,Company_Name) VALUES('" + vdate.Value.ToString("dd-MMMM-yyyy") + "','" + txtInvoice.Text + "','" + txtparty.Text + "','" + txtbname.Text + "','" + txtbano.Text + "','" + txtcheque.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + UID + "','" + txt_company.Text + "')";
                    clsDataLayer.ExecuteQuery(bcv);
                    string query = ""; query = "insert into BankCashRelease(VDate,BankRel,PartyName,BankName,AccountNo,ChequeNo,Dates,CashGiven,Remarks,UserName,Company_Name) VALUES('" + vdate.Value.ToString("dd-MMMM-yyyy") + "','" + txtInvoice.Text + "','" + txtparty.Text + "','" + txtbname.Text + "','" + txtbano.Text + "','" + txtcheque.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + UID + "','" + txt_company.Text + "')";
                    clsDataLayer.ExecuteQuery(query);
                    //String inst = "insert into BankTransaction(BankCode,PartyName,BankName,AccountNo,ChequeNo,Dates,BankCash,CurrentCash,Remarks,Status,UserName)values('" + txtInvoice.Text + "','" + txtparty.Text + "','" + txtbname.Text + "','" + txtbano.Text + "','" + txtcheque.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtNewAmount.Text + "','" + txtRemarks.Text + "','Paid','" + UID + "')";
                    //clsDataLayer.ExecuteQuery(inst);
                //    Bank();
                }
            }
            catch { }
            MessageBox.Show("Rs." + txtCashRelease.Text + " Release for " + txtparty.Text + " And New Amount in " + comboMode.Text + " is Rs." + txtNewAmount.Text + "!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ShowAll();
        }
       
           int cklen = txtcheque.Text.Length;
           if (cklen < 4)
           {
       
               String balue = "";
               int rowy = txtagainst.Text.Length;
               if (cmbStatus.Text.Equals("Return"))
               {
                   balue = "0";
               }
               else
               {
                   balue = txtCashRelease.Text;
               }
       
               fcash1 = Convert.ToDecimal(txtCashRelease.Text);
        
                   if (fcash != fcash1)
                   {
                   PartyCode(txtparty.Text);    decimal a2 = 0;
                   a2 = Convert.ToDecimal(fcash1);
                   if (HCode.Equals("2"))
                   {
                   ReceiveDue();
                   int index2 = GetRowIndex2(txtInvoice.Text);
                   DataTable dt2 = SetDT2();
                   ShowDt2(dt2);
                   DeleteRecord2();
                   dt2.Rows[index2][7] = a2;
                   dt2.Rows[index2 + 1][8] = a2;
                   dt2 = SomeOperation2(dt2);
                   InsertRecord2(dt2);
                   }
                   int index = GetRowIndex(txtInvoice.Text);
                   DataTable dt = SetDT();
                   ShowDt(dt);
                   DeleteRecord();
                   dt.Rows[index][7] = a2;
                   dt.Rows[index + 1][8] = a2;
                   dt = SomeOperation(dt);
                   InsertRecord(dt);
                   PaymentUpdateDue();
                   }
               
           }
        //
       
        MessageBox.Show("Payment Update Successfully!");
        if (!txtInvoice.Text.Equals(""))
        {
            if (!comboMode.Text.Equals("Bank"))
            {
                string q = "select * from CashRelease where CashRel='" + txtInvoice.Text + "'";
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
                    MessageBox.Show("No Record Found!");
                }
            }
            else
            {
                string q = "select * from BCR where BankRel='" + txtInvoice.Text + "'"; 
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
                    MessageBox.Show("No Record Found!");
                }
            }
        }
        else
        {
            MessageBox.Show("Please Fill Pay Id!");
        }
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

        private void txtInvoice_Leave(object sender, EventArgs e)
        {  
            //search(); 
        }

        public void PaymentDue() {
            PartyCode(txtparty.Text);
            decimal a = PartyBalance();
            decimal c = 0;
            int head = 0;
            bool req = false;
            if (!Code.Equals("528"))
            {
                String h = "select HeaderActCode from Accounts where ActTitle = '" + txtparty.Text + "'";
                DataTable dq = clsDataLayer.RetreiveQuery(h);

                if (dq.Rows.Count > 0)
                {
                    head = Convert.ToInt32(dq.Rows[0][0]);
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
            }
            else { req = true; }

            if (req == true)
            {
                String seldues = "Select * from PaymentDue where PartyCode='" + Code + "'";
                DataTable dxs = clsDataLayer.RetreiveQuery(seldues);
                if (dxs.Rows.Count > 0)
                {
                    decimal totals = Convert.ToDecimal(dxs.Rows[0]["TotalAmount"].ToString());
                    decimal dues = Convert.ToDecimal(dxs.Rows[0]["DueAmount"].ToString());
                    decimal paids = Convert.ToDecimal(dxs.Rows[0]["PaidAmount"].ToString());
                    decimal pays = Convert.ToDecimal(txtCashRelease.Text);
                    if (head == 10101)
                    {
                        totals = 0; dues = 0; paids = 0; pays = 0;
                    }
                    if (Code.Equals("528"))
                    {
                        dues += pays;
                        totals += pays;
                        String qs = "update PaymentDue set DueAmount=" + dues + ",TotalAmount=" + totals + " where PartyCode='" + Code + "'";
                        clsDataLayer.ExecuteQuery(qs);
                    }
                    else
                    {
                       
                              dues -= pays;
                              paids += pays;
                              String qs = "update PaymentDue set DueAmount=" + dues + ",PaidAmount=" + paids + " where PartyCode='" + Code + "'";
                              clsDataLayer.ExecuteQuery(qs);
                         
                    }

                }
                else
                {
                    if (Code.Equals("528"))
                    {
                        String ii = @"insert into PaymentDue (PayCode,PartyName,PartyCode,TotalAmount,DueAmount,PaidAmount,Company_Name)
                            values('" + txtInvoice.Text + "','" + txtparty.Text + "','" + Code + "'," + txtCashRelease.Text + "," + txtCashRelease.Text + ",0,'Reddot Technologies')";
                        clsDataLayer.ExecuteQuery(ii);
                    }
                }

            }
            }

         


        public void PaymentUpdateDue()
        {
            PartyCode(txtparty.Text);
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
                            //
                            String balue = "";
                            int rowy = txtagainst.Text.Length;
                            if (rowy > 4)
                            {
                                balue = "0";
                                dues = dues + old;
                                paids = paids - old;
                            }
                            else
                            { 
                                dues = (dues + old) - pays;
                                paids = (paids - old) + pays;
                            }
                            //
                            
                            if (head == 10101)
                            {
                                totals = 0; dues = 0; paids = 0; pays = 0;
                            }
                            String qs = "update PaymentDue set DueAmount=" + dues + ",PaidAmount=" + paids + " where PartyCode='" + Code + "'";
                            clsDataLayer.ExecuteQuery(qs);
                        }
                    }
                }
            }
        }

        public void RevPaymentUpdateDue()
        {
            PartyCode(PartyNames);
            decimal a = PartyBalance();

            String h = "select HeaderActCode from Accounts where ActTitle = '" + PartyNames + "'";
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
                String h2 = "select HeaderActCode from Accounts where ActTitle = '" + PartyNames + "'";
                DataTable dqs = clsDataLayer.RetreiveQuery(h2);

                if (dqs.Rows.Count > 1)
                {
                    int head = Convert.ToInt32(dqs.Rows[0][0]);
                    if (head == 20101 || head == 10101)
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

                            dues = (dues + old);
                            paids = (paids - old);
                            if (head == 10101)
                            {
                                totals = 0; dues = 0; paids = 0; pays = 0;
                            }
                            String qs = "update PaymentDue set DueAmount=" + dues + ",PaidAmount=" + paids + " where PartyCode='" + Code + "'";
                            clsDataLayer.ExecuteQuery(qs);
                        }
                    }
                }
            }
        }

        public void search()
        {
            try
            {
                String search = "select * from Transactions where TransId = '" + txtInvoice.Text + "' ";
                DataTable dt = clsDataLayer.RetreiveQuery(search);
                if (dt.Rows.Count > 0)
                {
               txtInvoice.Enabled = false;   txtInvoice.ReadOnly = true;
               OLDDATE = dt.Rows[0]["Dates"].ToString();
               vdate.Text = dt.Rows[0]["Dates"].ToString(); 
               txtparty.Text = dt.Rows[0]["PartyName"].ToString();     txtbname.Text = dt.Rows[0]["BankName"].ToString();
               txtbano.Text = dt.Rows[0]["AccountNo"].ToString();       txtcheque.Text = dt.Rows[0]["ChequeNo"].ToString(); 
               cmbStatus.Text = dt.Rows[0]["VoucherStatus"].ToString(); txtagainst.Text = dt.Rows[0]["AgainstVoucher"].ToString();
               txtpo.Text = dt.Rows[0]["PInvoice"].ToString();          String Status = dt.Rows[0]["Status"].ToString();
               dt9.Text = dt.Rows[0]["PostedDate"].ToString();   
               txtCashRelease.Text = dt.Rows[0]["CashGiven"].ToString();
               if(Status.Equals("POST"))
               {
                   btnpost.Enabled = false;
                   btnupdate.Enabled = false; 
               }
               else if (Status.Equals("UNPOST"))
               {
               int chelength = txtcheque.Text.Length;
               if(chelength > 2) {   btnpost.Enabled = true;   }
               else  {  btnpost.Enabled = false;   } 
               }
               else
               {
                   btnpost.Enabled = false;
               }
               txtNewAmount.Text = dt.Rows[0]["RemainingBalance"].ToString();
               txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
               txt_company.Text= dt.Rows[0]["Company_Name"].ToString();
                   decimal aa = Convert.ToDecimal(dt.Rows[0]["CashGiven"].ToString());
                    decimal bb = Convert.ToDecimal(dt.Rows[0]["RemainingBalance"].ToString());
                    decimal cc = aa + bb;
                    txtAvailable.Enabled = true;

                    txtAvailable.Text = cc.ToString();
                    if (!txtbname.Text.Equals("-"))
                    {
                        comboMode.Text = dt.Rows[0]["Mode"].ToString();
                        Mode = comboMode.Text;
                        txtCashRelease.Enabled = true;
                        txtNewAmount.Enabled = true;
                        txtcheque.Enabled = true;
                        txtbname.Enabled = true;
                        txtbano.Enabled = true;
                    }
                    else
                    {
                        comboMode.Text = dt.Rows[0]["Mode"].ToString();
                        Mode = comboMode.Text;
                        txtCashRelease.Enabled = true;
                        txtNewAmount.Enabled = true;
                        txtcheque.Enabled = false;
                        txtbname.Enabled = false;
                        txtbano.Enabled = false;
                        txtbname.Text = "-";
                        txtbano.Text = "-";
                        txtcheque.Text = "-";
                    }
                    //enable();
                    //Enable();
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
                SqlDataAdapter cmd = new SqlDataAdapter("select V_No, Datetime, AccountCode, RefCode, PaymentType, InvoiceNo, Particulars, Debit, Credit, Party_Balance,Company_Name,Description from LedgerPayment where RefCode = '" + Code + "' order by ID asc", con);
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
                        if (type.Equals("OnPayabale"))
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
                        if (type.Equals("OnPayabale"))
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
                        ///
                            String h2 = "select HeaderActCode from Accounts where ActTitle = '" + PartyNames + "'";
                        DataTable dqs = clsDataLayer.RetreiveQuery(h2);

                        if (dqs.Rows.Count > 0)
                        {
                            int head = Convert.ToInt32(dqs.Rows[0][0]);
                            
                                if(head == 10101)
                            {
                                dt.Rows[i][9] = 0;
                            }else{
                                if (type.Equals("OnPayabale"))
                                {
                                    dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[7]) + Convert.ToDecimal(dt.Rows[i - 1].ItemArray[9]);
                                }
                                else
                                {
                                    dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i - 1].ItemArray[9]) - Convert.ToDecimal(dt.Rows[i].ItemArray[7]);
                                } 
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
                    //dt.Rows[i].ItemArray[11] = txtbname.Text; dt.Rows[i].ItemArray[12] = txtbano.Text; dt.Rows[i].ItemArray[11] = txtcheque.Text;
                    String ins = "INSERT INTO LedgerPayment VALUES('" + dt.Rows[i].ItemArray[0].ToString() + "','" + dt.Rows[i].ItemArray[1].ToString() + "','" + dt.Rows[i].ItemArray[2].ToString() + "','" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','" + dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "'," + dt.Rows[i].ItemArray[7].ToString() + "," + dt.Rows[i].ItemArray[8].ToString() + "," + dt.Rows[i].ItemArray[9].ToString() + ",'" + dt.Rows[i].ItemArray[10].ToString() + "','" + dt.Rows[i].ItemArray[11].ToString() + "')";

                    SqlCommand cmd = new SqlCommand(ins, con);
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
        private void txtbname_Leave(object sender, EventArgs e)
        {
            String query = "select AccountNo from BankDetail where BankName = '"+txtbname.Text+"'";
            DataTable dq = clsDataLayer.RetreiveQuery(query);
            if (dq.Rows.Count > 0)
            { 
                clsGeneral.SetAutoCompleteTextBox(txtbano, dq);
            }
            else
            {
                MessageBox.Show("Bank Detail Not Found!");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtInvoice.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); btnEdit.Enabled = true;
            btnNew.Enabled = true;
        }

       

        private void txt_bcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                          //  select * from Transactions where PartyName = '"++"'
                          //  select * from Transactions where CashGiven = '"++"'
                String query5 = "select * from Transactions where TransId like '%"+txt_bcode.Text+"'";
                DataTable df = clsDataLayer.RetreiveQuery(query5);
                if(df.Rows.Count > 0)
                {
                    total("TransId", txt_bcode.Text);
                     dataGridView1.DataSource = df;
                }
                else
                {
                    String query6 = "select * from Transactions";
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

        private void total(String column,String value)
        {
            String query5 = " select sum(CashGiven) from Transactions where "+column+" = '" + value + "'"; 
         DataTable dt = clsDataLayer.RetreiveQuery(query5);
         if (dt.Rows.Count > 0)
         {
            decimal cg = Convert.ToDecimal(dt.Rows[0][0].ToString());
            lblamount.Text = cg.ToString();
         }
        }


        private void txt_pname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //  select * from Transactions where CashGiven = '"++"'
                String query5 = " select * from Transactions where PartyName like '%" + txt_pname.Text + "'";
                DataTable dt = clsDataLayer.RetreiveQuery(query5);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;

                    total("PartyName",txt_pname.Text);
                }
                else
                {
                    String query6 = "select * from Transactions";
                    DataTable dt3 = clsDataLayer.RetreiveQuery(query6);
                    if (dt3.Rows.Count > 0)
                    { 
                        dataGridView1.DataSource = dt3;
                    }
                }
            }
            catch
            {
            }
        }

        private void txt_pamount_TextChanged(object sender, EventArgs e)
        {
            try
            { 
                String query5 = " select * from Transactions where CashGiven like '%" + txt_pamount.Text + "'";
                DataTable df = clsDataLayer.RetreiveQuery(query5);
                if (df.Rows.Count > 0)
                {
                    total("CashGiven",txt_pamount.Text);
                    dataGridView1.DataSource = df;
                }
                else
                {
                    String query6 = "select * from Transactions";
                    DataTable df6 = clsDataLayer.RetreiveQuery(query6);
                    if (df6.Rows.Count > 0)
                    {
                        decimal cg = 0;
                        for (int t = 0; t < df.Rows.Count; t++)
                        {
                            cg += Convert.ToDecimal(df.Rows[0]["CashGiven"].ToString());
                        }
                        lblamount.Text = cg.ToString();
                        dataGridView1.DataSource = df6;
                    }
                }
            }
            catch
            {
            }
        }
         

        private void txt_remarks_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String query5 = "select * from Transactions where Remarks like '%" + txt_remarks.Text + "%'";
                DataTable df = clsDataLayer.RetreiveQuery(query5);
                if (df.Rows.Count > 0)
                {
                  //  total("Remarks", txt_remarks.Text);

                    dataGridView1.DataSource = df;
                }
                else
                {
                    String query6 = "select * from Transactions";
                    DataTable df6 = clsDataLayer.RetreiveQuery(query6);
                    if (df6.Rows.Count > 0)
                    {
                        decimal cg = 0;
                        for (int t = 0; t < df.Rows.Count; t++)
                        {
                            cg += Convert.ToDecimal(df.Rows[0]["CashGiven"].ToString());
                        }
                        lblamount.Text = cg.ToString();
                        dataGridView1.DataSource = df6;
                    }
                }
            }
            catch
            {
            }
        }
         

        private void txtbname_TextChanged(object sender, EventArgs e)
        {
            String query = "select AccountNo from BankDetail where BankName = '" + txtbname.Text + "'";
            DataTable dq = clsDataLayer.RetreiveQuery(query);
            if (dq.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txtbano, dq);
            }
            else{ }
        }

        private void txtparty_Leave(object sender, EventArgs e)
        {  
           try {  PartyCode(txtparty.Text); }catch{}
           String balance ="";
           balance = "select DueAmount from PaymentDue where PartyCode='" + Code + "'";
           DataTable dr1 = clsDataLayer.RetreiveQuery(balance);
           if (dr1.Rows.Count > 0)
           {
               lblamount.Text = Convert.ToDecimal(dr1.Rows[0][0]).ToString();
           } 
        }

        private void ledgerincrease()
        {
            PaymentDue();
            decimal a = PartyBalance();
            decimal b = Convert.ToDecimal(txtCashRelease.Text);
            decimal c = 0;

                String h3 = "select HeaderActCode from Accounts where ActTitle = '" + txtparty.Text + "'";
                DataTable dq3 = clsDataLayer.RetreiveQuery(h3);
                if (dq3.Rows.Count > 0)
                {
                  int  head = Convert.ToInt32(dq3.Rows[0][0]);
                    if (head == 20101 || head == 10101)
                    { if (head == 10101) { c = 0; } else { c = a - b; } }
                    else
                    {
                        if (a > 0)
                        {
                            c = a + b;
                        }
                        else
                        {
                            if (a < 0)
                            {
                                c = a + b;
                            }
                            else { c = a + b; }
                        }
                    }
                }
             String status = "OnPaid";
             String second = "Bank"; String scode = "01010208";
             String insr = @"insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
              " values('" + txtInvoice.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtInvoice.Text + "','" + txtparty.Text + "'," + txtCashRelease.Text + ",0.00,'OnPaid'," + c + ",'" + txt_company.Text + "') insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
              " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtInvoice.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + scode + "','" + Code + "','" + txtInvoice.Text + "','" + second + "'," + txtCashRelease.Text + ",0.00,'OnPaid' ," + c + ",'" + txt_company.Text + "')";
             if (clsDataLayer.ExecuteQuery(insr) > 0)
             {
                 Console.WriteLine("Query  " + insr);

             }
        }

        private void limitupdate()
        {
            String getname = "select RemainingLimit,OpeningBalance from Accounts where ActTitle='" + txtparty.Text + "'";
            DataTable de = clsDataLayer.RetreiveQuery(getname);
            if (de.Rows.Count < 1)
            {
                MessageBox.Show("UserName dont have a chart Account");
            }
            else
            {
                decimal limit = 0;
                decimal Avlimit = Convert.ToDecimal(de.Rows[0][0].ToString());
                decimal Add = Avlimit + ck;
                String Opb = de.Rows[0][1].ToString();
                if (Opb.Equals("NILL"))
                { }
                else
                {
                    if (Add != 0)
                    {
                        limit = Convert.ToDecimal(de.Rows[0][1].ToString());
                        decimal LimitUse = Add;
                        decimal CashRe = Convert.ToDecimal(txtCashRelease.Text);
                        if (CashRe > LimitUse)
                        {
                            MessageBox.Show("Balance Limit : " + limit + "  " + "Remaining Limit : " + Avlimit);
                        }
                        else
                        {
                            decimal final = LimitUse - CashRe;
                            String upd = "update Accounts set RemainingLimit =" + final + " where ActCode='" + Code + "'"; clsDataLayer.ExecuteQuery(upd);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Balance Limit : " + limit + "  " + "Remaining Limit : " + Avlimit);
                    }
                }
            } 
        }

        private void alfaincrease()
        {
            try
            {

                String Balance = "select RemainingBalance from AccountBalance where AccountNo='10321'";
                DataTable dte = clsDataLayer.RetreiveQuery(Balance);
                if (dte.Rows.Count > 0)
                {
                    decimal balance = Convert.ToDecimal(dte.Rows[0][0].ToString());
                    decimal cr = Convert.ToDecimal(txtCashRelease.Text);
                    decimal final = balance + cr;
                    string query3 = "update AccountBalance set RemainingBalance=" + final + " where AccountNo='10321'";
                    clsDataLayer.ExecuteQuery(query3);

                    String q1s = "insert into BankCashIn(BankCode,PartyName,BankName,AccountNo,ChequeNo,Dates,CashIn,Remarks,UserName,Company_Name) values ('" + txtInvoice.Text + "','" + txtparty.Text + "','Alfa Adhi Securities','10321','" + txtcheque.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + Login.UserID + "','" + txt_company.Text + "')";
                    clsDataLayer.ExecuteQuery(q1s);

                    String inst = "insert into BankTransaction(BankCode,PartyName,BankName,AccountNo,ChequeNo,Dates,BankCash,CurrentCash,Remarks,Status,UserName)values('" + txtInvoice.Text + "','" + txtparty.Text + "','" + txtbname.Text + "','10321','" + txtcheque.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtNewAmount.Text + "','" + txtRemarks.Text + "','Received','" + UID + "')";
                    clsDataLayer.ExecuteQuery(inst);
                }
            }
            catch { }
        }

        private void btnpost_Click(object sender, EventArgs e)
        {
        try
        {
            String hym = "select Status from Transactions where Status='NILL' and TransId = '"+txtInvoice.Text+"'";
            DataTable dgf = clsDataLayer.RetreiveQuery(hym);
            if (dgf.Rows.Count > 0)
            {
            MessageBox.Show("Cant Post this Voucher!");
            }
            else
            {
                String Balance = "select RemainingBalance from AccountBalance where AccountNo='" + txtbano.Text + "'";
                DataTable dte = clsDataLayer.RetreiveQuery(Balance);
                if (dte.Rows.Count > 0)
                {
                    decimal balance = Convert.ToDecimal(dte.Rows[0][0].ToString());
                    decimal pay = Convert.ToDecimal(txtCashRelease.Text);
                    decimal final = balance - pay;
                    String MinusCheck = final.ToString();
                    if (MinusCheck.StartsWith("-") || !MinusCheck.StartsWith("-"))
                    {
                        //    MessageBox.Show("You do not have bank balance to pay this voucher!");
                        //}
                        //else
                        //{
                        if (cmbStatus.Text.Equals("Paid"))
                        {
                            string query9 = ""; query9 = "update AccountBalance set RemainingBalance=" + final + " where AccountNo='" + txtbano.Text + "'"; clsDataLayer.ExecuteQuery(query9);
                            String up = "update Transactions set Status='POST' where TransId='" + txtInvoice.Text + "'"; clsDataLayer.ExecuteQuery(up);
                            string query = ""; query = "insert into BankCashRelease(BankRel,PartyName,BankName,AccountNo,ChequeNo,Dates,CashGiven,Remarks,UserName,Company_Name) VALUES('" + txtInvoice.Text + "','" + txtparty.Text + "','" + txtbname.Text + "','" + txtbano.Text + "','" + txtcheque.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtRemarks.Text + "','" + UID + "','" + txt_company.Text + "')";
                            clsDataLayer.ExecuteQuery(query);
                            String inst = "insert into BankTransaction(BankCode,PartyName,BankName,AccountNo,ChequeNo,Dates,BankCash,CurrentCash,Remarks,Status,UserName)values('" + txtInvoice.Text + "','" + txtparty.Text + "','" + txtbname.Text + "','" + txtbano.Text + "','" + txtcheque.Text + "','" + vdate.Value.ToString("yyyy-MM-dd") + "','" + txtCashRelease.Text + "','" + txtNewAmount.Text + "','" + txtRemarks.Text + "','Paid','" + UID + "')";
                            clsDataLayer.ExecuteQuery(inst);
                            ledgerincrease();
                            PartyCode(txtparty.Text);
                            if (Code.Equals("304"))
                            {
                                alfaincrease();
                            }
                            MessageBox.Show("Voucher Posted Successfully");
                        }
                        else
                        {
                            int aglength = txtagainst.Text.Length;
                            if (aglength > 4)
                            {
                                String u1 = "update Transactions set VoucherStatus='" + cmbStatus.Text + "' where TransId='" + txtInvoice.Text + "'"; clsDataLayer.ExecuteQuery(u1);
                                String up = "update Transactions set Status='POST' where TransId='" + txtInvoice.Text + "'"; clsDataLayer.ExecuteQuery(up);
                                limitupdate();
                                MessageBox.Show("Voucher Posted Successfully","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            }
                        }

                        btnpost.Enabled = false;
                        btnEdit.Enabled = false;
                        btnupdate.Enabled = false;
                        btnSave.Enabled = false;
                        btnNew.Enabled = true;
                    }
                }
            }
        }catch{}
        }

        private void txtcno_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //  select * from Transactions where CashGiven = '"++"'
                String query5 = " select * from Transactions where ChequeNo like '%" + txtcno.Text + "'";
                DataTable df = clsDataLayer.RetreiveQuery(query5);
                if (df.Rows.Count > 0)
                {
                    dataGridView1.DataSource = df;
                    total("ChequeNo", txtcno.Text);

                }
                else
                {
                    String query6 = "select * from Transactions";
                    DataTable df6 = clsDataLayer.RetreiveQuery(query6);
                    if (df6.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = df6;
                        decimal cg = 0;
                        for (int t = 0; t < df.Rows.Count; t++)
                        {
                            cg += Convert.ToDecimal(df.Rows[0]["CashGiven"].ToString());
                        }
                        lblamount.Text = cg.ToString();
                    }
                }
            }
            catch
            {
            }
        }

        private void cmbstatus1_SelectedIndexChanged(object sender, EventArgs e)
        {
        try
        {
      String sel = "";
        if(txt_pname.Text.Equals("-"))
        {
            sel = "select * from Transactions where VoucherStatus='" + cmbstatus1.Text + "'";
            total("VoucherStatus", cmbstatus1.Text);
        }
        else
        {
            sel = "select * from Transactions where VoucherStatus='" + cmbstatus1.Text + "' and PartyName='" + txt_pname.Text + "'";

            String query5 = " select sum(CashGiven) from Transactions where VoucherStatus='" + cmbstatus1.Text + "' and PartyName='" + txt_pname.Text + "'";
            DataTable dt3 = clsDataLayer.RetreiveQuery(query5);
            if (dt3.Rows.Count > 0)
            {
                decimal cg = Convert.ToDecimal(dt3.Rows[0][0].ToString());
                lblamount.Text = cg.ToString();
            }
        }
        DataTable dt = clsDataLayer.RetreiveQuery(sel);
        if(dt.Rows.Count > 0)
        {
        dataGridView1.DataSource = dt;
        dataGridView1.PerformLayout();
        }
        else
        {
            dataGridView1.DataSource = null;        }
        }
        catch{}
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT top 20 TransId,Dates,PartyName,CashGiven,Remarks,BankName,AccountNo,ChequeNo,RemainingBalance,UserName,Mode FROM Transactions order by TransId asc", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
            }
            catch { }
        }

        private void btnall_Click(object sender, EventArgs e)
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT TransId,Dates,PartyName,CashGiven,Remarks,BankName,AccountNo,ChequeNo,RemainingBalance,UserName,Mode FROM Transactions", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
            }
            catch { }
        }

        private void btnlast_Click(object sender, EventArgs e)
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT top 20 TransId,Dates,PartyName,CashGiven,Remarks,BankName,AccountNo,ChequeNo,RemainingBalance,UserName,Mode FROM Transactions order by TransId desc", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
            }
            catch { }
        }

        private void btnNew_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        try
        {
          //  InvoiceSearch isv = new InvoiceSearch(this,txtparty.Text); isv.Show();
       } catch { }
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

        private void Payment_voucher_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            } 
        }

        //
        #region LedgerUpdate
        private int GetRowIndex2(string input)
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
        private void DeleteRecord2()
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
        private DataTable SetDT2()
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
        private DataTable SomeOperation2(DataTable dt)
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
                        if (type.Equals("OnCredit"))
                        {
                            dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[7]) + Convert.ToDecimal(dt.Rows[i - 1].ItemArray[9]);
                        }
                        else
                        {
                            dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i - 1].ItemArray[9]) - Convert.ToDecimal(dt.Rows[i].ItemArray[7]);
                        }

                    }
                }
            }
            catch { }
            return dt;
        }
        private void InsertRecord2(DataTable dt)
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
        private void ShowDt2(DataTable dt)
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

        private void btnpo_Click(object sender, EventArgs e)
        {
        try
        {
       InvoiceSearch ivs = new InvoiceSearch(this,txtparty.Text); ivs.Show();
        } catch { }
        }

        private void txtpo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String sl = "select RemainingAmount from Purchase where PurCode='" + txtpo.Text + "'"; DataTable d9 = clsDataLayer.RetreiveQuery(sl);
                if (d9.Rows.Count > 0)
                {
                    decimal remain = Convert.ToDecimal(d9.Rows[0][0].ToString()); txtCashRelease.Text = remain.ToString();
                }
                else
                {
                    txtpo.Text = "-"; txtCashRelease.Text = "0";
                }
                CashRelease();
            } catch  {}
        }

        private void Payment_voucher_KeyDown(object sender, KeyEventArgs e)
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
                btnEdit.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.U)
            {
                btnupdate.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.P)
            {
                btn_print.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        private void Payment_voucher_FormClosing(object sender, FormClosingEventArgs e)
        {
            Master_Form ms = new Master_Form(); ms.clm();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            AddExpense aexp = new AddExpense(this, txtparty.Text);
            aexp.Show();
        }
         

         

        //
     
    
    }
}
