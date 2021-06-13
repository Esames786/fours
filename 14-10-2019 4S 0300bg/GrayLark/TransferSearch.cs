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
    public partial class TransferSearch : Form
    {
        TrasnferForm ts = null;
        public TransferSearch(TrasnferForm ts1)
        {
            InitializeComponent();
            ts = ts1;
            String query3 = "select Dates,TransferCode,TransferFrom,TransferTo,TransferAmount from tbl_transfer";
            DataTable dts3 = clsDataLayer.RetreiveQuery(query3);
            if(dts3.Rows.Count > 0)
            {
                grid.DataSource = dts3;
            }
            onload();
        }

        public void onload()
        {
            String a1 = "select TransferFrom from tbl_transfer";
            DataTable d1 = clsDataLayer.RetreiveQuery(a1); clsGeneral.SetAutoCompleteTextBox(txt_transferfrom, d1);

            String a2 = "select TransferTo from tbl_transfer";
            DataTable d2 = clsDataLayer.RetreiveQuery(a2); clsGeneral.SetAutoCompleteTextBox(txt_transferto, d2);

            String a3 = "select TransferAmount from tbl_transfer";
            DataTable d3 = clsDataLayer.RetreiveQuery(a3); clsGeneral.SetAutoCompleteTextBox(txt_transferamount, d3);
             
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {  ts.txt_code.Text = grid.CurrentRow.Cells[1].Value.ToString();
                this.Close();
            }catch{}
        }

        private void txt_transferfrom_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String q1 = "select Dates,TransferCode,TransferFrom,TransferTo,TransferAmount from tbl_transfer where TransferFrom='"+txt_transferfrom.Text+"'";
                DataTable dtg = clsDataLayer.RetreiveQuery(q1);
                if (dtg.Rows.Count > 0) { grid.DataSource = dtg; }
                else
                {
                    String query3 = "select Dates,TransferCode,TransferFrom,TransferTo,TransferAmount from tbl_transfer";
                    DataTable dts3 = clsDataLayer.RetreiveQuery(query3);
                    if (dts3.Rows.Count > 0)
                    {
                        grid.DataSource = dts3;
                    }
                }
            }
            catch {  }
        }

        private void txt_transferto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String q1 = "select Dates,TransferCode,TransferFrom,TransferTo,TransferAmount from tbl_transfer where TransferTo='" + txt_transferto.Text + "'";
                DataTable dtg = clsDataLayer.RetreiveQuery(q1);
                if (dtg.Rows.Count > 0) { grid.DataSource = dtg; }
                else
                {
                    String query3 = "select Dates,TransferCode,TransferFrom,TransferTo,TransferAmount from tbl_transfer";
                    DataTable dts3 = clsDataLayer.RetreiveQuery(query3);
                    if (dts3.Rows.Count > 0)
                    {
                        grid.DataSource = dts3;
                    }
                }
            }
            catch { }
        }

        private void txt_transferamount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String q1 = "select Dates,TransferCode,TransferFrom,TransferTo,TransferAmount from tbl_transfer where TransferAmount='" + txt_transferamount.Text + "'";
                DataTable dtg = clsDataLayer.RetreiveQuery(q1);
                if (dtg.Rows.Count > 0) { grid.DataSource = dtg; }
                else
                {
                    String query3 = "select Dates,TransferCode,TransferFrom,TransferTo,TransferAmount from tbl_transfer";
                    DataTable dts3 = clsDataLayer.RetreiveQuery(query3);
                    if (dts3.Rows.Count > 0)
                    {
                        grid.DataSource = dts3;
                    }
                }
            }
            catch { }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                String q1 = "select Dates,TransferCode,TransferFrom,TransferTo,TransferAmount from tbl_transfer where Dates='" + dateTimePicker1.Text + "'";
                DataTable dtg = clsDataLayer.RetreiveQuery(q1);
                if (dtg.Rows.Count > 0) { grid.DataSource = dtg; }
                else
                {
                    String query3 = "select Dates,TransferCode,TransferFrom,TransferTo,TransferAmount from tbl_transfer";
                    DataTable dts3 = clsDataLayer.RetreiveQuery(query3);
                    if (dts3.Rows.Count > 0)
                    {
                        grid.DataSource = dts3;
                    }
                }
            }
            catch { }
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            try
            {
                String q1 = "select Dates,TransferCode,TransferFrom,TransferTo,TransferAmount from tbl_transfer where Dates='" + dateTimePicker1.Text + "'";
                DataTable dtg = clsDataLayer.RetreiveQuery(q1);
                if (dtg.Rows.Count > 0) { grid.DataSource = dtg; }
                else
                {
                    String query3 = "select Dates,TransferCode,TransferFrom,TransferTo,TransferAmount from tbl_transfer";
                    DataTable dts3 = clsDataLayer.RetreiveQuery(query3);
                    if (dts3.Rows.Count > 0)
                    {
                        grid.DataSource = dts3;
                    }
                }
            }
            catch { }
        }
    }
}
