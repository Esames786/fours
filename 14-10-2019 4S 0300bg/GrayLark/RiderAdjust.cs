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
    public partial class RiderAdjust : Form
    {
        TextBox tb = new TextBox();
        String UID = Login.UserID; String Code = "";
        public string GreenSignal = "";
        public string FormID = ""; decimal ot = 0;
        public RiderAdjust()
        {
        InitializeComponent();
        refresh();
        String get = "select ActTitle from Accounts where HeaderActCode='2' and Status='1' and ID > 24 Order BY ID DESC"; DataTable df = clsDataLayer.RetreiveQuery(get);
        if (df.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txtname, df); }
        String h3 = "select RA_Code from tbl_ReceiveAdjust";
        DataTable d3 = clsDataLayer.RetreiveQuery(h3);if (d3.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txt_search, d3);}
        btnEdit.Enabled = false;
        btnUpdate.Enabled = false;
        btnsave.Enabled = false;
        grid.Enabled = false; this.KeyPreview = true;
        }
         
        public void UserNametesting()
        {
            string query = "  select * from user_access where USENAME='" + UID + "' AND FORM_NAME='" + FormID + "'";
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

        private void New_Click(object sender, EventArgs e)
        {
        try
        {
        FormID = "Add";
        UserNametesting();
        if (GreenSignal == "YES")
        {
        grid.Enabled = true; btnsave.Enabled = true; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = false;
        grid.Rows.Clear(); grid.Rows.Add(); grid.Enabled = true;
        
        txtcode.Text = clsGeneral.getMAXCode("tbl_ReceiveAdjust", "RA_Code", "RA");
        txt_search.Text = "-"; txtname.Focus();
        }
        else
        {
            MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        }
        catch { }
        }

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
        try
        {
        if (grid.CurrentCell.ColumnIndex == 2)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // same = 1;
                grid.Rows.Add();
                int yhs = grid.CurrentCell.RowIndex;
                grid.CurrentCell = grid.Rows[yhs].Cells[0];
                grid.BeginEdit(true);
            }
        }
        else
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (grid.Rows.Count > 0)
                {
                    int yh = grid.CurrentCell.RowIndex;
                    grid.Rows.RemoveAt(yh);
                }
            }
        }
        }
        catch { }
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

        private bool CheckDataGridCells(DataGridView dgv)
        {
            bool flag = false;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (dgv.Rows[i].Cells[j].Value == null)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == true)
                {
                    break;
                }
            }
            return flag;
        }

        //RECEIVEABLE ADJUST

        private void PartyCode(String title)
        {
            String sel = "select ActCode from Accounts where ActTitle = '" + title + "'";
            DataTable dc = clsDataLayer.RetreiveQuery(sel);
            if (dc.Rows.Count > 0)
            {
                Code = dc.Rows[0][0].ToString();
            }
        }
        private void ReceiveDue(String Pname,decimal Amount,decimal old)
        {
            //
            decimal c = 0;
            PartyCode(Pname);
            decimal avv = PartyBalance();
            bool req = false;

            if (!Code.Equals("528"))
            {
                String h = "select HeaderActCode from Accounts where ActTitle = '" + Pname + "'";
                DataTable dq = clsDataLayer.RetreiveQuery(h);
                if (dq.Rows.Count > 0)
                {
                    int head = Convert.ToInt32(dq.Rows[0][0]);
                    if (head == 20101 || head == 10101)
                    {
                        req = true;
                    }
                    else
                    {
                        if (avv > 0)
                        {
                            req = true;
                        }
                        else
                        {
                            if (avv < 0)
                            {
                                req = true;
                            }
                            else
                            {
                                req = false;
                            }
                        }
                    }
                }
            }
            else { req = true; }

            if (req == true)
            {
                String seldues = "Select * from ReceiveDue where PartyCode='" + Code + "'";
                DataTable dxs = clsDataLayer.RetreiveQuery(seldues);
                if (dxs.Rows.Count > 0)
                {
                    decimal totals = Convert.ToDecimal(dxs.Rows[0]["TotalAmount"].ToString());
                    decimal dues = Convert.ToDecimal(dxs.Rows[0]["DueAmount"].ToString());
                    decimal paids = Convert.ToDecimal(dxs.Rows[0]["ReceivedAmount"].ToString());
                    decimal pays = Convert.ToDecimal(Amount);


                    if (Code.Equals("528"))
                    {
                        dues += pays;
                        totals += pays;
                        String qs = "update ReceiveDue set DueAmount=" + dues + ",TotalAmount=" + totals + " where PartyCode='" + Code + "'";
                        clsDataLayer.ExecuteQuery(qs);
                    }
                    else
                    {
                        dues =dues+old-pays;
                        paids =paids-old+pays;
                        String qs = "update ReceiveDue set DueAmount=" + dues + ",ReceivedAmount=" + paids + " where PartyCode='" + Code + "'";
                        clsDataLayer.ExecuteQuery(qs);
                    }

                }
                else
                {
                    if (Code.Equals("528"))
                    {
                        String ii = @"insert into ReceiveDue (RemCode,PartyName,PartyCode,TotalAmount,DueAmount,ReceivedAmount,Company_Name)
                values('" + txtcode.Text + "','" + Pname + "','" + Code + "'," + Amount + "," + Amount + ",0,'Delizia')";
                        clsDataLayer.ExecuteQuery(ii);
                    }
                }
            }

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
            catch { }
            return balance;

        } 
        private void ledger()
        {
            PartyCode(txtname.Text); decimal a = PartyBalance();
            decimal b = Convert.ToDecimal(txtamount.Text);
            decimal c = 0; String Status = "";
            if (!Code.Equals("528"))
            {
                String h = "select HeaderActCode from Accounts where ActTitle = '" + txtname.Text + "'";
                DataTable dq = clsDataLayer.RetreiveQuery(h);
                if (dq.Rows.Count > 0)
                {
                int head = Convert.ToInt32(dq.Rows[0][0]);
                if (head == 20101 || head == 10101)
                {
                c = a - b;
                }
                else
                {
                    if (a > 0)
                    {
                        c = a - b;
                    }
                    else
                    {
                        if (a < 0)
                        {
                            c = a - b;
                        }
                        else
                        {
                            c = 0;
                        }
                    }
                }
                }
                Status = "OnReceived";
            }
            else { c = a + b; Status = "OnCredit"; }

            String ins = @"insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
              " values('" + txtremarks.Text + "','" + txtcode.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','0101020','" + Code + "','" + txtcode.Text + "','Cash'," + txtamount.Text + ",0.00,'OnReceived'," + c + ",'Delizia') insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
              " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtremarks.Text + "','" + txtcode.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtcode.Text + "','" + txtname.Text + "'," + txtamount.Text + ",0.00,'OnReceived' ," + c + ",'Delizia')";
            if (clsDataLayer.ExecuteQuery(ins) > 0) { }
        } 
        //EXPENSE ADJUST 
       
        private void PayLedger(String Pname,decimal Amount)
        {
        PartyCode(Pname); decimal b = PartyBalance2(Pname); decimal c = b + Amount;
        String insr = @"insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
        " values('"+txtremarks.Text+"','" + txtcode.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + Code + "','" + Code + "','" + txtcode.Text + "','" + Pname + "'," + Amount + ",0.00,'OnPaid'," + c + ",'Delizia') insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
        " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('" + txtremarks.Text + "','" + txtcode.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','01010204','" + Code + "','" + txtcode.Text + "','Cash'," + Amount + ",0.00,'OnPaid' ," + c + ",'Delizia')";
        clsDataLayer.ExecuteQuery(insr);
        }

        decimal balance2 = 0;
        public decimal PartyBalance2(String Pname)
        {
            try
            {
                String h = "select * from Accounts where ActTitle = '" + Pname + "' and HeaderActCode='02'";
                DataTable dq = clsDataLayer.RetreiveQuery(h);
                if (dq.Rows.Count < 1)
                {
                    String get = "select Party_Balance from LedgerPayment where RefCode='" + Code + "' order by ID desc";
                    DataTable dfr = clsDataLayer.RetreiveQuery(get);
                    if (dfr.Rows.Count > 0) {  balance2 = decimal.Parse(dfr.Rows[0]["Party_Balance"].ToString()); }
                    else
                    {
                        balance2 = 0;
                    }
                }
                else
                {
                    balance2 = 0;
                }
            }
            catch { }
            return balance2;

        }

        // 

        private void Save()
        {
        if (!CheckAllFields() && !CheckDataGridCells(grid))
        {
            btnsave.Enabled = false;
        txtcode.Text = clsGeneral.getMAXCode("tbl_ReceiveAdjust", "RA_Code", "RA");
        PartyCode(txtname.Text); ReceiveDue(txtname.Text, Convert.ToDecimal(txtamount.Text),0); ledger();
        for (int a = 0; a < grid.Rows.Count; a++)
        {
            PayLedger(grid.Rows[a].Cells[0].Value.ToString(), Convert.ToDecimal(grid.Rows[a].Cells[1].Value.ToString()));
            String ins = "insert into tbl_ReceiveAdjust(Description,RA_Code,ReceiveableName,PartyName,Amount,Remarks,TotalAmount,Date,Username)values('"+txtremarks.Text+"','" + txtcode.Text + "','" + txtname.Text + "','" + grid.Rows[a].Cells[0].Value.ToString() + "'," + grid.Rows[a].Cells[1].Value.ToString() + ",'" + grid.Rows[a].Cells[2].Value.ToString() + "'," + txtamount.Text + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Login.UserID + "')";
        clsDataLayer.ExecuteQuery(ins);
            refresh();
        }
        MessageBox.Show("Voucher Save Successfully!","Success",MessageBoxButtons.OK,MessageBoxIcon.Information); dataGridView1.Refresh();
        grid.Enabled = false; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = true;
         
        }
        else { MessageBox.Show("Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Update()
        {
        if (!CheckAllFields() && !CheckDataGridCells(grid))
        {
        String d1 = "delete from tbl_ReceiveAdjust where RA_Code='" + txtcode.Text + "'"; clsDataLayer.ExecuteQuery(d1);
        ReceiveDue(txtname.Text, Convert.ToDecimal(txtamount.Text), ot);
        PartyCode(txtname.Text);
        int index = GetRowIndex(txtcode.Text);
        DataTable dt = SetDT();
        ShowDt(dt);
        DeleteRecord();
        dt.Rows[index][7] = Convert.ToDecimal(txtamount.Text);
        dt.Rows[index + 1][8] = Convert.ToDecimal(txtamount.Text);
        dt = SomeOperation(dt); InsertRecord(dt); con.Close();
        //
        for (int a = 0; a < grid.Rows.Count; a++)
        {
            PartyCode(grid.Rows[a].Cells[0].Value.ToString()); 
            decimal a2 = 0;   a2 = Convert.ToDecimal(grid.Rows[a].Cells[1].Value);
            int index2 = GetRowIndex2(txtcode.Text);   DataTable dt2 = SetDT2();
            ShowDt2(dt2); DeleteRecord2();
            dt2.Rows[index2][7] = a2;  dt2.Rows[index2 + 1][8] = a2;
            dt2 = SomeOperation2(dt2);  InsertRecord2(dt2);
            //
            String ins = "insert into tbl_ReceiveAdjust(Description,RA_Code,ReceiveableName,PartyName,Amount,Remarks,TotalAmount,Date,Username)values('" + txtremarks.Text + "','" + txtcode.Text + "','" + txtname.Text + "','" + grid.Rows[a].Cells[0].Value.ToString() + "'," + grid.Rows[a].Cells[1].Value.ToString() + ",'" + grid.Rows[a].Cells[2].Value.ToString() + "'," + txtamount.Text + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Login.UserID + "')";
            clsDataLayer.ExecuteQuery(ins);
        }
        refresh();  MessageBox.Show("Voucher Update Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        grid.Enabled = false; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = false; New.Enabled = true;
         
        }
        else { MessageBox.Show("Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
         
        private void btnsave_Click(object sender, EventArgs e)
        {
        try
        {   
            //
        FormID = "Save";
        UserNametesting();
        if (GreenSignal == "YES")
        {
            String getname = "select RemainingLimit,OpeningBalance from Accounts where ActTitle='" + txtname.Text + "'";
            DataTable de = clsDataLayer.RetreiveQuery(getname);
            if (de.Rows.Count < 1)
            {
                MessageBox.Show("UserName dont have a chart Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Save();
            }
        }
        else
        {
        MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        }
        catch { }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
        if (!CheckAllFields() && !CheckDataGridCells(grid))
        {
       FormID = "Update";  UserNametesting();
        if (GreenSignal == "YES")
        {
            String getname = "select RemainingLimit,OpeningBalance from Accounts where ActTitle='" + txtname.Text + "'";
            DataTable de = clsDataLayer.RetreiveQuery(getname);
            if (de.Rows.Count < 1)
            {
                MessageBox.Show("UserName dont have a chart Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Update();
            }   
        }
        else {   MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);    }
        }
        else { MessageBox.Show("Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnEdit_Click(object sender, EventArgs e){ 
                 FormID = "Edit";
        UserNametesting();
        if (GreenSignal == "YES")
        {
            grid.Enabled = true; btnsave.Enabled = false; btnEdit.Enabled = false; btnUpdate.Enabled = true; New.Enabled = false; ot = Convert.ToDecimal(txtamount.Text);
            txtremarks.Enabled = true; txtname.Enabled = false;
        }
        else { MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
         }

        private void grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
        if (grid.CurrentCell.ColumnIndex == 0)
        {
        int yy = grid.CurrentCell.ColumnIndex;
        string columnHeaders = grid.Columns[yy].HeaderText;
        if (columnHeaders.Equals("Party Name"))
        {
        tb = e.Control as TextBox;
        if (tb != null)
        {
            String get = "select ActTitle from Accounts where HeaderActCode='5' and Status='1' and ID > 24 Order BY ID DESC"; DataTable df = clsDataLayer.RetreiveQuery(get);
        if (df.Rows.Count > 0)
        {
        AutoCompleteStringCollection acsc = new AutoCompleteStringCollection(); 
        foreach (DataRow row in df.Rows) { acsc.Add(row[0].ToString()); }
        tb.AutoCompleteCustomSource = acsc;
        tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
        } 
        } 
        }
        }
        TextBox tb2 = null; tb2 = e.Control as TextBox;
        if (tb2 != null) { tb2.KeyPress += new KeyPressEventHandler(tb_KeyPress); } 
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (grid.CurrentCell.ColumnIndex == 1) //Desired Column
                {
                    if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.'))
                    { e.Handled = true; }
                    TextBox txtDecimal = sender as TextBox;
                    if (e.KeyChar == '.' && txtDecimal.Text.Contains("."))
                    {
                        e.Handled = true;
                    }
                }
            }
            catch { }
        }
         
        private void grid_KeyPress(object sender, KeyPressEventArgs e)
        {
        if (grid.CurrentCell.ColumnIndex == 1)
        {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        {   e.Handled = true; } }
        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        if (txt_search.Text == "")
        {
            refresh();
        }
        else
        { 
            String hn = "SELECT distinct(RA_Code) FROM tbl_ReceiveAdjust Where RA_Code = '" + txt_search.Text + "'";
        DataTable dt = clsDataLayer.RetreiveQuery(hn);
        if (dt.Rows.Count > 0)
        {
        dataGridView1.DataSource = null; dataGridView1.DataSource = dt;
        }
        } 
        }

        public void refresh()
        { 
            String sel = "SELECT distinct(RA_Code) FROM tbl_ReceiveAdjust"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
        if (ds.Rows.Count > 0)    {  dataGridView1.DataSource = null; dataGridView1.DataSource = ds;   } 
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
            btnsave.Enabled = false;
            btnEdit.Enabled = true;
            btnUpdate.Enabled = false;
            New.Enabled = false;
            txt_search.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
 
            String sel = "SELECT distinct(RA_Code) FROM tbl_ReceiveAdjust Where RA_Code ='" + txt_search.Text + "'"; DataTable ds = clsDataLayer.RetreiveQuery(sel);
            if (ds.Rows.Count > 0)
            {
                txtcode.Text = ds.Rows[0][0].ToString();
            }

            String vb = "select ReceiveableName,Amount from tbl_ReceiveAdjust where RA_Code='" + txt_search.Text + "'"; DataTable fd = clsDataLayer.RetreiveQuery(vb);
            if (fd.Rows.Count > 0)
            {
            txtname.Text = fd.Rows[0][0].ToString(); 
            }
            String sb = "SELECT PartyName,Amount,Remarks FROM tbl_ReceiveAdjust Where RA_Code ='" + txt_search.Text + "'";
            DataTable dt1 = clsDataLayer.RetreiveQuery(sb);
            if (dt1.Rows.Count > 0)
            {
            grid.Rows.Clear();
            foreach (DataRow row in dt1.Rows)
            {
           int n = grid.Rows.Add(); grid.Rows[n].Cells[0].Value = row[0].ToString();
           grid.Rows[n].Cells[1].Value = row[1].ToString(); grid.Rows[n].Cells[2].Value = row[2].ToString();
           }
            txtremarks.Text = dt1.Rows[0]["Remarks"].ToString();
            Total();
            String gb = "SELECT distinct(RA_Code) FROM tbl_ReceiveAdjust";
            DataTable dt = clsDataLayer.RetreiveQuery(gb);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = null; dataGridView1.DataSource = dt;
            } txtremarks.Enabled = false; txtname.Enabled = false;
            }
            } catch { } 
        }

        private void close(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void Total()
        {
            try
            {    decimal amo = 0;
                    for (int a = 0; a < grid.Rows.Count; a++)
                    {
                        amo += Convert.ToDecimal(grid.Rows[a].Cells[1].Value.ToString());
                    }
                    txtamount.Text = amo.ToString();
                 
            }
            catch { }
        }

        private void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (grid.CurrentCell.ColumnIndex == 1)
                {
                    decimal amo = 0;
                    for (int a = 0; a < grid.Rows.Count; a++)
                    {
                        amo += Convert.ToDecimal(grid.Rows[a].Cells[1].Value.ToString());
                    }
                    txtamount.Text = amo.ToString();
                }
            }
            catch { }
        }


        #region LedgerUpdate
        private int GetRowIndex(string input)
        {
            int index = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT V_No FROM LedgerReceived  where RefCode = '" + Code + "' order by ID asc", con);
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
                SqlCommand cmd = new SqlCommand("DELETE FROM LedgerReceived  where RefCode = '" + Code + "'", con);
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
                SqlDataAdapter cmd = new SqlDataAdapter("select V_No, Datetime, AccountCode, RefCode, PaymentType, InvoiceNo, Particulars, Debit, Credit, Party_Balance,Company_Name,Description from LedgerReceived  where RefCode = '" + Code + "' order by ID asc", con);
                cmd.Fill(dt);  con.Close();
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
                    if (type.Equals("OnCredit"))
                    {
                        Console.WriteLine("True");
                    }
                    else
                    {
                        Console.WriteLine("False");
                    }

                    if (i == 0)
                    {
                        if (type.Equals("OnCredit"))
                        {
                            dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[7]);
                        }
                        else
                        {
                            dt.Rows[i][9] = 0 - Convert.ToDecimal(dt.Rows[i].ItemArray[7]);
                        }
                    }
                    else if (i == 1)
                    {
                        if (type.Equals("OnCredit"))
                        {
                            dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[8]);
                        }
                        else
                        {
                            dt.Rows[i][9] = 0 - Convert.ToDecimal(dt.Rows[i].ItemArray[8]);
                        }
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

                    SqlCommand cmd = new SqlCommand("INSERT INTO LedgerReceived  VALUES('" + dt.Rows[i].ItemArray[0].ToString() + "','" + dt.Rows[i].ItemArray[1].ToString() + "','" + dt.Rows[i].ItemArray[2].ToString() + "','" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','" + dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "'," + dt.Rows[i].ItemArray[7].ToString() + "," + dt.Rows[i].ItemArray[8].ToString() + "," + dt.Rows[i].ItemArray[9].ToString() + ",'" + dt.Rows[i].ItemArray[10].ToString() + "','" + dt.Rows[i].ItemArray[11].ToString() + "')", con);
                    Console.WriteLine("Update Ledger " + cmd.CommandText);
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
        
        //
        //Ledger Update
        #region LedgerUpdate
        private int GetRowIndex2(string input)
        {
            int index = 0;
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT V_No FROM LedgerPayment where RefCode = '" + Code + "' order by ID asc", con);
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
        private void DeleteRecord2()
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM LedgerPayment where RefCode = '" + Code + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch { }
        }
        private DataTable SetDT2()
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlDataAdapter cmd = new SqlDataAdapter("select V_No, Datetime, AccountCode, RefCode, PaymentType, InvoiceNo, Particulars, Debit, Credit, Party_Balance,Company_Name,Description from LedgerPayment where RefCode = '" + Code + "' order by ID asc", con);
                cmd.Fill(dt);
                con.Close();
            }
            catch { }
            return dt;
        }
        private DataTable SomeOperation2(DataTable dt)
        {
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                { 
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
                dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[7]) + Convert.ToDecimal(dt.Rows[i - 1].ItemArray[9]);
                     }
                }
            }
            catch { }
            return dt;
        }
        private void InsertRecord2(DataTable dt)
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
                    //dt.Rows[i].ItemArray[11] = txtbname.Text; dt.Rows[i].ItemArray[12] = txtbano.Text; dt.Rows[i].ItemArray[11] = txtcheque.Text;
                    String ins = "INSERT INTO LedgerPayment VALUES('" + dt.Rows[i].ItemArray[0].ToString() + "','" + dt.Rows[i].ItemArray[1].ToString() + "','" + dt.Rows[i].ItemArray[2].ToString() + "','" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','" + dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "'," + dt.Rows[i].ItemArray[7].ToString() + "," + dt.Rows[i].ItemArray[8].ToString() + "," + dt.Rows[i].ItemArray[9].ToString() + ",'" + dt.Rows[i].ItemArray[10].ToString() + "','" + dt.Rows[i].ItemArray[11].ToString() + "')";

                    SqlCommand cmd = new SqlCommand(ins, con);
                    Console.WriteLine("Update Ledger " + cmd.CommandText);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            catch { }
        }
        private void ShowDt2(DataTable dt)
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

        private void btnprint_Click(object sender, EventArgs e)
        {
        try
        {
        String query = "";
        if (!txtcode.Text.Equals("")) { query = "select * from tbl_ReceiveAdjust where RA_Code='" + txtcode.Text + "'"; }
        else { query = "select * from tbl_ReceiveAdjust"; }
        DataTable d1 = clsDataLayer.RetreiveQuery(query);
        if (d1.Rows.Count > 0)
        {
            rptAdjust rpt = new rptAdjust(); rpt.SetDataSource(d1); PaymentView pv = new PaymentView(rpt); pv.Show();
        }
        else
        {
        MessageBox.Show("No Record Found !","Error",MessageBoxButtons.OK,MessageBoxIcon.Error); 
        }
        }
        catch{ }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear(); txtamount.Text = ""; txtname.Text = ""; btnsave.Enabled = false; New.Enabled = true;
        }

        private void txtcode_TextChanged(object sender, EventArgs e)
        {

        }

        private void RiderAdjust_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.A)
            {
                New.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.S)
            {
                btnsave.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.U)
            {
                btnUpdate.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.E)
            {
                btnEdit.PerformClick();
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
        //LedgerUpdate Closed
        //

      

    }
}
