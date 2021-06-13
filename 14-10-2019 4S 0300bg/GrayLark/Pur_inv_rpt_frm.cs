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
    public partial class Pur_inv_rpt_frm : Form
    {
        public Pur_inv_rpt_frm()
        {
            InitializeComponent();
         }

        private void PurInven()
        {
            DataTable dv = new DataTable();
            dv.Columns.Add("DProduct"); dv.Columns.Add("SQuantity", typeof(decimal)); dv.Columns.Add("SellPrice", typeof(decimal));
            string query = "select distinct(DProduct) from DamageView where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "'";
            DataTable dt = clsDataLayer.RetreiveQuery(query);
            if (dt.Rows.Count > 0)
            {
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    string prod = dt.Rows[a][0].ToString();
                    string qq = "select sum(SQuantity) from DamageView where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "' and DProduct='" + prod + "'";
                    DataTable d2 = clsDataLayer.RetreiveQuery(qq);
                    if (d2.Rows.Count > 0)
                    { 
                        decimal quant = Convert.ToDecimal(d2.Rows[0][0].ToString());
                        decimal sps = 0;
                        String h1 = "  select SellPrice from DamageView where DProduct='"+prod+"'"; DataTable h2 = clsDataLayer.RetreiveQuery(h1);
                        if (h2.Rows.Count > 0)
                        {
                            sps = Convert.ToDecimal(h2.Rows[0][0].ToString());
                        }
                        dv.Rows.Add(prod, quant,sps);
                    }
                }
            }
            if (dv.Rows.Count > 0)
            {
                delizia_all_damage pi = new delizia_all_damage();
                pi.SetDataSource(dv);
                product_kitchen_view pv = new product_kitchen_view(pi, txt_dt_from.Text, txt_dt_to.Text);
                pv.Show();
            }
            else
            {
                MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void SaleInven()
        { 
            DataTable dv = new DataTable();
            dv.Columns.Add("RawProduct"); dv.Columns.Add("QuantityType");  dv.Columns.Add("RawQuantity", typeof(decimal)); dv.Columns.Add("PURCHASE_PRICE", typeof(decimal));
           
            string query = "select distinct(RawProduct) from tbl_TransferKitchen where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "'";
            DataTable dt = clsDataLayer.RetreiveQuery(query);
            if (dt.Rows.Count > 0)
            {
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    string prod = dt.Rows[a][0].ToString();
                    string qq = "select sum(RawQuantity) from tbl_TransferKitchen where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "' and RawProduct='" + prod + "'";
                    DataTable d2 = clsDataLayer.RetreiveQuery(qq);
                    if (d2.Rows.Count > 0)
                    { 
                    decimal quant = Convert.ToDecimal(d2.Rows[0][0].ToString());  decimal sell = 0; String QT = "";
                    String gp = "select PURCHASE_PRICE,QuantityType from ProductTransferKitchen where RawProduct = '" + prod + "'";
                    DataTable fv = clsDataLayer.RetreiveQuery(gp);
                    if (fv.Rows.Count > 0) { sell = Convert.ToDecimal(fv.Rows[0][0].ToString()); QT = fv.Rows[0][1].ToString(); }
                    dv.Rows.Add(prod,QT,quant,sell);
                    }
                }
            }
            if (dv.Rows.Count > 0)
            {
                delizia_all_kitchen pi = new delizia_all_kitchen();
                pi.SetDataSource(dv);
                product_kitchen_view pv = new product_kitchen_view(pi, txt_dt_from.Text, txt_dt_to.Text);
                pv.Show();
            }
            else
            {
                MessageBox.Show("No Record Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); 
            }
        }

        private void KitchenToCold()
        {
            DataTable dv = new DataTable();
            dv.Columns.Add("SProduct"); dv.Columns.Add("SQuantity", typeof(decimal)); dv.Columns.Add("SellPrice", typeof(decimal));
            string query = "select distinct(SProduct) from tbl_TransferColdStorage where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "'";
            DataTable dt = clsDataLayer.RetreiveQuery(query);
            if (dt.Rows.Count > 0)
            {
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    string prod = dt.Rows[a][0].ToString();
                    string qq = "select sum(SQuantity) from tbl_TransferColdStorage where Date between '" + txt_dt_from.Value.ToString("yyyy-MM-dd") + "' and '" + txt_dt_to.Value.ToString("yyyy-MM-dd") + "' and SProduct='" + prod + "'";
                    DataTable d2 = clsDataLayer.RetreiveQuery(qq);
                    if (d2.Rows.Count > 0)
                    { 
                        decimal quant = Convert.ToDecimal(d2.Rows[0][0].ToString());
                        decimal sell = 0;
                        String gp = "select SellPrice from  ProductTransferColdstorage where SProduct = '"+prod+"'";
                        DataTable fv = clsDataLayer.RetreiveQuery(gp);
                        if (fv.Rows.Count > 0) { sell = Convert.ToDecimal(fv.Rows[0][0].ToString()); }
                        dv.Rows.Add(prod, quant,sell);
                    }
                }
            }
            if (dv.Rows.Count > 0)
            {
                delizia_all_wkitchen pi = new delizia_all_wkitchen();
                pi.SetDataSource(dv);
                product_kitchen_view pv = new product_kitchen_view(pi, txt_dt_from.Text, txt_dt_to.Text); pv.Show();
            }
            else
            {
                MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }
        }
        //
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                SaleInven();
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                KitchenToCold();
            }
            else
            {
                PurInven();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        { 
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
    //if(comboBox1.SelectedIndex == 0)
    //{
    //    txt_vend_name.Enabled = false;
    //}else{
    //    txt_vend_name.Enabled = true;
    //}
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                { this.Close(); }
            }
            catch { }
        } 
    }
}
