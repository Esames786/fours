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
    public partial class DamageReports : Form
    {
        String Code = "";
        public DamageReports()
        {
            InitializeComponent();

        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        SqlCommand cmd;

        private void PartyCode()
        {
            String sel = "select ActCode from Accounts where ActTitle = '" + txtcus.Text + "'";
            DataTable dc = clsDataLayer.RetreiveQuery(sel);
            if (dc.Rows.Count > 0)
            {
                Code = dc.Rows[0][0].ToString();
            }
        }

        private void Printing()
        {
            try
            {
                String query = ""; DataTable d1;
                if (comboBox1.SelectedIndex == 0)
                {
                    query = "select * from DamageView where Date between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "' Order By TCS_CODE asc";
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    query = "select * from DamageView where Date between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "' and outlet_client='" + txtcus.Text + "' Order By TCS_CODE asc";

                }  
                else
                {
                    //  MessageBox.Show("No Record Found!");
                }
                d1 = clsDataLayer.RetreiveQuery(query);

                if (d1.Rows.Count > 0)
                {
                    rptDamageProduct fp = new rptDamageProduct(); fp.SetDataSource(d1);
                    LedgerView pv = new LedgerView(date1.Value.ToString("yyyy-MM-dd"), date2.Value.ToString("yyyy-MM-dd"),fp); pv.Show();
                }
            }
            catch { }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            Printing();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            txtcus.Text = "";
        }

        private void Sale_Report_Load(object sender, EventArgs e)
        {

        }

        private void Sale_Report_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
       if (comboBox1.SelectedIndex == 0)
       {
           label3.Visible = false;  txtcus.Visible = false;
       }
       if (comboBox1.SelectedIndex == 1)
       {
           label3.Text = "Customer Name";  label3.Visible = true;  txtcus.Visible = true;
      
           String sel = "select distinct outlet_client from tbl_DamageProduct"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
           if (ds.Rows.Count > 0)
           {
               clsGeneral.SetAutoCompleteTextBox(txtcus, ds);
           }
       }  
        }

        }
    }
