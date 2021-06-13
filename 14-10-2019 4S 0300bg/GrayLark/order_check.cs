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
using Skyview.bin.Debug.Reports;

namespace Skyview
{
    public partial class order_check : Form
    
    {
        string currentdate = DateTime.Now.ToString("yyyy-MM-dd");
        public order_check()
        {
            InitializeComponent();
            
            
        }
        

        

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

        public void total()
        {
            textBox1.Text = "";
            decimal add = 0;
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    add  += Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value.ToString());
                }
                textBox1.Text = add.ToString();
            
            }
        }
        private void order_check_Load(object sender, EventArgs e)
        {
            try
            {
                refresh();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public void refresh()
        {
            textBox1.Text = "";

            dateTimePicker1.Text = currentdate;
            dateTimePicker2.Text = currentdate;

            String h = "SELECT TokenCode,SaleCode,Product_Name,Quantity,Sell_Price,Total_Amount,FloorNo,TableNo,Waiter,TableStatus,UserName,Date FROM tbl_SaleByTable Where Date between '" + currentdate + "' and '" + currentdate + "' ORDER BY S_Id asc";
            DataTable dt = clsDataLayer.RetreiveQuery(h);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                total();
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }

        public void refresh2()
        {
            textBox1.Text = "";
            
            SqlDataAdapter sda = new SqlDataAdapter("SELECT TokenCode,SaleCode,Product_Name,Quantity,Sell_Price,Total_Amount,FloorNo,TableNo,Waiter,TableStatus,UserName,Date FROM tbl_SaleByTable Where Date between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' ORDER BY S_Id asc", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                total();
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                refresh();

            
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
                refresh2();
           
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

            
            refresh2();
            
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            textBox1.Text = "";

            SqlDataAdapter sda = new SqlDataAdapter("SELECT TokenCode,SaleCode,Product_Name,Quantity,Sell_Price,Total_Amount,FloorNo,TableNo,Waiter,TableStatus,UserName,Date FROM tbl_SaleByTable Where Date between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' ORDER BY S_Id asc", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {

                Sale_Inventory pi = new Sale_Inventory();
                pi.SetDataSource(dt);
                order_check_viewer pv = new order_check_viewer(pi);
                pv.Show();
            }
            else
            {
                MessageBox.Show("No data Found");
            }


        }

        

        

    }
}
