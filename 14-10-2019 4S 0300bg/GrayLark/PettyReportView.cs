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
    public partial class PettyReportView : Form
    {
        decimal NL = 0;
        public PettyReportView()
        {
            InitializeComponent(); this.KeyPreview = true;
         }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        SqlCommand cmd; 

        private void BalanceProfit()
        {
            try
            {
            decimal sales = 0;
            String sale = "select sum(CashReceived) from Transactionss where Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(sale);
            String s1 = "";
            if (d1.Rows.Count > 0)
            {
                s1 = d1.Rows[0][0].ToString();
                if (s1.Equals(""))
                {
                    sales = 0;
                }
                else
                {
                    sales = Convert.ToDecimal(d1.Rows[0][0].ToString());
                }
            }
            decimal purchase = 0;
            String q2 = "select sum(NetAmount) from Purchase where Date between '" + date1.Text + "' and '" + date2.Text + "' "; DataTable d2 = clsDataLayer.RetreiveQuery(q2);
            if (d2.Rows.Count > 0)
            {
                String hk = d2.Rows[0][0].ToString();
                if (hk.Equals(""))
                {
                    purchase = 0;
                }
                else
                {
                    purchase = Convert.ToDecimal(d2.Rows[0][0].ToString());
                }
            }
            else { purchase = 0; } 
                String a1 = "select distinct PartyName from Transactions where HeadActCode='5'";
                DataTable gh = clsDataLayer.RetreiveQuery(a1);
                if (gh.Rows.Count > 0)
                {  
                String query = "select sum(CashGiven) from Transactions where HeadActCode='5' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'";
                DataTable ds = clsDataLayer.RetreiveQuery(query); if (ds.Rows.Count > 0)
                {
                    String totexp = ds.Rows[0][0].ToString();
                    decimal e = Convert.ToDecimal(totexp);
                    decimal netpurchase = purchase;
                    decimal ff = sales - netpurchase;
                    decimal f1 = ff - e; NL = f1;
                }
                else
                {
                } 
                } 
            }
            catch { }
        }

        //
        private void Profit()
        {
            try
            {
                decimal sales = 0;
                String sale = "select sum(CashReceived) from Transactionss where VoucherStatus='Received' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(sale);
                String s1 = "";
                if (d1.Rows.Count > 0)
                {
                    s1 = d1.Rows[0][0].ToString();
                    if (s1.Equals(""))
                    {
                        sales = 0;
                    }
                    else
                    {
                        sales = Convert.ToDecimal(d1.Rows[0][0].ToString());
                    }
                }
                decimal purchase = 0;
                String q2 = "select sum(NetAmount) from Purchase where Date between '" + date1.Text + "' and '" + date2.Text + "' "; DataTable d2 = clsDataLayer.RetreiveQuery(q2);
                if (d2.Rows.Count > 0)
                {
                    String hk = d2.Rows[0][0].ToString();
                    if (hk.Equals(""))
                    {
                        purchase = 0;
                    }
                    else
                    {
                        purchase = Convert.ToDecimal(d2.Rows[0][0].ToString());
                    }
                }
                else { purchase = 0; } 
                DataTable fd = new DataTable();
                fd.Columns.Add("PartyName"); fd.Columns.Add(new DataColumn("CashGiven", System.Type.GetType("System.Int64")));
                String a1 = "select distinct PartyName from Transactions where HeadActCode='5'";
                DataTable gh = clsDataLayer.RetreiveQuery(a1);
                if (gh.Rows.Count > 0)
                {
                    for (int u = 0; u < gh.Rows.Count; u++)
                    {
                    String name = gh.Rows[u][0].ToString();
                    String query = "select sum(CashGiven) from Transactions where PartyName='" + name + "' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'";
                    DataTable ds = clsDataLayer.RetreiveQuery(query); if (ds.Rows.Count > 0)
                    {
                    String totexp = ds.Rows[0][0].ToString();
                    decimal e = 0;
                    if (totexp.Equals(""))
                    {
                        e = 0;
                    }
                    else
                    {
                        e = Convert.ToDecimal(totexp);
                    }
                    fd.Rows.Add(name, e);
                    }
                    }
                }

                String a9 = "select distinct Exp_Head from tbl_PettyCashDetail where Date between  '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'";
                DataTable gh3 = clsDataLayer.RetreiveQuery(a9);
                    if (gh3.Rows.Count > 0)
                    {
                        for (int u4 = 0; u4 < gh3.Rows.Count; u4++)
                        {
                            String name = gh.Rows[u4][0].ToString();
                            String query = "select sum(Amount) from tbl_PettyCashDetail where Exp_Head='" + name + "' and Date between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'";
                            DataTable ds = clsDataLayer.RetreiveQuery(query); if (ds.Rows.Count > 0)
                            {
                                String totexp = ds.Rows[0][0].ToString();
                                decimal e = 0;
                                if (totexp.Equals(""))
                                {
                                    e = 0;
                                }
                                else
                                {
                                    e = Convert.ToDecimal(totexp);
                                }
                                fd.Rows.Add(name, e);
                            }
                        }
                    }
                ProfitMulticon pfm = new ProfitMulticon(); pfm.SetDataSource(fd);
                ProfitViewss pm = new ProfitViewss(sales.ToString(), purchase.ToString(), date1.Value.ToString("yyyy-MM-dd"), date2.Value.ToString("yyyy-MM-dd"), pfm); pm.Show();

            }
            catch { }
        }

        //
        private void AssetList()
        {
            String hm = "select PartyName,Remarks,CashGiven from Transactions where HeadActCode='101' or HeadActCode='102' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "' "; DataTable vf = clsDataLayer.RetreiveQuery(hm);
            if (vf.Rows.Count > 0) {
                rptAsset rp = new rptAsset(); rp.SetDataSource(vf); LedgerView pv = new LedgerView(date1.Value.ToString("dd-MMMM-yyyy"), date2.Value.ToString("dd-MMMM-yyyy"), rp); pv.Show();
            }
            else
            {
                MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void PettyCash()
        {
            String qu = "select * from PettyView where Petty_User='" + txtuser.Text + "' and Date between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'";
            DataTable de = clsDataLayer.RetreiveQuery(qu);
            if (de.Rows.Count > 0)
            {
                String Total = "";
                String a1 = "select  sum(TotalAmount) from PettyView where Petty_User='" + txtuser.Text + "' and Date between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(a1); if (d1.Rows.Count > 0)
                {
                    Total = d1.Rows[0][0].ToString();
                }
                String pb = "";
                String a2 = "select Remaining_Amount from  tbl_issuepettyamount where Petty_User='" + txtuser.Text + "' and MonthYear='" + DateTime.Now.AddMonths(-1).ToString("MMMM,yyyy") + "'";
                DataTable d2 = clsDataLayer.RetreiveQuery(a2); if (d2.Rows.Count > 0)
                {
                    pb = d2.Rows[0][0].ToString();
                }
                else
                {
                    pb = "0";
                }
                LatestPettyCash pcs = new LatestPettyCash(); pcs.SetDataSource(de);
                PettyCReport pv = new PettyCReport(txtuser.Text, date1.Value.ToString("yyyy-MM-dd"), date2.Value.ToString("yyyy-MM-dd"), Total, pb); pv.Show();
            }
            else
            {
                MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
        try
        {
            if (comboBox1.SelectedIndex == 0)
            {
                Profit();
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                BalanceSheet();
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                AssetList();
            }
            else
            {
                PettyCash();
            }
        }
        catch{ }
         }

        private void BalanceSheet()
        {
        try
        {
        decimal cash = 0, ReceiveDue = 0, PaymentDue = 0, Inventory = 0, Salary = 0, Capital = 0;
        String q1 = "select CASH from tech_cash"; DataTable d1 = clsDataLayer.RetreiveQuery(q1); if (d1.Rows.Count > 0) {
            String rm = d1.Rows[0][0].ToString();
            if (!rm.Equals(""))
            {
                cash = Convert.ToDecimal(d1.Rows[0][0].ToString());
            } 
        }
        String q2 = "select sum(DueAmount) from ReceiveDue where DueAmount!=0 and PartyCode!=528"; DataTable d2 = clsDataLayer.RetreiveQuery(q2); if (d2.Rows.Count > 0)
        {
            String rm = d2.Rows[0][0].ToString();
        if (!rm.Equals(""))
        {
            ReceiveDue = Convert.ToDecimal(d2.Rows[0][0].ToString());
        } 
        }
        String q3 = "select sum(DueAmount) from PaymentDue where DueAmount!=0 and PartyCode!=528"; DataTable d3 = clsDataLayer.RetreiveQuery(q3); if (d3.Rows.Count > 0)
        {
       String rm = d3.Rows[0][0].ToString();
       if (!rm.Equals(""))
       {
           PaymentDue = Convert.ToDecimal(d3.Rows[0][0].ToString());
       }      
        }
        String q5 = "select sum(PURCHASE_PRICE) from add_product where Quantity!=0"; DataTable d5 = clsDataLayer.RetreiveQuery(q5); if (d5.Rows.Count > 0)
        {
            String rm = d5.Rows[0][0].ToString();
            if (!rm.Equals(""))
            {
                Inventory = Convert.ToDecimal(d5.Rows[0][0].ToString());
            }  
        }
        String q6 = "select sum(CashGiven) from Transactions where PartyName='Salary' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'"; DataTable d6 = clsDataLayer.RetreiveQuery(q6); if (d6.Rows.Count > 0) {
            String rm = d6.Rows[0][0].ToString();
            if (!rm.Equals(""))
            {
                Salary = Convert.ToDecimal(d6.Rows[0][0].ToString());
            }
        }
        String q7 = "select sum(CashReceived) from Transactionss where PartyName='Capital' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'"; DataTable d7 = clsDataLayer.RetreiveQuery(q7); if (d7.Rows.Count > 0)
        {
            String rm = d7.Rows[0][0].ToString();
            if (!rm.Equals(""))
            {
                Capital = Convert.ToDecimal(d7.Rows[0][0].ToString());
            }
        }
            decimal Drawing = 0;
            String q8 = "select sum(CashGiven) from Transactions where PartyName='Drawing' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'"; DataTable d8 = clsDataLayer.RetreiveQuery(q8); if (d8.Rows.Count > 0)
            {
                String rm1 = d8.Rows[0][0].ToString();
                if (!rm1.Equals(""))
                {
                    Drawing = Convert.ToDecimal(d8.Rows[0][0].ToString());
                }
            }

            String hm = "select PartyName,CashGiven from Transactions where HeadActCode='101' or HeadActCode='102' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "' "; DataTable vf = clsDataLayer.RetreiveQuery(hm);
            if (vf.Rows.Count > 0) { }
            BalanceProfit();
            BalanceSheet bs = new BalanceSheet(); bs.SetDataSource(vf);
        BalanceSheetViewer bv = new BalanceSheetViewer(bs,cash, ReceiveDue, PaymentDue, Inventory, Salary, Capital,Drawing,NL); bv.Show();
        }catch { }
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
            if (comboBox1.SelectedIndex == 3)
            {
                txtuser.Enabled = true; txtuser.Text = "";
                String query = "select ActTitle from Accounts where HeaderActCode = '10103'"; DataTable d = clsDataLayer.RetreiveQuery(query); if (d.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txtuser, d); }
            }
            else
            {
                txtuser.Enabled = false; txtuser.Text = "";
            }
        }

        
          
    }
}
