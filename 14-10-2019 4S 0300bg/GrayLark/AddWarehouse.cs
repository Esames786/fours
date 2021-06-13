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
    public partial class AddWarehouse : Form
    {  public string GreenSignal = "";
        public string FormID = "";
        String Cus = "";
        String UID = Login.UserID;
        public AddWarehouse()
        {
            InitializeComponent();
            BankDetailShow(); clear.Enabled = true; submit.Enabled = false; this.KeyPreview = true;
        }

        private void clear_Click(object sender, EventArgs e)
        {  FormID = "Add";
            UserNametesting();
            if (GreenSignal == "YES")
            {
            txtwname.Enabled = true; 
            txtwname.Text = ""; clear.Enabled = false; submit.Enabled = true;
            txtid.Text = clsGeneral.getMAXCode("tbl_warehouse", "W_Code", "WH"); txtwname.Focus();
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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

        private void BankDetailShow()
        {
            String abc = "select W_Code,WarehouseName,Dates,UserName from tbl_warehouse";
            DataTable ds = clsDataLayer.RetreiveQuery(abc);
            if(ds.Rows.Count > 0)
            {
                dataGridView1.DataSource = ds;
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

         private void Save2()
         {
             String insert = "insert into tbl_warehouse(W_Code,WarehouseName,Dates,UserName) values('" + txtid.Text + "','" + txtwname.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Login.UserID + "')";
             if (clsDataLayer.ExecuteQuery(insert) > 0)
             {
                
                 txtwname.Enabled = false; BankDetailShow(); clear.Enabled = true; submit.Enabled = false;
             }
         }

        private void submit_Click(object sender, EventArgs e)
        {FormID = "Save";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                if (!CheckAllFields())
                {
                    txtid.Text = clsGeneral.getMAXCode("tbl_warehouse", "W_Code", "WH");
                    String sel = "select * from tbl_warehouse where WarehouseName='" + txtwname.Text + "'";
                    DataTable dt = clsDataLayer.RetreiveQuery(sel);
                    if (dt.Rows.Count < 1)
                    {
                        Save2(); MessageBox.Show("Inserted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        String d3 = "delete from tbl_warehouse where WarehouseName='" + txtwname.Text + "'"; clsDataLayer.ExecuteQuery(d3); Save2(); MessageBox.Show("Updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void AddBankDetail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
         
        private void txtbac_KeyPress_1(object sender, KeyPressEventArgs e)
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

        private void AddBankDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.A)
            {
                clear.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.S)
            {
                submit.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        txtid.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        String sel = "select WarehouseName from tbl_warehouse where W_Code='" + txtid.Text + "'";
        DataTable dt = clsDataLayer.RetreiveQuery(sel);
        if (dt.Rows.Count > 0)
        {
            txtwname.Text = dt.Rows[0][0].ToString(); txtwname.Enabled = true; submit.Enabled = true;
        }
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            try
            {
                String get = "select * from tbl_warehouse where W_Code='" + txtid.Text + "'";
                DataTable d1 = clsDataLayer.RetreiveQuery(get);
                if (d1.Rows.Count > 0)
                {
                    GrayLark.bin.Debug.Report.rptwarehouse rpt = new rptwarehouse();
                    rpt.SetDataSource(d1);
                    rptWarehouseview frm = new rptWarehouseview(rpt,txtid.Text);
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }
         
    
    }
}
