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
using System.Configuration;
using Reddot_Express_Inventory.bin.Debug.Report;

namespace Reddot_Express_Inventory
{
    public partial class ReceivingReceipt : Form
    {
        public ReceivingReceipt(){ InitializeComponent(); txtbx_id.Text =clsGeneral.getMAXCode("PaymentReceiving", "ID", "PR");}

        private void btn_rpt_Click(object sender, EventArgs e)
        {
            try
            {
                String value = "";

                if (rbtn_cash.Checked)
                {
                    value = rbtn_cash.Text;
                }
                else if (rbtn_chq.Checked)
                {
                    value = rbtn_chq.Text;
                }
                else if (rbtn_payorder.Checked)
                {
                    value = rbtn_payorder.Text;
                }
                //MessageBox.Show(value);

                string insertion = "";
                insertion = "insert into PaymentReceiving (Date,ID,Amount,Company_name,Contact_No,Payment_mode,Pay_against) values ('" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + txtbx_id.Text + "','" + txtbx_amnt.Text + "','" + txtbx_cn.Text + "','" + txtbx_cno.Text + "','" + value + "','" + txtbx_payagainst.Text + "') ";
                //MessageBox.Show(insertion);
                Console.WriteLine(insertion);
                clsDataLayer.ExecuteQuery(insertion);
                MessageBox.Show("RECORD INSERTED!");
                string query;
                query = "select a.Date,a.Amount,a.Company_name,a.Contact_No,a.Payment_Mode,a.Pay_Against from [PaymentReceiving] a where ID = '" + txtbx_id.Text + "' ";
                txtbx_id.Text = clsGeneral.getMAXCode("PaymentReceiving", "ID", "PR");

                DataTable dt = clsDataLayer.RetreiveQuery(query);
                if (dt.Rows.Count > 0)
                {
                    CrystalReportRecvRpt rpt = new CrystalReportRecvRpt();
                    rpt.SetDataSource(dt);
                    viewer view = new viewer(rpt);
                    view.Show();
                }

            }
            catch { }
        }

        private void txtbx_amnt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
     }
     
}

