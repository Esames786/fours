
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
    public partial class User_Access : Form
    {
        String UID = Login.UserID;
        public string GreenSignal = "";
        public string FormID = "";
        SqlConnection con;
        public User_Access()
        {
        InitializeComponent();
        con = null;
        if (con == null)
        {
            con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        }
        String qs = "select USERNAME from Login where STATUS='Active'"; DataTable ds = clsDataLayer.RetreiveQuery(qs); clsGeneral.SetAutoCompleteTextBox(txtUserSearch, ds);
        this.KeyPreview = true;
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
        private void User_Access_Load(object sender, EventArgs e)
        {
            UserID();
            FormIDs();
            Users();
        }

        private void Users()
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM Login ORDER BY ID ASC", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                dataGridView2.Rows.Clear();

                foreach (DataRow item in dt.Rows)
                {
                    int i = dataGridView2.Rows.Add();

                    dataGridView2.Rows[i].Cells[0].Value = item["ID"].ToString();
                    dataGridView2.Rows[i].Cells[1].Value = item["USERNAME"].ToString();
                }
                dataGridView2.PerformLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormIDs()
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM form_id_name", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                dataGridView1.Rows.Clear();

                foreach (DataRow item in dt.Rows)
                {
                    int i = dataGridView1.Rows.Add();

                    dataGridView1.Rows[i].Cells[0].Value = item["FORM_ID"].ToString();
                    dataGridView1.Rows[i].Cells[1].Value = item["FORM_NAME"].ToString();
                }
                dataGridView1.PerformLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UserID()
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM Login ORDER BY ID ASC", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboUserID.Items.Add(dt.Rows[i]["ID"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboUserID_Click(object sender, EventArgs e)
        {
            comboUserID.Items.Clear();
            UserID();
        }

        private void comboUserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM Login WHERE ID = '" + comboUserID.Text + "'", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0][0].ToString();
                    dataGridView1.Rows.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            { 
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT USER_ID FROM user_access WHERE USER_ID = @User", con);
                cmd.Parameters.AddWithValue("@User", comboUserID.Text);
                SqlDataReader sdr = cmd.ExecuteReader();

 if (sdr.HasRows)
 {
 sdr.Close();
 try
 {
     SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM user_access WHERE USER_ID = '" + comboUserID.Text + "'", con);

     DataTable dt = new DataTable();
     sda.Fill(dt);

     dataGridView1.Rows.Clear();

     foreach (DataRow item in dt.Rows)
     {
         int i = dataGridView1.Rows.Add();

         dataGridView1.Rows[i].Cells[0].Value = item["FORM_ID"].ToString();
         dataGridView1.Rows[i].Cells[1].Value = item["FORM_NAME"].ToString();
         dataGridView1.Rows[i].Cells[2].Value = item["ACCESS"].ToString();
     }
     dataGridView1.PerformLayout();
 }catch{}

           DataTable dt3 = new DataTable();
           dt3.Columns.Add("FORM_ID");
           dt3.Columns.Add("FORM_NAME");
           dt3.Columns.Add("ACCESS");
           bool abc = false;
           String FormId = ""; String FormId1 = ""; String FNAME = ""; String ACCESS = "";
           String query = "select FORM_ID,FORM_NAME from form_id_name"; DataTable dt1 = clsDataLayer.RetreiveQuery(query); if (dt1.Rows.Count > 0) { }
           String query1 = "select FORM_ID,FORM_NAME from user_access where USER_ID='" + comboUserID.Text + "'"; DataTable dt2 = clsDataLayer.RetreiveQuery(query1); if (dt2.Rows.Count > 0) {
           for (int h = 0; h < dt1.Rows.Count; h++) {
               //
               FormId = dt1.Rows[h][0].ToString();
               FNAME = dt1.Rows[h][1].ToString();
               abc = false;
               for (int h1 = 0; h1 < dt2.Rows.Count; h1++)
               {
                   FormId1 = dt2.Rows[h1][0].ToString();
                   if (FormId.Equals(FormId1)) { abc = true; } 
               }
               if (abc == false)
               {
                   dt3.Rows.Add(new string[] { FormId, FNAME, "" });
               }
                   //
           }
           foreach (DataRow item in dt3.Rows)
           {
               int i = dataGridView1.Rows.Add();

               dataGridView1.Rows[i].Cells[0].Value = item["FORM_ID"].ToString();
               dataGridView1.Rows[i].Cells[1].Value = item["FORM_NAME"].ToString();
               dataGridView1.Rows[i].Cells[2].Value = item["ACCESS"].ToString();
           }
               
 }

 }
 else 
 {
                    sdr.Close();
                    FormIDs();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdateAcess_Click(object sender, EventArgs e)
        {
            FormID = "Update";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                if (btnUpdateAcess.Text == "Update User Access")
                {
                    btnUpdateAcess.Text = "Done";
                    Enable();
                }
                else if (btnUpdateAcess.Text == "Done")
                {
                    btnUpdateAcess.Text = "Update User Access"; try
                    {
                        string query = "";
                        con.Open();
                        query = "DELETE FROM user_access Where USER_ID = '" + comboUserID.Text + "'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    } try
                    {
                        string query = "";
                        con.Open();
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            query = " INSERT INTO user_access (USER_ID, USENAME, FORM_ID, FORM_NAME, ACCESS) ";
                            query = query + " VALUES ('" + comboUserID.Text + "','" + txtName.Text + "','";

                            if (dataGridView1.Rows[i].Cells[0].Value == null)
                            {
                                query = query + "" + "','";
                            }
                            else
                            {
                                query = query + dataGridView1.Rows[i].Cells[0].Value.ToString() + "','";

                            }

                            if (dataGridView1.Rows[i].Cells[1].Value == null)
                            {
                                query = query + "" + "','";
                            }
                            else
                            {
                                query = query + dataGridView1.Rows[i].Cells[1].Value.ToString() + "','";
                            }

                            if (dataGridView1.Rows[i].Cells[2].Value == null)
                            {
                                query = query + "" + "')";
                            }
                            else
                            {
                                query = query + dataGridView1.Rows[i].Cells[2].Value.ToString() + "')";
                            }

                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    con.Close();
                    MessageBox.Show(txtName.Text + " Rights are now changed!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Disable();
                    FormIDs();
                }
            }
            else { MessageBox.Show("Sorry! You do not have permission to perform this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
              

            
        }

        private void Disable()
        {
            comboUserID.Enabled = false;
            txtName.Enabled = false;
            dataGridView1.Enabled = false;
            comboUserID.Items.Clear();
            txtName.Clear();
            txtUserSearch.Clear();
        }

        private void Enable()
        {
            comboUserID.Enabled = true;
            txtName.Enabled = true;
            dataGridView1.Enabled = true;
        }

        private void txtUserSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM login WHERE USERNAME LIKE '%" + txtUserSearch.Text + "%' ORDER BY ID ASC", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                dataGridView2.Rows.Clear();

                foreach (DataRow item in dt.Rows)
                {
                    int i = dataGridView2.Rows.Add();

                    dataGridView2.Rows[i].Cells[0].Value = item["ID"].ToString();
                    dataGridView2.Rows[i].Cells[1].Value = item["USERNAME"].ToString();
                }
                dataGridView2.PerformLayout();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else
            {
                e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
            }
        }

        private void txtUserSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else
            {
                e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
            }
        }

        private void Close(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            } 
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                comboUserID.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                txtName.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            }   catch { }
        }

        private void User_Access_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.U)
            {
                btnUpdateAcess.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.C)
            {
                btnClose.PerformClick();
            } 
        }
    }
}
