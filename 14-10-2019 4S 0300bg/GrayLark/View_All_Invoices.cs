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
    public partial class View_All_Invoices : Form
    {
        String FormName = "";
        String Code = ""; String HeaderAccount = "";
        public View_All_Invoices(String abc)
        {
            InitializeComponent();
             FormName = abc;
         }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

        private void View_All_Sale_Load(object sender, EventArgs e)
        {
            onload2();
        }


        private void onload2()
        {
            if (checkBox1.Checked)
            {
                #region p1
                String query = "select ProductName from tblnInvoiceDetail";
                DataTable da = clsDataLayer.RetreiveQuery(query);
                if (da.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtSID, da);
                }
                comboSearch.SelectedIndex = 0;
                this.txtSID.Focus();
                try
                {
                    SqlDataAdapter sda = new SqlDataAdapter("select InCode,InstituteName,DeliveryCode,InvoiceAmount,InvoiceDate,status,Remarks from tblnInvoice", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dt;
                    }
                }
                catch { }
                #endregion p1
            }
            else
            {
                #region p2
                String query = "select ProductName from tblInvoiceDetail";
                DataTable da = clsDataLayer.RetreiveQuery(query);
                if (da.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtSID, da);
                }
                comboSearch.SelectedIndex = 0;
                this.txtSID.Focus();
                try
                {
                    SqlDataAdapter sda = new SqlDataAdapter("select InCode,InstituteName,DeliveryCode,InvoiceAmount,InvoiceDate,status,Remarks from tblInvoice", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dt;
                    }
                }
                catch { }
                #endregion p2
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          
                txtSID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                Invoice ps = new Invoice();
      
                ps.txtsale.Text = txtSID.Text;
                ps.Show();
                
                this.Hide();
            
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
                    String tbl2 = "";
                    if (checkBox1.Checked)
                    {
                        tbl2 = "tblnInvoiceDetail";
                    }
                    else
                    {
                        tbl2 = "tblInvoiceDetail";
                    }
                    SqlDataAdapter sda = new SqlDataAdapter(@"select InCode from " + tbl2 + " where ProductName='" + txtSID.Text + "'", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        String code = dt.Rows[0][0].ToString();
                        SqlDataAdapter sdas = new SqlDataAdapter("select * from " + tbl2 + " where ProductName='" + txtSID.Text + "' ", con);
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
                    String tbl23 = "";
                    if (checkBox1.Checked)
                    {
                        tbl23 = "tblnInvoice";
                    }
                    else
                    {
                        tbl23 = "tblInvoice";
                    }
                    String query = "select InCode,InstituteName,DeliveryCode,InvoiceAmount,InvoiceDate,status,Remarks from "+tbl23+" where InstituteName='" + txtSID.Text + "'";
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
                        String tbl23 = "";
                        if (checkBox1.Checked)
                        {
                            tbl23 = "tblnInvoice";
                        }
                        else
                        {
                            tbl23 = "tblInvoice";
                        }
                        String query = "select InCode from "+tbl23+""; 
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
                        String tbl23 = "";
                        if (checkBox1.Checked)
                        {
                            tbl23 = "tblnInvoice";
                        }
                        else
                        {
                            tbl23 = "tblInvoice";
                        }
                        String query = "select InstituteName from " + tbl23 + "";

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
                    String tbl23 = "";
                    if (checkBox1.Checked)
                    {
                        tbl23 = "tblnInvoiceDetail";
                    }
                    else
                    {
                        tbl23 = "tblInvoiceDetail";
                    }
                    String sd = "select InCode from "+tbl23+" where ProductName='" + txtSID.Text + "'";
                    DataTable dt = clsDataLayer.RetreiveQuery(sd);
                    if (dt.Rows.Count > 0)
                    {
                        String tbl223 = "";
                        if (checkBox1.Checked)
                        {
                            tbl223 = "tblnInvoice";
                        }
                        else
                        {
                            tbl223 = "tblInvoice";
                        }
                        String code = dt.Rows[0][0].ToString();
                        String sh = "select InCode,InstituteName,DeliveryCode,InvoiceAmount,InvoiceDate,status,Remarks from " + tbl223 + " where InCode='" + code + "'";
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
                    String tbl223 = "";
                    if (checkBox1.Checked)
                    {
                        tbl223 = "tblnInvoice";
                    }
                    else
                    {
                        tbl223 = "tblInvoice";
                    }
                    String sh = "select InCode from "+tbl223+" where InCode = '" + txtSID.Text + "'";
                    DataTable dt = clsDataLayer.RetreiveQuery(sh);  
                    if (dt.Rows.Count > 0)
                    {
                        String code = dt.Rows[0][0].ToString();
                        String s2 = "select InCode,InstituteName,DeliveryCode,InvoiceAmount,InvoiceDate,status,Remarks from " + tbl223 + " where InCode='" + txtSID.Text + "' ";
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
                String tbl243 = "";
                if (checkBox1.Checked)
                {
                    tbl243 = "tblnInvoice";
                }
                else
                {
                    tbl243 = "tblInvoice";
                }
                String s2 = "select InCode,InstituteName,DeliveryCode,InvoiceAmount,InvoiceDate,status,Remarks from "+tbl243+"";
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
                String tbl243 = "";
                if (checkBox1.Checked)
                {
                    tbl243 = "viewinv2";
                }
                else
                {
                    tbl243 = "viewinv";
                }
                String q = "";
                if(comboSearch.Text.Equals("InstituteName"))
                {
                    q = "select * from "+tbl243+" where InstituteName = '" + txtSID.Text + "' ";
                }
                else if (comboSearch.Text.Equals("InvoiceNo"))
                {
                    q = "select * from " + tbl243 + " where ProductName = '" + txtSID.Text + "' ";
                } 
                    DataTable dt = clsDataLayer.RetreiveQuery(q);
                    if (dt.Rows.Count > 0)
                    {
                        rptinvoice rpt = new rptinvoice();
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            onload2();
        }
         
    }
}
