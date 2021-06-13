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

namespace GrayLark
{
    public partial class PurchaseInquiry : Form
    {
        String Code = ""; TextBox tb = new TextBox();
        String UID = Login.UserID;
        int same = 0;
        public string GreenSignal = "";
        public string FormID = "";
        decimal old = 0;
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        public PurchaseInquiry()
        {
            InitializeComponent();
            clsGeneral.SetAutoCompleteTextBox(txtname, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 13 and HeaderActCode='20101' Order BY ID DESC"));
            Disable(); btnNew.Focus(); this.KeyPreview = true; txtscan.Enabled=false;
        }
        private string IID;
        public string passIID
        {
            get { return IID; }
            set { IID = value; }
        }
        public void UserNametesting()
        {
            string query = "select * from user_access where USENAME='" + UID + "' AND FORM_NAME='" + FormID + "'";
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
            return flag;
        }

        private bool CheckDataGridCells(DataGridView dgv)
        {
            bool flag = false;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    //  if (dgv.Rows[i].Cells[j].Value == null)
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
            _quantity.Text = "";
            dataGridView1.Rows.Clear();
        }


        private void totalbillandquant()
        {
            try
            {
                int namt = 0;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[5].Value != null)
                    {
                        namt = namt + int.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                    }
                }
                this._quantity.Text = namt.ToString();
            }
            catch (Exception ex)
            {
                string name = ex.Message;
            }


        }
        private bool Enable()
        {
            bool flag = false;
            dataGridView1.Enabled = true;
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

        private bool Disable()
        {
            dataGridView1.Enabled = false;
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

        private bool Clears2()
        {
            bool flag = false;
            dataGridView1.Enabled = true; dataGridView1.Rows.Clear();
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
            return flag;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            inqnew();
        }

        public void inqnew()
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
                    btn_detail.Enabled = true; dte.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    txtcode.Text = clsGeneral.getMAXCode("PurchaseInq", "PIurCode", "PI");
                    //  dataGridView1.Rows.Add();
                    //  dataGridView1.Rows[0].Cells[0].Value = txtcode.Text;
                    txtname.Enabled = true; txtname.ReadOnly = false;
                    clsGeneral.SetAutoCompleteTextBox(txtname, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 13 and HeaderActCode='20101' Order BY ID DESC")); txtname.Focus();
                    txtname.Focus(); txtscan.Enabled = true;
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
                    //PurchaseInq(LordNumber,VStatus,PIurCode,VendorName,VendorCode,Date,UserName,Remarks)values('" + txtlord.Text + "','Inquiry',
                    String get = "select * from PurchaseInq where PIurCode='" + txtcode.Text + "' and  VStatus='Inquiry'";
                    DataTable d2 = clsDataLayer.RetreiveQuery(get);
                    if (d2.Rows.Count > 0)
                    {
                        countquant();
                        Enable(); txtcode.ReadOnly = false; txtcode.Enabled = true; 
                        btnSave.Enabled = false; btnUpdate.Enabled = true; 
                        btnNew.Enabled = false; btnEdit.Enabled = false;
                        btn_detail.Enabled = true; txtname.Enabled = true;
                    }
                    else { MessageBox.Show("Cant Update Already make purchase order!","Stop",MessageBoxButtons.OK,MessageBoxIcon.Stop); }
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

                            String m1 = "select Date from PurchaseInq where PIurCode='" + txtcode.Text + "'"; DataTable m2 = clsDataLayer.RetreiveQuery(m1);
                            if (m2.Rows.Count > 0)
                            {
                                Odate = m2.Rows[0][0].ToString();
                            }

                        }
                        catch { }
                        try
                        {
                            String getname = "select * from Accounts where ActTitle='" + txtname.Text + "'";
                            DataTable de = clsDataLayer.RetreiveQuery(getname);
                            if (de.Rows.Count < 1)
                            {
                                MessageBox.Show("UserName dont have a chart Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                btnUpdate.Enabled = false;


                                string query = "";
                                query = "Delete From PurchaseInq Where PIurCode = '" + txtcode.Text + "'";
                                clsDataLayer.ExecuteQuery(query);


                                string query9 = "";
                                query9 = "Delete From PI_PurchaseDetail Where PIurCode = '" + txtcode.Text + "'";
                                clsDataLayer.ExecuteQuery(query9);

                                String p3 = "insert into PurchaseInq(LordNumber,VStatus,PIurCode,VendorName,VendorCode,Date,UserName,Remarks)values('" + txtlord.Text + "','Inquiry','" + txtcode.Text + "','" + txtname.Text + "','" + Code + "','" + dte.Value.ToString("yyyy-MM-dd") + "','" + Login.UserID + "','" + txtremarks.Text + "')";

                                if (clsDataLayer.ExecuteQuery(p3) > 0)
                                {
                                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                                    {
                                        String purchase_order_no = dataGridView1.Rows[i].Cells[0].Value.ToString(); String category = dataGridView1.Rows[i].Cells[1].Value.ToString();

                                        String product_name = dataGridView1.Rows[i].Cells[2].Value.ToString();
                                        String mileage = dataGridView1.Rows[i].Cells[3].Value.ToString();
                                        String Reference = dataGridView1.Rows[i].Cells[4].Value.ToString();

                                        decimal Quantity = Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value.ToString());

                                        String pd1 = "insert into PI_PurchaseDetail(PIReference,PIurCode,PIurchaseItem,PICategory,PISize,Quantity)values('" + Reference + "','" + purchase_order_no + "','" + product_name + "','" + category + "','" + mileage + "'," + Quantity + ")";
                                        if (clsDataLayer.ExecuteQuery(pd1) > 0) { }
                                    }
                                    Disable(); btnUpdate.Enabled = false; btnNew.Enabled = true;
                                    btnEdit.Enabled = true;
                                    MessageBox.Show("Update Success!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                FormID = "Print";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    if (txtcode.Text != "")
                    {
                        String qu = "select * from VU_PINQ where PIurCode = '" + txtcode.Text + "' ";
                        DataTable ds = clsDataLayer.RetreiveQuery(qu);
                        if (ds.Rows.Count > 0)
                        {
                            rptPurchaseInquiry pur = new rptPurchaseInquiry();
                            pur.SetDataSource(ds);
                            purchase_inquiry_rpt_display pop = new purchase_inquiry_rpt_display(pur,txtcode.Text);
                            pop.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select Purchase ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        bool pcs = true;
        private void CheckProd()
        {
            pcs = true;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                String pd = dataGridView1.Rows[i].Cells[2].Value.ToString(); String cg = dataGridView1.Rows[i].Cells[1].Value.ToString();
                String ps = dataGridView1.Rows[i].Cells[3].Value.ToString();
                String Db = "select * from add_product where CATEGORY='" + cg + "' and Size='" + ps + "' and NAME = '" + pd + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(Db); if (d1.Rows.Count > 0)
                {

                }
                else { pcs = false; }
            }
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
                        String getname = "select * from Accounts where ActTitle='" + txtname.Text + "'";
                        DataTable de = clsDataLayer.RetreiveQuery(getname);
                        if (de.Rows.Count < 1)
                        {
                            MessageBox.Show("UserName dont have a chart Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            if (!CheckDataGridCells(dataGridView1))
                            {
                                CheckProd();
                                if (pcs == true)
                                {
                                    btnSave.Enabled = false; btnNew.Enabled = true; txtcode.Text = clsGeneral.getMAXCode("PurchaseInq", "PIurCode", "PI");

                                    totalbillandquant();
                                    String p3 = "insert into PurchaseInq(LordNumber,VStatus,PIurCode,VendorName,VendorCode,Date,UserName,Remarks)values('" + txtlord.Text + "','Inquiry','" + txtcode.Text + "','" + txtname.Text + "','" + Code + "','" + dte.Value.ToString("yyyy-MM-dd") + "','" + Login.UserID + "','" + txtremarks.Text + "')";

                                    if (clsDataLayer.ExecuteQuery(p3) > 0)
                                    {
                                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                                        {
                                            String purchase_order_no = dataGridView1.Rows[i].Cells[0].Value.ToString(); String category = dataGridView1.Rows[i].Cells[1].Value.ToString();

                                            String product_name = dataGridView1.Rows[i].Cells[2].Value.ToString();
                                            String mileage = dataGridView1.Rows[i].Cells[3].Value.ToString();
                                            String Reference = dataGridView1.Rows[i].Cells[4].Value.ToString();

                                            decimal Quantity = Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value.ToString());

                                            String pd1 = "insert into PI_PurchaseDetail(PIReference,PIurCode,PIurchaseItem,PICategory,PISize,Quantity)values('" + Reference + "','" + purchase_order_no + "','" + product_name + "','" + category + "','" + mileage + "','" + Quantity + "')";
                                            if (clsDataLayer.ExecuteQuery(pd1) > 0)
                                            {
                                            }
                                        }

                                        MessageBox.Show("Saved ", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        Disable();
                                        btnNew.Enabled = true; btnEdit.Enabled = true; btnSave.Enabled = false;
                                        btnUpdate.Enabled = false; btn_detail.Enabled = true;
                                    }

                                }
                                else
                                { MessageBox.Show("Product Name is Incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                            }
                            else
                            {
                                MessageBox.Show("Please Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (dataGridView1.Rows[i].Cells[5].Value != null)
                {
                    namt = namt + Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value.ToString());
                }
            }
            //this.txtQuantity.Text = namt.ToString();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell.ColumnIndex == 4)
                {
                 
                    bool match = false;
                    String prefer = dataGridView1.CurrentRow.Cells[4].Value.ToString(); String pcateg = dataGridView1.CurrentRow.Cells[1].Value.ToString(); String pname = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    int nn = 0; String psize = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    String fetch2 = "select Referencecode from add_product where CATEGORY='" + pcateg + "' and NAME='" + pname + "' and Size='" + psize + "'"; DataTable df3 = clsDataLayer.RetreiveQuery(fetch2);
                    if (df3.Rows.Count > 0)
                    {
                        //for (int a = 0; a < dataGridView1.Rows.Count; a++)
                        //{
                        //    String Grefer = dataGridView1.Rows[a].Cells[4].Value.ToString(); String Gpcateg = dataGridView1.Rows[a].Cells[1].Value.ToString(); String Gpname = dataGridView1.Rows[a].Cells[2].Value.ToString(); String Gpsize = dataGridView1.Rows[a].Cells[3].Value.ToString();
                        //    if (Gpcateg.Equals("") || Gpname.Equals("") || Gpsize.Equals("") || Grefer.Equals(""))
                        //    {

                        //    }
                        //    else if (pcateg.Equals(Gpcateg) && pname.Equals(Gpname) && psize.Equals(Gpsize) && Grefer.Equals(prefer))
                        //    {
                        //        nn++;
                        //    }
                        //}
                        //if (nn > 1)
                        //{
                        //    MessageBox.Show("Same Product Cant Add Multiple Time", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); match = true;
                        //    dataGridView1.CurrentRow.Cells[4].Value = "";
                        //}
                    }
                    else
                    {
                        MessageBox.Show("Selected ReferenceCode is not Found!!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dataGridView1.CurrentRow.Cells[4].Value = "";
                    } 
                }
                else if (dataGridView1.CurrentCell.ColumnIndex == 3)
                {
                    #region ColumnIndex3
                    String psize = dataGridView1.CurrentRow.Cells[3].Value.ToString(); 
                    String s1 = "select Size from add_product where Size='" + psize + "'"; DataTable ds1 = clsDataLayer.RetreiveQuery(s1);
                    if (ds1.Rows.Count > 0)
                    { String pcateg = dataGridView1.CurrentRow.Cells[1].Value.ToString(); String pname = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                     
                            //String fetch2 = "select Referencecode from add_product where CATEGORY='" + pcateg + "' and NAME='" + pname + "' and Size='" + psize + "'"; DataTable df3 = clsDataLayer.RetreiveQuery(fetch2);
                            //if (df3.Rows.Count > 0)
                            //{
                            //    String fref = df3.Rows[0][0].ToString(); dataGridView1.CurrentRow.Cells[4].Value = fref;
                            //}
                    }
                    else
                    {
                        MessageBox.Show("Selected Size is not Found!!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); 
                        dataGridView1.CurrentRow.Cells[3].Value = "";
                    }
                    #endregion ColumnIndex3
                }
                else if (dataGridView1.CurrentCell.ColumnIndex == 4)
                {
                    String get4 = "select Size from add_product where Referencecode='"+dataGridView1.CurrentRow.Cells[4].Value.ToString()+"'";
                    DataTable e3 = clsDataLayer.RetreiveQuery(get4);
                    if (e3.Rows.Count > 0)
                    {
                        String size6 = e3.Rows[0][0].ToString(); dataGridView1.CurrentRow.Cells[3].Value = size6;
                    }
                    else
                    {
                        MessageBox.Show("Reference Code not Found!"); dataGridView1.CurrentRow.Cells[4].Value = "";
                    }
                }
                else if (dataGridView1.CurrentCell.ColumnIndex == 2)
                {
                    #region ColumnIndex2
                    String pname = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    String s1 = "select NAME from add_product where NAME='" + pname + "'"; DataTable ds1 = clsDataLayer.RetreiveQuery(s1);
                    if (ds1.Rows.Count > 0)
                    {
                    }
                    else
                    {
                        MessageBox.Show("Selected Product Name is not Found!!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dataGridView1.CurrentRow.Cells[2].Value = "";
                    }
                    #endregion ColumnIndex2
                }
                else if (dataGridView1.CurrentCell.ColumnIndex == 1)
                {
                    #region ColumnIndex1
                    String pcateg = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    String s1 = "select CATEGORY from add_product where CATEGORY='" + pcateg + "'"; DataTable ds1 = clsDataLayer.RetreiveQuery(s1);
                    if (ds1.Rows.Count > 0)
                    {
                    }
                    else
                    {
                        MessageBox.Show("Selected Product Category is not Found!!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dataGridView1.CurrentRow.Cells[1].Value = "";

                    }
                    #endregion ColumnIndex1
                }
                else if (dataGridView1.CurrentCell.ColumnIndex == 5)
                {
                    decimal iq = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[5].Value);
                    if (iq > 0)
                    {
                        int namt = 0;
                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[5].Value != null)
                            {
                                namt = namt + int.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                            }
                        }
                        this._quantity.Text = namt.ToString();
                    }
                    else
                    {
                        dataGridView1.CurrentRow.Cells[5].Value = ""; MessageBox.Show("Quantity Must be greater than 0", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
            }
            catch { }

        }


        private void btnclear_Click(object sender, EventArgs e)
        {
            Clears2();
            Disable();
            btnNew.Enabled = true;
            btnEdit.Enabled = true;
            btnSave.Enabled = false;
            btnUpdate.Enabled = false; dataGridView1.Rows.Clear();
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
            PurchaseInquiry_Detail frm = new PurchaseInquiry_Detail(this);
            frm.ShowDialog();
        }

        String hn = "";
        private void txtcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Select Inquiry
                String get1 = "select * from VU_PINQ where PIurCode='" + txtcode.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(get1);
                if (d1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                    txtname.Text = d1.Rows[0]["VendorName"].ToString(); txtremarks.Text = d1.Rows[0]["Remarks"].ToString(); txtlord.Text = d1.Rows[0]["LordNumber"].ToString();
                    dte.Text = d1.Rows[0]["Date"].ToString(); btnSave.Enabled = false; btnEdit.Enabled = true; btnNew.Enabled = true;
                    foreach (DataRow item in d1.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = txtcode.Text;
                        dataGridView1.Rows[n].Cells[1].Value = item["PICategory"].ToString();
                        dataGridView1.Rows[n].Cells[2].Value = item["PIurchaseItem"].ToString();
                        dataGridView1.Rows[n].Cells[3].Value = item["PISize"].ToString();
                        dataGridView1.Rows[n].Cells[4].Value = item["PIReference"].ToString();
                        dataGridView1.Rows[n].Cells[5].Value = item["Quantity"].ToString();
                    } totalbillandquant();
                }
            }
            catch { }

        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //
            try
            {
                if (txtname.Text.Length > 0)
                {
                    int yy = dataGridView1.CurrentCell.ColumnIndex;
                    string columnHeaders = dataGridView1.Columns[yy].HeaderText;
                    if (columnHeaders.Equals("Product Name"))
                    {
                        tb = e.Control as TextBox;
                        if (tb != null)
                        {
                            
                            string chk = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                            String hname = "";
                            hname = "select NAME from add_product where principleName='" + txtname.Text + "' and [CATEGORY] = '"+chk+"' ";
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
                            string chk = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                            string chk1 = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                            hname = "select Size from add_product where principleName='" + txtname.Text + "' and [CATEGORY] = '" + chk + "' and [NAME] = '"+chk1+"'";
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
                             hname = "select CATEGORY from add_product where principleName='" + txtname.Text + "'"; 
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
                        tb = e.Control as TextBox;
                        if (tb != null)
                        {
                            String hname = "";
                            string chk = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                            string chk1 = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                            string chk2 = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                            hname = "select Referencecode from add_product where principleName='" + txtname.Text + "' and [CATEGORY] = '" + chk + "' and [NAME] = '" + chk1 + "' and [Size] = '"+chk2+"'";
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
                    else
                    {
                        tb.AutoCompleteCustomSource = null;
                    }
                }
                else
                {
                        dataGridView1.Rows.Clear();
                        MessageBox.Show("Enter Principle name first");
                        txtname.Focus();
                }
                TextBox tb2 = null; tb2 = e.Control as TextBox;
                if (tb2 != null) { tb2.KeyPress += new KeyPressEventHandler(tb_KeyPress); }
                //
                e.Control.KeyPress -= new KeyPressEventHandler(gridColumns_KeyPress);
                if (dataGridView1.CurrentCell.ColumnIndex <= 5) //Desired Column
                {
                    TextBox tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        tb.KeyPress += new KeyPressEventHandler(gridColumns_KeyPress);
                    }
                }
            }
            catch { }
            //
            
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell.ColumnIndex == 5) //Desired Column
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


        private void gridColumns_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar == (char)Keys.Space) && (e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void txtinvoice_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell.ColumnIndex == 5)
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
                            dataGridView1.Rows[yhs + 1].Cells[0].Value = txtcode.Text;
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
            }
            catch { }

        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            try
            {
                String q1 = "select PIurCode from PurchaseInq Order By PIurCode asc";
                DataTable d1 = clsDataLayer.RetreiveQuery(q1); if (d1.Rows.Count > 0) { txtcode.Text = d1.Rows[0][0].ToString(); }
                btnNew.Enabled = true; btnSave.Enabled = false; btnEdit.Enabled = true; btnUpdate.Enabled = false;
            }
            catch { }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            try
            {
                String q1 = "select PIurCode from PurchaseInq Order By PIurCode desc";
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
                    ck = "PI" + "-" + "0000" + value.ToString();
                }
                else if (ck.Length == 2)
                {
                    ck = "PI" + "-" + "000" + value.ToString();
                }
                else if (ck.Length == 3)
                {
                    ck = "PI" + "-" + "00" + value.ToString();
                }
                else if (ck.Length == 4)
                {
                    ck = "PI" + "-" + "0" + value.ToString();
                }
                else if (ck.Length == 5)
                {
                    ck = "PI" + "-" + value.ToString();
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
                    ck = "PI" + "-" + "0000" + value.ToString();
                }
                else if (ck.Length == 2)
                {
                    ck = "PI" + "-" + "000" + value.ToString();
                }
                else if (ck.Length == 3)
                {
                    ck = "PI" + "-" + "00" + value.ToString();
                }
                else if (ck.Length == 4)
                {
                    ck = "PI" + "-" + "0" + value.ToString();
                }
                else if (ck.Length == 5)
                {
                    ck = "PI" + "-" + value.ToString();
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
            try
            {
                if (dataGridView1.CurrentCell.ColumnIndex == 2 || dataGridView1.CurrentCell.ColumnIndex == 4)
                {
                    if (e.KeyChar == (char)Keys.Escape)
                    {
                        this.Close();
                    }
                }
            }
            catch { }
        }

        private void close(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
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
                if (!txtscan.Text.Equals("")) 
                {
                    String g2 = "select CATEGORY,NAME,Size,Referencecode from add_product where productcode='" + txtscan.Text + "' and principleName='" + txtname.Text + "'";
                    DataTable d3 = clsDataLayer.RetreiveQuery(g2);
                    if (d3.Rows.Count > 0)
                    {
                        String Dbpdname = d3.Rows[0][1].ToString(); String Dbsize = d3.Rows[0][2].ToString();

                        bool h = false; int GetRow = 0;
                        for (int r = 0; r < dataGridView1.Rows.Count; r++)
                        {
                            String gprod = dataGridView1.Rows[r].Cells[2].Value.ToString(); String gsize = dataGridView1.Rows[r].Cells[3].Value.ToString();
                            String tvalue = Dbpdname;
                            if (gprod.Equals(tvalue) && Dbsize.Equals(gsize))
                            {
                                h = true; GetRow = Convert.ToInt32(r);
                            }
                        }

                        if (h == false)
                        {
                            dataGridView1.Rows.Add();
                            int nn = dataGridView1.Rows.Count; --nn;
                            dataGridView1.Rows[nn].Cells[0].Value = txtcode.Text;
                            dataGridView1.Rows[nn].Cells[1].Value = d3.Rows[0][0].ToString();
                            dataGridView1.Rows[nn].Cells[2].Value = d3.Rows[0][1].ToString();
                            dataGridView1.Rows[nn].Cells[3].Value = d3.Rows[0][2].ToString();
                            dataGridView1.Rows[nn].Cells[4].Value = d3.Rows[0][3].ToString();
                            dataGridView1.Rows[nn].Cells[5].Value = "1"; txtscan.Text = "";
                        }
                        else
                        {
                            decimal gq1 = Convert.ToDecimal(dataGridView1.Rows[GetRow].Cells[5].Value); gq1++;

                            dataGridView1.Rows[GetRow].Cells[5].Value = gq1.ToString(); txtscan.Text = "";
                        }
                        //    SearchProduct(txtscan.Text, "Textbox");

                    }
                    else
                    {
                       // txtscan.Text = ""; MessageBox.Show("No Record Found!","Stop",MessageBoxButtons.OK,MessageBoxIcon.Stop); 
                    }
                }

            }
            catch { }
        }

        private void btnaddrow_Click(object sender, EventArgs e)
        {
            addrow();
        }

        public void addrow()
        {
            try
            {
                dataGridView1.Rows.Add();
                int nm = dataGridView1.Rows.Count;
                nm--;
                dataGridView1.Rows[nm].Cells[0].Value = txtcode.Text;
            }
            catch { }
        }

        private void txtlord_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!txtlord.Text.Equals(""))
                {
                    String m1 = "select * from PurchaseInq where LordNumber='" + txtlord.Text + "' and PIurCode='" + txtcode.Text + "'";
                    DataTable d1 = clsDataLayer.RetreiveQuery(m1);
                    if (d1.Rows.Count > 0) { MessageBox.Show("Lord Number Already Exist.!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); txtlord.Text = ""; }
                }
            }
            catch { }
        }

        private void btn_demand_Click(object sender, EventArgs e)
        {
            try
            {
                demand_details frm = new demand_details(this);
                frm.ShowDialog();
            }
            catch
            {


            }
        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }




        //LedgerUpdate Closed

    }
}
