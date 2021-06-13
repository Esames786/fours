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
    public partial class AddBankDetail : Form
    {  public string GreenSignal = "";
        public string FormID = "";
        String Cus = "";
        String UID = Login.UserID;
        public AddBankDetail()
        {
            InitializeComponent();
            BankDetailShow(); clear.Enabled = true; submit.Enabled = false; this.KeyPreview = true;
        }

        private void clear_Click(object sender, EventArgs e)
        {  FormID = "Add";
            UserNametesting();
            if (GreenSignal == "YES")
            {
            txtbname.Enabled = true;
            txtbac.Enabled = true;
            txtbalance.Enabled = true;
            txtbname.Text = "";
            txtbac.Text = "";
            txtbalance.Text = ""; clear.Enabled = false; submit.Enabled = true;
            txtid.Text = clsGeneral.getMAXCode("BankDetail", "BankId", "B"); txtbname.Focus();
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
                    {
                        if (((ComboBox)c).Text == "")
                        {
                            flag = true;
                            break;
                        }
                    }
                }
            }
            return flag;
        }

        private void BankDetailShow()
        {
            String abc = "select BankName,AccountNo,RemainingBalance,AccountId,Dates,UserName from AccountBalance";
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

        private void submit_Click(object sender, EventArgs e)
        {FormID = "Save";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                if (!CheckAllFields())
                {
                    txtid.Text = clsGeneral.getMAXCode("BankDetail", "BankId", "B");
                    String sel = "select * from BankDetail where AccountNo='" + txtbac.Text + "'";
                    DataTable dt = clsDataLayer.RetreiveQuery(sel);
                    if (dt.Rows.Count < 1)
                    {
                        String insert = "insert into BankDetail (BankId,Dates,BankName,AccountNo,OpeningBalance,UserName) values ('" + txtid.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + txtbname.Text + "','" + txtbac.Text + "','" + txtbalance.Text + "','" + Login.UserID + "')";
                        if (clsDataLayer.ExecuteQuery(insert) > 0)
                        { 
                            String ins = "insert into AccountBalance(AccountId,Dates,BankName,AccountNo,RemainingBalance,UserName)values ('" + txtid.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + txtbname.Text + "','" + txtbac.Text + "','" + txtbalance.Text + "','" + Login.UserID + "')";
                            if (clsDataLayer.ExecuteQuery(ins) > 0)
                            {
                                String inst = "insert into BankTransaction(BankCode,PartyName,BankName,AccountNo,ChequeNo,Dates,BankCash,CurrentCash,Remarks,Status,UserName)values('" + txtid.Text + "','Opening Balance','" + txtbname.Text + "','" + txtbac.Text + "','-','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + txtbalance.Text + "','" + txtbalance.Text + "','Opening Balance','Received','" + UID + "')";
                              clsDataLayer.ExecuteQuery(inst);

                                MessageBox.Show("Inserted!","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                                txtbname.Enabled = false;
                                txtbac.Enabled = false;
                                txtbalance.Enabled = false;
                                BankDetailShow(); clear.Enabled = true; submit.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Already Inserted!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
   
    
    
    
    
    
    
    
    }
}
