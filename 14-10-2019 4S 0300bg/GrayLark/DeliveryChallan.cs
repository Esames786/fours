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
    public partial class DeliveryChallan : Form
    {
        DataTable dz; String cb = "";
        int same = 0;
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


        public DeliveryChallan()
        {
            InitializeComponent();
            //or HeaderActCode='1'
            txtsale.Enabled = false;    Disable();  dataGridView1.Enabled = false;   btnNew.Focus();
             txtparty.TabIndex = 1;     dte_Date.TabStop = false;    dataGridView1.TabIndex = 3; 
            New();   btnFirst.Enabled = true; btnLast.Enabled = true; btnPrevious.Enabled = true; btnNext.Enabled = true; btnSave.Enabled = true;
            txtparty.Focus(); this.KeyPreview = true; btnuses.Enabled = false; Enable(); Enable2(); dataGridView1.Enabled = true;
            String hname = "select LordNumber from add_product_stock where WarehouseName='" + txtwarehouse.Text + "' and Status='Active' and  Quantity!=0"; DataTable df = clsDataLayer.RetreiveQuery(hname);
            if (df.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txtlod, df);
            }
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
                        else
                        {
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

        
            return flag;
        }

        private bool CheckDataGridCells(DataGridView dgv)
        {
            bool flag = false;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < 5; j++)
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
                decimal abc = 0;
                for (int ry = 0; ry < dataGridView1.Rows.Count; ry++)
                {
                    abc = Convert.ToDecimal(dataGridView1.Rows[ry].Cells[2].Value);
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
                String query = "select VoucherStatus from tbl_sale where SaleCode = '" + txtsale.Text + "' and PostStatus='UNPOST'"; DataTable de = clsDataLayer.RetreiveQuery(query);
                if (de.Rows.Count > 0) { VoucherStatus = de.Rows[0][0].ToString(); }
                Posting();
                String upd = "update tbl_sale set PostStatus='POST' where SaleCode = '" + txtsale.Text + "'"; clsDataLayer.ExecuteQuery(upd);
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

        private void StockDeduct()
        {

            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    decimal oldq = 0;  
                    String s5 = "select Quantity from tbl_DeliveryDetail where PurchaseOrder='" + dataGridView1.Rows[i].Cells[3].Value + "' and DCode='" + txtsale.Text + "' and ProductItem='" + dataGridView1.Rows[i].Cells[0].Value + "' and Size='" + dataGridView1.Rows[i].Cells[1].Value + "'"; DataTable ds5 = clsDataLayer.RetreiveQuery(s5);
                    if (ds5.Rows.Count > 0)
                    {
                        oldq = Convert.ToDecimal(ds5.Rows[0][0].ToString());
                    }
                    String sel = "select Quantity from add_product_stock where Status='Active' and Size='" + dataGridView1.Rows[i].Cells[1].Value + "' and NAME='" + dataGridView1.Rows[i].Cells[0].Value + "' and LordNumber='" + dataGridView1.Rows[i].Cells[3].Value + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(sel);
                    if (d1.Rows.Count > 0)
                    {
                        decimal dq = Convert.ToDecimal(d1.Rows[0][0].ToString()); decimal iqs = Convert.ToDecimal(dataGridView1.Rows[i].Cells[2].Value);

                        decimal final = dq + oldq - iqs;
                        String upd = "update add_product_stock set Quantity=" + final + " where Status='Active' and Size='" + dataGridView1.Rows[i].Cells[1].Value + "' and NAME='" + dataGridView1.Rows[i].Cells[0].Value + "' and LordNumber='" + dataGridView1.Rows[i].Cells[3].Value + "'"; clsDataLayer.ExecuteQuery(upd);
                    }
                }
            }
            catch { }
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
                            String bs = "select * from  tbl_Delivery where DCode='" + txtsale.Text + "'";
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
                                    SaveButton(); txtsale.Enabled = false; dataGridView1.Enabled = false; btnNew.Enabled = true; btnuses.Enabled = true;
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
        
            #region SaveStartFirst
            
            txtsale.Text = clsGeneral.getMAXCode("tbl_Delivery", "DCode", "DC");
            btnSave.Enabled = false;
            VoucherStatus = "";
//                    DD(Delivery Challan)
//FOC(Free of Cost)
//SAMPLE
                    if(cmbstatus.Text.Equals("DD"))
                    {
                        VoucherStatus = "Delivery";
                    }
                    else
                    {
                        VoucherStatus = "Invoice";
                    }
                    StockDeduct();

            String ins = "insert into tbl_Delivery(DCode,InstituteName,Remarks,Warehouse,Saleperson,ChallanStatus,Dates,vstatus) values ('" + txtsale.Text + "','" + txtparty.Text + "','" + txtremarks.Text + "','" + txtwarehouse.Text + "','" + txtsaleperson.Text + "','" + cmbstatus.Text + "','" + dte_Date.Value.ToString("yyyy-MM-dd") + "','"+VoucherStatus+"')";
             if (clsDataLayer.ExecuteQuery(ins) > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
               //Stock Update
                    decimal fsp = 0; decimal dquantity = 0;
                    String h4 = "select SellPrice,Quantity from add_product_stock where LordNumber= '" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "' and WarehouseName='"+txtwarehouse.Text+"' and NAME = '" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "' and Size ='" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'";
                    DataTable d3 = clsDataLayer.RetreiveQuery(h4); if (d3.Rows.Count > 0)
                    {
                        fsp = Convert.ToDecimal(d3.Rows[0][0].ToString()); dquantity = Convert.ToDecimal(d3.Rows[0][1]);
                    }
                    decimal iq = Convert.ToDecimal(dataGridView1.Rows[i].Cells[2].Value.ToString());
                            if (VoucherStatus.Equals("Invoice"))
                            {
               decimal sfinal = dquantity - iq;
               //String update4 = "update add_product_stock set Quantity="+sfinal+ " where LordNumber= '" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "' and WarehouseName='" + txtwarehouse.Text + "' and NAME = '" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "' and Size ='" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'"; clsDataLayer.ExecuteQuery(update4);
                          
                            }
                    decimal ftotal = iq * fsp;
                    String insert2 = "insert into tbl_DeliveryDetail(DCode,ProductItem,Size,Quantity,PurchaseOrder,ExpiryDate,PricePerItem,TotalPrice,Referencecode) values('" + txtsale.Text + "','" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'," + dataGridView1.Rows[i].Cells[2].Value.ToString() + ",'" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[4].Value.ToString() + "'," + fsp + "," + ftotal + ",'" + dataGridView1.Rows[i].Cells[5].Value.ToString() + "')";
               if (clsDataLayer.ExecuteQuery(insert2) > 0) { }

                }
         
            }
           // LedgerDone();
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
//            try
//            {
//                PartyCode(txtparty.Text);
//                String rec = "select * from ReceiveDue where PartyCode = '" + Code + "'";
//                DataTable d = clsDataLayer.RetreiveQuery(rec);
//                if (d.Rows.Count > 0)
//                {
//                    decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
//                    decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
//                    decimal received = decimal.Parse(d.Rows[0]["ReceivedAmount"].ToString());
//                    decimal final = Convert.ToDecimal(txtGrand.Text);
//                    total =total-old+final;
//                    due =due-old+final;
//                    string updateblnc = "update ReceiveDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
//                    clsDataLayer.ExecuteQuery(updateblnc);
//                }
//                else
//                {
//                    decimal b = Convert.ToDecimal(txtGrand.Text);
//                    String ii = @"insert into ReceiveDue (RemCode,PartyName,PartyCode,TotalAmount,DueAmount,ReceivedAmount,Company_Name)
//                values('" + txtsale.Text + "','" + txtparty.Text + "','" + Code + "'," + b + "," + b + ",0,'Delizia')";
//                    clsDataLayer.ExecuteQuery(ii);
//                }
//            }
//            catch { }
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
       
            return flag;
        }

        private bool Disable()
        {
            bool flag = false;
            
            return flag;
        }
        private bool Disable1()
        {
            bool flag = false;
            dataGridView1.Enabled = false;
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
            dataGridView1.Rows.Clear();
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
                clsGeneral.SetAutoCompleteTextBox(txtsaleperson, clsDataLayer.RetreiveQuery("select distinct Saleperson from tbl_Delivery "));
                clsGeneral.SetAutoCompleteTextBox(txtwarehouse, clsDataLayer.RetreiveQuery("select distinct WarehouseName from add_product_stock where Status='Active' and  Quantity!=0"));

                Enable();   Enable2();     Empty();
                dataGridView1.Enabled = true;
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                btnNew.Enabled = false;
                btnUpdate.Enabled = false;
                //dataGridView1.Rows.Clear(); 
                //dataGridView1.Rows.Add();
                Clears(); String hname = "select distinct LordNumber from add_product_stock where Status='Active' and  Quantity!=0"; DataTable df = clsDataLayer.RetreiveQuery(hname);
                    if (df.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtlod, df);
                    }
                    txtsale.Text = clsGeneral.getMAXCode("tbl_Delivery", "DCode", "DC");
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
            New(); txtsale.Enabled = false; old = 0; dataGridView1.Enabled = true; btnSave.Enabled = true; btnuses.Enabled = false; Enable2();
        }

        private void Empty()
        {
            txtparty.Clear(); txtparty.Text = "Select";
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
                                StockDeduct(); 
                            string query = "";
                            query = "Delete From tbl_Delivery Where DCode = '" + txtsale.Text + "'";
                            clsDataLayer.ExecuteQuery(query);
                            String VoucherStatus = "";
                            PartyCode(txtparty.Text);

                            //ReceiveDue();
                            VoucherStatus = "";
                                if (cmbstatus.Text.Equals("DD"))
                                {
                                    VoucherStatus = "Delivery";
                                }
                                else
                                {
                                    VoucherStatus = "Invoice";
                                }

                                String ins = "insert into tbl_Delivery(DCode,InstituteName,Remarks,Warehouse,Saleperson,ChallanStatus,Dates,vstatus) values ('" + txtsale.Text + "','" + txtparty.Text + "','" + txtremarks.Text + "','" + txtwarehouse.Text + "','" + txtsaleperson.Text + "','" + cmbstatus.Text + "','" + dte_Date.Value.ToString("yyyy-MM-dd") + "','"+VoucherStatus+"')";
 
                                if (clsDataLayer.ExecuteQuery(ins) > 0)
                                    {
                                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                                {
                                        decimal oldq = 0;
                                        String g4 = "select Quantity from tbl_DeliveryDetail Where DCode = '" + txtsale.Text + "' and ProductItem='" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "' and Size='" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "' and PurchaseOrder='" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "'"; DataTable vf = clsDataLayer.RetreiveQuery(g4);
                                        if (vf.Rows.Count > 0)
                                        {
                                            oldq = Convert.ToDecimal(vf.Rows[0][0]);
                                        }
                                        decimal fsp = 0; decimal dquantity = 0;
                                        String h4 = "select SellPrice,Quantity from add_product_stock where LordNumber= '" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "' and WarehouseName='" + txtwarehouse.Text + "' and NAME = '" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "' and Size ='" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'";
                                        DataTable d3 = clsDataLayer.RetreiveQuery(h4); if (d3.Rows.Count > 0)
                                        {
                                            fsp = Convert.ToDecimal(d3.Rows[0][0].ToString()); dquantity = Convert.ToDecimal(d3.Rows[0][1]);
                                        }
                                        decimal iq = Convert.ToDecimal(dataGridView1.Rows[i].Cells[2].Value.ToString());
                                        if (VoucherStatus.Equals("Invoice"))
                                        {
                                            decimal sfinal = dquantity = oldq - iq;
                                      //      String update4 = "update add_product_stock set Quantity=" + sfinal + " where LordNumber= '" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "' and WarehouseName='" + txtwarehouse.Text + "' and NAME = '" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "' and Size ='" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'"; clsDataLayer.ExecuteQuery(update4);
                                        }
                                        decimal ftotal = iq * fsp;
                                        String del = "Delete From tbl_DeliveryDetail Where DCode = '" + txtsale.Text + "' and ProductItem='"+ dataGridView1.Rows[i].Cells[0].Value.ToString() + "' and Size='"+ dataGridView1.Rows[i].Cells[1].Value.ToString() + "' and PurchaseOrder='"+ dataGridView1.Rows[i].Cells[3].Value.ToString() + "'"; clsDataLayer.ExecuteQuery(del);
                                        String insert2 = "insert into tbl_DeliveryDetail(DCode,ProductItem,Size,Quantity,PurchaseOrder,ExpiryDate,PricePerItem,TotalPrice,Referencecode) values('" + txtsale.Text + "','" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "'," + dataGridView1.Rows[i].Cells[2].Value.ToString() + ",'" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[4].Value.ToString() + "'," + fsp + "," + ftotal + ",'" + dataGridView1.Rows[i].Cells[5].Value.ToString() + "')";
               if (clsDataLayer.ExecuteQuery(insert2) > 0) { }

                                } 
                            
                                //ReceiveDue();
                                //int index = GetRowIndex(txtsale.Text);
                                //DataTable dt = SetDT();
                                //ShowDt(dt);
                                //DeleteRecord();
                                //dt.Rows[index][7] = Convert.ToDecimal(txtGrand.Text);
                                //dt.Rows[index + 1][8] = Convert.ToDecimal(txtGrand.Text);
                                //dt = SomeOperation(dt);
                                //InsertRecord(dt);
                          MessageBox.Show("Voucher Updated Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                                dataGridView1.Rows.Clear(); btnSave.Enabled = false; btnEdit.Enabled = true;
                                btnNew.Enabled = true; btnUpdate.Enabled = false; dataGridView1.Enabled = false; btnuses.Enabled = true;
                                }
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
                    bool mf = false; decimal usedq = 0;
                  String h6 = "select uUsedQuantity from tbl_DeliveryUses where DCode='" + txtsale.Text + "'";
                    DataTable d6 = clsDataLayer.RetreiveQuery(h6);
                    if (d6.Rows.Count > 0)
                    {
                    for (int g = 0; g < d6.Rows.Count; g++)
                    {
                    usedq = Convert.ToDecimal(d6.Rows[g][0].ToString());
                    if (usedq > 0)
                    {
                        mf = true;
                    }
                    }
                    }
                 

                    if (mf == true)
                    {
                        MessageBox.Show("User Cant Update Challan. Lot Number Against Quantity Already Use in Delivery Challan!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        txtsale.Enabled = true;
                        txtsale.ReadOnly = false;
                        btnSave.Enabled = false;
                        btnEdit.Enabled = false;
                        btnNew.Enabled = false;
                        btnUpdate.Enabled = true;
                        Enable();
                        Enable2();
                        dataGridView1.Enabled = true; cmbstatus.Enabled = false; txtparty.Enabled = false;
                    }
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
                UserNametesting();
                if (GreenSignal == "YES")
                {

                    String q = "select * from vdel where DCode = '" + txtsale.Text + "' ";
                    DataTable dt = clsDataLayer.RetreiveQuery(q);
                    if (dt.Rows.Count > 0)
                    {
                       GrayLark.bin.Debug.Report.DeliveryReport rpt = new DeliveryReport();
                        rpt.SetDataSource(dt); 
                         rptDeliveryview frm = new rptDeliveryview(rpt,txtsale.Text);
                         frm.Show(); 
                    }
                    else
                    {
                        MessageBox.Show("No Record Found!", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
        
        private void grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                e.Control.KeyPress -= new KeyPressEventHandler(gridColumns_KeyPress);
                if (dataGridView1.CurrentCell.ColumnIndex == 2) //Desired Column
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
                    #region rg1
                    String inprod= dataGridView1.CurrentRow.Cells[0].Value.ToString(); String size= dataGridView1.CurrentRow.Cells[1].Value.ToString();
                String s1 = "select Size from add_product_stock where Size='" + size + "' and WarehouseName='"+txtwarehouse.Text+"'"; DataTable ds1 = clsDataLayer.RetreiveQuery(s1);
                      if (ds1.Rows.Count > 0)
                      { 
  //                        int nn = 0;
  //                        for (int a = 0; a < dataGridView1.Rows.Count; a++)
  //                        {
  //                            String Gpname = dataGridView1.Rows[a].Cells[0].Value.ToString();
  //                            String Gpsize = dataGridView1.Rows[a].Cells[1].Value.ToString();
  //                            if (Gpname.Equals("") || Gpsize.Equals(""))
  //                            {

  //                            }
  //                            else if (inprod.Equals(Gpname) && size.Equals(Gpsize))
  //                            {
  //                                nn++;
  //                            }
  //                        }
  //if (nn > 1) { MessageBox.Show("Same Product Cant Add Multiple Time", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); 
  //    dataGridView1.CurrentRow.Cells[1].Value = ""; }
                      }
                      else
                      {
                          MessageBox.Show("Selected Size not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); dataGridView1.CurrentRow.Cells[1].Value = "";
                      }
                    #endregion rg1
                }
                else if (dataGridView1.CurrentCell.ColumnIndex == 0)
                { 
                    try
                    {
                        #region ColumnIndex2
                        String pname = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        String s1 = "select NAME from add_product_stock where NAME='" + pname + "' and WarehouseName='" + txtwarehouse.Text + "'"; DataTable ds1 = clsDataLayer.RetreiveQuery(s1);
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
                else if (dataGridView1.CurrentCell.ColumnIndex == 3)
                {
                    #region r3
                    String mlot = dataGridView1.CurrentRow.Cells[3].Value.ToString(); String pname = dataGridView1.CurrentRow.Cells[0].Value.ToString();   String size = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    String g1 = "select Expdate,Quantity from add_product_stock where NAME='" + pname + "' and Size='" + size + "' and  LordNumber = '" + dataGridView1.CurrentRow.Cells[3].Value.ToString() + "' and Status = 'Active'"; DataTable d1 = clsDataLayer.RetreiveQuery(g1);
                    if (d1.Rows.Count > 0)
                    {
                        String e5 = d1.Rows[0][0].ToString(); dataGridView1.CurrentRow.Cells[4].Value = e5; decimal dbq = Convert.ToDecimal(d1.Rows[0][1].ToString());
                        decimal ipq= Convert.ToDecimal(dataGridView1.CurrentRow.Cells[2].Value.ToString());
                        if(ipq > dbq)
                        {
                            dataGridView1.CurrentRow.Cells[2].Value = dbq.ToString();
                        }

                        int nn = 0;
                        for (int a = 0; a < dataGridView1.Rows.Count; a++)
                        {
                            String Gpname = dataGridView1.Rows[a].Cells[0].Value.ToString();
                            String Gpsize = dataGridView1.Rows[a].Cells[1].Value.ToString();
                            String Glot = dataGridView1.Rows[a].Cells[3].Value.ToString();
                            if (Gpname.Equals("") || Gpsize.Equals("") || Glot.Equals(""))
                            {

                            }
                            else if (pname.Equals(Gpname) && size.Equals(Gpsize) && Glot.Equals(mlot))
                            {
                                nn++;
                            }
                        }
                        if (nn > 1)
                        {
                            MessageBox.Show("Same Product Cant Add Multiple Time", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dataGridView1.CurrentRow.Cells[3].Value = "";
                        }


                    }
                    else
                    { MessageBox.Show("LordNumber not found.!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); dataGridView1.CurrentRow.Cells[3].Value = ""; }
                    #endregion r3
                }
                else if (dataGridView1.CurrentCell.ColumnIndex == 4)
                {
                    try
                    {
                        String getvc = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                        if (getvc.Contains("-"))
                        {
                            #region DateLogic
                            String[] dis = getvc.Split('-');
                            String ddd = dis[0].ToString(); String mm = dis[1].ToString(); String yy = dis[2].ToString();
                            if (ddd.Length == 2)
                            {

                            }
                            else
                            {
                                MessageBox.Show("Day Length must be 2", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); dataGridView1.CurrentRow.Cells[4].Value = "";
                            }

                            if (mm.Length == 2)
                            {

                            }
                            else
                            {
                                MessageBox.Show("Month Length must be 2 Month=01 ", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); dataGridView1.CurrentRow.Cells[4].Value = "";
                            }

                            if (yy.Length == 4)
                            {

                            }
                            else
                            {
                                MessageBox.Show("Year Length must be 4 Year=2015 ", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); dataGridView1.CurrentRow.Cells[4].Value = "";
                            }
                            #endregion DateLogic
                        }
                        else
                        {
                            MessageBox.Show("Date Format Include - hyphen Format:02-05-2019", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); dataGridView1.CurrentRow.Cells[4].Value = "";
                        }
                    }
                    catch { }
                }
                else if (dataGridView1.CurrentCell.ColumnIndex == 2)
                {
                    //Quantity
                    //
                    decimal oldq = 0;
                    String s5 = "select Quantity from tbl_DeliveryDetail where PurchaseOrder='" + dataGridView1.CurrentRow.Cells[3].Value + "' and DCode='" + txtsale.Text + "' and ProductItem='" + dataGridView1.CurrentRow.Cells[0].Value + "' and Size='" + dataGridView1.CurrentRow.Cells[1].Value + "'"; DataTable ds5 = clsDataLayer.RetreiveQuery(s5);
                    if (ds5.Rows.Count > 0)
                    {
                        oldq = Convert.ToDecimal(ds5.Rows[0][0].ToString());
                    }
                    //
                    String pname = dataGridView1.CurrentRow.Cells[0].Value.ToString(); String size = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    String g1 = "select Expdate,Quantity from add_product_stock where NAME='" + pname + "' and Size='" + size + "' and  LordNumber = '" + dataGridView1.CurrentRow.Cells[3].Value.ToString() + "' and Status = 'Active'"; DataTable d1 = clsDataLayer.RetreiveQuery(g1);
                    if (d1.Rows.Count > 0)
                    {
                        String e5 = d1.Rows[0][0].ToString(); dataGridView1.CurrentRow.Cells[4].Value = e5; decimal dbq = Convert.ToDecimal(d1.Rows[0][1].ToString());
                        dbq += oldq; decimal ipq = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[2].Value.ToString());
                        if (ipq > dbq)
                        {
                            dataGridView1.CurrentRow.Cells[2].Value = dbq.ToString();
                        }
                    }
                    else
                    { MessageBox.Show("LordNumber not found.!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); dataGridView1.CurrentRow.Cells[3].Value = ""; }
              
                }

 
                
            }
            catch { }
            }

   //     private void ResQuant()
   //     {
   //         try
   //         {
   //         String pname = dataGridView1.CurrentRow.Cells[0].Value.ToString(); String size = dataGridView1.CurrentRow.Cells[1].Value.ToString();
   //         String poc = dataGridView1.CurrentRow.Cells[3].Value.ToString();
   //         String g1 = "select Expdate,Quantity from add_product_stock where NAME='" + pname + "' and Size='" + size + "' and  LordNumber = '" + dataGridView1.CurrentRow.Cells[3].Value.ToString() + "' and Status = 'Active'"; DataTable d1 = clsDataLayer.RetreiveQuery(g1);
   //         if (d1.Rows.Count > 0)
   //         {
   //             String e5 = d1.Rows[0][0].ToString(); dataGridView1.CurrentRow.Cells[4].Value = e5; decimal stock = Convert.ToDecimal(d1.Rows[0][1].ToString());
   //             decimal input = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[2].Value.ToString());

   //             decimal dcquant = 0;
   //             String m4 = "select sum(Quantity) from vdel where vstatus='Delivery' and ProductItem='" + pname + "' and Size='" + size + "' and PurchaseOrder='" + poc + "'"; DataTable d2 = clsDataLayer.RetreiveQuery(m4);
   //if (d2.Rows.Count > 0) { dcquant = Convert.ToDecimal(d2.Rows[0][0].ToString()); }

   //decimal dcreturn = 0;
   //String m5 = "select sum(uReturnQuantity) from vdel2 where vstatus='Delivery' and ProductName='" + pname + "' and USize='" + size + "' and UPurchaseOrder='" + poc + "'";
   //             DataTable d3 = clsDataLayer.RetreiveQuery(m5);
   //             if (d3.Rows.Count > 0) { dcreturn = Convert.ToDecimal(d3.Rows[0][0].ToString()); }

   //             decimal final1 = stock - dcquant;
   //             final1 = final1 + dcreturn;
   //             if (input > final1)
   //             {
   //                 dataGridView1.CurrentRow.Cells[2].Value = final1.ToString(); MessageBox.Show("Available Quantity = " + final1, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
   //             }
   //             //if (ipq > dbq)
   //             //{
   //             //    dataGridView1.CurrentRow.Cells[2].Value = dbq.ToString();
   //             //}
   //         }
   //         else
   //         { MessageBox.Show("LordNumber not found.!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); dataGridView1.CurrentRow.Cells[3].Value = ""; }
           
   //         }
   //         catch { }

   //     }

         
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
                this.Close();
                View_All_Sale ror = new View_All_Sale("ProductSale");
                ror.Show();
            }
            catch { }
        }

        private void txtsale_TextChanged(object sender, EventArgs e)
        { try
            {
                String query4 = "select * from tbl_DeliveryDetail where DCode = '" + txtsale.Text + "' ";
                DataTable d9 = clsDataLayer.RetreiveQuery(query4);
                if (d9.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                    foreach (DataRow item in d9.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = item["ProductItem"].ToString();
                        dataGridView1.Rows[n].Cells[1].Value = item["Size"].ToString();
                        dataGridView1.Rows[n].Cells[2].Value = item["Quantity"].ToString();
                        dataGridView1.Rows[n].Cells[3].Value = item["PurchaseOrder"].ToString();
                        dataGridView1.Rows[n].Cells[4].Value = item["ExpiryDate"].ToString();
                        dataGridView1.Rows[n].Cells[5].Value = item["Referencecode"].ToString();
                    }
                    this.dataGridView1.Focus();
                    dataGridView1.Enabled = false; btnSave.Enabled = false; btnNew.Enabled = true;
                }


                btnEdit.Enabled = true; btnSave.Enabled = false;
                String query = "select * from tbl_Delivery where DCode = '" + txtsale.Text + "' ";
                DataTable d = clsDataLayer.RetreiveQuery(query);
                if (d.Rows.Count > 0)
                {  
                    txtsale.Enabled = false;
                    txtsale.ReadOnly = true;
                    txtparty.Text = d.Rows[0]["InstituteName"].ToString();
                    txtremarks.Text = d.Rows[0]["Remarks"].ToString();

                    txtwarehouse.Text = d.Rows[0]["Warehouse"].ToString();
                    txtsaleperson.Text = d.Rows[0]["Saleperson"].ToString();
                    cmbstatus.Text = d.Rows[0]["ChallanStatus"].ToString();

                    dte_Date.Text = d.Rows[0]["Dates"].ToString();
                   // vstatus = d.Rows[0]["PostStatus"].ToString(); 
 
                 
                    if (cmbstatus.Text.Equals("DD"))
                    {
                        btnuses.Enabled = true;
                    }
                    else
                    {
                        btnuses.Enabled = false;
                    }
                }
                else
                {
                 //   MessageBox.Show("No Record Found!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); Clears();
                }

                //     old = Convert.ToDecimal(txtnetamount.Text);
                Disable(); Disable1();

            }
            catch
            {
            }
        }
         

        private void txtAddress_Leave(object sender, EventArgs e)
        {
            dataGridView1.Focus();
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
                        String hname = "select NAME from add_product_stock where Status='Active' and  Quantity!=0 and WarehouseName='" + txtwarehouse.Text + "'"; DataTable df = clsDataLayer.RetreiveQuery(hname);
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
                else if (columnHeaders.Equals("Size"))
                {
                    tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        String hname = "select Size from add_product_stock where NAME='" + dataGridView1.CurrentRow.Cells[0].Value + "' and Status='Active' and  Quantity!=0 and WarehouseName='" + txtwarehouse.Text + "'"; DataTable df = clsDataLayer.RetreiveQuery(hname);
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
                else if (columnHeaders.Equals("Lord No"))
                {
                    tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        String hname = "select LordNumber from add_product_stock where WarehouseName='" + txtwarehouse.Text + "' and Size='" + dataGridView1.CurrentRow.Cells[1].Value + "' and NAME='" + dataGridView1.CurrentRow.Cells[0].Value + "' and Status='Active' and  Quantity!=0"; DataTable df = clsDataLayer.RetreiveQuery(hname);
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
                else
                {
                    tb.AutoCompleteCustomSource = null;
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
                if (dataGridView1.CurrentCell.ColumnIndex == 2) //Desired Column
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
                if (dataGridView1.CurrentCell.ColumnIndex == 3 || dataGridView1.CurrentCell.ColumnIndex == 4 )
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

                //if (e.KeyCode == Keys.Tab)
                //{
                //    if (dataGridView1.CurrentCell.ColumnIndex == 2)
                //    {
                //        dataGridView1.CurrentCell = dataGridView1.CurrentRow.Cells[8];
                //    } 
                //    e.Handled = true;
                //}
                //base.OnKeyDown(e);
            }
            catch { }
        }

        String VoucherStatus = "";
        private void Posting()
        {
           // try
           // {
           //     #region cashinc
           //     PartyCode(txtparty.Text);
           //     if (!HeaderAccount.Equals("1"))
           //     {
           //         ReceiveDue();
           //         decimal a2 = PartyBalance();
           //         decimal b2 = Convert.ToDecimal(txtGrand.Text);
           //         decimal c2 = a2 + b2;
           //         VoucherStatus = "OnCredit";
           //         String ins = @"insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
           //" values('Sales','" + txtsale.Text + "','" + dte_Date.Value.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtsale.Text + "','" + txtparty.Text + "'," + b2 + ",0.00,'" + VoucherStatus + "'," + c2 + ",'Delizia') insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
           //" Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('Sales','" + txtsale.Text + "','" + dte_Date.Value.ToString("yyyy-MM-dd") + "','01010201','" + Code + "','" + txtsale.Text + "','Sales'," + b2 + ",0.00,'" + VoucherStatus + "' ," + c2 + ",'Delizia')";
           //         clsDataLayer.ExecuteQuery(ins); 
           //     }
           //     #endregion cashinc

           // }
           // catch { }
        } 
        private void btnadd_Click_1(object sender, EventArgs e)
        {try { dataGridView1.Rows.Add(); }catch { }
        }
           
        private void btnFirst_Click(object sender, EventArgs e)
        {
            try
            {
                String q1 = "select DCode from vdelivery Order By DCode asc";
                DataTable d1 = clsDataLayer.RetreiveQuery(q1); if (d1.Rows.Count > 0) { txtsale.Text = d1.Rows[0][0].ToString(); }
                btnNew.Enabled = true; btnSave.Enabled = false; btnEdit.Enabled = true; btnUpdate.Enabled = false;
            }
            catch { }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            try
            {
                String q1 = "select DCode from vdelivery Order By DCode desc";
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
                ck = "DC" + "-" + "0000" + value.ToString(); 
            }
            else if (ck.Length == 2)
            {
                ck = "DC" + "-" + "000" + value.ToString(); 
            }
            else if (ck.Length == 3)
            {
                ck = "DC" + "-" + "00" + value.ToString(); 
            }else if (ck.Length == 4)
            {
                ck = "DC" + "-" + "0" + value.ToString(); 
            }
            else if (ck.Length == 5)
            {
                ck = "DC" + "-" + value.ToString(); 
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
                    ck = "DC" + "-" + "0000" + value.ToString();
                }
                else if (ck.Length == 2)
                {
                    ck = "DC" + "-" + "000" + value.ToString();
                }
                else if (ck.Length == 3)
                {
                    ck = "DC" + "-" + "00" + value.ToString();
                }
                else if (ck.Length == 4)
                {
                    ck = "DC" + "-" + "0" + value.ToString();
                }
                else if (ck.Length == 5)
                {
                    ck = "DC" + "-" + value.ToString();
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

        private void txtscan_TextChanged(object sender, EventArgs e)
        {
        try{
                if (!txtlod.Text.Equals(""))
                {
                    #region r3
                    String g2 = "select NAME,Quantity,Size,SellPrice,LordNumber,Expdate,Referencecode from add_product_stock where productcode='" + txtscan.Text + "' and Status = 'Active' and LordNumber='" + txtlod.Text + "'";
                    DataTable d3 = clsDataLayer.RetreiveQuery(g2);
                    if (d3.Rows.Count > 0)
                    {
                        String Dbpdname = d3.Rows[0][0].ToString(); String Size = d3.Rows[0][2].ToString();
                        String get_lord_number = d3.Rows[0][4].ToString();

                        decimal msp = 0; msp = Convert.ToDecimal(d3.Rows[0][3].ToString());

                        bool h = false; int GetRow = 0;
                        for (int r = 0; r < dataGridView1.Rows.Count; r++)
                        {
                            String gprod = dataGridView1.Rows[r].Cells[0].Value.ToString(); 
                            String gprodsz = dataGridView1.Rows[r].Cells[1].Value.ToString();
                            String lord_num = dataGridView1.Rows[r].Cells[3].Value.ToString(); ;

                            String tvalue = Dbpdname;
                            if (gprod.Equals(tvalue) && Size.Equals(gprodsz) && lord_num.Equals(get_lord_number))
                            {
                                h = true; GetRow = Convert.ToInt32(r);
                            }
                        }

                        decimal dq = Convert.ToDecimal(d3.Rows[0][1].ToString());
                        if (dq > 0)
                        {
                            if (h == false)
                            {
                                dataGridView1.Rows.Add();
                                int nn = dataGridView1.Rows.Count; --nn;
                                dataGridView1.Rows[nn].Cells[0].Value = d3.Rows[0][0].ToString(); dataGridView1.Rows[nn].Cells[1].Value = d3.Rows[0][2].ToString();
                                dataGridView1.Rows[nn].Cells[2].Value = "1";
                                dataGridView1.Rows[nn].Cells[3].Value = d3.Rows[0][4].ToString();
                                dataGridView1.Rows[nn].Cells[4].Value = d3.Rows[0][5].ToString();
                                dataGridView1.Rows[nn].Cells[5].Value = d3.Rows[0][6].ToString();
                                txtscan.Text = "";
                            }
                            else
                            {
                                decimal gq1 = Convert.ToDecimal(dataGridView1.Rows[GetRow].Cells[2].Value); gq1++;
                                decimal mno = dq - gq1;
                                String fquant = mno.ToString();
                                if (fquant.StartsWith("-"))
                                {
                                    MessageBox.Show("Available Quantity : " + dq, "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); txtscan.Text = "";
                                }
                                else
                                {
                                    dataGridView1.Rows[GetRow].Cells[2].Value = gq1.ToString();  txtscan.Text = "";
                                }
                                txtscan.Text = "";
                            }
                            //    SearchProduct(txtscan.Text, "Textbox");
                        }
                        else { MessageBox.Show("Quantity Not available", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
                    }
                    else
                    {
                        // MessageBox.Show("Product not available", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); txtscan.Text = "";
                    }
                    #endregion r3
                }
                else
                {
                    MessageBox.Show("Select Lord No ","Stop",MessageBoxButtons.OK,MessageBoxIcon.Stop);txtscan.Text = "";
                }

            }
            catch { }
        }

        private void btnaddrow_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtwarehouse.Text.Equals("") && !txtlod.Text.Equals("")) {

                    String g1 = "select * from add_product_stock where LordNumber= '" + txtlod.Text + "' and WarehouseName = '" + txtwarehouse.Text + "' and Status='Active' and  Quantity!=0";
                    DataTable d1 = clsDataLayer.RetreiveQuery(g1);
                    if (d1.Rows.Count > 0)
                    {
                        dataGridView1.Rows.Add(); int n = dataGridView1.Rows.Count; int mp = n - 1;  dataGridView1.Rows[mp].Cells[3].Value = txtlod.Text;
                    }
                    else
                    {
                        MessageBox.Show("Product not found!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }


                }
                else
                {
                    MessageBox.Show("Select First Warehouse & PurchaseOrder!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }
         
        private void txtwarehouse_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!txtwarehouse.Text.Equals(""))
                { 
                    String g1 = "select * from add_product_stock where WarehouseName = '" + txtwarehouse.Text + "' and Status='Active' and  Quantity!=0"; DataTable d1 = clsDataLayer.RetreiveQuery(g1);
                    if (d1.Rows.Count > 0)
                    {
                        //
                        String hname = "select LordNumber from add_product_stock where WarehouseName='" + txtwarehouse.Text + "' and Status='Active' and  Quantity!=0"; DataTable df = clsDataLayer.RetreiveQuery(hname);
                        if (df.Rows.Count > 0)
                        {
                            clsGeneral.SetAutoCompleteTextBox(txtlod, df);
                        }
                        //

                    }
                    else
                    { MessageBox.Show("Warehouse not found.!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); txtwarehouse.Text = ""; }
                }
            }
            catch { }
        }

        private void btnuses_Click(object sender, EventArgs e)
        {
            DeliveryProductUses dpu = new DeliveryProductUses(this); dpu.txtcode.Text = txtsale.Text; dpu.Show();
        }

        private void dataGridView1_Leave(object sender, EventArgs e)
        {
            decimal mq = 0;
            for (int a = 0; a < dataGridView1.Rows.Count; a++)
            {
                mq += Convert.ToDecimal(dataGridView1.Rows[a].Cells[2].Value);
            }
            label12.Text = mq.ToString();
        }
 
        //LedgerUpdate Closed
       }
    }
                                                                                               