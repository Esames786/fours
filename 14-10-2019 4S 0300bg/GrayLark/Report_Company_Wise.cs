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
using Reddot_Express_Inventory.bin.Debug.Report;

namespace Reddot_Express_Inventory
{
    public partial class Report_Company_Wise : Form
    {
        public Report_Company_Wise()
        {
            InitializeComponent();
            this.BackColor = Color.SteelBlue;
        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        SqlCommand cmd;

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Report_Company_Wise_Load(object sender, EventArgs e)
        {
            vender_name();
        }

        private void vender_name()
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT NAME FROM vender", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            comboVender.DataSource = dt;
            comboVender.DisplayMember = "NAME";
            comboVender.ValueMember = "NAME";
             
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {


            string q = "select * from VU_Purchase where Date between '" + date1.Text + "' and '" + date2.Text + "' AND VendorName='" + comboVender.Text + "'";
            DataTable vender = clsDataLayer.RetreiveQuery(q);

            PurchaseSale rpt = new PurchaseSale();
            rpt.SetDataSource(vender);
            Purchase_Report_Company_Preview prcp = new Purchase_Report_Company_Preview(comboVender.Text, date1.Text , date2.Text , rpt);
            prcp.Show();
        }

        private void comboVender_Click(object sender, EventArgs e)
        {
           
            vender_name();
        }

        private void Report_Company_Wise_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
