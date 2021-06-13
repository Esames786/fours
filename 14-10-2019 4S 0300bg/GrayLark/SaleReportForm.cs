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
    public partial class SaleReportForm : Form
    {
        public SaleReportForm()
        {
            InitializeComponent();
            String tbl = "";
            if(checkBox1.Checked)
            {
                tbl = "tblnInvoice";
            }
            else
            {
                tbl = "tblInvoice";
            }
            String get = "select distinct InstituteName from "+tbl+"";
            DataTable ds = clsDataLayer.RetreiveQuery(get);
            if (ds.Rows.Count > 0)
            {
                grid.Rows.Clear();
                foreach (DataRow db in ds.Rows)
                {
                    int g = grid.Rows.Add();
                    grid.Rows[g].Cells[1].Value = db["InstituteName"].ToString(); 
                }
            }
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dd = new DataTable(); dd.Columns.Add("InstituteName"); dd.Columns.Add("InvoiceAmount", typeof(decimal));
                String tbl = "";
                if (checkBox1.Checked)
                {
                    tbl = "tblnInvoice";
                }
                else
                {
                    tbl = "tblInvoice";
                }
                for (int i = grid.Rows.Count - 1; i >= 0; i--)
                {
                    if ((bool)grid.Rows[i].Cells[0].FormattedValue)
                    {
                    String Party = grid.Rows[i].Cells[1].Value.ToString();
                    String query = "select sum(InvoiceAmount) from "+tbl+" where InvoiceDate between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "' and InstituteName='" + Party + "'";
                    DataTable ds = clsDataLayer.RetreiveQuery(query);
                    if (ds.Rows.Count > 0)
                    {
                        decimal totalsales = 0;
                        String bh = ds.Rows[0][0].ToString();
                        if (bh.Equals(""))
                        {
                            totalsales = 0;
                        }
                        else
                        {
                            totalsales = Convert.ToDecimal(ds.Rows[0][0].ToString());
                        }
                        dd.Rows.Add(Party,totalsales);
                    }
                    }
                }
          
                //Call Report
      TotalSales ts = new TotalSales();  ts.SetDataSource(dd); LedgerView pv = new LedgerView(date1.Value.ToString("yyyy-MM-dd"), date2.Value.ToString("yyyy-MM-dd"), ts); pv.Show();
            }
            catch
            {  }
        }
    }
}
