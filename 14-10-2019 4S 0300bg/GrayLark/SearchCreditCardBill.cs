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
    public partial class SearchCreditCardBill : Form
    {
        public SearchCreditCardBill()
        {
            InitializeComponent(); 
            clsGeneral.SetAutoCompleteTextBox(txtcno, clsDataLayer.RetreiveQuery("select CreditCardNo from tbl_CreditDue"));
             
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreditCardViewer cv = new CreditCardViewer(txtcno.Text, dt1.Text, dt2.Text); cv.Show();
        }
    }
}
