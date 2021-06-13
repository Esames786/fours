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
    public partial class View_All_Return_Orders : Form
    {
        public View_All_Return_Orders()
        {
            InitializeComponent();
            SaleId();
            String search = " select CustomerId from returnreports";
            DataTable dr = clsDataLayer.RetreiveQuery(search);
            if(dr.Rows.Count>0)
            {
                clsGeneral.SetAutoCompleteTextBox(txt_custname, dr);
            }
        }

      SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
      
        private void SaleId()
        {
            String query = "select ReturnCode FROM tblReturnOrder";
            DataTable dt = clsDataLayer.RetreiveQuery(query);
            if(dt.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBoxs(txtSearch, dt);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    txtSearch.Items.Add(dt.Rows[i]["ReturnCode"]);
                //}
                clsGeneral.SetAutoCompleteTextBoxs(txtSearch, dt);
                txtSearch.DataSource = dt;
                txtSearch.DisplayMember = "ReturnCode";
                txtSearch.ValueMember = "ReturnCode";

            }
        }

        private void View_All_Return_Orders_Load(object sender, EventArgs e)
        {
            try 
            {   String qs = "select  * from returnreports";
                DataTable dz = clsDataLayer.RetreiveQuery(qs);
                if(dz.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dz;
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            methodonSearch();
        }

        private void methodonSearch()
        { 
            try
            {
                if(!txtSearch.Text.Equals(""))
                {

                    String qs = "select * from returnreports where ReturnCode = '" + txtSearch.Text + "'";
                DataTable dz = clsDataLayer.RetreiveQuery(qs);
                if (dz.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dz;
                }
                else
                {
                    dataGridView1.DataSource = null;
                }
                }
                else
                {
                    String qs = "select * from returnreports";
                    DataTable dz = clsDataLayer.RetreiveQuery(qs);
                    if (dz.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dz;
                    }
                    else
                    {
                        dataGridView1.DataSource = null;
                    }
                }
            }
            catch  
            {
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtSearch.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!txtSearch.Text.Equals(""))
            {
                  ReturnOrder ret = new ReturnOrder();
                  ret.SetDataSource(clsDataLayer.RetreiveQuery("select * from returnreports where SaleId = '" + txtSearch.Text + "'"));


                  Return_Order_Preview rop = new Return_Order_Preview(ret);
                rop.passRetSellID = txtSearch.Text;
                rop.Show();
            }
            else
            {
                MessageBox.Show("Please Search Any Return Id!");
            }
        }

        private void txtSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            methodonSearch();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txt_custname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!txt_custname.Text.Equals(""))
                {
                    String qs = "select * from returnreports where CustomerId='" + txt_custname.Text + "'";
                    DataTable dz = clsDataLayer.RetreiveQuery(qs);
                    if (dz.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dz;
                        txtSearch.Text = dz.Rows[0]["SaleId"].ToString();

                    }
                    else
                    {
                        dataGridView1.DataSource = null;
                    }
                }
                else
                {
                    String qs = "select * from returnreports";
                    DataTable dz = clsDataLayer.RetreiveQuery(qs);
                    if (dz.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dz;
                    }
                    else
                    {
                        dataGridView1.DataSource = null;
                    }
                }
            }
            catch
            {
            }
        }
    }
}
