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
    public partial class SearchingTransfer : Form
    {
        public SearchingTransfer()
        {
            InitializeComponent(); 
        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        SqlCommand cmd;

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string q = "select * from tbl_transfer where Dates between '" + date1.Text + "' and '" + date2.Text + "'";
            DataTable sale = clsDataLayer.RetreiveQuery(q);
            if (sale.Rows.Count > 0) { TransferDaily rpt = new TransferDaily(); rpt.SetDataSource(sale); PaymentView dsp = new PaymentView(rpt); dsp.Show(); }
            else
            {
                MessageBox.Show("No Record Found!","Empty",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
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
