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
using Microsoft.SqlServer;
using System.Diagnostics;
using System.IO;

namespace GrayLark
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

            #region onload
            
            //string root = @"C:\Temp";
            //// If directory does not exist, don't even try   
            //if (Directory.Exists(root))
            //{
            //    Directory.Delete(root);
            //}
            btnExit.FlatAppearance.BorderColor = Color.White; btnLogin.FlatAppearance.BorderColor = Color.White;
            String set = "select * from restric";
            DataTable de = clsDataLayer.RetreiveQuery(set);
            if (de.Rows.Count > 0)
            {
                txtUser.Enabled = false;
                txtPass.Enabled = false;
                Console.WriteLine("Restriction ");
                MessageBox.Show("Your Software Services Has Been Expired Please Contact Codev Solution : 923145414554");
            }
            else
            {
                //   int ed = 10;
                int em = 05;
                int ey = 2021;

                //  int wd = 10;
                int wm = 04;
                int wy = 2021;

                int cd = Convert.ToInt32(DateTime.Now.ToString("dd"));
                int cm = Convert.ToInt32(DateTime.Now.ToString("MM"));
                int cy = Convert.ToInt32(DateTime.Now.ToString("yyyy"));

                String currentdate = DateTime.Now.ToString("dd-MM-yyyy");
                if ((cm >= em) && (cy >= ey))
                {
                    MessageBox.Show("Your Software Services Has Been Expired Please Contact Codev Solution : 0332 8252838 0333 3681466");
                    txtUser.Enabled = false;
                    txtPass.Enabled = false;
                    String ins = "insert into restric(Expire) values ('1')";
                    clsDataLayer.ExecuteQuery(ins);

                }
                else if ((cm >= wm) && (cy >= wy))
                {
                    MessageBox.Show("Your Software will expire in One Month! Contact Codev Solution :0332 8252838 0333 3681466");
                    Console.WriteLine("One Month ");
                }
            }
            #endregion onload 
        }
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        SqlDataAdapter sda;
        DataTable dt;
        public static string UserID = "";

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtPass.Enabled == false)
            {
                MessageBox.Show("Your Software will expire in One Month! Contact Codev Solution","Error",MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }
            else
            {
                login();
            }
          
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {                
                login();
            } 
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }

        private void login()
        {
            if (txtUser.Text == "" && txtPass.Text == "")
            {
                MessageBox.Show("Must Enter Username and Password!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                { 
                    string query = "select * from login where USERNAME = '" + txtUser.Text + "' AND PASSWORD = '" + txtPass.Text + "'";

                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    int count = ds.Tables[0].Rows.Count;
                    if (count > 0)
                    {
                        UserID = txtUser.Text;
                        
                        this.Hide();
                        Master_Form mf = new Master_Form();
                        mf.Show();

                    }
                         
                    else
                    {
                        MessageBox.Show("Mr./Ms. " + txtUser.Text + " Please Enter valid Password!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtPass.Clear();
                    }
                    con.Close();
                }
                catch  { }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit(); Environment.Exit(0); Process.GetCurrentProcess().Kill();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            User();
        }

        private void User()
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM login WHERE STATUS = '" + "Active" +"' ORDER BY USERNAME ASC", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txtUser.Items.Add(dt.Rows[i]["USERNAME"]);
                }
            }
            catch  {  }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); Environment.Exit(0); Process.GetCurrentProcess().Kill(); 
        }

         

      
    }
}
