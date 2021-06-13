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

namespace Reddot_Express_Inventory
{
    public partial class Replace_Or_Return : Form
    {
        public Replace_Or_Return()
        {
            InitializeComponent();
            this.BackColor = Color.SteelBlue;
        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
        String UID = Login.UserID;

        private void Replace_Or_Return_Load(object sender, EventArgs e)
        {
            Venders();
            Products();
        }

        private void Products()
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM add_product ORDER BY NAME ASC", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboProductName.Items.Add(dt.Rows[i]["NAME"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Venders()
        {
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM vender ORDER BY VENDER_ID ASC", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboVenderName.Items.Add(dt.Rows[i]["NAME"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkReturn_CheckedChanged(object sender, EventArgs e)
        {
            if (checkReturn.Checked == true) 
            {
                dataGridView1.Enabled = true;
                dataGridView2.Enabled = false;
                checkReplace.Checked = false;
            }
        }

        private void checkReplace_CheckedChanged(object sender, EventArgs e)
        {
            if (checkReplace.Checked == true)
            {
                dataGridView1.Enabled = false;
                dataGridView2.Enabled = true;
                checkReturn.Checked = false;
            }
        }

        private void comboSize_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    try
                    {
                        if (comboProductName.Text != "" && comboSize.Text != "" && checkReturn.Checked == true)
                        {
                            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM add_product WHERE NAME = '" + comboProductName.Text + "' AND SIZE = '" + comboSize.Text + "'", con);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            foreach (DataRow item in dt.Rows)
                            {
                                int i = dataGridView1.Rows.Add();

                                dataGridView1.Rows[i].Cells[0].Value = item["NAME"].ToString();
                                dataGridView1.Rows[i].Cells[1].Value = item["QUANTITY"].ToString();
                                dataGridView1.Rows[i].Cells[4].Value = item["PURCHASE_PRICE"].ToString();
                                dataGridView1.Rows[i].Cells[7].Value = item["SIZE"].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    try
                    {
                        if (comboProductName.Text != "" && comboSize.Text != "" && checkReplace.Checked == true)
                        {
                            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM add_product WHERE NAME = '" + comboProductName.Text + "' AND SIZE = '" + comboSize.Text + "'", con);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            foreach (DataRow item in dt.Rows)
                            {
                                int n = dataGridView2.Rows.Add();

                                dataGridView2.Rows[n].Cells[0].Value = item["NAME"].ToString();
                                dataGridView2.Rows[n].Cells[1].Value = item["QUANTITY"].ToString();
                                dataGridView2.Rows[n].Cells[4].Value = item["PURCHASE_PRICE"].ToString();
                                dataGridView2.Rows[n].Cells[8].Value = item["SIZE"].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try 
            {
                decimal Price = 0;
                int ReturnQty = 0;
                int NewQty = 0;

                if (dataGridView1.CurrentRow.Cells["Column3"].Value != null && dataGridView1.CurrentRow.Cells["Column3"].Value.ToString().Trim() != null)
                {
                    ReturnQty = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Column3"].Value);
                }

                if (dataGridView1.CurrentRow.Cells["Column4"].Value != null && dataGridView1.CurrentRow.Cells["Column4"].Value.ToString().Trim() != null)
                {
                    Price = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Column4"].Value);
                }

                int Now = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Column2"].Value) - ReturnQty;
                dataGridView1.CurrentRow.Cells["Column14"].Value = Now.ToString();


                if (dataGridView1.CurrentRow.Cells["Column14"].Value != null && dataGridView1.CurrentRow.Cells["Column14"].Value.ToString().Trim() != null)
                {
                    NewQty = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Column14"].Value);
                }

                decimal TotalPrice = ReturnQty * Price;
                dataGridView1.CurrentRow.Cells["Column5"].Value = Math.Round(TotalPrice,2).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            try
            {
                int namt = 0;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[2].Value != null)
                    {
                        namt = namt + int.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    }
                }
                this.txtReturnQty.Text = namt.ToString();
            }
            catch (Exception ex)
            {
                string name = ex.Message;
            }
            
            try
            {
                decimal namt = 0;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[5].Value != null)
                    {
                        namt = namt + decimal.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                    }
                }
                this.txtReturnPrice.Text = Math.Round(namt,2).ToString();
            }
            catch (Exception ex)
            {
                string name = ex.Message;
            }
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                decimal Price = 0;
                int ReturnQty = 0;
                int NewQty = 0;

                if (dataGridView2.CurrentRow.Cells["Column8"].Value != null && dataGridView2.CurrentRow.Cells["Column8"].Value.ToString().Trim() != null)
                {
                    ReturnQty = Convert.ToInt32(dataGridView2.CurrentRow.Cells["Column8"].Value);
                }

                if (dataGridView2.CurrentRow.Cells["Column10"].Value != null && dataGridView2.CurrentRow.Cells["Column10"].Value.ToString().Trim() != null)
                {
                    Price = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["Column10"].Value);
                }

                int Now = Convert.ToInt32(dataGridView2.CurrentRow.Cells["Column7"].Value) - ReturnQty;
                dataGridView2.CurrentRow.Cells["Column9"].Value = Now.ToString();


                if (dataGridView2.CurrentRow.Cells["Column9"].Value != null && dataGridView2.CurrentRow.Cells["Column9"].Value.ToString().Trim() != null)
                {
                    NewQty = Convert.ToInt32(dataGridView2.CurrentRow.Cells["Column9"].Value);
                }

                decimal TotalPrice = ReturnQty * Price;
                dataGridView2.CurrentRow.Cells["Column11"].Value = Math.Round(TotalPrice, 2).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                int namt = 0;
                for (int i = 0; i < dataGridView2.RowCount; i++)
                {
                    if (dataGridView2.Rows[i].Cells[2].Value != null)
                    {
                        namt = namt + int.Parse(dataGridView2.Rows[i].Cells[2].Value.ToString());
                    }
                }
                this.txtReplaceQty.Text = namt.ToString();
            }
            catch (Exception ex)
            {
                string name = ex.Message;
            }

            try
            {
                decimal namt = 0;
                for (int i = 0; i < dataGridView2.RowCount; i++)
                {
                    if (dataGridView2.Rows[i].Cells[5].Value != null)
                    {
                        namt = namt + decimal.Parse(dataGridView2.Rows[i].Cells[5].Value.ToString());
                    }
                }
                this.txtReplacePrice.Text = Math.Round(namt, 2).ToString();
            }
            catch (Exception ex)
            {
                string name = ex.Message;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (btnGenerate.Text == "New") 
            {
                Clear();
                Enable();

                string Salary = null;
                string Counter = null;
                string MonthYear = null;
                try
                {
                    string query = "select ISNULL( MAX(ORDER_NO),0) FROM Return_Replace_Header";
                    con.Open();
                    int i = 0;
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            Salary = sdr[0].ToString();
                            if (Salary == "0")
                            {
                                Counter = "0";
                                MonthYear = string.Format(@"{0:yyMM}", DateTime.Now);
                            }
                            else
                            {
                                Counter = Salary.ToString().Remove(0, 7);
                                MonthYear = Salary.ToString().Remove(7, 6);
                                MonthYear = MonthYear.ToString().Remove(0, 3);
                            }
                        }
                        string Result = string.Format(@"{0:yyMM}", DateTime.Now);
                        if (Result == MonthYear)
                        {
                            i = Convert.ToInt32(Counter);
                            i++;
                            Counter = "" + i;
                            Counter = i.ToString("D6");
                            string SID1 = "RR-" + Result + Counter;
                            txtOrderNo.Text = SID1;
                        }
                        else
                        {
                            string SID2 = "RR-" + Result + "000001";
                            txtOrderNo.Text = SID2;
                        }
                    }
                    sdr.Close();
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                btnGenerate.Text = "Generate";
            }
            else if (btnGenerate.Text == "Generate") 
            {
                if (comboProductStatus.Text == "")
                {
                    MessageBox.Show("Kindly Select the Product(s) e.g: Return, Replace Or Return And Replace", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else 
                {
                    if (comboProductStatus.Text == "Return") 
                    {
                        if (comboVenderName.Text != "" && txtVenderInvoice.Text != "")
                        {
                            try
                            {
                                try
                                {
                                    string query = "INSERT INTO Return_Replace_Header (ORDER_NO, VENDER_NAME, ORDER_DATE, VENDER_INVOICE, RECEIVED_DATE, PRODUCT_STATUS, USERNAME, DATETIME)";
                                    query = query + "VALUES ('" + txtOrderNo.Text + "','" + comboVenderName.Text + "','" + date1.Value.ToString("MM/dd/yyyy") + "','" + txtVenderInvoice.Text + "',";
                                    query = query + "'" + date2.Value.ToString("MM/dd/yyyy") + "','" + comboProductStatus.Text + "','" + UID + "','" + DateTime.Now + "')";
                                    con.Open();
                                    SqlCommand cmd = new SqlCommand(query, con);
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }

                                try
                                {
                                    con.Open();
                                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                                    {
                                        string query = "INSERT INTO Return_Detail (ORDER_NO, PRODUCT_NAME, RETURN_QTY, PRICE, TOTAL_PRICE, REASON, USERNAME, DATETIME, SIZE)";
                                        query = query + "VALUES ('" + txtOrderNo.Text + "', '";

                                        if (dataGridView1.Rows[i].Cells[0].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView1.Rows[i].Cells[0].Value.ToString() + "','";

                                        }

                                        if (dataGridView1.Rows[i].Cells[2].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView1.Rows[i].Cells[2].Value.ToString() + "','";
                                        }

                                        if (dataGridView1.Rows[i].Cells[4].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView1.Rows[i].Cells[4].Value.ToString() + "','";
                                        }

                                        if (dataGridView1.Rows[i].Cells[5].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView1.Rows[i].Cells[5].Value.ToString() + "','";
                                        }

                                        if (dataGridView1.Rows[i].Cells[6].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView1.Rows[i].Cells[6].Value.ToString() + "','";
                                        }

                                        query = query + UID + "', '" + DateTime.Now + "','";

                                        if (dataGridView1.Rows[i].Cells[7].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView1.Rows[i].Cells[7].Value.ToString() + "')";
                                        }
                                        SqlCommand cmd = new SqlCommand(query, con);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                con.Close();

                                try
                                {
                                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                                    {
                                        string query = " UPDATE add_product SET  QUANTITY ='" + dataGridView1.Rows[j].Cells[3].Value.ToString() + "' WHERE NAME = '" + dataGridView1.Rows[j].Cells[0].Value.ToString() + "' AND SIZE = '" + dataGridView1.Rows[j].Cells[7].Value.ToString() + "'";
                                        con.Open();
                                        SqlCommand cmd = new SqlCommand(query, con);
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            MessageBox.Show("The Order is Returned Now.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Return_Vender_Preview rvp = new Return_Vender_Preview();
                            rvp.passRetID = txtOrderNo.Text;
                            rvp.Show();
                            btnGenerate.Text = "New";
                            Disable();
                        }
                    }

                    if (comboProductStatus.Text == "Replace")
                    {
                        if (comboVenderName.Text != "" && txtVenderInvoice.Text != "")
                        {
                            try
                            {
                                try
                                {
                                    string query = "INSERT INTO Return_Replace_Header (ORDER_NO, VENDER_NAME, ORDER_DATE, VENDER_INVOICE, RECEIVED_DATE, PRODUCT_STATUS, USERNAME, DATETIME)";
                                    query = query + "VALUES ('" + txtOrderNo.Text + "','" + comboVenderName.Text + "','" + date1.Value.ToString("MM/dd/yyyy") + "','" + txtVenderInvoice.Text + "',";
                                    query = query + "'" + date2.Value.ToString("MM/dd/yyyy") + "','" + comboProductStatus.Text + "','" + UID + "','" + DateTime.Now + "')";
                                    con.Open();
                                    SqlCommand cmd = new SqlCommand(query, con);
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }

                                try
                                {
                                    con.Open();
                                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                                    {
                                        string query = "INSERT INTO Replace_Detail (ORDER_NO, PRODUCT_NAME, REPLACE_QTY, PRICE, TOTAL_PRICE, REPLACE_PRODUCT_NAME, REASON, USERNAME, DATETIME, SIZE)";
                                        query = query + "VALUES ('" + txtOrderNo.Text + "', '";

                                        if (dataGridView2.Rows[i].Cells[0].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView2.Rows[i].Cells[0].Value.ToString() + "','";

                                        }

                                        if (dataGridView2.Rows[i].Cells[2].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView2.Rows[i].Cells[2].Value.ToString() + "','";
                                        }

                                        if (dataGridView2.Rows[i].Cells[4].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView2.Rows[i].Cells[4].Value.ToString() + "','";
                                        }

                                        if (dataGridView2.Rows[i].Cells[5].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView2.Rows[i].Cells[5].Value.ToString() + "','";
                                        }

                                        if (dataGridView2.Rows[i].Cells[6].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView2.Rows[i].Cells[6].Value.ToString() + "','";
                                        }

                                        if (dataGridView2.Rows[i].Cells[7].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView2.Rows[i].Cells[7].Value.ToString() + "','";
                                        }

                                        query = query + UID + "', '" + DateTime.Now + "','";

                                        if (dataGridView2.Rows[i].Cells[8].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView2.Rows[i].Cells[8].Value.ToString() + "')";
                                        }
                                        SqlCommand cmd = new SqlCommand(query, con);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                con.Close();

                                try
                                {
                                    for (int j = 0; j < dataGridView2.Rows.Count; j++)
                                    {
                                        string query = " UPDATE add_product SET  QUANTITY ='" + dataGridView2.Rows[j].Cells[3].Value.ToString() + "' WHERE NAME = '" + dataGridView2.Rows[j].Cells[0].Value.ToString() + "' AND SIZE = '" + dataGridView2.Rows[j].Cells[8].Value.ToString() + "'";
                                        con.Open();
                                        SqlCommand cmd = new SqlCommand(query, con);
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            MessageBox.Show("The Order is Replaced Now.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Replace_Return_Preview rrp = new Replace_Return_Preview();
                            rrp.passRepID = txtOrderNo.Text;
                            rrp.Show();
                            btnGenerate.Text = "New"; 
                            Disable();
                        }
                    }

                    if (comboProductStatus.Text == "Return And Replace")
                    {
                        if (comboVenderName.Text != "" && txtVenderInvoice.Text != "")
                        {
                            try
                            {
                                try
                                {
                                    string query = "INSERT INTO Return_Replace_Header (ORDER_NO, VENDER_NAME, ORDER_DATE, VENDER_INVOICE, RECEIVED_DATE, PRODUCT_STATUS, USERNAME, DATETIME)";
                                    query = query + "VALUES ('" + txtOrderNo.Text + "','" + comboVenderName.Text + "','" + date1.Value.ToString("MM/dd/yyyy") + "','" + txtVenderInvoice.Text + "',";
                                    query = query + "'" + date2.Value.ToString("MM/dd/yyyy") + "','" + comboProductStatus.Text + "','" + UID + "','" + DateTime.Now + "')";
                                    con.Open();
                                    SqlCommand cmd = new SqlCommand(query, con);
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }

                                try
                                {
                                    con.Open();
                                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                                    {
                                        string query = "INSERT INTO Return_Detail (ORDER_NO, PRODUCT_NAME, RETURN_QTY, PRICE, TOTAL_PRICE, REASON, USERNAME, DATETIME, SIZE)";
                                        query = query + "VALUES ('" + txtOrderNo.Text + "', '";

                                        if (dataGridView1.Rows[i].Cells[0].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView1.Rows[i].Cells[0].Value.ToString() + "','";

                                        }

                                        if (dataGridView1.Rows[i].Cells[2].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView1.Rows[i].Cells[2].Value.ToString() + "','";
                                        }

                                        if (dataGridView1.Rows[i].Cells[4].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView1.Rows[i].Cells[4].Value.ToString() + "','";
                                        }

                                        if (dataGridView1.Rows[i].Cells[5].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView1.Rows[i].Cells[5].Value.ToString() + "','";
                                        }

                                        if (dataGridView1.Rows[i].Cells[6].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView1.Rows[i].Cells[6].Value.ToString() + "','";
                                        }

                                        query = query + UID + "', '" + DateTime.Now + "','";

                                        if (dataGridView1.Rows[i].Cells[7].Value == null)
                                        {
                                            query = query + "" + "','";
                                        }
                                        else
                                        {
                                            query = query + dataGridView1.Rows[i].Cells[7].Value.ToString() + "')";
                                        }

                                        SqlCommand cmd = new SqlCommand(query, con);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                con.Close();

                                try
                                {
                                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                                    {
                                        string query = " UPDATE add_product SET  QUANTITY ='" + dataGridView1.Rows[j].Cells[3].Value.ToString() + "' WHERE NAME = '" + dataGridView1.Rows[j].Cells[0].Value.ToString() + "' AND SIZE = '" + dataGridView1.Rows[j].Cells[7].Value.ToString() + "'";
                                        con.Open();
                                        SqlCommand cmd = new SqlCommand(query, con);
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }

                                    try
                                    {
                                        con.Open();
                                        for (int k = 0; k < dataGridView2.Rows.Count; k++)
                                        {
                                            string query = "INSERT INTO Replace_Detail (ORDER_NO, PRODUCT_NAME, REPLACE_QTY, PRICE, TOTAL_PRICE, REPLACE_PRODUCT_NAME, REASON, USERNAME, DATETIME, SIZE)";
                                            query = query + "VALUES ('" + txtOrderNo.Text + "', '";

                                            if (dataGridView2.Rows[k].Cells[0].Value == null)
                                            {
                                                query = query + "" + "','";
                                            }
                                            else
                                            {
                                                query = query + dataGridView2.Rows[k].Cells[0].Value.ToString() + "','";

                                            }

                                            if (dataGridView2.Rows[k].Cells[2].Value == null)
                                            {
                                                query = query + "" + "','";
                                            }
                                            else
                                            {
                                                query = query + dataGridView2.Rows[k].Cells[2].Value.ToString() + "','";
                                            }

                                            if (dataGridView2.Rows[k].Cells[4].Value == null)
                                            {
                                                query = query + "" + "','";
                                            }
                                            else
                                            {
                                                query = query + dataGridView2.Rows[k].Cells[4].Value.ToString() + "','";
                                            }

                                            if (dataGridView2.Rows[k].Cells[5].Value == null)
                                            {
                                                query = query + "" + "','";
                                            }
                                            else
                                            {
                                                query = query + dataGridView2.Rows[k].Cells[5].Value.ToString() + "','";
                                            }

                                            if (dataGridView2.Rows[k].Cells[6].Value == null)
                                            {
                                                query = query + "" + "','";
                                            }
                                            else
                                            {
                                                query = query + dataGridView2.Rows[k].Cells[6].Value.ToString() + "','";
                                            }

                                            if (dataGridView2.Rows[k].Cells[7].Value == null)
                                            {
                                                query = query + "" + "','";
                                            }
                                            else
                                            {
                                                query = query + dataGridView2.Rows[k].Cells[7].Value.ToString() + "','";
                                            }

                                            query = query + UID + "', '" + DateTime.Now + "','";

                                            if (dataGridView2.Rows[k].Cells[8].Value == null)
                                            {
                                                query = query + "" + "','";
                                            }
                                            else
                                            {
                                                query = query + dataGridView2.Rows[k].Cells[8].Value.ToString() + "')";
                                            }
                                            SqlCommand cmd = new SqlCommand(query, con);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                    con.Close();

                                    try
                                    {
                                        for (int l = 0; l < dataGridView2.Rows.Count; l++)
                                        {
                                            string query = " UPDATE add_product SET  QUANTITY ='" + dataGridView2.Rows[l].Cells[3].Value.ToString() + "' WHERE NAME = '" + dataGridView2.Rows[l].Cells[0].Value.ToString() + "' AND SIZE = '" + dataGridView2.Rows[l].Cells[8].Value.ToString() + "'";
                                            con.Open();
                                            SqlCommand cmd = new SqlCommand(query, con);
                                            cmd.ExecuteNonQuery();
                                            con.Close();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            MessageBox.Show("The Order is Return And Replace with Different Products.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Replace_Return_Preview rrp = new Replace_Return_Preview();
                            rrp.passRepID = txtOrderNo.Text;
                            rrp.Show();
                            btnGenerate.Text = "New";
                            Disable();
                        }
                    }
                }
            }
        }

        private void Disable()
        {
            txtOrderNo.Enabled = false;
            date1.Enabled = false;
            comboVenderName.Enabled = false;
            txtVenderInvoice.Enabled = false;
            date2.Enabled = false;
            comboProductStatus.Enabled = false;
        }

        private void Enable()
        {
            txtOrderNo.Enabled = true;
            date1.Enabled = true;
            comboVenderName.Enabled = true;
            txtVenderInvoice.Enabled = true;
            date2.Enabled = true;
            comboProductStatus.Enabled = true;
        }

        private void Clear()
        {
            txtOrderNo.Clear();
            date1.Value = DateTime.Now;
            comboVenderName.Text = "";
            comboVenderName.Items.Clear();
            txtVenderInvoice.Clear();
            date2.Value = DateTime.Now;
            comboProductStatus.SelectedIndex = -1;
            checkReturn.Checked = false;
            checkReplace.Checked = false;
            comboProductName.Text = "";
            comboProductName.Items.Clear();
            comboSize.Text = "";
            comboSize.SelectedIndex = -1;
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            txtReturnQty.Clear();
            txtReturnPrice.Clear();
            txtReplaceQty.Clear();
            txtReplacePrice.Clear();
        }

        private void comboVenderName_Click(object sender, EventArgs e)
        {
            comboVenderName.Items.Clear();
            Venders();
        }

        private void comboProductName_Click(object sender, EventArgs e)
        {
            comboProductName.Items.Clear();
            Products();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboProductName.Text != "" && comboSize.Text != "" && checkReturn.Checked == true)
                {
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM add_product WHERE NAME = '" + comboProductName.Text + "' AND SIZE = '" + comboSize.Text + "'", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    foreach (DataRow item in dt.Rows)
                    {
                        int i = dataGridView1.Rows.Add();

                        dataGridView1.Rows[i].Cells[0].Value = item["NAME"].ToString();
                        dataGridView1.Rows[i].Cells[1].Value = item["QUANTITY"].ToString();
                        dataGridView1.Rows[i].Cells[4].Value = item["PURCHASE_PRICE"].ToString();
                        dataGridView1.Rows[i].Cells[7].Value = item["SIZE"].ToString();
                    }
                }else
                if (comboProductName.Text != "" && comboSize.Text != "" && checkReplace.Checked == true)
                {
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM add_product WHERE NAME = '" + comboProductName.Text + "' AND SIZE = '" + comboSize.Text + "'", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dataGridView2.Rows.Add();

                        dataGridView2.Rows[n].Cells[0].Value = item["NAME"].ToString();
                        dataGridView2.Rows[n].Cells[1].Value = item["QUANTITY"].ToString();
                        dataGridView2.Rows[n].Cells[4].Value = item["PURCHASE_PRICE"].ToString();
                        dataGridView2.Rows[n].Cells[8].Value = item["SIZE"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Please select return or replace");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
