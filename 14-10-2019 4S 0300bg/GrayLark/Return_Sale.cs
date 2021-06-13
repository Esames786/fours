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
    public partial class Return_Sale : Form
    {
        String UID = Login.UserID;
        public Return_Sale()
        {
            InitializeComponent();
            textBox1.Enabled = false;

            comboBox1.Text = "Date Wise";
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

        public void print()
        {
            try
            {
                if (comboBox2.SelectedIndex == 0) //Sale Return
                {
                    if (comboBox1.SelectedIndex == 0)
                    {

                        string q = "select * from tblReturnOrder where SaleId like 'SA-%' and ReturnDate between '" + date1.Text + "' and '" + date2.Text + "' ";
                        DataTable report = clsDataLayer.RetreiveQuery(q);
                        if (report.Rows.Count > 0)
                        {
                            ReturnSaleReport rpt = new ReturnSaleReport();
                            rpt.SetDataSource(report);
                            Return_Preview rp = new Return_Preview(UID, date1.Text, date2.Text, rpt);
                            rp.Show();
                        }
                        else
                        {
                            MessageBox.Show("No Record Found !");
                        }
                    }

                    else
                    {
                        string q1 = "select * from tblReturnOrder where SaleId like 'SA-%' and ReturnDate between '" + date1.Text + "' and '" + date2.Text + " ' and CustomerId = '" + textBox1.Text + "'";
                        DataTable report1 = clsDataLayer.RetreiveQuery(q1);
                        if (report1.Rows.Count > 0)
                        {
                            ReturnSaleReport rpt1 = new ReturnSaleReport();
                            rpt1.SetDataSource(report1);
                            Return_Preview rp1 = new Return_Preview(UID, date1.Text, date2.Text, rpt1);
                            rp1.Show();
                        }
                        else
                        {
                            MessageBox.Show("No Record Found !");
                        }
                    }

                }

                if (comboBox2.SelectedIndex == 1) //Purchase Return
                {
                    if (comboBox1.SelectedIndex == 0)
                    {

                        string q = "select * from tblReturnOrder where SaleId like 'P-%' and ReturnDate between '" + date1.Text + "' and '" + date2.Text + "' ";
                        DataTable report = clsDataLayer.RetreiveQuery(q);
                        if (report.Rows.Count > 0)
                        {
                            ReturnPurchaseReport rpt = new ReturnPurchaseReport();
                            rpt.SetDataSource(report);
                            Return_Preview rp = new Return_Preview(UID, date1.Text, date2.Text, rpt);
                            rp.Show();
                        }
                        else
                        {
                            MessageBox.Show("No Record Found !");
                        }
                    }

                    else
                    {
                        string q1 = "select * from tblReturnOrder where SaleId like 'P-%' and ReturnDate between '" + date1.Text + "' and '" + date2.Text + " ' and CustomerId = '" + textBox1.Text + "'";
                        DataTable report1 = clsDataLayer.RetreiveQuery(q1);
                        if (report1.Rows.Count > 0)
                        {
                            ReturnPurchaseReport rpt1 = new ReturnPurchaseReport();
                            rpt1.SetDataSource(report1);
                            Return_Preview rp1 = new Return_Preview(UID, date1.Text, date2.Text, rpt1);
                            rp1.Show();
                        }
                        else
                        {
                            MessageBox.Show("No Record Found !");
                        }
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

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0) // Sale
            {
                String h5 = "select CustomerId from tblReturnOrder where SaleId like 'SA-%'";
                DataTable d5 = clsDataLayer.RetreiveQuery(h5);
                if (d5.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(textBox1, d5);
                } 
            }
                else     // Purchase
                {
                    String h6 = "select CustomerId from tblReturnOrder where SaleId like 'P-%'";
                    DataTable d6 = clsDataLayer.RetreiveQuery(h6);
                    if (d6.Rows.Count > 0)
                    {
                        clsGeneral.SetAutoCompleteTextBox(textBox1, d6);
                    } 
                }
            }
        }

        
    }
