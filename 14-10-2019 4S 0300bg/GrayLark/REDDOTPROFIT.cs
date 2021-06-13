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
using Reddot_Express_Inventory.bin.Debug.Report;

namespace Reddot_Express_Inventory
{
    public partial class REDDOTPROFIT : Form
    {
        public REDDOTPROFIT()
        {
            InitializeComponent();
            this.BackColor = Color.SteelBlue;
        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        SqlCommand cmd;

        private void btnPrint_Click(object sender, EventArgs e)
        { 
            SProfit(); 
        }


        private void SProfit()
        {
            decimal fuel = 0;
            decimal Sale = 0; decimal purchase = 0;  decimal fadd = 0; decimal Exp = 0;
            String q1 = "";
            if (courier.Text.Equals("All"))
            {
                q1 = "select sum(BillAmount) from tracking_order where DeliveryStatus = '" + cmbstatus.Text + "' and Date between '" + date1.Text + "' and '" + date2.Text + "' "; 

            }
            else if (courier.Text.Equals("Karachi Delivery"))
            {
                q1 = "select sum(BillAmount) from tracking_order where SentBy='Karachi Delivery' and DeliveryStatus = '" + cmbstatus.Text + "' and Date between '" + date1.Text + "' and '" + date2.Text + "' ";  

            }
            else if (courier.Text.Equals("LEOPARD"))
            {
                q1 = "select sum(BillAmount) from tracking_order where SentBy='Leopard' and DeliveryStatus = '"+cmbstatus.Text+"' and Date between '" + date1.Text + "' and '" + date2.Text + "' "; 

            }
            DataTable d1 = clsDataLayer.RetreiveQuery(q1);
            if (d1.Rows.Count > 0)
            {
                String hk = d1.Rows[0][0].ToString();
                if (hk.Equals(""))
                {
                    Sale = 0;
                }
                else
                {
                    Sale = Convert.ToDecimal(d1.Rows[0][0].ToString());
                }
            }
            else { Sale = 0; }


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

            decimal salary = 0;
            String hq0 = "select sum(CashGiven) from Transactions where PartyName = 'Salary' and Dates between '" + date1.Text + "' and '" + date2.Text + "' "; DataTable dv10 = clsDataLayer.RetreiveQuery(hq0); if (dv10.Rows.Count > 0)
            {
                String hk1 = dv10.Rows[0][0].ToString();
                if (hk1.Equals(""))
                {
                    salary = 0;
                }
                else
                {
                    salary = Convert.ToDecimal(dv10.Rows[0][0].ToString());
                }
            }
            else { salary = 0; }

            if (courier.Text.Equals("All") || courier.Text.Equals("LEOPARD"))
            {

                String hq1 = "select sum(Cp_FreightCharges) from tbl_CPayment where Date between '" + date1.Text + "' and '" + date2.Text + "' "; DataTable dv1 = clsDataLayer.RetreiveQuery(hq1); if (dv1.Rows.Count > 0)
                {
                    String hk = dv1.Rows[0][0].ToString();
                    if (hk.Equals(""))
                    {
                        fuel = 0;
                    }
                    else
                    {
                        fuel = Convert.ToDecimal(dv1.Rows[0][0].ToString());
                    }
                }
                else { fuel = 0; }

            }
            else
            {
                 String gm1 = "select sum(CashGiven) from Transactions where PartyName = 'CourierFuel' and Dates between '" + date1.Text + "' and '" + date2.Text + "' "; DataTable dv0 = clsDataLayer.RetreiveQuery(gm1); if (dv0.Rows.Count > 0)
                {
                    String hk = dv0.Rows[0][0].ToString();
                    if (hk.Equals(""))
                    {
                        fuel = 0;
                    }
                    else
                    {
                        fuel = Convert.ToDecimal(dv0.Rows[0][0].ToString());
                    }
                }
                else { fuel = 0; }
            }
            
            ProfitReddot pr = new ProfitReddot();  
            RedProfit rp = new RedProfit(Sale.ToString(), purchase.ToString(), fadd.ToString(), fuel.ToString(), pr,salary.ToString());
            rp.Show();
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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
    }
}
