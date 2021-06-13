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
    public partial class Return_Order : Form
    {
        String Cus = "";
        String UID = Login.UserID;
        public string GreenSignal = "";
        public string FormID = "";
        String Code = "";
        decimal global = 0;
        decimal final = 0;
        public Return_Order()
        {
            InitializeComponent();
            txtsearching.Text = "-"; 
            txtreturn.Text = clsGeneral.getMAXCode("tblReturnOrder", "RID", "R");
            clsGeneral.SetAutoCompleteTextBox(txtvendor
            , clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 24 Order BY ID DESC ")); btnsve.Enabled = false;
            this.KeyPreview = true;

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
   

      
 
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM SaleBill WHERE BillCode LIKE '%" + txtSearch.Text + "%'", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView2.Rows.Clear();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dataGridView2.Rows.Add();
                    dataGridView2.Rows[n].Cells[0].Value = item["SALE_ID"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clear()
        {

            groupBox3.Enabled = false;
         
            txtCName.Clear();
            txttotal.Clear();

            txtSearchSID.Clear();

            dataGridView1.Rows.Clear();
            txtavailable.Clear();
            date3.Value = DateTime.Now;
            comboOrder.SelectedIndex = -1;
            txtRetReason.Clear();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FormID = "Add";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                txtreturn.Text = clsGeneral.getMAXCode("tblReturnOrder", "RID", "R");
                enable(); clear();
                if (txtsearching.SelectedIndex == 1)
                {
                    dataGridView1.Enabled=true;
                }
                else
                {
                    dataGridView3.Enabled = true;
                } 
                btnNew.Enabled = false; btnsve.Enabled = true;
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public void enable()
        {
            txtsearching.Enabled = true;
            txtSearchSID.Enabled = true;
            groupBox3.Enabled = true;
         
            txtCName.Enabled = true;
      
            dataGridView1.Enabled = true;
            txtavailable.Enabled = true;
        
             date3.Enabled = true;
            comboOrder.Enabled = true;
            txtRetReason.Enabled = true;
            
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

        private bool CheckDataGridCells2(DataGridView dgv)
        {
            bool flag = false;
            for (int i = 0; i <= dgv.Rows.Count - 1; i++)
            {
                for (int j = 0; j < 4; j++)
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

        private void btnsave()
        {
            String sel = "select * from tblReturnOrder where SaleId = '" + txtSearchSID.Text + "'";
            DataTable dts = clsDataLayer.RetreiveQuery(sel);
            if (dts.Rows.Count > 0)
            {
                MessageBox.Show("Already Created!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                FormID = "Save";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        if (txtavailable.Text.Equals(txttotal.Text))
                        {
                            if ((!CheckDataGridCells(dataGridView1) && !CheckAllFields()) && (!CheckAllFields1()))
                            {
                                try
                                {
                                Cus = txtCName.Text;
                                String ans = "";
                                if (Cus.Equals("-"))
                                {
                                    ans = txtvendor.Text;
                                }
                                else
                                {
                                    ans = Cus;
                                }
                                try
                                {
                                PartyCode();
                                txtreturn.Text = clsGeneral.getMAXCode("tblReturnOrder", "RID", "R"); 
                                String ins = "insert into tblReturnOrder(ReturnCode,SaleId,CustomerId,ReturnDate,OrderStatus,Reason,TotalPrice,PaymentMethod,BankName,AccountNo,ChequeNo,PaidAmount,UserName)values ('" + txtreturn.Text + "','" + txtSearchSID.Text + "','" + ans + "','" + date3.Value.ToString("MM-dd-yyyy") + "','" + comboOrder.Text + "','" + txtRetReason.Text + "','" + txttotal.Text + "','','','','','" + txtavailable.Text + "','" + Login.UserID + "')";
                                clsDataLayer.ExecuteQuery(ins);
                                for (int r = 0; r < dataGridView1.Rows.Count; r++)
                                {
                                String inss = "insert into tblreturndetail(RCode,RProductName,RQuantity,PerProductPrice,SaleId)VALUES ('" + txtreturn.Text + "','" + dataGridView1.Rows[r].Cells[0].Value.ToString() + "'," + dataGridView1.Rows[r].Cells[3].Value.ToString() + "," + dataGridView1.Rows[r].Cells[1].Value.ToString() + ",'" + txtSearchSID.Text + "')";
                                clsDataLayer.ExecuteQuery(inss);
                                }
                                }catch { }
                                }
                                catch
                                {
                                }

                              
                                // 0 Customer 1 Vendor
                                if (txtsearching.Text.Equals("Vender"))
                                {
                                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                                    {
                                    decimal GQuant = Convert.ToDecimal(dataGridView1.Rows[j].Cells[3].Value.ToString());
                                    String Name = dataGridView1.Rows[j].Cells[0].Value.ToString();
                                    String GetQuant = "select QUANTITY from add_product where NAME='" + Name + "'";
                                    DataTable ds = clsDataLayer.RetreiveQuery(GetQuant);
                                    decimal finals = 0;
                                    decimal Old = 0;
                                    if (ds.Rows.Count > 0)
                                    {
                                    Old = Convert.ToDecimal(ds.Rows[0][0].ToString()); finals = Old - GQuant;
                                    }
                                    string query = " UPDATE add_product SET  QUANTITY ='" + finals + "' WHERE NAME = '" + Name + "'";
                                    clsDataLayer.ExecuteQuery(query);
                                    }
                                    decimal total4 = 0;
                                    String aw = "select TotalPriceItem from PurchaseDetail where PurCode='" + txtSearchSID.Text + "'";
                                    DataTable dq = clsDataLayer.RetreiveQuery(aw);
                                    if (dq.Rows.Count > 0)
                                    {
                                        for (int a1 = 0; a1 < dq.Rows.Count; a1++)
                                        {
                                            total4 += Convert.ToDecimal(dq.Rows[a1][0]);
                                        }
                                    }

                                    String h22 = "update Purchase set ReturnAmount='" + txtavailable.Text + "',Status='" + comboOrder.Text + "' where PurCode = '" + txtSearchSID.Text + "'";
                                    if (clsDataLayer.ExecuteQuery(h22) > 0) { }

                                    PaymentDue(); btnsve.Enabled = false; btnNew.Enabled = true; dataGridView1.Enabled = false;
                                }
                                else { }
                            }
                            else
                            {
                                MessageBox.Show("Please Fill All Fields!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Cash Paid Amount Equal to Bill Amount!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Search Any Sale Id!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void CusSave()
        {
            String sel = "select * from tblReturnOrder where SaleId = '" + txtSearchSID.Text + "'";
            DataTable dts = clsDataLayer.RetreiveQuery(sel);
            if (dts.Rows.Count > 0)
            {
                MessageBox.Show("Already Created!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                FormID = "Save";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    if (dataGridView3.Rows.Count > 0)
                    {
                        if (txtavailable.Text.Equals(txttotal.Text))
                        {
                            if ((!CheckDataGridCells(dataGridView3) && !CheckAllFields()) && (!CheckAllFields1()))
                            {
                                try
                                {
                                    Cus = txtCName.Text;
                                    String ans = "";
                                    if (Cus.Equals("-"))
                                    {
                                        ans = txtvendor.Text;
                                    }
                                    else
                                    {
                                        ans = Cus;
                                    }
                                    try
                                    {
                                        PartyCode();
                                        txtreturn.Text = clsGeneral.getMAXCode("tblReturnOrder", "RID", "R");  
                                        String ins = "insert into tblReturnOrder(ReturnCode,SaleId,CustomerId,ReturnDate,OrderStatus,Reason,TotalPrice,PaymentMethod,BankName,AccountNo,ChequeNo,PaidAmount,UserName)values ('" + txtreturn.Text + "','" + txtSearchSID.Text + "','" + ans + "','" + date3.Value.ToString("MM-dd-yyyy") + "','" + comboOrder.Text + "','" + txtRetReason.Text + "','" + txttotal.Text + "','','','','','" + txtavailable.Text + "','" + Login.UserID + "')";
                                        clsDataLayer.ExecuteQuery(ins);
                                        for (int r = 0; r < dataGridView3.Rows.Count; r++)
                                        {
                                            String inss = "insert into tblreturndetail(RCode,RProductName,RQuantity,PerProductPrice,SaleId)VALUES ('" + txtreturn.Text + "','" + dataGridView3.Rows[r].Cells[0].Value.ToString() + "'," + dataGridView3.Rows[r].Cells[2].Value.ToString() + "," + dataGridView3.Rows[r].Cells[3].Value.ToString() + ",'" + txtSearchSID.Text + "')";
                                            clsDataLayer.ExecuteQuery(inss);
                                        }
                                    }
                                    catch { }
                                }
                                catch
                                {
                                }

                                if (txtsearching.Text.Equals("Customer"))
                                {
                                    try
                                    {
                                        decimal returns = Convert.ToDecimal(txttotal.Text);

                                        String h2 = "update tbl_sale set ReturnAmount='" + returns + "',VoucherStatus='" + comboOrder.Text + "' where SaleCode = '" + txtSearchSID.Text + "'";
                                        if (clsDataLayer.ExecuteQuery(h2) > 0) { }
                                        CustomerReturn();
                                        if (!txtCName.Text.Equals(""))
                                        {
                                            //ReceiveDue();
                                        
                                        }
                                        for (int j = 0; j < dataGridView3.Rows.Count; j++)
                                        {
                                            decimal GQuant = Convert.ToDecimal(dataGridView3.Rows[j].Cells[2].Value.ToString());
                                            String Name = dataGridView3.Rows[j].Cells[0].Value.ToString();
                                            String GetQuant = "select SQuantity from tbl_ColdStorageMaintain where SProduct='" + Name + "'";
                                            DataTable ds = clsDataLayer.RetreiveQuery(GetQuant);
                                            decimal finals = 0;
                                            decimal Old = 0;
                                            if (ds.Rows.Count > 0)
                                            {
                                                Old = Convert.ToDecimal(ds.Rows[0][0].ToString()); finals = Old + GQuant;
                                            }
                                            string query = " UPDATE tbl_ColdStorageMaintain SET  SQuantity ='" + finals + "' WHERE SProduct = '" + Name + "'";
                                            clsDataLayer.ExecuteQuery(query);
                                        }
                                        MessageBox.Show("Order Return Success!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        //btnsve.Enabled = false; 
                                        btnsve.Enabled = false; btnNew.Enabled = true; disable(); dataGridView3.Enabled = false;
                                    } catch { }
                                }    
                            }
                            else
                            {
                                MessageBox.Show("Please Fill All Fields!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Cash Paid Amount Equal to Bill Amount!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Search Any Sale Id!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1 vender 0 customer
            if (txtsearching.SelectedIndex == 1)
            {
                btnsave();
            }
            else
            {
                CusSave();
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
                String ins = @"insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                " values('Sale Return','" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','01010201','" + Code + "','" + txtreturn.Text + "','" + name + "'," + b + ",0.00,'OnReceived'," + c + ",'Delizia') insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('Sale Return','" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','Sales Return'," + b + ",0.00,'OnReceived' ," + c + ",'Delizia')";
                clsDataLayer.ExecuteQuery(ins);
                string updateblnc = "update ReceiveDue set DueAmount=" + due.ToString() + ",ReceivedAmount="+b.ToString()+" where PartyCode='" + Code + "' ";
                clsDataLayer.ExecuteQuery(updateblnc);
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
                decimal bill = decimal.Parse(txtavailable.Text);
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
                 if(fin.StartsWith("-"))
                 {
                     global = Math.Abs(final);
                     abc = 0;
                     PaymentDue();
                     decimal a = PartyBalance();
                     decimal b = Convert.ToDecimal(txttotal.Text);
                     decimal c = a-b;
                   
                     String ascc = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                     " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtreturn.Text + "','Sale Return'," + b + ",0.00,'OnReceived'," + c + ",'Delizia') insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                     " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtCName.Text + "','" + txtCName.Text + "'," + b + ",0.00,'OnReceived' ," + c + ",'Delizia')";
                     clsDataLayer.ExecuteQuery(ascc);
                  }
                 else
                 {
                     abc = final;
                     decimal a = PartyBalance();
                     decimal b = Convert.ToDecimal(txttotal.Text);
                     decimal c = a - b;
                     if (global > 0)
                     {  c = a + ab; }
                     else {  c = a - ab; }
                     String instr = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                     " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtreturn.Text + "','Sale Return'," + b + ",0.00,'OnReceived'," + c + ",'Delizia') insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                     " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + txtCName.Text + "'," + b + ",0.00,'OnReceived' ," + c + ",'Delizia')";
                     clsDataLayer.ExecuteQuery(instr);
                 }

                //
                
                //
                 string updateblnc = "update ReceiveDue set TotalAmount=" + total.ToString() + ", DueAmount=" + abc.ToString() + "  , ReceivedAmount = "+received+" where PartyCode='" + Code + "' ";
                 if(clsDataLayer.ExecuteQuery(updateblnc) > 0)
                 {
                     MessageBox.Show("Order Return Success");
                     disable();      dataGridView1.Enabled = false;  
                 } 
            }
//            else
//            {
//                decimal ab = 0;
//                if (global > 0)
//                {
//                    ab = global;
//                }
//                else
//                {
//                    ab = Convert.ToDecimal(txttotal.Text);
//                }

//                String ii = @"insert into ReceiveDue (RemCode,PartyName,PartyCode,TotalAmount,DueAmount,ReceivedAmount,Company_Name)
//                values('" + txtreturn.Text + "','" + txtvendor.Text + "','" + Code + "'," + ab + "," + ab + ",0,'Delizia')";

//                if (clsDataLayer.ExecuteQuery(ii) > 0)
//                {
//                    decimal b = Convert.ToDecimal(ab);
//                    decimal c = ab;
//                    String insq = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + txtvendor.Text + "'," + b + ",0.00,'OnReceiveable' ," + c + ",'Delizia') insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
//                    " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtreturn.Text + "','Purchase Return'," + b + ",0.00,'OnReceiveable'," + c + ",'Delizia') ";
//                    clsDataLayer.ExecuteQuery(insq);
//                    global = 0;
//                    MessageBox.Show("Order Return Success");

//                }
//            }
        }

        private void PaymentDue()
        {
            PartyCode();
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
                    String ins = @"insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                    " values('Purchase Return','" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtreturn.Text + "','Purchase Return'," + b + ",0.00,'OnPayable'," + c + ",'Delizia') insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                    " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('Purchase Return','" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + txtvendor.Text + "'," + b + ",0.00,'OnPayable' ," + c + ",'Delizia')";
              
                    if (clsDataLayer.ExecuteQuery(ins) > 0)
                    {
                        MessageBox.Show("Order Return Success","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }
                else
                {
                    abc = final;
                    decimal a = PartyBalance();
                    decimal b = Convert.ToDecimal(ab);
                    decimal c = 0;
                    if (global > 0)
                    {   c = a + ab; }
                    else {   c = a - ab; }
                    String ins = @"insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                    " values('Purchase Return','" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtreturn.Text + "','Purchase Return'," + b + ",0.00,'OnPayable'," + c + ",'Delizia') insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                    " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('Purchase Return','" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + txtvendor.Text + "'," + b + ",0.00,'OnPayable' ," + c + ",'Delizia')";
                    if (clsDataLayer.ExecuteQuery(ins) > 0)
                    {
                        MessageBox.Show("Order Return Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                string updateblnc = "update PaymentDue set DueAmount=" + abc.ToString() + "  , PaidAmount = " + paids + " where PartyCode='" + Code + "' ";
                if (clsDataLayer.ExecuteQuery(updateblnc) > 0)
                {
                    MessageBox.Show("Order Return Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
          }
            else
            {
                decimal ab = 0;
                if(global > 0)
                {
                    ab = global;
                }
                else
                {
                    ab = Convert.ToDecimal(txttotal.Text); 
                }
                String ii = @"insert into PaymentDue (PayCode,PartyName,PartyCode,TotalAmount,DueAmount,PaidAmount,Company_Name)
                values('" + txtreturn.Text + "','" + txtCName.Text + "','" + Code + "'," + ab + "," + ab + ",0,'Delizia')";
             
                if (clsDataLayer.ExecuteQuery(ii) > 0)
                {
                    global = 0;
                    decimal b = Convert.ToDecimal(ab);
                    decimal c = ab;
                    String ins = @"insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " + " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtreturn.Text + "','" + txtCName.Text + "'," + b + ",0.00,'OnPayable' ," + c + ",'Delizia') insert into LedgerPayment(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                    " values('" + txtreturn.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtreturn.Text + "','Purchase Return'," + b + ",0.00,'OnPayable'," + c + ",'Delizia') ";
                    clsDataLayer.ExecuteQuery(ins);
                    MessageBox.Show("Order Return Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        decimal balance = 0;
        public decimal PartyBalance()
        {
            try
            {
                String get = "";
                if (txtsearching.SelectedIndex == 1)
                {
                    get = "select Party_Balance from LedgerPayment where RefCode='" + Code + "' order by ID desc";
                }
                else {
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
            if(txtsearching.SelectedIndex==0)
            {
                name = txtCName.Text;
            }
            else
            {
                name = txtvendor.Text;
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
            txtvendor.Enabled = false;
            txtsearching.Enabled = false;
            txtSearchSID.Enabled = false;
            groupBox3.Enabled = false;
           
            txtCName.Enabled = false;
            txtavailable.Enabled = false;
         
            date3.Enabled = false;
            comboOrder.Enabled = false;
            txtRetReason.Enabled = false;
       //     btnsve.Enabled = false;
            btnNew.Enabled = true;
            btnCustomerSearch.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        DataTable dt;
        private void btnPrint_Click(object sender, EventArgs e)
        {
            FormID = "Print";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                if (!txtSearchSID.Text.Equals(""))
                {
                    if (txtsearching.Text.Equals("Customer"))
                    {
                        SaleReturnOrder ret = new SaleReturnOrder();
                        String query = "select * from returnreports where SaleId = '" + txtSearchSID.Text + "'";
                        dt = clsDataLayer.RetreiveQuery(query);
                        if (dt.Rows.Count > 0)
                        {
                            ret.SetDataSource(dt);
                            Return_Order_Preview rop = new Return_Order_Preview(ret);
                            rop.passRetSellID = txtSearchSID.Text;
                            rop.Show();
                        }
                        else
                        {
                            MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                    ReturnOrder ret = new ReturnOrder();
                    String query = "select * from returnreports where SaleId = '" + txtSearchSID.Text + "'";
                    dt = clsDataLayer.RetreiveQuery(query);
                    if (dt.Rows.Count > 0)
                    {
                        ret.SetDataSource(dt);
                        Return_Order_Preview rop = new Return_Order_Preview(ret);
                        rop.passRetSellID = txtSearchSID.Text;
                        rop.Show();
                    }
                    else
                    {
                        MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    }
                    
             
                }
                else
                {
                    MessageBox.Show("Please Fill Sale Id!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            this.Hide();
     
            if(txtsearching.Text.Equals("Vender"))
            {
                Purchase_Detail v1 = new Purchase_Detail("Vender");
                v1.Show();
            }
            else
            {
                View_All_Sale v = new View_All_Sale("Return");
                v.Show();
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
            if (txtsearching.SelectedIndex == -1)
            {
                String hy = "select CustomerName from tbl_sale where SaleCode='"+txtSearchSID.Text+"'";
                DataTable df = clsDataLayer.RetreiveQuery(hy); if (df.Rows.Count > 0)
                {
                    txtCName.Text = df.Rows[0][0].ToString();
                }

                string qs = "select ProductName,PricePerProduct,Quantity,NetAmount from tbl_SaleDetail where SDCode='" + txtSearchSID.Text + "'";
                DataTable dd = clsDataLayer.RetreiveQuery(qs);
                if (dd.Rows.Count > 0)
                {
                    dataGridView3.Rows.Clear();
                    foreach (DataRow item in dd.Rows)
                    {
                        int n = dataGridView3.Rows.Add();
                        dataGridView3.Rows[n].Cells[0].Value = item["ProductName"].ToString();
                        dataGridView3.Rows[n].Cells[1].Value = item["PricePerProduct"].ToString();
                        dataGridView3.Rows[n].Cells[2].Value = item["Quantity"].ToString();
                        dataGridView3.Rows[n].Cells[3].Value = item["NetAmount"].ToString();

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
                    }    int ans = Convert.ToInt32(namt);   txttotal.Text = ans.ToString(); 
                    }
                    catch (Exception ex)
                    {
                        string name = ex.Message;
                    }
                }
            }
         }
 
        private void txtavailable_Leave(object sender, EventArgs e)
        {
            if (txtavailable.Text.Equals(""))
            {
                
                txtavailable.Text = "";
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                decimal Quantity = 0;
                decimal Price = 0;
               // Sprice QUANTITY TOTAL_AMOUNT
                // Calculation for New Product
                if (dataGridView1.CurrentRow.Cells["Sprice"].Value != null && dataGridView1.CurrentRow.Cells["Sprice"].Value.ToString().Trim() != null)
                {
                    Price = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Sprice"].Value);
                }
                if (dataGridView1.CurrentRow.Cells["QUANTITY"].Value != null && dataGridView1.CurrentRow.Cells["QUANTITY"].Value.ToString().Trim() != null)
                { 
              String Name = dataGridView1.CurrentRow.Cells["Pname"].Value.ToString();
              String h = "select Quantity from PurchaseDetail where PurCode = '" + txtSearchSID.Text + "' and PurchaseItem = '" + Name + "' ";
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
                            MessageBox.Show("You Cant Enter Quantity Greater Than!" + qus);
                          dataGridView1.CurrentRow.Cells["QUANTITY"].Value = qus.ToString();
                          Quantity = qus;
             
                      } else
                      {
                          decimal qu = Convert.ToDecimal(dx.Rows[0][0].ToString());
             
                          if (qu == 0)
                          { }
                                    else if (Quantity > qu)
                                    {
                                        MessageBox.Show("Stock Only " + qu + " Quantity Available");
                                        dataGridView1.CurrentRow.Cells["QUANTITY"].Value = qu.ToString();
                                    }
                                }
                            }

                        } 
                }
                decimal Now = Quantity * Price;
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
                    if (dataGridView1.Rows[i].Cells[4].Value != null)
                    {
                        namt = namt + Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value.ToString());
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

        private void txtsearching_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 0 Customer 1 Vendor
           if (txtsearching.SelectedIndex == 0)
            {
                txtSearchSID.Enabled = true;
                btnCustomerSearch.Enabled = true;
                txtCName.Enabled = true;
                txtvendor.Enabled = false; //SellPrice 
                Available.Text = "Bill Amount"; dataGridView3.Visible = true; dataGridView1.Visible = false;
            }
            else if (txtsearching.SelectedIndex == 1)
            {
                btnNew.Enabled = false;
                btnCustomerSearch.Enabled = true;
                txtSearchSID.Enabled = true;
                txtCName.Enabled = false;
                txtvendor.Enabled = true; 
                Available.Text = "Bill Amount"; dataGridView3.Visible = false; dataGridView1.Visible = true;
          }
        }

        private void txtvendor_TextChanged(object sender, EventArgs e)
        {
            string q = @"SELECT   PurchaseDetail.PurchaseItem, PurchaseDetail.PricePerItem,PurchaseDetail.size,PurchaseDetail.Quantity, PurchaseDetail.TotalPriceItem, Purchase.PurCode
            FROM PurchaseDetail INNER JOIN Purchase ON PurchaseDetail.PurCode = Purchase.PurCode where Purchase.PurCode ='" + txtSearchSID.Text + "'";
            DataTable vender = clsDataLayer.RetreiveQuery(q);
            if (vender.Rows.Count > 0)
            {
              txtSearchSID.Text = vender.Rows[0]["PurCode"].ToString();  
              dataGridView1.Rows.Clear();
              foreach (DataRow item in vender.Rows)
              {
                  int n = dataGridView1.Rows.Add();
                  dataGridView1.Rows[n].Cells[0].Value = item["PurchaseItem"].ToString();
                  dataGridView1.Rows[n].Cells[1].Value = item["PricePerItem"].ToString();
                  dataGridView1.Rows[n].Cells[2].Value = item["size"].ToString();
                  dataGridView1.Rows[n].Cells[3].Value = item["Quantity"].ToString();
                   dataGridView1.Rows[n].Cells[4].Value = item["TotalPriceItem"].ToString();
              }
              enable(); txtCName.Text = "-"; txtCName.Enabled = false; txtSearchSID.Enabled = false; btnsve.Enabled = true;
            }
            else
            {
                dataGridView1.Rows.Clear();
            }

            try
            {
                double namt = 0;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[4].Value != null)
                    {
                        namt = namt + Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value.ToString());
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

        private void txtRetReason_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }

            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
        }
 
        private void txtSearchSID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == '\b' || e.KeyChar == '-' || e.KeyChar == '=' || e.KeyChar == '.' || e.KeyChar == (char)Keys.Space || e.KeyChar == '+')
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
            else if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && (e.KeyChar != '-') && (e.KeyChar == (char)Keys.Space) && (e.KeyChar == (char)Keys.Back))
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

            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
        }

        private void txtavailable_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!Char.IsDigit(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '=' || e.KeyChar == '.' || e.KeyChar == (char)Keys.Space || e.KeyChar == '+')
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
            else if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar) && (e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void txtcheque_KeyPress(object sender, KeyPressEventArgs e)
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
            else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void txtPayment_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
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

        private void btndelete_Click(object sender, EventArgs e)
        {
            //
            if (txtsearching.SelectedIndex == 1)
            {
                for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
                {
                    if ((bool)dataGridView1.Rows[i].Cells[5].FormattedValue)
                    {
                        dataGridView1.Rows.RemoveAt(i);
                    }
                }

                try
                {
                    double namt = 0;
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[4].Value != null)
                        {
                            namt = namt + Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value.ToString());
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
            else
            {
                for (int i = dataGridView3.Rows.Count - 1; i >= 0; i--)
                {
                    if ((bool)dataGridView3.Rows[i].Cells[4].FormattedValue)
                    {
                        dataGridView3.Rows.RemoveAt(i);
                    }
                }

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
                    txttotal.Text = ans.ToString();

                }
                catch (Exception ex)
                {
                    string name = ex.Message;
                }
            }  
        }

        private void txttotal_TextChanged(object sender, EventArgs e)
        {
            txtavailable.Text = txttotal.Text;
        }

        private void dataGridView3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                decimal Quantity = 0;
                decimal Price = 0;
                // Sprice QUANTITY TOTAL_AMOUNT
                // Calculation for New Product
                if (dataGridView3.CurrentRow.Cells["Ssp"].Value != null && dataGridView3.CurrentRow.Cells["Ssp"].Value.ToString().Trim() != null)
                {
                    Price = Convert.ToDecimal(dataGridView3.CurrentRow.Cells["Ssp"].Value);
                }
                if (dataGridView3.CurrentRow.Cells[2].Value != null && dataGridView3.CurrentRow.Cells[2].Value.ToString().Trim() != null)
                {

                    if (txtsearching.SelectedIndex == 0)
                    {
                        String Name = dataGridView3.CurrentRow.Cells[0].Value.ToString();
                         
                            String hm = "select Quantity from tbl_SaleDetail where SDCode = '" + txtSearchSID.Text + "' and ProductName = '" + Name + "'";
                            DataTable dxs = clsDataLayer.RetreiveQuery(hm);
                            if (dxs.Rows.Count > 0)
                            {
                                decimal qus = Convert.ToDecimal(dxs.Rows[0][0].ToString());
                                Quantity = Convert.ToDecimal(dataGridView3.CurrentRow.Cells[2].Value);
                                if (Quantity > qus)
                                { 
                                MessageBox.Show("You Cant Enter Quantity Greater Than!" + qus);
                                dataGridView3.CurrentRow.Cells[2].Value = qus.ToString();
                                Quantity = qus; 
                                } 
                            }
                         
                    }
 
                }
                decimal Now = Quantity * Price;
                dataGridView3.CurrentRow.Cells[3].Value = Now.ToString();
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
                txttotal.Text = ans.ToString();

            }
            catch (Exception ex)
            {
                string name = ex.Message;
            }

        }

        private void dataGridView3_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb2 = null; tb2 = e.Control as TextBox;
            if (tb2 != null) { tb2.KeyPress += new KeyPressEventHandler(tb_KeyPress); } 
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dataGridView3.CurrentCell.ColumnIndex == 2) //Desired Column
                {
                    if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.'))
                    { e.Handled = true; }
                    TextBox txtDecimal = sender as TextBox;
                    if (e.KeyChar == '.' && txtDecimal.Text.Contains("."))
                    {
                        e.Handled = true;
                    }
                }
            }
            catch { }
        }

        private void Return_Order_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.A)
            {
                btnNew.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.S)
            {
                btnsve.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.E)
            {
                btnExit.PerformClick();
            } 
            else if (e.Control == true && e.KeyCode == Keys.P)
            {
                btnPrint.PerformClick();
            }
        }
    }
}
