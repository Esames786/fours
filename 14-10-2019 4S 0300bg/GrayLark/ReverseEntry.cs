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

namespace Reddot_Express_Inventory
{
    public partial class ReverseEntry : Form
    {
        ActionModes actMode = ActionModes.Browsing;
        String Particulara = "";
        String Particularb = "";
        String Particularc = "";
        decimal getvalue = 0,duess=0,tot=0;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

        decimal TotalAmounts;
        public DataGridViewRow dataRow = new DataGridViewRow();
        int[] DetailID = null;
        public enum ActionModes
        {
            Add,
            Edit,
            Browsing
        }
        private void SeletionModes(ActionModes modes)
        {
            if (modes == ActionModes.Browsing)
            {
                txt_Code.Enabled = true;
                txt_VType.Enabled = false;
                dtp_Date.Enabled = false;
                txt_CashActCode.Enabled = false;
                txt_CashActTitle.Enabled = false;
                txt_inv_type.Enabled = false;
                txt_InvoiceNo.Enabled = false;
                txt_Narration.Enabled = false;
                txt_PartyActCode.Enabled = false;
                txt_RecAmount.Enabled = false;
                txt_Remarks.Enabled = false;
                txt_TotalAmount.Enabled = false;
                txt_VType.Enabled = false;
                txt_Bill_Amount.Enabled = false;
                comboBox_PartyAccount.Enabled = false;

                dgv_Master.Enabled = true;

                btn_SelectInvoice.Enabled = false;

                btn_Add.Enabled = true;
                btn_Edit.Enabled = true;
                btn_Delete.Enabled = false;
                btn_Save.Enabled = false;
                btn_Undo.Enabled = false;
                btn_Print.Enabled = true;
                setMasterGrid(clsDataLayer.RetreiveQuery(@" SELECT  V_No, AccountCode FROM ReverseView "));
                SetCombo();
            }
            else if (modes == ActionModes.Add)
            {
                txt_Code.Text = "";
                txt_VType.Text = "";
                dtp_Date.Text = "";
                txt_CashActCode.Text = "";
                txt_CashActTitle.Text = "";
                txt_inv_type.Text = "";
                txt_InvoiceNo.Text = "";
                txt_Narration.Text = "";
                txt_PartyActCode.Text = "";
                txt_RecAmount.Text = "";
                txt_Remarks.Text = "";
                txt_TotalAmount.Text = "";
                txt_VType.Text = "";
                txt_Bill_Amount.Text = "";
                txt_Code.Text = clsGeneral.getMAXCode("CRMaster", "ID", "CR");
                comboBox_PartyAccount.Text = "";
                txt_VType.Text = "CR";

                SetCashAccounts();


                txt_Code.Enabled = false;
                txt_VType.Enabled = false;
                dtp_Date.Enabled = true;
                txt_CashActCode.Enabled = false;
                txt_CashActTitle.Enabled = false;
                txt_inv_type.Enabled = false;
                txt_InvoiceNo.Enabled = true;
                txt_Narration.Enabled = true;
                txt_PartyActCode.Enabled = false;
                txt_RecAmount.Enabled = true;
                txt_Remarks.Enabled = true;
                txt_TotalAmount.Enabled = true;
                txt_VType.Enabled = false;
                comboBox_PartyAccount.Enabled = true;

                dgv_Master.Enabled = false;

                btn_SelectInvoice.Enabled = true;

                btn_Add.Enabled = false;
                btn_Edit.Enabled = false;
                btn_Delete.Enabled = false;
                btn_Save.Enabled = true;
                btn_Undo.Enabled = true;
                btn_Print.Enabled = false;
            }
            else if (modes == ActionModes.Edit)
            {
                txt_Code.Enabled = false;
                txt_VType.Enabled = false;
                dtp_Date.Enabled = true;
                txt_CashActCode.Enabled = false;
                txt_CashActTitle.Enabled = false;
                txt_inv_type.Enabled = false;
                txt_InvoiceNo.Enabled = true;
                txt_Narration.Enabled = true;
                txt_PartyActCode.Enabled = false;
                txt_RecAmount.Enabled = true;
                txt_Remarks.Enabled = true;
                txt_TotalAmount.Enabled = true;
                txt_VType.Enabled = false;
                comboBox_PartyAccount.Enabled = true;

                dgv_Master.Enabled = false;

                btn_SelectInvoice.Enabled = true;

                btn_Add.Enabled = false;
                btn_Edit.Enabled = false;
                btn_Delete.Enabled = false;
                btn_Save.Enabled = true;
                btn_Undo.Enabled = true;
                btn_Print.Enabled = false;

            }
        }

        private void setMasterGrid(DataTable dt)
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
        private void SetCombo()
        {
        
        }

        private void SetCashAccounts()
        {
            DataTable dt = clsDataLayer.RetreiveQuery("  Select ActCode, ActTitle From Accounts Where ActCode='01010201' ");
            if (dt.Rows.Count > 0)
            {
                txt_CashActCode.Text = dt.Rows[0]["ActCode"].ToString();
                txt_CashActTitle.Text = dt.Rows[0]["ActTitle"].ToString();
            }
        }
        public ReverseEntry()
        {
            InitializeComponent();
            loadingFields();
            clsGeneral.SetAutoCompleteTextBox(comboBox_PartyAccount, clsDataLayer.RetreiveQuery(" select ActTitle from Accounts where HeaderActCode='010101' Order BY ID DESC "));

            dtp_Date.Value = DateTime.Now;
           
        }
        private void loadingFields()
        {
            SeletionModes(actMode);
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            actMode = ActionModes.Add;
            SeletionModes(actMode);
            dtp_Date.Focus();
        }

        private void GetAmountFromRemainAccount(String input)
        {
            try
            {
                String qr = "SELECT DueAmount,TotalAmount FROM tbl_RemainingAmount WHERE RefCode='" + input + "'";
                DataTable de = clsDataLayer.RetreiveQuery(qr);
                if (de.Rows.Count > 0)
                {
                    duess = Convert.ToDecimal(de.Rows[0]["DueAmount"]);
                    tot = Convert.ToDecimal(de.Rows[0]["TotalAmount"]);
                }

            }
            catch { }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {

                if (actMode == ActionModes.Add)
                {
                    string query = @"INSERT INTO CRMaster( V_No, V_Type, Date, Account_Code, Amount, Remarks,  " +
                    " CreatedBy, CreatedOn, ModifiedBy, ModifiedOn) " +
                    " VALUES ('" + txt_Code.Text + "','" + txt_VType.Text + "','" + dtp_Date.Value.ToString("dd-MM-yyyy") + "', " +
                    " '" + txt_PartyActCode.Text + "'," + txt_TotalAmount.Text + ",'" + txt_Remarks.Text + "'," + Login.UserID + ",getdate()," +
                    " 0,' ') ";
                    if ((!txt_TotalAmount.Text.Equals("") && !txt_Remarks.Text.Equals("")))
                    {
                        if (clsDataLayer.ExecuteQuery(query) > 0)
                        {
                            string DetailQuery = "";

                            DetailQuery = "INSERT INTO CRDetail(V_No,  AccountCode,ActTitle, Narration, Inv_No, Inv_Type, Inv_Amount, Rec_Amount) " +
                           "VALUES('" + txt_Code.Text + "', '" + txt_PartyActCode.Text + "','" + comboBox_PartyAccount.Text + "','" + txt_Narration.Text + "', " +
                           " '" + txt_InvoiceNo.Text + "','" + txt_inv_type.Text + "'," + txt_Bill_Amount.Text + ", " +
                            txt_RecAmount.Text + ")";

                            if (clsDataLayer.ExecuteQuery(DetailQuery) > 0)
                            {
                                // label_Message.Text = "Record Saved Successfully";
                                decimal w = 0;
                                w = decimal.Parse(txt_RecAmount.Text);
                                decimal c = w + PartyBalance();
                                Console.WriteLine("W is  + " + w + "   " + "Balance Is " + PartyBalance());




                                Particulara = "OnCredit";

                                String leddetails = @"insert into tbl_Ledger(V_No,Datetime,AccountCode,RefCode, " +
                               " InvoiceNo,Particulars,Debit,Credit,PaymentType,Party_Balance)  " +
                               " values('" + txt_Code.Text + "' , '" + dtp_Date.Value.ToString("dd-MM-yyyy") + "' ,'" + txt_PartyActCode.Text + "', '" + txt_PartyActCode.Text + "'," +
                               " '" + txt_InvoiceNo.Text + "','" + comboBox_PartyAccount.Text + "'," + txt_RecAmount.Text + ",0.00,'" + Particulara + "'," + c + ") " + " insert into tbl_Ledger(V_No,Datetime,AccountCode, RefCode," +
                               " InvoiceNo,Particulars,Credit,Debit,PaymentType,Party_Balance) " +
                               " values('" + txt_Code.Text + "' , '" + dtp_Date.Value.ToString("dd-MM-yyyy") + "' ,'" + txt_CashActCode.Text + "', '" + txt_PartyActCode.Text + "'," +
                               " '" + txt_InvoiceNo.Text + "','Cash Book'," + txt_RecAmount.Text + ",0.00,'" + Particulara + "'," + c + ") ";

                                string ad = @"insert into tbl_Ledger(V_No,Datetime,AccountCode,PaymentType,RefCode,InvoiceNo,Particulars,Debit,Credit,Party_Balance) " +
                           " values('" + txt_Code.Text + "','" + dtp_Date.Value.ToString("dd-MM-yyyy") + "','" + txt_PartyActCode.Text + "','OnCredit','" + txt_PartyActCode.Text + "','" + txt_Code.Text + "','" + comboBox_PartyAccount.Text + "'," +
                           txt_RecAmount.Text + " , 0.00," + c + ") insert into tbl_Ledger(V_No,Datetime,AccountCode,RefCode,InvoiceNo, " +
                           " Particulars,Credit,Debit,PaymentType,Party_Balance) values('" + txt_Code.Text + "','" + dtp_Date.Value.ToString("dd-MM-yyyy") + "','" +
                           txt_CashActCode.Text + "','" + txt_PartyActCode.Text + "','" + txt_Code.Text + "','" + txt_CashActTitle.Text + "'," +
                           txt_RecAmount.Text + ",0.00,'OnCredit'," + c + ")";

                                if (clsDataLayer.ExecuteQuery(ad) > 0)
                                {
                                    remain();
                                    updatebill();
                                }
                            }


                        }
                        MessageBox.Show("Record Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        actMode = ActionModes.Browsing;
                        SeletionModes(actMode);
                    }

                    else
                    {
                        MessageBox.Show("Fill All The TextFields");

                    }
                }
                else if (actMode == ActionModes.Edit)
                {

                    GetAmountFromRemainAccount(txt_PartyActCode.Text);

                    decimal duee = 0, total = 0;
                    if (txt_RecAmount.Text.Equals(""))
                    {
                        duee = 0;
                        total = 0;
                    }
                    else
                    {
                        duee = (duess - getvalue + Convert.ToDecimal(txt_RecAmount.Text));
                        total = (tot - getvalue + Convert.ToDecimal(txt_RecAmount.Text));
                    }

                    String quer = "UPDATE tbl_RemainingAmount SET Dates='" + DateTime.Now.ToString("dd-MM-yyyy") + "', TotalAmount='" + total.ToString() + "', DueAmount='" + duee.ToString() + "' WHERE RefCode='" + txt_PartyActCode.Text + "'";
                    clsDataLayer.ExecuteQuery(quer);

                    string query = @"Update CRMaster Set Date='" + dtp_Date.Value.ToString("dd-MM-yyyy") + "', Account_Code='" + txt_CashActCode.Text + "', " +
                    " Amount=" + txt_TotalAmount.Text + ",Remarks='" + txt_Remarks.Text + "', ModifiedBy=" + Login.UserID + ", " +
                    " ModifiedOn=getdate() Where V_No='" + txt_Code.Text + "'";

                    if (clsDataLayer.ExecuteQuery(query) > 0)
                    {
                        if (!txt_TotalAmount.Text.Equals(""))
                        {

                            if (!txt_PartyActCode.Text.Equals("") && !txt_RecAmount.Text.Equals("") && !txt_Bill_Amount.Text.Equals(""))
                            {
                                string DetailQuery = "";
                                DetailQuery = "Update CRDetail Set AccountCode= '" + txt_PartyActCode.Text + "', " +
                                 "Narration='" + txt_Narration.Text + "', Inv_No='" + txt_InvoiceNo.Text + "', " +
                                 " Inv_Type='" + txt_inv_type.Text + "', Inv_Amount=" + txt_Bill_Amount.Text + ", " +
                                 " Rec_Amount=" + txt_RecAmount.Text + " , ActTitle='" + comboBox_PartyAccount.Text + "' Where  V_No='" + txt_Code.Text + "'";

                                if (clsDataLayer.ExecuteQuery(DetailQuery) > 0)
                                {
                                    updatebill();

                                    //int index = RowNumber();
                                    //DataTable dt = SetDT();
                                    //ShowDt(dt);
                                    //Console.WriteLine("Index=" + index);
                                    //DeleteRecord();
                                    //dt.Rows[index][7] = Convert.ToDecimal(txt_RecAmount.Text);
                                    //dt.Rows[index + 1][8] = Convert.ToDecimal(txt_RecAmount.Text);
                                    //dt = SomeOperation(dt);
                                    //InsertRecord(dt);

                                    int index = GetRowIndex(txt_Code.Text);
                                    DataTable dt = SetDT();
                                    ShowDt(dt);
                                    Console.WriteLine("Index=" + index);
                                    DeleteRecord();
                                    dt.Rows[index][7] = Convert.ToDecimal(txt_RecAmount.Text);
                                    dt.Rows[index + 1][8] = Convert.ToDecimal(txt_RecAmount.Text);
                                    dt = SomeOperation(dt);
                                    InsertRecord(dt);
                                }


                                MessageBox.Show("Record Update Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                actMode = ActionModes.Browsing;
                                SeletionModes(actMode);
                            }
                            else
                            {
                                MessageBox.Show("Action Rollback", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                    }

                }
            }
            catch { }
        }

     




        private void btn_SelectInvoice_Click(object sender, EventArgs e)
        {
            string Selectquery = @" SELECT   tbl_CustomerBilling.ID,   tbl_CustomerBilling.Dates, tbl_CustomerBilling.BNCode BillNo,
            tbl_CustomerBilling.DeliveryChallanCode DeliveryNo, tbl_CustomerBilling.IsPaid,tbl_CustomerBilling.Amount BillAmount,ISNULL( tbl_CustomerBilling.RecAmount,0) ExtraAmount, 
            tbl_CustomerBilling.Amount + ISNULL(tbl_CustomerBilling.RecAmount,0) TotalAmount
            FROM  tbl_CustomerBilling 
            Where tbl_CustomerBilling.PartyActCode='" + txt_PartyActCode.Text + "' And tbl_CustomerBilling.IsPaid='UnPaid' ";

            //frmSelectInvoice inv = new frmSelectInvoice(this, Selectquery);
            //inv.ShowDialog();
        }
        //CLEAR BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            clear();
        }
        private void clear()
        {
            txt_RecAmount.Text = "";
            txt_Bill_Amount.Text = "";
            txt_InvoiceNo.Text = "";
            txt_Narration.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        public void updatebill()
        {
            //String billupd = "update tbl_CustomerBilling set RecAmount =" + txt_RecAmount.Text + " where BNCode='" + txt_InvoiceNo.Text + "'";
            //if (clsDataLayer.ExecuteQuery(billupd) > 0)
            //{
            //    MessageBox.Show("BillUpdated");
            //}
        }
        private void TotalAmount()
        {
           
            try
            {
                if (!txt_RecAmount.Text.Equals(""))
                {
                    TotalAmounts = 0;       
                    TotalAmounts = decimal.Parse(txt_Bill_Amount.Text) + decimal.Parse(txt_RecAmount.Text);


                    txt_TotalAmount.Text = TotalAmounts.ToString();
                }
                else
                {
                    txt_TotalAmount.Text = TotalAmounts.ToString();
                }
            }
            catch { }
        }
        private void txt_Searching_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (radiobtn_Code.Checked == true)
                {
                    string Query = " SELECT  V_No, AccountCode FROM ReverseView Where V_No Like '%" + txt_Searching.Text + "%' Order By  V_No DESC ";
                    setMasterGrid(clsDataLayer.RetreiveQuery(Query));

                }
                else if (radiobtn_Name.Checked == true)
                {
                    //                string Query = @" SELECT  CRMaster.V_No, Accounts.ActTitle FROM  CRMaster 
                    //                RIGHT JOIN  CRDetail ON CRMaster.V_No = CRDetail.V_No 
                    //                RIGHT JOIN  Accounts ON CRDetail.AccountCode = Accounts.ActCode
                    //                Where Accounts.ActTitle Like '%" + txt_Searching.Text + "%' Order by CRMaster.V_No DESC ";
                    String Query = @"SELECT  crm.V_No, crm.V_Type, crm.Date, crm.Remarks, crm.Amount,
                  crd.ID DetailID, crd.Narration, crd.Inv_No, crd.Inv_Type, crd.Inv_Amount, 
                  crd.Rec_Amount,crd.AccountCode as DetailAcctCode,a.ActTitle as DetailAccountTitle,
                  crm.Account_Code as MasterActCode,aa.ActTitle as MasterActTitle
                  FROM  CRMaster crm
                  Inner JOIN CRDetail crd on crd.V_No=crm.V_No
                  Inner JOIN Accounts a on a.ActCode= crd.AccountCode
                  Inner JOIN Accounts aa on aa.ActCode= crm.Account_Code
                  Where a.ActTitle Like '%" + txt_Searching.Text + "%' ";


                    setMasterGrid(clsDataLayer.RetreiveQuery(Query));
                }
            }
            catch { }
        }

        private void comboBox_PartyAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actMode != ActionModes.Browsing)
            {
                DataTable dt = clsDataLayer.RetreiveQuery(" Select ActCode, ActTitle from Accounts Where ID=" + comboBox_PartyAccount.Text);
                if (dt.Rows.Count > 0)
                {
                    txt_PartyActCode.Text = dt.Rows[0][0].ToString();

                }
                else
                    txt_PartyActCode.Text = "";
            }
        }

        private void grid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            TotalAmount();
        }

        private void dgv_Master_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgv_Master.Rows.Count > 0 && dgv_Master[0, e.RowIndex].Value != null)
                {
                    string query = @"SELECT  crm.V_No, crm.V_Type, crm.Date, crm.Remarks, crm.Amount,
                  crd.ID DetailID, crd.Narration, crd.Inv_No, crd.Inv_Type, crd.Inv_Amount, 
                  crd.Rec_Amount,crd.AccountCode as DetailAcctCode,a.ActTitle as DetailAccountTitle,
                  crm.Account_Code as MasterActCode,aa.ActTitle as MasterActTitle
                  FROM  CRMaster crm
                  Inner JOIN CRDetail crd on crd.V_No=crm.V_No
                  Inner JOIN Accounts a on a.ActCode= crd.AccountCode
                  Inner JOIN Accounts aa on aa.ActCode= crm.Account_Code
                  Where crm.V_No='" + dgv_Master[0, e.RowIndex].Value.ToString() + "'";
                    Console.WriteLine(query);
                    DataTable dt = clsDataLayer.RetreiveQuery(query);
                    if (dt.Rows.Count > 0)
                    {
                        txt_Code.Text = dgv_Master[0, e.RowIndex].Value.ToString();
                        txt_VType.Text = dt.Rows[0]["V_Type"].ToString();

                        String da = dt.Rows[0]["Date"].ToString();
                        String[] hh = da.Split('-');
                        String ge = hh[1] + "-" + hh[0] + "-" + hh[2];
                        dtp_Date.Text = ge;
                        //   txt_CashActCode.Text = dt.Rows[0]["MasterActCode"].ToString();
                        //  txt_CashActTitle.Text = dt.Rows[0]["MasterActTitle"].ToString();
                        SetCashAccounts();

                        txt_Remarks.Text = dt.Rows[0]["Remarks"].ToString();
                        txt_TotalAmount.Text = dt.Rows[0]["Amount"].ToString();

                        comboBox_PartyAccount.Text = dt.Rows[0]["DetailAccountTitle"].ToString();
                        txt_PartyActCode.Text = dt.Rows[0]["DetailAcctCode"].ToString();
                        txt_Narration.Text = dt.Rows[0]["Narration"].ToString();
                        txt_InvoiceNo.Text = dt.Rows[0]["Inv_No"].ToString();
                        txt_Bill_Amount.Text = dt.Rows[0]["Inv_Amount"].ToString();
                        txt_RecAmount.Text = dt.Rows[0]["Rec_Amount"].ToString();
                        //DetailID[i] = int.Parse(dt.Rows[0]["DetailID"].ToString());
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btn_Undo_Click(object sender, EventArgs e)
        {
            getvalue = 0;
            actMode = ActionModes.Browsing;
            SeletionModes(actMode);
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if(txt_RecAmount.Text.Equals(""))
            {
                getvalue = 0;
            }
            else
            {
                getvalue = Convert.ToDecimal(txt_RecAmount.Text); 
            }
         
            actMode = ActionModes.Edit;
            SeletionModes(actMode);
            
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (actMode == ActionModes.Browsing)
            {
                string deleteQuery = "Delete from CRMaster Where V_No='" + txt_Code.Text + "'" +
              "Delete from CRDetail Where V_No='" + txt_Code.Text + "'" + "Delete from tbl_Ledger where V_No='" + txt_Code.Text + "'";
                string[] str = deleteQuery.Split('@');
                Console.WriteLine(deleteQuery);
                DialogResult result = MessageBox.Show("Are you sure to delete this Record", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    //  foreach (string s in str)
                    // {
                    if (clsDataLayer.ExecuteQuery(deleteQuery) > 0)
                    {

                        txt_CashActCode.Text = "";
                        txt_CashActTitle.Text = "";
                        txt_inv_type.Text = "";
                        txt_InvoiceNo.Text = "";
                        txt_Narration.Text = "";
                        txt_PartyActCode.Text = "";
                        txt_RecAmount.Text = "";
                        txt_Remarks.Text = "";
                        txt_TotalAmount.Text = "";
                        txt_VType.Text = "";
                        txt_Bill_Amount.Text = "";
                        comboBox_PartyAccount.Text = "";
                        dtp_Date.Text = DateTime.Now.ToString();
                        setMasterGrid(clsDataLayer.RetreiveQuery(@" SELECT  V_No, AccountCode FROM ReverseView "));
                        actMode = ActionModes.Browsing;
                        SeletionModes(actMode);
                        decreaseUpdatevalue();

                    }
                    //}
                }
            }
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_Save_Leave(object sender, EventArgs e)
        {

        }

        private void txt_RecAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (Char.IsDigit(e.KeyChar) || e.KeyChar == '\b' || e.KeyChar == '.')
            //{
            //    e.Handled = false;
            //}
            //else
            //{
            //    e.Handled = true;
            //}
        }

        private void getpartycode(object sender, EventArgs e)
        {

        }

        private void party(object sender, EventArgs e)
        {
            if (actMode != ActionModes.Browsing)
            {
                DataTable dt = clsDataLayer.RetreiveQuery("Select ActCode, ActTitle from Accounts Where ActTitle='" + comboBox_PartyAccount.Text + "'");

                if (dt.Rows.Count > 0)
                {
                    txt_PartyActCode.Text = dt.Rows[0]["ActCode"].ToString();

                }
                else
                    txt_PartyActCode.Text = "";
            }
        }

        public void remain()
        {
            decimal dd = 0;
            decimal rec = 0;
            decimal billamount = 0;
            string oldrow = "select * from tbl_RemainingAmount where RefCode = '" + txt_PartyActCode.Text + "' ";
            Console.WriteLine("Oldrow " + oldrow);
            DataTable df = clsDataLayer.RetreiveQuery(oldrow);
            decimal total = 0;
            decimal due = 0;
            decimal received = 0;
            if (df.Rows.Count > 0)
            {
                total = decimal.Parse(df.Rows[0]["TotalAmount"].ToString());
                due = decimal.Parse(df.Rows[0]["DueAmount"].ToString());
                received = decimal.Parse(df.Rows[0]["ReceivedAmount"].ToString());
            }
            else
            {
                total = 0;
                due = 0;
                received = 0;

            }
            billamount = decimal.Parse(txt_RecAmount.Text);
            total = total + billamount;

            dd = due + billamount;
            string updateblnc = "update tbl_RemainingAmount set TotalAmount=" + total.ToString() + ", DueAmount=" + dd.ToString() + "  where RefCode='" + txt_PartyActCode.Text + "' ";
            Console.WriteLine(updateblnc + "RemainBlance");
            if (clsDataLayer.ExecuteQuery(updateblnc) > 0)
            {
                //  MessageBox.Show("Remain Update");
            }
        }


        public void decreaseUpdatevalue()
        {
            decimal dd = 0;
            decimal rec = 0;
            decimal billamount = 0;
            decimal total = 0;
            decimal due = 0;
            string oldrow = "select * from tbl_RemainingAmount where RefCode = '" + txt_PartyActCode.Text + "' ";
            DataTable df = clsDataLayer.RetreiveQuery(oldrow);
            Console.WriteLine(oldrow + "this one");
            if (df.Rows.Count > 0)
            {
                decimal received = decimal.Parse(df.Rows[0]["ReceivedAmount"].ToString());
                billamount = decimal.Parse(txt_TotalAmount.Text);
                total = decimal.Parse(df.Rows[0]["TotalAmount"].ToString());
                due = decimal.Parse(df.Rows[0]["DueAmount"].ToString());
                received = decimal.Parse(df.Rows[0]["ReceivedAmount"].ToString());

                total = total - billamount;

                dd = due - billamount;

                string updateblnc = "update tbl_RemainingAmount set Dates='" + DateTime.Now.ToString("dd-MM-yyyy") + "', ReceivedAmount=" + rec.ToString() + " where RefCode='" + txt_PartyActCode.Text + "' ";
                if (clsDataLayer.ExecuteQuery(updateblnc) > 0)
                {
                    // MessageBox.Show("change Update");

                }
            }

        }

        private void getTotal(object sender, EventArgs e)
        {
            TotalAmount();
        }


        decimal balance = 0;
        public decimal PartyBalance()
        {
            try
            {
                String get = "select Party_Balance from tbl_Ledger where RefCode='" + txt_PartyActCode.Text + "' order by ID desc";
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

        private void Filter()
        {
            if (radiobtn_Code.Checked)
            {
                string query = @"SELECT V_No from VU_CashRecieve where V_No  like '%" + txt_Searching.Text + "%'";
                setMasterGrid(clsDataLayer.RetreiveQuery(query));


            }
            else if (radiobtn_Name.Checked)
            {
                string query = @"SELECT ActTitle from VU_CashRecieve where ActTitle  like '%" + txt_Searching.Text + "%'";
                setMasterGrid(clsDataLayer.RetreiveQuery(query));
            }


        }
        private void ReverseEntry_Load(object sender, EventArgs e)
        {
            Filter();
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            try
            {
                //string query = " Select * From  ReverseView Where V_No='" + txt_Code.Text + "'";
                //DataTable dt = clsDataLayer.RetreiveQuery(query);
                //rptReceipt rptCB = new rptReceipt();
                //rptCB.SetDataSource(dt);
                //CashPaymentViewer frmrptview = new CashPaymentViewer(rptCB, comboBox_PartyAccount.Text);
                //frmrptview.ShowDialog();
            }
            catch { }
        }

        private void txt_Code_TextChanged(object sender, EventArgs e)
        {
            if (actMode == ActionModes.Browsing)
            {
                btn_Print.Enabled = true;
            }
        }


        int ind = 0;
        private int RowNumber()
        {
            String gh = "SELECT V_No FROM tbl_Ledger where RefCode = '" + txt_PartyActCode.Text + "' order by ID asc";
            DataTable df = clsDataLayer.RetreiveQuery(gh);
            if (df.Rows.Count > 0)
            {
                ind++;
            }
            return ind;
        }

        private int GetRowIndex(string input)
        {
            int index = 0;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT V_No FROM tbl_Ledger where RefCode = '" + txt_PartyActCode.Text + "' order by ID asc", con);
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
                SqlCommand cmd = new SqlCommand("DELETE FROM tbl_Ledger where RefCode = '" + txt_PartyActCode.Text + "'", con);
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
                SqlDataAdapter cmd = new SqlDataAdapter("select V_No, Datetime, AccountCode, RefCode, PaymentType, InvoiceNo, Particulars, Debit, Credit, Party_Balance from tbl_Ledger where RefCode = '" + txt_PartyActCode.Text + "' order by ID asc", con);
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
                        dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[7]);
                    }
                    else if (i == 1)
                    {
                        dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[8]);
                    }
                    else
                    {
                        if (type.Equals("OnReceived"))
                        {
                            dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i - 1].ItemArray[9]) - Convert.ToDecimal(dt.Rows[i].ItemArray[7]);
                        }
                        else
                        {
                            dt.Rows[i][9] = Convert.ToDecimal(dt.Rows[i].ItemArray[7]) + Convert.ToDecimal(dt.Rows[i - 1].ItemArray[9]);
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
                    SqlCommand cmd = new SqlCommand("INSERT INTO tbl_Ledger VALUES('" + dt.Rows[i].ItemArray[0].ToString() + "','" + dt.Rows[i].ItemArray[1].ToString() + "','" + dt.Rows[i].ItemArray[2].ToString() + "','" + dt.Rows[i].ItemArray[3].ToString() + "','" + dt.Rows[i].ItemArray[4].ToString() + "','" + dt.Rows[i].ItemArray[5].ToString() + "','" + dt.Rows[i].ItemArray[6].ToString() + "'," + dt.Rows[i].ItemArray[7].ToString() + "," + dt.Rows[i].ItemArray[8].ToString() + "," + dt.Rows[i].ItemArray[9].ToString() + ")", con);
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

    }
}


