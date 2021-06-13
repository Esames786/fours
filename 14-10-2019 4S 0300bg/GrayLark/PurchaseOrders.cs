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
using System.Configuration;
using GrayLark.bin.Debug.Report;
using System.Net;
using System.Net.Mail;

namespace GrayLark
{
    public partial class PurchaseOrders : Form
    {
        String Code = ""; TextBox tb = new TextBox(); String Global = "";
        String UID = Login.UserID;
        int same = 0; decimal fexp8 = 0;
        public string GreenSignal = "";
        public string FormID = "";
        decimal old = 0;

        DateTimePicker dt785 = new DateTimePicker(); Rectangle rd;
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        public PurchaseOrders()
        {
            InitializeComponent();
            clsGeneral.SetAutoCompleteTextBox(txtname, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 14 and HeaderActCode='20101' Order BY ID DESC"));
            Disable(); btnNew.Focus(); this.KeyPreview = true; LoadSearching(); btnselectinquiry.Enabled = false;
            btnFirst.Enabled = true; btnLast.Enabled = true; btnNext.Enabled = true; btnPrevious.Enabled = true;
            //
            dataGridView1.Controls.Add(dt785); dt785.Visible = false; dt785.Format = DateTimePickerFormat.Custom; dt785.CustomFormat = "dd-MM-yyyy";
            dt785.TextChanged += new EventHandler(Dt785_TextChanged);
        }

        private void Dt785_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell.Value = dt785.Text.ToString();

            if (dataGridView1.CurrentCell.ColumnIndex == 9)
            {

                int mn = dataGridView1.Rows.Count; mn--;
                int m3 = dataGridView1.CurrentCell.RowIndex;
                if (mn == m3)
                {
                    same = 1;

                    int yhs = dataGridView1.CurrentCell.RowIndex;
                    int n4 = Convert.ToInt32(dataGridView1.Rows[yhs].Cells[8].Value);
                    dataGridView1.CurrentCell = dataGridView1.Rows[yhs].Cells[10];
                    dataGridView1.BeginEdit(true);


                }
            }
            else if (dataGridView1.CurrentCell.ColumnIndex == 10)
            {


                int mn = dataGridView1.Rows.Count; mn--;
                int m3 = dataGridView1.CurrentCell.RowIndex;
                if (mn == m3)
                {
                    same = 1;
                    dataGridView1.Rows.Add();
                    int yhs = dataGridView1.CurrentCell.RowIndex;
                    int n4 = Convert.ToInt32(dataGridView1.Rows[yhs].Cells[8].Value);
                    dataGridView1.CurrentCell = dataGridView1.Rows[yhs].Cells[0];
                    dataGridView1.BeginEdit(true);
                    dataGridView1.Rows[yhs + 1].Cells[0].Value = txtcode.Text;

                }
            }

        }

        private void LoadSearching()
        {
            try
            {
                String g1 = "select distinct Price from tbl_Purchasedt"; DataTable d1 = clsDataLayer.RetreiveQuery(g1); if (d1.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txtprice, d1); }
                String g2 = "select distinct Currency from tbl_Purchasedt"; DataTable d2 = clsDataLayer.RetreiveQuery(g2); if (d2.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txtcurrency, d2); }
                String g3 = "select distinct Payment from tbl_Purchasedt"; DataTable d3 = clsDataLayer.RetreiveQuery(g1); if (d3.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txtpayment, d3); }
                String g4 = "select distinct Delivery from tbl_Purchasedt"; DataTable d4 = clsDataLayer.RetreiveQuery(g1); if (d4.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txtdelivery, d4); }
                String g5 = "select distinct Partialshipment from tbl_Purchasedt"; DataTable d5 = clsDataLayer.RetreiveQuery(g1); if (d5.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txtshipment, d5); }
                String g6 = "select distinct Packing from tbl_Purchasedt"; DataTable d6 = clsDataLayer.RetreiveQuery(g1); if (d6.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txtpacking, d6); }
                String g7 = "select distinct ShelfLife from tbl_Purchasedt"; DataTable d7 = clsDataLayer.RetreiveQuery(g7); if (d7.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txtshelf, d7); }

            }
            catch
            { }
        }

        private string IID;
        public string passIID
        {
            get { return IID; }
            set { IID = value; }
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
            con.Close();
        }

        private bool CheckAllFields()
        {
            bool flag = false;
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
            foreach (Control c in this.tableLayoutPanel5.Controls)
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
            foreach (Control c in this.tableLayoutPanel8.Controls)
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
                for (int j = 0; j < 10; j++)
                {
                    //    if (dgv.Rows[i].Cells[j].Value == null)
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

        private void clear()
        {
            dataGridView1.Rows.Clear();
            foreach (Control c in this.tableLayoutPanel4.Controls)
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

            foreach (Control c in this.tableLayoutPanel5.Controls)
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

            foreach (Control c in this.tableLayoutPanel8.Controls)
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

        private bool Enable()
        {
            bool flag = false;
            dataGridView1.Enabled = true;
            btnFirst.Enabled = false; btnLast.Enabled = false; btnNext.Enabled = false; btnPrevious.Enabled = false;
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
            foreach (Control c in this.tableLayoutPanel5.Controls)
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

            foreach (Control c in this.tableLayoutPanel8.Controls)
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
            dataGridView1.Enabled = false;
            btnFirst.Enabled = true; btnLast.Enabled = true; btnNext.Enabled = true; btnPrevious.Enabled = true;
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
            }

            foreach (Control c in this.tableLayoutPanel5.Controls)
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

            foreach (Control c in this.tableLayoutPanel8.Controls)
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

        private bool Clears2()
        {
            bool flag = false;
            txtwarehouse.Text = ""; txtpinv.Text = ""; dataGridView1.Rows.Clear();
            dataGridView1.Enabled = true;
            foreach (Control c in this.tableLayoutPanel4.Controls)
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
            foreach (Control c in this.tableLayoutPanel8.Controls)
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

        private bool tblpnl8()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel8.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text = "-";
                    flag = true;

                }
                else if (c is ComboBox)
                {

                    ((ComboBox)c).Text = "-";
                    flag = true;

                }
            }

            foreach (Control c in this.tableLayoutPanel7.Controls)
            {
                if (c is TextBox)
                {

                    ((TextBox)c).Text = "-";
                    flag = true;
                }
                else if (c is ComboBox)
                {

                    ((ComboBox)c).Text = "-";
                    flag = true;
                }
            }
            return flag;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                FormID = "Add";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    Clears2();
                    Enable();
                    btnSave.Enabled = true;
                    btnUpdate.Enabled = false;
                    btnNew.Enabled = false;
                    btnEdit.Enabled = false;
                    btn_detail.Enabled = true;
                    txtcode.Text = clsGeneral.getMAXCode("Purchase", "PurCode", "PO");
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[0].Cells[0].Value = txtcode.Text;
                    //dataGridView1.Rows[0].Cells[9].Value = "-";
                    //dataGridView1.Rows[0].Cells[10].Value = "-";

                    //txtname.Enabled = false; txtname.ReadOnly = false; txtlord.Enabled = false; txtremarks.Enabled = false; 
                    clsGeneral.SetAutoCompleteTextBox(txtwarehouse, clsDataLayer.RetreiveQuery("select WarehouseName from tbl_warehouse"));
                    clsGeneral.SetAutoCompleteTextBox(txtname, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 14 and HeaderActCode='20101' Order BY ID DESC")); txtname.Focus();
                    txtdiscount.Text = "0"; txttax.Text = "0"; txttransport.Text = "0"; txtpinv.Text = "-"; btnselectinquiry.Enabled = true;
                    txtetax.Text = "0"; txtstamp.Text = "0"; txtcaa.Text = "0"; txtpiac.Text = "0"; txtcae.Text = "0"; txtname.Focus();
                    vstatus.Enabled = false; vstatus.Text = "Pending";
                    //
                    txtprice.Text = "-"; txtcurrency.Text = "-";
                    txtpayment.Text = "-";
                    txtdelivery.Text = "-";
                    txtshipment.Text = "-"; txtpacking.Text = "-";
                    txtshelf.Text = "-";

                }
                else { MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
            }
            catch { }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                FormID = "Edit";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    decimal usedq = 0;

                    old = Convert.ToDecimal(txtcpt.Text);

                    countquant();
                    Enable();
                    txtcode.ReadOnly = false; txtcode.Enabled = true;

                    btnSave.Enabled = false; btnUpdate.Enabled = true;

                    btnNew.Enabled = false; btnEdit.Enabled = false;
                    btn_detail.Enabled = true; txtname.Enabled = false; txtname.ReadOnly = true;
                    //                        Received
                    //Pending
                    if (vstatus.Text.Equals("Pending"))
                    {
                        vstatus.Enabled = true;
                    }
                    else
                    {
                        vstatus.Enabled = false;
                    }
                    #region add
                    decimal a1 = 0;
                    if (!txtetax.Text.Equals(""))
                    {
                        a1 = Convert.ToDecimal(txtetax.Text);
                    }

                    decimal a2 = 0;
                    if (!txtstamp.Text.Equals(""))
                    {
                        a2 = Convert.ToDecimal(txtstamp.Text);
                    }

                    decimal a3 = 0;
                    if (!txtcaa.Text.Equals(""))
                    {
                        a3 = Convert.ToDecimal(txtcaa.Text);
                    }

                    decimal a4 = 0;
                    if (!txtpiac.Text.Equals(""))
                    {
                        a4 = Convert.ToDecimal(txtpiac.Text);
                    }

                    decimal a5 = 0;
                    if (!txtcae.Text.Equals(""))
                    {
                        a5 = Convert.ToDecimal(txtcae.Text);
                    }
                    #endregion add
                    decimal total9 = a1 + a2 + a3 + a4 + a5; fexp8 = total9;

                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }



        private void btnUpdate_Click(object sender, EventArgs e)
        {
            String Odate = "";
            if (!CheckAllFields() && !CheckDataGridCells(dataGridView1))
            {
                try
                {
                    FormID = "Update";
                    UserNametesting();
                    if (GreenSignal == "YES")
                    {
                        try
                        {
                            totalbillandquant();

                            String m1 = "select Date from Purchase where PurCode='" + txtcode.Text + "'"; DataTable m2 = clsDataLayer.RetreiveQuery(m1);
                            if (m2.Rows.Count > 0)
                            {
                                Odate = m2.Rows[0][0].ToString();
                            }

                        }
                        catch { }
                        try
                        {
                            PartyCode(txtname.Text);
                            String getname = "select * from Accounts where ActTitle='" + txtname.Text + "'";
                            DataTable de = clsDataLayer.RetreiveQuery(getname);
                            if (de.Rows.Count < 1)
                            {
                                MessageBox.Show("UserName dont have a chart Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                btnUpdate.Enabled = false; MessageBox.Show("Update Success!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                                string query = "";
                                query = "Delete From Purchase Where PurCode = '" + txtcode.Text + "'";
                                clsDataLayer.ExecuteQuery(query);

                                string query7 = "";
                                query7 = "Delete From tbl_Purchasedt Where Pdcode = '" + txtcode.Text + "'";
                                clsDataLayer.ExecuteQuery(query7);

                                string query9 = "";
                                query9 = "Delete From PurchaseDetail Where PurCode = '" + txtcode.Text + "'";
                                clsDataLayer.ExecuteQuery(query9);

                                String Purchase = @"INSERT INTO Purchase(pstatus,InquiryCode,Warehouse,TotalCPTCost,TransportCost,Remarks,Tax,PaidAmount,RemainingAmount,PurCode,VendorName,VendorCode,Date,BillPrice,UserName,Discount,NetAmount,LordNumber)
     VALUES('" + vstatus.Text + "','" + txtpinv.Text + "','" + txtwarehouse.Text + "'," + txtcpt.Text + "," + txttransport.Text + ",'" + txtremarks.Text + "','" + txttax.Text + "',0," + txtcpt.Text + ",'" + txtcode.Text + "','" + txtname.Text + "','" + Code + "','" + dte.Value.ToString("yyyy-MM-dd") + "','" + txtbamount.Text + "','" + Login.UserID + "','" + txtdiscount.Text + "'," + txtnetamount.Text + ",'"+txt_purchase_no.Text+"')";

                                if (clsDataLayer.ExecuteQuery(Purchase) > 0)
                                {
                                    String ig1 = "insert into tbl_Purchasedt(expone,exptwo,expthree,expfour,expfive,Pdcode,Dates,UserName,Price,Currency,Payment,Delivery,Partialshipment,Packing,ShelfLife)values(" + txtetax.Text + "," + txtstamp.Text + "," + txtcaa.Text + "," + txtpiac.Text + "," + txtcae.Text + ",'" + txtcode.Text + "','" + dte.Value.ToString("yyyy-MM-dd") + "','" + Login.UserID + "','" + txtprice.Text + "','" + txtcurrency.Text + "','" + txtpayment.Text + "','" + txtdelivery.Text + "','" + txtshipment.Text + "','" + txtpacking.Text + "','" + txtshelf.Text + "')"; clsDataLayer.ExecuteQuery(ig1);
                                    String upd = "update PurchaseInq set VStatus='Purchase' where PIurCode='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(upd);
                                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                                    {
                                        String purchase_order_no = dataGridView1.Rows[i].Cells[0].Value.ToString();
                                        String product_name = dataGridView1.Rows[i].Cells[3].Value.ToString();
                                        Decimal price_per_product = Convert.ToDecimal(dataGridView1.Rows[i].Cells[7].Value.ToString());
                                        String size = dataGridView1.Rows[i].Cells[4].Value.ToString();
                                        Decimal Quantity = Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value.ToString());
                                        Decimal Total_price = Convert.ToDecimal(dataGridView1.Rows[i].Cells[8].Value.ToString());
                                        decimal pquant = 0;
                                        String Purq = "select Quantity from PurchaseDetail Where Size='" + dataGridView1.Rows[i].Cells[4].Value.ToString() + "' and CATEGORY='" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "' and PurchaseItem ='" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "' and PurCode='" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "'";
                                        DataTable d1 = clsDataLayer.RetreiveQuery(Purq);
                                        if (d1.Rows.Count > 0)
                                        {
                                            pquant = Convert.ToDecimal(d1.Rows[0][0].ToString());
                                        }
                                        else { pquant = 0; }
                                        if (vstatus.Text.Equals("Pending"))
                                        {

                                        }
                                        else
                                        {
                                            if (vstatus.Text.Equals("Pending"))
                                            {

                                            }
                                            else
                                            {
                                                String plot = dataGridView1.Rows[i].Cells[1].Value.ToString();
                                                String del5 = "delete from add_product_stock where LordNumber='" + plot + "' and CATEGORY='" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "' and Size='" + dataGridView1.Rows[i].Cells[4].Value.ToString() + "' and NAME='" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "'"; clsDataLayer.ExecuteQuery(del5);
                                            }
                                            String pgcode = ""; String pgqtype = ""; decimal m2sp = 0;
                                            String abcd = "select productcode,SELL_PRICE from add_product where CATEGORY='" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "' and Size='" + dataGridView1.Rows[i].Cells[4].Value.ToString() + "' and NAME='" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "'";
                                            DataTable dv = clsDataLayer.RetreiveQuery(abcd);
                                            if (dv.Rows.Count > 0)
                                            {
                                                pgcode = dv.Rows[0][0].ToString(); m2sp = Convert.ToDecimal(dv.Rows[0][1].ToString());
                                            }
                                            String mcode = clsGeneral.getMAXCode("add_product_stock", "PRODUCT_ID", "PS");
                                            String q2 = "insert into add_product_stock(SellPrice,Date,UserName,productcode,LordNumber,Status,PRODUCT_ID,CATEGORY,NAME,Size,Quantity,WarehouseName,Mfgdate,Expdate,Referencecode)values (" + m2sp + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Login.UserID + "','" + pgcode + "','" + dataGridView1.Rows[i].Cells[1].Value + "','Active','" + mcode + "','" + dataGridView1.Rows[i].Cells[2].Value + "','" + dataGridView1.Rows[i].Cells[3].Value + "','" + dataGridView1.Rows[i].Cells[4].Value + "'," + dataGridView1.Rows[i].Cells[6].Value + ",'" + txtwarehouse.Text + "','" + dataGridView1.Rows[i].Cells[9].Value + "','" + dataGridView1.Rows[i].Cells[10].Value + "','" + dataGridView1.Rows[i].Cells[5].Value + "')";
                                            clsDataLayer.ExecuteQuery(q2);
                                        }
                                        String PurchaseDetail = @"INSERT INTO PurchaseDetail(LordNumber,Reference,PurCode,CATEGORY,PurchaseItem,Size,PricePerItem,Quantity,TotalPriceItem,Mfgdate,Expdate)
     VALUES ('" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[5].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[4].Value.ToString() + "'," + dataGridView1.Rows[i].Cells[7].Value.ToString() + "," + dataGridView1.Rows[i].Cells[6].Value.ToString() + "," + dataGridView1.Rows[i].Cells[8].Value.ToString() + ",'" + dataGridView1.Rows[i].Cells[9].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[10].Value.ToString() + "')";

                                        if (clsDataLayer.ExecuteQuery(PurchaseDetail) > 0) { }
                                    }

                                    // balanceequal();
                                    PartyCode(txtname.Text); PaymentUpdateDue(txtname.Text);
                                    int index = GetRowIndex(txtcode.Text); DataTable dt = SetDT();
                                    ShowDt(dt); Console.WriteLine("Index=" + index);
                                    DeleteRecord(); dt.Rows[index][7] = Convert.ToDecimal(txtcpt.Text);
                                    dt.Rows[index + 1][8] = Convert.ToDecimal(txtcpt.Text); dt = SomeOperation(dt); InsertRecord(dt);
                                    expupdate(); Disable(); btnUpdate.Enabled = false; btnNew.Enabled = true;
                                    btnEdit.Enabled = true; Clears2(); MessageBox.Show("Update Success!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                catch { }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                dsa();
                FormID = "Print";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    if (txtcode.Text != "")
                    {
                        String qu = "select * from VU_Purchase where PurCode = '" + txtcode.Text + "' ";
                        DataTable ds = clsDataLayer.RetreiveQuery(qu);
                        if (ds.Rows.Count > 0)
                        {
                            PurchaseOrder pur = new PurchaseOrder();
                            pur.SetDataSource(ds);
                            rptPurchaseOrderview pop = new rptPurchaseOrderview(pur, txtcode.Text);
                            pop.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select Purchase ID", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }
        decimal balance = 0;
        public decimal PartyBalance()
        {
            try
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
            catch { }
            return balance;

        }
        private void PartyCode(String dev)
        {
            String sel = "select ActCode from Accounts where ActTitle = '" + dev + "'";
            DataTable dc = clsDataLayer.RetreiveQuery(sel);
            if (dc.Rows.Count > 0)
            {
                Code = dc.Rows[0][0].ToString();
            }
        }
        private void PaymentDue(String pty)
        {

            String rec = "select * from PaymentDue where PartyCode = '" + Code + "'";
            DataTable d = clsDataLayer.RetreiveQuery(rec);
            if (d.Rows.Count > 0)
            {
                decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
                decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
                decimal received = decimal.Parse(d.Rows[0]["PaidAmount"].ToString());
                decimal bill = decimal.Parse(txtcpt.Text);
                total += bill;
                due += bill;
                string updateblnc = "update PaymentDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                clsDataLayer.ExecuteQuery(updateblnc);
            }
            else
            {
                String ii = @"insert into PaymentDue (PayCode,PartyName,PartyCode,TotalAmount,DueAmount,PaidAmount,Company_Name)
                values('" + txtcode.Text + "','" + pty + "','" + Code + "'," + txtcpt.Text + "," + txtcpt.Text + ",0,'4S')";
                clsDataLayer.ExecuteQuery(ii);
            }
        }
        private void PaymentUpdateDue(String pty)
        {
            PartyCode(pty);
            String rec = "select * from PaymentDue where PartyCode = '" + Code + "'";
            DataTable d = clsDataLayer.RetreiveQuery(rec);
            if (d.Rows.Count > 0)
            {
                decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
                decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
                decimal received = decimal.Parse(d.Rows[0]["PaidAmount"].ToString());
                decimal bill = decimal.Parse(txtcpt.Text);
                total = (total - old) + bill;
                due = (due - old) + bill;

                string updateblnc = "update PaymentDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                clsDataLayer.ExecuteQuery(updateblnc);
            }
            else
            {
                String ii = @"insert into PaymentDue (PayCode,PartyName,PartyCode,TotalAmount,DueAmount,PaidAmount,Company_Name)
                values('" + txtcode.Text + "','" + pty + "','" + Code + "'," + txtcpt.Text + "," + txtcpt.Text + ",0,'4S')";
                clsDataLayer.ExecuteQuery(ii);
            }
        }

        bool pcs = true;
        private void CheckProd()
        {
            pcs = true;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                String pd = dataGridView1.Rows[i].Cells[3].Value.ToString(); String cg = dataGridView1.Rows[i].Cells[2].Value.ToString();
                String ps = dataGridView1.Rows[i].Cells[4].Value.ToString();
                String Db = "select * from add_product where CATEGORY='" + cg + "' and Size='" + ps + "' and NAME = '" + pd + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(Db); if (d1.Rows.Count > 0)
                { }
                else { pcs = false; }
            }
        }

        private void exp()
        {
            #region add
            decimal a1 = 0;
            if (!txtetax.Text.Equals(""))
            {
                a1 = Convert.ToDecimal(txtetax.Text);
            }

            decimal a2 = 0;
            if (!txtstamp.Text.Equals(""))
            {
                a2 = Convert.ToDecimal(txtstamp.Text);
            }

            decimal a3 = 0;
            if (!txtcaa.Text.Equals(""))
            {
                a3 = Convert.ToDecimal(txtcaa.Text);
            }

            decimal a4 = 0;
            if (!txtpiac.Text.Equals(""))
            {
                a4 = Convert.ToDecimal(txtpiac.Text);
            }

            decimal a5 = 0;
            if (!txtcae.Text.Equals(""))
            {
                a5 = Convert.ToDecimal(txtcae.Text);
            }
            #endregion add
            decimal total9 = a1 + a2 + a3 + a4 + a5;
            PartyCode("Purchase Expense");
            Decimal a = PartyBalance();
            Decimal b = total9;
            Decimal c = a + b;
            String ins = @"insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                         " Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) values('Purchase','" + txtcode.Text + "','" + dte.Value.ToString("yyyy-MM-dd") + "','01010209 ','" + Code + "','" + txtcode.Text + "','Expenses'," + total9 + ",0.00,'OnPayabale ' ," + c + ",'4S')  insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                         " values('Purchase','" + txtcode.Text + "','" + dte.Value.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtcode.Text + "','Purchase Expense',0.00," + total9 + ",'OnPayabale '," + c + ",'4S') ";
            clsDataLayer.ExecuteQuery(ins);
            //
            String rec = "select * from PaymentDue where PartyCode = '" + Code + "'";
            DataTable d = clsDataLayer.RetreiveQuery(rec);
            if (d.Rows.Count > 0)
            {
                decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
                decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
                decimal received = decimal.Parse(d.Rows[0]["PaidAmount"].ToString());
                decimal bill = total9;
                total += bill;
                due += bill;
                string updateblnc = "update PaymentDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                clsDataLayer.ExecuteQuery(updateblnc);
            }
            else
            {
                String ii = @"insert into PaymentDue (PayCode,PartyName,PartyCode,TotalAmount,DueAmount,PaidAmount,Company_Name)
                values('" + txtcode.Text + "','Purchase Expense','" + Code + "'," + total9 + "," + total9 + ",0,'4S')";
                clsDataLayer.ExecuteQuery(ii);
            }
        }

        private void expupdate()
        {
            #region add
            decimal a1 = 0;
            if (!txtetax.Text.Equals(""))
            {
                a1 = Convert.ToDecimal(txtetax.Text);
            }

            decimal a2 = 0;
            if (!txtstamp.Text.Equals(""))
            {
                a2 = Convert.ToDecimal(txtstamp.Text);
            }

            decimal a3 = 0;
            if (!txtcaa.Text.Equals(""))
            {
                a3 = Convert.ToDecimal(txtcaa.Text);
            }

            decimal a4 = 0;
            if (!txtpiac.Text.Equals(""))
            {
                a4 = Convert.ToDecimal(txtpiac.Text);
            }

            decimal a5 = 0;
            if (!txtcae.Text.Equals(""))
            {
                a5 = Convert.ToDecimal(txtcae.Text);
            }
            #endregion add
            decimal total9 = a1 + a2 + a3 + a4 + a5;
            PartyCode("Purchase Expense");
            int index = GetRowIndex1(txtcode.Text);
            DataTable dt1 = SetDT1();
            ShowDt1(dt1);
            Console.WriteLine("Index=" + index);
            DeleteRecord1();
            dt1.Rows[index][7] = total9;
            dt1.Rows[index + 1][8] = total9;
            dt1 = SomeOperation1(dt1); InsertRecord1(dt1);

            String rec = "select * from PaymentDue where PartyCode = '" + Code + "'";
            DataTable d = clsDataLayer.RetreiveQuery(rec);
            if (d.Rows.Count > 0)
            {
                decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
                decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
                decimal received = decimal.Parse(d.Rows[0]["PaidAmount"].ToString());
                decimal bill = total9;
                total = (total - fexp8) + bill;
                due = (due - fexp8) + bill;

                string updateblnc = "update PaymentDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                clsDataLayer.ExecuteQuery(updateblnc);
            }
            else
            {
                String ii = @"insert into PaymentDue (PayCode,PartyName,PartyCode,TotalAmount,DueAmount,PaidAmount,Company_Name)
                values('" + txtcode.Text + "','Purchase Expense','" + Code + "'," + txtcpt.Text + "," + txtcpt.Text + ",0,'4S')";
                clsDataLayer.ExecuteQuery(ii);
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
                    Subject = "Pharmacy Software Make Purchase Voucher " + DateTime.Now.ToString("dd-MM-yyyy HH:MM"),
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
                //
                FormID = "Save";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    if (!CheckAllFields() && !CheckDataGridCells(dataGridView1))
                    {
                        PartyCode(txtname.Text);
                        String getname = "select * from Accounts where ActTitle='" + txtname.Text + "'";
                        DataTable de = clsDataLayer.RetreiveQuery(getname);
                        if (de.Rows.Count < 1)
                        {
                            MessageBox.Show("UserName dont have a chart Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            CheckProd();
                            if (pcs == true)
                            {
                                exp();
                                PartyCode(txtname.Text);

                                btnSave.Enabled = false; btnNew.Enabled = true; txtcode.Text = clsGeneral.getMAXCode("Purchase", "PurCode", "PO");
                                totalbillandquant();
                                String Purchase = @"INSERT INTO Purchase(pstatus,InquiryCode,Warehouse,TotalCPTCost,TransportCost,Remarks,Tax,PaidAmount,RemainingAmount,PurCode,VendorName,VendorCode,Date,BillPrice,UserName,Discount,NetAmount,LordNumber)
     VALUES('" + vstatus.Text + "','" + txtpinv.Text + "','" + txtwarehouse.Text + "'," + txtcpt.Text + "," + txttransport.Text + ",'" + txtremarks.Text + "','" + txttax.Text + "',0," + txtcpt.Text + ",'" + txtcode.Text + "','" + txtname.Text + "','" + Code + "','" + dte.Value.ToString("yyyy-MM-dd") + "','" + txtbamount.Text + "','" + Login.UserID + "','" + txtdiscount.Text + "'," + txtnetamount.Text + ",'"+txt_purchase_no.Text+"')";

                                if (clsDataLayer.ExecuteQuery(Purchase) > 0)
                                {
                                    String ig1 = "insert into tbl_Purchasedt(expone,exptwo,expthree,expfour,expfive,Pdcode,Dates,UserName,Price,Currency,Payment,Delivery,Partialshipment,Packing,ShelfLife)values(" + txtetax.Text + "," + txtstamp.Text + "," + txtcaa.Text + "," + txtpiac.Text + "," + txtcae.Text + ",'" + txtcode.Text + "','" + dte.Value.ToString("yyyy-MM-dd") + "','" + Login.UserID + "','" + txtprice.Text + "','" + txtcurrency.Text + "','" + txtpayment.Text + "','" + txtdelivery.Text + "','" + txtshipment.Text + "','" + txtpacking.Text + "','" + txtshelf.Text + "')"; clsDataLayer.ExecuteQuery(ig1);

                                    String upd = "update PurchaseInq set VStatus='Purchase' where PIurCode='" + txtpinv.Text + "'"; clsDataLayer.ExecuteQuery(upd);
                                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                                    {
                                        String purchase_order_no = dataGridView1.Rows[i].Cells[0].Value.ToString();


                                        String PurchaseDetail = @"INSERT INTO PurchaseDetail(LordNumber,Reference,PurCode,CATEGORY,PurchaseItem,Size,PricePerItem,Quantity,TotalPriceItem,Mfgdate,Expdate)
     VALUES ('" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[5].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[4].Value.ToString() + "'," + dataGridView1.Rows[i].Cells[7].Value.ToString() + "," + dataGridView1.Rows[i].Cells[6].Value.ToString() + "," + dataGridView1.Rows[i].Cells[8].Value.ToString() + ",'" + dataGridView1.Rows[i].Cells[9].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[10].Value.ToString() + "')";

                                        if (clsDataLayer.ExecuteQuery(PurchaseDetail) > 0)
                                        {
                                            if (vstatus.Text.Equals("Pending"))
                                            {

                                            }
                                            else
                                            {
                                                String pgcode = ""; String pgqtype = ""; decimal m2sp = 0;
                                                String abcd = "select productcode,SELL_PRICE from add_product where CATEGORY='" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "' and Size='" + dataGridView1.Rows[i].Cells[4].Value.ToString() + "' and NAME='" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "'";
                                                DataTable dv = clsDataLayer.RetreiveQuery(abcd);
                                                if (dv.Rows.Count > 0)
                                                {
                                                    pgcode = dv.Rows[0][0].ToString(); m2sp = Convert.ToDecimal(dv.Rows[0][1].ToString());
                                                }
                                                String mcode = clsGeneral.getMAXCode("add_product_stock", "PRODUCT_ID", "PS");
                                                String q2 = "insert into add_product_stock(SellPrice,Date,UserName,productcode,LordNumber,Status,PRODUCT_ID,CATEGORY,NAME,Size,Quantity,WarehouseName,Mfgdate,Expdate,Referencecode)values (" + m2sp + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Login.UserID + "','" + pgcode + "','" + dataGridView1.Rows[i].Cells[1].Value + "','Active','" + mcode + "','" + dataGridView1.Rows[i].Cells[2].Value + "','" + dataGridView1.Rows[i].Cells[3].Value + "','" + dataGridView1.Rows[i].Cells[4].Value + "'," + dataGridView1.Rows[i].Cells[6].Value + ",'" + txtwarehouse.Text + "','" + dataGridView1.Rows[i].Cells[9].Value + "','" + dataGridView1.Rows[i].Cells[10].Value + "','" + dataGridView1.Rows[i].Cells[5].Value + "')";
                                                clsDataLayer.ExecuteQuery(q2);
                                            }

                                        }
                                    }
                                    Decimal a = PartyBalance();
                                    Decimal b = Convert.ToDecimal(txtcpt.Text);
                                    Decimal c = a + b;
                                    String ins = @"insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                                                 " Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) values('Purchase','" + txtcode.Text + "','" + dte.Value.ToString("yyyy-MM-dd") + "','01010204 ','" + Code + "','" + txtcode.Text + "','Purchase'," + txtcpt.Text + ",0.00,'OnPayabale ' ," + c + ",'4S')  insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                                                 " values('Purchase','" + txtcode.Text + "','" + dte.Value.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtcode.Text + "','" + txtname.Text + "',0.00," + txtcpt.Text + ",'OnPayabale '," + c + ",'4S') ";

                                    clsDataLayer.ExecuteQuery(ins); PartyCode(txtname.Text);
                                    PaymentDue(txtname.Text);
                                    MessageBox.Show("Saved ", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    Clears2();
                                    Disable();
                                    btnNew.Enabled = true;
                                    btnEdit.Enabled = true;
                                    btnSave.Enabled = false;
                                    btnUpdate.Enabled = false;
                                    btn_detail.Enabled = true;
                                }

                            }
                            else
                            {
                                MessageBox.Show("Product Name is Incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
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

        private void txtcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }

        }

        private void txtname_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void PurchaseOrders_Load(object sender, EventArgs e)
        {
            btn_detail.Enabled = true;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        decimal namt = 0;
        public void countquant()
        {
            namt = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[6].Value != null)
                {
                    namt = namt + Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value.ToString());
                }
            }
            //this.txtQuantity.Text = namt.ToString();
        }



        private void totalbillandquant()
        {
            try
            {
                int namt = 0;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[6].Value != null)
                    {
                        namt = namt + int.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString());
                    }
                }
                this._quantity.Text = namt.ToString();
            }
            catch (Exception ex)
            {
                string name = ex.Message;
            }

            //Total Amount 

            try
            {
                decimal namt = 0;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[8].Value != null)
                    {

                        namt = namt + Convert.ToDecimal(dataGridView1.Rows[i].Cells[8].Value.ToString());
                    }
                }
                this.txtbamount.Text = namt.ToString();
            }
            catch (Exception ex)
            {
                string name = ex.Message;
            }
        }

        private void restric()
        {
            #region ColumnIndex3
            String psize = dataGridView1.CurrentRow.Cells[4].Value.ToString(); bool match = false;
            String s1 = "select Size from add_product where Size='" + psize + "'"; DataTable ds1 = clsDataLayer.RetreiveQuery(s1);
            if (ds1.Rows.Count > 0)
            {
                String pcateg = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                String pname = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                int nn = 0;
                for (int a = 0; a < dataGridView1.Rows.Count; a++)
                {
                    String Gpcateg = dataGridView1.Rows[a].Cells[2].Value.ToString();
                    String Gpname = dataGridView1.Rows[a].Cells[3].Value.ToString();
                    String Gpsize = dataGridView1.Rows[a].Cells[4].Value.ToString();
                    if (Gpcateg.Equals("") || Gpname.Equals("") || Gpsize.Equals(""))
                    {

                    }
                    else if (pcateg.Equals(Gpcateg) && pname.Equals(Gpname) && psize.Equals(Gpsize))
                    {
                        nn++;
                    }
                }
                if (nn > 1)
                {
                    MessageBox.Show("Same Product Cant Add Multiple Time", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); match = true;
                }
                if (match == false)
                {
                    String fetch2 = "select Referencecode,PURCHASE_PRICE from add_product where CATEGORY='" + pcateg + "' and NAME='" + pname + "' and Size='" + psize + "'"; DataTable df3 = clsDataLayer.RetreiveQuery(fetch2);
                    if (df3.Rows.Count > 0)
                    {
                        String fref = df3.Rows[0][0].ToString(); dataGridView1.CurrentRow.Cells[5].Value = fref;
                        String fprice = df3.Rows[0][1].ToString(); dataGridView1.CurrentRow.Cells[7].Value = fprice;
                    }
                }
                else
                {
                    dataGridView1.CurrentRow.Cells[3].Value = ""; dataGridView1.CurrentRow.Cells[4].Value = ""; dataGridView1.CurrentRow.Cells[6].Value = "";
                }
            }
            else
            {
                MessageBox.Show("Selected Size is not Found!!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); match = true;
                dataGridView1.CurrentRow.Cells[4].Value = "";
            }
            #endregion ColumnIndex3
        }

        private void GridRef()
        {
            try
            {
                decimal Quantity = 0;
                decimal Price = 0;

                // Calculation for New Product
                if (dataGridView1.CurrentRow.Cells["Quantity"].Value != null && dataGridView1.CurrentRow.Cells["Quantity"].Value.ToString().Trim() != null)
                {
                    Quantity = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Quantity"].Value);
                }
                if (dataGridView1.CurrentRow.Cells[7].Value != null && dataGridView1.CurrentRow.Cells[7].Value.ToString().Trim() != null)
                {
                    Price = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[7].Value);
                }
                decimal Now = Quantity * Price;
                dataGridView1.CurrentRow.Cells[8].Value = Now.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // Total Quantity _quantity
            totalbillandquant();
        }


        private void btnclear_Click(object sender, EventArgs e)
        {
            Clears2();
            Disable();
            btnNew.Enabled = true;
            btnEdit.Enabled = true;
            btnSave.Enabled = false;
            btnUpdate.Enabled = false;
            // btn_detail.Enabled = false;

            txtname.Text = "Select";


        }

        private void txtpname_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void txtpname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-') && (e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void txtbamount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String hm = txtdiscount.Text; decimal d1 = 0;
                if (hm.Equals("")) { hm = "0"; d1 = 0; } else { d1 = Convert.ToDecimal(txtdiscount.Text); }
                String th = hm.Substring(hm.Length - 1);
                decimal bill = Convert.ToDecimal(txtbamount.Text);
                if (th.Equals("%"))
                {
                    String th1 = hm.Substring(0, hm.Length - 1);
                    decimal dis = Convert.ToDecimal(th1);
                    decimal final = bill * dis / 100;
                    decimal f1 = bill - final;
                    txtnetamount.Text = f1.ToString();
                }
                else
                {
                    decimal f2 = bill - d1;
                    txtnetamount.Text = f2.ToString();
                }
            }
            catch { }
        }

        private void txtname_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }
        }


        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btn_detail_Click(object sender, EventArgs e)
        {
            Global = "Search";
            Purchase_Detail frm = new Purchase_Detail("");
            frm.ShowDialog();
            this.Hide();
        }

        String hn = "";
        private void txtcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //
                String get44 = "select * from PurchaseDetail where PurCode='" + txtcode.Text + "'"; DataTable d18 = clsDataLayer.RetreiveQuery(get44);
                if (d18.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                    foreach (DataRow item in d18.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = item["PurCode"].ToString();
                        dataGridView1.Rows[n].Cells[1].Value = item["LordNumber"].ToString();
                        dataGridView1.Rows[n].Cells[2].Value = item["CATEGORY"].ToString();
                        dataGridView1.Rows[n].Cells[3].Value = item["PurchaseItem"].ToString();
                        dataGridView1.Rows[n].Cells[4].Value = item["Size"].ToString();
                        dataGridView1.Rows[n].Cells[5].Value = item["Reference"].ToString();
                        dataGridView1.Rows[n].Cells[6].Value = item["Quantity"].ToString();
                        dataGridView1.Rows[n].Cells[7].Value = item["PricePerItem"].ToString();
                        dataGridView1.Rows[n].Cells[8].Value = item["TotalPriceItem"].ToString();
                        dataGridView1.Rows[n].Cells[9].Value = item["Mfgdate"].ToString();
                        dataGridView1.Rows[n].Cells[10].Value = item["Expdate"].ToString();
                    }
                }

                String get = "select * from Purchase where PurCode='" + txtcode.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(get);
                if (d1.Rows.Count > 0)
                {
                    hn = txtcode.Text;
                    txt_purchase_no.Text = d1.Rows[0]["LordNumber"].ToString();
                    txtcode.Text = hn;
                    String q = @"select * from VU_Purchase where PurCode='" + txtcode.Text + "'";
                    DataTable de = clsDataLayer.RetreiveQuery(q);
                    if (de.Rows.Count > 0)
                    {
                        hn = txtcode.Text;
                        vstatus.Text = de.Rows[0]["pstatus"].ToString();
                        txtetax.Text = de.Rows[0]["expone"].ToString();
                        txtstamp.Text = de.Rows[0]["exptwo"].ToString();
                        txtcaa.Text = de.Rows[0]["expthree"].ToString();
                        txtpiac.Text = de.Rows[0]["expfour"].ToString();
                        txtcae.Text = de.Rows[0]["expfive"].ToString();

                        txtname.Text = de.Rows[0]["VendorName"].ToString();
                        txttax.Text = de.Rows[0]["Tax"].ToString();
                        txtbamount.Text = de.Rows[0]["BillPrice"].ToString();
                        txtdiscount.Text = de.Rows[0]["Discount"].ToString();
                        txtnetamount.Text = de.Rows[0]["NetAmount"].ToString();
                        txtremarks.Text = de.Rows[0]["Remarks"].ToString();


                        txtprice.Text = de.Rows[0]["Price"].ToString();
                        txtcurrency.Text = de.Rows[0]["Currency"].ToString();
                        txtpayment.Text = de.Rows[0]["Payment"].ToString();
                        txtdelivery.Text = de.Rows[0]["Delivery"].ToString();
                        txtpacking.Text = de.Rows[0]["Packing"].ToString();
                        txtshipment.Text = de.Rows[0]["Partialshipment"].ToString();
                        txtshelf.Text = de.Rows[0]["ShelfLife"].ToString();

                        txtwarehouse.Text = de.Rows[0]["Warehouse"].ToString();
                        txttransport.Text = de.Rows[0]["TransportCost"].ToString();
                        txtcpt.Text = de.Rows[0]["TotalCPTCost"].ToString();
                        txtpinv.Text = de.Rows[0]["InquiryCode"].ToString();

                        dte.Text = de.Rows[0]["Date"].ToString(); totalbillandquant();
                    }
                    else { }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //
            try
            {
                int yy = dataGridView1.CurrentCell.ColumnIndex;
                string columnHeaders = dataGridView1.Columns[yy].HeaderText;
                if (columnHeaders.Equals("Product Name"))
                {
                    tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        String hname = "";

                        string chk = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                        if (txtname.Text.Equals(""))
                        {
                            hname = "select NAME from add_product";
                        }
                        else { hname = "select NAME from add_product where principleName='" + txtname.Text + "' and [CATEGORY] = '" + chk + "'"; }
                        DataTable df = clsDataLayer.RetreiveQuery(hname);
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
                        String hname = "";

                        string chk = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                        string chk1 = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                        if (txtname.Text.Equals(""))
                        {
                            hname = "select Size from add_product";
                        }
                        else { hname = "select Size from add_product where principleName='" + txtname.Text + "' and [CATEGORY] = '" + chk + "' and [NAME] = '" + chk1 + "'"; }
                        DataTable df = clsDataLayer.RetreiveQuery(hname);
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
                else if (columnHeaders.Equals("Category"))
                {
                    tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        String hname = "";
                        if (txtname.Text.Equals(""))
                        {
                            hname = "select CATEGORY from add_product";
                        }
                        else { hname = "select CATEGORY from add_product where principleName='" + txtname.Text + "'"; }
                        DataTable df = clsDataLayer.RetreiveQuery(hname);
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
                else if (columnHeaders.Equals("Reference"))
                {
                    #region rm
                    tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        String hname = "";

                        string chk = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                        string chk1 = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                        string chk2 = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                        if (txtname.Text.Equals(""))
                        {
                            hname = "select Referencecode from add_product";
                        }
                        //select Referencecode from add_product where CATEGORY='' and NAME='' and Size=''
                        else { hname = "select Referencecode from add_product where principleName='" + txtname.Text + "' and [CATEGORY] = '" + chk + "' and [NAME] = '" + chk1 + "' and [Size] = '" + chk2 + "'"; }
                        DataTable df = clsDataLayer.RetreiveQuery(hname);
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
                    #endregion rm
                }

                else if (columnHeaders.Equals("MFG Date"))
                {
                    #region r3
                    tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        DataTable dv = new DataTable();
                        dv.Columns.Add("Date");
                        dv.Rows.Add(DateTime.Now.ToString("dd-MM-yyyy"));


                        if (dv.Rows.Count > 0)
                        {
                            AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();

                            foreach (DataRow row in dv.Rows)
                            {
                                acsc.Add(row[0].ToString());
                            }

                            tb.AutoCompleteCustomSource = acsc;
                            tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                    }
                    #endregion r3
                }
                else if (columnHeaders.Equals("ExpDate"))
                {
                    #region r3
                    tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        DataTable dv = new DataTable();
                        dv.Columns.Add("Date");
                        dv.Rows.Add(DateTime.Now.ToString("dd-MM-yyyy"));
                        dv.Rows.Add(DateTime.Now.AddDays(+1).ToString("dd-MM-yyyy"));
                        dv.Rows.Add(DateTime.Now.AddDays(+2).ToString("dd-MM-yyyy"));
                        dv.Rows.Add(DateTime.Now.AddDays(+3).ToString("dd-MM-yyyy"));
                        dv.Rows.Add(DateTime.Now.AddDays(+4).ToString("dd-MM-yyyy"));

                        dv.Rows.Add(DateTime.Now.AddDays(+27).ToString("dd-MM-yyyy"));
                        dv.Rows.Add(DateTime.Now.AddDays(+28).ToString("dd-MM-yyyy"));
                        dv.Rows.Add(DateTime.Now.AddDays(+29).ToString("dd-MM-yyyy"));
                        dv.Rows.Add(DateTime.Now.AddDays(+30).ToString("dd-MM-yyyy"));

                        if (dv.Rows.Count > 0)
                        {
                            AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();

                            foreach (DataRow row in dv.Rows)
                            {
                                acsc.Add(row[0].ToString());
                            }

                            tb.AutoCompleteCustomSource = acsc;
                            tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                    }
                    #endregion r3
                }
                else
                {
                    tb.AutoCompleteCustomSource = null;
                }
            }
            catch { }
            //
            TextBox tb2 = null; tb2 = e.Control as TextBox;
            if (tb2 != null) { tb2.KeyPress += new KeyPressEventHandler(tb_KeyPress); }
            //

        }
        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell.ColumnIndex == 7 || dataGridView1.CurrentCell.ColumnIndex == 6) //Desired Column
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

        private void txtinvoice_KeyPress(object sender, KeyPressEventArgs e)
        {

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
                        dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[7]);
                    }
                    else if (i == 1)
                    {
                        dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[8]);
                    }
                    else
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
                    String ins = "INSERT INTO LedgerPayment VALUES('" + dt.Rows[i].ItemArray[0].ToString() + "','" + dt.Rows[i].ItemArray[1].ToString() + "','" + dt.Rows[i].ItemArray[2].ToString() + "','" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','" + dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "'," + dt.Rows[i].ItemArray[7].ToString() + "," + dt.Rows[i].ItemArray[8].ToString() + "," + dt.Rows[i].ItemArray[9].ToString() + ",'" + dt.Rows[i].ItemArray[10].ToString() + "','" + dt.Rows[i].ItemArray[11].ToString() + "')";
                    SqlCommand cmd = new SqlCommand(ins, con); cmd.ExecuteNonQuery();
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

        //
        //Ledger Update
        #region LedgerUpdate

        private int GetRowIndex1(string input)
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
        private void DeleteRecord1()
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
        private DataTable SetDT1()
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
        private DataTable SomeOperation1(DataTable dt)
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
            catch { }
            return dt;
        }
        private void InsertRecord1(DataTable dt)
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
                    String ins = "INSERT INTO LedgerPayment VALUES('" + dt.Rows[i].ItemArray[0].ToString() + "','" + dt.Rows[i].ItemArray[1].ToString() + "','" + dt.Rows[i].ItemArray[2].ToString() + "','" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','" + dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "'," + dt.Rows[i].ItemArray[7].ToString() + "," + dt.Rows[i].ItemArray[8].ToString() + "," + dt.Rows[i].ItemArray[9].ToString() + ",'" + dt.Rows[i].ItemArray[10].ToString() + "','" + dt.Rows[i].ItemArray[11].ToString() + "')";
                    SqlCommand cmd = new SqlCommand(ins, con); cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch { }
        }
        private void ShowDt1(DataTable dt)
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
        //

        private void Automatic()
        {
            try
            {
                decimal bam = 0;
                if (!txtbamount.Text.Equals(""))
                {
                    bam = Convert.ToDecimal(txtbamount.Text);
                }

                decimal disc = 0;
                if (!txtdiscount.Text.Equals(""))
                {
                    disc = Convert.ToDecimal(txtdiscount.Text);
                }
                if (disc > bam)
                {
                    txtdiscount.Text = "0"; disc = 0;
                }
                decimal df = bam - disc;

                decimal tax = 0;
                if (!txttax.Text.Equals(""))
                {
                    tax = Convert.ToDecimal(txttax.Text);
                }
                df += tax;
                txtnetamount.Text = df.ToString();
                decimal transport = 0;
                if (!txttransport.Text.Equals(""))
                {
                    transport = Convert.ToDecimal(txttransport.Text);
                }
                //
                //decimal a1 = 0;
                //if (!txtetax.Text.Equals(""))
                //{
                //    a1 = Convert.ToDecimal(txtetax.Text);
                //}

                //decimal a2 = 0;
                //if (!txtstamp.Text.Equals(""))
                //{
                //    a2 = Convert.ToDecimal(txtstamp.Text);
                //}

                //decimal a3 = 0;
                //if (!txtcaa.Text.Equals(""))
                //{
                //    a3 = Convert.ToDecimal(txtcaa.Text);
                //}

                //decimal a4 = 0;
                //if (!txtpiac.Text.Equals(""))
                //{
                //    a4 = Convert.ToDecimal(txtpiac.Text);
                //}

                //decimal a5 = 0;
                //if (!txtcae.Text.Equals(""))
                //{
                //    a5 = Convert.ToDecimal(txtcae.Text);
                //}

                //  decimal exp = a1 + a2 + a3 + a4 + a5;
                decimal final = bam + transport + tax;
                final = final - disc;
                txtcpt.Text = final.ToString();
            }
            catch { }
        }
        private void txtdiscount_Leave(object sender, EventArgs e)
        {
            try
            {
                Automatic();
                // String hm = txtdiscount.Text;
                // String th = hm.Substring(hm.Length - 1);
                // decimal bill = Convert.ToDecimal(txtbamount.Text);
                //if(th.Equals("%"))
                //{
                //String th1 = hm.Substring(0,hm.Length-1);
                //decimal dis = Convert.ToDecimal(th1);
                //decimal final = bill * dis / 100;
                //decimal f1 = bill - final;
                //decimal tax = Convert.ToDecimal(txttax.Text);
                //f1 = f1 + tax;
                //txtnetamount.Text = f1.ToString();
                //}
                //else
                //{
                //decimal d1 = Convert.ToDecimal(txtdiscount.Text);
                //decimal f2 = bill - d1;
                //decimal tax = Convert.ToDecimal(txttax.Text);
                //f2 = f2 + tax;
                //txtnetamount.Text = f2.ToString();
                //}  
            }
            catch { }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell.ColumnIndex == 6 || dataGridView1.CurrentCell.ColumnIndex == 7 || dataGridView1.CurrentCell.ColumnIndex == 8 || dataGridView1.CurrentCell.ColumnIndex == 9 || dataGridView1.CurrentCell.ColumnIndex == 10)
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
                            int n4 = Convert.ToInt32(dataGridView1.Rows[yhs].Cells[8].Value);
                            dataGridView1.CurrentCell = dataGridView1.Rows[yhs].Cells[0];
                            dataGridView1.BeginEdit(true);
                            dataGridView1.Rows[yhs + 1].Cells[0].Value = txtcode.Text;
                            //String g6 = "select * from Purchase where PurCode='" + txtcode.Text + "'"; DataTable df6 = clsDataLayer.RetreiveQuery(g6);
                            //if (df6.Rows.Count > 0)
                            //{
                            //    n4++;
                            //    dataGridView1.Rows[yhs + 1].Cells[8].Value = n4.ToString(); 
                            //}
                            //dataGridView1.Rows[yhs + 1].Cells[9].Value = "-";
                            //dataGridView1.Rows[yhs + 1].Cells[10].Value = "-";
                        }
                    }
                    else if (e.KeyCode == Keys.Delete)
                    {
                        if (dataGridView1.Rows.Count == 1)
                        {

                        }
                        else if (dataGridView1.Rows.Count > 0)
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
                        if (dataGridView1.Rows.Count == 1)
                        {

                        }
                        else if (dataGridView1.Rows.Count > 0)
                        {
                            int yh = dataGridView1.CurrentCell.RowIndex;
                            dataGridView1.Rows.RemoveAt(yh);
                        }
                    }
                }
            }
            catch { }

        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            try
            {
                String q1 = "select PurCode from VU_Purchase Order By PurCode asc";
                DataTable d1 = clsDataLayer.RetreiveQuery(q1); if (d1.Rows.Count > 0) { txtcode.Text = d1.Rows[0][0].ToString(); }
                btnNew.Enabled = true; btnSave.Enabled = false; btnEdit.Enabled = true; btnUpdate.Enabled = false;
            }
            catch { }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            try
            {
                String q1 = "select PurCode from VU_Purchase Order By PurCode desc";
                DataTable d1 = clsDataLayer.RetreiveQuery(q1); if (d1.Rows.Count > 0) { txtcode.Text = d1.Rows[0][0].ToString(); }
                btnNew.Enabled = true; btnSave.Enabled = false; btnEdit.Enabled = true; btnUpdate.Enabled = false;
            }
            catch { }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                String getcode = txtcode.Text;
                String[] hm = getcode.Split('-');
                String data = hm[1];
                int value = Convert.ToInt32(data);
                value--;
                String ck = value.ToString();

                if (ck.Length == 1)
                {
                    ck = "P" + "-" + "0000" + value.ToString();
                }
                else if (ck.Length == 2)
                {
                    ck = "P" + "-" + "000" + value.ToString();
                }
                else if (ck.Length == 3)
                {
                    ck = "P" + "-" + "00" + value.ToString();
                }
                else if (ck.Length == 4)
                {
                    ck = "P" + "-" + "0" + value.ToString();
                }
                else if (ck.Length == 5)
                {
                    ck = "P" + "-" + value.ToString();
                }
                txtcode.Text = ck.ToString();
            }
            catch { }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                String getcode = txtcode.Text;
                String[] hm = getcode.Split('-');
                String data = hm[1];
                int value = Convert.ToInt32(data);
                value++;
                String ck = value.ToString();

                if (ck.Length == 1)
                {
                    ck = "P" + "-" + "0000" + value.ToString();
                }
                else if (ck.Length == 2)
                {
                    ck = "P" + "-" + "000" + value.ToString();
                }
                else if (ck.Length == 3)
                {
                    ck = "P" + "-" + "00" + value.ToString();
                }
                else if (ck.Length == 4)
                {
                    ck = "P" + "-" + "0" + value.ToString();
                }
                else if (ck.Length == 5)
                {
                    ck = "P" + "-" + value.ToString();
                }
                txtcode.Text = ck.ToString();
            }
            catch { }
        }

        private void txtdiscount_KeyPress(object sender, KeyPressEventArgs e)
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

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 2 || dataGridView1.CurrentCell.ColumnIndex == 4)
            {
                if (e.KeyChar == (char)Keys.Escape)
                {
                    this.Close();
                }
            }
        }

        private void close(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void txttax_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txttax.Text.Equals(""))
                {
                    txttax.Text = "0";
                }
            }
            catch
            { }
        }

        private void PurchaseOrders_KeyDown(object sender, KeyEventArgs e)
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

        private void txtscan_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Select Inquiry                Global = "Inquiry";
                if (Global.Equals("Inquiry"))
                {
                    String get1 = "select * from VU_PINQ where PIurCode='" + txtpinv.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(get1);
                    if (d1.Rows.Count > 0)
                    {
                        txtname.Text = d1.Rows[0]["VendorName"].ToString(); txtremarks.Text = d1.Rows[0]["Remarks"].ToString();
                        txt_purchase_no.Text = d1.Rows[0]["LordNumber"].ToString();
                        //txtlord.Text = d1.Rows[0]["LordNumber"].ToString();
                        dataGridView1.Rows.Clear();
                        foreach (DataRow item in d1.Rows)
                        {
                            int n = dataGridView1.Rows.Add();
                            dataGridView1.Rows[n].Cells[0].Value = txtcode.Text;
                            dataGridView1.Rows[n].Cells[1].Value = "";
                            dataGridView1.Rows[n].Cells[2].Value = item["PICategory"].ToString();
                            dataGridView1.Rows[n].Cells[3].Value = item["PIurchaseItem"].ToString();
                            dataGridView1.Rows[n].Cells[4].Value = item["PISize"].ToString();
                            dataGridView1.Rows[n].Cells[5].Value = item["PIReference"].ToString();
                            dataGridView1.Rows[n].Cells[6].Value = item["Quantity"].ToString();

                            dataGridView1.Rows[n].Cells[9].Value = "-";
                            dataGridView1.Rows[n].Cells[10].Value = "-";
                            decimal ppc = 0;
                            String n54 = "select PURCHASE_PRICE from add_product where NAME='" + item["PIurchaseItem"].ToString() + "' AND Size='" + item["PISize"].ToString() + "' AND CATEGORY='" + item["PICategory"].ToString() + "' ";
                            DataTable d9 = clsDataLayer.RetreiveQuery(n54);
                            if (d9.Rows.Count > 0) { ppc = Convert.ToDecimal(d9.Rows[0][0].ToString()); }
                            decimal ff = ppc * Convert.ToDecimal(item["Quantity"].ToString());
                            dataGridView1.Rows[n].Cells[7].Value = ppc; dataGridView1.Rows[n].Cells[8].Value = ff.ToString();
                        }
                    }
                    else { txtpinv.Text = "No Record Found!"; }
                    totalbillandquant();
                }
            }
            catch { }
        }

        private void btnselectinquiry_Click(object sender, EventArgs e)
        {
            try
            {
                Global = "Inquiry";
                SearchInquiry sc = new SearchInquiry(this); sc.Show();
            }
            catch
            { }
        }

        private void txtwarehouse_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!txtwarehouse.Text.Equals(""))
                {
                    String g1 = "select * from tbl_warehouse where WarehouseName = '" + txtwarehouse.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(g1);
                    if (d1.Rows.Count > 0)
                    { }
                    else
                    { MessageBox.Show("Warehouse not found.!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); txtwarehouse.Text = ""; }
                }
            }
            catch { }
        }

        private void txttransport_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtname_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!txtname.Text.Equals(""))
                {
                    String getname = "select * from Accounts where ActTitle='" + txtname.Text + "'";
                    DataTable de = clsDataLayer.RetreiveQuery(getname);
                    if (de.Rows.Count < 1)
                    {
                        MessageBox.Show("UserName dont have a chart Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); txtname.Text = "";
                    }
                }
            }
            catch
            { }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                String getpd = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                decimal Quant = 0;
                for (int a = 0; a < dataGridView1.Rows.Count; a++)
                {
                    String getpd5 = dataGridView1.Rows[a].Cells[3].Value.ToString();
                    if (getpd.Equals(getpd5))
                    {
                        Quant += Convert.ToDecimal(dataGridView1.Rows[a].Cells[6].Value);
                    }
                }
                lblpd.Text = getpd; lblQuantity.Text = Quant.ToString();


                switch (dataGridView1.Columns[e.ColumnIndex].Name)
                {
                    case "mfg":

                        rd = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true); //  
                        dt785.Size = new Size(rd.Width, rd.Height); //  
                        dt785.Location = new Point(rd.X, rd.Y); //  
                        dt785.Visible = true;

                        break;
                    case "edate":

                        rd = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true); //  
                        dt785.Size = new Size(rd.Width, rd.Height); //  
                        dt785.Location = new Point(rd.X, rd.Y); //  
                        dt785.Visible = true;
                        break;

                }
            }
            catch { }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            dt785.Visible = false;
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            dt785.Visible = false;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell.ColumnIndex == 6 || dataGridView1.CurrentCell.ColumnIndex == 7)
                {
                    decimal price = 0; price = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[7].Value.ToString()); decimal quant = 0; quant = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[6].Value.ToString());
                    decimal final = price * quant;
                    dataGridView1.CurrentRow.Cells[8].Value = final.ToString();
                }
                int nn = 0;
                String lot = dataGridView1.CurrentRow.Cells[1].Value.ToString();

                String lot2 = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                String lot3 = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                String lot4 = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                for (int a = 0; a < dataGridView1.Rows.Count; a++)
                {
                    String glot = dataGridView1.Rows[a].Cells[1].Value.ToString();
                    String glot2 = dataGridView1.Rows[a].Cells[2].Value.ToString();
                    String glot3 = dataGridView1.Rows[a].Cells[3].Value.ToString();
                    String glot4 = dataGridView1.Rows[a].Cells[4].Value.ToString();
                    if (glot.Equals("") || glot2.Equals("") || glot3.Equals("") || glot4.Equals(""))
                    {

                    }
                    else if (lot.Equals(glot) && lot2.Equals(glot2) && lot3.Equals(glot3) && lot4.Equals(glot4))
                    {
                        nn++;
                    }
                }
                if (nn > 1)
                {
                    MessageBox.Show("Same Lot Number Cant Add Multiple Time", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    dataGridView1.CurrentRow.Cells[1].Value = "";
                }
            }
            catch { }
        }

        private void lblpd_Click(object sender, EventArgs e)
        {

        }

        //LedgerUpdate Closed

    }
}
