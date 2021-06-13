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

namespace GrayLark
{
    public partial class SelectChallan : Form
    {
        Invoice pv; String p1;
        public SelectChallan(Invoice pv1, String Party)
        {
            InitializeComponent();
            pv = pv1; p1 = Party;
            load();
        }

        private void Algo()
        {
            try
            {
                DataTable l1 = new DataTable();
                l1.Columns.Add("DCode"); l1.Columns.Add("InstituteName"); l1.Columns.Add("Dates"); l1.Columns.Add("Warehouse"); l1.Columns.Add("ChallanStatus");
                 
                String m1 = "select distinct Dcode from tbl_Delivery where vstatus ='Delivery'";
                DataTable d1 = clsDataLayer.RetreiveQuery(m1);
                if (d1.Rows.Count > 0)
                {
                    #region r1
                    String Code = "";
                    for (int a = 0; a < d1.Rows.Count; a++)
                    {
                        Code = d1.Rows[a][0].ToString();
//
                        String m10 = "select uUsedQuantity,ProductName,USize,UPurchaseOrder from tbl_DeliveryUses where DCode='" + Code + "'";
                           DataTable d12 = clsDataLayer.RetreiveQuery(m10);
                           decimal used = 0; bool ms = false; bool nb = false;
                if (d12.Rows.Count > 0)
                {
                    for (int b = 0; b < d12.Rows.Count; b++)
                    {
                        String pname = d12.Rows[b][1].ToString(); String size = d12.Rows[b][2].ToString(); String purorder = d12.Rows[b][3].ToString();
                        used = Convert.ToDecimal(d12.Rows[b][0].ToString()); if (used > 0)
                        {
                            ms = true;
                        }
                        if (ms == true)
                        {
                            #region st2
                            decimal inq2 = 0;
       String n4 = "select sum(uQuantity) from viewinv where DeliveryCode='" + Code + "' and ProductName='" + pname + "' and UPurchaseOrder='" + purorder + "' and USize='" + size + "'";
       DataTable d9 = clsDataLayer.RetreiveQuery(n4);
       if (d9.Rows.Count > 0)
       {
           String e0 = d9.Rows[0][0].ToString();
           if (e0.Equals(""))
           {
               inq2 = 0;
           }
           else
           {
               inq2 = Convert.ToDecimal(d9.Rows[0][0]);
           }
       }
      
           #region start
           String n41 = "select sum(uQuantity) from viewinv2 where DeliveryCode='" + Code + "' and ProductName='" + pname + "' and UPurchaseOrder='" + purorder + "' and USize='" + size + "'";
           DataTable d29 = clsDataLayer.RetreiveQuery(n41);
           if (d29.Rows.Count > 0)
           {
               String e0 = d29.Rows[0][0].ToString();
               if (e0.Equals(""))
               {
                   inq2 = 0;
               }
               else
               {
                   inq2 += Convert.ToDecimal(d29.Rows[0][0]);
               }
           }
           #endregion start
       
                            #endregion st2
       if (used > inq2) { nb = true; } else { ms = false; }
                        }
                    }
                }
                #endregion r1
                if (nb == true) { ms = true; }
                if (ms == true)
                {
                    String nms = "select DCode,InstituteName,Dates,Warehouse,ChallanStatus from tbl_Delivery where Dcode='" + Code + "'"; DataTable sdq = clsDataLayer.RetreiveQuery(nms);
                    if (sdq.Rows.Count > 0)
                    {
                        String scode = sdq.Rows[0][0].ToString(); String sinst = sdq.Rows[0][1].ToString(); String sdate = sdq.Rows[0][2].ToString();
                        String sware = sdq.Rows[0][3].ToString(); String schallan = sdq.Rows[0][4].ToString();
                        l1.Rows.Add(scode, sinst, sdate, sware, schallan);
                    }
                }
 
                     }
                }
                dataGridView1.DataSource = l1;
            }
            catch { }
        }

        private void load()
        {
            Algo();
        //    String query = "select distinct InstituteName from tbl_Delivery where vstatus ='Delivery'";
        //DataTable dtt = clsDataLayer.RetreiveQuery(query);
        //if (dtt.Rows.Count > 0)
        //{
        //    clsGeneral.SetAutoCompleteTextBox(txtvendor, dtt);
        //}
        //try
        //{
        //    String a1 = "select DCode,InstituteName,Dates,Warehouse,ChallanStatus,vstatus from tbl_Delivery where vstatus ='Delivery'";
        //    DataTable d1 = clsDataLayer.RetreiveQuery(a1); if (d1.Rows.Count > 0)
        //    {
        //        dataGridView1.DataSource = d1;
        //    }
        //}
        //catch { }
        }
         
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        try
        {
           pv.txtscan.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); 
            this.Hide();
        } catch { }
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space)
            {
             //   pv.txtpo.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); pv.txtCashRelease.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                this.Close();
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
      
        }

        private void txtvendor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

    }
}
