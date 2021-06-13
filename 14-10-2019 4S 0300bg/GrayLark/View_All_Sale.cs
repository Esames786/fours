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
    public partial class View_All_Sale : Form
    {
        String FormName = "";
        String Code = ""; String HeaderAccount = "";
        public View_All_Sale(String abc)
        {
            InitializeComponent();
             FormName = abc;
         }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

        private void View_All_Sale_Load(object sender, EventArgs e)
        {
            String query = "select ProductItem from tbl_DeliveryDetail";
            DataTable da= clsDataLayer.RetreiveQuery(query);
            if(da.Rows.Count > 0)
            {
         clsGeneral.SetAutoCompleteTextBox(txtSID,da);
            }
                comboSearch.SelectedIndex = 0;
            this.txtSID.Focus();
            try 
            {
                SqlDataAdapter sda = new SqlDataAdapter("select DCode,InstituteName,Remarks,Warehouse,Saleperson,ChallanStatus,Dates,vstatus from tbl_Delivery", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if(dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                } 
            }
            catch  
            {
              
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (FormName.Equals("ProductSale"))
            {
                txtSID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                DeliveryChallan ps = new DeliveryChallan();
      
                ps.txtsale.Text = txtSID.Text;
                ps.Show();
                
                this.Close();
            } 
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

                    SqlDataAdapter sda = new SqlDataAdapter(@"select DCode from tbl_DeliveryDetail where ProductItem='" + txtSID.Text + "'", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        String code = dt.Rows[0][0].ToString();
                        SqlDataAdapter sdas = new SqlDataAdapter("select * from tbl_DeliveryDetail where ProductItem='" + txtSID.Text + "' ", con);
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
            
              if (comboSearch.Text == "InstituteName")
            {
                try
                {
                    String query = "select DCode,InstituteName,Remarks,Warehouse,Saleperson,ChallanStatus,Dates,vstatus from tbl_Delivery where InstituteName='" + txtSID.Text + "'";
                    DataTable d1 = clsDataLayer.RetreiveQuery(query);
                    if (d1.Rows.Count > 0)
                    {   dataGridView1.DataSource = d1;   }
                    
                }
                catch { }
            } 
         }

        private void comboSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboSearch.Text == "InvoiceNo")
                {
                    try
                    {
                        String query = "select DCode from tbl_Delivery"; 
                        DataTable da = clsDataLayer.RetreiveQuery(query);
                        if (da.Rows.Count > 0)
                        {  clsGeneral.SetAutoCompleteTextBox(txtSID, da); }
                    }
                    catch { }
                } 
                else if (comboSearch.Text == "InstituteName")
                {
                    try
                    {
                        String query = "select InstituteName from tbl_Delivery";

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
                    String sd = "select DCode from tbl_DeliveryDetail where ProductItem='" + txtSID.Text + "'";
                    DataTable dt = clsDataLayer.RetreiveQuery(sd);
                    if (dt.Rows.Count > 0)
                    { 
                        String code = dt.Rows[0][0].ToString();
                        String sh = "select DCode,InstituteName,Remarks,Warehouse,Saleperson,ChallanStatus,Dates,vstatus from tbl_Delivery where DCode='" + code + "'";
                        DataTable dts = clsDataLayer.RetreiveQuery(sh);
                        if (dts.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dts;
                        }
                    }
                }catch{  }
            }
            else if (comboSearch.Text == "InvoiceNo")
            {
                try
                {
                    String sh = "select DCode from tbl_Delivery where DCode = '" + txtSID.Text + "'";
                    DataTable dt = clsDataLayer.RetreiveQuery(sh);  
                    if (dt.Rows.Count > 0)
                    {
                        String code = dt.Rows[0][0].ToString();
                        String s2 = "select DCode,InstituteName,Remarks,Warehouse,Saleperson,ChallanStatus,Dates,vstatus from tbl_Delivery where DCode='" + txtSID.Text + "' ";
                        DataTable dts = clsDataLayer.RetreiveQuery(s2);
                        if (dts.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dts;
                        }
                    }
                }
                catch { }
            }
            else
            {
                String s2 = "select DCode,InstituteName,Remarks,Warehouse,Saleperson,ChallanStatus,Dates,vstatus from tbl_Delivery";
                DataTable dts = clsDataLayer.RetreiveQuery(s2);
                if (dts.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dts;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            try
            {
                String q = "";
                if(comboSearch.Text.Equals("InstituteName"))
                {
                    q = "select * from vdelivery where InstituteName = '" + txtSID.Text + "' ";
                }
                else if (comboSearch.Text.Equals("InvoiceNo"))
                {
                    q = "select * from vdelivery where ProductItem = '" + txtSID.Text + "' ";
                } 
                    DataTable dt = clsDataLayer.RetreiveQuery(q);
                    if (dt.Rows.Count > 0)
                    {
                        DeliveryReport rpt = new DeliveryReport();
                        rpt.SetDataSource(dt);
                        Sale_Preview frm = new Sale_Preview(rpt);
                        frm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("No Record Found!","Stop",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    }
           
            }
            catch { }
        }  
        private void PartyCode(String title)
        {
            String sel = "select ActCode,HeaderActCode from Accounts where ActTitle = '" + title + "'";
            DataTable dc = clsDataLayer.RetreiveQuery(sel);
            if (dc.Rows.Count > 0) { Code = dc.Rows[0][0].ToString(); HeaderAccount = dc.Rows[0][1].ToString(); }
        }
         
    }
}
