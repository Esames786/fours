using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrayLark.bin.Debug.Report;
using GrayLark;

namespace GrayLark
{
    public partial class frmParty : Form
    {
        ActionModes actMode = ActionModes.Browsing;
        decimal duess = 0, tot = 0;
        decimal getvalue = 0;
        String UID = Login.UserID;
        public string GreenSignal = "";
        public string FormID = "";
        int a = 0;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

        public enum ActionModes
        {
            Add,
            Edit,
            Browsing
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
            con.Close();
        }
        private void SetCombos()
        {
            try
            {
                string RecQuery = "Select 0 As ID, 'Select' As ActTitle " +
                " Union All Select ID, ActTitle from  Accounts  Where HeaderActCode='010101' ";

                comboBox_RecActCode.DataSource = clsDataLayer.RetreiveQuery(RecQuery);
                comboBox_RecActCode.DisplayMember = "ActTitle";
                comboBox_RecActCode.ValueMember = "ID";

                string PayQuery = "Select 0 As ID, 'Select' As ActTitle " +
               " Union All Select ID, ActTitle from  Accounts  Where HeaderActCode='020101' ";

                comboBox_PayActCode.DataSource = clsDataLayer.RetreiveQuery(PayQuery);
                comboBox_PayActCode.DisplayMember = "ActTitle";
                comboBox_PayActCode.ValueMember = "ID";
            }
            catch { }
        }
        private void SeletionModes(ActionModes modes)
        {
            if (modes == ActionModes.Browsing)
            {
                txt_Address.Enabled = false;
                txt_CNIC.Enabled = false;
                txt_FullName.Enabled = false;
                txt_ShortName.Enabled = false;
                txt_Contact.Enabled = false;
                dgv_Master.Enabled = true;
                comboBox_PayActCode.Enabled = false;
                comboBox_RecActCode.Enabled = false;
                txt_PayActCode.Enabled = false;
                txt_RecActCode.Enabled = false;
                txt_open.Enabled = false;

                btn_Add.Enabled = true;
                btn_Edit.Enabled = true;
                btn_Delete.Enabled = false;
                btn_Save.Enabled = false;
                btn_Undo.Enabled = false;
             //   btn_Print.Enabled = false;

                SetCombos();
                setMasterGrid(clsDataLayer.RetreiveQuery(@"Select Code from  Customer order by Code Desc "));
            }
            else if (modes == ActionModes.Add)
            {
                txt_Address.Text = "";
                txt_CNIC.Text = ""; 
                txt_ShortName.Text = "";
                txt_Contact.Text = "";
                txt_PayActCode.Text = "";
                txt_RecActCode.Text = "";
                txt_open.Text = "";

               comboBox_PayActCode.Enabled = false;
               comboBox_RecActCode.Enabled = false;
             
                dgv_Master.Enabled = false;
                txt_Address.Enabled = true;
                txt_CNIC.Enabled = true;
                txt_Contact.Enabled = true;
                txt_FullName.Enabled = true;
                txt_FullName.ReadOnly =false;
                txt_ShortName.Enabled = true;
                txt_open.Enabled = true;

                btn_Add.Enabled = false;
                btn_Edit.Enabled = false;
                btn_Delete.Enabled = false;
                btn_Save.Enabled = true;
                btn_Undo.Enabled = true;
             //   btn_Print.Enabled = false;
                txt_Code.Text = clsGeneral.getMAXCode("Customer", "ID", "PC");
            }
            else if (modes == ActionModes.Edit)
            {
                txt_Address.Enabled = true;
                txt_CNIC.Enabled = true;
                txt_FullName.Enabled = true;
                txt_Contact.Enabled = true;
                dgv_Master.Enabled = false;
                txt_ShortName.Enabled = true;
                comboBox_PayActCode.Enabled = false;
                comboBox_RecActCode.Enabled = false;
                txt_PayActCode.Enabled = false;
                txt_RecActCode.Enabled = false;
                txt_open.Enabled = true;
                txt_FullName.ReadOnly = false;
                btn_Add.Enabled = false;
                btn_Edit.Enabled = false;
                btn_Delete.Enabled = false;
                btn_Save.Enabled = true;
                btn_Undo.Enabled = true;
              //  btn_Print.Enabled = false;
            }
        }
        public frmParty()
        {
            InitializeComponent();
            loadingFields();
          
            clsGeneral.SetAutoCompleteTextBox(txt_FullName, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 14 Order BY ID DESC "));
            this.KeyPreview = true;
        }
        private void loadingFields()
        {
            SeletionModes(actMode);
        }
        private void button1_Click(object sender, EventArgs e)
        {
                FormID = "Add";
                UserNametesting();
                if (GreenSignal == "YES")
                {
            actMode = ActionModes.Add;
            SeletionModes(actMode);
            txt_FullName.Focus();
            txt_ShortName.Text = "-"; txt_CNIC.Text = "0"; txt_Contact.Text = "0"; txt_Address.Text = "-";
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
        }
        private void txt_Searching_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (radiobtn_Code.Checked)
                {
                    string query = @" Select Code from Customer Where Code Like '%" + txt_Searching.Text + "%' Order By  Code Desc ";

                    setMasterGrid(clsDataLayer.RetreiveQuery(query));
                }
                else if (radiobtn_Name.Checked)
                {
                    string query = @" Select Code, FullName from Customer Where FullName Like '%" + txt_Searching.Text + "%' Order by Code Desc ";

                    setMasterGrid(clsDataLayer.RetreiveQuery(query));
                }
            }
            catch { }
        }
        private void setMasterGrid(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    dgv_Master.Rows.Clear();
                    int rownumber = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        dgv_Master.Rows.Add();
                        dgv_Master[0, rownumber].Value = row[0];
                        rownumber++;
                    }
                }
                else
                {
                    dgv_Master.Rows.Clear();
                }
            }
            catch { }
        }
        private void dgv_Master_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgv_Master.Rows.Count > 0 && dgv_Master[0, e.RowIndex].Value != null)
                {
                    string query = @"  SELECT A.ID Rec_Id, A.ActTitle Rec_ActTitle,AA.ID Pay_Id  ,
                AA.ActTitle Pay_ActTitle,
                C.Code, C.FullName, C.ShortName, C.Contact, C.CNIC, C.Address, C.Rec_ActCode, C.OpeningBalance,
                C.PayActCode,C.Company_Name FROM Customer C
				Left Join Accounts A on A.ActCode=C.Rec_ActCode
                Left Join Accounts AA On AA.ActCode=C.PayActCode
                Where Code='" + dgv_Master[0, e.RowIndex].Value.ToString() + "'";

                    DataTable dt = clsDataLayer.RetreiveQuery(query);
                    if (dt.Rows.Count > 0)
                    {
                        txt_Code.Text = dgv_Master[0, e.RowIndex].Value.ToString();
                        txt_Address.Text = dt.Rows[0]["Address"].ToString();
                        txt_CNIC.Text = dt.Rows[0]["CNIC"].ToString();
                        txt_FullName.Text = dt.Rows[0]["FullName"].ToString();
                        txt_ShortName.Text = dt.Rows[0]["ShortName"].ToString();
                        txt_Contact.Text = dt.Rows[0]["Contact"].ToString();
                        txt_RecActCode.Text = dt.Rows[0]["Rec_ActCode"].ToString();
                        txt_PayActCode.Text = dt.Rows[0]["PayActCode"].ToString();
                        txt_open.Text = dt.Rows[0]["OpeningBalance"].ToString(); 
                        if (!txt_RecActCode.Text.Equals("-"))
                        {   
                            comboBox_PayActCode.Text = "-";
                            comboBox_RecActCode.Text = txt_FullName.Text;
                        }
                        else
                        { 
                            comboBox_RecActCode.Text = "-";
                            comboBox_PayActCode.Text = txt_FullName.Text;

                        }

                        //if (dt.Rows[0]["Rec_Id"].ToString() != "")
                        //    comboBox_RecActCode.SelectedValue = int.Parse(dt.Rows[0]["Rec_Id"].ToString());

                        //if (dt.Rows[0]["Pay_Id"].ToString() != "")
                        //    comboBox_PayActCode.SelectedValue = int.Parse(dt.Rows[0]["Pay_Id"].ToString());

                       // Console.WriteLine("PayVoucher"+dt.Rows[0]["Pay_Id"].ToString());
                    }
                }
            }
            catch { }
        }
        private void btn_Edit_Click(object sender, EventArgs e)
        {
        FormID = "Edit";
        UserNametesting();
        if (GreenSignal == "YES")
        {
        actMode = ActionModes.Edit;
        SeletionModes(actMode);
        txt_FullName.Focus();
        if (txt_open.Text.Equals(""))
        {
            getvalue = 0;
        }
        else
        {
            getvalue = Convert.ToDecimal(txt_open.Text);
        }
        txt_FullName.Enabled = false; txt_FullName.ReadOnly = true;
             }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
           }
        private void btn_Undo_Click(object sender, EventArgs e)
        {
            actMode = ActionModes.Browsing;
            SeletionModes(actMode);
            getvalue = 0;
        }

        private bool CheckAllFields()
        {
            bool flag = false;
            foreach (Control o in this.tableLayoutPanel1.Controls)
            {
                if (o is TextBox)
                {
                    if (((TextBox)o).Name == "txt_Searching")
                    {
                        continue;
                    }
                    if (((TextBox)o).Text == "")
                    {
                        flag = true;
                    }
                }
                else if (o is ComboBox)
                {
                    if (((ComboBox)o).Text == "")
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }

        private void GetAmountFromRemainAccount(string input)
        {
            try
            {
                if(!txt_RecActCode.Text.Equals("-"))
                {
                    String qr = "SELECT DueAmount,TotalAmount FROM ReceiveDue WHERE PartyCode='" + input + "'";
                    DataTable de = clsDataLayer.RetreiveQuery(qr);
                    if (de.Rows.Count > 0)
                    {
                        duess = Convert.ToDecimal(de.Rows[0]["DueAmount"]);
                        tot = Convert.ToDecimal(de.Rows[0]["TotalAmount"]);
                    }
                }
                else
                {
                    String qrs = "SELECT DueAmount,TotalAmount FROM PaymentDue WHERE PartyCode='" + input + "'";
                    DataTable des = clsDataLayer.RetreiveQuery(qrs);
                    if (des.Rows.Count > 0)
                    {
                        duess = Convert.ToDecimal(des.Rows[0]["DueAmount"]);
                        tot = Convert.ToDecimal(des.Rows[0]["TotalAmount"]);
                    }
                } 
            }
            catch { }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {

           FormID = "Save";
            UserNametesting();
            if (GreenSignal == "YES")
            {
            try
            {
                if (!CheckAllFields())
                {
                    if (actMode == ActionModes.Add)
                    {
                        String getnames = "select * from Customer where FullName='" + txt_FullName.Text + "'";
                        DataTable des = clsDataLayer.RetreiveQuery(getnames);
                        if (des.Rows.Count > 0)
                        {
                            MessageBox.Show("Already Registered","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        }
                        else
                        {
                            String getname = "select * from Accounts where ActTitle='" + txt_FullName.Text + "'";
                            DataTable de = clsDataLayer.RetreiveQuery(getname);
                            if (de.Rows.Count < 1)
                            {
                                MessageBox.Show("UserName dont have a chart Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                int flag = 0;
                                if (!comboBox_RecActCode.SelectedValue.Equals("Select"))
                                {
                                    flag = 1;
                                }
                                else if (comboBox_PayActCode.SelectedIndex > 0)
                                {
                                    flag = 2;
                                }
                                String Code = "";
                                if (txt_RecActCode.Text.Equals("-"))
                                {
                                    Code = txt_PayActCode.Text;
                                }
                                else if (txt_PayActCode.Text.Equals("-"))
                                {
                                    Code = txt_RecActCode.Text;
                                }
                                String aq = "0";
                                if (txtlimit.Text.Equals("NILL"))
                                {
                                    aq = "0";
                                }
                                else
                                {
                                    aq = txtlimit.Text;
                                }
                                String upd = "update Accounts set OpeningBalance='" + txtlimit.Text + "', RemainingLimit=" + aq + " where ActCode='" + Code + "'"; clsDataLayer.ExecuteQuery(upd);
                                string query = "Insert into Customer(Email,Code,FullName,ShortName,Contact,CNIC,Address,Rec_ActCode,PayActCode,OpeningBalance,Company_Name,UserName) " +
                                " values('" + txtemail.Text + "','" + txt_Code.Text + "','" + txt_FullName.Text + "','" + txt_ShortName.Text + "','" + txt_Contact.Text + "', " +
                                "'" + txt_CNIC.Text + "','" + txt_Address.Text + "','" + txt_RecActCode.Text + "','" + txt_PayActCode.Text + "','" + txt_open.Text + "','4S','" + Login.UserID + "')";
                                if (!comboBox_RecActCode.Text.Equals("") || !comboBox_PayActCode.Text.Equals(""))
                                {
                                    if ((!txt_FullName.Text.Equals("") && !txt_ShortName.Text.Equals("")) && (!txt_Address.Text.Equals("") && !txt_open.Text.Equals("")))
                                    {
                                        if (clsDataLayer.ExecuteQuery(query) > 0)
                                        {
                                            //
                                            String Cus_Name = "Select 0 ID, 'Select' ActTitle Union All Select ID, ActTitle From Accounts Where Status='True' Order by ID desc";
                                            DataTable dte = clsDataLayer.RetreiveQuery(Cus_Name);
                                            String name = dte.Rows[0]["ActTitle"].ToString();

                                            decimal w = 0;
                                            w = decimal.Parse(txt_open.Text);
                                            decimal c = w + PartyBalance();
                                            string leddetails = @"insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                                            " values('Opening Balance','" + txt_Code.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + txt_RecActCode.Text + "','" + txt_RecActCode.Text + "','" + txt_Code.Text + "','" + txt_FullName.Text + "'," + txt_open.Text + ",0.00,'OnCredit'," + txt_open.Text + ",'4S') insert into LedgerReceived(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                                            " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('Opening Balance','" + txt_Code.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','01010200','" + txt_RecActCode.Text + "','" + txt_Code.Text + "','OpeningBalance'," +
                                            txt_open.Text + ",0.00,'OnCredit' ," + txt_open.Text + ",'4S')";

                                            if (!txt_RecActCode.Text.Equals("-"))
                                            {
                                                if (clsDataLayer.ExecuteQuery(leddetails) > 0)
                                                {
                                                    getremaining();
                                                }
                                            }
                                            else
                                            {
                                                decimal ws = 0;
                                                ws = decimal.Parse(txt_open.Text);
                                                decimal cs = ws + PartyBalances();
                                                string leddetailss = @"insert into LedgerPayment(Description,V_No,Datetime,RefCode,AccountCode,InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance,Company_Name) " +
                                                " values('Opening Balance','" + txt_Code.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + txt_PayActCode.Text + "','01010200','" + txt_Code.Text + "','OpeningBalance'," + txt_open.Text + ",0.00,'OnPayable'," + txt_open.Text + ",'4S') insert into LedgerPayment(Description,V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                                                " Particulars,Credit,Debit,PaymentType,Party_Balance,Company_Name) values('Opening Balance','" + txt_Code.Text + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + txt_PayActCode.Text + "','" + txt_PayActCode.Text + "','" + txt_Code.Text + "','" + txt_FullName.Text + "'," +
                                                txt_open.Text + ",0.00,'OnPayable' ," + txt_open.Text + ",'4S')";
                                                if (clsDataLayer.ExecuteQuery(leddetailss) > 0)
                                                {
                                                    getremainings();
                                                }
                                            }
                                        }

                                        MessageBox.Show("Record Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                        actMode = ActionModes.Browsing;
                                        SeletionModes(actMode);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Action Rollback", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Please Fill All The TextField", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }

                    }
                    else if (actMode == ActionModes.Edit)
                    {
                        String get = "";
                        if (!txt_RecActCode.Text.Equals("-"))
                        {
                            GetAmountFromRemainAccount(txt_RecActCode.Text);
                            get = txt_RecActCode.Text;
                        }
                        else
                        {
                            GetAmountFromRemainAccount(txt_PayActCode.Text);
                            get = txt_PayActCode.Text;
                        }
                        decimal duee = 0, total = 0;
                        if (txt_open.Text.Equals(""))
                        {
                            duee = 0;
                            total = 0;
                        }
                        else
                        {
                            duee = (duess - getvalue + Convert.ToDecimal(txt_open.Text));
                            total = (tot - getvalue + Convert.ToDecimal(txt_open.Text));
                        }

                        string query = "UPDATE Customer set FullName='" + txt_FullName.Text + "',Email='"+txtemail.Text+"',ShortName='" + txt_ShortName.Text + "'," +
                        " Contact='" + txt_Contact.Text + "',CNIC='" + txt_CNIC.Text + "',Address='" + txt_Address.Text + "', " +
                        " Rec_ActCode='" + txt_RecActCode.Text + "',PayActCode='" + txt_PayActCode.Text + "' , OpeningBalance=" + txt_open.Text + " , UserName='" + Login.UserID + "' where Code= '" + txt_Code.Text + "' ";

                        if (clsDataLayer.ExecuteQuery(query) > 0)
                        {

                        }
                        if (!txt_RecActCode.Text.Equals("-"))
                        {
                            con.Open();
                            SqlCommand cmd = new SqlCommand("UPDATE ReceiveDue SET TotalAmount='" + total.ToString() + "', DueAmount='" + duee.ToString() + "' WHERE PartyCode='" + get + "'", con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        else
                        {
                            con.Open();
                            SqlCommand cmd = new SqlCommand("UPDATE PaymentDue SET TotalAmount='" + total.ToString() + "', DueAmount='" + duee.ToString() + "' WHERE PartyCode='" + get + "'", con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }

                        //if ((!txt_FullName.Text.Equals("") && !txt_ShortName.Text.Equals("")) && (!txt_Address.Text.Equals("") && !txt_open.Text.Equals("")) && (comboBox_RecActCode.SelectedIndex != 0 || comboBox_PayActCode.SelectedIndex != 0))
                        //{
                            if (clsDataLayer.ExecuteQuery(query) > 0)
                            {
                                  int index = GetRowIndex(txt_Code.Text);
                                DataTable dt = SetDT();
                                ShowDt(dt);
                                Console.WriteLine("Index=" + index);
                                DeleteRecord();
                                dt.Rows[index][7] = Convert.ToDecimal(txt_open.Text);
                                dt.Rows[index + 1][8] = Convert.ToDecimal(txt_open.Text);
                                dt = SomeOperation(dt);
                                InsertRecord(dt);

                                MessageBox.Show("Customer Detail Update Success!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                actMode = ActionModes.Browsing;
                                SeletionModes(actMode);
                            }
                            else
                            {
                                MessageBox.Show("Action Rollback", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        //}
                        //else
                        //{
                        //    MessageBox.Show("please Fill all field");
                        //}
                    }
                }
                else
                {
                    MessageBox.Show("please Fill all field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e5) { MessageBox.Show(e5.Message); }
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
        }
        private void comboBox_RecActCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actMode == ActionModes.Add || actMode == ActionModes.Edit)
            {
                string recQuery = " Select  ActCode from  Accounts  Where ID=" + comboBox_RecActCode.SelectedValue + " And HeaderActCode='010101'";
                DataTable dt = clsDataLayer.RetreiveQuery(recQuery);
                if (dt.Rows.Count > 0)
                {
                    txt_RecActCode.Text = dt.Rows[0][0].ToString();
                }
                else
                {
                    txt_RecActCode.Text = "";
                }
            }
        }
        private void comboBox_PayActCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actMode == ActionModes.Add || actMode == ActionModes.Edit)
            {
                string payQuery = "Select ActCode from  Accounts  Where ID=" + comboBox_PayActCode.SelectedValue + " And HeaderActCode='020101'";
                DataTable dt = clsDataLayer.RetreiveQuery(payQuery);
                if (dt.Rows.Count > 0)
                {
                    txt_PayActCode.Text = dt.Rows[0][0].ToString();
                }
                else
                {
                    txt_PayActCode.Text = "";
                }
            }
        }
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch { }
        }
        private void txt_CNIC_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txt_CNIC.Text, "[^0-9]"))
            {
                ToolTip toltip = new ToolTip();
                toltip.SetToolTip(txt_CNIC, "Enter Correct Format");
                txt_CNIC.Text = txt_CNIC.Text.Remove(txt_CNIC.Text.Length - 1);
            }
        } 
        private void txt_Contact_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txt_Contact.Text, "[^0-9]"))
            {
                ToolTip toltip = new ToolTip();
                toltip.SetToolTip(txt_Contact, "Enter Correct Format");
                txt_Contact.Text = txt_Contact.Text.Remove(txt_Contact.Text.Length - 1);
            } 
        }


        decimal balance = 0;
        public decimal PartyBalance()
        {
            try
            {
                String get = "select Party_Balance from LedgerReceived where RefCode='" + txt_FullName.Text + "' order by ID desc";
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

        public decimal PartyBalances()
        {
            try
            {
                String get = "select Party_Balance from LedgerPayment where RefCode='" + txt_FullName.Text + "' order by ID desc";
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

        private int GetRowIndex(string input)
        {
            int index = 0;
            try
            {
                con.Open();
                 String tbl = "";
                if (!txt_RecActCode.Text.Equals("-"))
                {
                    tbl = "SELECT V_No FROM LedgerReceived where RefCode = '" + txt_RecActCode.Text + "' order by ID asc";
                }
                else
                {
                    tbl = "SELECT V_No FROM LedgerPayment where RefCode = '" + txt_RecActCode.Text + "' order by ID asc";
                }
                SqlCommand cmd = new SqlCommand(tbl, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetValue(0).ToString() == input)
                    {
                        break;
                    }
                    index++;
                    Console.WriteLine("Index Is " + index);
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
                String tbl = "";
                if(!txt_RecActCode.Text.Equals("-"))
                {
               tbl = "DELETE FROM LedgerReceived where RefCode = '" + txt_RecActCode.Text + "'";
                }
                else
                {
                 tbl = "DELETE FROM LedgerPayment where RefCode = '" + txt_PayActCode.Text + "'";
                }
                con.Open();
                SqlCommand cmd = new SqlCommand(tbl, con);
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
                String tbl = "";
                if (!txt_RecActCode.Text.Equals("-"))
                {
                    tbl = "select V_No, Datetime, AccountCode, RefCode, PaymentType, InvoiceNo, Particulars, Debit, Credit, Party_Balance,Company_Name,Description from LedgerReceived where RefCode = '" + txt_RecActCode.Text + "' order by ID asc";
                }
                else
                {
                    tbl = "select V_No, Datetime, AccountCode, RefCode, PaymentType, InvoiceNo, Particulars, Debit, Credit, Party_Balance,Company_Name,Description from LedgerPayment where RefCode = '" + txt_PayActCode.Text + "' order by ID asc";
                }
                con.Open();
                SqlDataAdapter cmd = new SqlDataAdapter(tbl, con);
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
                  String type = "";
                    type = dt.Rows[i].ItemArray[4].ToString();
                    //if (type.Equals("OnCredit"))
                    //{
                    //    Console.WriteLine("True");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("False");
                    //}
                    String tbl = "";
                    if (!txt_RecActCode.Text.Equals("-"))
                    {
                        tbl = "OnCredit";
                    }
                    else
                    {
                        tbl = "OnPayable";
                    }
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
                        if (type.Equals(tbl))
                        {
                            dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[7]) + Convert.ToDecimal(dt.Rows[i - 1].ItemArray[9]);
                        }
                        else
                        {
                            dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i-1].ItemArray[9]) - Convert.ToDecimal(dt.Rows[i].ItemArray[7]);
//                            dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[9]) - Convert.ToDecimal(dt.Rows[i - 1].ItemArray[7]);

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
                    String tbl = "";
                    if (!txt_RecActCode.Text.Equals("-"))
                    {  tbl = "LedgerReceived"; }
                    else
                    {
                        tbl = "LedgerPayment";
                    }
                    //String qs="INSERT INTO " + tbl + " (V_No, Datetime, AccountCode, RefCode, PaymentType, InvoiceNo, Particulars, Debit, Credit, Party_Balance,Company_Name) VALUES('" + dt.Rows[i].ItemArray[0].ToString() + "','" + dt.Rows[i].ItemArray[1].ToString() + "','" + dt.Rows[i].ItemArray[2].ToString() + "','" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','" + dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "'," + dt.Rows[i].ItemArray[7].ToString() + "," + dt.Rows[i].ItemArray[8].ToString() + "," + dt.Rows[i].ItemArray[9].ToString() + "," + dt.Rows[i].ItemArray[10].ToString() + ",'" + dt.Rows[i].ItemArray[11].ToString() + "')";
                    //MessageBox.Show(qs);
                    //SqlCommand cmd = new SqlCommand(qs, con);
                    //Console.WriteLine("Update Ledger " + cmd.CommandText);
                    //cmd.ExecuteNonQuery();


                    String ins = "INSERT INTO " + tbl + " VALUES('" + dt.Rows[i].ItemArray[0].ToString() + "','" + dt.Rows[i].ItemArray[1].ToString() + "','" + dt.Rows[i].ItemArray[2].ToString() + "','" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','" + dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "'," + dt.Rows[i].ItemArray[7].ToString() + "," + dt.Rows[i].ItemArray[8].ToString() + "," + dt.Rows[i].ItemArray[9].ToString() + ",'" + dt.Rows[i].ItemArray[10].ToString() + "','" + dt.Rows[i].ItemArray[11].ToString() + "')";
                    SqlCommand cmd = new SqlCommand(ins, con);
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
                   // Console.Write("  " + dt.Rows[i].ItemArray[j].ToString());
                }
               // Console.WriteLine();
            }
        }
         
        private void getremaining()
        {
            try
            {
                string chkledger ="select * from ReceiveDue where PartyCode='" + txt_RecActCode.Text + "' ";
                DataTable dv = clsDataLayer.RetreiveQuery(chkledger);
                if (dv.Rows.Count < 1)
                {
                    string blncledger = "insert into ReceiveDue (RemCode,PartyName,PartyCode,TotalAmount,DueAmount,ReceivedAmount,Company_Name)values('" + txt_Code.Text + "','" + txt_FullName.Text + "','" + txt_RecActCode.Text + "'," + txt_open.Text + "," + txt_open.Text + ",0,'4S')";
                    if (clsDataLayer.ExecuteQuery(blncledger) > 0)
                    {
                      //  MessageBox.Show("Add Remaing");

                    } 
                    else
                    {
                        //MessageBox.Show(blncledger + "Cant Inserted");
                        //Console.WriteLine(blncledger + "check");
                    }
                }
                else
                {
                    decimal dd = 0;
                    decimal rec = 0;
                    decimal billamount = 0;
                    string oldrow = "select * from ReceiveDue where PartyCode = '" + txt_RecActCode.Text + "' ";
                    DataTable df = clsDataLayer.RetreiveQuery(oldrow);
                    decimal total = decimal.Parse(df.Rows[0]["TotalAmount"].ToString());
                    decimal due = decimal.Parse(df.Rows[0]["DueAmount"].ToString());
                    decimal received = decimal.Parse(df.Rows[0]["ReceivedAmount"].ToString());
                    billamount = decimal.Parse(txt_open.Text);

                    total += billamount;
                    due += billamount;

                    string updateblnc = "update ReceiveDue set TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + txt_RecActCode.Text + "' ";
                    if (clsDataLayer.ExecuteQuery(updateblnc) > 0)
                    {
                        MessageBox.Show("Remain Update");

                    }
                }
            }
            catch { }

        }

        private void getremainings()
        {
            try
            {
                string chkledger = "select * from PaymentDue where PartyCode='" + txt_PayActCode.Text + "' ";
                DataTable dv = clsDataLayer.RetreiveQuery(chkledger);
                if (dv.Rows.Count < 1)
                {
                    string blncledger = "insert into PaymentDue (PayCode,PartyName,PartyCode,TotalAmount,DueAmount,PaidAmount,Company_Name)values('" + txt_Code.Text + "','" + txt_FullName.Text + "','" + txt_PayActCode.Text + "'," + txt_open.Text + "," + txt_open.Text + ",0,'4S')";
                    if (clsDataLayer.ExecuteQuery(blncledger) > 0)
                    {
                        //  MessageBox.Show("Add Remaing"); 
                    }
                    else
                    { //MessageBox.Show(blncledger + "Cant Inserted");   //Console.WriteLine(blncledger + "check");  }
                    }
                }
                else
                {
                    decimal dd = 0;
                    decimal rec = 0;
                    decimal billamount = 0;
                    string oldrow = "select * from PaymentDue where PartyCode = '" + txt_PayActCode.Text + "' ";
                    DataTable df = clsDataLayer.RetreiveQuery(oldrow);
                    decimal total = decimal.Parse(df.Rows[0]["TotalAmount"].ToString());
                    decimal due = decimal.Parse(df.Rows[0]["DueAmount"].ToString());
                    decimal received = decimal.Parse(df.Rows[0]["PaidAmount"].ToString());
                    billamount = decimal.Parse(txt_open.Text);

                    total += billamount;
                    due += billamount;

                    string updateblnc = "update PaymentDue set   TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where PartyCode='" + txt_PayActCode.Text + "' ";
                    if (clsDataLayer.ExecuteQuery(updateblnc) > 0)
                    {
                        MessageBox.Show("Remain Update","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);

                    }
                }
            }
            catch { }

        }

        public void decreaseUpdatevalue()
        {
            try
            { 
                decimal billamount = 0;
                string oldrow = "select * from tbl_RemainingAmount where RefCode = '" + txt_RecActCode.Text + "' ";
                DataTable df = clsDataLayer.RetreiveQuery(oldrow);
                decimal total = decimal.Parse(df.Rows[0]["TotalAmount"].ToString());
                decimal due = decimal.Parse(df.Rows[0]["DueAmount"].ToString());
                decimal received = decimal.Parse(df.Rows[0]["ReceivedAmount"].ToString());
                billamount = decimal.Parse(txt_open.Text);

                total -= billamount;
                due -= billamount;

                string updateblnc = "update tbl_RemainingAmount set Dates='" + DateTime.Now.ToString("dd-MM-yyyy") + "', TotalAmount=" + total.ToString() + ", DueAmount=" + due.ToString() + " where RefCode='" + txt_RecActCode.Text + "' ";
                if (clsDataLayer.ExecuteQuery(updateblnc) > 0)
                {
                    MessageBox.Show("change Update");

                }
            }
            catch
            {  }
        }

        private void abcd(object sender, KeyPressEventArgs e)
        {
         //   e.Handled = !char.IsDigit(e.KeyChar); 
        }
        String headcode = "";
        private void CodeFind(object sender, EventArgs e)
        {
            String hm = "select ShortTitle from Accounts where ActTitle='"+txt_FullName.Text+"'";
            DataTable d1 = clsDataLayer.RetreiveQuery(hm); if (d1.Rows.Count > 0) { txt_ShortName.Text = d1.Rows[0][0].ToString(); }
            if (actMode == ActionModes.Add || actMode == ActionModes.Edit)
            {
                string recQuery = " Select  ActCode,HeaderActCode from  Accounts  Where ActTitle = '"+txt_FullName.Text+"' ";
                DataTable dt = clsDataLayer.RetreiveQuery(recQuery);
                if (dt.Rows.Count > 0)
                {
                    headcode = dt.Rows[0][1].ToString();
                    if (headcode.Equals("10101"))
                    {
                        txt_RecActCode.Text = dt.Rows[0][0].ToString();
                        comboBox_RecActCode.Text = txt_FullName.Text;
                        txt_PayActCode.Text = "-";
                        comboBox_PayActCode.Text ="-";
                        comboBox_RecActCode.Enabled = true;
                        comboBox_PayActCode.Enabled = false;
                    }
                    else
                    {
                        txt_PayActCode.Text = dt.Rows[0][0].ToString();
                        comboBox_PayActCode.Text = txt_FullName.Text;
                        txt_RecActCode.Text = "-";
                        comboBox_RecActCode.Text ="-";
                        comboBox_RecActCode.Enabled = false;
                        comboBox_PayActCode.Enabled = true;
                    }
                }
                else
                {
                    txt_RecActCode.Text = "";
                }
            }
        }

        private void txt_Address_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else
            {
                e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
            }
        }

        private void txt_FullName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else
            {
                e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
            }
        }

        private void txt_ShortName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else
            {
                e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
            }
        }
        private void radiobtn_Name_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radiobtn_Code.Checked)
                {
                    string query = @" Select Code from Customer Order By  Code Desc ";

                    DataTable ds = clsDataLayer.RetreiveQuery(query); if (ds.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txt_Searching, ds); }
                }
                else if (radiobtn_Name.Checked)
                {
                    string query = @" Select Code, FullName from Customer Order by Code Desc ";
                    DataTable ds = clsDataLayer.RetreiveQuery(query); if (ds.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txt_Searching, ds); }
                }
            }
            catch { }
        }

        private void Close(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void frmParty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.A)
            {
                btn_Add.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.S)
            {
                btn_Save.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.E)
            {
                btn_Edit.PerformClick();
            }
        } 
          
    }
}
