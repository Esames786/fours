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
    public partial class demand_details : Form
    {
        SqlConnection connectionString = null;
        String abc = "";
        PurchaseInquiry pi;
        public demand_details(PurchaseInquiry pi1)
        {
            InitializeComponent();
            connectionString = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
            pi = pi1; 

        }

        private void Showall()
        {
            try
            {
                String get1 = "select PICode,Product_Name,Ref_no,Sell_Price,Qty,Size,category from tbl_demand"; DataTable d1 = clsDataLayer.RetreiveQuery(get1); if (d1.Rows.Count > 0)
                {
                    dataGridView2.DataSource = d1;
                }
            }
            catch
            {
            }
        }

        private void Purchase_Detail_Load(object sender, EventArgs e)
        {
            try
            {

                clsGeneral.SetAutoCompleteTextBox(txtInvoiceID, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 14 "));
            }
            catch
            {
            }
        }

        private void SearchID()
        {
            
        }

        private void SearchDate()
        {
            try
            {
                String get1 = "Select PICode,Product_Name,Ref_no,Sell_Price,Qty,Size,vendor,category from tbl_demand where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and vendor = '" + txtInvoiceID.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(get1); if (d1.Rows.Count > 0)
                {
                    dataGridView2.DataSource = d1;
                }
                else
                {
                    dataGridView2.DataSource = null;
                }
            }
            catch
            {
            }
        }

        private void btn_view_Click(object sender, EventArgs e)
        {
            
                SearchDate();
            
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                pi.inqnew();
                pi.dataGridView1.Rows.Clear();
                for (int a = 0; a < dataGridView2.Rows.Count; a++)
                {
                    pi.addrow();
                    pi.txtname.Text = dataGridView2.Rows[a].Cells[6].Value.ToString();
                    pi.dataGridView1.Rows[a].Cells[2].Value = dataGridView2.Rows[a].Cells[1].Value.ToString();
                    pi.dataGridView1.Rows[a].Cells[3].Value = dataGridView2.Rows[a].Cells[5].Value.ToString();
                    pi.dataGridView1.Rows[a].Cells[4].Value = dataGridView2.Rows[a].Cells[2].Value.ToString();
                    pi.dataGridView1.Rows[a].Cells[5].Value = dataGridView2.Rows[a].Cells[4].Value.ToString();
                    pi.dataGridView1.Rows[a].Cells[1].Value = dataGridView2.Rows[a].Cells[7].Value.ToString();
                    

                }
                this.Hide();
            }
            catch 
            {
                
                
            }
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtInvoiceID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SearchDate();
            }
            catch 
            {
                
                
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchDate();
            }
            catch
            {


            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchDate();
            }
            catch
            {


            }
        }




    }
}

