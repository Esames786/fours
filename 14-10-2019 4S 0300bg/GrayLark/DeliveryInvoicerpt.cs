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
using System.Runtime.InteropServices.ComTypes;
using GrayLark.bin.Debug.Report; 

namespace GrayLark
{
    public partial class DeliveryInvoicerpt : Form
    {
        decimal NL = 0;
        public DeliveryInvoicerpt()
        {
            InitializeComponent(); this.KeyPreview = true;
            txtpname.Text = ""; txtsize.Text = "";

         }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        SqlCommand cmd;



        //        Delivery Challan DateWise
        //Delivery Challan Product Wise
        //Delivery Uses Date Wise
        //Delivery Uses Product Wise
        //Invoice Date Wise
        //Invoice Product Wise
        private void btnPrint_Click(object sender, EventArgs e)
        {
        try
        {
            #region re
            String get = "";
            if (comboBox1.SelectedIndex == 0)
            { 
                get = "select * from vdel where Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'";
            }
            else if (comboBox1.SelectedIndex == 1)
            { 
                get = "select * from vdel where Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "' and ProductItem='" + txtpname.Text + "' and Size='" + txtsize.Text + "'";
            }
            else if (comboBox1.SelectedIndex == 2)
            { 
                get = "select * from vdel2 where Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'"; 
            }
            else if (comboBox1.SelectedIndex == 3)
            { 
                get = "select * from vdel2 where USize='" + txtpname.Text + "' and ProductName='" + txtsize.Text + "' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'"; 
            }
            else if (comboBox1.SelectedIndex == 4)
            { 
                get = "select * from viewinv where InvoiceDate between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'";
                 }
            else if (comboBox1.SelectedIndex == 5)
            {
                
                get = "select * from viewinv where ProductName='" + txtpname.Text + "' and USize='" + txtsize.Text + "' and InvoiceDate between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'"; 
            }
            else if (comboBox1.SelectedIndex == 6)
            {
               
                get = "select * from viewinv2 where InvoiceDate between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'";
            }
            else if (comboBox1.SelectedIndex == 7)
            {
        
                get = "select * from viewinv2 where ProductName='" + txtpname.Text + "' and USize='" + txtsize.Text + "' and InvoiceDate between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'";
            }
            else if (comboBox1.SelectedIndex == 8)
            {
                get = "select * from vdel InstituteName='" + txtpname.Text + "' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'"; 
            }
            else if (comboBox1.SelectedIndex == 9)
            {
                get = "select * from vdel2 where InstituteName='" + txtpname.Text + "' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'"; 
            }
            else if (comboBox1.SelectedIndex == 10)
            {
                get = "select * from viewinv where InstituteName='" + txtpname.Text + "' and InvoiceDate between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'";
            }
            else if (comboBox1.SelectedIndex == 11)
            {
                get = "select * from viewinv2 where InstituteName='" + txtpname.Text + "' and InvoiceDate between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'";
            }
            


            #endregion re
            #region r3
             DataTable d1 = clsDataLayer.RetreiveQuery(get);
            if (d1.Rows.Count > 0) {
                //Report
#region viewreport 
                if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 1 || comboBox1.SelectedIndex == 8)
                {
                     DeliveryReport rpt = new DeliveryReport();
                    rpt.SetDataSource(d1);
                    Sale_Preview frm = new Sale_Preview(rpt);
                    frm.Show(); 
                }
                else if (comboBox1.SelectedIndex == 2 || comboBox1.SelectedIndex == 3 || comboBox1.SelectedIndex == 9) 
                {
                    DeliveryUsesReport dus = new DeliveryUsesReport(); dus.SetDataSource(d1); PaymentView sp = new PaymentView(dus); sp.Show();
                }
                else if (comboBox1.SelectedIndex == 4 || comboBox1.SelectedIndex == 5)
                {
                    rptinvoice rpt = new rptinvoice();
                    rpt.SetDataSource(d1);

                    Sale_Preview frm = new Sale_Preview(rpt);
                    frm.Show();
                }
                else if (comboBox1.SelectedIndex == 6 || comboBox1.SelectedIndex == 7 || comboBox1.SelectedIndex == 10 || comboBox1.SelectedIndex == 11)
                {
                   rptinvoice rpt = new rptinvoice();
                   rpt.SetDataSource(d1);

                    Sale_Preview frm = new Sale_Preview(rpt);
                    frm.Show();
                }    
               #endregion viewreport
                #endregion r3

            }
            else
            {
                MessageBox.Show("No Record Found!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }
        catch{ }
         }
 
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        } 

        private void Sale_Report_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
       
        private void cbstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
        try
        {  
        } catch { }
        }

        private void PettyReportView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
            else if (e.Control == true && e.KeyCode == Keys.P)
            {
                btnPrint.PerformClick();
            }
        }

     
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                #region r4
                String get = ""; String get2 = "";
                if (comboBox1.SelectedIndex == 0)
                {
                    txtpname.Enabled = false; txtsize.Enabled = false;
                    txtpname.Text = ""; txtsize.Text = ""; label4.Text = "Product Name";
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    txtpname.Enabled = true; txtsize.Enabled = true;
                    txtpname.Text = ""; txtsize.Text = ""; label4.Text = "Product Name";
                    get = "select distinct ProductItem from vdel"; get2 = "select distinct Size from vdel";
                }
                else if (comboBox1.SelectedIndex == 2)
                {
                    txtpname.Enabled = false; txtsize.Enabled = false;
                    txtpname.Text = ""; txtsize.Text = ""; label4.Text = "Product Name";
                }
                else if (comboBox1.SelectedIndex == 3)
                {
                    txtpname.Enabled = true; txtsize.Enabled = true;
                    txtpname.Text = ""; txtsize.Text = ""; label4.Text = "Product Name";
                    get = "select distinct ProductName from vdel2"; get2 = "select distinct USize from vdel2";
                }
                else if (comboBox1.SelectedIndex == 4)
                {
                    txtpname.Enabled = false; txtsize.Enabled = false;
                    txtpname.Text = ""; txtsize.Text = ""; label4.Text = "Product Name";
                }
                else if (comboBox1.SelectedIndex == 5)
                {
                    txtpname.Enabled = true; txtsize.Enabled = true;
                    txtpname.Text = ""; txtsize.Text = "";
                    label4.Text = "Product Name"; get = "select distinct ProductName from viewinv"; get2 = "select distinct USize from viewinv";
                }
                else if (comboBox1.SelectedIndex == 6)
                {
                    txtpname.Enabled = false; txtsize.Enabled = false;
                    txtpname.Text = ""; txtsize.Text = ""; label4.Text = "Product Name";
                }
                else if (comboBox1.SelectedIndex == 7)
                {
                    txtpname.Enabled = true; txtsize.Enabled = true;
                    txtpname.Text = ""; txtsize.Text = ""; label4.Text = "Product Name";
                    get = "select distinct ProductName from viewinv2";
                    get2 = "select distinct USize from viewinv2";
                }
                else if (comboBox1.SelectedIndex == 8)
                {
                    label4.Text = "Institute Name"; txtpname.Enabled = true; txtsize.Enabled = false; txtpname.Text = ""; txtsize.Text = ""; get = "select distinct InstituteName from vdel";
                }
                else if (comboBox1.SelectedIndex == 9)
                {
                    label4.Text = "Institute Name"; txtpname.Enabled = true; txtsize.Enabled = false; txtpname.Text = ""; txtsize.Text = ""; get = "select distinct InstituteName from vdel2";
                }
                else if (comboBox1.SelectedIndex == 10)
                {
                    label4.Text = "Institute Name"; txtpname.Enabled = true; txtsize.Enabled = false; txtpname.Text = ""; txtsize.Text = ""; get = "select distinct InstituteName from viewinv";
                }
                else if (comboBox1.SelectedIndex == 11)
                {
                    label4.Text = "Institute Name"; txtpname.Enabled = true; txtsize.Enabled = false; txtpname.Text = ""; txtsize.Text = ""; get = "select distinct InstituteName from viewinv2";
                }


                DataTable d1 = clsDataLayer.RetreiveQuery(get);
                if (d1.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txtpname, d1); }

                DataTable d2 = clsDataLayer.RetreiveQuery(get2);
                if (d2.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txtsize, d2); }
                #endregion r4
            }
            catch { }
        }

        
          
    }
}
