using Reddot_Express_Inventory.bin.Debug.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reddot_Express_Inventory
{
    public partial class b_close_ledger_report : Form
    {
        public b_close_ledger_report()
        {
            InitializeComponent();
        }

        private void p_l_print_Click(object sender, EventArgs e)
        {
            try
            {
                close_ledger();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public void close_ledger()
        { 
                if (report_type.SelectedIndex == 0)
                {
                    string q = "select * from close_ledger where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and Name = '" + status.Text + "' Order By close_ledger_id asc";
                    DataTable sale = clsDataLayer.RetreiveQuery(q);
                    if (sale.Rows.Count > 0)
                    {
                        close_ledger_rptt rpt = new close_ledger_rptt();
                        rpt.SetDataSource(sale);
                        b_close_ledger_view dsp = new b_close_ledger_view(rpt, dateTimePicker1.Text, dateTimePicker2.Text);
                        dsp.Show();
                    }
                    else { MessageBox.Show("No Record Found!"); }
                }
                else if (report_type.SelectedIndex == 1)
                {
                    string q = "select * from close_ledger where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and USERNAME = '" + status.Text + "' Order By close_ledger_id asc";
                    DataTable sale = clsDataLayer.RetreiveQuery(q);
                    if (sale.Rows.Count > 0)
                    {
                        close_ledger_rptt rpt = new close_ledger_rptt();
                        rpt.SetDataSource(sale);
                        b_close_ledger_view dsp = new b_close_ledger_view(rpt,dateTimePicker1.Text, dateTimePicker2.Text);
                        dsp.Show();
                    }
                    else { MessageBox.Show("No Record Found!"); }
                }
                else if(report_type.SelectedIndex == 2)
                {
                    string p = "select * from close_ledger where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' Order By close_ledger_id asc";
                    DataTable sale = clsDataLayer.RetreiveQuery(p);
                    if (sale.Rows.Count > 0)
                    {
                        close_ledger_rptt rpt = new close_ledger_rptt();
                        rpt.SetDataSource(sale);
                        b_close_ledger_view dsp = new b_close_ledger_view(rpt, dateTimePicker1.Text, dateTimePicker2.Text);
                        dsp.Show();
                    }
                    else { MessageBox.Show("No Record Found!"); }
                }
                else if (report_type.SelectedIndex == 3)
                {
                    decimal totalpnl = 0;
                    String pname = ""; String PID = "";
                    DataTable ww = new DataTable();
                    ww.Columns.Add("PRODUCT_ID"); ww.Columns.Add("NAME");
                    ww.Columns.Add("OpeningQuantity");
                    ww.Columns.Add("SELLQUANTITY"); ww.Columns.Add("PURCHASEQUANTITY"); ww.Columns.Add("QUANTITY"); ww.Columns.Add("PURCHASE_PRICE"); ww.Columns.Add("SELL_PRICE");
                    ww.Columns.Add("PNL");
                    String sp5 = "select NAME,PRODUCT_ID from add_product"; DataTable sw = clsDataLayer.RetreiveQuery(sp5); if (sw.Rows.Count > 0)
                    {
                        for (int a = 0; a < sw.Rows.Count; a++)
                        {
                            pname = sw.Rows[a][0].ToString(); PID = sw.Rows[a][1].ToString();
                  
                    String qq = @"select sum(OpeningQuantity) as OpeningQuantity,sum(SELLQUANTITY) as SELLQUANTITY,sum(PURCHASEQUANTITY) as PURCHASEQUANTITY,
sum(QUANTITY) as QUANTITY,sum(PURCHASE_PRICE) as PURCHASE_PRICE,sum(SELL_PRICE) as SELL_PRICE,sum(PNL) as PNL from close_ledger where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and NAME='"+pname+"'"; DataTable sale = clsDataLayer.RetreiveQuery(qq);
                    if (sale.Rows.Count > 0)
                    {
                        decimal Opq = Convert.ToDecimal(sale.Rows[0][0].ToString()); decimal Sq = Convert.ToDecimal(sale.Rows[0][1].ToString()); decimal Pq = Convert.ToDecimal(sale.Rows[0][2].ToString());
                        decimal Cq = Convert.ToDecimal(sale.Rows[0][3].ToString()); decimal Pp = Convert.ToDecimal(sale.Rows[0][4].ToString()); decimal Sp = Convert.ToDecimal(sale.Rows[0][5].ToString());
                        decimal Pnl = Convert.ToDecimal(sale.Rows[0][6].ToString());
                        ww.Rows.Add(PID, pname, Opq, Sq, Pq, Cq, Pp, Sp, Pnl);
                        totalpnl += Pnl;
                    } 
                 }
             }

                    if (ww.Rows.Count > 0)
                    {
                        clr rpt = new clr();
                        rpt.SetDataSource(ww);
                        ProfitView dsp = new ProfitView(totalpnl.ToString(), rpt, dateTimePicker1.Text, dateTimePicker2.Text);
                        dsp.Show();
                    }
                    else { MessageBox.Show("No Record Found!"); }
                 
                }
          
 
        }


        
        private void report_type_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (report_type.SelectedIndex == 0)
            {
                status.Enabled = true;
                String query = "select Name from close_ledger"; DataTable d1 = clsDataLayer.RetreiveQuery(query); if (d1.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(status, d1); }
            }
            else if (report_type.SelectedIndex == 1)
            {
                status.Enabled = true;
                String query = "select USERNAME from close_ledger"; DataTable d1 = clsDataLayer.RetreiveQuery(query); if (d1.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(status, d1); }

            }
            else if (report_type.SelectedIndex == 2)
            {
                status.Enabled = false;
            }
            else if (report_type.SelectedIndex == 3)
            {
                status.Enabled = false;
            }
        }


        private void ProductClose()
        {
            String search = "select * from close_ledger where DATE= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            DataTable dt = clsDataLayer.RetreiveQuery(search);
            if (dt.Rows.Count > 0)
            {
                String delete = "delete from close_ledger where DATE= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
                clsDataLayer.ExecuteQuery(delete);
                insert_ledger();
            }
            else { insert_ledger(); }
        }


        public void insert_ledger()
        {
            String rec = "select * from add_product";
            DataTable d = clsDataLayer.RetreiveQuery(rec);
            if (d.Rows.Count > 0)
            {
                bool abc = false;
                for (int i = 0; i < d.Rows.Count; i++)
                {
                    String product_id = d.Rows[i]["PRODUCT_ID"].ToString();
                    String Name = d.Rows[i]["NAME"].ToString();
                    int Quantity = Convert.ToInt32(d.Rows[i]["QUANTITY"].ToString());
                    String Size = d.Rows[i]["SIZE"].ToString();
                    decimal purchase_price = Convert.ToDecimal(d.Rows[i]["PURCHASE_PRICE"].ToString());
                    int sale_price = Convert.ToInt32(d.Rows[i]["SELL_PRICE"].ToString());
                    String user_name = d.Rows[i]["USERNAME"].ToString();
                    String dateTime = d.Rows[i]["DATETIME"].ToString();
                    String status = d.Rows[i]["STATUS"].ToString();

                    decimal Quantity0 = 0; decimal sn = 0; decimal pnl = 0;
                    //var dt2 = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    var dt2 = DateTime.Now.ToString("yyyy-MM-dd");
                    var dq = "";
                    String sdate = "select Date from close_ledger where Date < '" + dt2 + "' Order BY close_ledger_id desc"; DataTable dsd = clsDataLayer.RetreiveQuery(sdate); if (dsd.Rows.Count > 0) { dq = dsd.Rows[0][0].ToString(); }
                    String Opq = "select sum(QUANTITY) from close_ledger where Date = '" + dq + "' and NAME='" + Name + "'"; DataTable ds = clsDataLayer.RetreiveQuery(Opq);
                    if (ds.Rows.Count > 0)
                    {
                        String qsd = ds.Rows[0][0].ToString();
                        if (qsd.Equals(""))
                        {
                            Quantity0 = 0;
                        }
                        else
                        {
                            Quantity0 = Convert.ToDecimal(ds.Rows[0][0].ToString());

                        }
                    }
                    else { Quantity0 = 0; }

                    decimal sq = 0;
                    String q1 = "select sum(Quantity),sum(TotalPriceItem) from vsale where Date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and SaleItem='" + Name + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(q1);
                    if (d1.Rows.Count > 0)
                    {
                        String sqa = d1.Rows[0][0].ToString();
                        if (sqa.Equals(""))
                        {
                            sq = 0; sn = 0;
                        }
                        else { sq = Convert.ToDecimal(d1.Rows[0][0].ToString()); sn = Convert.ToDecimal(d1.Rows[0][1].ToString()); }

                    }
                    else { sq = 0; }

                    decimal pq = 0;
                    String q2 = "select sum(Quantity) from VU_Purchase where Date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and PurchaseItem='" + Name + "'"; DataTable d2 = clsDataLayer.RetreiveQuery(q2);
                    if (d2.Rows.Count > 0)
                    {
                        String pur = d2.Rows[0][0].ToString();
                        if (pur.Equals(""))
                        {
                            pq = 0;
                        }
                        else { pq = Convert.ToDecimal(d2.Rows[0][0].ToString()); }
                    }
                    else { pq = 0; }
                    decimal tp = purchase_price * sq;
                    pnl = sn - tp;
                    string query1 = "INSERT INTO close_ledger (OpeningQuantity,SELLQUANTITY,PURCHASEQUANTITY,DATE,PRODUCT_ID,NAME,QUANTITY,SIZE,PURCHASE_PRICE,SELL_PRICE,USERNAME,DATETIME,STATUS,PNL) values(" + Quantity0 + "," + sq + "," + pq + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + product_id + "','" + Name + "'," + Quantity + ",'" + Size + "'," + purchase_price + "," + sale_price + ",'" + user_name + "','" + dateTime + "','" + status + "'," + pnl + ")";
                    if (clsDataLayer.ExecuteQuery(query1) > 0)
                    {
                        abc = true;
                    }

                }
                if (abc == true)
                {
                    MessageBox.Show("Close Ledger Saved..!");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProductClose();
        }
    }
}
