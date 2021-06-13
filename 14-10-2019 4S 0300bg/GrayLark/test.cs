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

namespace Reddot_Express_Inventory
{
    public partial class test : Form
    {
         DataTable dt   ;
         String hy = "";
        public test()
        {
            InitializeComponent();
            DateTime d1 = new DateTime();
            DateTime d2 = new DateTime();

            //this.BackColor = Color.SteelBlue;
            //String th = "20-04-2018";
            //String[] hn = th.Split('-');
            //MessageBox.Show(hn[2]);

         //   BankTransactionUpdate();
           // Bank();
            #region OldAlgorithm
            //decimal aa = -4;
            //if(aa  < 0)
            //{
            //    MessageBox.Show("Greater");

            //}
            //else
            //{
            //    MessageBox.Show("Small");
            //}

            //String a1 = "select PRODUCT_ID from add_product";
            //DataTable dts = clsDataLayer.RetreiveQuery(a1);
            //if(dts.Rows.Count > 0)
            //{
            //    int t = 1;
            //    for(int r = 0 ; r< dts.Rows.Count ; r++)
            //    {
            //        String h = t.ToString();
            //        String z = "P-0000"+h;
            //        String a = dts.Rows[r][0].ToString();
            //        String Update = "update add_product set PRODUCT_ID='"+z+"' where PRODUCT_ID='"+a+"'";
            //        clsDataLayer.ExecuteQuery(Update);
            //        t++;
            //    }
            //}


            //List<int> list = new List<int>();


            //list.Add(2); 
            //list.Add(3);
         


            //foreach (var auth in list)
            //{
            //    MessageBox.Show(auth.ToString());
            // }

            //list.Remove(3);
            //foreach (var auth in list)
            //{
            //    MessageBox.Show(auth.ToString());
            //}
          //  dataGridView1.Rows[0].Cells[0].Value = "asd";
          //  dataGridView1.Rows[0].Cells[1].Value = "50";
          //  dataGridView1.Rows[0].Cells[2].Value = "100";
          //  dataGridView1.Rows[0].Cells[3].Value = "500";
          ////  dataGridView1.Rows.Add();
        
           // DataTable dt = null;
           // dt = new DataTable();
           // dt.Columns.AddRange(new DataColumn[10] { new DataColumn("V_No", typeof(String)),
           //                     new DataColumn("Datetime", typeof(string)),
           //                     new DataColumn("AccountCode",typeof(string)), new DataColumn("RefCode",typeof(string)), new DataColumn("PaymentType",typeof(string)), new DataColumn("InvoiceNo",typeof(string)),
           //          new DataColumn("Particulars",typeof(string)), new DataColumn("Debit",typeof(string)), new DataColumn("Credit",typeof(string)), new DataColumn("Party_Balance",typeof(string))
           //});

            // dt.Rows.Add(dr.Rows[i]["V_No"], dr.Rows[i]["Datetime"], dr.Rows[i]["AccountCode"], dr.Rows[i]["RefCode"], dr.Rows[i]["PaymentType"], dr.Rows[i]["InvoiceNo"], dr.Rows[i]["Particulars"], dr.Rows[i]["Debit"], dr.Rows[i]["Credit"], dr.Rows[i]["Party_Balance"]);
            #endregion OldAlgorithm
        }

        private void Bank()
        {
            decimal oldbl = 0; String status = ""; decimal cashout = 0; decimal f = 0; String Code = "";
       //
            String hi = "select BankCode from BankTransaction where AccountNo='01-1670815-01' and BankCode < 'TP-00284' Order By BankCode desc";
            DataTable d20 = clsDataLayer.RetreiveQuery(hi);
            if (d20.Rows.Count > 0)
            {
                Code = d20.Rows[0][0].ToString();
            }
            //


        String q1 = "select CurrentCash from BankTransaction where BankCode < 'TP-00263'";
        DataTable d1 = clsDataLayer.RetreiveQuery(q1);
        if (d1.Rows.Count > 0)
        {
            oldbl = Convert.ToDecimal(d1.Rows[0][0].ToString());
        }
        String q2 = "select BankCash,CurrentCash,Status from BankTransaction where BankCode = 'TP-00264'";
        DataTable d2 = clsDataLayer.RetreiveQuery(q2);
        if (d2.Rows.Count > 0)
        {
            cashout = Convert.ToDecimal(d2.Rows[0][0].ToString()); status = d2.Rows[0][2].ToString();
            cashout = 5000;
            if (status.Equals("Paid"))
            {
                f = oldbl - cashout;
            }
            else
            {
                f = oldbl + cashout;
            }
            oldbl = f;

            String upd = "update BankTransaction set BankCash=" + cashout + ",CurrentCash=" + f + " where BankCode = 'TP-00264'"; clsDataLayer.ExecuteQuery(upd);
        }

        String q3 = "select BankCode,BankCash,CurrentCash,Status from BankTransaction where BankCode > 'TP-00264'";
        DataTable d3 = clsDataLayer.RetreiveQuery(q3);
        if (d3.Rows.Count > 0)
        {
            for (int a = 0; a < d3.Rows.Count; a++)
            { 
            cashout = Convert.ToDecimal(d3.Rows[a][1].ToString()); status = d3.Rows[a][3].ToString(); Code = d3.Rows[a][0].ToString();
            if (status.Equals("Paid"))
            {
                f = oldbl - cashout;
            }
            else
            {
                f = oldbl + cashout;
            }
            oldbl = f;
            String upd = "update BankTransaction set BankCash=" + cashout + ",CurrentCash=" + f + " where BankCode = '"+Code+"'"; clsDataLayer.ExecuteQuery(upd);

            }
            MessageBox.Show("Updated!");
        }
        }


        private void CashTransactionUpdate()
        {
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
                            String upd = "update BankTransaction set CurrentCash=" + balance + " where BankCode='" + rno + "' "; clsDataLayer.ExecuteQuery(upd);

                        }


                    }
                }
            }

        }
         
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  dataGridView1.DataSource = dt;
                 
            for(int r=0;r < dt.Rows.Count;r++)
            {
              //  dataGridView1.Rows[r].Cells[0].Value = dt.Rows[r][0];
             //   dataGridView1.Rows[r].Cells[1].Value = dt.Rows[r][1];
                int p = Convert.ToInt32(dataGridView1.Rows[r].Cells[2].Value);
                int y = Convert.ToInt32(dt.Rows[r][2]);
                int o = y - p;
              //  dataGridView1.Rows[r].Cells[3].Value = dt.Rows[r][3];
                MessageBox.Show(" " + o);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Price");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Total");

           dt.Rows.Add(new string[] {"sasd", dataGridView1.Rows[0].Cells[1].Value.ToString(), dataGridView1.Rows[0].Cells[2].Value.ToString(), dataGridView1.Rows[0].Cells[3].Value.ToString() });
         
            //foreach(DataGridViewRow dgvR in dataGridView1.Rows)
            //{
            //    dt.Rows.Add("sads", dgvR.Cells[1].Value, dgvR.Cells[2].Value, dgvR.Cells[3].Value);
                
            //}
            //hy = dataGridView1.Rows[0].Cells[0].Value.ToString();

        }

     

    
    }
}
