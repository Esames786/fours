using GrayLark.bin.Debug.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrayLark
{
    public partial class frmPettyCash : Form
    {
        String ProjCode = ""; String Status = ""; TextBox tb = new TextBox();
        decimal available = 0;
        public frmPettyCash()
        {
            InitializeComponent(); OnLoad();
        }

        private void OnLoad()
        {
            Disable();
            //clsGeneral.SetAutoCompleteTextBox(txt_party, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 13 and Status = 1 Order BY ID DESC"));
            clsGeneral.SetAutoCompleteTextBox(txt_purchase, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 13 and Status = 1 Order BY ID DESC"));
            //String GetCode = "select Project_Name from tbl_CreateProject"; DataTable d1 = clsDataLayer.RetreiveQuery(GetCode); if (d1.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txt_project, d1); }
            btnSave.Enabled = false;
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
            }
            return flag;
        }
        private bool Clears()
        {
            bool flag = false;
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (c is TextBox)
                {
                    if (((TextBox)c).Text != "")
                    {
                        ((TextBox)c).Text = "";
                        flag = true; 
                    }
                }
                else if (c is ComboBox)
                {
                    if (((ComboBox)c).Text != "")
                    {
                        ((ComboBox)c).Text = "";
                        flag = true; 
                    } 
                }
            }


            return flag;
        }
        
        
        private void GetProjectCode(String pname)
        {
            String GetCode = "select project_code from begin_product where  project_name = '" + pname + "'";
        DataTable d1 = clsDataLayer.RetreiveQuery(GetCode); if (d1.Rows.Count > 0) {ProjCode = d1.Rows[0][0].ToString(); }
        }
        private void BtnEnbDsb(String Status)
        {
        if (Status.Equals("New"))
        {
            btnNew.Enabled = false; btnEdit.Enabled = false; btnSave.Enabled = true; btnPrint.Enabled = false;
        }
        else if (Status.Equals("Edit"))
        {
            btnNew.Enabled = false; btnEdit.Enabled = false; btnSave.Enabled = true; btnPrint.Enabled = false;       
        }
        else if (Status.Equals("Save"))
        {
            btnNew.Enabled = true; btnEdit.Enabled = true; btnSave.Enabled = false; btnPrint.Enabled = true;
        } 
        }

        private void LedgerInsert()
        {
        
        }

        private void PettyCashs()
        {
        if (Status.Equals("New"))
        {
        String sel = "select Paid_Amount,Remaining_Amount  from tbl_issuepettyamount where MonthYear='" + DateTime.Now.ToString("MMMM,yyyy") + "' and Petty_User='" + txt_purchase.Text + "'";
        DataTable ds = clsDataLayer.RetreiveQuery(sel);
        if (ds.Rows.Count > 0)
        {
        decimal remain = 0; decimal Paid = 0; decimal cashout = 0; decimal pf = 0, rf = 0;
        cashout = Convert.ToDecimal(txt_total.Text);
        Paid = Convert.ToDecimal(ds.Rows[0][0].ToString()); remain = Convert.ToDecimal(ds.Rows[0][1].ToString()); pf = Paid + cashout; rf = remain - cashout;
        String upd = "update tbl_issuepettyamount set Paid_Amount=" + pf + ",Remaining_Amount=" + rf + " where Petty_User='" + txt_purchase.Text + "' and MonthYear='" + DateTime.Now.ToString("MMMM,yyyy") + "'"; clsDataLayer.ExecuteQuery(upd);
        }
        }
        else if (Status.Equals("Edit"))
        {
        decimal old = 0;
        String qs = "Select TotalAmount from tbl_PettyCash where Pe_Code='" + txt_code.Text + "'"; DataTable d2 = clsDataLayer.RetreiveQuery(qs); if (d2.Rows.Count > 0)
        {
        old = Convert.ToDecimal(d2.Rows[0][0].ToString());
        }
        String sel1 = "select Paid_Amount,Remaining_Amount  from tbl_issuepettyamount where MonthYear='" + DateTime.Now.ToString("MMMM,yyyy") + "' and Petty_User='" + txt_purchase.Text + "'";
        DataTable ds1 = clsDataLayer.RetreiveQuery(sel1);
        if (ds1.Rows.Count > 0)
        {
        decimal remain = 0; decimal Paid = 0; decimal cashout = 0; decimal pf = 0, rf = 0; 
        cashout = Convert.ToDecimal(txt_total.Text);
        Paid = Convert.ToDecimal(ds1.Rows[0][0].ToString()); remain = Convert.ToDecimal(ds1.Rows[0][1].ToString()); pf = Paid - old +  cashout; rf = remain + old - cashout;
        String upd = "update tbl_issuepettyamount set Paid_Amount=" + pf + ",Remaining_Amount=" + rf + " where Petty_User='" + txt_purchase.Text + "' and MonthYear='" + DateTime.Now.ToString("MMMM,yyyy") + "'"; clsDataLayer.ExecuteQuery(upd);
        }
        } 
        }

        private void AvailablePetty()
        {
            try
            {
                String sel = "select Remaining_Amount  from tbl_issuepettyamount where MonthYear='" + DateTime.Now.ToString("MMMM,yyyy") + "' and Petty_User='" + txt_purchase.Text + "'";
                DataTable ds = clsDataLayer.RetreiveQuery(sel);
                if (ds.Rows.Count > 0)
                {
                    txtavailable.Text = ds.Rows[0][0].ToString(); available = Convert.ToDecimal(ds.Rows[0][0].ToString());
                }
            }
            catch { }
        }
        
        private void New()
        {
        try
        {
        Clears();
        BtnEnbDsb("New"); Enable(); grid.Rows.Clear(); grid.Rows.Add(); Status = "New";
        txt_code.Text = clsGeneral.getMAXCode("tbl_PettyCash", "Pe_Code", "PE");
        //String quer = "select project_code from begin_product"; DataTable d1 = clsDataLayer.RetreiveQuery(quer); if (d1.Rows.Count > 0) { clsGeneral.SetAutoCompleteTextBox(txt_project, d1); }
        }
        catch { }
        }
        private void Edit() { grid.Enabled = true; BtnEnbDsb("Edit"); Enable(); Status = "Edit"; }
        private void Update()
        {
            PettyCashs();
       String del = "delete from tbl_PettyCash where Pe_Code='" + txt_code.Text + "'"; clsDataLayer.ExecuteQuery(del);
       Save(); Disable(); grid.Enabled = false; MessageBox.Show("Record Update Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }

        private void Save()
        {
            BtnEnbDsb("Save"); PettyCashs();

        String ins =@"insert into tbl_PettyCash(Pe_Code,DateofMonth,TotalAmount,Petty_User,UserName)values ('"+txt_code.Text+"','"+DateTime.Now.ToString("MMMM,yyyy")+"',"+txt_total.Text+",'"+txt_purchase.Text+"','"+Login.UserID+"')"; clsDataLayer.ExecuteQuery(ins);
        for (int a = 0; a < grid.Rows.Count; a++)
        {  
            String dd = "delete from tbl_PettyCashDetail where Pet_Code='" + txt_code.Text + "'"; clsDataLayer.ExecuteQuery(dd);
        String ins1 = "insert into tbl_PettyCashDetail(Pet_Code,Date,Exp_Head,Item,Unit,Qty,Price,Amount)values('" + txt_code.Text + "','" + grid.Rows[a].Cells[0].Value.ToString() + "','" + grid.Rows[a].Cells[1].Value.ToString() + "','" + grid.Rows[a].Cells[2].Value.ToString() + "','" + grid.Rows[a].Cells[3].Value.ToString() + "'," + grid.Rows[a].Cells[4].Value.ToString() + "," + grid.Rows[a].Cells[5].Value.ToString() + "," + grid.Rows[a].Cells[6].Value.ToString() + ")"; clsDataLayer.ExecuteQuery(ins1);
        }   Disable(); grid.Enabled = false;
        }
        
        private void Print()
        {
        try
        {
        String print = "select * from PettyView where Pe_Code = '"+txt_code.Text+"'"; DataTable dp = clsDataLayer.RetreiveQuery(print);
        if (dp.Rows.Count > 0)
        {
            rptPetty rp = new rptPetty(); rp.SetDataSource(dp); PaymentView pv = new PaymentView(rp); pv.Show();
        }
        else
        {
            MessageBox.Show("No Record Found!","Error",MessageBoxButtons.OK,MessageBoxIcon.Stop);
        }
        }
        catch { }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            New(); grid.Enabled = true;
        }

        private void txt_purchase_TextChanged(object sender, EventArgs e)
        {
        try
        {
           AvailablePetty();
        }
        catch { }
        }

        private void grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (grid.CurrentCell.ColumnIndex == 1)
                {
                    int yy = grid.CurrentCell.ColumnIndex;
                    string columnHeaders = grid.Columns[yy].HeaderText;
                    if (columnHeaders.Equals("Exp. Head"))
                    {
                        tb = e.Control as TextBox;
                        if (tb != null)
                        {
                            String hname = "select ActTitle from Accounts where HeaderActCode = '5'"; DataTable df = clsDataLayer.RetreiveQuery(hname);
                            if (df.Rows.Count > 0)
                            {
                                AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();

                                foreach (DataRow row in df.Rows)
                                {
                                    acsc.Add(row[0].ToString());
                                }

                                tb.AutoCompleteCustomSource = acsc;
                                tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                            }
                        }
                    }
                }
                else if (grid.CurrentCell.ColumnIndex == 0)
                {
                    int yy = grid.CurrentCell.ColumnIndex;
                    string columnHeaders = grid.Columns[yy].HeaderText;
                    if (columnHeaders.Equals("Date"))
                    {
                        tb = e.Control as TextBox;
                        if (tb != null)
                        {
                            DataTable dv = new DataTable();
                            dv.Columns.Add("Date");
                            dv.Rows.Add(DateTime.Now.ToString("yyyy-MM-dd"));
                            dv.Rows.Add(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                            dv.Rows.Add(DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"));
                            dv.Rows.Add(DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd"));
                            dv.Rows.Add(DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd"));
                            dv.Rows.Add(DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd"));
                            dv.Rows.Add(DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd"));
                            dv.Rows.Add(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"));
                            dv.Rows.Add(DateTime.Now.AddDays(-8).ToString("yyyy-MM-dd"));
                            dv.Rows.Add(DateTime.Now.AddDays(-9).ToString("yyyy-MM-dd"));
                            dv.Rows.Add(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
                            dv.Rows.Add(DateTime.Now.AddDays(-11).ToString("yyyy-MM-dd"));
                            dv.Rows.Add(DateTime.Now.AddDays(-12).ToString("yyyy-MM-dd"));
                            dv.Rows.Add(DateTime.Now.AddDays(-13).ToString("yyyy-MM-dd"));
                            dv.Rows.Add(DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd"));
                            dv.Rows.Add(DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd"));
                             
                            if (dv.Rows.Count > 0)
                            {
                                AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();

                                foreach (DataRow row in dv.Rows)
                                {
                                    acsc.Add(row[0].ToString());
                                }

                                tb.AutoCompleteCustomSource = acsc;
                                tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                            }
                        }
                    }
                }
                else
                {
                     tb = e.Control as TextBox;
                     if (tb != null)
                     {
                         DataTable fd = new DataTable();
                         fd.Columns.Add("null");
                         fd.Rows.Add("");
                         if (fd.Rows.Count > 0)
                         {
                             AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();

                             foreach (DataRow row in fd.Rows)
                             {
                                 acsc.Add(row[0].ToString());
                             }

                             tb.AutoCompleteCustomSource = acsc;
                             tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                             tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                         }
                     }

                }
            }
            catch { } 
        }

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (grid.CurrentCell.ColumnIndex == 6)
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

                //if (e.KeyCode == Keys.Tab)
                //{
                //    if (grid.CurrentCell.ColumnIndex == 2)
                //    {
                //        grid.CurrentCell = grid.CurrentRow.Cells[8];
                //    }
                //    e.Handled = true;
                //}
                //base.OnKeyDown(e);
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
                for (int j = 0; j < 8; j++)
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


        private void btnSave_Click(object sender, EventArgs e)
        {try{
            //String query = "select ActTitle from Accounts where ActTitle = '"+txt_code.Text+"'";
            //DataTable d1 = clsDataLayer.RetreiveQuery(query);
            //if (d1.Rows.Count > 0)
            //{
            String q1 = "select * from Accounts where ActTitle = '" + txt_purchase.Text + "' and HeaderActCode = '10103'";
                DataTable d2 = clsDataLayer.RetreiveQuery(q1);
                if (d2.Rows.Count > 0)
                {
                    if (!CheckAllFields() && !CheckDataGridCells(grid))
                    {
                        if (txtavailable.Text.StartsWith("-"))
                        {
                            MessageBox.Show("PettyCash Available Amount not enough!","Error",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                        }
                        else
                        {
                      if (Status.Equals("New"))
                      { Save();  MessageBox.Show("Record Save Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      }
                      else if (Status.Equals("Edit"))   { Update(); }
                          
                        }
                    }
                    else
                    {
                        MessageBox.Show("Fill All Fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                else
                {
                    MessageBox.Show("Purchase Officer Not Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            //}
            //else
            //{
            //    MessageBox.Show("PartyName Not Found!");
            //}
        }
        catch { }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        { PettySearch ps = new PettySearch(this); ps.Show(); }

        private void btnEdit_Click(object sender, EventArgs e) { 
            Edit();
            if (Status.Equals("Edit")) { decimal total = Convert.ToDecimal(txt_total.Text); available = available + total; txtavailable.Text = available.ToString(); }
        txt_purchase.Enabled = false;
        }

        private void totalgrid()
        {
            int total = 0;
            for (int a = 0; a < grid.Rows.Count; a++)
            {
                total += Convert.ToInt32(grid.Rows[a].Cells[6].Value.ToString());
            }
            txt_total.Text = total.ToString();
            decimal f = available - total; txtavailable.Text = f.ToString();
        }
        private void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        try
        {
        if (grid.CurrentCell.ColumnIndex == 5)
        {
            int a = Convert.ToInt32(grid.CurrentRow.Cells[4].Value);
            int b = Convert.ToInt32(grid.CurrentRow.Cells[5].Value);
            grid.CurrentRow.Cells[6].Value = a * b;

            totalgrid();
        }
        }
        catch { }
        }

        private void txt_code_TextChanged(object sender, EventArgs e)
        {
        try
        { 
       String search = "select TotalAmount,Petty_User from tbl_PettyCash where Pe_Code='"+txt_code.Text+"'";
        DataTable d1 = clsDataLayer.RetreiveQuery(search); if (d1.Rows.Count > 0)
        {
        txt_total.Text = d1.Rows[0][0].ToString(); txt_purchase.Text = d1.Rows[0][1].ToString();
         }

        String search1 = "select Date,Exp_Head,Item,Unit,Qty,Price,Amount from tbl_PettyCashDetail where Pet_Code='" + txt_code.Text + "'";
        DataTable d10 = clsDataLayer.RetreiveQuery(search1); if (d10.Rows.Count > 0)
        {
            grid.Rows.Clear();
        foreach(DataRow dr in d10.Rows){
        int n = grid.Rows.Add();
        grid.Rows[n].Cells[0].Value = dr[0].ToString();
        grid.Rows[n].Cells[1].Value = dr[1].ToString();
        grid.Rows[n].Cells[2].Value = dr[2].ToString();
        grid.Rows[n].Cells[3].Value = dr[3].ToString();
        grid.Rows[n].Cells[4].Value = dr[4].ToString();
        grid.Rows[n].Cells[5].Value = dr[5].ToString();
        grid.Rows[n].Cells[6].Value = dr[6].ToString();
      }
        }

        } catch { }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void PettyCash_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (grid.CurrentCell.ColumnIndex == 4)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            } if (grid.CurrentCell.ColumnIndex == 5)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
          
       
    }
}
