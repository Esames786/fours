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
using GrayLark;
using GrayLark.bin.Debug.Report;

namespace GrayLark
{
    public partial class View_Product_Stock : Form
    {
        public View_Product_Stock()
        {
            InitializeComponent(); comboSearch.SelectedIndex = 0;
        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

        private void showall()
        {
            try
            {
                this.txtSearch.Focus();


                decimal total = 0; decimal pp = 0;
                SqlDataAdapter sda = new SqlDataAdapter("select * from add_product_stock where Status='Active'", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                    for (int n = 0; n < dataGridView1.Rows.Count; n++)
                    {
                        decimal val = Convert.ToDecimal(dataGridView1.Rows[n].Cells[6].Value.ToString());

                        if (val < 2)
                        {
                            dataGridView1.Rows[n].Cells[6].Style.BackColor = Color.Yellow;
                        }
                        else if (val == 2)
                        {
                            dataGridView1.Rows[n].Cells[6].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            dataGridView1.Rows[n].Cells[6].Style.BackColor = Color.White;
                        }

                        decimal val1 = Convert.ToDecimal(dataGridView1.Rows[n].Cells[6].Value.ToString());
                        if (val1 > 0)
                        {
                            total += Convert.ToDecimal(dataGridView1.Rows[n].Cells[6].Value.ToString());
                        }
                        txtQuantity.Text = total.ToString();

                    }
                }
                else
                {
                    dataGridView1.Rows.Clear();
                    txtQuantity.Text = "0";
                }
                dataGridView1.PerformLayout();
            }
            catch { }
        }
        private void View_Product_Stock_Load(object sender, EventArgs e)
        {
            showall();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String qs = "";
                if (comboSearch.SelectedIndex == 0)
                {
                    qs = "SELECT * FROM add_product_stock WHERE NAME = '" + txtSearch.Text + "' and Status='Active'";
                }
                else if (comboSearch.Text == "Categories")
                {
                    qs = "SELECT * FROM add_product_stock WHERE CATEGORY = '" + txtSearch.Text + "' and Status='Active'";
                }
                else if (comboSearch.Text == "Lord Number")
                {
                    qs = "SELECT * FROM add_product_stock WHERE LordNumber = '" + txtSearch.Text + "' and Status='Active'";
                }
                else if (comboSearch.Text == "Warehouse")
                {
                    qs = "SELECT * FROM add_product_stock WHERE WarehouseName = '" + txtSearch.Text + "' and Status='Active'";
                }

                else if (comboSearch.Text == "Referencecode")
                {
                    qs = "SELECT * FROM add_product_stock WHERE Referencecode = '" + txtSearch.Text + "' and Status='Active'";
                }

                else if (comboSearch.Text == "Size")
                {
                    qs = "SELECT * FROM add_product_stock WHERE Size = '" + txtSearch.Text + "' and Status='Active'";
                }
                decimal total = 0; decimal pp = 0;

                DataTable dt = clsDataLayer.RetreiveQuery(qs);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;

                    for (int n = 0; n < dataGridView1.Rows.Count; n++)
                    {
                        decimal val = Convert.ToDecimal(dataGridView1.Rows[n].Cells[6].Value.ToString());

                        if (val < 2)
                        {
                            dataGridView1.Rows[n].Cells[6].Style.BackColor = Color.Yellow;
                        }
                        else if (val == 2)
                        {
                            dataGridView1.Rows[n].Cells[6].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            dataGridView1.Rows[n].Cells[6].Style.BackColor = Color.White;
                        }

                        decimal val1 = Convert.ToDecimal(dataGridView1.Rows[n].Cells[6].Value.ToString());
                        if (val1 > 0)
                        {
                            total += Convert.ToDecimal(dataGridView1.Rows[n].Cells[6].Value.ToString());
                        }
                        txtQuantity.Text = total.ToString();
                    }
                    dataGridView1.PerformLayout();
                }
                else { showall(); }
            }
            catch { }
        }

        private void comboSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboSearch.Text == "Product Name")
                {
                    txtSearch.Clear();
                    String query = "select Name from add_product_stock  where Status='Active'";
                    DataTable dts = clsDataLayer.RetreiveQuery(query);
                    if (dts.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtSearch, dts);
                    }
                }
                else if (comboSearch.Text == "Categories")
                {
                    txtSearch.Clear();
                    String query = "select distinct CATEGORY from add_product_stock  where Status='Active'";
                    DataTable dts = clsDataLayer.RetreiveQuery(query);
                    if (dts.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtSearch, dts);
                    }
                }
                else if (comboSearch.Text == "Lord Number")
                {
                    txtSearch.Clear();
                    String query = "select distinct LordNumber from add_product_stock  where Status='Active'";
                    DataTable dts = clsDataLayer.RetreiveQuery(query);
                    if (dts.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtSearch, dts);
                    }
                }
                else if (comboSearch.Text == "Warehouse")
                {
                    txtSearch.Clear();
                    String query = "select distinct WarehouseName from add_product_stock  where Status='Active'";
                    DataTable dts = clsDataLayer.RetreiveQuery(query);
                    if (dts.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(txtSearch, dts);
                    }
                }
            }
            catch { }
        }
        private void View_Product_Stock_KeyPress(object sender, KeyPressEventArgs e)
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
                string q = "";
                if (txtSearch.Text.Equals(""))
                {
                    q = "select * from add_product_stock where Status='Active'";
                }
                else if (comboSearch.Text == "Product Name")
                {
                    q = "select * from add_product_stock where Name='" + txtSearch.Text + "'  and Status='Active'";
                }
                else if (comboSearch.Text == "Categories")
                {
                    q = "select * from add_product_stock where CATEGORY='" + txtSearch.Text + "'  and Status='Active'";
                }
                else if (comboSearch.Text == "Lord Number")
                {
                    q = "select * from add_product_stock where CATEGORY='" + txtSearch.Text + "'  and Status='Active'";
                }
                else if (comboSearch.Text == "Warehouse")
                {
                    q = "select * from add_product_stock where WarehouseName='" + txtSearch.Text + "'  and Status='Active'";
                }
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    StockReport rpt = new StockReport();
                    rpt.SetDataSource(purchase);
                    PaymentView pop = new PaymentView(rpt);
                    pop.Show();
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
