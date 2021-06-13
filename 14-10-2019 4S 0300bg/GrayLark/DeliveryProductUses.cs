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
    public partial class DeliveryProductUses : Form
    {
        DeliveryChallan sp; decimal lmuses = 0;
        public DeliveryProductUses(DeliveryChallan sp1)
        {
            InitializeComponent(); sp = sp1;
            txtcode.Text = sp.txtsale.Text;
        }

        private void restricUsed()
        {
        if (grid.CurrentCell.ColumnIndex == 4)
        {
        String pname = grid.CurrentRow.Cells[0].Value.ToString(); String psize = grid.CurrentRow.Cells[1].Value.ToString();
        String poc = grid.CurrentRow.Cells[2].Value.ToString(); String dc = txtcode.Text; decimal input =Convert.ToDecimal(grid.CurrentRow.Cells[4].Value.ToString());
        decimal inv1 = 0; decimal inv2 = 0; decimal foc1 = 0; decimal foc2 = 0;
                String a1 = "select sum(uQuantity),sum(ufocQuantity) from viewinv where status='Invoice' and DeliveryCode='" + dc + "' and ProductName='" + pname + "' and UPurchaseOrder='" + poc + "' and USize='" + psize + "'"; DataTable a2 = clsDataLayer.RetreiveQuery(a1); if (a2.Rows.Count > 0)
        {
            String hr = a2.Rows[0][0].ToString();
            if (hr.Equals("")) { inv1 = 0; }
            else
            {
                inv1 = Convert.ToDecimal(a2.Rows[0][0].ToString());
            } 
                    String hr2 = a2.Rows[0][1].ToString();
                    if (hr2.Equals("")) { foc1 = 0; }
                    else
                    {
                        foc1 = Convert.ToDecimal(a2.Rows[0][1].ToString());
                    }
                }
        String a20 = "select sum(uQuantity),sum(ufocQuantity) from viewinv2 where status='Invoice' and DeliveryCode='" + dc + "' and ProductName='" + pname + "' and UPurchaseOrder='" + poc + "' and USize='" + psize + "'"; DataTable a3 = clsDataLayer.RetreiveQuery(a20);
        if (a3.Rows.Count > 0)
        {
            String hr = a3.Rows[0][0].ToString();
            if (hr.Equals("")) { inv2 = 0; }
            else
            {
                inv2 = Convert.ToDecimal(a3.Rows[0][0].ToString());
            }

                    String hr2 = a3.Rows[0][1].ToString();
                    if (hr2.Equals("")) { foc2 = 0; }
                    else
                    {
                        foc2 = Convert.ToDecimal(a3.Rows[0][1].ToString());
                    }
                }
        decimal totalinvoice = inv1 + inv2 + foc1 + foc2; lmuses = totalinvoice;
        if(input < totalinvoice){
            MessageBox.Show("Invoice Making Quantity is = " + totalinvoice); grid.CurrentRow.Cells[4].Value = totalinvoice.ToString();
        }
        else{  }
        }
        }

        private void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
         try
         {
             restricUsed();
         decimal gq = 0; gq= Convert.ToDecimal(grid.CurrentRow.Cells[3].Value);   decimal used = 0; used = Convert.ToDecimal(grid.CurrentRow.Cells[4].Value);
         decimal returns = 0; returns = Convert.ToDecimal(grid.CurrentRow.Cells[5].Value); decimal dmg = 0; dmg = Convert.ToDecimal(grid.CurrentRow.Cells[6].Value);
         used = used + returns;
         decimal fg = 0; fg = gq - used; fg = fg - dmg;
          String ck = fg.ToString();
          if (ck.StartsWith("-"))
          {
              MessageBox.Show("Stock Can Not be negative value","Error",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    decimal mr4 = gq - lmuses;
              grid.CurrentRow.Cells[7].Value = mr4; grid.CurrentRow.Cells[4].Value = lmuses; grid.CurrentRow.Cells[5].Value = "0"; grid.CurrentRow.Cells[6].Value = "0";
          }
          else
          {
              grid.CurrentRow.Cells[7].Value = fg.ToString();
          }
         }
         catch { }
        }

        private void btnupd_Click(object sender, EventArgs e)
        {
            try
            {   for (int i = 0; i < grid.Rows.Count; i++)
                {
               decimal oldq = 0; decimal dm2 = 0; String st = "";
               String s5 = "select uReturnQuantity from tbl_DeliveryUses where DCode='" + txtcode.Text + "' and ProductName='" + grid.Rows[i].Cells[0].Value + "' and USize='" + grid.Rows[i].Cells[1].Value + "'"; DataTable ds5 = clsDataLayer.RetreiveQuery(s5);
               if (ds5.Rows.Count > 0)
               {
                   oldq = Convert.ToDecimal(ds5.Rows[0][0].ToString()); 
                   //dm2 = Convert.ToDecimal(ds5.Rows[0][1].ToString()); 
                   st = "true";
               }
               else { st = "false"; }
                    String sel = "select Quantity from add_product_stock where Status='Active' and Size='" + grid.Rows[i].Cells[1].Value + "' and NAME='" + grid.Rows[i].Cells[0].Value + "' and LordNumber='" + grid.Rows[i].Cells[2].Value + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(sel);
                if (d1.Rows.Count > 0)
                {
                    decimal dq = Convert.ToDecimal(d1.Rows[0][0].ToString()); decimal iqs = Convert.ToDecimal(grid.Rows[i].Cells[5].Value); decimal dm3 = Convert.ToDecimal(grid.Rows[i].Cells[6].Value);

                    decimal final = dq - oldq + iqs; 
                    //final = final + dm2 - dm3;
                    String upd = "update add_product_stock set Quantity=" + final + " where Status='Active' and Size='" + grid.Rows[i].Cells[1].Value + "' and NAME='" + grid.Rows[i].Cells[0].Value + "' and LordNumber='" + grid.Rows[i].Cells[2].Value + "'"; clsDataLayer.ExecuteQuery(upd);
                }
                if (st.Equals("true"))
                {
                    String del5 = "delete from tbl_DeliveryUses where DCode='" + txtcode.Text + "' and ProductName='" + grid.Rows[i].Cells[0].Value + "' and USize='" + grid.Rows[i].Cells[1].Value + "' and UPurchaseOrder='" + grid.Rows[i].Cells[2].Value + "'"; clsDataLayer.ExecuteQuery(del5);
                } 
                
                String ins = @"insert into tbl_DeliveryUses(uDamageQuantity,Description,DCode,ProductName,USize,UPurchaseOrder,uQuantity,uUsedQuantity,uReturnQuantity,uRemainingQuantity)values
	(" + grid.Rows[i].Cells[6].Value + ",'" + txtdesc.Text + "','" + txtcode.Text + "','" + grid.Rows[i].Cells[0].Value + "','" + grid.Rows[i].Cells[1].Value + "','" + grid.Rows[i].Cells[2].Value + "'," + grid.Rows[i].Cells[3].Value + "," + grid.Rows[i].Cells[4].Value + "," + grid.Rows[i].Cells[5].Value + "," + grid.Rows[i].Cells[7].Value + ")";
                clsDataLayer.ExecuteQuery(ins);

                }
            MessageBox.Show("Record Save Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }catch{ }
        }

        private void txtcode_TextChanged(object sender, EventArgs e)
        {
        try
        {
            String f21 = "select ProductName,USize,UPurchaseOrder,uQuantity,uUsedQuantity,uReturnQuantity,uRemainingQuantity,Description,uDamageQuantity from tbl_DeliveryUses where DCode='" + txtcode.Text + "'"; DataTable d4 = clsDataLayer.RetreiveQuery(f21);
            if (d4.Rows.Count > 0)
            {
                txtdesc.Text = d4.Rows[0]["Description"].ToString();
                grid.Rows.Clear();
                foreach(DataRow dr in d4.Rows){
                    int n = grid.Rows.Add();
                    grid.Rows[n].Cells[0].Value = dr["ProductName"].ToString(); grid.Rows[n].Cells[1].Value = dr["USize"].ToString(); grid.Rows[n].Cells[2].Value = dr["UPurchaseOrder"].ToString();
                    grid.Rows[n].Cells[3].Value = dr["uQuantity"].ToString(); grid.Rows[n].Cells[4].Value = dr["uUsedQuantity"].ToString(); grid.Rows[n].Cells[5].Value = dr["uReturnQuantity"].ToString();
                    grid.Rows[n].Cells[6].Value = dr["uDamageQuantity"].ToString(); grid.Rows[n].Cells[7].Value = dr["uRemainingQuantity"].ToString();
                    
                }
                grid.PerformLayout();
            }
            else
            {
                String f51 = "select ProductItem,Size,PurchaseOrder,Quantity from tbl_DeliveryDetail where DCode='" + txtcode.Text + "'";
                DataTable d45 = clsDataLayer.RetreiveQuery(f51);
                if (d45.Rows.Count > 0)
                {
                    grid.Rows.Clear();
                    foreach (DataRow dr in d45.Rows)
                    {
                        int n = grid.Rows.Add();
                        grid.Rows[n].Cells[0].Value = dr["ProductItem"].ToString(); grid.Rows[n].Cells[1].Value = dr["Size"].ToString(); grid.Rows[n].Cells[2].Value = dr["PurchaseOrder"].ToString();
                        grid.Rows[n].Cells[3].Value = dr["Quantity"].ToString(); grid.Rows[n].Cells[4].Value = "0"; grid.Rows[n].Cells[5].Value = "0";
                        grid.Rows[n].Cells[6].Value = "0";
                    }
                    grid.PerformLayout();
                }
                //
            }
        } catch { }
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            try
            {
            String get = "select * from tbl_DeliveryUses where DCode='" + txtcode.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(get);
            if (d1.Rows.Count > 0)
            {
             DeliveryUsesReport dus = new DeliveryUsesReport(); dus.SetDataSource(d1); rptDeliveryusesview sp = new rptDeliveryusesview(dus,txtcode.Text); sp.Show();
            }
            else
            {
                MessageBox.Show("No Record Found!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            }
            catch { }
        }

         
    }
}
