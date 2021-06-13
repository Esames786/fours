
using GrayLark.bin.Debug.Report;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrayLark
{
    public partial class DamageProduct : Form
    {
        TextBox tb = new TextBox();

        String UID = Login.UserID;
        public string GreenSignal = "";
        public string FormID = "";
        public DamageProduct()
        {
            InitializeComponent();
            onsearch(); GridRefresh(); btnSave.Enabled = false; btnupdate.Enabled = false; btnedit.Enabled = true; Disable(); 
            this.KeyPreview = true;
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
        }
        

        private void onsearch()
        {
            String qs = "select distinct ProductName from Vdamage"; DataTable ds = clsDataLayer.RetreiveQuery(qs);
            if (ds.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txt_search, ds);
            }
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

        private void Clears()
        {
            foreach (Control c in this.tableLayoutPanel1.Controls)
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
        private void GridRefresh(){
            String gb = "select distinct TCS_CODE,Date from tbl_DamageProduct"; DataTable ds = clsDataLayer.RetreiveQuery(gb);
            if(ds.Rows.Count > 0){dataGridView1.DataSource=ds;}
        }
        private void OnNew() { grid.Rows.Clear(); grid.Rows.Add(); Clears(); Enable(); txt_id.Text = clsGeneral.getMAXCode("tbl_DamageProduct", "TCS_CODE", "DP"); }

         

        private void OnSave() {
       if (!CheckAllFields())
       {
           CheckProd();
        if (pcs == true)
        {
            txt_id.Text = clsGeneral.getMAXCode("tbl_DamageProduct", "TCS_CODE", "DP");

            String ins = "insert into tbl_DamageProduct(TCS_CODE,Username,Date,Remarks)values('" + txt_id.Text + "','" + Login.UserID + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + txt_remarks.Text + "')"; clsDataLayer.ExecuteQuery(ins);
           
    for (int i = 0; i < grid.Rows.Count; i++)
   {
       String fs = "insert into tbl_DamageProductDetail(TCS_CODE,ProductName,CATEGORY,Size,LordNumber,Warehouse,Quantity)values('" + txt_id.Text + "','" + grid.Rows[i].Cells[0].Value + "','" + grid.Rows[i].Cells[1].Value + "','" + grid.Rows[i].Cells[2].Value + "','" + grid.Rows[i].Cells[3].Value + "','" + grid.Rows[i].Cells[4].Value + "'," + grid.Rows[i].Cells[5].Value + ")"; clsDataLayer.ExecuteQuery(fs);
       String getq = "select Quantity from add_product_stock where NAME='" + grid.Rows[i].Cells[0].Value + "' and CATEGORY='" + grid.Rows[i].Cells[1].Value + "' and LordNumber='" + grid.Rows[i].Cells[3].Value + "' and  Size='" + grid.Rows[i].Cells[2].Value + "' and WarehouseName='" + grid.Rows[i].Cells[4].Value + "'"; DataTable dq = clsDataLayer.RetreiveQuery(getq);
       if (dq.Rows.Count > 0)
       {
           decimal dbq = Convert.ToDecimal(dq.Rows[0][0]); decimal gq = Convert.ToDecimal(grid.Rows[i].Cells[5].Value);
           decimal fp = dbq - gq;
           String u6 = "update add_product_stock set Quantity=" + fp + " where NAME='" + grid.Rows[i].Cells[0].Value + "' and CATEGORY='" + grid.Rows[i].Cells[1].Value + "' and LordNumber='" + grid.Rows[i].Cells[3].Value + "' and  Size='" + grid.Rows[i].Cells[2].Value + "' and WarehouseName='" + grid.Rows[i].Cells[4].Value + "'"; clsDataLayer.ExecuteQuery(u6);
       }
    }
             
           MessageBox.Show("Voucher Saved! ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); Disable(); GridRefresh();
           btnedit.Enabled = true; btnSave.Enabled = false; btnupdate.Enabled = false; btnNew.Enabled = true; onsearch();
        }
        else
        {
            MessageBox.Show("Product Name is Incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
       }
       else { MessageBox.Show("Fill All Fields! ", "Alert", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error); }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
           FormID = "Add";
            UserNametesting();
            if (GreenSignal == "YES")
            {  OnNew(); btnedit.Enabled = false; btnSave.Enabled = true; btnupdate.Enabled = false; btnNew.Enabled = false; txt_search.Text = "-";
            txt_search.Text = "-";  
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            }

        private void UpdateQuant()
        {
                String del = "delete from tbl_DamageProduct where TCS_CODE='" + txt_id.Text + "'"; clsDataLayer.ExecuteQuery(del);
            
                txt_id.Text = clsGeneral.getMAXCode("tbl_DamageProduct", "TCS_CODE", "DP");

                String ins = "insert into tbl_DamageProduct(TCS_CODE,Username,Date,Remarks)values('" + txt_id.Text + "','" + Login.UserID + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + txt_remarks.Text + "')"; clsDataLayer.ExecuteQuery(ins);

                for (int i = 0; i < grid.Rows.Count; i++)
                {
                    decimal ocd = 0;
                    String m4 = "select Quantity from tbl_DamageProductDetail where TCS_CODE='" + txt_id.Text + "' and ProductName='" + grid.Rows[i].Cells[0].Value + "' and CATEGORY='" + grid.Rows[i].Cells[1].Value + "' and LordNumber='" + grid.Rows[i].Cells[3].Value + "' and  Size='" + grid.Rows[i].Cells[2].Value + "' and Warehouse='" + grid.Rows[i].Cells[4].Value + "'";
                    DataTable m56 = clsDataLayer.RetreiveQuery(m4);
                    if (m56.Rows.Count > 0)
                    {
                        ocd = Convert.ToDecimal(m56.Rows[0][0].ToString());
                    }
                    String getq = "select Quantity from add_product_stock where NAME='" + grid.Rows[i].Cells[0].Value + "' and CATEGORY='" + grid.Rows[i].Cells[1].Value + "' and LordNumber='" + grid.Rows[i].Cells[3].Value + "' and  Size='" + grid.Rows[i].Cells[2].Value + "' and WarehouseName='" + grid.Rows[i].Cells[4].Value + "'"; DataTable dq = clsDataLayer.RetreiveQuery(getq);
                    if (dq.Rows.Count > 0)
                    {
                        decimal dbq = Convert.ToDecimal(dq.Rows[0][0]); decimal gq = Convert.ToDecimal(grid.Rows[i].Cells[5].Value);
                        decimal fp = dbq +ocd - gq;
                        String u6 = "update add_product_stock set Quantity=" + fp + " where NAME='" + grid.Rows[i].Cells[0].Value + "' and CATEGORY='" + grid.Rows[i].Cells[1].Value + "' and LordNumber='" + grid.Rows[i].Cells[3].Value + "' and  Size='" + grid.Rows[i].Cells[2].Value + "' and WarehouseName='" + grid.Rows[i].Cells[4].Value + "'"; clsDataLayer.ExecuteQuery(u6);
                    }
                    String del3 = "delete from tbl_DamageProductDetail where TCS_CODE='" + txt_id.Text + "' and ProductName='" + grid.Rows[i].Cells[0].Value + "' and CATEGORY='" + grid.Rows[i].Cells[1].Value + "' and LordNumber='" + grid.Rows[i].Cells[3].Value + "' and  Size='" + grid.Rows[i].Cells[2].Value + "' and Warehouse='" + grid.Rows[i].Cells[4].Value + "'"; clsDataLayer.ExecuteQuery(del3);
                    String fs = "insert into tbl_DamageProductDetail(TCS_CODE,ProductName,CATEGORY,Size,LordNumber,Warehouse,Quantity)values('" + txt_id.Text + "','" + grid.Rows[i].Cells[0].Value + "','" + grid.Rows[i].Cells[1].Value + "','" + grid.Rows[i].Cells[2].Value + "','" + grid.Rows[i].Cells[3].Value + "','" + grid.Rows[i].Cells[4].Value + "'," + grid.Rows[i].Cells[5].Value + ")"; clsDataLayer.ExecuteQuery(fs);
                }
            
            MessageBox.Show("Voucher Updated! ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Disable(); GridRefresh(); btnedit.Enabled = true; btnSave.Enabled = false; btnupdate.Enabled = false; btnNew.Enabled = true; onsearch();
        }

        //On Update
        private void button3_Click(object sender, EventArgs e)
        {
               FormID = "Update";  UserNametesting();
            if (GreenSignal == "YES")
            {
                if (!CheckAllFields() && !CheckDataGridCells(grid))
                {
        CheckProd();
        if (pcs == true)
        { UpdateQuant(); txt_search.Enabled = true; }
        else
        {
            MessageBox.Show("Product Name is Incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        }
        else { MessageBox.Show("Fill All Fields! ", "Alert", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error); }
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
            }

        bool pcs = true;
        private void CheckProd()
        {
            pcs = true;
            for (int i = 0; i < grid.RowCount; i++)
            {
                String pd = grid.Rows[i].Cells[0].Value.ToString();
                String Db = "select * from add_product_stock where NAME = '" + pd + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(Db); if (d1.Rows.Count > 0)
                {  }  else { pcs = false; }
            }
        }

        private bool CheckDataGridCells(DataGridView dgv)
        {
            bool flag = false;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < 6; j++)
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


        private void btnSave_Click(object sender, EventArgs e)
        {
              FormID = "Save";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    if (!CheckAllFields() && !CheckDataGridCells(grid))
                    {
                        OnSave(); txt_search.Enabled = true;
        }
       else { MessageBox.Show("Fill All Fields! ", "Alert", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error); }
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                }

        private void btnedit_Click(object sender, EventArgs e)
        {
                    FormID = "Edit";
        UserNametesting();
        if (GreenSignal == "YES")
        {
            btnedit.Enabled = false; btnSave.Enabled = false; btnupdate.Enabled = true; btnNew.Enabled = true; Enable(); txt_search.Text = "-";
             }
        else { MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
        
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try { txt_id.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); btnedit.Enabled = true;  
            }
            catch { }
        }

        private void txt_id_TextChanged(object sender, EventArgs e)
        {
            try
            {
          
                String sel = "select ProductName,Quantity,Remarks,CATEGORY,Size,LordNumber,Warehouse from Vdamage where TCS_CODE='" + txt_id.Text + "'"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
                if (ds.Rows.Count > 0) {  
                    grid.Rows.Clear();
                    foreach (DataRow dr in ds.Rows)
                    {
                        int n=grid.Rows.Add();
                        grid.Rows[n].Cells[0].Value = dr[0].ToString();
                        grid.Rows[n].Cells[1].Value = dr[3].ToString(); //Category
                        grid.Rows[n].Cells[2].Value = dr[4].ToString(); //Size
                        grid.Rows[n].Cells[3].Value = dr[5].ToString(); //LordNumber
                        grid.Rows[n].Cells[4].Value = dr[6].ToString(); //Warehouse
                        grid.Rows[n].Cells[5].Value = dr[1].ToString(); //Quantity
                     
                    }
                    txt_remarks.Text = ds.Rows[0][2].ToString(); 
                }
            }
            catch { }
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (grid.CurrentCell.ColumnIndex == 5)
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        grid.Rows.Add();
                        int yhs = grid.CurrentCell.RowIndex;
                        grid.CurrentCell = grid.Rows[yhs].Cells[0];
                        grid.BeginEdit(true);
                    }
                    else if (e.KeyCode == Keys.Delete)
                    {
                        if (grid.Rows.Count > 0)
                        {
                            int yh = grid.CurrentCell.RowIndex;
                            grid.Rows.RemoveAt(yh);
                        }
                    }
                }
                else
                {
                    if (e.KeyCode == Keys.Delete)
                    {
                        if (grid.Rows.Count > 0)
                        {
                            int yh = grid.CurrentCell.RowIndex;
                            grid.Rows.RemoveAt(yh);
                        }
                    }
                } 
            }
            catch { }
        }

        private void grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //
            try
            {
                int yy = grid.CurrentCell.ColumnIndex;
                string columnHeaders = grid.Columns[yy].HeaderText;
                if (columnHeaders.Equals("Product Name"))
                {
                    tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        String hname = "select distinct NAME from add_product_stock where Status='Active'";
                        
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
                        hname = "select distinct Size from add_product_stock where Status='Active'";
                        
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

                        hname = "select distinct CATEGORY from add_product_stock where Status='Active'";
                         
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
                else if (columnHeaders.Equals("Lord Number"))
                {
                    tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        String hname = "";

                        hname = "select distinct LordNumber from add_product_stock where Status='Active'";
  
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
                else if (columnHeaders.Equals("WareHouse"))
                {
                    tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        String hname = "";

                        hname = "select distinct WarehouseName from add_product_stock where Status='Active'";

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
                if (grid.CurrentCell.ColumnIndex == 5) //Desired Column
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

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            try
            {
            String qs = "select TCS_CODE,ProductName,Quantity,Remarks from Vdamage where ProductName='" + txt_search.Text + "'"; DataTable ds = clsDataLayer.RetreiveQuery(qs);
            if (ds.Rows.Count > 0)   {  dataGridView1.DataSource = ds;  }
            }
            catch { }
        }

        private void grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (grid.CurrentCell.ColumnIndex == 5)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                { e.Handled = true; }
                else if (e.KeyChar == (char)Keys.Escape)
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                FormID = "Print";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    String qs = "";
                    if (!txt_id.Text.Equals(""))
                    {
                        qs = "select * from Vdamage where TCS_CODE='" + txt_id.Text + "'";
                    }
                    else
                    {
                        qs = "select * from Vdamage";  
                    }
                    DataTable d1 = clsDataLayer.RetreiveQuery(qs); if (d1.Rows.Count > 0)
                    {
                        rptDamageProduct fp = new rptDamageProduct(); fp.SetDataSource(d1); PaymentView pv = new PaymentView(fp); pv.Show();
                    }
                    else { MessageBox.Show("No Record Found! ", "Alert", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error); }
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            } catch { }
        }

        private void btnref_Click(object sender, EventArgs e)
        {
            btnNew.Enabled = true; btnedit.Enabled = false; btnSave.Enabled = false; btnupdate.Enabled = false;
        }

        private void DamageProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.A)
            {
                btnNew.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.S)
            {
                btnSave.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.U)
            {
                btnupdate.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.E)
            {
                btnedit.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.P)
            {
                btnPrint.PerformClick();
            }
        }

        private void txt_outlet_Leave(object sender, EventArgs e)
        {
            grid.Focus();
        }

        private void grid_Leave(object sender, EventArgs e)
        {
            txt_remarks.Focus();
        }

        private void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (grid.CurrentCell.ColumnIndex == 3)
                {
                    String fet = "select WarehouseName from add_product_stock where LordNumber='" + grid.CurrentRow.Cells[3].Value + "'"; DataTable df = clsDataLayer.RetreiveQuery(fet);
                    if (df.Rows.Count > 0)
                    {
                        grid.CurrentRow.Cells[4].Value = df.Rows[0][0].ToString();
                    }
                }
            }
            catch { }
        }
    }
}
