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

namespace GrayLark
{
    public partial class Add_Product_Type_And_Category : Form
    {
        String UID = Login.UserID;
        public string GreenSignal = "";
        public string FormID = "";
        public Add_Product_Type_And_Category()
        {
            InitializeComponent(); this.KeyPreview = true;
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
        

        private void btnSave_Click(object sender, EventArgs e)
        {
            FormID = "Save";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                if (!CheckAllFieldscat())
                {
                    try
                    {
                        String j = "select * from product_cat where PRODUCT_CATEGORY='" + txtPCat.Text + "'";
                    DataTable d1 = clsDataLayer.RetreiveQuery(j);
                    if (d1.Rows.Count > 0)
                    {
                        MessageBox.Show("Already Added!","Error",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    }
                    else
                    {
                        string query = "INSERT INTO product_cat (PRODUCT_CATEGORY, USERNAME, DATETIME) VALUES ('" + txtPCat.Text + "','" + UID + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        con.Open();
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    MessageBox.Show(txtPCat.Text + " is now Saved in Product Category!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPCat.Clear();   category();
                }
                else
                {
                    MessageBox.Show("Please fill all fields","Error",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                }
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
         
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
             
        }

        private void Add_Product_Type_And_Category_Load(object sender, EventArgs e)
        {
            category();
        }


        private void category()
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM product_cat", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView2.Rows.Clear();
                foreach (DataRow item in dt.Rows)
                {
                    int i = dataGridView2.Rows.Add();
                    dataGridView2.Rows[i].Cells[0].Value = item["PRODUCT_CATEGORY"].ToString();
                    dataGridView2.Rows[i].Cells[1].Value = item["USERNAME"].ToString();
                }
            }
            catch {  }
        }
          
        private void txtPCat_Leave(object sender, EventArgs e)
        {
            txtPCat.Text = txtPCat.Text.ToUpper();
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormID = "Add";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                Add_Product ap = new Add_Product();
                ap.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
     
        } 
        private bool CheckAllFieldscat()
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

        private void txtPCat_Leave_1(object sender, EventArgs e)
        {
            txtPCat.Text = txtPCat.Text.ToUpper();
        }

        private void close(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
         

        private void txtPType_TextChanged(object sender, EventArgs e)
        {
            //txtPType.Text = txtPType.Text.ToUpper();
        }

        private void txtPCat_TextChanged(object sender, EventArgs e)
        {
           // txtPCat.Text = txtPCat.Text.ToUpper();
        }

    
        private void Add_Product_Type_And_Category_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.C)
            {
                btnSave.PerformClick();
            }
        }        
    }
}
