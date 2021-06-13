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
    public partial class TaxForm : Form
    {
        public TaxForm()
        {
            InitializeComponent();
            String get = "select Tax_Percent from tbl_tax"; DataTable dqw = clsDataLayer.RetreiveQuery(get); if (dqw.Rows.Count > 0) { txttax.Text = dqw.Rows[0][0].ToString(); } else { txttax.Text = "0"; }

        }

        private void button1_Click(object sender, EventArgs e)
        {
        try
        {
        String delete = "truncate table tbl_tax"; clsDataLayer.ExecuteQuery(delete);
        String insert = "insert into tbl_tax values(" + txttax.Text + ")"; clsDataLayer.ExecuteQuery(insert);
        MessageBox.Show("Tax Add Successfully!");
        }catch{}
        }
    }
}
