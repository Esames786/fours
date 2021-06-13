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
    public partial class View_All_Receipt : Form
    {
        String fcol = "PartyName";
        String table = "Transactionss";
        String FormName = "";
        public View_All_Receipt()
        {
            InitializeComponent();  
        } 
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

        private void View_All_Receipt_Load(object sender, EventArgs e)
        {
            String query = "select PartyName from Transactionss";
            DataTable da= clsDataLayer.RetreiveQuery(query);
            if(da.Rows.Count > 0)
            {
         clsGeneral.SetAutoCompleteTextBox(txtSID,da);
            }
                comboSearch.SelectedIndex = 0;
            this.txtSID.Focus();
            try 
            { 
                SqlDataAdapter sda = new SqlDataAdapter("select * from Transactionss", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if(dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                } 
            }
            catch  
            {
              
            }
        }
  
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try { txtSID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); }
            catch { }
        }

        private void txtSID_Leave(object sender, EventArgs e)
        {
            String query = "";
            if (comboSearch.SelectedIndex == 0)
            {
                query = "select * from Transactionss where Remarks='" + txtSID.Text + "'";
            }
            else if (comboSearch.SelectedIndex == 1)
            {
                query = "select * from Transactionss where PartyName='" + txtSID.Text + "'";
            }
            else if (comboSearch.SelectedIndex == 2)
            {
                query = "select * from Transactionss where CashReceived='" + txtSID.Text + "'";
            }
            else if (comboSearch.SelectedIndex == 3)
            {
                query = "select * from Transactionss where Mode='" + txtSID.Text + "'";
            }
            DataTable d = clsDataLayer.RetreiveQuery(query);
            if (d.Rows.Count > 0)
            { dataGridView1.DataSource = d; }
        }

        private void comboSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
 if (comboSearch.SelectedIndex == 0)
            {
                String query = "select Remarks from Transactionss";

                DataTable da = clsDataLayer.RetreiveQuery(query);
                if (da.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtSID, da);
                }
            }
            else if (comboSearch.SelectedIndex == 1)
            {
                String query = "select PartyName from Transactionss";

                DataTable da = clsDataLayer.RetreiveQuery(query);
                if (da.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtSID, da);
                }
            }
            else if (comboSearch.SelectedIndex == 2)
            {
                String query = "select CashReceived from Transactionss";

                DataTable da = clsDataLayer.RetreiveQuery(query);
                if (da.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtSID, da);
                }
            }
            else if (comboSearch.SelectedIndex == 3)
            {
                String query = "select Mode	from Transactionss";

                DataTable da = clsDataLayer.RetreiveQuery(query);
                if (da.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtSID, da);
                }
            }
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            String query = "";
            query = "select * from Transactionss where TransId = '" + txtSID.Text + "'";
            DataTable d = clsDataLayer.RetreiveQuery(query);
            if(d.Rows.Count > 0)
            { 
            rptReceipt rpt = new rptReceipt();
            rpt.SetDataSource(d);
            PaymentView pop = new PaymentView(rpt);
            pop.Show();
            }
            else
            {
                MessageBox.Show("No Record Found!");
            }


        }
         
    }
}
