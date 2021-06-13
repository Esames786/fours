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
    public partial class STesting : Form
    {
        String Code = ""; String HeaderAccount = "";
        public STesting()
        {
            InitializeComponent();
            clsGeneral.SetAutoCompleteTextBox(txtname, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 24 and HeaderActCode='10101' Order BY ID DESC"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String f1 = "select BillAmount,SaleCode,Dates from tbl_sale where CustomerName='" + txtname.Text + "'";
            DataTable d1 = clsDataLayer.RetreiveQuery(f1);
            if (d1.Rows.Count > 0)
            {
                for (int a = 0; a < d1.Rows.Count; a++)
                {
             decimal mbill = Convert.ToDecimal(d1.Rows[a][0].ToString());
             String date1 = d1.Rows[a][2].ToString(); String scode = d1.Rows[a][1].ToString();
             Posting(scode, date1, mbill.ToString());
                }
                MessageBox.Show("Done");
            }
        }

        private void PartyCode(String title)
        {
            try
            {
                String sel = "select ActCode,HeaderActCode from Accounts where ActTitle = '" + title + "'";
                DataTable dc = clsDataLayer.RetreiveQuery(sel);
                if (dc.Rows.Count > 0) { Code = dc.Rows[0][0].ToString(); HeaderAccount = dc.Rows[0][1].ToString(); }
            }
            catch { }
        }


        private void ReceiveDue(String Code,String Amount)
        {
            try
            {
                PartyCode(txtname.Text);
                String rec = "select * from ReceiveDue where PartyCode = '" + Code + "'";
                DataTable d = clsDataLayer.RetreiveQuery(rec);
                if (d.Rows.Count > 0)
                {
                    decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
                    decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
                    decimal received = decimal.Parse(d.Rows[0]["ReceivedAmount"].ToString());
                    decimal final = Convert.ToDecimal(Amount);
                    total = total+ final;
                    due = due + final;
                    string updateblnc = "update ReceiveDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                    clsDataLayer.ExecuteQuery(updateblnc);
                }
                else
                {
                    decimal b = Convert.ToDecimal(Amount);
                    String ii = @"insert into ReceiveDue (RemCode,PartyName,PartyCode,TotalAmount,DueAmount,ReceivedAmount,Company_Name)
                values('" + Code + "','" + txtname.Text + "','" + Code + "'," + b + "," + b + ",0,'Delizia')";
                    clsDataLayer.ExecuteQuery(ii);
                }
            }
            catch { }
        }
          

        private void Posting(String Sale,String Date,String Amount)
        {
            try
            {
                #region cashinc
                PartyCode(txtname.Text);
                if (!HeaderAccount.Equals("1"))
                {
                    ReceiveDue(Sale, Amount);
                    decimal a2 = PartyBalance();
                    decimal b2 = Convert.ToDecimal(Amount);
                    decimal c2 = a2 + b2;
                  String  VoucherStatus = "OnCredit";
                    String ins = @"insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
           " values('Sales','" + Sale + "','" + Date + "','" + Code + "','" + Code + "','" + Sale + "','" + txtname.Text + "'," + b2 + ",0.00,'" + VoucherStatus + "'," + c2 + ",'Delizia') insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
           " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('Sales','" + Sale + "','" + Date + "','01010201','" + Code + "','" + Sale + "','Sales'," + b2 + ",0.00,'" + VoucherStatus + "' ," + c2 + ",'Delizia')";
                    clsDataLayer.ExecuteQuery(ins);
                }
                #endregion cashinc

            }
            catch { }
        }

        decimal balance = 0;
        public decimal PartyBalance()
        {
            try
            {
                String get = "select Party_Balance from LedgerReceived where RefCode='" + Code + "' order by ID desc";
                DataTable dfr = clsDataLayer.RetreiveQuery(get);
                if (dfr.Rows.Count > 0)
                {
                    balance = decimal.Parse(dfr.Rows[0]["Party_Balance"].ToString());
                }
                else
                {
                    balance = 0;
                }

            }
            catch { } return balance;
        }


    }
}
