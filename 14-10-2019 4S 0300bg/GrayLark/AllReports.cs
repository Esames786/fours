
using GrayLark;
using GrayLark.bin.Debug.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrayLark
{
    public partial class AllReports : Form
    {
        String Code = "";
        public AllReports()
        {
            InitializeComponent();
            Disable();
            date1.Enabled = false; date2.Enabled = false;
            String query = "select ActTitle from Accounts where ID > 14 and Status=1 Order BY ID DESC";
            DataTable d = clsDataLayer.RetreiveQuery(query);
            if(d.Rows.Count > 0) {  clsGeneral.SetAutoCompleteTextBox(txtcustomer, d); } Enable();
        }

        private bool Enable()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Enabled == false)
                    {
                        ((TextBox)c).Enabled = true;
                        flag = true;

                    }
                }
                else if (c is ComboBox)
                { 
                        if (((ComboBox)c).Enabled == false)
                        {
                            ((ComboBox)c).Enabled = true;
                            flag = true;

                        }
                 }
                else if (c is MaskedTextBox)
                {
                     
                 if (((MaskedTextBox)c).Enabled == false)
                 {
                     ((MaskedTextBox)c).Enabled = true;
                     flag = true;

                 }
                     
                }
            }
            return flag;
        }
        private bool Disable()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Enabled == true)
                    {
                        ((TextBox)c).Enabled = false;
                        flag = true;

                    }
                }
                else if (c is ComboBox)
                {
                     
                        if (((ComboBox)c).Enabled == true)
                        {
                            ((ComboBox)c).Enabled = false;
                            flag = true;

                        }
                 }
            }
            return flag;
        }
        private bool Clears()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Text != "")
                    {
                        ((TextBox)c).Text = "";
                        flag = true;
                        break;
                    }
                }
                else if (c is ComboBox)
                {
                     
                        if (((ComboBox)c).Text != "")
                        {
                            ((ComboBox)c).Text = "";
                            flag = true;
                            break;
                        }
                    
                }
            }
            return flag;
        }
        private void PartyCode()
        {
            String sel = "select ActCode from Accounts where ActTitle = '" + txtcustomer.Text + "'";
            DataTable dc = clsDataLayer.RetreiveQuery(sel);
            if (dc.Rows.Count > 0)
            {
                Code = dc.Rows[0][0].ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Enable();
        }

        private void btnSave_Click(object sender, EventArgs e)
        { 
         PartyCode();
            if(txtreports.SelectedIndex == 0)
            {
                string q = "";
                q = "select * from LedgerReceived where Datetime between '" + date1.Text + "' and '" + date2.Text + "' and AccountCode='" + Code + "'  Order By ID ASC";
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
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else if (txtreports.SelectedIndex == 1)
            {
                string q = "";
                q = "select * from LedgerPayment where Datetime between '" + date1.Text + "' and '" + date2.Text + "' and AccountCode='" + Code + "'  Order By ID ASC";
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
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else if (txtreports.SelectedIndex == 2)
            {
                string q = "select * from CashIn where Dates between '" + date1.Text + "' and '" + date2.Text + "'";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    rptCashBook rpt = new rptCashBook();
                    rpt.SetDataSource(purchase);
                    LedgerView pop = new LedgerView(date1.Text, date2.Text, rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }

            }
            else if (txtreports.SelectedIndex == 3)
            {
                string q = "select * from AccountBalance";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    rptAccountBalance rpt = new rptAccountBalance();
                    rpt.SetDataSource(purchase);
                    frmAccountbalance pop = new frmAccountbalance(rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else if (txtreports.SelectedIndex == 4)
            {
                string q = "select * from Transactions WHERE Dates between '" + date1.Text + "' and '" + date2.Text + "'";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                 rptPaymentTr rpt = new rptPaymentTr();
                    rpt.SetDataSource(purchase);
                    PaymentView pop = new PaymentView(rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else if (txtreports.SelectedIndex == 5)
            {
                string q = "select * from Transactionss WHERE Dates between '" + date1.Text + "' and '" + date2.Text + "'";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    rptReceipt rpt = new rptReceipt();
                    rpt.SetDataSource(purchase);
                    PaymentView pop = new PaymentView(rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else if (txtreports.SelectedIndex == 6)
            {
                string q = "select * from Transactions WHERE Dates between '" + date1.Text + "' and '" + date2.Text + "' and PartyName='"+txtcustomer.Text+"'";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    rptPaymentTr rpt = new rptPaymentTr();
                    rpt.SetDataSource(purchase);
                    PaymentView pop = new PaymentView(rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else if (txtreports.SelectedIndex == 7)
            {
                string q = "select * from Transactionss WHERE Dates between '" + date1.Text + "' and '" + date2.Text + "' and PartyName = '" + txtcustomer.Text + "'";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    rptReceipt rpt = new rptReceipt();
                    rpt.SetDataSource(purchase);
                    PaymentView pop = new PaymentView(rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else if (txtreports.SelectedIndex == 8)
            {
                string q = "select * from CashTransaction where Dates between '" + date1.Text + "' and '" + date2.Text + "' and PartyName = '" + txtcustomer.Text + "' Order by CashIn asc";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    //
                    String CashRel = "";
                    string q5 = "select sum(Cash) from CashTransaction where Dates between '" + date1.Text + "' and '" + date2.Text + "' and Status='Paid' and PartyName = '" + txtcustomer.Text + "'";
                    DataTable purchase5 = clsDataLayer.RetreiveQuery(q5);
                    if (purchase5.Rows.Count > 0)
                    {
                        CashRel = purchase5.Rows[0][0].ToString();
                        if (CashRel.Equals("")) { CashRel = "0"; }
                    }
                    else { CashRel = "0"; }
                    String CashRel2 = "";
                    string q6 = "select sum(Cash) from CashTransaction where Dates between '" + date1.Text + "' and '" + date2.Text + "' and Status='Received' and PartyName = '" + txtcustomer.Text + "'";
                    DataTable purchase6 = clsDataLayer.RetreiveQuery(q6);
                    if (purchase6.Rows.Count > 0)
                    {
                        CashRel2 = purchase6.Rows[0][0].ToString();
                        if (CashRel2.Equals("")) { CashRel2 = "0"; }
                    }
                    else
                    {
                        CashRel2 = "0";
                    }
                    rptCashTransaction rpt = new rptCashTransaction();
                    rpt.SetDataSource(purchase);
                    CashTransView pop = new CashTransView(date1.Text, date2.Text, rpt, CashRel2, CashRel);
                    pop.Show();
                }
                else { MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            else if (txtreports.SelectedIndex == 9)
            {
                string q = "select * from BankTransaction where Dates between '" + date1.Text + "' and '" + date2.Text + "' and PartyName = '" + txtcustomer.Text + "' Order by BankIn asc";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    //rptBankInorOut rpt = new rptBankInorOut();
                    //rpt.SetDataSource(purchase);
                    //LedgerView pop = new LedgerView(date1.Text, date2.Text, rpt);
                    //pop.Show();
                    //
                    String CashRel = "";
                    string q5 = "select sum(BankCash) from BankTransaction where Dates between '" + date1.Text + "' and '" + date2.Text + "' and Status='Paid' and PartyName = '" + txtcustomer.Text + "'";

                    DataTable purchase5 = clsDataLayer.RetreiveQuery(q5);
                    if (purchase5.Rows.Count > 0)
                    {
                        CashRel = purchase5.Rows[0][0].ToString();
                        if (CashRel.Equals("")) { CashRel = "0"; }
                    }
                    else { CashRel = "0"; }
                    String CashRel2 = "";

                    string q6 = "select sum(BankCash) from BankTransaction where Dates between '" + date1.Text + "' and '" + date2.Text + "' and Status='Received' and PartyName = '" + txtcustomer.Text + "'";

                    DataTable purchase6 = clsDataLayer.RetreiveQuery(q6);
                    if (purchase6.Rows.Count > 0)
                    {
                        CashRel2 = purchase6.Rows[0][0].ToString();
                        if (CashRel2.Equals("")) { CashRel2 = "0"; }
                    }
                    else
                    {
                        CashRel2 = "0";
                    }
                    rptBankInorOut rpt = new rptBankInorOut();
                    rpt.SetDataSource(purchase);
                    CashTransView pop = new CashTransView(date1.Text, date2.Text, rpt, CashRel2, CashRel);
                    pop.Show();
                    //
                }
                else
                {
                    MessageBox.Show("No Record Found!");
                }
            }
            else if (txtreports.SelectedIndex == 10)
            {
                string q = "select * from Transactions where Status = 'UNPOST' Order by TransId asc";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    PostedDateCheque rpt = new PostedDateCheque();
                    rpt.SetDataSource(purchase);
                    PaymentView pop = new PaymentView(rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else if (txtreports.SelectedIndex == 11)
            {
                string q = "select * from Transactions where Dates between '" + date1.Text + "' and '" + date2.Text + "' and VoucherStatus='" + cmbstatus.Text + "' and PartyName='" + txtcustomer.Text + "'";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    rptPaymentTr rpt = new rptPaymentTr();
                    rpt.SetDataSource(purchase);
                    PaymentView pop = new PaymentView(rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else if (txtreports.SelectedIndex == 12)
            {
                string q = "select * from Transactionss where Dates between '" + date1.Text + "' and '" + date2.Text + "' and VoucherStatus='" + cmbstatus.Text + "' and PartyName='" + txtcustomer.Text + "'";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    rptReceipt rpt = new rptReceipt();
                    rpt.SetDataSource(purchase);
                    PaymentView pop = new PaymentView(rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            } 
            else if (txtreports.SelectedIndex == 13)
            {
                string q = "select * from Transactions where Dates between '" + date1.Text + "' and '" + date2.Text + "' and VoucherStatus!='" + cmbstatus.Text + "'";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    rptPaymentTr rpt = new rptPaymentTr();
                    rpt.SetDataSource(purchase);
                    PaymentView pop = new PaymentView(rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else if (txtreports.SelectedIndex == 14)
            {
                BalanceSheet();
            }
            else if (txtreports.SelectedIndex == 15)
            {
                string q = "select * from Purchase where Date between '" + date1.Text + "' and '" + date2.Text + "'";
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
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else if (txtreports.SelectedIndex == 16)
            {
                string q = "select * from Purchase where Date between '" + date1.Text + "' and '" + date2.Text + "' and VendorName='" + txtcustomer.Text + "'";
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
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
               
        }

        private void BalanceSheet()
        {
            try
            {
                decimal cash = 0, ReceiveDue = 0, PaymentDue = 0, Inventory = 0, Salary = 0, Capital = 0;
                String q1 = "select CASH from tech_cash"; DataTable d1 = clsDataLayer.RetreiveQuery(q1); if (d1.Rows.Count > 0)
                {
                    String rm = d1.Rows[0][0].ToString();
                    if (!rm.Equals(""))
                    {
                        cash = Convert.ToDecimal(d1.Rows[0][0].ToString());
                    }
                }
                String q2 = "select sum(DueAmount) from ReceiveDue"; DataTable d2 = clsDataLayer.RetreiveQuery(q2); if (d2.Rows.Count > 0)
                {
                    String rm = d2.Rows[0][0].ToString();
                    if (!rm.Equals(""))
                    {
                        ReceiveDue = Convert.ToDecimal(d2.Rows[0][0].ToString());
                    }
                }
                String q3 = "select sum(DueAmount) from PaymentDue"; DataTable d3 = clsDataLayer.RetreiveQuery(q3); if (d3.Rows.Count > 0)
                {
                    String rm = d3.Rows[0][0].ToString();
                    if (!rm.Equals(""))
                    {
                        PaymentDue = Convert.ToDecimal(d3.Rows[0][0].ToString());
                    }
                }
                String q5 = "select sum(Purchase_Price) from tbl_AvailableProduct where Quantity!=0"; DataTable d5 = clsDataLayer.RetreiveQuery(q5); if (d5.Rows.Count > 0)
                {
                    String rm = d5.Rows[0][0].ToString();
                    if (!rm.Equals(""))
                    {
                        Inventory = Convert.ToDecimal(d5.Rows[0][0].ToString());
                    }
                }
                String q6 = "select sum(CashGiven) from Transactions where PartyName='Salary' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date1.Value.ToString("yyyy-MM-dd") + "'"; DataTable d6 = clsDataLayer.RetreiveQuery(q6); if (d6.Rows.Count > 0)
                {
                    String rm = d6.Rows[0][0].ToString();
                    if (!rm.Equals(""))
                    {
                        Salary = Convert.ToDecimal(d6.Rows[0][0].ToString());
                    }
                }
                String q7 = "select sum(CashReceived) from Transactionss where PartyName='Capital' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date1.Value.ToString("yyyy-MM-dd") + "'"; DataTable d7 = clsDataLayer.RetreiveQuery(q7); if (d7.Rows.Count > 0)
                {
                    String rm = d7.Rows[0][0].ToString();
                    if (!rm.Equals(""))
                    {
                        Capital = Convert.ToDecimal(d7.Rows[0][0].ToString());
                    }
                }
                decimal Drawing = 0;
                String q8 = "select sum(CashGiven) from Transactions where PartyName='Drawing' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date1.Value.ToString("yyyy-MM-dd") + "'"; DataTable d8 = clsDataLayer.RetreiveQuery(q8); if (d8.Rows.Count > 0)
                {
                    String rm1 = d8.Rows[0][0].ToString();
                    if (!rm1.Equals(""))
                    {
                        Drawing = Convert.ToDecimal(d8.Rows[0][0].ToString());
                    }
                }

                BalanceSheet bs = new BalanceSheet();
                BalanceSheetViewer bv = new BalanceSheetViewer(bs, cash, ReceiveDue, PaymentDue, Inventory, Salary, Capital, Drawing); bv.Show();
            }
            catch { }
        }

        private void SProfit()
        {
            decimal sbill = 0; decimal npur = 0;  
        String q1 = "select sum(BillAmount) from tracking_order where DeliveryStatus = 'Delivered' and Date between '"+date1.Text+"' and '"+date2.Text+"' "; DataTable d1 = clsDataLayer.RetreiveQuery(q1);
        if (d1.Rows.Count > 0)
        {
            sbill = Convert.ToDecimal(d1.Rows[0][0].ToString());
        }
        else { sbill = 0; }

        String q2 = " select sum(NetAmount) from VU_Purchase where Date between '" + date1.Text + "' and '" + date2.Text + "' "; DataTable d2 = clsDataLayer.RetreiveQuery(q2);
        if (d2.Rows.Count > 0)
        {
            npur = Convert.ToDecimal(d2.Rows[0][0].ToString());
        }
        else { npur = 0; }

        decimal fadd = 0; decimal Exp = 0;
        String hq = "select sum(CashGiven) from Transactions where PartyName = 'FaceBook' and Dates between '" + date1.Text + "' and '" + date2.Text + "' "; DataTable dv = clsDataLayer.RetreiveQuery(hq); if (dv.Rows.Count > 0)
        {
            String hk = dv.Rows[0][0].ToString();
            if (hk.Equals(""))
            {
                fadd = 0;
            }
            else
            {
                fadd = Convert.ToDecimal(dv.Rows[0][0].ToString());
            }
        }
        else { fadd = 0; }

        String hq1 = "select sum(Cp_FreightCharges) from tbl_CPayment where Date between '" + date1.Text + "' and '" + date2.Text + "' "; DataTable dv1 = clsDataLayer.RetreiveQuery(hq1); if (dv1.Rows.Count > 0)
        {
            String hk = dv1.Rows[0][0].ToString();
            if (hk.Equals(""))
            {
                Exp = 0;
            }
            else
            {
                Exp = Convert.ToDecimal(dv1.Rows[0][0].ToString());
            }
        }
        else { Exp = 0; }



        }

        private void Profit()
        {
            try
            {
                DataTable dw = new DataTable();
                dw.Columns.Add("Quantity");
                dw.Columns.Add("ProductName");
                dw.Columns.Add("Purchase_Price");
                dw.Columns.Add("Sale_Rate");
                dw.Columns.Add("PurVal");
                dw.Columns.Add("SalVal");
                dw.Columns.Add("PNL"); dw.Columns.Add("Discount");
                 
                decimal squant = 0;
                decimal snetamount = 0;
                decimal spp = 0; decimal tdis = 0;
                decimal fpl = 0; decimal dis = 0;
                decimal pprice = 0; decimal tprice = 0; decimal ppl = 0;
                String selpd = "select NAME from add_product";
                DataTable d1 = clsDataLayer.RetreiveQuery(selpd);
                if (d1.Rows.Count > 0)
                {
                    for (int a = 0; a < d1.Rows.Count; a++)
                    {
                        String pname = d1.Rows[a][0].ToString();
                        //
                          String sm = "select PricePerItem from vsale where Date between '" + date1.Text + "' and '" + date2.Text + "' and SaleItem='" + pname + "'";
                          DataTable d20 = clsDataLayer.RetreiveQuery(sm); if (d20.Rows.Count > 0)
                          {
                              spp = Convert.ToDecimal(d20.Rows[0][0].ToString());
                          }
                          else { spp = 0; }
                        //
                        String sels = "select sum(Quantity),sum(PricePerItem),sum(TotalPriceItem),sum(Discount) from vsale where Date between '" + date1.Text + "' and '" + date2.Text + "' and SaleItem='" + pname + "'";
                        DataTable d2 = clsDataLayer.RetreiveQuery(sels); if (d2.Rows.Count > 0)
                        {
                            String pc1 = d2.Rows[0][0].ToString();
                            if (!pc1.Equals(""))
                            {
                                 squant = Convert.ToDecimal(d2.Rows[0][0].ToString()); snetamount = Convert.ToDecimal(d2.Rows[0][2].ToString());
                            }
                            else { squant = 0; snetamount = 0;  }
                             
                            String er = "select sum(Discount) from SaleBill where Date between '" + date1.Text + "' and '" + date2.Text + "'"; DataTable df = clsDataLayer.RetreiveQuery(er);
                            if (df.Rows.Count > 0) { dis = Convert.ToDecimal(df.Rows[0][0].ToString()); } else { dis = 0; }
                             String s3 = "select sum(PricePerItem),sum(TotalPriceItem) from VU_Purchase where Date between '" + date1.Text + "' and '" + date2.Text + "' and PurchaseItem='" + pname + "'";
                            DataTable d3 = clsDataLayer.RetreiveQuery(s3);
                            if (d3.Rows.Count > 0)
                            {
                            String pc = d3.Rows[0][0].ToString();
                            if (!pc.Equals(""))
                            {
                                pprice = Convert.ToDecimal(d3.Rows[0][0].ToString()); tprice = Convert.ToDecimal(d3.Rows[0][1].ToString());
                            }
                            else { pprice = 0; tprice = 0; }
                            }
                        } 
                        ppl = snetamount - tprice;
                        fpl += ppl;
                        String PfLf = ppl.ToString();
                        dw.Rows.Add(squant, pname, tprice, snetamount, pprice, spp, PfLf,dis);
                    }

                    //String Ex = "";
                    //String set = "select sum(CashGiven) from Transactions where PartyName = 'Office Expense' and Dates between '" + date1.Text + "' and '" + date2.Text + "'";
                    //DataTable dv = clsDataLayer.RetreiveQuery(set);
                    //if (dv.Rows.Count > 0) {
                    //    Ex = dv.Rows[0][0].ToString();
                    //}
                    //else { Ex = "0"; }
                    decimal fadd = 0; decimal Exp = 0;
                    String hq = "select sum(CashGiven) from Transactions where PartyName = 'FaceBook' and Dates between '" + date1.Text + "' and '" + date2.Text + "' "; DataTable dv = clsDataLayer.RetreiveQuery(hq); if (dv.Rows.Count > 0)
                    {
                        String hk = dv.Rows[0][0].ToString();
                        if (hk.Equals(""))
                        {
                            fadd = 0;
                        }
                        else
                        {
                            fadd = Convert.ToDecimal(dv.Rows[0][0].ToString());
                        }
                    }
                    else { fadd = 0; }

                    String hq1 = "select sum(Cp_FreightCharges) from tbl_CPayment where Date between '" + date1.Text + "' and '" + date2.Text + "' "; DataTable dv1 = clsDataLayer.RetreiveQuery(hq1); if (dv1.Rows.Count > 0)
                    {
                        String hk = dv1.Rows[0][0].ToString();
                        if (hk.Equals(""))
                        {
                            Exp = 0;
                        }
                        else
                        {
                            Exp = Convert.ToDecimal(dv1.Rows[0][0].ToString());
                        }
                    }
                    else { Exp = 0; }

                    //ProfitLoss rpt = new ProfitLoss();
                    //rpt.SetDataSource(dw);
                    //Form3 fr = new Form3(rpt, date1.Value.ToString("yyyy-MM-dd").ToString(), date2.Value.ToString("yyyy-MM-dd"), fpl.ToString(),dis.ToString(),Exp.ToString(),fadd.ToString());
                    //fr.Show();
                }
            }
            catch { }
        }

        private void txtreports_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(txtreports.SelectedIndex == 0)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                cmbstatus.Enabled = false;
            }
            else if (txtreports.SelectedIndex == 1)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                cmbstatus.Enabled = false;
            }
            else if (txtreports.SelectedIndex == 2)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                txtcustomer.Enabled = false;
                cmbstatus.Enabled = false;
            }
            else if (txtreports.SelectedIndex == 3)
            {
                Disable();
                txtreports.Enabled = true;
                date1.Enabled = false;
                date2.Enabled = false;
                cmbstatus.Enabled = false;
            }
            else if (txtreports.SelectedIndex == 4)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                txtcustomer.Enabled = false;
                cmbstatus.Enabled = false;
            }
            else if (txtreports.SelectedIndex == 5)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                txtcustomer.Enabled = false;
                cmbstatus.Enabled = false;
            }
            else if (txtreports.SelectedIndex == 6)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                txtcustomer.Enabled = true;
                cmbstatus.Enabled = false;
            }
            else if (txtreports.SelectedIndex == 7)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                txtcustomer.Enabled = true;
                cmbstatus.Enabled = false;
            }
            else if (txtreports.SelectedIndex == 8)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                cmbstatus.Enabled = true;
            }
            else if (txtreports.SelectedIndex == 9)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                cmbstatus.Enabled = true;
            }
            else if (txtreports.SelectedIndex ==10)
            {
                Enable();
                date1.Enabled = false;
                date2.Enabled = false;
                txtcustomer.Enabled = false;

                cmbstatus.Enabled = false;
            }
            else if (txtreports.SelectedIndex == 11)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                txtcustomer.Enabled = true;
                cmbstatus.Enabled = true;
            }
            else if (txtreports.SelectedIndex == 12)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                txtcustomer.Enabled = true;
                cmbstatus.Enabled = true;
            } 
            else if (txtreports.SelectedIndex == 13)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                txtcustomer.Enabled = false;
                cmbstatus.Enabled = true;
            }
            else if (txtreports.SelectedIndex == 14)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                txtcustomer.Enabled = false;
                cmbstatus.Enabled = false;
            }
            else if (txtreports.SelectedIndex == 15)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                txtcustomer.Enabled = false;
                cmbstatus.Enabled = false;
            }
            else if (txtreports.SelectedIndex == 16)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                txtcustomer.Enabled = true;
                cmbstatus.Enabled = false;
            }
            else if (txtreports.SelectedIndex == 17)
            {
                Enable();
                date1.Enabled = true;
                date2.Enabled = true;
                txtcustomer.Enabled = true;
                cmbstatus.Enabled = false;
            } 
        }
         


    }
}
