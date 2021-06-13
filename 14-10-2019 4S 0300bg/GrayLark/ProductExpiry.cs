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
    public partial class ProductExpiry : Form
    {
        public ProductExpiry()
        {
            InitializeComponent();
        }

        private void ProductExpiry_Load(object sender, EventArgs e)
        {
            ProductExpiry3(); ProductExpiry30();      
        }

        private void ProductExpiry3()
        {
            String q1 = "select NAME,LordNumber,Size,CATEGORY from add_product_stock where Status='Active'";
            String pdname = ""; String lord = ""; String Mileage = ""; String Categ = "";
            String Exp = ""; String Mfg = "";

            DataTable d = clsDataLayer.RetreiveQuery(q1);
            if (d.Rows.Count > 0)
            {
                for (int i = 0; i < d.Rows.Count; i++)
                {
                    pdname = d.Rows[i][0].ToString(); lord = d.Rows[i][1].ToString(); Mileage = d.Rows[i][2].ToString(); Categ = d.Rows[i][3].ToString();
                    String q2 = "select Expdate,Mfgdate,Quantity from add_product_stock where NAME = '" + pdname + "' and Size='" + Mileage + "' and LordNumber='" + lord + "' and Status='Active' and CATEGORY='" + Categ + "' ";
                    DataTable d1 = clsDataLayer.RetreiveQuery(q2);
                    if (d1.Rows.Count > 0)
                    {
                        #region algo
                        Exp = d1.Rows[0][0].ToString(); Mfg = d1.Rows[0][1].ToString();
                        String mquant = d1.Rows[0][2].ToString();
                        String[] get = Exp.Split('-');
                        if (Exp.Equals("-"))
                        {

                        }
                        else {
                            int dd = Convert.ToInt32(get[0]); int mm = Convert.ToInt32(get[1]); int yy = Convert.ToInt32(get[2]);

                            int cd = Convert.ToInt32(DateTime.Now.ToString("dd"));
                            int cm = Convert.ToInt32(DateTime.Now.ToString("MM"));
                            int cy = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
                            DateTime dfg = new DateTime(yy, mm, dd);
                            String expdate = dfg.AddMonths(-2).ToString("dd-MM-yyyy");
                            String[] eget = expdate.Split('-');

                            int dd2 = Convert.ToInt32(eget[0]); int mm2 = Convert.ToInt32(eget[1]); int yy2 = Convert.ToInt32(eget[2]);

                            if (cm >= mm && cy >= yy)
                            {
                                if (cd >= dd2)
                                {
                                    String dup = "update add_product_stock set Status='DeActive' where NAME = '" + pdname + "' and Size='" + Mileage + "' and LordNumber='" + lord + "' and Status='Active' and CATEGORY='" + Categ + "' "; clsDataLayer.ExecuteQuery(dup);
                                    MessageBox.Show("Category " + Categ + "Product Name " + pdname + " Mileage " + Mileage + " Lord Number " + lord + " has been Expired!");
                                }
                                else
                                {
                                    #region grid
                                    int n = grid.Rows.Add();
                                    grid.Rows[n].Cells[0].Value = pdname; grid.Rows[n].Cells[1].Value = Categ; grid.Rows[n].Cells[2].Value = Mileage;
                                    grid.Rows[n].Cells[3].Value = mquant; grid.Rows[n].Cells[4].Value = Mfg; grid.Rows[n].Cells[5].Value = Exp;
                                    #endregion grid
                                }
                            }
                            else if (cm >= mm2 && cy >= yy2)
                            {
                                if (cd >= dd2)
                                {
                                    #region grid
                                    int n = grid.Rows.Add();
                                    grid.Rows[n].Cells[0].Value = pdname; grid.Rows[n].Cells[1].Value = Categ; grid.Rows[n].Cells[2].Value = Mileage;
                                    grid.Rows[n].Cells[3].Value = mquant; grid.Rows[n].Cells[4].Value = Mfg; grid.Rows[n].Cells[5].Value = Exp;
                                    #endregion grid
                                }
                            }
                        }
                        #endregion algo
                    }
                }
            }
        }

        private void ProductExpiry30()
        {
            dataGridView1.Rows.Clear();
            String tbl = "";
            if (checkBox1.Checked)
            {
                tbl = "viewinv2";
            }
            else
            {
                tbl = "viewinv";
            }
            String q1 = "select distinct InCode,ProductName,USize,UPurchaseOrder from viewinv where status='Invoice'";
            String pdname = ""; String lord = ""; String Mileage = ""; String Categ = "";
            String Exp = ""; String Mfg = "";

            DataTable d = clsDataLayer.RetreiveQuery(q1);
            if (d.Rows.Count > 0)
            {
                for (int i = 0; i < d.Rows.Count; i++)
                {
                    String pcode = d.Rows[i][0].ToString();
                    pdname = d.Rows[i][1].ToString(); lord = d.Rows[i][3].ToString(); Mileage = d.Rows[i][2].ToString();
                    String tbl2 = "";
                    if (checkBox1.Checked)
                    {
                        tbl2 = "tblnInvoiceDetail";
                    }
                    else
                    {
                        tbl2 = "tblInvoiceDetail";
                    }
                    String q2 = "select UExpdate,uQuantity from "+tbl2+" where InCode='" + pcode + "' and ProductName = '" + pdname + "' and USize='" + Mileage + "' and UPurchaseOrder='" + lord + "'";
                    DataTable d1 = clsDataLayer.RetreiveQuery(q2);
                    if (d1.Rows.Count > 0)
                    {
                        #region algo
                        Exp = d1.Rows[0][0].ToString();   String mquant = d1.Rows[0][1].ToString();
                        String[] get = Exp.Split('-');

                        int dd = Convert.ToInt32(get[0]); int mm = Convert.ToInt32(get[1]); int yy = Convert.ToInt32(get[2]);

                        int cd = Convert.ToInt32(DateTime.Now.ToString("dd"));
                        int cm = Convert.ToInt32(DateTime.Now.ToString("MM"));
                        int cy = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
                        DateTime dfg = new DateTime(yy, mm, dd);
                        String expdate = dfg.AddMonths(-2).ToString("dd-MM-yyyy");
                        String[] eget = expdate.Split('-');

                        int dd2 = Convert.ToInt32(eget[0]); int mm2 = Convert.ToInt32(eget[1]); int yy2 = Convert.ToInt32(eget[2]);

                        if (cm >= mm && cy >= yy)
                        {
                            if (cd >= dd2)
                            {
                                 MessageBox.Show("Category " + Categ + "Product Name " + pdname + " Mileage " + Mileage + " Lord Number " + lord + " has been Expired!");
                            }
                            else
                            {
                                #region grid
                                int n = dataGridView1.Rows.Add();
                                dataGridView1.Rows[n].Cells[0].Value = pdname;
                                dataGridView1.Rows[n].Cells[1].Value = Mileage;
                                dataGridView1.Rows[n].Cells[2].Value = mquant;
                                dataGridView1.Rows[n].Cells[3].Value = Exp;
                                #endregion grid
                            }
                        }
                        else if (cm >= mm2 && cy >= yy2)
                        {
                            if (cd >= dd2)
                            {
                                #region grid
                                int n = dataGridView1.Rows.Add();
                                dataGridView1.Rows[n].Cells[0].Value = pdname;  
                                dataGridView1.Rows[n].Cells[1].Value = Mileage;
                                dataGridView1.Rows[n].Cells[2].Value = mquant; 
                                dataGridView1.Rows[n].Cells[3].Value = Exp;
                                #endregion grid
                            }
                        }

                        #endregion algo
                    }
                }
            }
        }

        private void ProductExpiry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ProductExpiry30();
        }
    }
}
