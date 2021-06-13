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
    public partial class deal : Form
    {
        TextBox tb = new TextBox();
        public deal()
        {
            InitializeComponent();

            refresh();


            String h3 = "select PICode from tbl_demand";
            DataTable d3 = clsDataLayer.RetreiveQuery(h3);
            if (d3.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txt_search, d3);
            }
            //String h4 = "select Referencecode from add_product";
            //DataTable d4 = clsDataLayer.RetreiveQuery(h4);
            //if (d4.Rows.Count > 0)
            //{
            //    clsGeneral.SetAutoCompleteTextBox(txtcat, d4);
            //}




            clsGeneral.SetAutoCompleteTextBox(txtprinc, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 14 "));
            btnEdit.Enabled = false;
            btnUpdate.Enabled = false;
            btnsave.Enabled = false;
            grid.Enabled = false;
            txtcode.Enabled = false;

            textBox1.Enabled = false;
            txtcat.Enabled = false;
            txtpname.Enabled = false;
            txtprice.Enabled = false;
            txtqty.Enabled = false;
            txtprinc.Enabled = false;
            txtcateogry.Enabled = false;
        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

        public void refresh()
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT distinct(PICode) FROM tbl_demand", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = dt;
            }
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_add_Click(object sender, EventArgs e)
        {

        }

        private void New_Click(object sender, EventArgs e)
        {
            grid.Enabled = true; btnsave.Enabled = true; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = false;
            grid.Rows.Clear();

            grid.Enabled = true;

            txtpname.Enabled = true;
            txtprinc.Enabled = true;
            txtprice.Enabled = true;
            txtcat.Enabled = true;
            txtqty.Enabled = true;
            txtcateogry.Enabled = true;
            txtcode.Text = clsGeneral.getMAXCode("tbl_demand", "PICode", "DD");
            txtprinc.Focus();
        }

        private bool CheckDataGridCells(DataGridView dgv)
        {
            bool flag = false;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (dgv.Rows[i].Cells[j].Value == null)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == true)
                {
                    break;
                }
            }
            return flag;
        }

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            {
                try
                {
                    if (grid.CurrentCell.ColumnIndex == 2)
                    {
                        if (e.KeyCode == Keys.Enter)
                        {
                            // same = 1;
                            grid.Rows.Add();
                            int yhs = grid.CurrentCell.RowIndex;
                            grid.CurrentCell = grid.Rows[yhs].Cells[0];
                            grid.BeginEdit(true);
                        }

                    }
                    else
                    {
                        if (e.KeyCode == Keys.Delete)
                        {
                            if (grid.Rows.Count > 0)
                            {
                                int yh = grid.CurrentCell.RowIndex;
                                grid.Rows.RemoveAt(yh);
                            }
                        }
                    }
                }
                catch { }
            }
        }

        public void Save()
        {

            String sel = "select * from tbl_demand where PICode ='" + txtcode.Text + "'";
            DataTable gb = clsDataLayer.RetreiveQuery(sel);
            if (gb.Rows.Count > 0)
            {
                MessageBox.Show("Already Add", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                for (int a = 0; a < grid.Rows.Count; a++)
                {


                    String insert = "insert into tbl_demand (PICode,Product_Name,Ref_no,Sell_Price,Qty,Size,UserName,Date,vendor,category)values('" + txtcode.Text + "','" + grid.Rows[a].Cells[1].Value.ToString() + "','" + grid.Rows[a].Cells[0].Value.ToString() + "','" + Convert.ToDecimal(grid.Rows[a].Cells[3].Value.ToString()) + "','" + Convert.ToDecimal(grid.Rows[a].Cells[4].Value.ToString()) + "','" + grid.Rows[a].Cells[2].Value.ToString() + "','" + Login.UserID + "','" + dateTimePicker1.Text + "','" + txtprinc.Text + "','" + grid.Rows[a].Cells[5].Value.ToString() + "')";
                    clsDataLayer.ExecuteQuery(insert);
                    refresh();
                    btnsave.Enabled = false;
                    New.Enabled = true;


                }
                grid.Rows.Clear();
                textBox1.Clear();
                txtprinc.Clear();
                txtcateogry.Clear();

            }

        }


        private void btnsave_Click(object sender, EventArgs e)
        {

            try
            {
                if (!CheckDataGridCells(grid))
                {

                    Save(); MessageBox.Show("Product Save Successfully!");
                    grid.Enabled = false; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = true;
                    dataGridView2.Refresh();
                }
                else
                {
                    MessageBox.Show("Fill All Fields!");
                }

            }
            catch { }

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btnsave.Enabled = false;
            btnEdit.Enabled = true;
            btnUpdate.Enabled = false;
            New.Enabled = false;
            txt_search.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            SqlDataAdapter sda2 = new SqlDataAdapter("SELECT distinct(PICode) FROM tbl_demand Where PICode ='" + txt_search.Text + "'", con);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            if (dt2.Rows.Count > 0)
            {
                txtcode.Text = dt2.Rows[0][0].ToString();

            }

            SqlDataAdapter sda = new SqlDataAdapter("SELECT PICode,Product_Name,Ref_no,Sell_Price,Qty,Size,UserName,Date,vendor,category FROM tbl_demand Where PICode ='" + txt_search.Text + "'", con);
            DataTable dt1 = new DataTable();
            sda.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                string ck = "";
                grid.Rows.Clear();
                foreach (DataRow row in dt1.Rows)
                {
                    int n = grid.Rows.Add();
                    ck = row[1].ToString();
                    grid.Rows[n].Cells[0].Value = row[2].ToString();
                    grid.Rows[n].Cells[1].Value = row[1].ToString();
                    grid.Rows[n].Cells[2].Value = row[5].ToString();
                    grid.Rows[n].Cells[3].Value = row[3].ToString();
                    grid.Rows[n].Cells[4].Value = row[4].ToString();
                    dateTimePicker1.Text = row[7].ToString();
                    txtprinc.Text = row[8].ToString();
                    grid.Rows[n].Cells[5].Value = row[9].ToString();

                }
                refresh();

            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            grid.Enabled = true; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = true; New.Enabled = false;
            txtpname.Enabled = true;
            txtprice.Enabled = true;
            txtcat.Enabled = true;
            txtqty.Enabled = true;
            txtprinc.Enabled = true;
            txtcateogry.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckDataGridCells(grid))
            {
                if (txtcode.Text != "")
                {
                    //


                    String del = "delete from tbl_demand where PICode='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(del); Save(); MessageBox.Show("Product Update Successfully!");
                    grid.Enabled = false; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = true;

                }
                else
                {
                    MessageBox.Show("Fill All Fields!");
                }
                //
            }
            else
            {
                MessageBox.Show("Fill All Fields!");
            }
        }


        private void grid_KeyPress(object sender, KeyPressEventArgs e)
        {

            try
            {
                if (grid.CurrentCell.ColumnIndex == 4)
                {
                    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    { e.Handled = true; }

                }
            }
            catch 
            {
                
                
            }
            
        }

        public void total_c()
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            refresh();

            btnEdit.Enabled = false;
            btnUpdate.Enabled = false;
            btnsave.Enabled = false;
            grid.Enabled = false;
            txtcode.Enabled = false;
            txtprinc.Enabled = false;

            New.Enabled = true;

            grid.Rows.Clear();
            textBox1.Clear();
            txtqty.Clear();
            txtpname.Clear();
            txtprice.Clear();
            txtcat.Clear();
            txtcat.Enabled = false;
            txtpname.Enabled = false;
            txtprice.Enabled = false;
            txtqty.Enabled = false;

        }

        private void grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
                textBox1.Text = CellSum().ToString();
            if (grid.Rows.Count  > 0)
            {
                txtprinc.Enabled = false;
            }
            else
            {
                txtprinc.Enabled = true;
            }
        }
        private double CellSum()
        {
            double sum = 0;
            try
            {
                sum = 0;
                for (int i = 0; i < grid.Rows.Count; ++i)
                {
                    double d = 0;
                    Double.TryParse(grid.Rows[i].Cells[4].Value.ToString(), out d);
                    sum += d;
                }

            }
            catch
            {
            }
            return sum;
        }

        private void txtpname_TextChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch
            {


            }

        }

        private void txtcat_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String h4 = "select NAME,SELL_PRICE,Size from add_product where Referencecode = '" + txtcat.Text + "' and CATEGORY = '" + txtcateogry.Text + "' and [principleName] =  '" + txtprinc.Text + "'";
                DataTable d4 = clsDataLayer.RetreiveQuery(h4);
                if (d4.Rows.Count > 0)
                {
                    txtpname.Text = d4.Rows[0][0].ToString();
                    txtprice.Text = d4.Rows[0][1].ToString();
                    txtsize.Text = d4.Rows[0][2].ToString();
                }
                else
                {
                    txtpname.Text = "";
                    txtprice.Text = "";
                    txtsize.Text = "";
                }
            }
            catch
            {


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtpname.Text.Length > 0)
                {
                    if (txtprice.Text.Length > 0)
                    {

                        int check = 0;

                        if (grid.Rows.Count > 0)
                        {
                            string p_namee = txtcat.Text;

                            for (int i = 0; i < grid.Rows.Count; i++)
                            {
                                string p2 = grid.Rows[i].Cells[0].Value.ToString();
                                if (p2 == p_namee)
                                {
                                    check = 1;

                                }

                            }
                        }

                        if (check == 0)
                        {

                            int n = grid.Rows.Add();
                            grid.Rows[n].Cells[0].Value = txtcat.Text;
                            grid.Rows[n].Cells[1].Value = txtpname.Text;
                            grid.Rows[n].Cells[3].Value = txtprice.Text;
                            grid.Rows[n].Cells[2].Value = txtsize.Text;
                            grid.Rows[n].Cells[4].Value = txtqty.Text;
                            grid.Rows[n].Cells[5].Value = txtcateogry.Text;
                            txtpname.Text = "";
                            txtprice.Text = "";
                            txtsize.Text = "";
                            txtqty.Text = "";
                            txtcat.Text = "";
                            txtcat.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Already Exist in list with same reference code");
                            txtpname.Text = "";
                            txtprice.Text = "";
                            txtsize.Text = "";
                            txtqty.Text = "";
                            txtcat.Text = "";
                            txtcat.Focus();
                        }

                    }
                }


            }
            catch
            {


            }


        }

        private void deal_Load(object sender, EventArgs e)
        {

        }

        private void txtcateogry_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtcat.Clear();
                DataTable d42 = new DataTable();
                String h41 = "select Referencecode from [add_product] where [CATEGORY] = '" + txtcateogry.Text + "' and [principleName] = '" + txtprinc.Text + "'";
                DataTable d41 = clsDataLayer.RetreiveQuery(h41);
                if (d41.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtcat, d41);
                }
                else
                {
                    clsGeneral.SetAutoCompleteTextBox(txtcat, d42);
                }
            }
            catch
            {


            }

        }

        private void txtprinc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable d42 = new DataTable();
                txtcateogry.Clear();
                String h41 = "select [CATEGORY] from add_product where [principleName] = '" + txtprinc.Text + "'";
                DataTable d41 = clsDataLayer.RetreiveQuery(h41);
                if (d41.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtcateogry, d41);
                }
                else
                {
                    clsGeneral.SetAutoCompleteTextBox(txtcateogry, d42);
                }
            }
            catch
            {


            }
        }

        private void txtprice_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtcateogry_Leave(object sender, EventArgs e)
        {
            txtcat.Focus();
        }

        private void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }


    }
}
