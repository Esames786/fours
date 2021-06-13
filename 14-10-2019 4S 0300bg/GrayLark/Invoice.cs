using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;

using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Printing;
using GrayLark;
using GrayLark.bin.Debug.Report;

namespace GrayLark
{
    public partial class Invoice : Form
    {
        String wg = "";
        DataTable dz; String cb = "";
        int same = 0; String GetComb = "";
        String Code = "";
        String UID = Login.UserID;
        public string GreenSignal = "";

        public string FormID = "";
        decimal old = 0;
        int h = 0;
        String TrcCode = "";
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        DataTable drs = new DataTable();
        TextBox tb = new TextBox();
        String vstatus = "";
        String HeaderAccount = "";


        public Invoice()
        {
            InitializeComponent();
            //or HeaderActCode='1'
            txtsale.Enabled = false;    Disable();  dataGridView1.Enabled = false;   btnNew.Focus();
            txtparty.TabIndex = 1; dte_Date.TabStop = false; dataGridView1.TabIndex = 3; Clears();
            New();   btnFirst.Enabled = true; btnLast.Enabled = true; btnPrevious.Enabled = true; btnNext.Enabled = true; btnSave.Enabled = true;
            txtparty.Focus(); this.KeyPreview = true;
            txtsale.Enabled = false; old = 0; dataGridView1.Enabled = true; dataGridView1.Rows.Clear(); btnSave.Enabled = true;  txtsale.Text = clsGeneral.getMAXCode("tblInvoice", "InCode", "IN"); 
        }

        private DataGridViewCell GetNextCell(DataGridViewCell dataGridViewCell)
        {
            throw new NotImplementedException();
        }

        private DataTable loading()
        {
            try
            {
                String fetch = "select customer_name from tbl_b_sale";
                drs = clsDataLayer.RetreiveQuery(fetch);
                if (drs.Rows.Count > 0)
                {

                }
           
            }
            catch { }
            return drs;
        }

        public void UserNametesting()
        {
            try
            {
                string query = "  select * from user_access where USENAME='" + UID + "' AND FORM_NAME='" + FormID + "'";
                 
                SqlCommand sc = new SqlCommand(query, con);
                con.Open();
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
                con.Close();
                dr.Close();
            }
            catch { }
        }

        private bool CheckAllFields()
        {
            bool flag = false;

            foreach (Control c in this.tableLayoutPanel2.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Text == "")
                    {
                        if (((TextBox)c).Name == "txtscan")
                        {

                        }
                        else {
                            flag = true;
                            break;
                        }
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

            foreach (Control c in this.tableLayoutPanel4.Controls)
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

        private bool CheckDataGridCells(DataGridView dgv)
        {
            bool flag = false;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (Convert.ToString(dgv.Rows[i].Cells[j].Value) == string.Empty)
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

        private void PartyCode(String title)
        {
            try
            {
                String sel = "select ActCode,HeaderActCode from Accounts where ActTitle = '" + title + "'";
                DataTable dc = clsDataLayer.RetreiveQuery(sel);
                if (dc.Rows.Count > 0) { Code = dc.Rows[0][0].ToString(); HeaderAccount = dc.Rows[0][1].ToString(); }
            }
            catch { }
        }

        bool cmv = false;
        public void CheckMinusValue()
        {
            try
            {
                int abc = 0;
                for (int ry = 0; ry < dataGridView1.Rows.Count; ry++)
                {
                    abc = Convert.ToInt32(dataGridView1.Rows[ry].Cells[4].Value);
                    String qs = abc.ToString();
                    if (qs.StartsWith("-"))
                    {
                        cmv = true;
                    }
                }
            }
            catch { }
        }
 
        private void LedgerDone()
        {
            try
            { 
                Posting();
             }
            catch { }
        }

        bool pcs = true;
        private void CheckProd()
        {
            pcs = true;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                String pd = dataGridView1.Rows[i].Cells[0].Value.ToString();
                String Db = "select * from add_product_stock where NAME = '" + pd + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(Db); if (d1.Rows.Count > 0)
                { }
                else { pcs = false; }
            }
        }

        private void lookdone()
        {
            String get4 = "select * from tbl_DeliveryUses where DCode='" + txtscan.Text + "' and uUsedQuantity!=0"; DataTable da4 = clsDataLayer.RetreiveQuery(get4);
            if (da4.Rows.Count > 0)
            {
                //
                dataGridView1.Rows.Clear(); int nvb = 0; bool bon = false; int mob = 0;
                foreach (DataRow dr in da4.Rows)
                {
                    #region foreachloop
                    decimal GusedQuantity = 0; GusedQuantity = Convert.ToDecimal(dr["uUsedQuantity"].ToString());
                    decimal Quant = 0; decimal focQuant = 0;
                    #region abc
                    String f1 = "select uQuantity,ufocQuantity from viewinv where DeliveryCode='" + txtscan.Text + "' and ProductName='" + dr["ProductName"].ToString() + "' and USize='" + dr["USize"].ToString() + "'";
                    DataTable g2 = clsDataLayer.RetreiveQuery(f1);
                    if (g2.Rows.Count > 0)
                    {
                        for (int y = 0; y < g2.Rows.Count; y++)
                        {
                            Quant += Convert.ToDecimal(g2.Rows[y][0].ToString()); focQuant += Convert.ToDecimal(g2.Rows[y][1].ToString());
                        }
                    }
                    else
                    {
                        String f12 = "select uQuantity,ufocQuantity from viewinv2 where DeliveryCode='" + txtscan.Text + "' and ProductName='" + dr["ProductName"].ToString() + "' and USize='" + dr["USize"].ToString() + "'";
                        DataTable g22 = clsDataLayer.RetreiveQuery(f12);
                        if (g22.Rows.Count > 0)
                        {
                            for (int y = 0; y < g2.Rows.Count; y++)
                            {
                                Quant += Convert.ToDecimal(g22.Rows[y][0].ToString()); focQuant += Convert.ToDecimal(g22.Rows[y][1].ToString());
                            }
                        }
                    }
                    #endregion abc

                    decimal qf = Quant + focQuant;
                    decimal final = GusedQuantity - qf;
                    decimal uremain = 0;
                    String pname = dr["ProductName"].ToString(); String psize = dr["USize"].ToString();
                    String m4 = "select  uRemainingQuantity from  tbl_DeliveryUses where ProductName='" + pname + "' and USize='" + psize + "' and DCode='" + txtscan.Text + "'"; DataTable d90 = clsDataLayer.RetreiveQuery(m4);
                    if (d90.Rows.Count > 0)
                    {
                        uremain = Convert.ToDecimal(d90.Rows[0][0].ToString());
                    }
                    if (final != 0)
                    { 
                        mob++;
                    } 
                    #endregion foreachloop
                }

                
                if (mob >0)
                {
                    String up6 = "update tbl_Delivery set vstatus='Delivery' where DCode='" + txtscan.Text + "' "; clsDataLayer.ExecuteQuery(up6);
                   // MessageBox.Show("Delivery Voucher Status Updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Stop); txtscan.Text = "";
                }
            }
        }

        //Save
        private void button8_Click(object sender, EventArgs e)
        {
            try
            { 
                FormID = "Save";
                UserNametesting();
                CheckMinusValue();
                if (GreenSignal == "YES")
                {
                    if (!CheckAllFields() && !CheckDataGridCells(dataGridView1))
                    {
                        if (cmv == false)
                        {
                            String tbname = "";
                            if (checkBox1.Checked)
                            {
                                tbname = "tblnInvoice";
                            }
                            else
                            {
                                tbname = "tblInvoice";
                            }
                            String bs = "select * from  "+tbname+" where InCode='" + txtsale.Text + "'";
                            DataTable rg = clsDataLayer.RetreiveQuery(bs);
                            if (rg.Rows.Count > 0)
                            {
                                MessageBox.Show("Already Saved!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            }
                            else
                            {
                                CheckProd();
                                if (pcs == true)
                                {
                                    SaveButton(); txtsale.Enabled = false; dataGridView1.Enabled = false; btnNew.Enabled = true; btnaddrow.Enabled = false;
                                }
                                else
                                {
                                    MessageBox.Show("Product Name is Incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            //MessageBox.Show("Check Grid View Current Stock Must Be Positive Value.!");
                            //DialogResult dialogResult = MessageBox.Show("Are You Sure You Want to Save Current Stock in Negative Value ?", "Stock Negative Issue", MessageBoxButtons.YesNo);
                            //if (dialogResult == DialogResult.Yes)
                            //{
                            SaveButton(); txtsale.Enabled = false;
                            //}
                            //else if (dialogResult == DialogResult.No)
                            //{
                            //    //do something else
                            //}
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
            catch  {  }
        }


        public void SaveButton()
        {
            try
            {
                String VoucherStatus = "";
                String getname = "select * from Accounts where ActTitle='" + txtparty.Text + "'";
                DataTable de = clsDataLayer.RetreiveQuery(getname);
                if (de.Rows.Count < 1) { MessageBox.Show("UserName dont have a chart Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else
                {
           decimal rp = 0;
           for (int p = 0; p < dataGridView1.Rows.Count; p++)
           {
               rp += Convert.ToDecimal(dataGridView1.Rows[p].Cells[8].Value);
           }txtGrand.Text = rp.ToString();
            #region SaveStartFirst
           
            decimal final = 0; decimal Net = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                String product_name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                decimal price_per_product = Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value.ToString());
                decimal Quantity = Convert.ToDecimal(dataGridView1.Rows[i].Cells[2].Value.ToString());
                decimal Total_price = Convert.ToDecimal(dataGridView1.Rows[i].Cells[8].Value.ToString());

                decimal DiscountAmount = Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value.ToString());
                decimal discountPercent = Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value.ToString());
                decimal Gross = Convert.ToDecimal(dataGridView1.Rows[i].Cells[8].Value.ToString());
                 Net = Gross * discountPercent / 100;
                final = DiscountAmount + Net;
            }
            btnSave.Enabled = false;
            VoucherStatus = "";
                    if (checkBox1.Checked)
                    {
                         String insert4 = "insert into tblnInvoice(ReceiveAmount,RemainingAmount,status,Remarks,InCode,InstituteName,DeliveryCode,InvoiceAmount,InvoiceDate,UserName)values(0," + txtGrand.Text + ",'Invoice','" + txtremarks.Text + "','" + txtsale.Text + "','" + txtparty.Text + "','" + txtscan.Text + "'," + txtGrand.Text + ",'" + dte_Date.Value.ToString("yyyy-MM-dd") + "','" + Login.UserID + "')";

                        if (clsDataLayer.ExecuteQuery(insert4) > 0)
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                            //    String up6 = "update tbl_Delivery set vstatus='Invoice' where DCode='"+txtscan.Text+"' "; clsDataLayer.ExecuteQuery(up6);
                                String lot = "";
                                String n5 = "select UPurchaseOrder from tbl_DeliveryUses where DCode='" + txtscan.Text + "' and ProductName='" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "' and USize='" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'"; DataTable db5 = clsDataLayer.RetreiveQuery(n5);
                                if (db5.Rows.Count > 0)
                                {
                                    lot = db5.Rows[0][0].ToString();
                                }

                                String edate = "";
                                String n52 = "select ExpiryDate from tbl_DeliveryDetail where ProductItem='" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "' and Size='" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "' and PurchaseOrder='"+lot+"' ";
                                DataTable db50 = clsDataLayer.RetreiveQuery(n52);
                                if (db50.Rows.Count > 0)
                                {
                                    edate = db50.Rows[0][0].ToString();
                                }
                                //Stock Update
                                String insert24 = @"insert into tblnInvoiceDetail(UExpdate,InCode,ProductName,USize,UPurchaseOrder,uQuantity,ufocQuantity,price,DiscountPercent,Discount,DiscountPercentValue,SaleTax,total)
                                        values('"+edate+"','" + txtsale.Text + "','" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "','"+lot+"', " + dataGridView1.Rows[i].Cells[2].Value.ToString() + "," + dataGridView1.Rows[i].Cells[3].Value.ToString() + ", " + dataGridView1.Rows[i].Cells[4].Value.ToString() + ",  " + dataGridView1.Rows[i].Cells[5].Value.ToString() + ", " + dataGridView1.Rows[i].Cells[6].Value.ToString() + ", " + Net + ", " + dataGridView1.Rows[i].Cells[7].Value.ToString() + ", " + dataGridView1.Rows[i].Cells[8].Value.ToString() + ")";
                                if (clsDataLayer.ExecuteQuery(insert24) > 0) { }
                            }
                        }
                    }
                    else {
                        txtsale.Text = clsGeneral.getMAXCode("tblInvoice", "InCode", "IN");

      String insert = "insert into tblInvoice(ReceiveAmount,RemainingAmount,status,Remarks,InCode,InstituteName,DeliveryCode,InvoiceAmount,InvoiceDate,UserName)values(0,"+txtGrand.Text+",'Invoice','" + txtremarks.Text + "','" + txtsale.Text + "','" + txtparty.Text + "','" + txtscan.Text + "'," + txtGrand.Text + ",'" + dte_Date.Value.ToString("yyyy-MM-dd") + "','" + Login.UserID + "')";
                        if (clsDataLayer.ExecuteQuery(insert) > 0)
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                //Stock Update
                                String lot = "";
                                String n5 = "select UPurchaseOrder from tbl_DeliveryUses where DCode='" + txtscan.Text + "' and ProductName='" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "' and USize='" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'"; DataTable db5 = clsDataLayer.RetreiveQuery(n5);
              if (db5.Rows.Count > 0)
              {
                  lot = db5.Rows[0][0].ToString();
              }
              String edate = "";
              String n52 = "select ExpiryDate from tbl_DeliveryDetail where ProductItem='" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "' and Size='" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "' and PurchaseOrder='" + lot + "' ";
              DataTable db50 = clsDataLayer.RetreiveQuery(n52);
              if (db50.Rows.Count > 0)
              {
                  edate = db50.Rows[0][0].ToString();
              }
              String insert2 = @"insert into tblInvoiceDetail(UExpdate,InCode,ProductName,USize,UPurchaseOrder,uQuantity,ufocQuantity,price,DiscountPercent,Discount,DiscountPercentValue,SaleTax,total)
                     values('"+edate+"','" + txtsale.Text + "','" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "','"+lot+"', " + dataGridView1.Rows[i].Cells[2].Value.ToString() + "," + dataGridView1.Rows[i].Cells[3].Value.ToString() + ", " + dataGridView1.Rows[i].Cells[4].Value.ToString() + ",  " + dataGridView1.Rows[i].Cells[5].Value.ToString() + ", " + dataGridView1.Rows[i].Cells[6].Value.ToString() + ", " + Net + ", " + dataGridView1.Rows[i].Cells[7].Value.ToString() + ", " + dataGridView1.Rows[i].Cells[8].Value.ToString() + ")";
             if (clsDataLayer.ExecuteQuery(insert2) > 0) { }
                            }
                        }
                    }
            LedgerDone();
            MessageBox.Show("Record Save Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
          //  DoPrint();
            Disable(); Disable1(); dataGridView1.Enabled = false;
            btnSave.Enabled = false; btnNew.Enabled = true; btnEdit.Enabled = true; btnUpdate.Enabled = false;  
            #endregion SaveStartFirst
                 }
            }
            catch { }
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
                    decimal final = Convert.ToDecimal(txtGrand.Text);
                    total =total-old+final;
                    due =due-old+final;
                    string updateblnc = "update ReceiveDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                    clsDataLayer.ExecuteQuery(updateblnc);
                }
                else
                {
                    decimal b = Convert.ToDecimal(txtGrand.Text);
                    String ii = @"insert into ReceiveDue (RemCode,PartyName,PartyCode,TotalAmount,DueAmount,ReceivedAmount,Company_Name)
                values('" + txtsale.Text + "','" + txtparty.Text + "','" + Code + "'," + b + "," + b + ",0,'Delizia')";
                    clsDataLayer.ExecuteQuery(ii);
                }
            }
            catch { }
        }
          
        private bool Enable2()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel2.Controls)
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

        private bool Enable()
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
            foreach (Control c in this.tableLayoutPanel4.Controls)
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
                else if (c is MaskedTextBox)
                {
                    if (((MaskedTextBox)c).Enabled == true)
                    {
                        ((MaskedTextBox)c).Enabled = false;
                        flag = true;

                    }
                }
            }
            return flag;
        }
        private bool Disable1()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel2.Controls)
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
                else if (c is MaskedTextBox)
                {
                    if (((MaskedTextBox)c).Enabled == true)
                    {
                        ((MaskedTextBox)c).Enabled = false;
                        flag = true;

                    }
                }
            }
            return flag;
        }

        private void Clears()
        {
            foreach (Control c in this.tableLayoutPanel2.Controls)
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

        private void New()
        {
            try
            {
                FormID = "Add";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                clsGeneral.SetAutoCompleteTextBox(txtparty, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 14 and HeaderActCode='10101' Order BY ID DESC"));
 
                Enable();   Enable2();     Empty();
                dataGridView1.Enabled = true;
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                btnNew.Enabled = false;
                btnUpdate.Enabled = false;
                //dataGridView1.Rows.Clear(); 
                //dataGridView1.Rows.Add();
                Clears();  
                txtsale.Text = clsGeneral.getMAXCode("tblInvoice", "InCode", "IN"); 
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            New(); txtsale.Enabled = false; old = 0; dataGridView1.Enabled = true; dataGridView1.Rows.Clear(); btnSave.Enabled = true; Clears(); txtsale.Text = clsGeneral.getMAXCode("tblInvoice", "InCode", "IN"); btnaddrow.Enabled = true; checkBox1.Enabled = true;
            checkBox1.Checked = false;
        }

        private void Empty()
        {
            txtGrand.Clear();
            txtparty.Clear();
            txtparty.Text = "Select";

        }

       
        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                FormID = "Update";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    if (!CheckAllFields() && !CheckDataGridCells(dataGridView1))
                    {
                        try
                        {
                            CheckProd();
                            if (pcs == true)
                            { 
                            decimal final = 0; decimal Net = 0;
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                decimal DiscountAmount = Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value.ToString());
                                decimal discountPercent = Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value.ToString());
                                decimal Gross = Convert.ToDecimal(dataGridView1.Rows[i].Cells[8].Value.ToString());
                                 Net = Gross * discountPercent / 100;
                                final = DiscountAmount + Net;
                            }
                        
                            String VoucherStatus = "";
                            PartyCode(txtparty.Text);

                            //ReceiveDue();
                            VoucherStatus = "";
                                //
                       //     String up6 = "update tbl_Delivery set vstatus='Invoice' where DCode='" + txtscan.Text + "' "; clsDataLayer.ExecuteQuery(up6);

                            String m03 = "select * from tblnInvoice where InCode = '" + GetComb + "'"; DataTable dh = clsDataLayer.RetreiveQuery(m03);
                            if (dh.Rows.Count > 0)
                            {
                                String fet5 = "delete from tblnInvoice Where InCode = '" + GetComb + "'";
                                clsDataLayer.ExecuteQuery(fet5);
                                String fet52 = "delete from tblnInvoiceDetail Where InCode = '" + GetComb + "'";
                                clsDataLayer.ExecuteQuery(fet52);
                                if (checkBox1.Checked) { millegal(Net); } else { Mlegal(Net); }
                            }
                            else
                            {
                                String fet5 = "delete from tblInvoice Where InCode = '" + GetComb + "'";
                                clsDataLayer.ExecuteQuery(fet5);
                                String fet52 = "delete from tblInvoiceDetail Where InCode = '" + GetComb + "'";
                                clsDataLayer.ExecuteQuery(fet52);
                                if (checkBox1.Checked) { millegal(Net); } else { Mlegal(Net); }
                            }
                       

//legal 
                                        //
                                 VoucherStatus = "OnCredit";
                                 lookdone();
                                ReceiveDue();
                                int index = GetRowIndex(txtsale.Text);
                                DataTable dt = SetDT();
                                ShowDt(dt);
                                DeleteRecord();
                                dt.Rows[index][7] = Convert.ToDecimal(txtGrand.Text);
                                dt.Rows[index + 1][8] = Convert.ToDecimal(txtGrand.Text);
                                dt = SomeOperation(dt);
                                InsertRecord(dt);
                                MessageBox.Show("Voucher Updated Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //DoPrint();
                                dataGridView1.Rows.Clear(); btnSave.Enabled = false; btnEdit.Enabled = true;
                                btnNew.Enabled = true; btnUpdate.Enabled = false;
                                dataGridView1.Enabled = false; btnaddrow.Enabled = false;
                            
                            }
                            else
                            {
                                MessageBox.Show("Product Name is Incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch { }
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

        private void Mlegal(decimal Net)
        {
            #region legal
            string query09 = "";
            query09 = "Delete From tblInvoiceDetail Where InCode = '" + txtsale.Text + "' Delete From tblInvoice Where InCode = '" + txtsale.Text + "'";
            clsDataLayer.ExecuteQuery(query09);
            String insert = "insert into tblInvoice(ReceiveAmount,RemainingAmount,status,Remarks,InCode,InstituteName,DeliveryCode,InvoiceAmount,InvoiceDate,UserName)values(0," + txtGrand.Text + ",'Invoice','" + txtremarks.Text + "','" + txtsale.Text + "','" + txtparty.Text + "','" + txtscan.Text + "'," + txtGrand.Text + ",'" + dte_Date.Value.ToString("yyyy-MM-dd") + "','" + Login.UserID + "')";
            if (clsDataLayer.ExecuteQuery(insert) > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    String lot = "";
                    String n5 = "select UPurchaseOrder from tbl_DeliveryUses where DCode='" + txtscan.Text + "' and ProductName='" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "' and USize='" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'"; DataTable db5 = clsDataLayer.RetreiveQuery(n5);
                    if (db5.Rows.Count > 0)
                    {
                        lot = db5.Rows[0][0].ToString();
                    }
                    String edate = "";
                    String n52 = "select ExpiryDate from tbl_DeliveryDetail where ProductItem='" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "' and Size='" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "' and PurchaseOrder='" + lot + "' ";
                    DataTable db50 = clsDataLayer.RetreiveQuery(n52);
                    if (db50.Rows.Count > 0)
                    {
                        edate = db50.Rows[0][0].ToString();
                    }
                    //Stock Update
                    String insert2 = @"insert into tblInvoiceDetail(UExpdate,InCode,ProductName,USize,UPurchaseOrder,uQuantity,ufocQuantity,price,DiscountPercent,Discount,DiscountPercentValue,SaleTax,total)
                                        values('" + edate + "','" + txtsale.Text + "','" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "','" + lot + "', " + dataGridView1.Rows[i].Cells[2].Value.ToString() + "," + dataGridView1.Rows[i].Cells[3].Value.ToString() + ", " + dataGridView1.Rows[i].Cells[4].Value.ToString() + ",  " + dataGridView1.Rows[i].Cells[5].Value.ToString() + ", " + dataGridView1.Rows[i].Cells[6].Value.ToString() + ", " + Net + ", " + dataGridView1.Rows[i].Cells[7].Value.ToString() + ", " + dataGridView1.Rows[i].Cells[8].Value.ToString() + ")";
                    if (clsDataLayer.ExecuteQuery(insert2) > 0) { }
                }
            }
            #endregion legal
        }

        private void millegal(decimal Net)
        { 
                #region illegal
                string query = "";
                query = "Delete From tblnInvoiceDetail Where InCode = '" + txtsale.Text + "' Delete From tblnInvoice Where InCode = '" + txtsale.Text + "'";
                clsDataLayer.ExecuteQuery(query);
                String insert4 = "insert into tblnInvoice(ReceiveAmount,RemainingAmount,status,Remarks,InCode,InstituteName,DeliveryCode,InvoiceAmount,InvoiceDate,UserName)values(0," + txtGrand.Text + ",'Invoice','" + txtremarks.Text + "','" + txtsale.Text + "','" + txtparty.Text + "','" + txtscan.Text + "'," + txtGrand.Text + ",'" + dte_Date.Value.ToString("yyyy-MM-dd") + "','" + Login.UserID + "')";
                if (clsDataLayer.ExecuteQuery(insert4) > 0)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        String lot = "";
                        String n5 = "select UPurchaseOrder from tbl_DeliveryUses where DCode='" + txtscan.Text + "' and ProductName='" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "' and USize='" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'"; DataTable db5 = clsDataLayer.RetreiveQuery(n5);
                        if (db5.Rows.Count > 0)
                        {
                            lot = db5.Rows[0][0].ToString();
                        }
                        String edate = "";
                        String n52 = "select ExpiryDate from tbl_DeliveryDetail where ProductItem='" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "' and Size='" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "' and PurchaseOrder='" + lot + "' ";
                        DataTable db50 = clsDataLayer.RetreiveQuery(n52);
                        if (db50.Rows.Count > 0)
                        {
                            edate = db50.Rows[0][0].ToString();
                        }
                        //Stock Update
                        String insert24 = @"insert into tblnInvoiceDetail(UExpdate,InCode,ProductName,USize,UPurchaseOrder,uQuantity,ufocQuantity,price,DiscountPercent,Discount,DiscountPercentValue,SaleTax,total)
                values('" + edate + "','" + txtsale.Text + "','" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "','" + lot + "', " + dataGridView1.Rows[i].Cells[2].Value.ToString() + "," + dataGridView1.Rows[i].Cells[3].Value.ToString() + ", " + dataGridView1.Rows[i].Cells[4].Value.ToString() + ",  " + dataGridView1.Rows[i].Cells[5].Value.ToString() + ", " + dataGridView1.Rows[i].Cells[6].Value.ToString() + ", " + Net + ", " + dataGridView1.Rows[i].Cells[7].Value.ToString() + ", " + dataGridView1.Rows[i].Cells[8].Value.ToString() + ")";
                        if (clsDataLayer.ExecuteQuery(insert24) > 0) { }
                    }
                }
                #endregion illegal
             
        }


        private void btnexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            try
            {
                FormID = "Edit";
                UserNametesting();
                if (GreenSignal == "YES")
                {   
                 btnSave.Enabled = false;
                 btnEdit.Enabled = false;
                 btnNew.Enabled = false;
                 btnUpdate.Enabled = true;
                 Enable();
                 Enable2(); btnaddrow.Enabled = true;
                 dataGridView1.Enabled = true; old = Convert.ToDecimal(txtGrand.Text); checkBox1.Enabled = false; GetComb = txtsale.Text; txtsale.Enabled = false;
                 }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                //txtparty.Enabled = false;
            }
            catch { }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
        DoPrint();
         }


        private void DoPrint()
        {
            try
            {
                FormID = "Print";
                UserNametesting();String bt = "";
                if (GreenSignal == "YES")
                {
                    bt = "viewinv";
                    String q = "select * from viewinv where InCode = '" + txtsale.Text + "' ";
                    DataTable dt = clsDataLayer.RetreiveQuery(q);
                    if (dt.Rows.Count > 0)
                    {
                      GrayLark.bin.Debug.Report.rptinvoice rpt = new rptinvoice();
                        rpt.SetDataSource(dt);
                        
                         Sale_Preview frm = new Sale_Preview(rpt);
                         frm.Show();
                    }
                    else
                    {
                        bt = "viewinv2";
                        String q3 = "select * from viewinv2 where InCode = '" + txtsale.Text + "' ";
                        DataTable dt3 = clsDataLayer.RetreiveQuery(q3);
                        if (dt3.Rows.Count > 0)
                        {
                            GrayLark.bin.Debug.Report.rptinvoice rpt = new rptinvoice();
                            rpt.SetDataSource(dt3);

                            rptInvoiceview frm = new rptInvoiceview(rpt,txtsale.Text,bt);
                            frm.Show();
                        }
                        else
                        {
                            MessageBox.Show("No Record Found!", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }             
            }
            catch { }
        }

         
        //CustomerName
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }

            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
        }

        //Discount
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);

        }

        private void txtparty_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
        }

        //SenderName
        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
        }
         
        private void gridrefresh()
        {
            try
            {
                //for (int i = 0; i < dataGridView1.Rows.Count; i++)
                //{
                    //decimal st = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[2].Value);
                    //decimal qu = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[3].Value);
                  
                        decimal Quantity = 0;
                        decimal Stock = 0;
                        if (dataGridView1.CurrentRow.Cells[2].Value != null && dataGridView1.CurrentRow.Cells[2].Value.ToString().Trim() != null)
                        {
                            Quantity = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[2].Value);
                        }
                     
                        // Calculation for Price
                        decimal Price = 0;

                        if (dataGridView1.CurrentRow.Cells[4].Value != null && dataGridView1.CurrentRow.Cells[4].Value.ToString().Trim() != null)
                        {
                            Price = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[4].Value);
                        }
                decimal New_Price = 0;
                        if (dataGridView1.CurrentCell.ColumnIndex != 0)
                        {
                             New_Price = Quantity * Price;
                    ////dataGridView1.CurrentRow.Cells["TotalAmount"].Value = New_Price.ToString();
                    //decimal stax3 = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[7].Value);
                    //New_Price += stax3;
                    //dataGridView1.CurrentRow.Cells["NAmount"].Value = New_Price.ToString();
                   
                        }
                //Discount Amount
                New_Price = Quantity * Price;

                decimal totamount = New_Price;
                        decimal disamount = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["DiscAmount"].Value);

                decimal stax = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[7].Value);
                decimal f = 0;

                        //Discount Percentage
                        decimal dispercent = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["DiscPer"].Value);

                        if (dataGridView1.CurrentCell.ColumnIndex != 5 && dataGridView1.CurrentCell.ColumnIndex != 6)
                        {
                            if (!dataGridView1.CurrentRow.Cells[5].Value.Equals("0"))
                            {
                                decimal fsol = totamount * dispercent / 100;
                                f = totamount - fsol;
                                dataGridView1.CurrentRow.Cells["DiscAmount"].Value = "0"; f += stax;
                                dataGridView1.CurrentRow.Cells["NAmount"].Value = f.ToString();
                            }
                            else if (!dataGridView1.CurrentRow.Cells[6].Value.Equals("0"))
                            {
                                f = totamount - disamount;
                                dataGridView1.CurrentRow.Cells["DiscPer"].Value = "0";
                                f += stax;
                                dataGridView1.CurrentRow.Cells["NAmount"].Value = f.ToString();
                            }
                        }
                        else
                        {
                             if (dataGridView1.CurrentCell.ColumnIndex == 5)
                             {
                                 decimal fsol = totamount * dispercent / 100;
                                 f = totamount - fsol;
                                 dataGridView1.CurrentRow.Cells["DiscAmount"].Value = "0"; f += stax;
                                 dataGridView1.CurrentRow.Cells["NAmount"].Value = f.ToString();
                             }
                             else if (dataGridView1.CurrentCell.ColumnIndex == 6)
                             {
                                 f = totamount - disamount;
                                 dataGridView1.CurrentRow.Cells["DiscPer"].Value = "0";
                                 f += stax;
                                 dataGridView1.CurrentRow.Cells["NAmount"].Value = f.ToString();
                             }


                        }
             //   }
            }
            catch
            {
                //MessageBox.Show(ex.Message);
            }
            try
            {
                decimal namt = 0;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[8].Value != null)
                    {
                        namt = namt + decimal.Parse(dataGridView1.Rows[i].Cells[8].Value.ToString());
                    }
                }
                this.txtGrand.Text = namt.ToString();
            }
            catch
            {
                // string name = ex.Message;
            }
        }

        private void plustotal()
        {
            try
            {
                decimal namt3 = 0;
                decimal namt = 0;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[2].Value != null)
                    { 
                        namt3 = namt3 + decimal.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());

                    }

                    if (dataGridView1.Rows[i].Cells[8].Value != null)
                    {
                        namt = namt + decimal.Parse(dataGridView1.Rows[i].Cells[8].Value.ToString());
                    }
                }
                label6.Text = namt3.ToString();

               
                this.txtGrand.Text = namt.ToString();
            }
            catch
            {
                // string name = ex.Message;
            }
        }

        private void grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                e.Control.KeyPress -= new KeyPressEventHandler(gridColumns_KeyPress);
                if (dataGridView1.CurrentCell.ColumnIndex <= 3) //Desired Column
                {
                    TextBox tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        tb.KeyPress += new KeyPressEventHandler(gridColumns_KeyPress);
                    }
                }

            }
            catch { }
        }

        private void gridColumns_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.'))
                { e.Handled = true; }
                TextBox txtDecimal = sender as TextBox;
                if (e.KeyChar == '.' && txtDecimal.Text.Contains("."))
                {
                    e.Handled = true;
                }
            }
            catch { }
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
            catch { }       return balance; 
        }

        private void SaleProduct_Load(object sender, EventArgs e) { }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell.ColumnIndex == 1)
                {
                    String inprod = dataGridView1.CurrentRow.Cells[0].Value.ToString(); String size = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    String s1 = "select Size from add_product_stock where Size='" + size + "'"; DataTable ds1 = clsDataLayer.RetreiveQuery(s1);
                    if (ds1.Rows.Count > 0)
                    {
                        //for (int a = 0; a < dataGridView1.Rows.Count - 1; a++)
                        //{
                        //    String gprod = dataGridView1.Rows[a].Cells[0].Value.ToString(); String gsize = dataGridView1.Rows[a].Cells[1].Value.ToString();
                        //    if (inprod.Equals(gprod) && gsize.Equals(size))
                        //    {
                        //        MessageBox.Show("Change Product Same Product Cant Add Multiple Times!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); dataGridView1.CurrentRow.Cells[1].Value = "";
                        //    }
                        //}

                        //
                        int nn = 0;
                        for (int a = 0; a < dataGridView1.Rows.Count; a++)
                        {
             String Gpname = dataGridView1.Rows[a].Cells[0].Value.ToString(); 
                            String Gpsize = dataGridView1.Rows[a].Cells[1].Value.ToString();
                            if (Gpname.Equals("") || Gpsize.Equals(""))
                            {

                            }
                            else if (inprod.Equals(Gpname) && size.Equals(Gpsize))
                            {
                                nn++;
                            }
                        }
                        if (nn > 1)
                        {
          MessageBox.Show("Same Product Cant Add Multiple Time", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);  
                        }
                        //
                    }
                    else
                    {
                        MessageBox.Show("Selected Size not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); dataGridView1.CurrentRow.Cells[1].Value = "";
                    }
                }
                else if (dataGridView1.CurrentCell.ColumnIndex == 0)
                {
                    try
                    {
                        #region ColumnIndex2
                        String pname = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        String s1 = "select NAME from add_product_stock where NAME='" + pname + "'"; DataTable ds1 = clsDataLayer.RetreiveQuery(s1);
                        if (ds1.Rows.Count > 0)
                        {
                        }
                        else
                        {
                            MessageBox.Show("Selected Product Name is not Found!!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dataGridView1.CurrentRow.Cells[0].Value = "";
                        }
                        #endregion ColumnIndex2
                    }
                    catch { }
                }
                else if (dataGridView1.CurrentCell.ColumnIndex == 2 || dataGridView1.CurrentCell.ColumnIndex == 4)
                {
                    try
                    {
                        String ng3 = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                        if (ng3.Equals("0")) { dataGridView1.CurrentRow.Cells[2].Value = "1"; }
                        else
                        {
                            QuantVal();
                            decimal namt = 0;
                            for (int i = 0; i < dataGridView1.RowCount; i++)
                            {
                                if (dataGridView1.Rows[i].Cells[2].Value != null)
                                {
                                    String ng = dataGridView1.Rows[i].Cells[2].Value.ToString();
                                    if (ng.Equals(""))
                                    { }
                                    else
                                    {
                                   namt = namt + Convert.ToDecimal(dataGridView1.Rows[i].Cells[2].Value.ToString());
                                    }
                                }
                            }
                            label6.Text = namt.ToString();
                            decimal isq = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[2].Value);
                            decimal upr = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[4].Value);
                            decimal fpc = isq * upr; dataGridView1.CurrentRow.Cells[8].Value = fpc.ToString();
                            plustotal();
                        }
                    }
                    catch (Exception ex)
                    {
                        string name = ex.Message;
                    }
                }
                //
                else if (dataGridView1.CurrentCell.ColumnIndex == 3)
                {
                    QuantVal(); plustotal();
                }
                else { gridrefresh(); }
                //if (dataGridView1.CurrentCell.ColumnIndex == 0)
                //{
                //    SearchProduct(dataGridView1.CurrentRow.Cells[0].Value.ToString(), "grid");
                //}
                //else { gridrefresh(); }
            }
            catch { }
            }

        private void QuantVal()
        {
        String get = "select uUsedQuantity from tbl_DeliveryUses where DCode='" + txtscan.Text + "' and  uUsedQuantity!=0 and ProductName='" + dataGridView1.CurrentRow.Cells[0].Value + "' and USize='" + dataGridView1.CurrentRow.Cells[1].Value + "'"; DataTable da = clsDataLayer.RetreiveQuery(get);
        if (da.Rows.Count > 0)
        {
        int GusedQuantity = 0; GusedQuantity = Convert.ToInt32(da.Rows[0]["uUsedQuantity"]); decimal Quant = 0; decimal focQuant = 0;
        decimal OQuant = 0; decimal OfocQuant = 0;
       
        String ics = "";
        String m3 = "select InCode from tblnInvoice where DeliveryCode='" + txtscan.Text + "'"; DataTable d9 = clsDataLayer.RetreiveQuery(m3);
        if (d9.Rows.Count > 0)
        {
            for (int u = 0; u < d9.Rows.Count; u++)
            {
                ics = d9.Rows[u][0].ToString();
                String f12 = "select uQuantity,ufocQuantity from tblnInvoiceDetail where InCode='" + ics + "' and ProductName='" + dataGridView1.CurrentRow.Cells[0].Value + "' and USize='" + dataGridView1.CurrentRow.Cells[1].Value + "'";
                DataTable g22 = clsDataLayer.RetreiveQuery(f12);
                if (g22.Rows.Count > 0)
                {
                    Quant += Convert.ToDecimal(g22.Rows[0][0].ToString()); focQuant += Convert.ToDecimal(g22.Rows[0][1].ToString());

                    if (FormID.Equals("Edit"))
                    {
                        if (ics.Equals(txtsale.Text))
                        {
                            OQuant = Convert.ToDecimal(g22.Rows[0][0].ToString()); OfocQuant = Convert.ToDecimal(g22.Rows[0][1].ToString());
                        }
                    }
                }
            }
        }  
                    String ics4 = "";
                    String m30 = "select InCode from tblInvoice where DeliveryCode='"+txtscan.Text+"'"; DataTable d90 = clsDataLayer.RetreiveQuery(m30);
                    if (d90.Rows.Count > 0)
                    {
                        for (int u = 0; u < d90.Rows.Count; u++)
                        {
                            ics4 = d90.Rows[u][0].ToString();
                            String f1 = "select uQuantity,ufocQuantity from tblInvoiceDetail where InCode='" + ics4 + "' and ProductName='" + dataGridView1.CurrentRow.Cells[0].Value + "' and USize='" + dataGridView1.CurrentRow.Cells[1].Value + "'";
                            DataTable g2 = clsDataLayer.RetreiveQuery(f1);
                            if (g2.Rows.Count > 0)
                            {
                                Quant += Convert.ToDecimal(g2.Rows[0][0].ToString()); focQuant += Convert.ToDecimal(g2.Rows[0][1].ToString());
                                if (FormID.Equals("Edit"))
                                {
                                    if (ics4.Equals(txtsale.Text))
                                    {
                                        OQuant = Convert.ToDecimal(g2.Rows[0][0].ToString()); OfocQuant = Convert.ToDecimal(g2.Rows[0][1].ToString());
                                    }
                                }
                            }
                        }
                    }
                
        if (FormID.Equals("Edit"))
        {
            Quant -= OQuant; focQuant -= OfocQuant;
        }
                decimal qf = Quant + focQuant;
                decimal final = GusedQuantity - qf;
                decimal input = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[2].Value); decimal input1 = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[3].Value);
                decimal totalinput = input + input1;
                decimal lmao = final - totalinput;
                String Conv = lmao.ToString();
                if (Conv.StartsWith("-"))
                {
                    if (dataGridView1.CurrentCell.ColumnIndex == 2)
                    {
                        final -= input1;
                        if (lmao <= 0)
                        {
                            dataGridView1.CurrentRow.Cells[2].Value = "";
                        }
                        else if (lmao > 0)
                        {
                     dataGridView1.CurrentRow.Cells[2].Value = "1";
                        }
                    }
                    else if (dataGridView1.CurrentCell.ColumnIndex == 3)
                    {
                        final -= input;
                        if (lmao <= 0)
                        {
                            dataGridView1.CurrentRow.Cells[3].Value = "";
                        }
                        else if (lmao > 0)
                        {
                            dataGridView1.CurrentRow.Cells[3].Value = "1";
                        }
                    }
                   
                    MessageBox.Show("Available Quantity : " + final, "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void SearchProduct(String ProdName,String place)
        { 
          #region search
                AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
                tb.AutoCompleteCustomSource = acsc;
                tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                tb.AutoCompleteSource = AutoCompleteSource.CustomSource;

                bool h = true; int rsame = 0;
                if (same == 1)
                {
                    rsame = 1;
                }
                else if (same == 0)
                {
                    rsame = 0;
                }
                if (dataGridView1.Rows.Count > 1 && same != 1)
                {
                    for (int r = 0; r < dataGridView1.Rows.Count; r++)
                    {
                        if (h == false)
                        {

                        }
                        else
                        {
                            if (dataGridView1.Rows[r].Cells[0].Value.Equals(ProdName))
                            {
                                int p = dataGridView1.CurrentCell.RowIndex;
                                if (p == r)
                                {
                                    h = true;
                                }
                                else
                                {
                                    h = false;
                                }
                            }
                            else
                            {
                                h = true;
                            }
                        }
                    }
                }
                else { same = 0; }
                if (h == true)
                {
                    String abc = "";
                    if (place.Equals("grid"))
                    {
                        abc = "SProduct";
                    }
                    else
                    {
                        abc = "TCS_CODE";
                    }
                    String hk = "select SQuantity,SellPrice from tbl_ColdStorageMaintain where "+abc+"='" + ProdName + "'";
                    DataTable dqw = clsDataLayer.RetreiveQuery(hk);
                    if (dqw.Rows.Count > 0)
                    {
                        if (rsame == 0)
                        {
                            dataGridView1.CurrentRow.Cells[2].Value = dqw.Rows[0][1].ToString();
                        }
                        else if (rsame == 1)
                        {
                        }
                        dataGridView1.CurrentRow.Cells[3].Value = dqw.Rows[0][0].ToString();

                        dataGridView1.CurrentRow.Cells[7].Value = "0"; dataGridView1.CurrentRow.Cells[6].Value = "0";
                    }
                }
                else
                {
                    MessageBox.Show(" Change Product Same Product Cant Add Multiple Times!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                #endregion search 
        }




        private void Searching(object sender, EventArgs e)
        {
            try
            {
                String query = "select * from SaleBill where BillCode = '" + txtsale.Text + "' ";
                DataTable d = clsDataLayer.RetreiveQuery(query);
                if (d.Rows.Count > 0)
                {
                    txtsale.Enabled = false;
                    txtsale.ReadOnly = true;
                    txtparty.Text = d.Rows[0]["PartyName"].ToString();
                    txtGrand.Text = d.Rows[0]["BillPrice"].ToString(); 
                    //                  txtPayment.Text = d.Rows[0]["PaymentMode"].ToString();
                    String q1 = "select SenderName from SaleBy where BillCode = '" + txtsale.Text + "' ";
                    DataTable dd = clsDataLayer.RetreiveQuery(q1);
                    if (dd.Rows.Count > 0)
                    {
                        txtparty.Text = dd.Rows[0][0].ToString();
                        cb = txtparty.Text;
                    }
                    //
                    dataGridView1.Rows.Clear();
                    String q2 = "select * from SaleBillDetail where BillCode = '" + txtsale.Text + "' ";
                    DataTable d2 = clsDataLayer.RetreiveQuery(q2);

                    foreach (DataRow item in d2.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = item["SaleItem"].ToString();
                        String sb = item["SaleItem"].ToString();

                        String quer = "select SQuantity from tbl_ColdStorageMaintain where SProduct='" + sb + "'";
                        DataTable df = clsDataLayer.RetreiveQuery(quer);
                        int quant = 0;
                        if (df.Rows.Count > 0)
                        {
                            quant = Convert.ToInt32(df.Rows[0][0].ToString());
                        }
                        dataGridView1.Rows[n].Cells[3].Value = item["QUANTITY"].ToString();
                        dataGridView1.Rows[n].Cells[2].Value = item["PricePerItem"].ToString();
                        dataGridView1.Rows[n].Cells[5].Value = item["TotalPriceItem"].ToString();
                        dataGridView1.Rows[n].Cells[1].Value = quant;
                        dataGridView1.Rows[n].Cells[4].Value = quant - Convert.ToInt32(item["QUANTITY"].ToString());
                    }
                    //
                    try
                    {

                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            int Quantity = 0;
                            int Stock = 0;

                            // Calculation for Stock
                            if (dataGridView1.CurrentRow.Cells["Quantity"].Value != null && dataGridView1.CurrentRow.Cells["Quantity"].Value.ToString().Trim() != null)
                            {
                                Quantity = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Quantity"].Value);
                            }
                            if (dataGridView1.CurrentRow.Cells["Stock"].Value != null && dataGridView1.CurrentRow.Cells["Stock"].Value.ToString().Trim() != null)
                            {
                                Stock = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Stock"].Value);
                            }
                            int Now = Stock - Quantity;
                            dataGridView1.CurrentRow.Cells["CurrentStock"].Value = Now.ToString();
                        }
                        dz = new DataTable();
                        dz.Columns.Add("Name");
                        dz.Columns.Add("Quantity");
                        for (int h = 0; h < dataGridView1.Rows.Count; h++)
                        {
                            dz.Rows.Add(new string[] { dataGridView1.Rows[h].Cells[0].Value.ToString(), dataGridView1.Rows[h].Cells[3].Value.ToString() });
                        }
                    }
                    catch { }
                    this.dataGridView1.Focus();
                    dataGridView1.Enabled = true;
                }
                Enable();
                Enable2();
                //  old = Convert.ToDecimal(txtnetamount.Text);
            }
            catch
            {
            }
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
                System.Data.SqlClient.SqlDataAdapter cmd = new SqlDataAdapter("select V_No, Datetime, AccountCode, RefCode, PaymentType, InvoiceNo, Particulars, Debit, Credit, Party_Balance,Company_Name,Description from LedgerReceived where RefCode = '" + Code + "' order by ID asc", con);
                cmd.Fill(dt);  con.Close();
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
                    SqlCommand cmd = new SqlCommand("INSERT INTO LedgerReceived VALUES('" + dt.Rows[i].ItemArray[0].ToString() + "','" + dt.Rows[i].ItemArray[1].ToString() + "','" + dt.Rows[i].ItemArray[2].ToString() + "','" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','" + dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "'," + dt.Rows[i].ItemArray[7].ToString() + "," + dt.Rows[i].ItemArray[8].ToString() + "," + dt.Rows[i].ItemArray[9].ToString() + ",'" + dt.Rows[i].ItemArray[10].ToString() + "','" + dt.Rows[i].ItemArray[11].ToString() + "')", con);
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


        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                wg = "search";
                this.Close();
                View_All_Invoices ror = new View_All_Invoices("ProductSale");
                ror.Show();
            }
            catch { }
        }

        private void txtsale_TextChanged(object sender, EventArgs e)
        { try
            {
                String tbl1 = ""; String tbl2 = "";
            //
        String m3 = "select * from tblInvoice where InCode = '" + txtsale.Text + "' ";
        DataTable d5 = clsDataLayer.RetreiveQuery(m3);
        if (d5.Rows.Count > 0)
                {
                    tbl1 = "tblInvoice"; tbl2 = "tblInvoiceDetail";   txtsale.Enabled = false;
                }
        else
        {
            tbl1 = "tblnInvoice"; tbl2 = "tblnInvoiceDetail";  txtsale.Enabled = true;
        }
            
                //tblInvoiceDetail(InCode,ProductName,USize,UPurchaseOrder,uQuantity,ufocQuantity,price,DiscountPercent,Discount,DiscountPercentValue,SaleTax,total)
                String g5 = "select * from "+tbl2+" where InCode='"+txtsale.Text+"'"; DataTable d6 = clsDataLayer.RetreiveQuery(g5);
                if(d6.Rows.Count > 0)
                {
                    btnEdit.Enabled = true; btnSave.Enabled = false;
                    dataGridView1.Rows.Clear();
                    foreach (DataRow item in d6.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = item["ProductName"].ToString(); 
                        dataGridView1.Rows[n].Cells[1].Value = item["USize"].ToString();
                        dataGridView1.Rows[n].Cells[2].Value = item["uQuantity"].ToString();
                        dataGridView1.Rows[n].Cells[3].Value = item["ufocQuantity"].ToString();
                        dataGridView1.Rows[n].Cells[4].Value = item["price"].ToString();
                        dataGridView1.Rows[n].Cells[5].Value = item["DiscountPercent"].ToString();
                        dataGridView1.Rows[n].Cells[6].Value = item["Discount"].ToString();
                        dataGridView1.Rows[n].Cells[7].Value = item["SaleTax"].ToString(); dataGridView1.Rows[n].Cells[8].Value = item["total"].ToString();

                    }
                    plustotal();
                }
                else {   }

             //   tblInvoice(status, , InCode, , DeliveryCode, , InvoiceDate, UserName)v
                       String query = "select * from "+tbl1+" where InCode = '" + txtsale.Text + "' ";
                DataTable d = clsDataLayer.RetreiveQuery(query);
                if (d.Rows.Count > 0)
                {  
                    txtsale.Enabled = false;
                    txtsale.ReadOnly = true; checkBox1.Enabled = false;
                     txtparty.Text = d.Rows[0]["InstituteName"].ToString();
                     txtremarks.Text = d.Rows[0]["Remarks"].ToString();
                    txtGrand.Text = d.Rows[0]["InvoiceAmount"].ToString();
                    txtscan.Text = d.Rows[0]["DeliveryCode"].ToString();

                    
                    dte_Date.Text = d.Rows[0]["InvoiceDate"].ToString();  
                    this.dataGridView1.Focus();
                    dataGridView1.Enabled = false; btnSave.Enabled = false; btnNew.Enabled = true;
                }
                else {   }
            
                //     old = Convert.ToDecimal(txtnetamount.Text);
                int namt = 0;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[2].Value != null)
                    {
                        namt = namt + int.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    }
                }
                label6.Text = namt.ToString();

            }
            catch
            {
            }
        }

        private void SaleProduct_Load_1(object sender, EventArgs e) { }

        private void txtAddress_Leave(object sender, EventArgs e)
        {
            dataGridView1.Focus();
        }

        private void btn_Delete_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            }
        }

        private void btnConfirmation_Click(object sender, EventArgs e)
        { 
        }

        private void txtconfirmcode_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    String query = "select * from Confirmation where BillCode = '" + txtconfirmcode.Text + "' ";
            //    DataTable d = clsDataLayer.RetreiveQuery(query);
            //    if (d.Rows.Count > 0)
            //    {
            //        txtsale.Enabled = false;
            //        txtsale.ReadOnly = true; 
            //        txtparty.Text = d.Rows[0]["PartyName"].ToString();
            //        txtbill.Text = d.Rows[0]["BillPrice"].ToString();   
            //        txtAddress.Text = d.Rows[0]["Address"].ToString(); comboOrderFrom.Text = d.Rows[0]["OrderTakenFrom"].ToString();


            //    //    String q2 = "select * from ConfirmationDetail where BillCode = '" + txtconfirmcode.Text + "' ";
            //    //    DataTable d2 = clsDataLayer.RetreiveQuery(q2);

            //    //    comboBox1.Text = d2.Rows[0]["SaleItem"].ToString(); 
            //    //    foreach (DataRow item in d2.Rows)
            //    //    {
            //    //        int n = dataGridView1.Rows.Add();
            //    //        dataGridView1.Rows[n].Cells[0].Value = item["SaleItem"].ToString();
            //    //        String sb = item["SaleItem"].ToString();
            //    //        String quer = "select QUANTITY from add_product where NAME='" + sb + "'";
            //    //        DataTable df = clsDataLayer.RetreiveQuery(quer);
            //    //        int quant = 0;
            //    //        if (df.Rows.Count > 0)
            //    //        {
            //    //            quant = Convert.ToInt32(df.Rows[0][0].ToString());
            //    //        }  
            //    //        if(quant>0)
            //    //        {
            //    //            dataGridView1.Rows[n].Cells[3].Value = item["QUANTITY"].ToString();
            //    //            dataGridView1.Rows[n].Cells[4].Value = item["CurrentStock"].ToString();
            //    //            dataGridView1.Rows[n].Cells[5].Value = item["TotalPriceItem"].ToString();
            //    //            dataGridView1.Rows[n].Cells[1].Value = item["Stock"].ToString();
            //    //        }
            //    //        else
            //    //        {
            //    //            dataGridView1.Rows[n].Cells[3].Value = "0";
            //    //            dataGridView1.Rows[n].Cells[4].Value = "0";
            //    //            dataGridView1.Rows[n].Cells[5].Value = "0";
            //    //            dataGridView1.Rows[n].Cells[1].Value = "0";
            //    //        }
            //    //        dataGridView1.Rows[n].Cells[2].Value = item["PricePerItem"].ToString();

            //    //    } 
            //    //    this.dataGridView1.Focus();
            //    //    dataGridView1.Enabled = true;
            //    //}
            //    //String q5 = "select SenderName from ConfirmationBy where BillCode = '" + txtsale.Text + "' ";
            //    //DataTable d5 = clsDataLayer.RetreiveQuery(q5);
            //    //if (d5.Rows.Count > 0)
            //    //{
            //    //txtparty.Text = d5.Rows[0][0].ToString();   cb = txtparty.Text;
            //    //}

            //    //decimal namt = 0;
            //    //for (int i = 0; i < dataGridView1.RowCount; i++)
            //    //{
            //    //if (dataGridView1.Rows[i].Cells[5].Value != null)
            //    //{
            //    //    namt = namt + decimal.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
            //    //}
            //    //}
            //    //this.txtbill.Text = namt.ToString();
            //    //Enable();
            //    //Enable1();
            //    //Enable2();
            //    //old = Convert.ToDecimal(txtnetamount.Text);
            // }
            //catch
            //{
            //}
        }

        private void dataGridView1_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal namt = 0;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[8].Value != null)
                    {
                        namt = namt + decimal.Parse(dataGridView1.Rows[i].Cells[8].Value.ToString());
                    }
                }
                this.txtGrand.Text = namt.ToString();
                btnSave.Focus(); plustotal();
            }
            catch { }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //try
            //{
            //    //Searching in GridViewTextBoxColumn 
            try
            {
                int yy = dataGridView1.CurrentCell.ColumnIndex;
                string columnHeaders = dataGridView1.Columns[yy].HeaderText;
                if (columnHeaders.Equals("Product Name"))
                {
                    tb = e.Control as TextBox;
                    if (tb != null)
                    { 
                        String hname = "select SProduct from tbl_ColdStorageMaintain where SQuantity!=0"; DataTable df = clsDataLayer.RetreiveQuery(hname);
                        if (df.Rows.Count > 0)
                        {
                            AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();

                            foreach (DataRow row in df.Rows)
                            {
                                acsc.Add(row[0].ToString());
                            }

                            tb.AutoCompleteCustomSource = acsc;
                            tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                    }
                }
                TextBox tb2 = null; tb2 = e.Control as TextBox;
                if (tb2 != null) { tb2.KeyPress += new KeyPressEventHandler(tb_KeyPress); }
            }
            catch { } 
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell.ColumnIndex != 0) //Desired Column
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
         

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell.ColumnIndex == 8 || dataGridView1.CurrentCell.ColumnIndex == 1 || dataGridView1.CurrentCell.ColumnIndex == 2)
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        int mn = dataGridView1.Rows.Count; mn--;
                        int m3 = dataGridView1.CurrentCell.RowIndex;
                        if (mn == m3)
                        {
                            same = 1;
                            dataGridView1.Rows.Add();
                            int yhs = dataGridView1.CurrentCell.RowIndex;
                            dataGridView1.CurrentCell = dataGridView1.Rows[yhs].Cells[0];
                            dataGridView1.BeginEdit(true);
                        }
                    }
                    else if (e.KeyCode == Keys.Delete)
                    {
                        if (dataGridView1.Rows.Count > 0)
                        {
                            int yh = dataGridView1.CurrentCell.RowIndex;
                            dataGridView1.Rows.RemoveAt(yh);
                        }
                    }
                }
                else
                {
                    if (e.KeyCode == Keys.Delete)
                    {
                        if (dataGridView1.Rows.Count > 0)
                        {
                            int yh = dataGridView1.CurrentCell.RowIndex;
                            dataGridView1.Rows.RemoveAt(yh);
                        }
                    }
                }

                if (e.KeyCode == Keys.Tab)
                {
                    if (dataGridView1.CurrentCell.ColumnIndex == 2)
                    {
                        dataGridView1.CurrentCell = dataGridView1.CurrentRow.Cells[8];
                    } 
                    e.Handled = true;
                }
                base.OnKeyDown(e);
            }
            catch { }
        }

        String VoucherStatus = "";
        private void Posting()
        {
            try
            {
                #region cashinc
                PartyCode(txtparty.Text);
                if (!HeaderAccount.Equals("1"))
                {
                    ReceiveDue();
                    decimal a2 = PartyBalance();
                    decimal b2 = Convert.ToDecimal(txtGrand.Text);
                    decimal c2 = a2 + b2;
                    VoucherStatus = "OnCredit";
                    String ins = @"insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
           " values('Sales','" + txtsale.Text + "','" + dte_Date.Value.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtsale.Text + "','" + txtparty.Text + "'," + b2 + ",0.00,'" + VoucherStatus + "'," + c2 + ",'4S') insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
           " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('Sales','" + txtsale.Text + "','" + dte_Date.Value.ToString("yyyy-MM-dd") + "','01010201','" + Code + "','" + txtsale.Text + "','Sales'," + b2 + ",0.00,'" + VoucherStatus + "' ," + c2 + ",'4S')";
                    clsDataLayer.ExecuteQuery(ins); 
                }
                #endregion cashinc

            }
            catch { }
        } 
        private void btnadd_Click_1(object sender, EventArgs e)
        {try { dataGridView1.Rows.Add(); }catch { }
        }
           
        private void btnFirst_Click(object sender, EventArgs e)
        {
            try
            {
                String q1 = "select SaleCode from View_Sale_Latest Order By SaleCode asc";
                DataTable d1 = clsDataLayer.RetreiveQuery(q1); if (d1.Rows.Count > 0) { txtsale.Text = d1.Rows[0][0].ToString(); }
                btnNew.Enabled = true; btnSave.Enabled = false; btnEdit.Enabled = true; btnUpdate.Enabled = false;
            }
            catch { }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            try
            {
                String q1 = "select SaleCode from View_Sale_Latest Order By SaleCode desc";
                DataTable d1 = clsDataLayer.RetreiveQuery(q1); if (d1.Rows.Count > 0) { txtsale.Text = d1.Rows[0][0].ToString(); }
                btnNew.Enabled = true; btnSave.Enabled = false; btnEdit.Enabled = true; btnUpdate.Enabled = false;
            }
            catch { }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
        try
        {
            String getcode = txtsale.Text;
            String[] hm = getcode.Split('-');
            String data = hm[1];
            int value = Convert.ToInt32(data);
            value++;
            String ck = value.ToString();
            
            if (ck.Length == 1)
            {
                ck ="SA"+"-"+"0000"+value.ToString(); 
            }
            else if (ck.Length == 2)
            {
                ck = "SA" + "-" + "000" + value.ToString(); 
            }
            else if (ck.Length == 3)
            {
                ck = "SA" + "-" + "00" + value.ToString(); 
            }else if (ck.Length == 4)
            {
                ck = "SA" + "-" + "0" + value.ToString(); 
            }
            else if (ck.Length == 5)
            {
                ck = "SA"+"-"+ value.ToString(); 
            }
            txtsale.Text = ck.ToString();
            btnNew.Enabled = true;
             
        } catch{ }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                String getcode = txtsale.Text;
                String[] hm = getcode.Split('-');
                String data = hm[1];
                int value = Convert.ToInt32(data);
                value--;
                String ck = value.ToString();

                if (ck.Length == 1)
                {
                    ck = "SA" + "-" + "0000" + value.ToString();
                }
                else if (ck.Length == 2)
                {
                    ck = "SA" + "-" + "000" + value.ToString();
                }
                else if (ck.Length == 3)
                {
                    ck = "SA" + "-" + "00" + value.ToString();
                }
                else if (ck.Length == 4)
                {
                    ck = "SA" + "-" + "0" + value.ToString();
                }
                else if (ck.Length == 5)
                {
                    ck = "SA" + "-" + value.ToString();
                }
                txtsale.Text = ck.ToString();
                btnNew.Enabled = true;
          }
            catch { }
        } 
        private void SaleProduct_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void close(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void SaleProduct_KeyDown(object sender, KeyEventArgs e)
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
                btnUpdate.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.P)
            {
                btnPrint.PerformClick();
            } 
            else if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        private void txtparty_Leave(object sender, EventArgs e)
        {
            txtremarks.Focus();
        }

         
        private void btnaddrow_Click(object sender, EventArgs e)
        {
            try { wg = "challan"; SelectChallan sc = new SelectChallan(this,txtparty.Text); sc.Show(); }
            catch { }
        }
          

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Ccheck();
        }

        private void Ccheck()
        {
            if (checkBox1.Checked)
            {
                txtsale.Text = ""; txtsale.ReadOnly = false; txtsale.Enabled = true;
            }
            else
            {
                txtsale.Text = clsGeneral.getMAXCode("tblInvoice", "InCode", "IN");
                txtsale.ReadOnly = true; txtsale.Enabled = false;
            }
        }

        private void btnuses_Click(object sender, EventArgs e)
        {
           // DeliveryProductUses dpu = new DeliveryProductUses(this); dpu.Show();
        }

        private void txtscan_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (wg.Equals("challan"))
                {
                    String get4 = "select * from tbl_DeliveryUses where DCode='" + txtscan.Text + "' and uUsedQuantity!=0"; DataTable da4 = clsDataLayer.RetreiveQuery(get4);
                    if (da4.Rows.Count > 0)
                    {
                        //
                        dataGridView1.Rows.Clear(); int nvb = 0; bool bon = false; int mob = 0;
                        foreach (DataRow dr in da4.Rows)
                        {
                            #region foreachloop
                            decimal GusedQuantity = 0; GusedQuantity = Convert.ToDecimal(dr["uUsedQuantity"].ToString()); 
                            decimal Quant = 0; decimal focQuant = 0;
                            #region abc
                            String f1 = "select uQuantity,ufocQuantity from viewinv where DeliveryCode='" + txtscan.Text + "' and ProductName='" + dr["ProductName"].ToString() + "' and USize='" + dr["USize"].ToString() + "'";
                            DataTable g2 = clsDataLayer.RetreiveQuery(f1);
                            if (g2.Rows.Count > 0)
                            {
                                for (int y = 0; y < g2.Rows.Count; y++)
                                {
                                    Quant += Convert.ToDecimal(g2.Rows[y][0].ToString()); focQuant += Convert.ToDecimal(g2.Rows[y][1].ToString());
                                }
                            }
                          
                                String f12 = "select uQuantity,ufocQuantity from viewinv2 where DeliveryCode='" + txtscan.Text + "' and ProductName='" + dr["ProductName"].ToString() + "' and USize='" + dr["USize"].ToString() + "'";
                                DataTable g22 = clsDataLayer.RetreiveQuery(f12);
                                if (g22.Rows.Count > 0)
                                {
                                    for (int y = 0; y < g22.Rows.Count; y++)
                                    {
                                 Quant += Convert.ToDecimal(g22.Rows[y][0].ToString()); focQuant += Convert.ToDecimal(g22.Rows[y][1].ToString());
                                    }
                                }
                            
                            #endregion abc

                            decimal qf = Quant + focQuant;
                            decimal final = GusedQuantity - qf;
                            decimal uremain = 0; 
                            String pname = dr["ProductName"].ToString(); String psize = dr["USize"].ToString();
                            String m4 = "select  uRemainingQuantity from  tbl_DeliveryUses where ProductName='" + pname + "' and USize='"+psize+"' and DCode='" + txtscan.Text + "'"; DataTable d90 = clsDataLayer.RetreiveQuery(m4);
                            if (d90.Rows.Count > 0)
                            {
                                uremain =Convert.ToDecimal(d90.Rows[0][0].ToString());
                            }
                            if (uremain == 0 && final == 0)
                            {
                        bon = true;
                        mob++;
                            }
                            else if (uremain != 0 && final == 0)
                            {
                                nvb++; bon = false;
                            }
                            else
                            {
                                bon = false;  
                                #region gridinsert
                                int n = dataGridView1.Rows.Add();
                                dataGridView1.Rows[n].Cells[0].Value = dr["ProductName"].ToString(); dataGridView1.Rows[n].Cells[1].Value = dr["USize"].ToString();
                                dataGridView1.Rows[n].Cells[2].Value = final.ToString(); dataGridView1.Rows[n].Cells[3].Value = "0";
                                decimal ppc = 0;
                                String n54 = "select PricePerItem from tbl_DeliveryDetail where DCode='" + txtscan.Text + "' and ProductItem='" + dr["ProductName"].ToString() + "' and Size='" + dr["USize"].ToString() + "' and PurchaseOrder='" + dr["UPurchaseOrder"].ToString() + "'"; DataTable d9 = clsDataLayer.RetreiveQuery(n54);
                                if (d9.Rows.Count > 0) { ppc = Convert.ToDecimal(d9.Rows[0][0].ToString()); }
                                dataGridView1.Rows[n].Cells[4].Value = ppc.ToString(); dataGridView1.Rows[n].Cells[5].Value = "0";
                                dataGridView1.Rows[n].Cells[6].Value = "0"; dataGridView1.Rows[n].Cells[7].Value = "0";
                                decimal ff = ppc * final;
                                dataGridView1.Rows[n].Cells[8].Value = ff.ToString();
                                #endregion gridinsert
                            }
                            #endregion foreachloop
                        }

                        if (nvb == da4.Rows.Count)
                        {    MessageBox.Show("Available Product Quantity for Making Invoice is = 0!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Stop); txtscan.Text = "";
                        }
                        if (mob == da4.Rows.Count)
                        {
      String up6 = "update tbl_Delivery set vstatus='Invoice' where DCode='" + txtscan.Text + "' "; clsDataLayer.ExecuteQuery(up6);
      MessageBox.Show("Delivery Voucher Status Updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Stop); txtscan.Text = "";
                         }
                    }
                    //

                    String get = "select * from vdelivery where DCode='" + txtscan.Text + "' and uUsedQuantity!=0"; DataTable da = clsDataLayer.RetreiveQuery(get);
                    if (da.Rows.Count > 0)
                    {
                        txtparty.Text = da.Rows[0]["InstituteName"].ToString(); txtremarks.Text = da.Rows[0]["Remarks"].ToString();

                    }
                    else
                    {
                        txtscan.Text = "No Record Found!";
                    }
                }
                plustotal();
            }
            catch  
            {  }
        }
         
        //LedgerUpdate Closed
    }
    }
                                                                                               