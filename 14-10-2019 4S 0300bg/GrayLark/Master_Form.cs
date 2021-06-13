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
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Diagnostics;
using GrayLark.bin.Debug.Report;
using System.IO;

namespace GrayLark
{
    public partial class Master_Form : Form
    {
        String serverName = "";
        String userName = "";
        String path = ""; DataSet fds = new DataSet();
        String password = "";

        public Master_Form() {  InitializeComponent();

        menuStrip1.Renderer = new MyRenderer();
        clm(); ProductExpiry pe = new ProductExpiry(); pe.Show();
        }

       
        public void clm()
        {
            try
            { 
                    String f2 = "select CurrentCash from CashTransaction Order by CashIn desc"; DataTable df2 = clsDataLayer.RetreiveQuery(f2);
                    if (df2.Rows.Count > 0)
                    {
                        var t = Convert.ToDecimal(df2.Rows[0][0].ToString()).ToString("#,##0.00");
                        txtopen.Text = t.ToString();
                    }
                    else { txtopen.Text = "0"; }
                //}
                //else
                //{
                //    txtopen.Text = "";
                //}

                String Open1 = "select CASH from tech_cash where CompanyName='Delizia'"; DataTable d11 = clsDataLayer.RetreiveQuery(Open1);
                if (d11.Rows.Count > 0)
                {
                    var t = Convert.ToDecimal(d11.Rows[0][0].ToString()).ToString("#,##0.00");
                    txtclose.Text = t.ToString();

                    decimal m981 = Convert.ToDecimal(d11.Rows[0][0].ToString());
                    var rt = Convert.ToDecimal(d11.Rows[0][0].ToString()).ToString("#,##0.00");
                    txtclose.Text = rt.ToString();
                 }
                else
                {
                    txtclose.Text = "";
                }

                String o2 = "select sum(CashGiven) from Transactions where HeadActCode='5' and Dates='" + DateTime.Now.ToString("yyyy-MM-dd") + "'"; DataTable d9 = clsDataLayer.RetreiveQuery(o2);
                if (d9.Rows.Count > 0)
                {
                    decimal m99 = 0;
                    String h90 = d9.Rows[0][0].ToString();
                    if (h90.Equals(""))
                    {
                        m99 = 0; txtexpense.Text = m99.ToString();
                    }
                    else
                    {
                        m99 = Convert.ToDecimal(d9.Rows[0][0].ToString());
                        var t = Convert.ToDecimal(d9.Rows[0][0].ToString()).ToString("#,##0.00");
                        txtexpense.Text = t.ToString();
                    }
                }
                else
                {
                    txtexpense.Text = "";
                }
            }
            catch { }
        }

        private class MyRenderer : ToolStripProfessionalRenderer
        {
            public MyRenderer() : base(new MyColors()) { }
        }

        private class MyColors : ProfessionalColorTable
        {
            public override Color MenuItemSelected
            {
                get { return Color.AliceBlue; }
            }
            public override Color MenuItemSelectedGradientBegin
            {
                get { return Color.AliceBlue; }
            }
            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.AliceBlue; }
            }
        }

        private Timer _Timer = new Timer();
        String UID = Login.UserID;
        public string GreenSignal = "";
        public string FormID = "";

        public void UserNametesting()
        {
            string query = "SELECT FORM_ID,ACCESS FROM user_access WHERE USENAME = '" + UID + "' AND FORM_ID = '" + FormID + "'";
            DataTable drs = clsDataLayer.RetreiveQuery(query); 
            if (drs.Rows.Count>0)
            {
                String frmid = drs.Rows[0]["FORM_ID"].ToString();
                String Acs = drs.Rows[0]["ACCESS"].ToString();
                if( Acs.Equals("Yes"))
                {
                    GreenSignal = "YES";
                 }
                else
                {
                    GreenSignal = "NO";
                }
            }
            else
            {
                GreenSignal = "NO";
            }
        }
         
        private void Master_Form_Load(object sender, EventArgs e)
        {
            _Timer.Interval = 1000;
            _Timer.Tick += new EventHandler(timer1_Tick);
            _Timer.Start();
            labelUser.Text = UID;
            DateTime dt = DateTime.Now;
            labelDate.Text = dt.ToString("D");
        }

        int yu = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTime.Text = DateTime.Now.ToString("T"); clm();
            //if (yu == 0)
            //{ 
            //        System.Windows.Forms.Form f1 = System.Windows.Forms.Application.OpenForms["ProductExpiry"];
            //        if (((ProductExpiry)f1) == null)
            //        {
            //            ProductExpiry frmdel = new ProductExpiry();
            //            frmdel.Show();
            //        }
                
            //}
            //yu++; 

        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {           
        DialogResult Result = MessageBox.Show("Mr. " + UID + "! Do you want to logout?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        if (Result == DialogResult.Yes)
        {
            //run the program again and close this one
            Process.Start(Application.StartupPath + "\\GrayLark.exe");
            //or you can use Application.ExecutablePath

            //close this one
            Process.GetCurrentProcess().Kill();

        }else if (Result == DialogResult.No)
        {
            MessageBox.Show(" Mr/Ms" + UID + " continue your work!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
         }

        private void addProductTypeAndCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {            
        Add_Product_Type_And_Category a = new Add_Product_Type_And_Category();
        FormID = "1007"; UserNametesting();
        if (GreenSignal == "YES") { a.Show(); }
        else {  MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);    }
        }
         
        private void addProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add_Product ap = new Add_Product();
            FormID = "1005";
            UserNametesting();
            if (GreenSignal == "YES")
            { 
             ap.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

       private void saleDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeliveryChallan ps = new DeliveryChallan();
            FormID = "1011";
            UserNametesting();
            if (GreenSignal == "YES")
            { 
                ps.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void purchaseOrdeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PurchaseOrders po = new PurchaseOrders();
            FormID = "1006";
            UserNametesting();
            if (GreenSignal == "YES")
            { 
                po.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        } 

        private void returnOrCancelOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Return_Order ro = new Return_Order();
            FormID = "1026";
            UserNametesting();
            if (GreenSignal == "YES")
            {

                ro.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void saleReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sale_Report sr = new Sale_Report();
            FormID = "1027";
            UserNametesting();
            if (GreenSignal == "YES")
            { 
            sr.Show();
            }
            else
            {
                MessageBox.Show("You do not have any rights to perform this activity. Contact Your Administrator.", "Warning..", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        } 
        private void userAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User_Access ua = new User_Access();
            FormID = "1001";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                ua.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Change_Password cp = new Change_Password();
            cp.Show();
        }

        private void viewProductStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View_Product_Stock vps = new View_Product_Stock();
            FormID = "1029";
            UserNametesting();
            if (GreenSignal == "YES")
            {

                vps.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void buyProductReportCompanyWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Report_Company_Wise rcw = new Report_Company_Wise();
            //FormID = "1030";
            //UserNametesting();
            //if (GreenSignal == "YES")
            //{

            //    rcw.Show();
            //}
            //else
            //{
            //    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //}
        }

        private void returnReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Return_Sale rs = new Return_Sale();
            FormID = "1031";
            UserNametesting();
            if (GreenSignal == "YES")
            {
             rs.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
        }


        private void userAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add_User au = new Add_User();
            FormID = "1032";
            UserNametesting();
            if (GreenSignal == "YES")
            {

                au.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
        } 
     
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //test t = new test();
            //FormID = "1022";
            //UserNametesting();
            //if (GreenSignal == "YES")
            //{

            //    t.Show();
            //}
            //else
            //{
            //    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //} 
        } 

        private void replaceReturnProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
        Return_Order ror = new Return_Order();
        FormID = "1034";
        UserNametesting();
        if (GreenSignal == "YES")
        {
        ror.Show();
        }
        else
        {
        MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        } 
        }

        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Master_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void labelTime_Click(object sender, EventArgs e)
        {
      
        }

        private void customerDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View_All_Sale ror = new View_All_Sale("Replace");
            FormID = "1035";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                ror.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
        }
         

        private void returnOrderDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View_All_Return_Orders ror = new View_All_Return_Orders();
            FormID = "1036";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                ror.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
        }

        private void addBankDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddBankDetail ror = new AddBankDetail();
            FormID = "1016";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                ror.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
        }

        private void transactionOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Payment_voucher frm = new Payment_voucher();
            FormID = "1012";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                frm.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
        } 
        private void replaceProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplaceOrder frm = new ReplaceOrder();
            FormID = "1038";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                frm.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
        }
         
        private void chartOfAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();
            frmChartofAccount frm = new frmChartofAccount();
            FormID = "1039";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                frm.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
        }

        private void customerInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();

            frmParty frm = new frmParty();
            FormID = "1040";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                frm.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
        } 
        private void cashBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();

            string q = "select * from CashIn";
            DataTable purchase = clsDataLayer.RetreiveQuery(q);
            if (purchase.Rows.Count > 0)
            {
                rptCashBook rpt = new rptCashBook();
                rpt.SetDataSource(purchase);
                LedgerView pop = new LedgerView("", "", rpt);
                pop.Show();
            }
            else
            {
                MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void remainingReceiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();

            FormID = "1056";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                string q = "select * from ReceiveDue where DueAmount!=0 and PartyCode!=528";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    RemainingDue rpt = new RemainingDue();
                    rpt.SetDataSource(purchase);
                    PaymentView pop = new PaymentView(rpt);
                    pop.Show();
                }
                else
                {
                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("You do not have any rights to perform this activity. Contact Your Administrator.", "Warning..", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void remainingPaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();

            FormID = "1055";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                string q = "select * from PaymentDue where DueAmount!=0 and PartyCode!=528";
                DataTable purchase = clsDataLayer.RetreiveQuery(q);
                if (purchase.Rows.Count > 0)
                {
                    RemainingPaymentDue rpt = new RemainingPaymentDue();
                    rpt.SetDataSource(purchase);
                    PaymentView pop = new PaymentView(rpt);
                    pop.Show();
                }
                else
                {
                                        MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("You do not have any rights to perform this activity. Contact Your Administrator.", "Warning..", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
        }

        private void ledgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();

            //LedgerPayment
            string q = "select * from LedgerReceived";
            DataTable purchase = clsDataLayer.RetreiveQuery(q);
            if (purchase.Rows.Count > 0)
            {
                rptLedger rpt = new rptLedger();
                rpt.SetDataSource(purchase);
                LedgerView pop = new LedgerView("01-01-2017", "01-01-2099", rpt);
                pop.Show();
            }
            else
            {
                                    MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void allReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();

            AllReports pop = new AllReports();
            FormID = "1035";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                pop.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
        }

        private void receiptVoucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();

            Receipt_voucher frm = new Receipt_voucher();
            FormID = "1040";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                frm.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
        }

        private void invoicesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            clm();

            //InvoiceBill frm = new InvoiceBill();
            //FormID = "1054";
            //UserNametesting();
            //if (GreenSignal == "YES")
            //{
            //    frm.Show();
            //}
            //else
            //{
            //    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //} 
            //FormID = "1041";
            //UserNametesting();
      
            //if (GreenSignal == "YES")
            //{
            //    frm.Show();
            //}
            //else
            //{
            //    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //} 
        }
          
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            clm(); 
            View_All_Payment frm = new View_All_Payment();
            FormID = "1064";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                frm.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
        }

        private void allReceiptDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();
            View_All_Receipt frm = new View_All_Receipt();
            FormID = "1065";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                frm.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
        }
         
        private void transferVoucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();

            TrasnferForm frm = new TrasnferForm();
            FormID = "1053";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                frm.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void cashReleaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                clm();

                FormID = "1052";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    string q = "select * from CashRelease";
                    DataTable purchase = clsDataLayer.RetreiveQuery(q);
                    if (purchase.Rows.Count > 0)
                    {
                        rptCashRelease rpt = new rptCashRelease();
                        rpt.SetDataSource(purchase);
                        LedgerView pop = new LedgerView("", "", rpt);
                        pop.Show();
                    }
                    else
                    {
                                            MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("You do not have any rights to perform this activity. Contact Your Administrator.", "Warning..", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                } 
            }
            catch { }
        }

        private void bankCashInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                clm();

                FormID = "1051";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    string q = "select * from BankCashIn";
                    DataTable purchase = clsDataLayer.RetreiveQuery(q);
                    if (purchase.Rows.Count > 0)
                    {
                        rptBankCashIn rpt = new rptBankCashIn();
                        rpt.SetDataSource(purchase);
                        LedgerView pop = new LedgerView("", "", rpt);
                        pop.Show();
                    }
                    else
                    {
                                            MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("You do not have any rights to perform this activity. Contact Your Administrator.", "Warning..", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                } 
            }
            catch
            {}
        }

        private void bankCashReleaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                clm();

                FormID = "1050";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    string q = "select * from BankCashRelease";
                    DataTable purchase = clsDataLayer.RetreiveQuery(q);
                    if (purchase.Rows.Count > 0)
                    {
                        rptBankCashRelease rpt = new rptBankCashRelease();
                        rpt.SetDataSource(purchase);
                        LedgerView pop = new LedgerView("", "", rpt);
                        pop.Show();
                    }
                    else
                    {
                                            MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
                else
                {MessageBox.Show("You do not have any rights to perform this activity. Contact Your Administrator.", "Warning..", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                } 
            }catch{ }
        }

        private void payableAndReceiveableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();

            PayableandReceiveable sr = new PayableandReceiveable();
            FormID = "1047";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                sr.Show();
            }
            else
            {
                MessageBox.Show("You do not have any rights to perform this activity. Contact Your Administrator.", "Warning..", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        } 

        private void saleReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            clm();

            Sale_Report sr = new Sale_Report();
            FormID = "1027";
            UserNametesting();
            if (GreenSignal == "YES")
            {
             sr.Show();
            }
            else
            {
                MessageBox.Show("You do not have any rights to perform this activity. Contact Your Administrator.", "Warning..", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void returnReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            clm();

            Return_Sale rs = new Return_Sale();
            FormID = "1031";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                rs.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
        }

        private void manualReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();

            ManualReturn rs = new ManualReturn();
            FormID = "1048";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                rs.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void reminderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();

            //Reminders rs = new Reminders();
            //FormID = "1049";
            //UserNametesting();
            //if (GreenSignal == "YES")
            //{
            //    rs.Show();
            //}
            //else
            //{
            //    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //}
        }

        private void bankSearchingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();

            BankSearching rs = new BankSearching();
            FormID = "1044";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                rs.Show();
            }
            else
            {
                MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void labelDate_Click(object sender, EventArgs e)
        {   } 
            
        private void searchingTransferDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();

            System.Windows.Forms.Form f1 = System.Windows.Forms.Application.OpenForms["SearchingTransfer"];
            if (((SearchingTransfer)f1) == null)
            {
                SearchingTransfer ua = new SearchingTransfer();
                ua.Show();
            }
        }

        private void suspenseVoucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clm();
            //SuspenseTransfer st = new SuspenseTransfer();
            //st.Show();
        }
         
        private void cashTransactionUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();
            CashTransactionUpdate();
            MessageBox.Show("Cash Transaction Update!");
        }
         
        private void CashTransactionUpdate()
        {
            clm();
            decimal add, less, balance = 0;
            decimal oldbalance = 0;
            String pled = "select CashCode,Cash,CurrentCash,Status from CashTransaction";
            DataTable d2 = clsDataLayer.RetreiveQuery(pled);
            if (d2.Rows.Count > 0)
            {
                for (int c = 0; c < d2.Rows.Count; c++)
                {
                    String rno = d2.Rows[c][0].ToString();
                    String Status = d2.Rows[c][3].ToString();

                    balance = Convert.ToDecimal(d2.Rows[c][2].ToString());


                    if (Status.Equals("Paid"))
                    {
                        less = Convert.ToDecimal(d2.Rows[c][1].ToString());
                        balance = oldbalance - less; oldbalance = balance;

                    }

                    else if (Status.Equals("Received"))
                    {
                        add = Convert.ToDecimal(d2.Rows[c][1].ToString());
                        balance = oldbalance + add; oldbalance = balance;

                    }
                    String upd = "update CashTransaction set CurrentCash=" + balance + " where CashCode='" + rno + "' "; clsDataLayer.ExecuteQuery(upd);

                }


            }

        }

        private void BankTransactionUpdate()
        {
            clm();
            decimal add, less, balance = 0;
            decimal oldbalance = 0; String state = "start";
            String selpd = "select AccountNo from AccountBalance";
            DataTable d1 = clsDataLayer.RetreiveQuery(selpd);
            if (d1.Rows.Count > 0)
            {
                for (int b = 0; b < d1.Rows.Count; b++)
                {
                    String pname = d1.Rows[b][0].ToString();
                    String pled = "select BankCode,BankCash,CurrentCash,Status from BankTransaction where AccountNo='" + pname + "'";
                    DataTable d2 = clsDataLayer.RetreiveQuery(pled);
                    if (d2.Rows.Count > 0)
                    {
                        for (int c = 0; c < d2.Rows.Count; c++)
                        {
                            String rno = d2.Rows[c][0].ToString();
                            String Status = d2.Rows[c][3].ToString();

                            balance = Convert.ToDecimal(d2.Rows[c][2].ToString());

                            if (c > 0)
                            {
                            if (Status.Equals("Paid"))
                            {
                                less = Convert.ToDecimal(d2.Rows[c][1].ToString());
                                balance = oldbalance - less; oldbalance = balance;
                            }
                            else if (Status.Equals("Received"))
                            {
                                add = Convert.ToDecimal(d2.Rows[c][1].ToString());
                                balance = oldbalance + add; oldbalance = balance;
                            }
                            }
                            else
                            {
                                oldbalance = balance;
                            }
                            String upd = "update BankTransaction set CurrentCash=" + balance + " where BankCode='" + rno + "' "; clsDataLayer.ExecuteQuery(upd);

                        }


                    }
                }
            }

        }

        private void bankTransactionUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();
            BankTransactionUpdate();
            MessageBox.Show("Bank Transaction Update!");
        }

        private void serverconnect()
        { 
            String re = "select ServerName,UserId,Passwords,Path from tbl_Connection"; DataTable d1 = clsDataLayer.RetreiveQuery(re); if (d1.Rows.Count > 0)
            {
                serverName = d1.Rows[0][0].ToString();
                userName = d1.Rows[0][1].ToString();
                password = d1.Rows[0][2].ToString();
                path = d1.Rows[0][3].ToString();
                 string str = "Data Source=" + serverName + ";User ID=" + userName + ";Password=" + password + "";
               // string str = "Data Source=" + serverName + ";Integrated Security=True";

                SqlConnection con = new SqlConnection(str);
                try
                {
                    con.Open();
                    // MessageBox.Show("connection gets established");
                    SqlCommand cmd = new SqlCommand("SELECT  db.[name] as dbname FROM [master].[sys].[databases] db", con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                    sda.Fill(fds, "DatabaseName");
                    con.Close();
                    // MessageBox.Show("Server Connected!");
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }
         

        private void pROFITLOSSToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
         
        private void remainingCreditCardDuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void searchCreditCardInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void balanceSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {     
        
        }
          
        private void Master_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); Environment.Exit(0); Process.GetCurrentProcess().Kill();
        }

        private void balanceSheetToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            clm();
            System.Windows.Forms.Form f1 = System.Windows.Forms.Application.OpenForms["PettyReportView"];
                if (((PettyReportView)f1) == null)
                {
                    PettyReportView ua = new PettyReportView();
                    FormID = "1062";
                    UserNametesting();
                    if (GreenSignal == "YES")
                    {
                        ua.Show();
                    }
                    else
                    {
                        MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
        }
         
        private void Master_Form_KeyPress(object sender, KeyPressEventArgs e)
        {
        if (e.KeyChar == (char)Keys.Escape)
        {
            this.Close();
        }
        }

        private void Master_Form_KeyDown(object sender, KeyEventArgs e)
        {
        if (e.KeyCode == Keys.F && e.KeyCode == Keys.C)
        {
        //Show the form
        MessageBox.Show("Enter D key pressed");
        }
        }

      

        private void outletClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
         try
        {
            //clm(); 
            //System.Windows.Forms.Form f1 = System.Windows.Forms.Application.OpenForms["OutletClient"];
            //if (((OutletClient)f1) == null)
            //{ 
            //    //
            //    OutletClient ua = new OutletClient();
            //    FormID = "1060";
            //    UserNametesting();
            //    if (GreenSignal == "YES")
            //    {
            //        ua.Show();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    } 
            //}
        }
        catch { }
        }

        private void serverConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                clm();

                System.Windows.Forms.Form f1 = System.Windows.Forms.Application.OpenForms["ConnectServer"];
                if (((ConnectServer)f1) == null)
                {
                    ConnectServer ua = new ConnectServer();
                    ua.Show();
                }
            }
            catch { }
        }

        private void backupDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
        try
        {
            try
            { 
                serverconnect();
                String DbName = "4S"; String DestPath = "path";
                if (DestPath == "" || DbName == "")
                {
                    MessageBox.Show("Try to select Database and Destination Folder !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    string databaseName = DbName;//dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue.ToString();
                    databaseName = "4S";
                    DbName = "4S";
                    //Define a Backup object variable.
                    Backup sqlBackup = new Backup();

                    ////Specify the type of backup, the description, the name, and the database to be backed up.
                    sqlBackup.Action = BackupActionType.Database;
                    sqlBackup.BackupSetDescription = "BackUp of:" + databaseName + "on" + DateTime.Now.ToShortDateString();
                    sqlBackup.BackupSetName = "FullBackUp";
                    sqlBackup.Database = databaseName;

                    ////Declare a BackupDeviceItem
                    string destinationPath = DestPath;


                    string backupfileName = DbName + DateTime.Now.ToString("dd-MM-yyyy hh.MM.ss") + ".bak";
                    var y = @"" + path;
                    BackupDeviceItem deviceItem = new BackupDeviceItem(y + "\\" + backupfileName, DeviceType.File); 
                    ServerConnection connection = new ServerConnection(serverName);

                    ////To Avoid TimeOut Exception
                    Server sqlServer = new Server(connection);
                    sqlServer.ConnectionContext.StatementTimeout = 60 * 60;

                    Database db = sqlServer.Databases[databaseName];

                    sqlBackup.Initialize = true;
                    sqlBackup.Checksum = true;
                    sqlBackup.ContinueAfterError = true;

                    ////Add the device to the Backup object.
                    sqlBackup.Devices.Add(deviceItem);
                    ////Set the Incremental property to False to specify that this is a full database backup.
                    sqlBackup.Incremental = false;

                    sqlBackup.ExpirationDate = DateTime.Now.AddDays(3);
                    ////Specify that the log must be truncated after the backup is complete.
                    sqlBackup.LogTruncation = BackupTruncateLogType.Truncate;

                    sqlBackup.FormatMedia = false;
                    ////Run SqlBackup to perform the full database backup on the instance of SQL Server.
                    sqlBackup.SqlBackup(sqlServer);
                    ////Remove the backup device from the Backup object.
                    sqlBackup.Devices.Remove(deviceItem);
                    MessageBox.Show("Successful backup is created!","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
            catch
            {
                // MessageBox.Show(ex.Message);
            }
        } catch { }
        } 
      
        private void stockReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();

            System.Windows.Forms.Form f1 = System.Windows.Forms.Application.OpenForms["Pur_inv_rpt_frm"];
            if (((Pur_inv_rpt_frm)f1) == null)
            { 
                Pur_inv_rpt_frm ua = new Pur_inv_rpt_frm();
                FormID = "1063";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    ua.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

            }
        }

        private void damageProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();

            System.Windows.Forms.Form f1 = System.Windows.Forms.Application.OpenForms["DamageProduct"];
            if (((DamageProduct)f1) == null)
            {
                 DamageProduct ua = new DamageProduct();
                FormID = "1061";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    ua.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

      

        private void receiveableAdjustToolStripMenuItem_Click(object sender, EventArgs e)
        {
       //     clm(); 
       //System.Windows.Forms.Form f1 = System.Windows.Forms.Application.OpenForms["RiderAdjust"];
       // if (((RiderAdjust)f1) == null)
       // {
       // RiderAdjust ua = new RiderAdjust();
       // FormID = "1068";
       // UserNametesting();
       // if (GreenSignal == "YES")
       // {
       // ua.Show();
       // }
       // else
       // {
       // MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
       // }
       // } 
        }

        private void expenseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm(); 
            System.Windows.Forms.Form f1 = System.Windows.Forms.Application.OpenForms["ExpenseReport"];
            if (((ExpenseReport)f1) == null)
            {
                ExpenseReport ua = new ExpenseReport();
                FormID = "1069";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    ua.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }  
        }

        private void purchaseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm(); 
            System.Windows.Forms.Form f1 = System.Windows.Forms.Application.OpenForms["Purchase_Sale"];
            if (((Purchase_Sale)f1) == null)
            {
                Purchase_Sale ua = new Purchase_Sale();
                FormID = "1070";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    ua.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }  
        }

        private void saleReportFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm();

            System.Windows.Forms.Form f1 = System.Windows.Forms.Application.OpenForms["SaleReportForm"];
            if (((SaleReportForm)f1) == null)
            {
                SaleReportForm ua = new SaleReportForm();
                FormID = "1071";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    ua.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }  
        }

        private void damageReportFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clm(); 
            System.Windows.Forms.Form f1 = System.Windows.Forms.Application.OpenForms["DamageReports"];
            if (((DamageReports)f1) == null)
            {
                DamageReports ua = new DamageReports();
                FormID = "1072";
                UserNametesting();
                if (GreenSignal == "YES")
                {  ua.Show();
                }
                else
                {
                    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }  
        }

        private void txtopen_Click(object sender, EventArgs e)
        {

        }

        private void updateFurnishProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
        //System.Windows.Forms.Form f1 = System.Windows.Forms.Application.OpenForms["UpdateFurnish"];
        //if (((UpdateFurnish)f1) == null)
        //{
        //    //
        //    UpdateFurnish ua = new UpdateFurnish();
        //    FormID = "1080";
        //    UserNametesting();
        //    if (GreenSignal == "YES")
        //    {
        //        ua.Show();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //    } 
        //} 
        }

        private void updateFurnishReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
        //System.Windows.Forms.Form f1 = System.Windows.Forms.Application.OpenForms["RptUpdateFurnished"];
        //if (((RptUpdateFurnished)f1) == null)
        //{
        //    //
        //    //RptUpdateFurnished ua = new RptUpdateFurnished();
        //    //FormID = "1081";
        //    //UserNametesting();
        //    //if (GreenSignal == "YES")
        //    //{
        //    //    ua.Show();
        //    //}
        //    //else
        //    //{
        //    //    MessageBox.Show("Sorry! You do not have permission to access this section. Contact Your Administrator for further assistance.", "STOP....!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //    //} 
        //    //
        // } 
        }

        private void warehouseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddWarehouse ads = new AddWarehouse(); ads.Show();
        }

        private void purchaseInquiryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PurchaseInquiry ads = new PurchaseInquiry(); ads.Show();

        }

        private void pettyCashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPettyCash ads = new frmPettyCash(); ads.Show();
        }

        private void productExpiryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductExpiry pe = new ProductExpiry(); pe.Show();
        }

        private void invoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Invoice pe = new Invoice(); pe.Show();
        }

        private void sReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeliveryInvoicerpt pe = new DeliveryInvoicerpt(); pe.Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                deal dd = new deal();
                dd.Show();
            }
            catch 
            {
                
                
            }
        }

     
    
    }
}
