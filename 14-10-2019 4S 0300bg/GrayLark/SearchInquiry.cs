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
    public partial class SearchInquiry : Form
    {
        PurchaseOrders po;
        public SearchInquiry(PurchaseOrders po1)
        {
            InitializeComponent(); Search();
            po = po1;
        }


        private void Search()
        {
            String get = "select * from PurchaseInq where VStatus='Inquiry'"; DataTable ds = clsDataLayer.RetreiveQuery(get);
            if (ds.Rows.Count > 0)
            {
                dataGridView1.DataSource = ds;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            String GetContent = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            po.txtpinv.Text = GetContent; this.Hide();
        }
    }
}
