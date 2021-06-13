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
    public partial class ReplaceOrder : Form
    {
        String Cus = "";
        String UID = Login.UserID;
        public string GreenSignal = "";
        public string FormID = "";
        String status = "";
        String Code = "";
        decimal global = 0;
        public ReplaceOrder()
        {
            InitializeComponent();
            this.BackColor = Color.SteelBlue;
            //String sh = "select SaleId from tbl_replace order by SaleId desc"; DataTable ds = clsDataLayer.RetreiveQuery(sh);
            //if (ds.Rows.Count > 0) { txtSearchSID.Text = ds.Rows[0][0].ToString(); }
            try
            {
                String ques = "select NAME from add_product where QUANTITY !=0";
                DataTable d = clsDataLayer.RetreiveQuery(ques);
                if (d.Rows.Count > 0)
                {
                   clsGeneral.SetAutoCompleteTextBox(txt1, d);
                }else{ }
            }
            catch { }
            txtreturn.Text = clsGeneral.getMAXCode("tbl_replace", "REID", "RE");
    
        }

        private string SID;
        public string passSID
        {
            get { return SID; }
            set { SID = value; }
        }

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

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        SqlCommand cmd;

        private void Return_Order_Load(object sender, EventArgs e)
        {
        
        }

        private void clear()
        {
            groupBox3.Enabled = false;
           
            txtCName.Clear();
            dataGridView1.Rows.Clear();
            
            date3.Value = DateTime.Now;
            comboOrder.SelectedIndex = -1;
            txtRetReason.Clear();
            btnSave.Enabled = false;
        }

        private void Clean()
        {
          
            foreach (Control c in this.groupBox3.Controls)
            {
                if (c is TextBox)
                {
                 c.Text = "" ;
                }
                else if (c is ComboBox)
                {
             c.Text ="";
                }
            }

            foreach (Control c in this.groupBox5.Controls)
            {
                if (c is TextBox)
                {
                    c.Text = "";
                }
                else if (c is ComboBox)
                {
                    c.Text = "";
                }
            }

         
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
                FormID = "Add";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                Clean();
                txtreturn.Text = clsGeneral.getMAXCode("tbl_replace", "REID", "RE");
                enable();
                btnNew.Enabled = false;
                btnSave.Enabled = true;
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public void enable()
        {
         
            button1.Enabled = true;
            txt1.Enabled = true;
            row.Enabled = true;
            button1.Enabled = true;
            textBox1.Enabled = true;
            comboBox1.Enabled = true;
            txt1.Enabled = true;
            txtSearchSID.Enabled = true;
            groupBox3.Enabled = true;
           
          
            txtCName.Enabled = true;
            dataGridView3.Enabled = true;
            dataGridView1.Enabled = true;
            
            date3.Enabled = true;
            comboOrder.Enabled = true;
            txtRetReason.Enabled = true;
            btnSave.Enabled = true;
            txt1.Text = "";

        }

        private bool CheckDataGridCells(DataGridView dgv)
        {
            bool flag = false;
            for (int i = 0; i <= dgv.Rows.Count-1 ; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                     if (dgv.Rows[i].Cells[j].Value == null)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == true)
                {
                    break;
                }
            }
            return flag;
        }
        private bool CheckAllFields()
        {
            bool flag = false;
            foreach (Control c in this.groupBox3.Controls)
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

        private bool CheckAllFields1()
        {
            bool flag = false;
            foreach (Control c in this.groupBox5.Controls)
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

         private void btnSave_Click(object sender, EventArgs e)
        {
                 FormID = "Save";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                          if ((!CheckDataGridCells(dataGridView1) && !CheckAllFields()) && (!CheckAllFields1()))
                           {
                            try
                                {
                  Cus = txtCName.Text;
                  String ans = "";
                  if (Cus.Equals(""))
                  {
                      ans = textBox1.Text;
                  }
                  else
                  {
                   ans = Cus;
                  }
                  PartyCode();
                  String ins = "insert into tbl_replace (discount,NetAmount,RCode,SaleId,CustomerId,ReplaceDate,Orders,Reason,TotalAmount,UserName) values (" + txtdiscount.Text + "," + txtnetamount.Text + ",'" + txtreturn.Text + "','" + txtSearchSID.Text + "','" + ans + "','" + date3.Value.ToString("MM-dd-yyyy") + "','" + comboOrder.Text + "','" + txtRetReason.Text + "','" + txtavailable.Text + "','" + Login.UserID + "')";
                  clsDataLayer.ExecuteQuery(ins);
                  for (int r = 0; r < dataGridView1.Rows.Count; r++)
                  {
                      String q = "insert into tbl_RetReplace_Detail(ReCode,ReDate,ReturnProduct,RetQuantity,RetTotalPrice)values ('" + txtreturn.Text + "' , '" + date3.Value.ToString("MM-dd-yyyy") + "','" + dataGridView1.Rows[r].Cells[0].Value.ToString() + "'," + dataGridView1.Rows[r].Cells[2].Value.ToString() + "," + dataGridView1.Rows[r].Cells[3].Value.ToString() + ")";
                      clsDataLayer.ExecuteQuery(q);
                  }
                  for (int r = 0; r < dataGridView3.Rows.Count; r++)
                  {
                      String inss = " insert into tbl_Replace_Detail (ReCode,ReDate,ReplaceProduct,RepQuantity,RepTotalPrice)values ('" + txtreturn.Text + "' , '" + date3.Value.ToString("MM-dd-yyyy") + "' ,'" + dataGridView3.Rows[r].Cells[0].Value.ToString() + "'," + dataGridView3.Rows[r].Cells[2].Value.ToString() + "," + dataGridView3.Rows[r].Cells[3].Value.ToString() + ")";
                      clsDataLayer.ExecuteQuery(inss);
                  }
           }  catch   {  }

                                try
                                {
                                //    MessageBox.Show("Selected Index " + comboBox1.SelectedIndex);
                                    if (comboBox1.SelectedIndex == 0)
                                    {
                                        for (int j = 0; j < dataGridView1.Rows.Count; j++)
                                        {
                                            decimal GQuant = Convert.ToDecimal(dataGridView1.Rows[j].Cells[2].Value.ToString());
                                            String Name = dataGridView1.Rows[j].Cells[0].Value.ToString();
                                            String GetQuant = "select QUANTITY from add_product where NAME = '" + Name + "' ";
                                            DataTable ds = clsDataLayer.RetreiveQuery(GetQuant);
                                            decimal finals = 0;
                                            decimal Old = 0;
                                            if (ds.Rows.Count > 0)
                                            {
                                                Old = Convert.ToDecimal(ds.Rows[0][0].ToString());
                                                finals = GQuant + Old;
                                            }
                                            string query = " UPDATE add_product SET  QUANTITY ='" + finals + "' WHERE NAME = '" + Name + "'";
                                            clsDataLayer.ExecuteQuery(query);
                                        }

                                        for (int j = 0; j < dataGridView3.Rows.Count; j++)
                                        {
                                            decimal GQuant = Convert.ToDecimal(dataGridView3.Rows[j].Cells[2].Value.ToString());
                                            String Name = dataGridView3.Rows[j].Cells[0].Value.ToString();
                                            String GetQuant = "select QUANTITY from add_product where NAME = '" + Name + "' ";
                                            DataTable ds = clsDataLayer.RetreiveQuery(GetQuant);
                                            decimal finals = 0;
                                            decimal Old = 0;
                                            if (ds.Rows.Count > 0)
                                            {
                                                Old = Convert.ToDecimal(ds.Rows[0][0].ToString());
                                                finals = Old - GQuant;

                                            }
                                            string query = " UPDATE add_product SET  QUANTITY ='" + finals + "' WHERE NAME = '" + Name + "'";
                                            clsDataLayer.ExecuteQuery(query);
                                        }

                                    }
                                    else
                                    {
                                        //Vender
                                        //
                                        for (int j = 0; j < dataGridView1.Rows.Count; j++)
                                        {
                                            decimal GQuant = Convert.ToDecimal(dataGridView1.Rows[j].Cells[2].Value.ToString());
                                            String Name = dataGridView1.Rows[j].Cells[0].Value.ToString();
                                            String GetQuant = "select QUANTITY from add_product where NAME = '" + Name + "' ";
                                            DataTable ds = clsDataLayer.RetreiveQuery(GetQuant);
                                            decimal finals = 0;
                                            decimal Old = 0;
                                            if (ds.Rows.Count > 0)
                                            {
                                                Old = Convert.ToDecimal(ds.Rows[0][0].ToString());
                                                finals = Old - GQuant;
                                            }
                                            string query = " UPDATE add_product SET  QUANTITY ='" + finals + "' WHERE NAME = '" + Name + "'";
                                            clsDataLayer.ExecuteQuery(query);
                                        }

                                        for (int j = 0; j < dataGridView3.Rows.Count; j++)
                                        {
                                            decimal GQuant = Convert.ToDecimal(dataGridView3.Rows[j].Cells[2].Value.ToString());
                                            String Name = dataGridView3.Rows[j].Cells[0].Value.ToString();
                                            String GetQuant = "select QUANTITY from add_product where NAME = '" + Name + "' ";
                                            DataTable ds = clsDataLayer.RetreiveQuery(GetQuant);
                                            decimal finals = 0;
                                            decimal Old = 0;
                                            if (ds.Rows.Count > 0)
                                            {
                                                Old = Convert.ToDecimal(ds.Rows[0][0].ToString());
                                                finals = Old + GQuant;

                                            }
                                            string query = " UPDATE add_product SET  QUANTITY ='" + finals + "' WHERE NAME = '" + Name + "'";
                                            clsDataLayer.ExecuteQuery(query);
                                        }
                                        //
                                    }
                                }
                                catch
                                {

                                }
                                if (comboBox1.Text.Equals("Customer"))
                                {
                                    CustomerLedger();

                                    String qus = "select BillAmount,TotalDiscount from tbl_sale";
                                    DataTable dg = clsDataLayer.RetreiveQuery(qus);
                                    if(dg.Rows.Count > 0)
                                    {
                                        int Bill = Convert.ToInt32(dg.Rows[0][0]);
                                        int dis = Convert.ToInt32(dg.Rows[0][1]);
                                       
                                        int fprice = Convert.ToInt32(txttotal.Text);
                                        int sprice = Convert.ToInt32(totrep.Text);
                                        int final = (Bill - fprice) + sprice;
                                        int nets = final + dis;
                                        String h2 = "update tbl_sale set ReplaceAmount='" + txtavailable.Text + "',VoucherStatus='" + comboOrder.Text + "' where SaleCode = '" + txtSearchSID.Text + "'";
                                        if (clsDataLayer.ExecuteQuery(h2) > 0)
                                        { }
                                    } 
                                   //} //End 1st For Lopp
                                  }
                                else
                                {
                                    PaymentDue();
                                    PaymentDues();
                                    int totaly=0;
                                    //Start
                                    String qus = "select BillPrice from Purchase";
                                    DataTable dg = clsDataLayer.RetreiveQuery(qus);
                                    if (dg.Rows.Count > 0)
                                    {
                                        int Bill = Convert.ToInt32(dg.Rows[0][0]);

                                        int fprice = Convert.ToInt32(txttotal.Text);
                                        int sprice = Convert.ToInt32(totrep.Text);
                                        int final = (Bill - fprice) + sprice;
                                        totaly = final;
                                        String h2 = "update Purchase set ReplaceAmount='" + txtavailable.Text + "',Status='" + comboOrder.Text + "' where PurCode = '" + txtSearchSID.Text + "'";
                                        if (clsDataLayer.ExecuteQuery(h2) > 0)
                                        { }
                                    }
                                     //End
                                      }
                                MessageBox.Show("Customer Order is now Replace!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                disable();
                                dataGridView1.Rows.Clear();
                                dataGridView3.Rows.Clear();
                                btnNew.Enabled = true;
                                btnSave.Enabled = false;
                                totrep.Text = ""; DoPrint();
                        }
                        else
                        {
                            MessageBox.Show("Please Fill All Fields!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Search Any Sale Id!");
                    }
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
        }

         private void ReceiveDue()
         {
             String rec = "select * from ReceiveDue where PartyCode = '" + Code + "'";
             DataTable d = clsDataLayer.RetreiveQuery(rec);
             if (d.Rows.Count > 0)
             {
                 decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
                 decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
                 decimal received = decimal.Parse(d.Rows[0]["ReceivedAmount"].ToString());
                 decimal bill = decimal.Parse(txttotal.Text);
                 decimal ab = 0;
                 decimal final = 0;

                 if (global > 0)
                 {
                     ab = global;
                     final = (due + ab);
                 }
                 else
                 {
                     ab = Convert.ToDecimal(txttotal.Text);
                     final = (due - ab);
                     received += ab;
                 }

                 String fin = final.ToString();
                 decimal abc = 0;
                 if (fin.StartsWith("-"))
                 {
                     global = Math.Abs(final);
                     abc = 0;
                     PaymentDue();
                     decimal a = PartyBalance();
                     decimal b = Convert.ToDecimal(due);
                     decimal c = 0;
                     String name = "";
                     if (!txtCName.Text.Equals("-"))
                     {
                         name = txtCName.Text;
                     }
                     else
                     {
                         name = textBox1.Text;
                     }
                     String ascc = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                     " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtreturn.Text + "','Sale Return'," + b + ",0.00,'OnReceived'," + c + ",'Bilal Communication') insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                     " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + name + "'," + b + ",0.00,'OnReceived' ," + c + ",'Bilal Communication')";
                     clsDataLayer.ExecuteQuery(ascc);
                 }
                 else
                 {
                     abc = final;
                     decimal a = PartyBalance();
                     decimal b = Convert.ToDecimal(ab);
                     decimal c = 0;
                     if (global > 0)
                     { c = a + ab; }
                     else { c = a - ab; }
                     String name = "";
                     if (!txtCName.Text.Equals("-"))
                     {
                         name = txtCName.Text;
                     }
                     else
                     {
                         name = textBox1.Text;
                     }
                     String instr = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                     " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtreturn.Text + "','Sale Return'," + b + ",0.00,'OnReceived'," + c + ",'Bilal Communication') insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                     " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + name + "'," + b + ",0.00,'OnReceived' ," + c + ",'Bilal Communication')";
                     clsDataLayer.ExecuteQuery(instr);
                 }

                 //

                 //
                 string updateblnc = "update ReceiveDue set TotalAmount=" + total.ToString() + ", DueAmount=" + abc.ToString() + "  , ReceivedAmount = " + received + " where PartyCode='" + Code + "' ";
                 if (clsDataLayer.ExecuteQuery(updateblnc) > 0)
                 {
                     MessageBox.Show("Order Replace Success");
                     disable();
                     dataGridView1.Enabled = false;
                     btnSave.Enabled = false;
                     btnNew.Enabled = true;
                 }

             }
             else
             {
                 decimal ab = 0;
                 if (global > 0)
                 {
                     ab = global;
                 }
                 else
                 {
                     ab = Convert.ToDecimal(txttotal.Text);
                 }

                 String ii = @"insert into ReceiveDue (RemCode,PartyName,PartyCode,TotalAmount,DueAmount,ReceivedAmount,Company_Name)
                values('" + txtreturn.Text + "','" + textBox1.Text + "','" + Code + "'," + ab + "," + ab + ",0,'Bilal Communication')";

                 if (clsDataLayer.ExecuteQuery(ii) > 0)
                 {
                     decimal b = Convert.ToDecimal(ab);
                     decimal c = ab;
                     String name = "";
                     if (!txtCName.Text.Equals("-"))
                     {
                         name = txtCName.Text;
                     }
                     else
                     {
                         name = textBox1.Text;
                     }
                     String insq = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " + " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + name + "'," + b + ",0.00,'OnReceiveable' ," + c + ",'Bilal Communication') insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                     " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtreturn.Text + "','Purchase Return'," + b + ",0.00,'OnReceiveable'," + c + ",'Bilal Communication') ";
                     clsDataLayer.ExecuteQuery(insq);
                     global = 0; 

                 }
             }
         }

         private void PaymentDue()
         {
             String seldues = "Select * from PaymentDue where PartyCode='" + Code + "'";
             DataTable dxs = clsDataLayer.RetreiveQuery(seldues);
             if (dxs.Rows.Count > 0)
             {
                 decimal totals = Convert.ToDecimal(dxs.Rows[0]["TotalAmount"].ToString());
                 decimal dues = Convert.ToDecimal(dxs.Rows[0]["DueAmount"].ToString());
                 decimal paids = Convert.ToDecimal(dxs.Rows[0]["PaidAmount"].ToString());
                 decimal pays = Convert.ToDecimal(txttotal.Text);
                 decimal ab = 0;
                 decimal final = 0;
                 if (global > 0)
                 {
                     ab = global;
                     final = (dues + ab);
                 }
                 else
                 {
                     ab = Convert.ToDecimal(txttotal.Text);
                     final = (dues - ab);
                     paids += ab;
                 }

                 String fin = final.ToString();
                 decimal abc = 0;
                 if (fin.StartsWith("-"))
                 {
                     global = Math.Abs(final);
                     abc = 0;
                     ReceiveDue();
                 }
                 else
                 {
                     abc = final;
                 }
                 if (fin.StartsWith("-"))
                 {
                     global = Math.Abs(final);
                     abc = 0;
                     ReceiveDue();
                     decimal a = PartyBalance();
                     decimal b = Convert.ToDecimal(dues);
                     decimal c = 0;
                     String name = "";
                     if (!txtCName.Text.Equals("-"))
                     {
                         name = txtCName.Text;
                     }
                     else
                     {
                         name = textBox1.Text;
                     }
                     String ins = @"insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                     " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtreturn.Text + "','Purchase Return'," + b + ",0.00,'OnPayable'," + c + ",'Bilal Communication') insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                     " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + name + "'," + b + ",0.00,'OnPayable' ," + c + ",'Bilal Communication')";

                     if (clsDataLayer.ExecuteQuery(ins) > 0)
                     { 
                     }
                 }
                 else
                 {
                     abc = final;
                     decimal a = PartyBalance();
                     decimal b = Convert.ToDecimal(ab);
                     decimal c = 0;
                     if (global > 0)
                     { c = a + ab; }
                     else { c = a - ab; }
                     String name = "";
                     if (!txtCName.Text.Equals("-"))
                     {
                         name = txtCName.Text;
                     }
                     else
                     {
                         name = textBox1.Text;
                     }
                     String ins = @"insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                     " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtreturn.Text + "','Sale Return'," + b + ",0.00,'OnPayable'," + c + ",'Bilal Communication') insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                     " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + name + "'," + b + ",0.00,'OnPayable' ," + c + ",'Bilal Communication')";
                     if (clsDataLayer.ExecuteQuery(ins) > 0)
                     {
                         
                     }

                 }
                 string updateblnc = "update PaymentDue set DueAmount=" + abc.ToString() + "  , PaidAmount = " + paids + " where PartyCode='" + Code + "' ";
                 if (clsDataLayer.ExecuteQuery(updateblnc) > 0)
                 { 
                 }
             }
             else
             {
                 decimal ab = 0;
                 if (global > 0)
                 {
                     ab = global;
                 }
                 else
                 {
                     ab = Convert.ToDecimal(txttotal.Text);
                 }
                 String ii = @"insert into PaymentDue (PayCode,PartyName,PartyCode,TotalAmount,DueAmount,PaidAmount,Company_Name)
                values('" + txtreturn.Text + "','" + txtCName.Text + "','" + Code + "'," + ab + "," + ab + ",0,'Bilal Communication')";

                 if (clsDataLayer.ExecuteQuery(ii) > 0)
                 {
                     global = 0;
                     decimal b = Convert.ToDecimal(ab);
                     decimal c = ab;
                       String name = "";
                 if (!txtCName.Text.Equals("-"))
                 {
                     name = txtCName.Text;
                 }
                 else
                 {
                     name = textBox1.Text;
                 }
                     String ins = @"insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " + " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + name + "'," + b + ",0.00,'OnPayable' ," + c + ",'Bilal Communication') insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                     " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtreturn.Text + "','Purchase Return'," + b + ",0.00,'OnPayable'," + c + ",'Bilal Communication') ";
                     clsDataLayer.ExecuteQuery(ins); 
                 }
             }
         }


         private void CustomerLedger()
         {
         double b = Convert.ToDouble(txttotal.Text);
         double a = Convert.ToDouble(totrep.Text);
         double c = a - b;
         String ans = c.ToString();
         if (ans.StartsWith("-"))
         {
             //Payable
             CustomerReturn(); CustomerPayable();
         }
         else
         {
             //Receiveable
             CustomerReturn(); CustomerReceiveable();
         }
         }

         private void CustomerReturn()
         {
             PartyCode();
             String rec = "select * from ReceiveDue where PartyCode = '" + Code + "'";
             DataTable d = clsDataLayer.RetreiveQuery(rec);
             if (d.Rows.Count > 0)
             {
                 decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
                 decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
                 decimal received = decimal.Parse(d.Rows[0]["ReceivedAmount"].ToString());
                 decimal bill = decimal.Parse(txttotal.Text);
                 total -= bill; due -= bill;

                 decimal a = PartyBalance();
                 decimal b = Convert.ToDecimal(txttotal.Text);
                 decimal c = a - b;
                 String name = "";
                 name = txtCName.Text;
                 String ins = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                 " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','01010201','" + Code + "','" + txtreturn.Text + "','" + name + "'," + b + ",0.00,'OnReceived'," + c + ",'Bilal Communication') insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                 " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','"+Code+"','" + Code + "','" + txtreturn.Text + "','Sales Return',"+b+",0.00,'OnReceived' ," + c + ",'Bilal Communication')";
                 clsDataLayer.ExecuteQuery(ins);
                 string updateblnc = "update ReceiveDue set DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                 clsDataLayer.ExecuteQuery(updateblnc);
             }
         }

         private void CustomerReceiveable()
         {
              PartyCode();
             String rec = "select * from ReceiveDue where PartyCode = '" + Code + "'";
             DataTable d = clsDataLayer.RetreiveQuery(rec);
             if (d.Rows.Count > 0)
             {
             decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
             decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
             decimal received = decimal.Parse(d.Rows[0]["ReceivedAmount"].ToString());
             decimal bill = decimal.Parse(txtnetamount.Text);
             total += bill;
             due += bill;
             
             decimal a = PartyBalance();
             decimal b = Convert.ToDecimal(txtnetamount.Text);
             decimal c = a + b;
             String name = "";
                 name = txtCName.Text;
             String ins = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
             " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + name + "'," + b + ",0.00,'OnCredit'," + c + ",'Bilal Communication') insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
             " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','01010201','" + Code + "','" + txtreturn.Text + "','Sales'," + b + ",0.00,'OnCredit' ," + c + ",'Bilal Communication')";
             clsDataLayer.ExecuteQuery(ins);
             string updateblnc = "update ReceiveDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
             clsDataLayer.ExecuteQuery(updateblnc);
             }
         }

         private void CustomerPayable()
         {
             PartyCode();
             String rec = "select * from PaymentDue where PartyCode = '" + Code + "'";
             DataTable d = clsDataLayer.RetreiveQuery(rec);
             if (d.Rows.Count > 0)
             {
            decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
            decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
            decimal received = decimal.Parse(d.Rows[0]["PaidAmount"].ToString());
            decimal bill = decimal.Parse(txtnetamount.Text);
            decimal sg = Math.Abs(bill);
            total += sg;
            due += sg;
            string updateblnc = "update PaymentDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
            clsDataLayer.ExecuteQuery(updateblnc);
             }
             else
             {
                 decimal bs = Math.Abs(Convert.ToDecimal(txtnetamount.Text));
            String ii = @"insert into PaymentDue (PayCode,PartyName,PartyCode,TotalAmount,DueAmount,PaidAmount,Company_Name) values('" + txtreturn.Text + "','" + txtCName.Text + "','" + Code + "'," + bs + "," + bs + ",0,'Bilal Communication')";
            clsDataLayer.ExecuteQuery(ii);
             }
                 decimal a = PartyBalance();
                 decimal b = Convert.ToDecimal(txtnetamount.Text);
                 decimal c = a + b;
                 String name = "";
                 if (!txtCName.Text.Equals("-"))
                 {
                     name = txtCName.Text;
                 }
                 String ins = @"insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                              " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + name + "',0.00," + txtnetamount.Text + ",'OnPayabale '," + c + ",'Bilal Communication')  insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                              " Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','01010204 ','" + Code + "','" + txtreturn.Text + "','Replace'," + txtnetamount.Text + ",0.00,'OnPayabale ' ," + c + ",'Bilal Communication')";
                 clsDataLayer.ExecuteQuery(ins);
             
         }



         private void ReceiveDues()
         {
             PartyCode();
             String rec = "select * from ReceiveDue where PartyCode = '" + Code + "'";
             DataTable d = clsDataLayer.RetreiveQuery(rec);
             if (d.Rows.Count > 0)
             {
                 decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
                 decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
                 decimal received = decimal.Parse(d.Rows[0]["ReceivedAmount"].ToString());
                 decimal bill = decimal.Parse(totrep.Text);
                 total += bill;
                 due += bill;

                 decimal a = PartyBalance();
                 decimal b = Convert.ToDecimal(totrep.Text);
                 decimal c = a + b;
                 String name = "";
                 if (!txtCName.Text.Equals("-"))
                 {
                     name = txtCName.Text;
                 }
                 else
                 {
                     name = textBox1.Text;
                 }
                 String ins = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                                      " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + name + "'," + totrep.Text + ",0.00,'OnCredit'," + c + ",'Bilal Communication') insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                                      " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','01010201','" + Code + "','" + txtreturn.Text + "','Sales'," + totrep.Text + ",0.00,'OnCredit' ," + c + ",'Bilal Communication')";
                 clsDataLayer.ExecuteQuery(ins);

                 string updateblnc = "update ReceiveDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                 clsDataLayer.ExecuteQuery(updateblnc);
             }
             else
             {
                 String ii = @"insert into ReceiveDue (RemCode,PartyName,PartyCode,TotalAmount,DueAmount,ReceivedAmount,Company_Name)
                values('" + txtreturn.Text + "','" + txtCName.Text + "','" + Code + "'," + totrep.Text + "," + totrep.Text + ",0,'Bilal Communication')";
                 clsDataLayer.ExecuteQuery(ii);

                 decimal a = PartyBalance();
                 decimal b = Convert.ToDecimal(totrep.Text);
                 decimal c = a + b;
                 String name = "";
                 if (!txtCName.Text.Equals("-"))
                 {
                     name = txtCName.Text;
                 }
                 else
                 {
                     name = textBox1.Text;
                 }
                 String ins = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                                      " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + name + "'," + totrep.Text + ",0.00,'OnCredit'," + c + ",'Bilal Communication') insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                                      " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','01010201','" + Code + "','" + txtreturn.Text + "','Sales'," + totrep.Text + ",0.00,'OnCredit' ," + c + ",'Bilal Communication')";
                 clsDataLayer.ExecuteQuery(ins);
             }
         }

         private void PaymentDues()
         {
             PartyCode();
             String rec = "select * from PaymentDue where PartyCode = '" + Code + "'";
             DataTable d = clsDataLayer.RetreiveQuery(rec);
             if (d.Rows.Count > 0)
             {
                 decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
                 decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
                 decimal received = decimal.Parse(d.Rows[0]["PaidAmount"].ToString());
                 decimal bill = decimal.Parse(totrep.Text);
                 total += bill;
                 due += bill;

                 string updateblnc = "update PaymentDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                 clsDataLayer.ExecuteQuery(updateblnc);
                 decimal a = PartyBalance();
                 decimal b = Convert.ToDecimal(totrep.Text);
                 decimal c = a + b;
                 String name = "";
                 if (!txtCName.Text.Equals("-"))
                 {
                     name = txtCName.Text;
                 }
                 else
                 {
                     name = textBox1.Text;
                 }
                 String ins = @"insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                              " Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','01010204 ','" + Code + "','" + txtreturn.Text + "','Purchase'," + totrep.Text + ",0.00,'OnPayabale ' ," + c + ",'Bilal Communication')  insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                              " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + name + "',0.00," + totrep.Text + ",'OnPayabale '," + c + ",'Bilal Communication') ";
                 clsDataLayer.ExecuteQuery(ins);
             }
             else
             {
                 String ii = @"insert into PaymentDue (PayCode,PartyName,PartyCode,TotalAmount,DueAmount,PaidAmount,Company_Name)
                values('" + txtreturn.Text + "','" + txtCName.Text + "','" + Code + "'," + totrep.Text + "," + totrep.Text + ",0,'Bilal Communication')";
                 clsDataLayer.ExecuteQuery(ii);
                 decimal a = PartyBalance();
                 decimal b = Convert.ToDecimal(totrep.Text);
                 decimal c = a + b;
                 String name = "";
                 if (!txtCName.Text.Equals("-"))
                 {
                     name = txtCName.Text;
                 }
                 else
                 {
                     name = textBox1.Text;
                 }
                 String ins = @"insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                              " Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','01010204 ','" + Code + "','" + txtreturn.Text + "','Purchase'," + totrep.Text + ",0.00,'OnPayabale ' ," + c + ",'Bilal Communication')  insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                              " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + name + "',0.00," + totrep.Text + ",'OnPayabale '," + c + ",'Bilal Communication') ";
                 clsDataLayer.ExecuteQuery(ins);
             }
         }

         #region comment


         //         private void ReceiveDue()
//         {
//             String rec = "select * from ReceiveDue where PartyCode = '" + Code + "'";
//             DataTable d = clsDataLayer.RetreiveQuery(rec);
//             if (d.Rows.Count > 0)
//             {
//                 decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
//                 decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
//                 decimal received = decimal.Parse(d.Rows[0]["ReceivedAmount"].ToString());
//                 decimal bill = decimal.Parse(txtavailable.Text);
//                 if (due > 0)
//                 {
//                     if (due >= bill)
//                     {
//                         total -= bill;
//                         due   -= bill;
//                         decimal a = PartyBalance();
//                         decimal b = bill;
//                         decimal c = a - b;
//                         String ins = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance) " +
//                         " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtreturn.Text + "','Sale Return'," + txtavailable.Text + ",0.00,'OnReceived'," + c + ") insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
//                         " Particulars,Credit,Debit,PaymentType,Party_Balance) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + txtCName.Text + "'," + txtavailable.Text + ",0.00,'OnReceived' ," + c + ")";

//                         if (clsDataLayer.ExecuteQuery(ins) > 0)
//                         {
//                             MessageBox.Show("Order is now " + comboOrder.Text + "!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                             disable();
//                             btnNew.Enabled = true;
//                             btnSave.Enabled = false;
//                         }
//                     }
//                     else
//                     {
//                         bill -= due;
//                         global = bill;
//                         decimal a = PartyBalance();
//                         decimal b = Convert.ToDecimal(due);
//                         decimal c = a - b;
//                         String ins = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance) " +
//                         " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtreturn.Text + "','Sale Return'," + bill + ",0.00,'OnReceived'," + c + ") insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
//                         " Particulars,Credit,Debit,PaymentType,Party_Balance) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + txtCName.Text + "'," + bill + ",0.00,'OnReceived' ," + c + ")";

//                         if (clsDataLayer.ExecuteQuery(ins) > 0)
//                         {
//                             decimal aa = PartyBalance();
//                             decimal bb = Convert.ToDecimal(global);
//                             decimal cc = aa + bb;
//                             String inss = @"insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance) " +
//                                           " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + txtCName.Text + "'," + global + ",0.00,'OnPayable'," + cc + ") insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
//                                           " Particulars,Credit,Debit,PaymentType,Party_Balance) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtreturn.Text + "','Sales'," + global + ",0.00,'OnPayable' ," + cc + ")";
//                             if (clsDataLayer.ExecuteQuery(inss) > 0)
//                             {
//                                 MessageBox.Show("Order is now " + comboOrder.Text + "!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                                 disable();
//                                 btnNew.Enabled = true;
//                                 btnSave.Enabled = false;
//                             }
//                         }
//                         PaymentDue();
//                         due = 0;
//                     }
//                     string updateblnc = "update ReceiveDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
//                     if (clsDataLayer.ExecuteQuery(updateblnc) > 0)
//                     {
//                         global = 0;
//                     }
//                 }
//                 else
//                 {
//                     global = bill;
//                     PaymentDue();
//                 }
//             }
//             else
//             {
//                 String ii = @"insert into ReceiveDue (RemCode,PartyName,PartyCode,TotalAmount,DueAmount,ReceivedAmount)
//                values('" + txtreturn.Text + "','" + txtCName.Text + "','" + Code + "'," + txtavailable.Text + "," + txtavailable.Text + ",0)";

//                 if (clsDataLayer.ExecuteQuery(ii) > 0)
//                 {
//                     global = 0;
//                 }
//             }
//         }

//         private void PaymentDue()
//         {
//             String seldues = "Select * from PaymentDue where PartyCode='" + Code + "'";
//             DataTable dxs = clsDataLayer.RetreiveQuery(seldues);
//             if (dxs.Rows.Count > 0)
//             {
//                 decimal totals = Convert.ToDecimal(dxs.Rows[0]["TotalAmount"].ToString());
//                 decimal dues = Convert.ToDecimal(dxs.Rows[0]["DueAmount"].ToString());
//                 decimal paids = Convert.ToDecimal(dxs.Rows[0]["PaidAmount"].ToString());
//                 decimal pays = Convert.ToDecimal(txtavailable.Text);
//                 if (dues > 0)
//                 {
//                     decimal ab = 0;
//                     if (global > 0)
//                     {
//                         ab = global;
//                     }
//                     else
//                     {
//                         ab = Convert.ToDecimal(txtavailable.Text);
//                     }

//                     if (comboBox1.SelectedIndex == 0)
//                     {
//                         totals += ab;
//                         dues += ab;

//                     }
//                     else
//                     {
//                         if (dues > pays)
//                         {
//                             totals -= pays;
//                             dues -= pays;
//                             decimal aa = PartyBalance();
//                             decimal bb = Convert.ToDecimal(txtavailable.Text);
//                             decimal cc = aa - bb;
//                             String inss = @"insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
//                                           " Particulars,Credit,Debit,PaymentType,Party_Balance) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101010','" + Code + "','" + txtreturn.Text + "','Purchase Return'," + txtavailable.Text + ",0.00,'OnPaid' ," + cc + ")insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance) " +
//                                           " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + txtCName.Text + "',0.00," + txtavailable.Text + ",'OnPaid'," + cc + ")";
//                             if (clsDataLayer.ExecuteQuery(inss) > 0)
//                             {
//                                 MessageBox.Show("Order is now " + comboOrder.Text + "!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                                 disable();
//                                 btnNew.Enabled = true;
//                                 btnSave.Enabled = false;
//                             }
//                         }
//                         else
//                         {
//                             pays -= dues;
//                             global = pays;
//                             decimal aa = PartyBalance();
//                             decimal bb = Convert.ToDecimal(dues);
//                             decimal cc = aa - bb;
//                             String inss = @"insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance) " +
//                                           " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + txtCName.Text + "',0.00," + dues + ",'OnPaid'," + cc + ") insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
//                                           " Particulars,Credit,Debit,PaymentType,Party_Balance) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101010','" + Code + "','" + txtreturn.Text + "','Purchase Return'," + dues + ",0.00,'OnPaid' ," + cc + ")";
//                             if (clsDataLayer.ExecuteQuery(inss) > 0)
//                             {
//                                 decimal a = PartyBalance();
//                                 decimal b = Convert.ToDecimal(pays);
//                                 decimal c = a + b;
//                                 String ins = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
//                                 " Particulars,Credit,Debit,PaymentType,Party_Balance) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + txtCName.Text + "'," + pays + ",0.00,'OnReceived' ," + c + ")insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance) " +
//                                 " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtreturn.Text + "','Purchase Return'," + pays + ",0.00,'OnReceived'," + c + ")";

//                                 MessageBox.Show("Order is now " + comboOrder.Text + "!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                                 disable();
//                                 btnNew.Enabled = true;
//                                 btnSave.Enabled = false;
//                             }
//                             ReceiveDue();
//                             dues = 0;
//                         }
         //                     }

      
         //                     String qs = "update PaymentDue set TotalAmount=" + totals + " , DueAmount=" + dues + " where PartyCode='" + Code + "'";
//                     if (clsDataLayer.ExecuteQuery(qs) > 0)
//                     {
//                         global = 0;
//                     }
//                 }
//                 else
//                 {
//                     global = pays;
//                     ReceiveDue();
//                 }
//             }
//             else
//             {
//                 decimal ab = 0;
//                 if (global > 0)
//                 {
//                     ab = global;
//                 }
//                 else
//                 {
//                     ab = Convert.ToDecimal(txtavailable.Text);
//                 }
//                 String ii = @"insert into PaymentDue (PayCode,PartyName,PartyCode,TotalAmount,DueAmount,PaidAmount)
//                values('" + txtreturn.Text + "','" + txtCName.Text + "','" + Code + "'," + ab + "," + ab + ",0)";

//                 if (clsDataLayer.ExecuteQuery(ii) > 0)
//                 {
//                     global = 0;
//                 }
//             }
         //         }

         #endregion comment

        decimal balance = 0;
        public decimal PartyBalance()
        {
            try
            {
                String get = "";
                if (comboBox1.SelectedIndex == 1)
                {
                    get = "select Party_Balance from LedgerPayment where RefCode='" + Code + "' order by ID desc";
                }
                else
                {
                    get = "select Party_Balance from LedgerReceived where RefCode='" + Code + "' order by ID desc";
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
            String name = "";
            if (!txtCName.Text.Equals("-"))
            {
                name = txtCName.Text;
            }
            else
            {
                name = textBox1.Text;
            }
            String sel = "select ActCode from Accounts where ActTitle = '" + name + "'";
            DataTable dc = clsDataLayer.RetreiveQuery(sel);
            if (dc.Rows.Count > 0)
            {
                Code = dc.Rows[0][0].ToString();
            }
        }

        private void disable()
        {
            btnCustomerSearch.Enabled = false;
            button1.Enabled = false;
            txt1.Enabled = false;
            row.Enabled = false;
            button1.Enabled = false;
            txt1.Enabled = false;
            comboBox1.Enabled = false;
            textBox1.Enabled = false;
            txtSearchSID.Enabled = false;
            groupBox3.Enabled = false;
         
            txtCName.Enabled = false;
    
            dataGridView1.Enabled = false;
            
            date3.Enabled = false;
            comboOrder.Enabled = false;
            txtRetReason.Enabled = false;
            btnSave.Enabled = false;
            dataGridView1.Enabled = false;
            dataGridView3.Enabled = false;
            dataGridView1.DataSource =null;
            dataGridView3.DataSource = null;
            txt1.Text = "";

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DoPrint();
        }

        private void DoPrint() 
        {
            try
            {
                FormID = "Print";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    if (!txtSearchSID.Text.Equals(""))
                    {
           DataTable df = clsDataLayer.RetreiveQuery("select * from RepalceView where SaleId = '" + txtSearchSID.Text + "'");
           if (df.Rows.Count > 0)
           {
               ReplaceOrders ret = new ReplaceOrders();
               ret.SetDataSource(df); 
               ReplaceView rop = new ReplaceView(ret);
               rop.Show();
           }
           else
           {
               MessageBox.Show("No Record Found!!");
           }
                    }
                    else
                    {
                        MessageBox.Show("Please Fill Sale Id!");
                    }
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }
        private void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            this.Hide();
            if(comboBox1.Text.Equals("Customer"))
            {
                View_All_Sale v = new View_All_Sale("Repalce");
                v.Show();
            }
            else
            {
                Purchase_Detail pd = new Purchase_Detail("Repalce");
                pd.Show();
            }
        
      }

        private void customerid()
        {
            String qs = "select * from tbl_sale where SaleCode = '" + txtSearchSID.Text + "' ";
            DataTable dc = clsDataLayer.RetreiveQuery(qs);
           if (dc.Rows.Count > 0)
           {
               Cus = dc.Rows[0][0].ToString();
  
           }
        }

        private void txtSearchSID_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                string qs = "select ProductName,PricePerProduct,Quantity,GrossAmount from tbl_SaleDetail WHERE SDCode='" + txtSearchSID.Text + "'";
                DataTable dd = clsDataLayer.RetreiveQuery(qs);
                if (dd.Rows.Count > 0)
                {
           foreach (DataRow item in dd.Rows)
           {
           int n = dataGridView1.Rows.Add();
           dataGridView1.Rows[n].Cells[0].Value = item["ProductName"].ToString();
           dataGridView1.Rows[n].Cells[1].Value = item["PricePerProduct"].ToString();
           dataGridView1.Rows[n].Cells[2].Value = item["Quantity"].ToString();
           dataGridView1.Rows[n].Cells[3].Value = item["GrossAmount"].ToString();
            } 
                    //Calculation For Total Amount
                    try
                    {
                        double namt = 0;
                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[3].Value != null)
                            {
                                namt = namt + Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value.ToString());
                            }
                        }
                   int ans = Convert.ToInt32(namt);   txttotal.Text = ans.ToString(); 
                    }
                    catch (Exception ex)   { string name = ex.Message;    }

                }

            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                decimal Quantity = 0;
                decimal Price = 0;
                decimal Now = 0;
               // Sprice QUANTITY TOTAL_AMOUNT
                // Calculation for New Product
                if (dataGridView1.CurrentRow.Cells["Sprice"].Value != null && dataGridView1.CurrentRow.Cells["Sprice"].Value.ToString().Trim() != null)
                {
                    Price = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Sprice"].Value);
                }
                if (dataGridView1.CurrentRow.Cells["QUANTITY"].Value != null && dataGridView1.CurrentRow.Cells["QUANTITY"].Value.ToString().Trim() != null)
                { 
                    if (comboBox1.SelectedIndex == 0)
                    {
                        String Name = dataGridView1.CurrentRow.Cells["Pname"].Value.ToString();
                        String h = "select QUANTITY from add_product where NAME = '" + Name + "' ";
                        DataTable dx = clsDataLayer.RetreiveQuery(h);
                        if (dx.Rows.Count > 0)
                        {
                            //
                            String hm = "select Quantity from tbl_SaleDetail where SDCode = '" + txtSearchSID.Text + "' and ProductName = '" + Name + "'";
                        DataTable dxs = clsDataLayer.RetreiveQuery(hm);
                        if (dxs.Rows.Count > 0)
                        {
                            decimal qus = Convert.ToDecimal(dxs.Rows[0][0].ToString());
                            Quantity = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["QUANTITY"].Value);
                            if (Quantity > qus)
                            { 
                               // MessageBox.Show("You Cant Enter Quantity Greater Than!" + qus);
                                dataGridView1.CurrentRow.Cells["QUANTITY"].Value = qus.ToString();
                                Quantity = qus;

                            }
                            else
                            {
                                //
                            decimal qu = Convert.ToDecimal(dx.Rows[0][0].ToString());
                            Quantity = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["QUANTITY"].Value);
                            if (qu == 0)
                            {

                            }
                            else if (Quantity > qu)
                            {
                                Now = qu * Price;
                              //  MessageBox.Show("Stock Only " + qu + " Quantity Available");
                                dataGridView1.CurrentRow.Cells["QUANTITY"].Value = qu.ToString();
                            }
                            else
                            {
                                Now = Quantity * Price;
                            }

                            }
                        }
                       }
                    }
                    else
                    {
                        String Name = dataGridView1.CurrentRow.Cells["Pname"].Value.ToString();
                        String h = "select Quantity from PurchaseDetail where PurchaseItem='" + Name + "'";
                        DataTable dx = clsDataLayer.RetreiveQuery(h);
                        if (dx.Rows.Count > 0)
                        {
                          String hm = "select Quantity from PurchaseDetail where PurCode = '" + txtSearchSID.Text + "' and PurchaseItem = '" + Name + "'";
                            DataTable dxs = clsDataLayer.RetreiveQuery(hm);
                            if (dxs.Rows.Count > 0)
                            {
                                decimal qus = Convert.ToDecimal(dxs.Rows[0][0].ToString());
                                Quantity = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["QUANTITY"].Value);
                                if (Quantity > qus)
                                {
                                 //   MessageBox.Show("You Cant Enter Quantity Greater Than!" + qus);
                                    dataGridView1.CurrentRow.Cells["QUANTITY"].Value = qus.ToString();
                                    Quantity = qus;

                                }
                                else
                                {
                                    decimal qu = Convert.ToDecimal(dx.Rows[0][0].ToString());
                                    Quantity = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["QUANTITY"].Value);
                                    if (qu == 0)
                                    {

                                    }
                                    else if (Quantity > qu)
                                    {
                                        Now = qu * Price;
                                     //   MessageBox.Show("Stock Only " + qu + " Quantity Available");
                                        dataGridView1.CurrentRow.Cells["QUANTITY"].Value = qu.ToString();
                                    }
                                    else
                                    {
                                        Now = Quantity * Price;
                                    }
                                }
                            }

                        
                        }
                    }

                    
                }
                 
                dataGridView1.CurrentRow.Cells["TOTAL_AMOUNT"].Value = Now.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //Calculation For Total Amount
            try
            {
                double namt = 0;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[3].Value != null)
                    {
                        namt = namt + Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    }
                }
                int ans = Convert.ToInt32(namt);
                txttotal.Text = ans.ToString();
             
            }
            catch (Exception ex)
            {
                string name = ex.Message;
            }

        }

        private void row_Click(object sender, EventArgs e)
        {

            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                if ((bool)dataGridView1.Rows[i].Cells[5].FormattedValue)
                {
                    dataGridView1.Rows.RemoveAt(i);
                }
            }
            double namt = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells[3].Value != null)
                {
                    namt = namt + Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value.ToString());
                }
            }
            int ans = Convert.ToInt32(namt);
            txttotal.Text = ans.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          //
                     bool h = false;
                     for (int r = 0; r < dataGridView3.Rows.Count; r++)
                {
                    if (dataGridView3.Rows[r].Cells[0].Value.Equals(txt1.Text))
                    {
                        h = true;
                    }
                }
                if (h == false)
                {
             
                string qs = "select NAME,SELL_PRICE,QUANTITY from [dbo].[add_product] where NAME='" + txt1.Text + "'";
                DataTable dd = clsDataLayer.RetreiveQuery(qs);
                if (dd.Rows.Count > 0)
                {
                    foreach (DataRow item in dd.Rows)
                    {
                        int n = dataGridView3.Rows.Add();
                        dataGridView3.Rows[n].Cells[0].Value = item["NAME"].ToString();
                        if (comboBox1.Text.Equals("Vender"))
                        {
                            dataGridView3.Rows[n].Cells[1].Value = 0;
                            dataGridView3.Rows[n].Cells[2].Value = 0;
                        }
                        else
                        {
                            dataGridView3.Rows[n].Cells[1].Value = item["SELL_PRICE"].ToString();
                            dataGridView3.Rows[n].Cells[2].Value = item["QUANTITY"].ToString();
                        }
                        decimal sprice = Convert.ToDecimal(dataGridView3.Rows[n].Cells[1].Value);
                        decimal squant = Convert.ToDecimal(dataGridView3.Rows[n].Cells[2].Value);
                        decimal total = sprice * squant;

                        dataGridView3.Rows[n].Cells[3].Value = total.ToString();
                    } 
                    //Calculation For Total Amount
                    try
                    {
                        double namt = 0;
                        for (int i = 0; i < dataGridView3.RowCount; i++)
                        {
                            if (dataGridView3.Rows[i].Cells[3].Value != null)
                            {
                                namt = namt + Convert.ToDouble(dataGridView3.Rows[i].Cells[3].Value.ToString());
                            }
                        }
                        int ans = Convert.ToInt32(namt);
                        totrep.Text = ans.ToString();

                    }
                    catch (Exception ex)
                    {
                        string name = ex.Message;
                    } 
                }
                }
                else
                {
                    MessageBox.Show("Already Added. Please Choose New Item!");
                } 
        }

        private void dataGridView3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                decimal Quantity = 0;
                decimal Price = 0;
                decimal Now = 0;
                // Sprice QUANTITY TOTAL_AMOUNT
                // Calculation for New Product
                if (dataGridView3.CurrentRow.Cells["Sprices"].Value != null && dataGridView3.CurrentRow.Cells["Sprices"].Value.ToString().Trim() != null)
                {
                    Price = Convert.ToDecimal(dataGridView3.CurrentRow.Cells["Sprices"].Value);
                }
                if (dataGridView3.CurrentRow.Cells["Squantity"].Value != null && dataGridView3.CurrentRow.Cells["Squantity"].Value.ToString().Trim() != null)
                {
                    if (comboBox1.SelectedIndex == 0)
                    {
                        String Name = dataGridView3.CurrentRow.Cells["Names"].Value.ToString();
                        String h = "select QUANTITY from add_product where NAME = '" + Name + "' ";
                        DataTable dx = clsDataLayer.RetreiveQuery(h);
                        if (dx.Rows.Count > 0)
                        {
                            decimal qu = Convert.ToDecimal(dx.Rows[0][0].ToString());
                            Quantity = Convert.ToDecimal(dataGridView3.CurrentRow.Cells["Squantity"].Value);
                            //if (Quantity > qu)
                            //{
                            //    Now = qu * Price;
                            //    MessageBox.Show("Stock Only " + qu + " Quantity Available");
                            //    dataGridView3.CurrentRow.Cells["Squantity"].Value = qu.ToString();
                            //}
                            //else
                            //{
                                Now = Quantity * Price;
                           // }
                        }
                    }
                    else
                    {
                        String Name = dataGridView3.CurrentRow.Cells["Names"].Value.ToString();
                        //String h = "select Quantity from PurchaseDetail where PurchaseItem='" + Name + "'";
                        //DataTable dx = clsDataLayer.RetreiveQuery(h);
                        //if (dx.Rows.Count > 0)
                        //{
                            //decimal qu = Convert.ToDecimal(dx.Rows[0][0].ToString());
                            //Quantity = Convert.ToDecimal(dataGridView3.CurrentRow.Cells["Squantity"].Value);
                            //if (Quantity > qu)
                            //{
                            //    Now = qu * Price;
                            //    MessageBox.Show("Stock Only " + qu + " Quantity Available");
                            //    dataGridView3.CurrentRow.Cells["Squantity"].Value = qu.ToString();
                            //}
                            //else
                            //{ 
                        Quantity = Convert.ToDecimal(dataGridView3.CurrentRow.Cells["Squantity"].Value);

                                Now = Quantity * Price; 
                    //}
                        //}
                    }
                }
            
                dataGridView3.CurrentRow.Cells["total"].Value = Now.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //Calculation For Total Amount
            try
            {
                double namt = 0;
                for (int i = 0; i < dataGridView3.RowCount; i++)
                {
                    if (dataGridView3.Rows[i].Cells[3].Value != null)
                    {
                        namt = namt + Convert.ToDouble(dataGridView3.Rows[i].Cells[3].Value.ToString());
                    }
                }
                int ans = Convert.ToInt32(namt);
                totrep.Text = ans.ToString();
                
            }
            catch (Exception ex)
            {
                string name = ex.Message;
            }
        }

        private void RepText() 
        {
            // 0 Customer 1 Vender
          
                try
                {
                    double b = Convert.ToDouble(txttotal.Text);
                    double a = Convert.ToDouble(totrep.Text);
                    double c = a - b;
                    String ans = c.ToString();
                    if (ans.StartsWith("-"))
                    {
                        if (comboBox1.SelectedIndex == 0)
                        {
                            status = "Receive";
                            txtavailable.Text = ans.ToString();
//                         txtavailable.Text = Math.Abs(c).ToString();
                        }
                        else
                        {
                            status = "Payable"; 
                             txtavailable.Text = ans.ToString();
                        }
                    }
                    else
                    {
                        if (comboBox1.SelectedIndex == 1)
                        {
                            status = "Receive";
                            txtavailable.Text = ans.ToString();
                        }
                        else
                        {
                            status = "Payable";
                            txtavailable.Text = ans.ToString();
                        }
                    }
                }
                catch { }
            
            }

        private void txttotal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                RepText();
            }
            catch { }
        }

        private void totrep_TextChanged(object sender, EventArgs e)
        {
            try
            {
                RepText();
            }
            catch { }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    btnCustomerSearch.Enabled = true;
                    txtSearchSID.Enabled = true;
                    txtCName.Enabled = true;
                    textBox1.Enabled = false; //SellPrice
                    dataGridView1.Columns[1].HeaderText = "SellPrice";
                    dataGridView3.Columns[1].HeaderText = "SellPrice";
                    dataGridView3.Columns[1].ReadOnly = true;
                    dataGridView1.Columns[4].Visible = false;
                    textBox1.Enabled = false;
                    textBox1.Text = "-";
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    txtSearchSID.Enabled = true;
                    txtCName.Enabled = false;
                    textBox1.Enabled = true;
                    dataGridView1.Columns[1].HeaderText = "Purchase Price";
                    dataGridView3.Columns[1].HeaderText = "Purchase Price";
                    dataGridView3.Columns[1].ReadOnly = false;
                    dataGridView1.Columns[4].Visible = true;
                    txtCName.Text = "-";
                    btnCustomerSearch.Enabled = true;
                    txtCName.Text = "-";
                    enable();
                    btnSave.Enabled = true;
                    btnNew.Enabled = false;
                }
            }
            catch { }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string q = "SELECT PurchaseItem,VendorName,PricePerItem, Quantity,TotalPriceItem FROM PurchaseDetail where PurCode='" + txtSearchSID.Text + "'";
                DataTable vender = clsDataLayer.RetreiveQuery(q);
                if (vender.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                    foreach (DataRow item in vender.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = item["PurchaseItem"].ToString();
                        dataGridView1.Rows[n].Cells[1].Value = item["PricePerItem"].ToString();
                        dataGridView1.Rows[n].Cells[2].Value = item["Quantity"].ToString();
                        dataGridView1.Rows[n].Cells[3].Value = item["TotalPriceItem"].ToString();
                        dataGridView1.Rows[n].Cells[4].Value = item["VendorName"].ToString();
                    }
                }
                else
                {
                    // dataGridView1.Rows.Clear();

                }
                //Calculation For Total Amount
                try
                {
                    double namt = 0;
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[3].Value != null)
                        {
                            namt = namt + Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        }
                    }
                    int ans = Convert.ToInt32(namt);
                    txttotal.Text = ans.ToString();

                }
                catch (Exception ex)
                {
                    string name = ex.Message;
                }
            }
            catch { }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Escape)
                {
                    this.Close();
                }
                else if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-') && (e.KeyChar == (char)Keys.Space) && (e.KeyChar == (char)Keys.Back))
                {
                    e.Handled = true;
                }
            }
            catch { }
        }

        private void txtSearchSID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-') && (e.KeyChar == (char)Keys.Space) && (e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void txtCName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar)  && (e.KeyChar != '.') && (e.KeyChar != '-') && (e.KeyChar == (char)Keys.Space) && (e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void txtRetReason_KeyPress(object sender, KeyPressEventArgs e)
        {
           

            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else {
                e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
            }
        }

        private void grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(gridColumns_KeyPress);
            if (dataGridView1.CurrentCell.ColumnIndex <= 2) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(gridColumns_KeyPress);
                }
            }
            else if (dataGridView3.CurrentCell.ColumnIndex <= 2) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(gridColumns_KeyPress);
                }
            }
           
        }

        private void gridColumns_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.'))
            { e.Handled = true; }
            TextBox txtDecimal = sender as TextBox;
            if (e.KeyChar == '.' && txtDecimal.Text.Contains("."))
            {
                e.Handled = false;
            }

            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsLetter(e.KeyChar) && (e.KeyChar != '-') && (e.KeyChar == (char)Keys.Space) && (e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }


        private void txtcheque_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (!char.IsControl(e.KeyChar)  && !char.IsDigit(e.KeyChar)  && (e.KeyChar == (char)Keys.Space) && (e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void txtGrandTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) || e.KeyChar == '\b' || e.KeyChar == '-' || e.KeyChar == '=' || e.KeyChar == '.' || e.KeyChar == (char)Keys.Space || e.KeyChar == '+')
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }

            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsLetter(e.KeyChar) && (e.KeyChar != '-') && (e.KeyChar == (char)Keys.Space) && (e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void txt1_KeyPress(object sender, KeyPressEventArgs e)
        {
           

            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
           
        }

        private void txtdiscount_TextChanged(object sender, EventArgs e)
        {
        try
        {
        decimal aa = Convert.ToDecimal(txtavailable.Text); decimal ab = Convert.ToDecimal(txtdiscount.Text); decimal cc = aa - ab; txtnetamount.Text = cc.ToString();
        }
        catch { }
        }
     }
}