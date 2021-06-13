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
    public partial class Reminders : Form
    {
        String Code = "";
        public Reminders()
        {
            InitializeComponent();
            Disable();
            clsGeneral.SetAutoCompleteTextBox(txt_party, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 12 Order BY ID DESC "));
            try
            {
                String query = "select * from tbl_Reminder";
                DataTable dt = clsDataLayer.RetreiveQuery(query);
                if (dt.Rows.Count > 0)
                {
                    grid.DataSource = dt;
                }


                String query7 = "select Amount from tbl_Reminder";
                DataTable dt7 = clsDataLayer.RetreiveQuery(query7);
                if (dt7.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(tamount, dt7);
                }

                String query9 = "select PartyName from tbl_Reminder";
                DataTable dt9 = clsDataLayer.RetreiveQuery(query9);
                if (dt9.Rows.Count > 0)
                {
                    clsGeneral.SetAutoCompleteTextBox(txt_party, dt9);
                }
            }
            catch { }
            tparty.Enabled = true;
            tamount.Enabled = true;
        }

        private void PartyCode()
        {
            String sel = "select ActCode from Accounts where ActTitle = '" + txt_party.Text + "'";
            DataTable dc = clsDataLayer.RetreiveQuery(sel);
            if (dc.Rows.Count > 0)
            {
                Code = dc.Rows[0][0].ToString();
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
                    {
                        if (((ComboBox)c).Enabled == false)
                        {
                            ((ComboBox)c).Enabled = true;
                            flag = true;
                            
                        }
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
                    {
                        if (((ComboBox)c).Enabled == true)
                        {
                            ((ComboBox)c).Enabled = false;
                            flag = true;

                        }
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
                        break;
                    }
                }
                else if (c is ComboBox)
                {
                    {
                        if (((ComboBox)c).Text != "")
                        {
                            ((ComboBox)c).Text = "";
                            flag = true;
                            break;
                        }
                    }
                }
            }
            return flag;
          
        }


        private void btnsubmit_Click(object sender, EventArgs e)
        {
            if (!CheckAllFields())
            {
                String query = "insert into tbl_Reminder(RemCode,PartyName,Date,Amount,ComboBoxIn,Types,Softwares,Descriptions)values('" + txt_code.Text + "','" + txt_party.Text + "','" + dateTimePicker1.Text + "','" + txt_amount.Text + "','" + txt_inout.Text + "','" + txt_cb.Text + "','" + txt_software.Text + "','" + txt_Description.Text+ "')";
                clsDataLayer.ExecuteQuery(query);
               
               
                MessageBox.Show("Record Save Successfully!");
                Disable();
                tparty.Enabled=true;
                tamount.Enabled = true;
              
             String query5 = "select * from tbl_Reminder";
             DataTable dt5 = clsDataLayer.RetreiveQuery(query5);
             if (dt5.Rows.Count > 0)
             {
                 grid.DataSource = dt5;
             }
                
            }
            else
            {
                MessageBox.Show("Please Fill All Fields!");
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                Enable();
                Clears();
                txt_code.Text = clsGeneral.getMAXCode("tbl_Reminder", "RemCode", "RM");
                tparty.Text = "Search By PartyName";
                tamount.Text = "Search By Amount";
                tparty.Enabled = true;
                tamount.Enabled = true;
            }
            catch  
            {  }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String query = "select * from tbl_Reminder where PartyName = '" + tparty.Text + "'";
                DataTable dt = clsDataLayer.RetreiveQuery(query);
                if (dt.Rows.Count > 0)
                {
                    grid.DataSource = dt;
                }
                else
                {
                    String querys = "select * from tbl_Reminder";
                    DataTable dts = clsDataLayer.RetreiveQuery(querys);
                    if (dts.Rows.Count > 0)
                    {
                        grid.DataSource = dts;
                    }

                }
            }
            catch { }
        }

        private void tamount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String query = "select * from tbl_Reminder where Amount = " + tamount.Text + "";
                DataTable dt = clsDataLayer.RetreiveQuery(query);
                if (dt.Rows.Count > 0)
                {
                    grid.DataSource = dt;
                }
                else
                {
                    String querys = "select * from tbl_Reminder";
                    DataTable dts = clsDataLayer.RetreiveQuery(querys);
                    if (dts.Rows.Count > 0)
                    {
                        grid.DataSource = dts;
                    }

                }
            }
            catch { }
        }

        private void dtm_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                String query = "select * from tbl_Reminder where Date = '" + dtm.Text + "'";
                DataTable dt = clsDataLayer.RetreiveQuery(query);
                if (dt.Rows.Count > 0)
                {
                    grid.DataSource = dt;
                }
                else
                {
                    String querys = "select * from tbl_Reminder";
                    DataTable dts = clsDataLayer.RetreiveQuery(querys);
                    if (dts.Rows.Count > 0)
                    {
                        grid.DataSource = dts;
                    }

                }
            }
            catch { }
        }

        private void tparty_Leave(object sender, EventArgs e)
        {
            tparty.Text = "Search By PartyName";
         
        }

        private void tamount_Leave(object sender, EventArgs e)
        {
            tamount.Text = "Search By Amount";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
               String upd = "delete from tbl_Reminder where RemCode='"+txt_code.Text+"'";
                clsDataLayer.ExecuteQuery(upd);

                String query = "insert into tbl_Reminder(RemCode,PartyName,Date,Amount,ComboBoxIn,Types,Softwares,Descriptions)values('" + txt_code.Text + "','" + txt_party.Text + "','" + dateTimePicker1.Text + "','" + txt_amount.Text + "','" + txt_inout.Text + "','" + txt_cb.Text + "','" + txt_software.Text + "','" + txt_Description.Text + "')";
                clsDataLayer.ExecuteQuery(query);

                if (clsDataLayer.ExecuteQuery(query) > 0)
                {
                    MessageBox.Show("Update Successfully!");
                }
                
            }
            catch { }
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txt_code.Text = grid.CurrentRow.Cells[1].Value.ToString();
            }
            catch { }
        }

        private void txt_code_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String search = "select * from tbl_Reminder where RemCode = '"+txt_code.Text+"'";
                DataTable dt = clsDataLayer.RetreiveQuery(search);
                if(dt.Rows.Count > 0)
                {
                    txt_party.Text = dt.Rows[0]["PartyName"].ToString();
                    dateTimePicker1.Text = dt.Rows[0]["Date"].ToString();
                    txt_inout.Text = dt.Rows[0]["ComboBoxIn"].ToString();
                    txt_Description.Text = dt.Rows[0]["Descriptions"].ToString();
                    txt_amount.Text = dt.Rows[0]["Amount"].ToString();
                    txt_software.Text = dt.Rows[0]["Softwares"].ToString();
                    txt_cb.Text = dt.Rows[0]["Types"].ToString();
            }
            }
            catch { }
        }

        private void BTN_EDIT_Click(object sender, EventArgs e)
        {
            try
            {
                Enable();
            }
            catch {  }
        }

        private void txt_amount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

    }
}
