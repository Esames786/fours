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
using System.Net.Mail;
using System.Net;
using GrayLark.bin.Debug.Report;

namespace GrayLark
{
    public partial class Add_Product : Form
    {
        String UID = Login.UserID; String Oldq = "";
        public string GreenSignal = ""; String OldPrice = "";
        public string FormID = "";
        bool flag;


        public Add_Product()
        {
            InitializeComponent();
            btnNew.Focus(); type();
            //  category();
            txtPType.SelectedIndex = 0; disable(); Disable3(); btnEdit.Enabled = true; btnSave.Enabled = false; btnUpdate.Enabled = false; btnNew.Focus();

            clsGeneral.SetAutoCompleteTextBox(txtvendor, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 14 and HeaderActCode='20101' Order BY ID DESC"));
            this.KeyPreview = true;
            btnSave.Enabled = false;
            btnNew.Enabled = true;
            btnEdit.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void setcombo()
        {

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Add_Product_Load(object sender, EventArgs e)
        {
            //  type();
            btnSave.Enabled = false;
            btnUpdate.Enabled = false;
            //            btnEdit.Enabled = false;
            //category();
            ShowAll();
        }

        private void category()
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM product_cat", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtPCat, dt);
                }
            }
            catch { }
        }

        private void type()
        {
            try
            {
                if (txtPType.SelectedIndex == 0)
                {
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT NAME FROM add_product", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtPCat, dt);
                    }
                }
                else if (txtPType.SelectedIndex == 1)
                {
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT CATEGORY FROM add_product", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtPCat, dt);
                    }
                }
                else if (txtPType.SelectedIndex == 2)
                {
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT PRODUCT_ID FROM add_product", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtPCat, dt);
                    }
                }
                else if (txtPType.SelectedIndex == 3)
                {
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT principleName FROM add_product", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtPCat, dt);
                    }
                }

                else if (txtPType.SelectedIndex == 4)
                {
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT Referencecode FROM add_product", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtPCat, dt);
                    }
                }
            }
            catch { }
        }

        private void ShowAll()
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM add_product ORDER BY PRODUCT_ID ASC", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView1.Rows.Clear();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[0].Value = item["PRODUCT_ID"].ToString();
                    dataGridView1.Rows[n].Cells[1].Value = item["CATEGORY"].ToString();
                    dataGridView1.Rows[n].Cells[2].Value = item["NAME"].ToString();
                    dataGridView1.Rows[n].Cells[3].Value = item["Size"].ToString();
                    dataGridView1.Rows[n].Cells[4].Value = item["Referencecode"].ToString();
                    dataGridView1.Rows[n].Cells[5].Value = item["PURCHASE_PRICE"].ToString();
                    dataGridView1.Rows[n].Cells[6].Value = item["SELL_PRICE"].ToString();
                    dataGridView1.Rows[n].Cells[7].Value = item["productcode"].ToString();

                }
                dataGridView1.PerformLayout();
            }
            catch
            {
            }
        }

        private void txtProfit_TextChanged(object sender, EventArgs e)
        {
            //price();
        }

        public void price()
        {
            try
            {
                string Price = txtPPrice.Text;
                decimal p;
                string Profit = txtProfit.Text;
                decimal pft;

                p = (decimal.Parse(Price) / 100);
                pft = (p * decimal.Parse(Profit));

                txtSPrice.Text = (decimal.Parse(Price) + pft).ToString();

            }
            catch (Exception ex)
            {
                string name = ex.Message;
            }
        }

        private void txtProfitRupees_TextChanged(object sender, EventArgs e)
        {
            //ProfitRupees();
        }

        public void ProfitRupees()
        {
            try
            {
                string Price = txtPPrice.Text;
                string Profit = txtProfitRupees.Text;

                txtSPrice.Text = (decimal.Parse(Price) + decimal.Parse(Profit)).ToString();

            }
            catch (Exception ex)
            {
                string name = ex.Message;
            }
        }

        private void enable()
        {
            txtPName.Enabled = true; txt_category.Enabled = true;
            txtPPrice.Enabled = true; txtSPrice.Enabled = true;
            txtsize.Enabled = true;
        }

        private void txtProfitType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtPPrice_TextChanged(object sender, EventArgs e)
        {
            //ProfitRupees();
            //price();
            profit();
        }




        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                FormID = "Add";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    Clears2(); txtPPrice.Text = "0"; enable(); txtSPrice.Text = "0";
                    btnEdit.Enabled = false; txtsize.Enabled = true; btnSave.Enabled = true; btnUpdate.Enabled = false;
                    btnEdit.Enabled = false; btnNew.Enabled = false; txtPName.Clear(); txtSPrice.Clear(); this.txtPName.Focus();
                    txtPID.Text = clsGeneral.getMAXCode("add_product", "PRODUCT_ID", "P");
                    //    GetType();
                    txtPPrice.ReadOnly = false; txtPPrice.Enabled = true;
                    clsGeneral.SetAutoCompleteTextBox(txtvendor, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 14 and HeaderActCode='20101' Order BY ID DESC"));
                    Enable(); txtpcode.Focus(); dataGridView1.Enabled = false; GetCategory(); txtSPrice.Text = "0";
                    btnSave.Enabled = true;
                    btnNew.Enabled = false;
                    btnEdit.Enabled = false;
                    btnUpdate.Enabled = false;
                    //  GetCategory();
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void Save()
        {
            FormID = "Save";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                if (!CheckAllFields())
                {
                    String hm = "select * from add_product where NAME='" + txtPName.Text + "' and CATEGORY='" + txt_category.Text + "' and Size='" + txtsize.Text + "'";
                    DataTable dtf = clsDataLayer.RetreiveQuery(hm);
                    if (dtf.Rows.Count > 0)
                    {
                        MessageBox.Show("Already Added!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string datetime = DateTime.Now.ToString("yyyy-MM-dd");
                        txtPID.Text = clsGeneral.getMAXCode("add_product", "PRODUCT_ID", "P"); btnSave.Enabled = false;
                        String query = " INSERT INTO add_product (Referencecode,productcode,principleName,PRODUCT_ID, CATEGORY, NAME , Size, PURCHASE_PRICE,SELL_PRICE, USERNAME, DATETIME, STATUS) values('" + txtref.Text + "','" + txtpcode.Text + "','" + txtvendor.Text + "','" + txtPID.Text + "','" + txt_category.Text + "','" + txtPName.Text + "','" + txtsize.Text + "','" + txtPPrice.Text + "','" + txtSPrice.Text + "','" + UID + "','" + datetime + "','')";
                        clsDataLayer.ExecuteQuery(query);

                        //String q2 = "insert into add_product_stock(QuantityType,Status,PRODUCT_ID,CATEGORY,NAME,Mileage,Quantity,WarehouseName,Mfgdate,Expdate)values ('" + txtqtype.Text + "','Active','" + txtPID.Text + "','" + txt_category.Text + "','" + txtPName.Text + "','" + txtmileage.Text + "',0,'','','')"; clsDataLayer.ExecuteQuery(q2);

                        MessageBox.Show(txtPName.Text + " is now Added in " + txtPCat.Text + " !", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnUpdate.Enabled = false; btnSave.Enabled = false;
                        btnNew.Enabled = true; btnEdit.Enabled = false;
                        btnExit.Enabled = true; btnClear.Enabled = true;
                        disable(); Disable3(); ShowAll(); dataGridView1.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Please fill all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                String getname = "select * from Accounts where ActTitle='" + txtvendor.Text + "'";
                DataTable de = clsDataLayer.RetreiveQuery(getname);
                if (de.Rows.Count < 1)
                {
                    MessageBox.Show("UserName dont have a chart Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Save();
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                    btnEdit.Enabled = false;
                    btnUpdate.Enabled = false;
                }
            }
            catch
            {
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clears2(); btnEdit.Enabled = false; btnNew.Enabled = true; dataGridView1.Enabled = true;
        }

        private bool Clears2()
        {
            bool flag = false;
            dataGridView1.Enabled = true;
            foreach (Control c in this.tableLayoutPanel3.Controls)
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

        private void clear()
        {
            ShowAll();
            btnSave.Enabled = false;
            btnUpdate.Enabled = false;
            btnEdit.Enabled = false;
            btnNew.Enabled = true;

            txtPID.Clear();
            txtPName.Clear();
            txtPType.SelectedIndex = -1;
            //  txtPCat.SelectedIndex = -1;

            txtProfitType.SelectedIndex = -1;
            txtProfitRupees.Clear();
            txtPPrice.Clear();
            txtProfit.Clear();
            txtStatus.Clear();
            txtSPrice.Clear();
            //disable();

        }

        private void Search()
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM add_product WHERE CATEGORY = '" + txtPType.Text + "'", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView1.Rows.Clear();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dataGridView1.Rows.Add();

                    dataGridView1.Rows[n].Cells[0].Value = item["PRODUCT_ID"].ToString();
                    dataGridView1.Rows[n].Cells[1].Value = item["CATEGORY"].ToString();
                    dataGridView1.Rows[n].Cells[2].Value = item["NAME"].ToString();
                    dataGridView1.Rows[n].Cells[3].Value = item["Size"].ToString();
                    dataGridView1.Rows[n].Cells[4].Value = item["Referencecode"].ToString();
                    dataGridView1.Rows[n].Cells[5].Value = item["QuantityType"].ToString();
                    dataGridView1.Rows[n].Cells[6].Value = item["PURCHASE_PRICE"].ToString();
                    dataGridView1.Rows[n].Cells[7].Value = item["SELL_PRICE"].ToString();
                }
            }
            catch { }
        }

        private void search4()
        {
            try
            {

                String sel = "select * from add_product where PRODUCT_ID='" + txtPID.Text + "'";
                DataTable ds = clsDataLayer.RetreiveQuery(sel);
                if (ds.Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Rows)
                    {
                        txtPID.Text = item["PRODUCT_ID"].ToString();
                        txtvendor.Text = item["principleName"].ToString();
                        txt_category.Text = item["CATEGORY"].ToString();
                        txtPName.Text = item["NAME"].ToString();
                        txtsize.Text = item["Size"].ToString();
                        txtPPrice.Text = item["PURCHASE_PRICE"].ToString();
                        txtSPrice.Text = item["SELL_PRICE"].ToString();

                        txtref.Text = item["Referencecode"].ToString();
                        txtpcode.Text = item["productcode"].ToString();

                    }
                }
            }
            catch { }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                btnEdit.Enabled = true; txtPID.Enabled = false; txt_category.Enabled = false; txtPName.Enabled = false;
                txtsize.Enabled = false;
                txtPPrice.Enabled = false; txtSPrice.Enabled = false;
                txtPID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                btnSave.Enabled = false;
                btnNew.Enabled = false;
                btnEdit.Enabled = true;
                btnUpdate.Enabled = false;
            }
            catch { }
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

        private bool Disable3()
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


        private bool Enable1()
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
        private bool Enable2()
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
        private void btnEdit_Click(object sender, EventArgs e)
        {
            FormID = "Edit";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                OldPrice = txtPPrice.Text;
                btnSave.Enabled = false; btnEdit.Enabled = false;
                btnUpdate.Enabled = true; enable(); Enable();
                btnSave.Enabled = false;
                btnNew.Enabled = false;
                btnEdit.Enabled = false;
                btnUpdate.Enabled = true;
            }
            else { MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop); }

        }

        private void disable()
        {
            txt_category.Enabled = false;
            txtPID.Enabled = false; txtPName.Enabled = false;
            txtsize.Enabled = false;
            txtPPrice.Enabled = false; txtSPrice.Enabled = false;
        }

        private bool CheckAllFields()
        {
            bool flag = false;
            foreach (Control o in this.tableLayoutPanel3.Controls)
            {
                if (o is TextBox)
                {
                    if (((TextBox)o).Text == "")
                    {
                        flag = true;
                    }
                }
                else if (o is ComboBox)
                {
                    if (((ComboBox)o).Text == "")
                    {
                        flag = true;
                    }
                }

            }
            return flag;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            con.Close();
            try
            {
                FormID = "Update"; UserNametesting();
                if (GreenSignal == "YES")
                {
                    if (!CheckAllFields())
                    {
                        String getname = "select * from Accounts where ActTitle='" + txtvendor.Text + "'";
                        DataTable de = clsDataLayer.RetreiveQuery(getname);
                        if (de.Rows.Count < 1)
                        {
                            MessageBox.Show("UserName dont have a chart Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {

                            String del1 = "delete from add_product where PRODUCT_ID='" + txtPID.Text + "'"; clsDataLayer.ExecuteQuery(del1);

                            //String query = " INSERT INTO add_product (Referencecode,productcode,principleName,PRODUCT_ID, CATEGORY, NAME , Size, PURCHASE_PRICE,SELL_PRICE, USERNAME, DATETIME, STATUS) values('" + txtref.Text + "','" + txtpcode.Text + "','" + txtvendor.Text + "','" + txtPID.Text + "','" + txt_category.Text + "','" + txtPName.Text + "','" + txtsize.Text + "','" + txtPPrice.Text + "','" + txtSPrice.Text + "','" + UID + "','" + DateTime.Now + "','')";
                            //clsDataLayer.ExecuteQuery(query);//mohsin comment

                            Save();

                            //String del2 = "delete from add_product_stock where PRODUCT_ID='" + txtPID.Text + "'"; clsDataLayer.ExecuteQuery(del2);

                            //String q2 = "update add_product_stock set NAME='" + txtPName.Text + "',QuantityType='"+txtqtype.Text+"',CATEGORY='" + txt_category.Text + "',Mileage='" + txtmileage.Text + "' where PRODUCT_ID ='" + txtPID.Text + "' ";

                            MessageBox.Show(txtPName.Text + " is now Updated!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            btnUpdate.Enabled = false; btnSave.Enabled = false; btnNew.Enabled = true; btnEdit.Enabled = true; btnExit.Enabled = true; btnClear.Enabled = true; ShowAll(); disable(); Disable3();
                            btnSave.Enabled = false;
                            btnNew.Enabled = true;
                            btnEdit.Enabled = false;
                            btnUpdate.Enabled = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please fill all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) { }


        private void txtPID_TextChanged(object sender, EventArgs e)
        {
            try { search4(); btnEdit.Enabled = true; }
            catch { }
        }



        private void txtPName_Leave(object sender, EventArgs e)
        {
            txtPName.Text = txtPName.Text.ToUpper();
        }

        private void txtSPrice_TextChanged(object sender, EventArgs e)
        {
            profit();
        }

        private void profit()
        {
            try
            {
                string Sell = txtSPrice.Text;
                string Purchase = txtPPrice.Text;
                txtProfitType.Text = "In Rupees";
                txtProfitRupees.Enabled = false;

                txtProfitRupees.Text = (int.Parse(Sell) - int.Parse(Purchase)).ToString();

            }
            catch (Exception ex)
            {
                string name = ex.Message;
            }
        }

        private void txtStatus_Leave(object sender, EventArgs e)
        {
            txtStatus.Text = txtStatus.Text.ToUpper();
        }

        private void txtPType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                type();
            }
            catch
            { }
        }

        private void ShowAlls()
        {
            String abc = "select * from add_product";
            DataTable df = clsDataLayer.RetreiveQuery(abc);
            if (df.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
                foreach (DataRow item in df.Rows)
                {
                    int n = dataGridView1.Rows.Add();

                   
                    dataGridView1.Rows[n].Cells[0].Value = item["PRODUCT_ID"].ToString();
                    dataGridView1.Rows[n].Cells[1].Value = item["CATEGORY"].ToString();
                    dataGridView1.Rows[n].Cells[2].Value = item["NAME"].ToString();
                    dataGridView1.Rows[n].Cells[3].Value = item["Size"].ToString();
                    dataGridView1.Rows[n].Cells[4].Value = item["Referencecode"].ToString();

                    dataGridView1.Rows[n].Cells[5].Value = item["PURCHASE_PRICE"].ToString();
                    dataGridView1.Rows[n].Cells[6].Value = item["SELL_PRICE"].ToString();
                }
            }
        }

        private void search()
        {
            try
            {
                if (txtPCat.Text.Length > 0)
                {
                    
                    String sa = "select * from add_product where " + txtPType.Text + " = '" + txtPCat.Text + "' ";
                    SqlDataAdapter sda = new SqlDataAdapter(sa, con);

                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.Rows.Clear();
                        foreach (DataRow item in dt.Rows)
                        {
                            int n = dataGridView1.Rows.Add();
                            dataGridView1.Rows[n].Cells[0].Value = item["PRODUCT_ID"].ToString();
                            dataGridView1.Rows[n].Cells[1].Value = item["CATEGORY"].ToString();
                            dataGridView1.Rows[n].Cells[2].Value = item["NAME"].ToString();
                            dataGridView1.Rows[n].Cells[3].Value = item["Size"].ToString();
                            dataGridView1.Rows[n].Cells[4].Value = item["Referencecode"].ToString();

                            dataGridView1.Rows[n].Cells[5].Value = item["PURCHASE_PRICE"].ToString();
                            dataGridView1.Rows[n].Cells[6].Value = item["SELL_PRICE"].ToString();
                            dataGridView1.Rows[n].Cells[7].Value = item["productcode"].ToString();
                        }
                    }
                    else
                    {
                        ShowAll();
                    }
                }
                else
                {
                    ShowAll();
                }

            }
            catch { }
        }

        private void showingrid(object sender, EventArgs e)
        {
            search();
        }

        private void close(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }

        }


        private void GetCategory()
        {
            DataTable dt = clsDataLayer.RetreiveQuery("select PRODUCT_CATEGORY from product_cat");
            if (dt.Rows.Count > 0)
            {
                txt_category.DataSource = dt;
                txt_category.DisplayMember = "PRODUCT_CATEGORY";
                txt_category.ValueMember = "PRODUCT_CATEGORY";
            }
        }

        private void txtPName_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtPPrice_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtSPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                Save();
            }
            else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txt_type_Click(object sender, EventArgs e)
        {
            //  GetType();

        }

        private void txt_category_Click(object sender, EventArgs e)
        {
            GetCategory();
        }

        private void txtPCat_TextChanged(object sender, EventArgs e)
        {
            search();
        }


        private void txt_category_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            try
            {
                FormID = "Print";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    String qs = "";
                    if (!txtPID.Text.Equals(""))
                    {
                        qs = "select * from add_product where PRODUCT_ID='" + txtPID.Text + "'";
                    }
                    else
                    {
                        qs = "select * from add_product";
                    }
                    DataTable d1 = clsDataLayer.RetreiveQuery(qs); if (d1.Rows.Count > 0)
                    {
                        RawProduct fp = new RawProduct(); fp.SetDataSource(d1); PaymentView pv = new PaymentView(fp); pv.Show();
                    }
                    else { MessageBox.Show("No Record Found! ", "Alert", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error); }
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void btnPrintAll_Click(object sender, EventArgs e)
        {
            try
            {
                FormID = "Print";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    String qs = ""; qs = "select * from add_product";

                    DataTable d1 = clsDataLayer.RetreiveQuery(qs); if (d1.Rows.Count > 0)
                    { RawProduct fp = new RawProduct(); fp.SetDataSource(d1); PaymentView pv = new PaymentView(fp); pv.Show(); }
                    else { MessageBox.Show("No Record Found! ", "Alert", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error); }
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }

        }

        private void Add_Product_KeyDown(object sender, KeyEventArgs e)
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
            else if (e.Control == true && e.KeyCode == Keys.X)
            {
                btnExit.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        private void txtref_Leave(object sender, EventArgs e)
        {
            try
            {
                txtSPrice.Text = "0";
                if (!txtref.Text.Equals("") && !txtPID.Text.Equals(""))
                {
                    String get = "select * from add_product where Referencecode='" + txtref.Text + "' AND PRODUCT_ID!='" + txtPID.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(get);
                    if (d1.Rows.Count > 0) { txtref.Text = ""; MessageBox.Show("Already Add Reference Code!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
                    else { }
                }
            }
            catch
            { }
        }

        private void txtsize_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!txtsize.Text.Equals(""))
                {
                    String get = "select * from add_product where Size='" + txtref.Text + "' and NAME='" + txtPName.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(get);
                    if (d1.Rows.Count > 0) { txtref.Text = ""; MessageBox.Show("Already Add Product with this Size!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
                    else { }
                }
            }
            catch
            { }
        }

        private void txtvendor_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!txtvendor.Text.Equals(""))
                {
                    String get = "select * from Accounts where ActTitle = '" + txtvendor.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(get);
                    if (d1.Rows.Count < 1) { txtvendor.Text = ""; MessageBox.Show("Add Principle in Chart of Account!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
                    else { }
                }
            }
            catch
            { }

        }

        private void txtpcode_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!txtpcode.Text.Equals("") && !txtPID.Text.Equals(""))
                {
                    String get = "select * from add_product where productcode='" + txtpcode.Text + "' AND PRODUCT_ID!='" + txtPID.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(get);
                    if (d1.Rows.Count > 0) { txtpcode.Text = ""; MessageBox.Show("Already Add Product Code!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
                    else { }
                }
            }
            catch
            { }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
