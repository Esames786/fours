using Reddot_Express_Inventory.bin.Debug.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reddot_Express_Inventory
{
    public partial class OutletClient : Form
    {
        String UID = Login.UserID;
        public string GreenSignal = "";
        public string FormID = ""; decimal old = 0;
        String Code,HeaderAccount ="";
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

        public OutletClient()
        {
            InitializeComponent(); Disable(); btnNew.Focus();
            clsGeneral.SetAutoCompleteTextBox(txtoutlet, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 24 and HeaderActCode='10101' Order BY ID DESC"));
            clsGeneral.SetAutoCompleteTextBox(txt_name, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 24 and HeaderActCode='10101' Order BY ID DESC"));
            loadsearch(); txtsearch.Enabled = true; this.KeyPreview = true;
        }

        private void loadsearch()
        {
            String h = "select OutletName from tbl_OutletClient order by OC_ID desc"; DataTable d1 = clsDataLayer.RetreiveQuery(h);
            if (d1.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txtsearch, d1);
            }

            String h2 = "select OC_Code,OutletName,Amount,CustomerName from tbl_OutletClient order by OC_ID desc"; DataTable d2 = clsDataLayer.RetreiveQuery(h2);
           if (d2.Rows.Count > 0)
           {
               dataGridView1.DataSource = d2; dataGridView1.PerformLayout();
           }
        }

        public void UserNametesting()
        {
            string query = "select * from user_access where USENAME='" + UID + "' AND FORM_NAME='" + FormID + "'";
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
            con.Open();
            SqlCommand sc = new SqlCommand(query, con);
            SqlDataReader dr = sc.ExecuteReader();
            dr.Read();

            if (dr.HasRows == true)
            {
                if (dr["ACCESS"].ToString() == "Yes")
                {
                    GreenSignal = "YES";
                }
                else
                {
                    GreenSignal = "NO";

                }
            }
        }
         
        private bool Enable()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
            if (c is TextBox)
            {
            if (((TextBox)c).Enabled == false)
            {
                ((TextBox)c).Enabled = true;
                flag = true; 
            }
            }
            else if (c is ComboBox)
            {
                if (((ComboBox)c).Enabled == false)
                {
                    ((ComboBox)c).Enabled = true;
                    flag = true;
            
                }
            }
            else if (c is MaskedTextBox)
            {
                if (((MaskedTextBox)c).Enabled == false)
                {
                    ((MaskedTextBox)c).Enabled = true;
                    flag = true;
            
                }
            }
            }
            return flag;
        }

        private void Clears()
        {
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Text != "")
                    {
                        ((TextBox)c).Text = "";
                    }
                }
                else if (c is ComboBox)
                { 
                    if (((ComboBox)c).Text != "")
                    {
                        ((ComboBox)c).Text = ""; 
                    } 
                }
            }
        }
        private bool CheckAllFields()
        {
            bool flag = false; 
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Text == "")
                    {
                        flag = true;
                        break;
                    }
                }
                else if (c is ComboBox)
                { 
                    if (((ComboBox)c).Text == "")
                    {
                        flag = true;
                        break;
                    } 
                }
            }
            return flag;
        }
        private bool Disable()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Enabled == true)
                    {
                        ((TextBox)c).Enabled = false;
                        flag = true; 
                    }
                }
                else if (c is ComboBox)
                {
                    if (((ComboBox)c).Enabled == true)
                    {
                        ((ComboBox)c).Enabled = false;
                        flag = true;

                    }
                }
                else if (c is MaskedTextBox)
                {
                    if (((MaskedTextBox)c).Enabled == true)
                    {
                        ((MaskedTextBox)c).Enabled = false;
                        flag = true; 
                    }
                }
            }
            return flag;
        }
        private void GridRefresh(){
            String gb = "select OC_Code,OutletName,CustomerName,Amount,Remarks,Username from tbl_OutletClient"; DataTable ds = clsDataLayer.RetreiveQuery(gb);
            if(ds.Rows.Count > 0){dataGridView1.DataSource=ds;}
        }
        private void OnNew() { Clears(); Enable(); txt_id.Text = clsGeneral.getMAXCode("tbl_OutletClient", "OC_Code", "OC"); }

        private void OnSave() {
       if (!CheckAllFields())
       {
       String sel = "select * from tbl_OutletClient where OC_Code='" + txt_id.Text + "'"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
       if (ds.Rows.Count < 1)
       {
           txt_id.Text = clsGeneral.getMAXCode("tbl_OutletClient", "OC_Code", "OC");
           String ins = "insert into tbl_OutletClient(OC_Code,OutletName,CustomerName,Amount,Remarks,Username,Dates)values('" + txt_id.Text + "','" + txtoutlet.Text+ "','"+txt_name.Text+"','"+txt_amount.Text+"','"+txtremarks.Text+"','"+Login.UserID+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"')"; clsDataLayer.ExecuteQuery(ins); OutletAdjust(); CustomerAdjust(); 
       }
       else { MessageBox.Show("Already Add! ", "Alert", MessageBoxButtons.AbortRetryIgnore); }
       }
       else { MessageBox.Show("Fill All Fields! ", "Alert", MessageBoxButtons.AbortRetryIgnore); }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            FormID = "Add";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                old = 0; OnNew(); btnedit.Enabled = false; btnSave.Enabled = true; btnupdate.Enabled = false; btnNew.Enabled = false; txtoutlet.Focus(); txtsearch.Text = "-";
                txtsearch.Enabled = true; txtsearch.Text = "-";
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        //On Update
        private void button3_Click(object sender, EventArgs e)
        {
               FormID = "Update";  UserNametesting();
            if (GreenSignal == "YES")
            {
        if (!CheckAllFields())
        {
            String del = "delete from tbl_OutletClient where OC_Code='" + txt_id.Text + "'"; clsDataLayer.ExecuteQuery(del); 
            //
                 String sel = "select * from tbl_OutletClient where OC_Code='" + txt_id.Text + "'"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
                if (ds.Rows.Count < 1)
                {
                //txt_id.Text = clsGeneral.getMAXCode("tbl_OutletClient", "OC_Code", "OC");
                String ins = "insert into tbl_OutletClient(OC_Code,OutletName,CustomerName,Amount,Remarks,Username,Dates)values('" + txt_id.Text + "','" + txtoutlet.Text + "','" + txt_name.Text + "','" + txt_amount.Text + "','" + txtremarks.Text + "','" + Login.UserID + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')"; clsDataLayer.ExecuteQuery(ins);
                
                ReceiveDueCustomer(); ReceiveDueOutlet();
                PartyCode(txtoutlet.Text);
                try
                {
                int index = GetRowIndex(txt_id.Text);
                DataTable dt = SetDT();
                ShowDt(dt);
                DeleteRecord();
                dt.Rows[index][7] = Convert.ToDecimal(txt_amount.Text);
                dt.Rows[index + 1][8] = Convert.ToDecimal(txt_amount.Text);
                dt = SomeOperation(dt);
                InsertRecord(dt);
                
                PartyCode(txt_name.Text);
                int index1 = GetRowIndex(txt_id.Text);
                DataTable dt1 = SetDT();
                ShowDt(dt1);
                DeleteRecord();
                dt1.Rows[index1][7] = Convert.ToDecimal(txt_amount.Text);
                dt1.Rows[index1 + 1][8] = Convert.ToDecimal(txt_amount.Text);
                dt1 = SomeOperation(dt1);
                InsertRecord(dt1);
                }
                catch { }
                MessageBox.Show("Voucher Updated! ", "Success", MessageBoxButtons.OK,MessageBoxIcon.Information);
                Disable(); GridRefresh(); btnedit.Enabled = true; btnSave.Enabled = false; btnupdate.Enabled = false; btnNew.Enabled = true; loadsearch();
                }
                else { MessageBox.Show("Already Add! ", "Alert", MessageBoxButtons.AbortRetryIgnore); }
        
            //

        }
         else { MessageBox.Show("Fill All Fields! ", "Alert", MessageBoxButtons.AbortRetryIgnore); }
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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

        private void ReceiveDueOutlet()
        {
        try
        {
        PartyCode(txtoutlet.Text);
        String rec = "select * from ReceiveDue where PartyCode = '" + Code + "'";
        DataTable d = clsDataLayer.RetreiveQuery(rec);
        if (d.Rows.Count > 0)
        {
            decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
            decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
            decimal received = decimal.Parse(d.Rows[0]["ReceivedAmount"].ToString());
            decimal final = Convert.ToDecimal(txt_amount.Text);
            received =received-old+final;
            due =due+old-final;
            string updateblnc = "update ReceiveDue set ReceivedAmount=" + received.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
            clsDataLayer.ExecuteQuery(updateblnc);
        } 
        } catch { }
        }

        private void OutletAdjust()
        {
      PartyCode(txt_name.Text);
      String CusCode = Code;
      PartyCode(txtoutlet.Text);

      decimal a2 = PartyBalance();
      decimal b2 = Convert.ToDecimal(txt_amount.Text);
      decimal c = a2 - b2;
          String VoucherStatus = "OnCredit";
          String ins = @"insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
          " values('" + txtremarks.Text + "','" + txt_id.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','000045','" + Code + "','" + txt_id.Text + "','Sale Transfer'," + txt_amount.Text + ",0.00,'OnReceived'," + c + ",'Delizia') insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
          " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtremarks.Text + "','" + txt_id.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txt_id.Text + "','" + txtoutlet.Text + "'," + txt_amount.Text + ",0.00,'OnReceived' ," + c + ",'Delizia')";
          if (clsDataLayer.ExecuteQuery(ins) > 0) { ReceiveDueOutlet(); }
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

        private void ReceiveDueCustomer()
        {
            try
            {
                PartyCode(txt_name.Text);
                String rec = "select * from ReceiveDue where PartyCode = '" + Code + "'";
                DataTable d = clsDataLayer.RetreiveQuery(rec);
                if (d.Rows.Count > 0)
                {
                    decimal total = decimal.Parse(d.Rows[0]["TotalAmount"].ToString());
                    decimal due = decimal.Parse(d.Rows[0]["DueAmount"].ToString());
                    decimal received = decimal.Parse(d.Rows[0]["ReceivedAmount"].ToString());
                    decimal final = Convert.ToDecimal(txt_amount.Text);
                    total =total-old+final;
                    due = due - old + final;
                    string updateblnc = "update ReceiveDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + Code + "' ";
                    clsDataLayer.ExecuteQuery(updateblnc);
                }
                else
                {
                    decimal b = Convert.ToDecimal(txt_amount.Text);
                    String ii = @"insert into ReceiveDue (RemCode,PartyName,PartyCode,TotalAmount,DueAmount,ReceivedAmount,Company_Name)
                values('" + txt_id.Text + "','" + txt_name.Text + "','" + Code + "'," + b + "," + b + ",0,'Delizia')";
                    clsDataLayer.ExecuteQuery(ii);
                }
            }
            catch { }
        }
        private void CustomerAdjust()
        {
         PartyCode(txt_name.Text);
         decimal a2 = PartyBalance();
         decimal b2 = Convert.ToDecimal(txt_amount.Text);
         decimal c = a2 + b2;
         String VoucherStatus = "OnCredit";
         String ins = @"insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
         " values('"+txtremarks.Text+"','" + txt_id.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txt_id.Text + "','" + txt_name.Text + "'," + txt_amount.Text + ",0.00,'OnCredit'," + c + ",'Delizia') insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
         " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtremarks.Text + "','" + txt_id.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','01010201','" + Code + "','" + txt_id.Text + "','Sale Received'," + txt_amount.Text + ",0.00,'OnCredit' ," + c + ",'Delizia')";
         if (clsDataLayer.ExecuteQuery(ins) > 0) {
             ReceiveDueCustomer();
         } 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
             FormID = "Save";
                UserNametesting();
                if (GreenSignal == "YES")
                {
           if (!CheckAllFields())
           {
               OnSave(); MessageBox.Show("Voucher Saved! ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); Disable(); GridRefresh();
           btnedit.Enabled = true; btnSave.Enabled = false; btnupdate.Enabled = false; btnNew.Enabled = true; loadsearch();
           }
           else { MessageBox.Show("Fill All Fields! ", "Alert", MessageBoxButtons.AbortRetryIgnore); }
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                } 
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
                  FormID = "Edit";
        UserNametesting();
        if (GreenSignal == "YES")
        {
            btnedit.Enabled = false; btnSave.Enabled = false; btnupdate.Enabled = true; btnNew.Enabled = true; Enable(); old = Convert.ToDecimal(txt_amount.Text);
            txtsearch.Text = "-"; txtoutlet.Enabled = false; txt_name.Enabled = false; txtsearch.Enabled = true; txtsearch.Text = "-";
        }
        else { MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try { txt_id.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); }   catch { }
        }

        private void txt_id_TextChanged(object sender, EventArgs e)
        {
        try
        {
        String sel = "select OutletName,CustomerName,Amount,Remarks from tbl_OutletClient where OC_Code='" + txt_id.Text + "'"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
        if (ds.Rows.Count > 0) {  txt_name.Text = ds.Rows[0]["CustomerName"].ToString(); txt_amount.Text = ds.Rows[0]["Amount"].ToString();
        txtremarks.Text = ds.Rows[0]["Remarks"].ToString(); txtoutlet.Text = ds.Rows[0]["OutletName"].ToString();
        } 
        } catch { }
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            try
            {
                FormID = "Print";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    String qs = "";
                    if (!txt_id.Text.Equals(""))
                    {
                        qs = "select * from tbl_OutletClient where OC_Code='" + txt_id.Text + "'";
                    }
                    else
                    {
                        qs = "select * from tbl_OutletClient";
                    }
                    DataTable d1 = clsDataLayer.RetreiveQuery(qs); if (d1.Rows.Count > 0)
                    {
                        OutClients fp = new OutClients(); fp.SetDataSource(d1); LedgerView pop = new LedgerView("","", fp);
                        pop.Show();
                    }
                    else { MessageBox.Show("No Record Found! ", "Alert", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error); }
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch { }
        }

        private void txt_amount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void close(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            try {
                String h = "select OC_Code,OutletName,CustomerName from tbl_OutletClient where OutletName='" + txtsearch.Text + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(h);
           if (d1.Rows.Count > 0)
           {
               dataGridView1.DataSource = d1;
           }
            }
            catch { }
        }
     
        //

        //Ledger Update
        #region LedgerUpdate
        private int GetRowIndex(string input)
        {
            int index = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT V_No FROM LedgerReceived where RefCode = '" + Code + "' order by ID asc", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetValue(0).ToString() == input)
                    {
                        break;
                    }
                    index++;
                }
                con.Close();
            }
            catch { }
            return index;
        }
        private void DeleteRecord()
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM LedgerReceived where RefCode = '" + Code + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch { }
        }
        private DataTable SetDT()
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                System.Data.SqlClient.SqlDataAdapter cmd = new SqlDataAdapter("select V_No, Datetime, AccountCode, RefCode, PaymentType, InvoiceNo, Particulars, Debit, Credit, Party_Balance,Company_Name,Description from LedgerReceived where RefCode = '" + Code + "' order by ID asc", con);
                cmd.Fill(dt);
                con.Close();
            }
            catch { }
            return dt;
        }
        private DataTable SomeOperation(DataTable dt)
        {
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Console.WriteLine("Payment Type" + dt.Rows[i].ItemArray[4]);
                    String type = "";
                    type = dt.Rows[i].ItemArray[4].ToString();

                    if (i == 0)
                    {
                        dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[7]);
                    }
                    else if (i == 1)
                    {
                        dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[8]);
                    }
                    else
                    {
                        if (type.Equals("OnCredit"))
                        {
                            dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[7]) + Convert.ToDecimal(dt.Rows[i - 1].ItemArray[9]);
                        }
                        else
                        {
                            dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i - 1].ItemArray[9]) - Convert.ToDecimal(dt.Rows[i].ItemArray[7]);
                        }
                    }
                }
            }
            catch { }
            return dt;
        }
        private void InsertRecord(DataTable dt)
        {
            try
            {
                con.Open();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i].ItemArray[8].ToString().Equals(""))
                    {
                        dt.Rows[i].ItemArray[8] = "0.00";
                    }
                    else
                    {
                        dt.Rows[i].ItemArray[8].ToString();
                    }
                    SqlCommand cmd = new SqlCommand("INSERT INTO LedgerReceived VALUES('" + dt.Rows[i].ItemArray[0].ToString() + "','" + dt.Rows[i].ItemArray[1].ToString() + "','" + dt.Rows[i].ItemArray[2].ToString() + "','" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','" + dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "'," + dt.Rows[i].ItemArray[7].ToString() + "," + dt.Rows[i].ItemArray[8].ToString() + "," + dt.Rows[i].ItemArray[9].ToString() + ",'" + dt.Rows[i].ItemArray[10].ToString() + "','" + dt.Rows[i].ItemArray[11].ToString() + "')", con);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch { }
        }
        private void ShowDt(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Console.Write("  " + dt.Rows[i].ItemArray[j].ToString());
                }
                Console.WriteLine();
            }
        }
        #endregion LedgerUpdate

        private void OutletClient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.A)
            {
                btnNew.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.S)
            {
                btnSave.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.U)
            {
                btnupdate.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.E)
            {
                btnedit.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.P)
            {
                btnprint.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }



        //
    
    }
}
