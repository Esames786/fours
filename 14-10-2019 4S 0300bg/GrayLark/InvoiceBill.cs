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
    public partial class InvoiceBill : Form
    {
        String UID = Login.UserID;
        public string GreenSignal = "";
        public string FormID = "";
        decimal old = 0;
        String Code = "";
        int rowindex = -1;
        String status = "";
        public InvoiceBill()
        {
            InitializeComponent();
            clsGeneral.SetAutoCompleteTextBox(txtparty, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 12 and Status = 1 Order BY ID DESC"));
     
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
                String inv = "select * from Invoice";

                DataTable dt = clsDataLayer.RetreiveQuery(inv);

     if(dt.Rows.Count > 0)
     {
         dataGridView1.DataSource = dt;
     }
          
                //foreach (DataRow item in dt.Rows)
                //{
                //    int n = dataGridView1.Rows.Add();
                //    dataGridView1.Rows[n].Cells[0].Value = item["TransId"].ToString();
                //    dataGridView1.Rows[n].Cells[1].Value = item["DATE"].ToString();
                //    dataGridView1.Rows[n].Cells[2].Value = item["TO_FROM"].ToString();
                //    dataGridView1.Rows[n].Cells[3].Value = item["MODE"].ToString();
                //    dataGridView1.Rows[n].Cells[4].Value = item["AVAILABLE"].ToString();
                //    dataGridView1.Rows[n].Cells[5].Value = item["CASH_R"].ToString();
                //    dataGridView1.Rows[n].Cells[6].Value = item["NEW_AMOUNT"].ToString();
                //    dataGridView1.Rows[n].Cells[7].Value = item["REMARKS"].ToString();
                //}
                //dataGridView1.PerformLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int GetRowIndex(string target, int columnIndex)
        {
            int index = -1;
            if (columnIndex - 1 < dataGridView1.ColumnCount)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[columnIndex - 1].Value.ToString() == target)
                    {
                        index = i;
                        //txt_late.Text = dataGridView1.Rows[i].Cells[columnIndex].Value.ToString();
                        //txt_half.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        //txt_absent.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        //txtpresnt.Text = dataGridView1.Rows[i].Cells[5].Value.ToString();


                        //txt_index.Text = i.ToString();
                        rowindex = i;

                        break;
                    }

                }
            }
            else
            {
                MessageBox.Show("Please enter correct Id!");
            }
            return index;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            status = "Add";
            FormID = "Add";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                Clears();
                dataGridView1.DataSource = null;
            enable();
            Enable();
            txttotal.Enabled = false;
            txtnetamount.Enabled = false;
            txtbamount.Enabled = false;
            this.txtparty.Focus();
                //
            txtRemarks.Text = ""; txtsaletax.Text = ""; txtnetamount.Text = ""; txtdiscount.Text = ""; txtbamount.Text = ""; txtparty.Text = ""; 
            try
            {
                txtInvoice.Text = clsGeneral.getMAXCode("Invoice", "TransId", "INV");
                btnEdit.Enabled = false;
                btnupdate.Enabled = false;
                btnSave.Enabled = true;
                btnNew.Enabled = false;
                String get = "select Tax_Percent from tbl_tax"; DataTable dqw = clsDataLayer.RetreiveQuery(get); if (dqw.Rows.Count > 0) { txtsaletax.Text = dqw.Rows[0][0].ToString(); } else { txtsaletax.Text = "0"; }
                txtdiscount.Text = "0";
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

        private void enable()
        {
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            txtInvoice.Enabled = true;
            btn_add.Enabled = true;
            btn_delete.Enabled = true;
            //date1.Enabled = true;
            //date2.Enabled = true;
 
           
            txtparty.Enabled = true;
            txtRemarks.Enabled = true;
        }

       

        private void txtCashRelease_TextChanged(object sender, EventArgs e)
        {
            CashRelease();
        
        }

        private void txtAvailable_TextChanged(object sender, EventArgs e)
        {
            CashRelease();
        }

        private void CashRelease()
        {
            try
            {
                double a = Convert.ToDouble(txttotal.Text);
                double r = Convert.ToDouble(txtbamount.Text);
                double na = a - r;
                String ans = na.ToString();
                if (ans.StartsWith("-"))
                {
                    txtdiscount.Text = "0";
                    txtbamount.Text = "0";
                }
                else
                {
                    txtdiscount.Text = na.ToString();
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
            foreach (Control c in this.Controls)
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
            status = "Save";
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
                            MessageBox.Show("UserName dont have a chart Account");
                        }
                        else
                        {
                            //
                            decimal namt = 0;
                            for (int i = 0; i < dataGridView1.RowCount; i++)
                            {
                                if (dataGridView1.Rows[i].Cells[6].Value != null)
                                {
                                    namt = namt + decimal.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString());
                                }
                            }
                            this.txtbamount.Text = namt.ToString();
                            //
                            String insert = "insert into Invoice(SaleTax,TransId,Dates,Party_Name,Remarks,Bill_Amount,Discount,Net_Amount,Company_Name,UserName)values ('"+txtsaletax.Text+"','" + txtInvoice.Text + "','" + txt_date.Value.ToString("yyyy-MM-dd") + "','" + txtparty.Text + "','" + txtRemarks.Text + "'," + txtbamount.Text + "," + txtdiscount.Text + "," + txtnetamount.Text + ",'Reddot Technologies','" + Login.UserID + "')";
                            clsDataLayer.ExecuteQuery(insert);
                           
                            String am = "delete from Invoice_Detail where TransId='" + txtInvoice.Text + "'";
                            clsDataLayer.ExecuteQuery(am);
                            for (int a = 0; a < dataGridView1.Rows.Count;a++)
                            {
         
               String ins = "insert into Invoice_Detail(TransId,Dates,Party_Name,Product_Name,Qty,PerProductPrice,Total_Amount,Company_Name)values ('" + txtInvoice.Text + "','" + txt_date.Value.ToString("yyyy-MM-dd") + "','" + txtparty.Text + "','" + dataGridView1.Rows[a].Cells[3].Value + "'," + dataGridView1.Rows[a].Cells[4].Value + "," + dataGridView1.Rows[a].Cells[5].Value + "," + dataGridView1.Rows[a].Cells[6].Value + ",'Reddot Technologies')";
             
                 if (clsDataLayer.ExecuteQuery(ins) > 0)  {  }
                               }

                            PartyCode();

                            ReceiveDue();


                            decimal ass = PartyBalance();
                            decimal b = Convert.ToDecimal(txtnetamount.Text);
                            decimal c = ass + b;
                            String inss = @"insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                           " values('" + txtInvoice.Text + "','" + txt_date.Value.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtInvoice.Text + "','" + txtparty.Text + "'," + txtnetamount.Text + ",0.00,'OnCredit'," + c + ",'Reddot Technologies') insert into LedgerReceived(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                           " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtInvoice.Text + "','" + txt_date.Value.ToString("yyyy-MM-dd") + "','01010201','" + Code + "','" + txtInvoice.Text + "','"+txtRemarks.Text+"'," + txtnetamount.Text + ",0.00,'OnCredit' ," + c + ",'Reddot Technologies')";
                      
                            
                            if (clsDataLayer.ExecuteQuery(inss) > 0)
                            {
                             MessageBox.Show("Invoice Create Successfully!");
                               
                                
                                    FormID = "Print";
                                    UserNametesting();
                                    
                                        if (!txtInvoice.Text.Equals(""))
                                        {
                                            String a = "select * from VU_Invoice where TransId = '" + txtInvoice.Text + "' ";
                                            DataTable dw = clsDataLayer.RetreiveQuery(a);
                                            if (dw.Rows.Count > 0)
                                            {
                                                rptCustomerBill cus = new rptCustomerBill();
                                                cus.SetDataSource(dw);
                                                string aa = Login.UserID;
                                                InvoiceView frmGL = new InvoiceView(cus, aa);
                                                frmGL.ShowDialog();
                                            }
                                            else
                                            {
                                                MessageBox.Show("No Record Found!");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Please Fill Pay Id!");
                                        }
                                        txtparty.Clear();
                                        txtnetamount.Clear();
                                        disable();
                                        ShowAll();
                                        btnEdit.Enabled = true;
                                        btn_add.Enabled = false;
                                        btn_delete.Enabled = false;
                            }
                        
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
     
           
            txttotal.Enabled = false;
            txtbamount.Enabled = false;

            txtpname.Enabled = false;
            txtquant.Enabled = false; 
            txtperprodprice.Enabled = false; 

            txtdiscount.Enabled = false;
            txtRemarks.Enabled = false;
             txtInvoice.Clear();
    
           
            txttotal.Clear();
             txtbamount.Clear();
            txtpname.Clear();
            txtquant.Clear();
            txtperprodprice.Clear();
            txtdiscount.Clear();
            txtRemarks.Clear();
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
                decimal bill = decimal.Parse(txtnetamount.Text);
                total += bill;
                due += bill;

                string updateblnc = "update ReceiveDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                clsDataLayer.ExecuteQuery(updateblnc);
            }
            else
            {
                String ii = @"insert into ReceiveDue (RemCode,PartyName,PartyCode,TotalAmount,DueAmount,ReceivedAmount,Company_Name)
                values('" + txtInvoice.Text + "','" + txtparty.Text + "','" + Code + "'," + txtnetamount.Text + "," + txtnetamount.Text + ",0,'Reddot Technologies')";
                clsDataLayer.ExecuteQuery(ii);
            }
        }


        private void txtCashRelease_Leave(object sender, EventArgs e)
        {
            if(txtbamount.Text.Equals(""))
            {
                txtdiscount.Text = txttotal.Text;
                txtbamount.Text = "0";
            }
        }

        private void txtCashRelease_KeyPress(object sender, KeyPressEventArgs e)
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
            else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar == (char)Keys.Space) && (e.KeyChar == (char)Keys.Back))
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
            else 
            {
                e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
            }
          
        }

        private void txtRemarks_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Escape)
            //{
            //    this.Close();
            //}
            //else
            //{

            //    e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
            //}
        }

        private void txtcheque_KeyPress(object sender, KeyPressEventArgs e)
        {

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

        }

        decimal balance = 0;
        public decimal PartyBalance()
        {
            try
            { String get = "select Party_Balance from LedgerReceived where RefCode='" + Code + "' order by ID desc";
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
                        String a = "select * from VU_Invoice where TransId = '" + txtInvoice.Text + "' ";
                        DataTable dw = clsDataLayer.RetreiveQuery(a);
                        if(dw.Rows.Count > 0)
                        {
                        rptCustomerBill cus = new rptCustomerBill();
                        cus.SetDataSource(dw);
                        string aa=Login.UserID;
                        InvoiceView frmGL = new InvoiceView(cus,aa);
                        frmGL.ShowDialog();
                        }
                        else
                        {
                      MessageBox.Show("No Record Found!");
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
          
            Find find = new Find("Transactions","Payment_voucher");
            find.Show();
            this.Hide();
        }

        private void txtInvoice_TextChanged(object sender, EventArgs e)
        {
     
        }

        private bool Enable()
        {
            bool flag = false;
            foreach (Control c in this.Controls)
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
            foreach (Control c in this.Controls)
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
            foreach (Control c in this.Controls)
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Enable();
            status = "Edit";
            old = Convert.ToDecimal(txtnetamount.Text);
            String search = "select Product_Name from VU_Invoice where TransId = '" + txtInvoice.Text+ "'";
            DataTable dt = clsDataLayer.RetreiveQuery(search);
            if (dt.Rows.Count > 0)
            { clsGeneral.SetAutoCompleteTextBox(txtpname, dt);   } 
            txtInvoice.Enabled = true;
            txtInvoice.ReadOnly = false;
            btnupdate.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = false;
            btnEdit.Enabled = false;
            btn_add.Enabled = true;
            btn_delete.Enabled = true;
            txtparty.Enabled = false;
            String get = "select Tax_Percent from tbl_tax"; DataTable dqw = clsDataLayer.RetreiveQuery(get); if (dqw.Rows.Count > 0) { txtsaletax.Text = dqw.Rows[0][0].ToString(); } else { txtsaletax.Text = "0"; }
            txtpname.Text = "";
            txtquant.Text = "";
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            status = "Update";
            FormID = "Update";
             UserNametesting();
            if (GreenSignal == "YES")
            { 
             if (!CheckAllFields())
                    {
     try
     {
   PartyCode();
   String querys = "Delete From Invoice Where TransId = '" + txtInvoice.Text + "' Delete From Invoice_Detail Where TransId = '" + txtInvoice.Text + "'";
   clsDataLayer.ExecuteQuery(querys);

   String insert = "insert into Invoice(SaleTax,TransId,Dates,Party_Name,Remarks,Bill_Amount,Discount,Net_Amount,Company_Name,UserName)values ('" + txtsaletax.Text + "','" + txtInvoice.Text + "','" + txt_date.Value.ToString("yyyy-MM-dd") + "','" + txtparty.Text + "','" + txtRemarks.Text + "'," + txtbamount.Text + "," + txtdiscount.Text + "," + txtnetamount.Text + ",'Reddot Technologies','" + Login.UserID + "')";
   clsDataLayer.ExecuteQuery(insert);

   for (int a = 0; a < dataGridView1.Rows.Count; a++)
   {
   String ins = "insert into Invoice_Detail(TransId,Dates,Party_Name,Product_Name,Qty,PerProductPrice,Total_Amount,Company_Name)values ('" + txtInvoice.Text + "','" + txt_date.Value.ToString("yyyy-MM-dd") + "','" + txtparty.Text + "','" + dataGridView1.Rows[a].Cells[3].Value + "'," + dataGridView1.Rows[a].Cells[4].Value + "," + dataGridView1.Rows[a].Cells[5].Value + "," + dataGridView1.Rows[a].Cells[6].Value + ",'Reddot Technologies')";
       if (clsDataLayer.ExecuteQuery(ins) > 0)    {   }
   }

                        }
                        catch
                        {

                        }

                        ReceiveUpdateDue();

                        int index = GetRowIndex(txtInvoice.Text);
                        DataTable dt = SetDT();
                        ShowDt(dt);
                        Console.WriteLine("Index=" + index);
                        DeleteRecord();
                        dt.Rows[index][7] = Convert.ToDecimal(txtnetamount.Text);
                        dt.Rows[index + 1][8] = Convert.ToDecimal(txtnetamount.Text);
                        dt = SomeOperation(dt);
                        InsertRecord(dt);

                        MessageBox.Show("Invoice Update Successfully!");
                        txtparty.Clear();
                        txtnetamount.Clear();
                        Clears();
                        Disable();
                        disable();
                        btnEdit.Enabled = true;
                        btnNew.Enabled = true;
                        btnupdate.Enabled = false;
                        btnNew.Enabled = true;
                        btn_add.Enabled = false;
                        btn_delete.Enabled = false;
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
        private void ReceiveUpdateDue()
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
                total = (total - old) + bill;
                due = (due - old) + bill;

                string updateblnc = "update ReceiveDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                clsDataLayer.ExecuteQuery(updateblnc);
            }
            else
            {
                String ii = @"insert into ReceiveDue (RemCode,PartyName,PartyCode,TotalAmount,DueAmount,ReceivedAmount,Company_Name)
                values('" + txtInvoice.Text + "','" + txtparty.Text + "','" + Code + "'," + txtnetamount.Text + "," + txtnetamount.Text + ",0,'Reddot Technologies')";
                clsDataLayer.ExecuteQuery(ii);
            }
        }
        private void txtInvoice_Leave(object sender, EventArgs e)
        {
            try
            {
                String search = "select * from Invoice where TransId = '" + txtInvoice.Text + "' ";
                DataTable dt = clsDataLayer.RetreiveQuery(search);
                if (dt.Rows.Count > 0)
                {

                 txtInvoice.Enabled = false;
                 txtInvoice.ReadOnly = true;
                 txtparty.Text = dt.Rows[0]["Party_Name"].ToString();
                 txt_date.Text = dt.Rows[0]["Dates"].ToString();
                 txtbamount.Text = dt.Rows[0]["Bill_Amount"].ToString();
                 txtdiscount.Text = dt.Rows[0]["Discount"].ToString();
                 txtnetamount.Text = dt.Rows[0]["Net_Amount"].ToString();
                 txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                 txtsaletax.Text = dt.Rows[0]["SaleTax"].ToString();
                 String query = "select TransId,Dates,Party_Name,Product_Name,Qty,PerProductPrice,Total_Amount from Invoice_Detail where TransId = '" + txtInvoice.Text + "'";
                  DataTable dc = clsDataLayer.RetreiveQuery(query);
                   if(dc.Rows.Count > 0)
                   {
                       dataGridView1.DataSource = dc;
                       dataGridView1.Enabled = true;
                   }

                    Enable();
                    btnSave.Enabled = false;
                    old = Convert.ToDecimal(txtnetamount.Text);
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
                SqlCommand cmd = new SqlCommand("SELECT V_No FROM LedgerReceived where RefCode = '" + Code + "' order by ID asc", con);
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
                SqlCommand cmd = new SqlCommand("DELETE FROM LedgerReceived where RefCode = '" + Code + "'", con);
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
                SqlDataAdapter cmd = new SqlDataAdapter("select V_No, Datetime, AccountCode, RefCode, PaymentType, InvoiceNo, Particulars, Debit, Credit, Party_Balance,Company_Name from LedgerReceived where RefCode = '" + Code + "' order by ID asc", con);
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
                    SqlCommand cmd = new SqlCommand("INSERT INTO LedgerReceived VALUES('" + dt.Rows[i].ItemArray[0].ToString() + "','" + dt.Rows[i].ItemArray[1].ToString() + "','" + dt.Rows[i].ItemArray[2].ToString() + "','" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','" + dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "'," + dt.Rows[i].ItemArray[7].ToString() + "," + dt.Rows[i].ItemArray[8].ToString() + "," + dt.Rows[i].ItemArray[9].ToString() + ",'" + dt.Rows[i].ItemArray[10].ToString() + "')", con);
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
            String query = "select AccountNo from BankDetail where BankName = '"+txtpname.Text+"'";
            DataTable dq = clsDataLayer.RetreiveQuery(query);
            if (dq.Rows.Count > 0)
            {
                //   clsGeneral.SetAutoCompleteTextBoxs(txtbank, dq);
                clsGeneral.SetAutoCompleteTextBox(txtquant, dq);
            }
            else
            {
                MessageBox.Show("Bank Detail Not Found!");
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            String hy = "select * from Invoice_Detail where Product_Name='" + txtpname.Text + "' and Party_Name='" + txtparty.Text + "' and TransId='"+txtInvoice.Text+"'";
            DataTable dq = clsDataLayer.RetreiveQuery(hy);
            
            if (dq.Rows.Count > 0)
            {
                if (rowindex > -1)
                {
                    dataGridView1.Rows[rowindex].Cells[2].Value = txtparty.Text;
                    dataGridView1.Rows[rowindex].Cells[3].Value = txtpname.Text;
                    dataGridView1.Rows[rowindex].Cells[4].Value = txtquant.Text;
                    dataGridView1.Rows[rowindex].Cells[5].Value = txtperprodprice.Text;
                    dataGridView1.Rows[rowindex].Cells[6].Value = txttotal.Text;
                    try
                    {
                        decimal namt = 0;
                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[6].Value != null)
                            {
                                namt = namt + decimal.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString());
                            }
                        }
                        this.txtbamount.Text = namt.ToString(); 

                    }
                    catch
                    {
                        // string name = ex.Message;
                    }
                }
            }
            else
            {
                try
                {
                    bool h = false;
                    for (int r = 0; r < dataGridView1.Rows.Count; r++)
                    {
                        if (dataGridView1.Rows[r].Cells[1].Value.Equals(txtpname.Text))
                        {
                            h = true;
                        }
                    }

                

                    if (h == false)
                    {
                        String ins = "insert into Invoice_Detail(TransId,Dates,Party_Name,Product_Name,Qty,PerProductPrice,Total_Amount,Company_Name)values ('" + txtInvoice.Text + "','" + txt_date.Value.ToString("yyyy-MM-dd") + "','" + txtparty.Text + "','" + txtpname.Text + "'," + txtquant.Text + "," + txtperprodprice.Text + "," + txttotal.Text + ",'Reddot Technologies')";

                        if (clsDataLayer.ExecuteQuery(ins) > 0)
                        {
                           String select = "SELECT TransId,Dates,Party_Name,Product_Name,Qty,PerProductPrice,Total_Amount FROM Invoice_Detail WHERE TransId = '" + txtInvoice.Text + "'";
                            DataTable dt = clsDataLayer.RetreiveQuery(select);
                            if (dt.Rows.Count > 0)
                            {
                                dataGridView1.DataSource = dt;
                              try
                                {
                                    decimal namt = 0;
                                    for (int i = 0; i < dataGridView1.RowCount; i++)
                                    {
                                        if (dataGridView1.Rows[i].Cells[6].Value != null)
                                        {
                                            namt = namt + decimal.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString());
                                        }
                                    }
                                    this.txtbamount.Text = namt.ToString();

                                    //decimal Bil = Convert.ToDecimal(txtbamount.Text);
                                    //decimal dis = Convert.ToDecimal(txtdiscount.Text);
                                    //decimal net = Bil - dis;
                                    //txtnetamount.Text = net.ToString();

                                }
                                catch
                                {
                                    // string name = ex.Message;
                                }
                            }
                            else
                            {
                                MessageBox.Show("No Record Found!");
                            }

                        }
                        else
                        {
                            MessageBox.Show("Already Added In GridView!");
                        }
                    }
                
                }
                catch { }
              
            } 
        }
        private void txtperprodprice_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int quant = Convert.ToInt32(txtquant.Text);
                int price = Convert.ToInt32(txtperprodprice.Text);
                int final = price * quant;
                txttotal.Text = final.ToString();  
            }
            catch  
            { 
            }
    
        }

    //    private void txtdiscount_TextChanged(object sender, EventArgs e)
    //    {
    //        try { 
    //        decimal Bil = Convert.ToDecimal(txtbamount.Text);
    //        decimal dis = Convert.ToDecimal(txtdiscount.Text);
    //        decimal net = Bil - dis;
    //        txtnetamount.Text = net.ToString();
    //             }catch{}
    //}
        private void txtdiscount_Validated(object sender, EventArgs e)
        {

        }

        private int GetRowIndexs(string target, int columnIndex)
        {
            int index = -1;
            if (columnIndex - 1 < dataGridView1.ColumnCount)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[columnIndex].Value.ToString() == target)
                    {
                        index = i;
                        //txt_late.Text = dataGridView1.Rows[i].Cells[columnIndex].Value.ToString();
                        //txt_half.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        //txt_absent.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        //txtpresnt.Text = dataGridView1.Rows[i].Cells[5].Value.ToString();
                        columnIndex++;
                        txtquant.Text = dataGridView1.Rows[i].Cells[columnIndex].Value.ToString();
                        columnIndex++;
                        txtperprodprice.Text = dataGridView1.Rows[i].Cells[columnIndex].Value.ToString();
                        columnIndex++;
                        txttotal.Text = dataGridView1.Rows[i].Cells[columnIndex].Value.ToString();

                        //txt_index.Text = i.ToString();
                        rowindex = i;

                        break;
                    }

                }
            }
             
            return index;
        }

        private void txtpname_TextChanged(object sender, EventArgs e)
        {
            if (status.Equals("Edit"))
            {
                Search();
            }
           
        }

        public void Search()
        {
            GetRowIndexs(txtpname.Text, 3);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                String search = "select * from Invoice where TransId = '" + dataGridView1.CurrentRow.Cells[0].ToString() + "' ";
                DataTable dt = clsDataLayer.RetreiveQuery(search);
                if (dt.Rows.Count > 0)
                {

                    txtInvoice.Enabled = false;
                    txtInvoice.ReadOnly = true;
                    txtparty.Text = dt.Rows[0]["Party_Name"].ToString();
                    txt_date.Text = dt.Rows[0]["Dates"].ToString();
                    txtbamount.Text = dt.Rows[0]["Bill_Amount"].ToString();
                    txtdiscount.Text = dt.Rows[0]["Discount"].ToString();
                    txtnetamount.Text = dt.Rows[0]["Net_Amount"].ToString();
                    txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();

                    String query = "select TransId,Dates,Party_Name,Product_Name,Qty,PerProductPrice,Total_Amount from Invoice_Detail where TransId = '" + txtInvoice.Text + "'";
                    DataTable dc = clsDataLayer.RetreiveQuery(query);
                    if (dc.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dc;
                        dataGridView1.Enabled = true;
                    }

                    Enable();
                    btnSave.Enabled = false;
                    old = Convert.ToDecimal(txtbamount.Text);
                }
            }
            catch { }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                String search = "select * from Invoice where TransId = '" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "' ";
                DataTable dt = clsDataLayer.RetreiveQuery(search);
                if (dt.Rows.Count > 0)
                {
                    txtInvoice.Enabled = false;
                    txtInvoice.ReadOnly = true;
                    txtInvoice.Text = dt.Rows[0]["TransId"].ToString();
                    txtparty.Text = dt.Rows[0]["Party_Name"].ToString();
                    txt_date.Text = dt.Rows[0]["Dates"].ToString();
                    txtbamount.Text = dt.Rows[0]["Bill_Amount"].ToString();
                    txtdiscount.Text = dt.Rows[0]["Discount"].ToString();
                    txtnetamount.Text = dt.Rows[0]["Net_Amount"].ToString();
                    txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                    txtpname.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    txtquant.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    txtperprodprice.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();

                    String query = "select TransId,Dates,Party_Name,Product_Name,Qty,PerProductPrice,Total_Amount from Invoice_Detail where TransId = '" + txtInvoice.Text + "'";
                    DataTable dc = clsDataLayer.RetreiveQuery(query);
                    if (dc.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dc;
                        dataGridView1.Enabled = true;
                    }
                    String get = "select Tax_Percent from tbl_tax"; DataTable dqw = clsDataLayer.RetreiveQuery(get); if (dqw.Rows.Count > 0) { txtsaletax.Text = dqw.Rows[0][0].ToString(); } else { txtsaletax.Text = "0"; }

                   // Enable();
                    btnSave.Enabled = false;
                    old = Convert.ToDecimal(txtnetamount.Text);
                    
                }
            }
            catch { }
        }

      private void txtsaletax_TextChanged(object sender, EventArgs e)
      {
      try
      {
      decimal Bil = Convert.ToDecimal(txtbamount.Text);
      decimal dis = Convert.ToDecimal(txtdiscount.Text);
      decimal tax = Convert.ToDecimal(txtsaletax.Text);
      decimal net = Bil - dis;
      decimal final = net * tax / 100;
      decimal last = net + final;
      txtnetamount.Text = last.ToString();
      } catch { }
      }

      private void txtdiscount_TextChanged(object sender, EventArgs e)
      {
          try
          {
              decimal Bil = Convert.ToDecimal(txtbamount.Text);
              decimal dis = Convert.ToDecimal(txtdiscount.Text);
              decimal tax = Convert.ToDecimal(txtsaletax.Text);
              decimal net = Bil - dis;
              decimal final = net * tax / 100;
              decimal last = net + final;
              txtnetamount.Text = last.ToString();
          }
          catch { }
      }

      private void txtbamount_TextChanged(object sender, EventArgs e)
      {
          try
          {
              decimal Bil = Convert.ToDecimal(txtbamount.Text);
              decimal dis = Convert.ToDecimal(txtdiscount.Text);
              decimal tax = Convert.ToDecimal(txtsaletax.Text);
              decimal net = Bil - dis;
              decimal final = net * tax / 100;
              decimal last = net + final;
              txtnetamount.Text = last.ToString();
          }
          catch { }
      }

      private void txtdiscount_KeyPress(object sender, KeyPressEventArgs e)
      {
      if (e.KeyChar == (char)Keys.Escape)
      {  this.Close();   }
      else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
      { e.Handled = true;  }
      } 
  

    }
}
