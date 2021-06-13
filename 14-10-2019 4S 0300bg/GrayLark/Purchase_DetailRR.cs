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
    public partial class Purchase_DetailRR : Form
    {
        SqlConnection connectionString = null;
        String abc = "";
        PurchaseOrders pr = new PurchaseOrders();
        public Purchase_DetailRR(String ForName)
        {
            InitializeComponent();
            connectionString = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
          abc = ForName;
          //  pr = pp;
            DataTable de = clsDataLayer.RetreiveQuery("select PurCode from Purchase"); clsGeneral.SetAutoCompleteTextBox(txtInvoiceID,de);
            this.BackColor = Color.SteelBlue;
            String get = "select VendorName from Purchase"; DataTable dts = clsDataLayer.RetreiveQuery(get); if (dts.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txtparty, dts); }
        }



        private void Purchase_Detail_Load(object sender, EventArgs e)
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(@"select PurCode,VendorName,BillPrice,Date from Purchase", connectionString);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                dataGridView2.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtInvoiceID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(@"select PurCode,VendorName,BillPrice,Date from Purchase where PurCode = '"+txtInvoiceID.Text+"'", connectionString);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                dataGridView2.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(abc.Equals("Return"))
            {
                txtInvoiceID.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                Return_Order po = new Return_Order();
                po.txtsearching.Text = "Vender";
                po.txtvendor.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
                po.txtSearchSID.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                po.Show();
            }
            else if (abc.Equals("Repalce"))
            {
                txtInvoiceID.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                ReplaceOrder po = new ReplaceOrder();
                
                po.txtSearchSID.Text = txtInvoiceID.Text;
                po.comboBox1.Text = "Vender";
                po.textBox1.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
                po.Show();
            }
            else
            {
             //   txtInvoiceID.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
             // //  PurchaseOrders po = new PurchaseOrders();
             //   //po.passIID = txtInvoiceID.Text;
             //   pr.txtcode.Text = txtInvoiceID.Text;
             //   this.Close();
             ////   po.Show();
            }
           // this.Close();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            PurchaseOrders po = new PurchaseOrders();
            po.Show();

        }
        private void SearchID()
        {
            try
            {            
                SqlDataAdapter sda = new SqlDataAdapter(@"SELECT Purchase.PurCode, PurchaseDetail.PurchaseItem, Purchase.VendorName, PurchaseDetail.size, PurchaseDetail.Quantity, PurchaseDetail.PricePerItem, PurchaseDetail.TotalPriceItem, 
                  Purchase.Date FROM Purchase INNER JOIN PurchaseDetail ON Purchase.PurCode = PurchaseDetail.PurCode WHERE Purchase.PurCode LIKE '%" + txtInvoiceID.Text + "%'", connectionString);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                dataGridView2.Rows.Clear();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dataGridView2.Rows.Add();
                    dataGridView2.Rows[n].Cells[0].Value = item["PurCode"].ToString();
                    dataGridView2.Rows[n].Cells[1].Value = item["PurchaseItem"].ToString();
                    dataGridView2.Rows[n].Cells[2].Value = item["VendorName"].ToString();
                    dataGridView2.Rows[n].Cells[3].Value = item["size"].ToString();
                    dataGridView2.Rows[n].Cells[4].Value = item["Quantity"].ToString();
                    dataGridView2.Rows[n].Cells[5].Value = item["PricePerItem"].ToString();
                    dataGridView2.Rows[n].Cells[6].Value = item["TotalPriceItem"].ToString();
                    dataGridView2.Rows[n].Cells[7].Value = item["Date"].ToString();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SearchDate()
        {
            try
            {  
                SqlDataAdapter sda = new SqlDataAdapter(@"SELECT Purchase.PurCode, PurchaseDetail.PurchaseItem, Purchase.VendorName, PurchaseDetail.size, PurchaseDetail.Quantity, PurchaseDetail.PricePerItem, PurchaseDetail.TotalPriceItem, 
                  Purchase.Date FROM Purchase INNER JOIN PurchaseDetail ON Purchase.PurCode = PurchaseDetail.PurCode WHERE  Date BETWEEN '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'", connectionString);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridView2.DataSource = dt;
                    //dataGridView2.Rows.Clear();
                    //foreach (DataRow item in dt.Rows)
                    //{
                    //    int n = dataGridView2.Rows.Add();
                    //    dataGridView2.Rows[n].Cells[0].Value = item["PurCode"].ToString();
                    //    dataGridView2.Rows[n].Cells[1].Value = item["PurchaseItem"].ToString();
                    //    dataGridView2.Rows[n].Cells[2].Value = item["VendorName"].ToString();
                    //    dataGridView2.Rows[n].Cells[3].Value = item["size"].ToString();
                    //    dataGridView2.Rows[n].Cells[4].Value = item["Quantity"].ToString();
                    //    dataGridView2.Rows[n].Cells[5].Value = item["PricePerItem"].ToString();
                    //    dataGridView2.Rows[n].Cells[6].Value = item["TotalPriceItem"].ToString();
                    //    dataGridView2.Rows[n].Cells[7].Value = item["Date"].ToString();

                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_view_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                SearchID();
            }
            else if (radioButton2.Checked)
            {
                SearchDate();
            }
        }

        private void txtparty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(@"select PurCode,VendorName,BillPrice,Date from Purchase where VendorName = '" + txtparty.Text + "'", connectionString);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridView2.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        
    }
}


/*
 
 * 
 *  
 * 

 */