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
using GrayLark;

namespace GrayLark
{
    public partial class ExpenseReport : Form
    {
        DataTable fd;
        public ExpenseReport()
        {
            InitializeComponent();
            String sel = "select distinct PartyName from Transactions where HeadActCode='5'"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
            if (ds.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txtcus, ds);
            }
            Printing(); this.KeyPreview = true;
        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        SqlCommand cmd;

        private void Printing()
        {
            //
            fd = new DataTable();
            fd.Columns.Add("PartyName"); fd.Columns.Add(new DataColumn("CashGiven", System.Type.GetType("System.Int64")));
            String a1 = "select distinct PartyName from Transactions where HeadActCode='5'";
            DataTable gh = clsDataLayer.RetreiveQuery(a1);
            if (gh.Rows.Count > 0)
            {
                for (int u = 0; u < gh.Rows.Count; u++)
                {
                    String name = gh.Rows[u][0].ToString();
                    String query = "select sum(CashGiven) from Transactions where PartyName='" + name + "' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'";
                    DataTable ds = clsDataLayer.RetreiveQuery(query); if (ds.Rows.Count > 0)
                    {
                        String totexp = ds.Rows[0][0].ToString();
                        decimal e = 0;
                        if (totexp.Equals("")) { e = 0; }
                        else
                        {
                            e = Convert.ToDecimal(totexp);
                        }
                       
                        fd.Rows.Add(name, e);
                    }else { }
                }
                if (fd.Rows.Count > 0)
                {
                    grid.DataSource = fd;
                }
                else
                {
                    grid.DataSource = null;
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
        //REPORT
        if (fd.Rows.Count > 0)
        {
            rptExpense epr = new rptExpense(); epr.SetDataSource(fd); LedgerView pv = new LedgerView(date1.Value.ToString("dd-MMMM-yyyy"), date2.Value.ToString("dd-MMMM-yyyy"), epr); pv.Show();
        }
        else {  MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error); }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            txtcus.Text = "";
        } 

        private void Sale_Report_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void date2_ValueChanged(object sender, EventArgs e)  { Printing();  }

        private void date1_ValueChanged(object sender, EventArgs e)
        {
            Printing();
        }

        private void txtcus_TextChanged(object sender, EventArgs e)
        {
        try
        {
            fd = new DataTable();
            fd.Columns.Add("PartyName"); fd.Columns.Add(new DataColumn("CashGiven", System.Type.GetType("System.Int64")));
            String a1 = "select distinct PartyName from Transactions where HeadActCode='5' and PartyName='"+txtcus.Text+"'";
            DataTable gh = clsDataLayer.RetreiveQuery(a1);
            if (gh.Rows.Count > 0)
            {
                for (int u = 0; u < gh.Rows.Count; u++)
                {
                    String name = gh.Rows[u][0].ToString();
                    String query = "select sum(CashGiven) from Transactions where PartyName='" + name + "' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "'";
                    DataTable ds = clsDataLayer.RetreiveQuery(query); if (ds.Rows.Count > 0)
                    {
                    String totexp = ds.Rows[0][0].ToString(); decimal e2 = Convert.ToDecimal(totexp);fd.Rows.Add(name, e2);
                    }
                    else { }
                }
                if (fd.Rows.Count > 0)
                {
                    grid.DataSource = fd;
                }
                else
                {
                    grid.DataSource = null;
                }
            }
        } catch{ }
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        try
        {
            String query = "select TransId,Dates,PartyName,Mode,CashGiven,Remarks,BankName,AccountNo,ChequeNo,UserName from Transactions where PartyName = '" + grid.CurrentRow.Cells[0].Value + "' and Dates between '" + date1.Value.ToString("yyyy-MM-dd") + "' and '" + date2.Value.ToString("yyyy-MM-dd") + "' "; 
        ExpenseForm ef = new ExpenseForm(query);
        ef.Show();
             
        }
        catch { }
        }

        private void btnshow_Click(object sender, EventArgs e)
        {
           
        }

        private void btnall_Click(object sender, EventArgs e)
        {
            Printing();
        }

        private void ExpenseReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
            else if (e.Control == true && e.KeyCode == Keys.P)
            {
                btnPrint.PerformClick();
            }
        }
         
    }
}
