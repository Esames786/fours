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
using GrayLark.bin.Debug.Report;


namespace GrayLark
{
    public partial class View_All_Sale_Post : Form
    {
        String FormName = "";
        public View_All_Sale_Post(String abc)
        {
            InitializeComponent();
            this.BackColor = Color.SteelBlue;
            FormName = abc;
        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

        private void View_All_Sale_Load(object sender, EventArgs e)
        {
            String query = "select ProductName from tbl_SaleDetail where PostStatus = 'POST'";
            DataTable da= clsDataLayer.RetreiveQuery(query);
            if(da.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txtSID,da); }
                comboSearch.SelectedIndex = 0;
            this.txtSID.Focus();
            try 
            {
                String query3 = "";
                if (FormName.Equals("Return"))
                {
                    query3 = "select SaleCode,Dates,CustomerName,BookerName,Remarks,BillAmount from tbl_sale where PostStatus = 'POST' and ReturnAmount =0"; 
                }
                else
                {
                    query3 = "select SaleCode,Dates,CustomerName,BookerName,Remarks,BillAmount from tbl_sale where PostStatus = 'POST' and ReplaceAmount =0";
                }

                DataTable dt = clsDataLayer.RetreiveQuery(query3);
                if(dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                } 
            }
            catch {   }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtSID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                if (FormName.Equals("Return"))
                {
                    Return_Order ps = new Return_Order(); 
                    ps.txtsearching.Text = "Customer";

                    ps.enable();
                    ps.txtCName.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    ps.txtSearchSID.Text = txtSID.Text;

                    ps.btnNew.Enabled = false;
                    ps.btnsve.Enabled = true;
                    ps.Show();
                }
                else if (FormName.Equals("Replace"))
                {
                    txtSID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    ReplaceOrder ps = new ReplaceOrder();
                    ps.txtSearchSID.Text = txtSID.Text;
                    ps.txtCName.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    ps.enable();
                    ps.txtSearchSID.Text = txtSID.Text;
                    ps.comboBox1.Text = "Customer";
                    ps.textBox1.Enabled = false;
                    ps.btnNew.Enabled = false;
                    ps.btnSave.Enabled = true;
                    ps.Show();


                }
                this.Close();
            } catch { }
        }

        private void View_All_Sale_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void txtSID_Leave(object sender, EventArgs e)
        {
          
  if (comboSearch.Text == "Product Name")
            {
                try
                {

                    SqlDataAdapter sda = new SqlDataAdapter(@"select SDCode from tbl_SaleDetail where ProductName='" + txtSID.Text + "' and PostStatus = 'POST'", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        String code = dt.Rows[0][0].ToString();
                        SqlDataAdapter sdas = new SqlDataAdapter("select * from tbl_SaleDetail where ProductName='" + txtSID.Text + "' and PostStatus = 'POST'", con);
                        DataTable dts = new DataTable();
                        sdas.Fill(dts);
                        if (dts.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dts;
                        }
                       
                    }
                 }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
              if (comboSearch.Text == "CustomerName")
              {
                try
                {
                    String query = "select SaleCode,Dates,CustomerName,Remarks,BillAmount,VoucherStatus from tbl_sale where CustomerName='" + txtSID.Text + "' and PostStatus = 'POST' and ReturnAmount =0";
                    DataTable d1 = clsDataLayer.RetreiveQuery(query);
                    if (d1.Rows.Count > 0)
                    {   dataGridView1.DataSource = d1;   }
                    
                }
                catch { }
            }

              if (comboSearch.Text == "CustomerName")
              {
                  try
                  {
                      String query = "select SaleCode,Dates,CustomerName,Remarks,BillAmount,VoucherStatus from tbl_sale where CustomerName='" + txtSID.Text + "' and PostStatus = 'POST' and ReturnAmount =0";
                      DataTable d1 = clsDataLayer.RetreiveQuery(query);
                      if (d1.Rows.Count > 0)
                      { dataGridView1.DataSource = d1; }

                  }
                  catch { }
              } 
         }

        private void comboSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboSearch.Text == "Product Name")
                {
                    try
                    {
                        String query = "select ProductName from tbl_SaleDetail where PostStatus = 'POST'";

                        DataTable da = clsDataLayer.RetreiveQuery(query);
                        if (da.Rows.Count > 0)
                        {
                            clsGeneral.SetAutoCompleteTextBox(txtSID, da);
                        }
                    }
                    catch { }
                } 
                else if (comboSearch.Text == "CustomerName")
                {
                    try
                    {

                        String query = "select CustomerName from tbl_sale where PostStatus = 'POST' and ReturnAmount =0";

                        DataTable da = clsDataLayer.RetreiveQuery(query);
                        if (da.Rows.Count > 0)
                        {
                            clsGeneral.SetAutoCompleteTextBox(txtSID, da);
                        }
                    }
                    catch { }
                }
                else if (comboSearch.Text == "Booker Name")
                {
                    try
                    {
                        //Booker Name
                        String query = "select BookerName from tbl_sale where PostStatus = 'POST'";

                        DataTable da = clsDataLayer.RetreiveQuery(query);
                        if (da.Rows.Count > 0)
                        {
                            clsGeneral.SetAutoCompleteTextBox(txtSID, da);
                        }
                    }
                    catch { }
                } 
            }
            catch
            {
            }
        }

        private void txtSID_TextChanged(object sender, EventArgs e)
        {
            if (comboSearch.Text == "Product Name")
            {
                try
                {
                    SqlDataAdapter sda = new SqlDataAdapter(@"select SDCode from tbl_SaleDetail where ProductName='" + txtSID.Text + "' and PostStatus = 'POST'", con); 
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {

                        String code = dt.Rows[0][0].ToString();
                        SqlDataAdapter sdas = new SqlDataAdapter("select SaleCode,CustomerName,BookerName,Dates,Remarks,BillAmount from tbl_sale where SaleCode='" + code + "' and PostStatus = 'POST' and ReturnAmount =0", con);
                        DataTable dts = new DataTable();
                        sdas.Fill(dts);
                        if (dts.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dts;
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (comboSearch.Text == "PartyName")
            {
                try
                {

                    SqlDataAdapter sda = new SqlDataAdapter(@"select PartyName from SaleBill where SaleCode = '" + txtSID.Text + "' and PostStatus = 'POST'", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        String code = dt.Rows[0][0].ToString();
                        SqlDataAdapter sdas = new SqlDataAdapter("select SaleCode,CustomerName,BookerName,Dates,Remarks,BillAmount from tbl_sale where PartyName='" + code + "' and PostStatus = 'POST'", con);
                        DataTable dts = new DataTable();
                        sdas.Fill(dts);
                        if (dts.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dts;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {  
                String q = "";
                if(comboSearch.Text.Equals("CustomerName"))
                {
                    q = "select * from View_Sale_Latest where CustomerName = '" + txtSID.Text + "' ";
                }else if(comboSearch.Text.Equals("Product Name"))
                {
                    q = "select * from View_Sale_Latest where ProductName = '" + txtSID.Text + "' ";
                }else if(comboSearch.Text.Equals("Booker Name"))
                {
                    q = "select * from View_Sale_Latest where BookerName = '" + txtSID.Text + "' ";
                } 
                    DataTable dt = clsDataLayer.RetreiveQuery(q);
                    if (dt.Rows.Count > 0)
                    {
                        Receipt rpt = new Receipt();
                        rpt.SetDataSource(dt);
                        Sale_Preview frm = new Sale_Preview(rpt);
                        frm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("No Record Found!");
                    }
           
            }
            catch { }
        }
         
    }
}
