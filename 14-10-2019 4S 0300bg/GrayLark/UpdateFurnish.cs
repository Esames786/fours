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
    public partial class UpdateFurnish : Form
    {
        TextBox tb = new TextBox();
        String UID = Login.UserID; String Code = "";
        public string GreenSignal = "";
        public string FormID = ""; decimal ot = 0;
        public UpdateFurnish()
        {
        InitializeComponent(); Onload(); txt_search.Enabled = true;
        this.KeyPreview = true;
        }

        private void Onload()
        {
            refresh();
            String get = "select ActTitle from Accounts where HeaderActCode='2' and Status='1' and ID > 24 Order BY ID DESC"; DataTable df = clsDataLayer.RetreiveQuery(get);
            if (df.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txtproduct, df); }
            String h3 = "select Up_Code from tbl_UpdFurnish";
            DataTable d3 = clsDataLayer.RetreiveQuery(h3); if (d3.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txt_search, d3); }
            btnEdit.Enabled = false;
            btnUpdate.Enabled = false;
            btnsave.Enabled = false;
            String h2 = "select SProduct from tbl_ColdStorageMaintain"; DataTable d5 = clsDataLayer.RetreiveQuery(h2);
            if (d5.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txtproduct, d5); clsGeneral.SetAutoCompleteTextBox(txtproduct2, d5);

            }
            Disable();
        }

        //
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
            txt_search.Enabled = true;
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

        private void New_Click(object sender, EventArgs e)
        {
        try
        {
        FormID = "Add";
        UserNametesting();
        if (GreenSignal == "YES")
        {
            Clears(); Enable();
       btnsave.Enabled = true; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = false;  
        txtcode.Text = clsGeneral.getMAXCode("tbl_UpdFurnish", "Up_Code", "UP");
        txt_search.Text = "-"; txtproduct.Focus();
        }  else  { MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);  }
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
         
        private void Save()
        {
        if (!CheckAllFields())
        {
            btnsave.Enabled = false;
            txtcode.Text = clsGeneral.getMAXCode("tbl_UpdFurnish", "Up_Code", "UP");
            String ins = "insert into tbl_UpdFurnish(Sproduct_Name,Remarks,Up_Code,Product_Name,Quantity,Dates,UserName)values('"+txtproduct2.Text+"','" + txtremarks.Text + "','" + txtcode.Text + "','" + txtproduct.Text + "','" + txtquant.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Login.UserID + "')";
        clsDataLayer.ExecuteQuery(ins);
            refresh();
            decimal odq = 0; decimal fdq = 0; decimal idq = 0;
            idq = Convert.ToDecimal(txtquant.Text);
            String q1 = "select SQuantity from tbl_ColdStorageMaintain where SProduct='" + txtproduct.Text + "'"; DataTable dq1 = clsDataLayer.RetreiveQuery(q1);
            if (dq1.Rows.Count > 0)
            {
          odq = Convert.ToDecimal(dq1.Rows[0][0].ToString());
            } 
             fdq = odq - idq;
             String Query9 = "update tbl_ColdStorageMaintain set SQuantity=" + fdq + " where SProduct='" + txtproduct.Text + "'"; clsDataLayer.ExecuteQuery(Query9);
     
 //
             String q10 = "select SQuantity from tbl_ColdStorageMaintain where SProduct='" + txtproduct2.Text + "'"; DataTable dq10 = clsDataLayer.RetreiveQuery(q10);
            if (dq10.Rows.Count > 0)
            {
                odq = Convert.ToDecimal(dq10.Rows[0][0].ToString());
            }
            fdq = odq + idq;
            String Query = "update tbl_ColdStorageMaintain set SQuantity=" + fdq + " where SProduct='" + txtproduct2.Text + "'"; clsDataLayer.ExecuteQuery(Query);
     
            //

            MessageBox.Show("Voucher Save Successfully!","Success",MessageBoxButtons.OK,MessageBoxIcon.Information); dataGridView1.Refresh();
       btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = true;
       Disable();
        }
        else { MessageBox.Show("Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Update()
        {
        if (!CheckAllFields())
        {

        decimal dbq = 0;
        String n1 = "select Quantity from tbl_UpdFurnish where Up_Code='" + txtcode.Text + "'"; DataTable n2 = clsDataLayer.RetreiveQuery(n1);  
        if (n2.Rows.Count > 0) {   dbq = Convert.ToDecimal(n2.Rows[0][0].ToString());  }
       
        decimal odq = 0; decimal fdq = 0; decimal idq = 0;
        idq = Convert.ToDecimal(txtquant.Text);
        String q1 = "select SQuantity from tbl_ColdStorageMaintain where SProduct='" + txtproduct.Text + "'"; DataTable dq1 = clsDataLayer.RetreiveQuery(q1);
        if (dq1.Rows.Count > 0)
        {
         odq = Convert.ToDecimal(dq1.Rows[0][0].ToString());
        } 
       fdq = odq + dbq - idq;
 
       String Query = "update tbl_ColdStorageMaintain set SQuantity=" + fdq + " where SProduct='" + txtproduct.Text + "'"; clsDataLayer.ExecuteQuery(Query);
       
            //
       String q10 = "select SQuantity from tbl_ColdStorageMaintain where SProduct='" + txtproduct2.Text + "'"; DataTable dq10 = clsDataLayer.RetreiveQuery(q10);
       if (dq10.Rows.Count > 0)
       {
           odq = Convert.ToDecimal(dq10.Rows[0][0].ToString());
       }
       fdq = odq - dbq + idq;

       String Query9 = "update tbl_ColdStorageMaintain set SQuantity=" + fdq + " where SProduct='" + txtproduct2.Text + "'"; clsDataLayer.ExecuteQuery(Query9);
       
            //


       String d1 = "delete from tbl_UpdFurnish where Up_Code='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(d1);

       String ins = "insert into tbl_UpdFurnish(Sproduct_Name,Remarks,Up_Code,Product_Name,Quantity,Dates,UserName)values('" + txtproduct2.Text + "','" + txtremarks.Text + "','" + txtcode.Text + "','" + txtproduct.Text + "','" + txtquant.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Login.UserID + "')";
       clsDataLayer.ExecuteQuery(ins);
       refresh();  MessageBox.Show("Voucher Update Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
       btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = true;Disable();
        }
        else { MessageBox.Show("Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
         
        private void btnsave_Click(object sender, EventArgs e)
        {
        try
        {   
            //
        FormID = "Save";
        UserNametesting();
        if (GreenSignal == "YES")
        { 
        Save(); 
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
        if (!CheckAllFields())
        {
       FormID = "Update";  UserNametesting();
        if (GreenSignal == "YES")
        { 
        Update(); 
        }
        else {   MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);    }
        }
        else { MessageBox.Show("Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnEdit_Click(object sender, EventArgs e){ 
                 FormID = "Edit";
        UserNametesting();
        if (GreenSignal == "YES")
        {
            Enable();
           btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = true;  txtremarks.Enabled = true; txtproduct.Enabled = false; 
        }
        else { MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
         } 

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        if (txt_search.Text == "")
        {
            refresh();
        }
        else
        { 
            String hn = "SELECT distinct(Up_Code) FROM tbl_UpdFurnish Where Up_Code = '" + txt_search.Text + "'";
        DataTable dt = clsDataLayer.RetreiveQuery(hn);
        if (dt.Rows.Count > 0)
        {
        dataGridView1.DataSource = null; dataGridView1.DataSource = dt;
        }
        } 
        }

        public void refresh()
        {
            String sel = "SELECT distinct(Up_Code) FROM tbl_UpdFurnish"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
        if (ds.Rows.Count > 0)    {  dataGridView1.DataSource = null; dataGridView1.DataSource = ds;   } 
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
            btnsave.Enabled = false;
            btnEdit.Enabled = true;
            btnUpdate.Enabled = false; 
            txt_search.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
 
            String sel = "SELECT distinct(Up_Code) FROM tbl_UpdFurnish Where Up_Code ='" + txt_search.Text + "'"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
            if (ds.Rows.Count > 0)
            {
                txtcode.Text = ds.Rows[0][0].ToString();
            }

            String vb = "select Product_Name,Quantity,Status,Remarks from tbl_UpdFurnish where Up_Code='" + txt_search.Text + "'"; DataTable fd = clsDataLayer.RetreiveQuery(vb);
            if (fd.Rows.Count > 0)
            {
            txtproduct.Text = fd.Rows[0][0].ToString();
            txtquant.Text = fd.Rows[0][1].ToString();
            txtremarks.Text = fd.Rows[0][3].ToString();
            } 
              String gb = "SELECT distinct(Up_Code) FROM tbl_UpdFurnish";
            DataTable dt = clsDataLayer.RetreiveQuery(gb);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = null; dataGridView1.DataSource = dt;
            } txtremarks.Enabled = false; txtproduct.Enabled = false;
            
            } catch { } 
        }

        private void close(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
          
        private void btnprint_Click(object sender, EventArgs e)
        {
        try
        {
        String query = "";
        if (!txtcode.Text.Equals("")) { query = "select * from tbl_UpdFurnish where Up_Code='" + txtcode.Text + "'"; }
        else { query = "select * from tbl_UpdFurnish"; }
        DataTable d1 = clsDataLayer.RetreiveQuery(query);
        if (d1.Rows.Count > 0)
        {
     rptupdatefurnish rpt = new rptupdatefurnish(); rpt.SetDataSource(d1); PaymentView pv = new PaymentView(rpt); pv.Show();
        }
        else
        {
        MessageBox.Show("No Record Found !","Error",MessageBoxButtons.OK,MessageBoxIcon.Error); 
        }
        }
        catch{ }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
      dataGridView1.Rows.Clear(); txtproduct.Text = ""; btnsave.Enabled = false; New.Enabled = true;
        }

        private void txtquant_KeyPress(object sender, KeyPressEventArgs e)
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

        private void UpdateFurnish_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.A)
            {
                New.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.S)
            {
                btnsave.PerformClick();
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
                btnprint.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        private void txtproduct_Leave(object sender, EventArgs e)
        {
            txtquant.Focus();
        }

        private void txtproduct2_Leave(object sender, EventArgs e)
        {
            txtremarks.Focus();
        }
         
    }
}
