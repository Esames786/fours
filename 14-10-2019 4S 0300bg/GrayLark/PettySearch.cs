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
    public partial class PettySearch : Form
    {
        frmPettyCash pc;
        public PettySearch(frmPettyCash ps)
        {
            InitializeComponent(); onLoad();
            pc = ps;
        }

        private void onLoad()
        {
        String query = "";
        query = "select * from PettyView";
        DataTable d1 = clsDataLayer.RetreiveQuery(query);
        if (d1.Rows.Count > 0) { dataGridView1.DataSource = d1; }
        }
        private void txt_search_TextChanged(object sender, EventArgs e)
        {
        try{ 
        String query = "";
        query = "select * from PettyView where " + cmb.Text + " = '" + txt_search.Text + "'";
        DataTable d1 = clsDataLayer.RetreiveQuery(query);
        if (d1.Rows.Count > 0) { dataGridView1.DataSource = d1; } else { onLoad(); }
        } catch { }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        pc.txt_code.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        pc.btnEdit.Enabled = true;
        this.Close();
        }

        private void cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
//            Pe_Code
//Project_Code
//Project_Name
//Petty_User
            String q1 = "select " + cmb.Text + " from PettyView"; DataTable ds = clsDataLayer.RetreiveQuery(q1); if (ds.Rows.Count > 0)
            {
                clsGeneral.SetAutoCompleteTextBox(txt_search, ds);
            }
        }

        private void PettySearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
