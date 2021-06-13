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
    public partial class Purchase_Sale : Form
    {
        String UID = Login.UserID;
        String Code = "";
        public Purchase_Sale()
        {
            InitializeComponent();
            textBox1.Enabled = false;

            comboBox1.Text = "Date Wise";

            String h5 = "select VendorName from Purchase";
            DataTable d5 = clsDataLayer.RetreiveQuery(h5);
            if (d5.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(textBox1, d5);
            } 

        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        SqlCommand cmd;

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            print();
        }

        private void PartyCode()
        {
            String sel = "select ActCode from Accounts where ActTitle = '" + textBox1.Text + "'";
            DataTable dc = clsDataLayer.RetreiveQuery(sel);
            if (dc.Rows.Count > 0)
            {
                Code = dc.Rows[0][0].ToString();
            }
        }
         
        public void print()
        {
            try
            {

                if (comboBox1.SelectedIndex == 0)
                {

                    string q = "select * from Purchase where Date between '" + date1.Text + "' and '" + date2.Text + "' ";
                    DataTable purchase = clsDataLayer.RetreiveQuery(q);
                    if (purchase.Rows.Count > 0)
                    {
                        dailypurchasereport rpt = new dailypurchasereport();
                        rpt.SetDataSource(purchase);
                        Daily_Sale_Report pop = new Daily_Sale_Report(date1.Text, date2.Text, rpt);
                        pop.Show();
                    }
                    else
                    {
                        MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    string q = "select * from Purchase where Date between '" + date1.Text + "' and '" + date2.Text + "'  and VendorName='" + textBox1.Text + "'";
                    DataTable purchase = clsDataLayer.RetreiveQuery(q);
                    if (purchase.Rows.Count > 0)
                    {
                        dailypurchasereport rpt = new dailypurchasereport();
                        rpt.SetDataSource(purchase);
                        Daily_Sale_Report pop = new Daily_Sale_Report(date1.Text, date2.Text, rpt);
                        pop.Show();
                    }
                    else
                    {
                        MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    PartyCode();
                    string q = "";
                    q = "select * from LedgerPayment where Datetime between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "' and AccountCode='" + Code + "'  Order By ID ASC";
                    DataTable purchase = clsDataLayer.RetreiveQuery(q);
                    if (purchase.Rows.Count > 0)
                    {
                        rptLedger rpt = new rptLedger();
                        rpt.SetDataSource(purchase);
                        LedgerView pop = new LedgerView(date1.Text, date2.Text, rpt);
                        pop.Show();
                    }
                    else
                    {
                        MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Return_Sale_Load(object sender, EventArgs e)
        {

        }

        private void Return_Sale_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {

                textBox1.Text = "";
                textBox1.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 1)
            {

                textBox1.Text = "";
                textBox1.Enabled = true;
            }
            else
            {
                textBox1.Text = "";                textBox1.Enabled = true;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                print();
            }
        }

        private void date1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                print();
            }
        }

        private void date2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                print();
            }
        }  
    }
}
