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
    public partial class payment_reminder : Form
    {
        public payment_reminder()
        {
            InitializeComponent();
            Disable();
            Clears();
            refresh();
            date_check();
            clsGeneral.SetAutoCompleteTextBox(party_name, clsDataLayer.RetreiveQuery("select ActTitle from Accounts where ID > 12 and Status = 1 Order BY ID DESC"));
        }
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        public void refresh()
        {

            SqlDataAdapter sda = new SqlDataAdapter("SELECT reminder_code,party_name,payment_date,voucher_date,remarks,Amount FROM payment_reminder", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
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
                        //if (((TextBox)c).Name == "search_rcpt") { }
                        //else
                        //{
                        flag = true;
                        break;
                        //}
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
                else if (c is RichTextBox)
                {

                    if (((RichTextBox)c).Text == "")
                    {
                        flag = true;
                        break;
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
                else if (c is RichTextBox)
                {
                    if (((RichTextBox)c).Text != "")
                    {
                        ((RichTextBox)c).Text = "";

                    }
                }
                //dataGridView1.Rows.Clear();


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
                else if (c is RichTextBox)
                {
                    if (((RichTextBox)c).Enabled == false)
                    {
                        ((RichTextBox)c).Enabled = true;
                        flag = true;
                    }
                }
                //dataGridView1.Enabled = true

          
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
                else if (c is RichTextBox)
                {
                    if (((RichTextBox)c).Enabled == true)
                    {
                        ((RichTextBox)c).Enabled = false;
                        flag = true;
                    }
                }
                //dataGridView1.Enabled = false;

                btn_edit.Enabled = false;
                btn_save.Enabled = false;
                btn_update.Enabled = false;

                btn_add.Enabled = true;

            }


            return flag;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clears();
            Enable();
            //dataGridView1.Rows.Add();
            trc_code.Text = clsGeneral.getMAXCode("payment_reminder", "reminder_code", "PR");
            btn_save.Enabled = true;
            btn_update.Enabled = false;
            btn_edit.Enabled = false;
            btn_add.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckAllFields())
                { 
                    Savee();
                }
                else
                {
                    MessageBox.Show("Fill All Fields");
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public void Savee()
        {
            string q = "select reminder_code from payment_reminder where reminder_code='" + trc_code.Text + "' ";

            DataTable d3 = clsDataLayer.RetreiveQuery(q);
            if (d3.Rows.Count == 0)
            {
                string query = "";
                query = "insert into payment_reminder (Amount,reminder_code,party_name,payment_date,voucher_date,remarks)values ("+txtamount.Text+",'" + trc_code.Text + "','" + party_name.Text + "','" + dateTimePicker1.Text.ToString() + "','" + dateTimePicker2.Text.ToString() + "','" + remarks.Text + "')";
                if (clsDataLayer.ExecuteQuery(query) > 0)
                {
                    MessageBox.Show("Saved Succesfully");
                    //refresh();
                    btn_save.Enabled = false;
                    btn_update.Enabled = false;
                    Clears();
                    Disable();
                    refresh();
                }
            }
            else
            {
                MessageBox.Show("Data Already Exist");
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            Enable();
            btn_add.Enabled = false;
            btn_edit.Enabled = false;
            btn_save.Enabled = false;
            btn_update.Enabled = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_add.Enabled = false;
            btn_save.Enabled = false;
            btn_update.Enabled = false;
            btn_edit.Enabled = true;

            trc_code.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            party_name.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            dateTimePicker2.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            remarks.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            txtamount.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            Clears();
            Disable();
            btn_add.Enabled = true;
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckAllFields())
                {
                    updatee();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public void updatee()
        {
            String queryya1 = "Delete From payment_reminder Where reminder_code = '" + trc_code.Text + "'";
            clsDataLayer.ExecuteQuery(queryya1);

            string q = "select reminder_code from payment_reminder where reminder_code='" + trc_code.Text + "' ";

            DataTable d3 = clsDataLayer.RetreiveQuery(q);
            if (d3.Rows.Count == 0)
            {
                string query = "";
                query = "insert into payment_reminder (Amount,reminder_code,party_name,payment_date,voucher_date,remarks)values (" + txtamount.Text + ",'" + trc_code.Text + "','" + party_name.Text + "','" + dateTimePicker1.Text.ToString() + "','" + dateTimePicker2.Text.ToString() + "','" + remarks.Text + "')";
                if (clsDataLayer.ExecuteQuery(query) > 0)
                {
                    MessageBox.Show("Saved Succesfully");
                    //refresh();
                    btn_save.Enabled = false;
                    btn_update.Enabled = false;
                    Clears();
                    Disable();
                    refresh();
                }
            }
            else
            {
                MessageBox.Show("Data Already Exist");
            }
        }

        public void date_check()
        {
        DataTable h = new DataTable();
        h.Columns.Add("reminder_code"); h.Columns.Add("party_name"); h.Columns.Add("payment_date"); h.Columns.Add("Amount"); h.Columns.Add("remarks"); h.Columns.Add("Status");
        DateTime haha;
        SqlDataAdapter sda1 = new SqlDataAdapter("select payment_date,reminder_code,party_name,Amount,remarks from payment_reminder", con);
        DataTable dt1 = new DataTable();
        sda1.Fill(dt1);
        if (dt1.Rows.Count > 0)
        {
        for (int i = 0; i < dt1.Rows.Count; i++)
        {
        string reminderr = dt1.Rows[i][1].ToString(); string party = dt1.Rows[i][2].ToString(); string Amount = dt1.Rows[i][3].ToString();
        string remarks = dt1.Rows[i][4].ToString();          string dte = dt1.Rows[i][0].ToString(); 
        haha = Convert.ToDateTime(dt1.Rows[i][0].ToString());
        DateTime today = DateTime.Today; 
        TimeSpan t = today - haha;
        double dDays = t.TotalDays;
        int days = Convert.ToInt32(dDays);
        if (days < 0)
        {
        int temp = -1 * days;
        if (days < 8)
        {
            h.Rows.Add(reminderr, party, dte, Amount, remarks,"Remaining "+temp+" Days");
        // MessageBox.Show("The Payment for " + reminderr + " is Due for " + temp + " Days");

        }
        }
        else if (days == 0)
        {
            h.Rows.Add(reminderr, party, dte, Amount, remarks, "Expire");
             //essageBox.Show("The Payment for " + reminderr + " is Expire");
        }  
        }

        }
        }

        private void trc_code_TextChanged(object sender, EventArgs e)
        {
        try
        {
        String q = "select party_name,payment_date,voucher_date,remarks,Amount from payment_reminder where reminder_code='"+trc_code.Text+"'";
        DataTable dq = clsDataLayer.RetreiveQuery(q);
        if (dq.Rows.Count > 0)
        {
            party_name.Text = dq.Rows[0][0].ToString(); dateTimePicker1.Text = dq.Rows[0][1].ToString(); dateTimePicker2.Text = dq.Rows[0][2].ToString();
            remarks.Text = dq.Rows[0][3].ToString(); txtamount.Text = dq.Rows[0][4].ToString();
        }
        }
        catch { }
        }

        private void txtamount_KeyPress(object sender, KeyPressEventArgs e)
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
