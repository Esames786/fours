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
    public partial class RptUpdateFurnished : Form
    {
        String UID = Login.UserID;
        public RptUpdateFurnished()
        {
        InitializeComponent();
        textBox1.Enabled = false; String h2 = "select distinct Product_Name from tbl_UpdFurnish"; DataTable d5 = clsDataLayer.RetreiveQuery(h2); if (d5.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(textBox1, d5); }
        this.KeyPreview = true;
        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        SqlCommand cmd;

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            print();
        }

        public void print()
        {
            try
            { 
            if (comboBox2.SelectedIndex == 0)
            {
                string q = "select * from tbl_UpdFurnish where Dates between '" + date1.Text + "' and '" + date2.Text + "' ";
                DataTable report = clsDataLayer.RetreiveQuery(q);
                if (report.Rows.Count > 0)
                {
                    rptupdatefurnishdate rpt = new rptupdatefurnishdate();
                    rpt.SetDataSource(report);
                    UpdFurnView rp = new UpdFurnView(date1.Text, date2.Text, rpt);
                    rp.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found !","Error",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            } 
            else
            {
                string q1 = "select * from tbl_UpdFurnish where Dates between '" + date1.Text + "' and '" + date2.Text + "' and Product_Name='"+textBox1.Text+"'";
                DataTable report1 = clsDataLayer.RetreiveQuery(q1);
          if (report1.Rows.Count > 0)
          {
              rptupdatefurnishdate rpt1 = new rptupdatefurnishdate();
              rpt1.SetDataSource(report1);
              UpdFurnView rp = new UpdFurnView(date1.Text, date2.Text, rpt1);
              rp.Show();
          }
          else
          {
              MessageBox.Show("No Record Found !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
            }        
            }
            catch{ }
        }

        private void Return_Sale_Load(object sender, EventArgs e)
        {

        }

        private void Return_Sale_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
         
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                print();
            }
        }

        private void date1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                print();
            }
        }

        private void date2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                print();
            }
        } 

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        if (comboBox2.SelectedIndex == 0) 
        {
            date1.Enabled = true; date2.Enabled = true; textBox1.Enabled = false;
        } 
        else    
        {
            date1.Enabled = true; date2.Enabled = true; textBox1.Enabled = true;
        }
        }

        private void RptUpdateFurnished_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

         } 
    }
