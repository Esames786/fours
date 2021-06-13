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
    public partial class Sale_Report : Form
    {
        String Code = "";
        public Sale_Report()
        {
            InitializeComponent();
            this.KeyPreview = true;
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
                String tbl = "";
                if (checkBox1.Checked)
                {
                    tbl = "tblnInvoice";
                }
                else
                {
                    tbl = "tblInvoice";
                }
                String query = ""; DataTable d1;
                if (comboBox1.SelectedIndex == 0)
                {
                    query = "select * from " + tbl + " where InvoiceDate between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "' Order By InCode asc";
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    query = "select * from " + tbl + " where InvoiceDate between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "' and InstituteName='" + txtcus.Text + "' Order By InCode asc";

                }
                else if (comboBox1.SelectedIndex == 2)
                {
                    PartyCode();
                    string q;
                    q = "select * from LedgerReceived where Datetime between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "' and AccountCode='" + Code + "' Order by ID asc";
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
                 
                
                d1 = clsDataLayer.RetreiveQuery(query);

                if (d1.Rows.Count > 0)
                {
                    dailysalereport rpt = new dailysalereport();
                    rpt.SetDataSource(d1);
                    Daily_Sale_Report dsp = new Daily_Sale_Report(date1.Text, date2.Text, rpt);
                    dsp.Show();
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
                label3.Visible = false;
                txtcus.Visible = false;
            }
            if (comboBox1.SelectedIndex == 1)
            {
                label3.Text = "Customer Name";
                label3.Visible = true;
                txtcus.Visible = true;
                String tbl = "";
                if (checkBox1.Checked)
                {
                    tbl = "tblnInvoice";
                }
                else
                {
                    tbl = "tblInvoice";
                }
                String sel = "select distinct InstituteName from "+tbl+""; DataTable ds = clsDataLayer.RetreiveQuery(sel);
                if (ds.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtcus, ds);
                }
            }
            if (comboBox1.SelectedIndex == 2)
            {
                label3.Text = "Ledger of Vendor";
                label3.Visible = true;
                txtcus.Visible = true;

                String sele = "select distinct Particulars from LedgerReceived";
                DataTable d = clsDataLayer.RetreiveQuery(sele);
                if (d.Rows.Count > 0)  { clsGeneral.SetAutoCompleteTextBox(txtcus, d);  }
            } 
                 

            }

        private void Sale_Report_KeyDown(object sender, KeyEventArgs e)
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

        }
    }
