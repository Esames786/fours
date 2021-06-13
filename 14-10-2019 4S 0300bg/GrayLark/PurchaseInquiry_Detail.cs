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

namespace GrayLark
{
    public partial class PurchaseInquiry_Detail : Form
    {
        SqlConnection connectionString = null;
        String abc = "";
        PurchaseInquiry pi;
        public PurchaseInquiry_Detail(  PurchaseInquiry pi1)
        {
            InitializeComponent();
            connectionString = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
            pi = pi1; Showall();
          }

        private void Showall()
        {
            try
            {
                String get1 = "select PIurCode,VendorName,Date,Remarks,VStatus,LordNumber from PurchaseInq"; DataTable d1 = clsDataLayer.RetreiveQuery(get1); if (d1.Rows.Count > 0)
                {
                    dataGridView2.DataSource = d1;
                }
            }
            catch
            {
            }
        }

        private void Purchase_Detail_Load(object sender, EventArgs e)
        {
            try
            {
                clsGeneral.SetAutoCompleteTextBox(txtInvoiceID, clsDataLayer.RetreiveQuery("select PIurCode from PurchaseInq"));
            }
            catch
            {
            }
        }
         
        private void SearchID()
        {
            try
            {
                String get1 = "select PIurCode,VendorName,Date,Remarks,VStatus,LordNumber from PurchaseInq where PIurCode = '" + txtInvoiceID.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(get1); if (d1.Rows.Count > 0)
                {
                    dataGridView2.DataSource = d1;
                }
            }
            catch    {
             }
        }

        private void SearchDate()
        {
            try
            {
                String get1 = "select PIurCode,VendorName,Date,Remarks,VStatus,LordNumber from PurchaseInq where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(get1); if (d1.Rows.Count > 0)
                {
                    dataGridView2.DataSource = d1;
                }
            }
            catch
            {
            }
        }

        private void btn_view_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                SearchID();
            }
            else if (radioButton2.Checked)
            {
                SearchDate();
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            String GetContent = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            pi.txtcode.Text = GetContent; this.Hide();
        }

      

        
    }
}
 
