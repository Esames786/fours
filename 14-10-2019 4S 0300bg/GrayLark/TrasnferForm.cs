
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
    public partial class TrasnferForm : Form
    {
        decimal FromCash = 0;
        decimal ToCash = 0;
        decimal OldValue = 0;
        String status = "";
        public TrasnferForm()
        {
            InitializeComponent();
            Disable();
            String query = "select AccountNo from BankDetail";
            DataTable dq = clsDataLayer.RetreiveQuery(query);
            if (dq.Rows.Count > 0)
            {
          clsGeneral.SetAutoCompleteTextBox(txtbano, dq);  clsGeneral.SetAutoCompleteTextBox(txtbano1, dq);
            }
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

                    }
                }
                else if (c is ComboBox)
                { 
               if (((ComboBox)c).Text != "")
               {
                   ((ComboBox)c).Text = "";
                   flag = true;

               } 
                }
            }

           
            return flag;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = "";
            Clears();
            Enable();
            button1.Enabled = false;
            txtbname.Enabled = false;
            txtbnames.Enabled = false;
            txtbano.Enabled = false;
            txtbano1.Enabled = false; 
            txt_code.Text=clsGeneral.getMAXCode("tbl_transfer","TransferCode","TF");
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            try
            {
                String query = "select AccountNo from BankDetail where BankName = '" + txtbname.Text + "'";
                DataTable dq = clsDataLayer.RetreiveQuery(query);
                if (dq.Rows.Count > 0)
                {
                    //clsGeneral.SetAutoCompleteTextBox(txtbano, dq);
                    txtbano.Text = dq.Rows[0][0].ToString();
                }
                else
                {
                    MessageBox.Show("Bank Detail Not Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Information);
                } txtbano.Focus();
            }
            catch { }
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

        private void FromExpense(String Exp)
        {
        String sel = "select * from tbl_Budget where Expense_Account='"+Exp+"'"; DataTable d1 = clsDataLayer.RetreiveQuery(sel);
        if(d1.Rows.Count > 0)
        {
        String s2 = "select TotalAmount,SpentAmount,RemainingAmount from tbl_Budget where MonthYear = '" + DateTime.Now.ToString("MMMM,yyyy") + "' and Expense_Account='" + Exp + "'";
        DataTable d2 = clsDataLayer.RetreiveQuery(s2);
        if (d2.Rows.Count > 0)
        {
        decimal total = Convert.ToDecimal(d2.Rows[0][0].ToString()); decimal spent = Convert.ToDecimal(d2.Rows[0][1].ToString());
        decimal rel = Convert.ToDecimal(txt_amount.Text); spent += rel; decimal remaining = total - spent; decimal fr = Convert.ToDecimal(txtfrom.Text);
        String upd = "update tbl_Budget set RemainingAmount=" + remaining + ",SpentAmount=" + spent + " where MonthYear = '" + DateTime.Now.ToString("MMMM,yyyy") + "' and Expense_Account='" + Exp + "'"; clsDataLayer.ExecuteQuery(upd);
        }
        else
        {
            String ins = "insert into tbl_Budget(Expense_Account,TotalAmount,SpentAmount,RemainingAmount,MonthYear)values('" + TransferFrom.Text + "'," + txtfrom.Text + ",0," + txtfrom.Text + ",'" + DateTime.Now.ToString("MMMM,yyyy") + "')"; clsDataLayer.ExecuteQuery(ins);
        }
        } 
        }

        private void ToExpense(String Exp)
        {
            String sel = "select * from tbl_Budget where Expense_Account='" + Exp + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(sel);
            if (d1.Rows.Count > 0)
            {
                String s2 = "select TotalAmount,RemainingAmount from tbl_Budget where MonthYear = '" + DateTime.Now.ToString("MMMM,yyyy") + "' and Expense_Account='" + Exp + "'";
                DataTable d2 = clsDataLayer.RetreiveQuery(s2);
                if (d2.Rows.Count > 0)
                {
                    decimal total = Convert.ToDecimal(d2.Rows[0][0].ToString()); decimal remain = Convert.ToDecimal(d2.Rows[0][1].ToString());
                    decimal rel = Convert.ToDecimal(txt_amount.Text); total += rel; decimal fr = Convert.ToDecimal(txtto.Text);
                    String upd = "update tbl_Budget set RemainingAmount=" + fr + ",TotalAmount=" + total + " where MonthYear = '" + DateTime.Now.ToString("MMMM,yyyy") + "' and Expense_Account='" + Exp + "'"; clsDataLayer.ExecuteQuery(upd);
                }
                else
                { 
                    String ins = "insert into tbl_Budget(Expense_Account,TotalAmount,SpentAmount,RemainingAmount,MonthYear)values('" + TransferTo.Text + "'," + txtto.Text + ",0," + txtto.Text + ",'" + DateTime.Now.ToString("MMMM,yyyy") + "')"; clsDataLayer.ExecuteQuery(ins);
                }
            }

        }


        private void button2_Click(object sender, EventArgs e)
        {
            if(!CheckAllFields())
            {
                if (TransferFrom.Text.Equals(TransferTo.Text))
                {
                    if (txtbano.Text.Equals(txtbano1.Text))
                    {
                        MessageBox.Show("Select Different Option In TransferFrom and TransferTo!");
                    }
                    else
                    {
                        save(); button1.Enabled = true; status = "";
                    }
                }
                else
                {
                    save();
                    button1.Enabled = true;
                    status = "";
                }
            }
            else
            {
                MessageBox.Show("Please Fill All Fields!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }



        private void CashTransUpdate()
        {
            String Code = txt_code.Text; decimal amount = Convert.ToDecimal(txt_amount.Text);
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
                query1 = "UPDATE tech_cash SET CASH = '" + oldb + "' where CompanyName = 'Delizia' ";
                clsDataLayer.ExecuteQuery(query1);
            }

        }
         
        private void BankTransUpdate(String bn)
        {
            String Code = txt_code.Text; decimal amount = Convert.ToDecimal(txt_amount.Text);
            String upd1 = "update BankTransaction set BankCash=" + amount + " where BankCode='" + Code + "'"; clsDataLayer.ExecuteQuery(upd1);
            String BID = ""; String bno = bn;
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
                    String query30 = "update AccountBalance set RemainingBalance=" + oldb + " where AccountNo='" + bn + "'";
                    clsDataLayer.ExecuteQuery(query30);
                }
            }
        }

        private void PettyCash()
        {
            String sel = "select Issue_Amount,Paid_Amount,Remaining_Amount from tbl_issuepettyamount where MonthYear='" + DateTime.Now.ToString("MMMM,yyyy") + "' and Petty_User='" + txtbnames.Text + "'";
            DataTable d1 = clsDataLayer.RetreiveQuery(sel);
            if (d1.Rows.Count < 1)
            {
                string query = "";
                String bcode = clsGeneral.getMAXCode("tbl_issuepettyamount", "ipa_Code", "ISP");
                query = "insert into tbl_issuepettyamount(ipa_Code,Petty_User,Issue_Amount,Paid_Amount,Remaining_Amount,MonthYear)values('" + bcode + "','" + txtbnames.Text + "'," + txt_amount.Text + ",0," + txt_amount.Text + ",'" + DateTime.Now.ToString("MMMM,yyyy") + "')";
                clsDataLayer.ExecuteQuery(query);
            }
            else
            {
                decimal issue = Convert.ToDecimal(d1.Rows[0][0].ToString());
                decimal paid = Convert.ToDecimal(d1.Rows[0][1].ToString());
                decimal increase = Convert.ToDecimal(txtfrom.Text);
                issue += increase;
                decimal final = issue - paid;
                String upd = "update tbl_issuepettyamount set Remaining_Amount=" + final + " where MonthYear='" + DateTime.Now.ToString("MMMM,yyyy") + "' and Petty_User='" + txtbnames.Text + "'"; clsDataLayer.ExecuteQuery(upd);
            }
            String ins = "insert into tbl_IssuePettyDate (Date,Petty_User,Issue_Amount,MonthYear)values('" + date.Value.ToString("yyyy-MM-dd") + "','" + txtbnames.Text + "','" + txt_amount.Text + "','" + DateTime.Now.ToString("MMMM,yyyy") + "')"; clsDataLayer.ExecuteQuery(ins);
        }
         
        private void save()
        {
   try
   {
   if(status.Equals("edit"))
   {
   String del1 = "delete from tbl_transfer where TransferCode = '"+txt_code.Text+"'"; clsDataLayer.ExecuteQuery(del1);
    String del3 = "delete from BankCashRelease where BankRel = '" + txt_code.Text + "'"; clsDataLayer.ExecuteQuery(del3);
   String del4 = "delete from CashRelease where CashRel = '" + txt_code.Text + "'"; clsDataLayer.ExecuteQuery(del4);
    String del6 = "delete from BankCashIn where BankCode = '" + txt_code.Text + "'"; clsDataLayer.ExecuteQuery(del6);
   String del7 = "delete from CashIn where CashCode = '" + txt_code.Text + "'"; clsDataLayer.ExecuteQuery(del7);
   }
   if (!status.Equals("edit"))
   {
   txt_code.Text = clsGeneral.getMAXCode("tbl_transfer", "TransferCode", "TF"); button2.Enabled = false;
   }
   String ins = "insert into tbl_transfer(TransferCode,Dates,TransferFrom,BankNameFrom,AccountNoFrom,TransferTo,BankNameTo,AccountNoTo,Remarks,TransferAmount,UserName)values('" + txt_code.Text + "','" + date.Text + "','" + TransferFrom.Text + "','" + txtbname.Text + "','" + txtbano.Text + "','" + TransferTo.Text + "','" + txtbnames.Text + "','" + txtbano1.Text + "','" + txt_remarks.Text + "'," + txt_amount.Text + ",'" + Login.UserID + "')";
   clsDataLayer.ExecuteQuery(ins);
   if (TransferFrom.Text.Equals("Bank"))
   {
       string query = "";
       query = "insert into BankCashRelease(BankRel,PartyName,BankName,AccountNo,ChequeNo,Dates,CashGiven,Remarks,UserName,Company_Name) VALUES('" + txt_code.Text + "','" + TransferTo.Text + "','" + txtbname.Text + "','" + txtbano.Text + "','-','" + date.Text + "','" + txt_amount.Text + "','" + txt_remarks.Text + "','" + Login.UserID + "','" + TransferFrom.Text + "')";
       clsDataLayer.ExecuteQuery(query);

       if (!status.Equals("edit"))
       {
           string query2 = "";
           query2 = "update AccountBalance set RemainingBalance=" + txtfrom.Text + " where AccountNo='" + txtbano.Text + "'";
           clsDataLayer.ExecuteQuery(query2);
           String inst = "insert into BankTransaction(BankCode,PartyName,BankName,AccountNo,ChequeNo,Dates,BankCash,CurrentCash,Remarks,Status,UserName)values('" + txt_code.Text + "','" + TransferFrom.Text + "','" + txtbname.Text + "','" + txtbano.Text + "','-','" + date.Text + "','" + txt_amount.Text + "','" + txtfrom.Text + "','" + txt_remarks.Text + "','Paid','" + Login.UserID + "')";
           clsDataLayer.ExecuteQuery(inst);
       }
       else
       {
           BankTransUpdate(txtbano.Text);
       }
   }
   else
   {
       string query = "insert into CashRelease(CashRel,PartyName,Dates,CashGiven,Remarks,UserName,Company_Name) values ('" + txt_code.Text + "','" + TransferTo.Text + "','" + date.Text + "','" + txt_amount.Text + "','" + txt_remarks.Text + "','" + Login.UserID + "','" + TransferFrom.Text + "')";
       clsDataLayer.ExecuteQuery(query);

       String querys = "";
       if (TransferTo.Text.Equals("Bank"))
       { querys = TransferFrom.Text; }
       else { querys = txtbname.Text; }
       if (!status.Equals("edit"))
       {
           string query3 = "";
           query3 = "UPDATE tech_cash SET CASH = '" + txtfrom.Text + "' where CompanyName = '" + TransferFrom.Text + "'";
           clsDataLayer.ExecuteQuery(query3);
           String ist = "insert into CashTransaction(CashCode,PartyName,Dates,Cash,CurrentCash,Remarks,Status,UserName) values('" + txt_code.Text + "','" + TransferFrom.Text + "','" + date.Text + "','" + txt_amount.Text + "','" + txtfrom.Text + "','" + txt_remarks.Text + "','Paid','" + Login.UserID + "')";
           clsDataLayer.ExecuteQuery(ist);
       }
       else
       {
           CashTransUpdate();
       }
   }
    if (TransferTo.Text.Equals("Bank"))
    {  
        String q1s = "insert into BankCashIn(BankCode,PartyName,BankName,AccountNo,ChequeNo,Dates,CashIn,Remarks,UserName,Company_Name) values ('" + txt_code.Text + "','" + TransferFrom.Text + "','" + txtbnames.Text + "','" + txtbano1.Text + "','-','" + date.Text + "','" + txt_amount.Text + "','" + txt_remarks.Text + "','" + Login.UserID + "','" + TransferTo.Text + "')";
        clsDataLayer.ExecuteQuery(q1s);
        if (!status.Equals("edit"))
        {
            string query4 = "";
            query4 = "update AccountBalance set RemainingBalance=" + txtto.Text + " where AccountNo='" + txtbano1.Text + "'";
            clsDataLayer.ExecuteQuery(query4);
            String inst = "insert into BankTransaction(BankCode,PartyName,BankName,AccountNo,ChequeNo,Dates,BankCash,CurrentCash,Remarks,Status,UserName)values('" + txt_code.Text + "','" + TransferTo.Text + "','" + txtbnames.Text + "','" + txtbano1.Text + "','-','" + date.Text + "','" + txt_amount.Text + "','" + txtto.Text + "','" + txt_remarks.Text + "','Received','" + Login.UserID + "')";
            clsDataLayer.ExecuteQuery(inst);
        }
        else
        {
            BankTransUpdate(txtbano1.Text);
        }
    }
    else if (TransferTo.Text.Equals("PettyCash"))
    {
        PettyCash();
    }
    else
    { 
        String cashget = "insert into CashIn (CashCode,PartyName,Dates,CashIn,Remarks,UserName,Company_Name) values ('" + txt_code.Text + "','" + TransferFrom.Text + "','" + date.Text + "','" + txt_amount.Text + "','" + txt_remarks.Text + "','" + Login.UserID + "','" + TransferTo.Text + "')";
        clsDataLayer.ExecuteQuery(cashget);
        if (!status.Equals("edit"))
        {
            string query = "";
            query = "UPDATE tech_cash SET CASH = '" + txtto.Text + "' where CompanyName = '" + TransferTo.Text + "'";
            clsDataLayer.ExecuteQuery(query);
            String querys = "";
            if (TransferFrom.Text.Equals("Bank"))
            { querys = TransferFrom.Text; }
            else { querys = txtbname.Text; }
            String ist = "insert into CashTransaction(CashCode,PartyName,Dates,Cash,CurrentCash,Remarks,Status,UserName) values('" + txt_code.Text + "','" + TransferTo.Text + "','" + date.Text + "','" + txt_amount.Text + "','" + txtto.Text + "','" + txt_remarks.Text + "','Received','" + Login.UserID + "')";
            clsDataLayer.ExecuteQuery(ist);
        }
        else
        {
            CashTransUpdate();
        }
    }
    MessageBox.Show("Transfered Successfully !", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
       Disable();
       Clears();
   }catch
   { }
   }

        private void TransferTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtfrom.Text = FromCash.ToString();
                txt_amount.Text = "0";
                txtto.Text = ToCash.ToString();
                
                if (TransferTo.Text.Equals("Bank"))
                {
                    label8.Text = "Bank Name";
                    txtbnames.Enabled = true;
                    txtbano1.Enabled = true;
                    String query = "select BankName from BankDetail ";
                    DataTable dq = clsDataLayer.RetreiveQuery(query);
                    if (dq.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtbnames, dq);
                    }
                    else
                    { 
                        txtbnames.Text = "Bank Detail Not Found!";
                         txtbano1.Text = "-";
                         txtbnames.Enabled = false;
                         txtbano1.Enabled = false;
                    }
                }
                else if (TransferTo.Text.Equals("PettyCash"))
                {
                     txtbnames.Enabled = true;
                     txtbano1.Enabled = false; label8.Text = "Petty User"; 
                    String query = "select ActTitle from Accounts where HeaderActCode='10103' and Status = 'True'";
                    DataTable dq = clsDataLayer.RetreiveQuery(query);
                    if (dq.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtbnames, dq); txtbano1.Enabled = false; txtbano1.Text = "-";
                    }
                    else
                    {
                        txtbnames.Text = "PettyCash User Detail Not Found!";
                        txtbano1.Text = "-";
                        txtbnames.Enabled = false;
                        txtbano1.Enabled = false;
                    }
                }


                else
                {
                    label8.Text = "Bank Name";
                    String quer = "select CASH from tech_cash where CompanyName = '" + TransferTo.Text + "'";
                    DataTable dts = clsDataLayer.RetreiveQuery(quer);
                    if (dts.Rows.Count > 0)
                    {
                        ToCash = Convert.ToDecimal(dts.Rows[0][0]);
                        txtto.Text = ToCash.ToString();
                    }
                    else
                    {
                        txtto.Text = "0";
                    }
                    txtbnames.Enabled = false;
                    txtbano1.Enabled = false;
                    txtbnames.Text = "-";
                    txtbano1.Text = "-";
                }
            }
            catch { }
        }

        private void TransferFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtfrom.Text = FromCash.ToString();
                txt_amount.Text = "0";
                txtto.Text = ToCash.ToString();

                
                if (TransferFrom.Text.Equals("Bank"))
                {
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
                        txtbname.Text = "Bank Detail Not Found!";
                        txtbano.Text = "-";
                        txtbname.Enabled = false;
                        txtbano.Enabled = false;
                    }
                }
                else 
                { 
                    String quer = "select CASH from tech_cash where CompanyName = '" + TransferFrom.Text + "'";
                    DataTable dts = clsDataLayer.RetreiveQuery(quer);
                    if(dts.Rows.Count > 0)
                    {
                        FromCash = Convert.ToDecimal(dts.Rows[0][0]);
                        txtfrom.Text = FromCash.ToString();
                    }
                    else
                    {
                        txtfrom.Text = "0";
                    }
                    txtbname.Enabled = false;
                    txtbano.Enabled = false;
                    txtbname.Text = "-";
                    txtbano.Text = "-";
                }
            }
            catch { }
        }
 

        private void txtbano1_Leave(object sender, EventArgs e)
        {
            try
            {
                String query = "select RemainingBalance from [dbo].[AccountBalance] where AccountNo = '" + txtbano1.Text + "' and BankName = '" + txtbnames.Text + "'";
                DataTable dq = clsDataLayer.RetreiveQuery(query);
                if (dq.Rows.Count > 0)
                {
                    ToCash = Convert.ToDecimal(dq.Rows[0][0].ToString());
                    txtto.Text = ToCash.ToString();
                }else{ }
            }
            catch { }
        }

        public void GetBalance(){
        try
        {
            String query = "select RemainingBalance from AccountBalance where AccountNo = '" + txtbano.Text + "' and BankName = '" + txtbname.Text + "'";
            DataTable dq = clsDataLayer.RetreiveQuery(query);
            if (dq.Rows.Count > 0)
            {
                FromCash = Convert.ToDecimal(dq.Rows[0][0].ToString());
                txtfrom.Text = FromCash.ToString(); 
            }
            else { }
        }
        catch { }
        }
        private void txtbano_Leave(object sender, EventArgs e)
        {
            GetBalance();
        }

        private void txt_amount_TextChanged(object sender, EventArgs e)
        {
            try
            {
            GetBalance();
            decimal a = Convert.ToDecimal(txtfrom.Text);
            decimal b = Convert.ToDecimal(txtto.Text);
            decimal c = Convert.ToDecimal(txt_amount.Text);
                 
            if (status.Equals("edit"))
            {
                if (OldValue > c)
                {
                    decimal f1 = OldValue - c;
                    decimal d = ToCash - f1;
                    txtto.Text = d.ToString();

                    decimal e1 = FromCash + f1;
                    txtfrom.Text = e1.ToString();
                }
                else
                {
                    decimal f1 = c - OldValue;
                    decimal d = ToCash + f1;
                    txtto.Text = d.ToString();

                    decimal e1 = FromCash - f1;
                    txtfrom.Text = e1.ToString();
                }
            }
            else
            {
             
                decimal d = ToCash + c;
                txtto.Text = d.ToString();

                decimal e1 = FromCash - c;
                txtfrom.Text = e1.ToString();
             
            }
              
           // }
            }catch{}
        }

        private void btnsearch_Click(object sender, EventArgs e)
        { try { TransferSearch ts = new TransferSearch(this);   ts.Show();  }catch{}  }

        private void txt_code_TextChanged(object sender, EventArgs e)
        {  try  {  String query = "select * from tbl_transfer where TransferCode='"+txt_code.Text+"'";DataTable dth = clsDataLayer.RetreiveQuery(query);
            if (dth.Rows.Count > 0)
                {
                    status = "edit";
                    date.Text=dth.Rows[0][2].ToString();
                    TransferFrom.Text = dth.Rows[0][3].ToString();
                    TransferTo.Text = dth.Rows[0][6].ToString();
                    txtbname.Text = dth.Rows[0][4].ToString();
                    txtbano.Text = dth.Rows[0][5].ToString();
                    txtbnames.Text = dth.Rows[0][7].ToString();
                    txtbano1.Text = dth.Rows[0][8].ToString();
                    txt_amount.Text = dth.Rows[0][10].ToString();
                    txt_remarks.Text = dth.Rows[0][9].ToString();
                    Enable();
                    OldValue = Convert.ToDecimal(txt_amount.Text);
                        String query3 = "select RemainingBalance from [dbo].[AccountBalance] where AccountNo = '" + txtbano.Text + "' and BankName = '" + txtbname.Text + "'";
                        DataTable dq = clsDataLayer.RetreiveQuery(query3);
                        if (dq.Rows.Count > 0)
                        {
                            FromCash = Convert.ToDecimal(dq.Rows[0][0].ToString());
                            txtfrom.Text = FromCash.ToString();
                        }else{txtbano.Text = "-";}

                        String query4 = "select RemainingBalance from [dbo].[AccountBalance] where AccountNo = '" + txtbano1.Text + "' and BankName = '" + txtbnames.Text + "'";
                        DataTable dq1= clsDataLayer.RetreiveQuery(query4);
                        if (dq1.Rows.Count > 0)
                        {
                            FromCash = Convert.ToDecimal(dq1.Rows[0][0].ToString());
                            txtto.Text = FromCash.ToString();
                        }
                        else { txtbano1.Text = "-"; }

                        String quer = "select CASH from tech_cash where CompanyName = '" + TransferFrom.Text + "'";
                        DataTable dts = clsDataLayer.RetreiveQuery(quer);
                        if (dts.Rows.Count > 0)
                        {
                            FromCash = Convert.ToDecimal(dts.Rows[0][0]);
                            txtfrom.Text = FromCash.ToString();
                        }
                        else
                        { 
                        }

                        String quer2 = "select CASH from tech_cash where CompanyName = '" + TransferTo.Text + "'";
                        DataTable dts2 = clsDataLayer.RetreiveQuery(quer2);
                        if (dts2.Rows.Count > 0)
                        {
                            FromCash = Convert.ToDecimal(dts2.Rows[0][0]);
                            txtto.Text = FromCash.ToString();
                        }
                        else
                        { 
                        }
                        
                    TransferFrom.Enabled=false;
                    txtbname.Enabled=false;
                    txtbano.Enabled=false;

                    TransferTo.Enabled=false;
                    txtbnames.Enabled=false;
                    txtbano1.Enabled = false; button2.Enabled = true;
               }
            }
            catch { } 
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            try
            {
                String query = "select * from tbl_transfer  where TransferCode='" + txt_code.Text + "'";
                DataTable dts = clsDataLayer.RetreiveQuery(query);
                if (dts.Rows.Count > 0)
                {
                    TransferReport tr = new TransferReport();
                    tr.SetDataSource(dts);
                    PaymentView pay = new PaymentView(tr);
                    pay.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void txtbnames_Leave(object sender, EventArgs e)
        {
            try
            {
                if (TransferTo.Text.Equals("Bank"))
                {
                    String query = "select AccountNo from BankDetail where BankName = '" + txtbnames.Text + "'";
                    DataTable dq = clsDataLayer.RetreiveQuery(query);
                    if (dq.Rows.Count > 0)
                    {
                        txtbano1.Text = dq.Rows[0][0].ToString();
                        //  clsGeneral.SetAutoCompleteTextBox(txtbano1, dq);
                    }
                    else
                    {
                        MessageBox.Show("Bank Detail Not Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                else if (TransferTo.Text.Equals("PettyCash"))
                {
                    String query = "select Remaining_Amount from tbl_issuepettyamount where MonthYear = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and Petty_User = '" + txtbnames.Text + "'";
                    DataTable dq = clsDataLayer.RetreiveQuery(query);
                    if (dq.Rows.Count > 0)
                    {
                        txtto.Text = dq.Rows[0][0].ToString();
                    }
                    else
                    {
                        txtto.Text = "0";
                    }
                }
            }
            catch { }
        }

        private void txtbano_KeyPress(object sender, KeyPressEventArgs e)
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

        private void TrasnferForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.A)
            {
                button1.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.S)
            {
                button2.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.F)
            {
                btnsearch.PerformClick();
            } 
            else if (e.Control == true && e.KeyCode == Keys.P)
            {
                btnprint.PerformClick();
            }
        }

        private void TransferFrom_Leave(object sender, EventArgs e)
        {
            txtbname.Focus();
        }

        private void txtbano_Leave_1(object sender, EventArgs e)
        {
            GetBalance();
        }

        private void TransferTo_Leave(object sender, EventArgs e)
        {
            txtbnames.Focus();
        }

        private void txtbano1_Leave_1(object sender, EventArgs e)
        {
            txtto.Focus();
        } 

       

        




    }
}
