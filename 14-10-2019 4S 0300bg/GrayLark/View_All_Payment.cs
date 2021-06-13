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
    public partial class View_All_Payment : Form
    {
        String fcol = "PartyName";
        String table = "Transactionss";
        String FormName = "";
        public View_All_Payment()
        {
            InitializeComponent();  
        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

        private void View_All_Payment_Load(object sender, EventArgs e)
        {
            String query = "select PartyName from Transactions";
            DataTable da= clsDataLayer.RetreiveQuery(query);
            if(da.Rows.Count > 0)
         {
        clsGeneral.SetAutoCompleteTextBox(txtSID,da);
        }
        comboSearch.SelectedIndex = 0;
        this.txtSID.Focus();
        try 
        { 
        SqlDataAdapter sda = new SqlDataAdapter("select * from Transactions", con);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        if(dt.Rows.Count > 0)
        {
            dataGridView1.DataSource = dt;
        }
 
        } catch { }
        }
    
        private void txtSID_Leave(object sender, EventArgs e)
        {
         String query = "";
            if (comboSearch.SelectedIndex == 0)
            {
                query = "select * from Transactions where Remarks='" + txtSID.Text + "'";
            }
            else if (comboSearch.SelectedIndex == 1)
            {
                query = "select * from Transactions where PartyName='" + txtSID.Text + "'";
            }
            else if (comboSearch.SelectedIndex == 2)
            {
                query = "select * from Transactions where CashGiven='" + txtSID.Text + "'";
            }
            else if (comboSearch.SelectedIndex == 3)
            {
                query = "select * from Transactions where Mode='" + txtSID.Text + "'";
            }
            DataTable d = clsDataLayer.RetreiveQuery(query);
            if (d.Rows.Count > 0)
            {   dataGridView1.DataSource = d;}
        }

        private void comboSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboSearch.SelectedIndex==0)
            {
                String query = "select Remarks from Transactions";

                DataTable da = clsDataLayer.RetreiveQuery(query);
                if (da.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtSID, da);
                }
            }
            else if (comboSearch.SelectedIndex == 1)
            {
                String query = "select PartyName from Transactions";

                DataTable da = clsDataLayer.RetreiveQuery(query);
                if (da.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtSID, da);
                }
            }
            else if (comboSearch.SelectedIndex == 2)
            {
                String query = "select CashGiven from Transactions";

                DataTable da = clsDataLayer.RetreiveQuery(query);
                if (da.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txtSID, da);
                }
            }
            else if (comboSearch.SelectedIndex == 3)
            {
                String query = "select Mode	from Transactions";

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
        query = "select * from Transactions where TransId = '"+txtSID.Text+"'";
        //if (comboSearch.SelectedIndex == 0)
        //{
        //    query = "select * from Transactions where Remarks='" + txtSID.Text + "'";
        //}
        //else if (comboSearch.SelectedIndex == 1)
        //{
        //    query = "select * from Transactions where PartyName='" + txtSID.Text + "'";
        //}
        //else if (comboSearch.SelectedIndex == 2)
        //{
        //    query = "select * from Transactions where CashGiven='" + txtSID.Text + "'";
        //}
        //else if (comboSearch.SelectedIndex == 3)
        //{
        //    query = "select * from Transactions where Mode='" + txtSID.Text + "'";
        //}
        DataTable d = clsDataLayer.RetreiveQuery(query);
        if(d.Rows.Count > 0)
        { 
        rptPayment rpt = new rptPayment();
        rpt.SetDataSource(d);
        Receiptreport frm = new Receiptreport(rpt);
        frm.ShowDialog();  }
        else
        {  MessageBox.Show("No Record Found!");    }
        }

        private void select_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (select.SelectedIndex == 0)
            //    {
            //        table = "Transactions";
            //        fcol = "TransId";

            //    }
            //    else if (select.SelectedIndex == 1)
            //    {
            //        table = "BankCashRelease";
            //        fcol = "BankRel";
            //    }
            //    else if (select.SelectedIndex == 2)
            //    {
            //        table = "CashRelease";
            //        fcol = "CashRel";
            //    }
            //}
            //catch { }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        { try{txtSID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();}catch { } }

    }
}
