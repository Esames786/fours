using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Data.SqlClient;
namespace GrayLark
{
    public partial class ConnectServer : Form
    {
        public ConnectServer()
        {
            InitializeComponent();
            String re = "select ServerName,UserId,Passwords from tbl_Connection"; DataTable d1 = clsDataLayer.RetreiveQuery(re); if (d1.Rows.Count > 0)
            {
                txtname.Text = d1.Rows[0][0].ToString();
                txtlogin.Text = d1.Rows[0][1].ToString();
                txtpass.Text = d1.Rows[0][2].ToString();
            }
        }

        private void btnconnect_Click(object sender, EventArgs e)
        {
            try
            {
                String del = "truncate table tbl_Connection"; clsDataLayer.ExecuteQuery(del);
                String ins = " insert into tbl_Connection(ServerName,UserId,Passwords,Path)values ('" + txtname.Text + "','" + txtlogin.Text + "','" + txtpass.Text + "','" + txtpath.Text + "')"; clsDataLayer.ExecuteQuery(ins);
                MessageBox.Show("Record Inserted");
            }catch{}
        } 
        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnbrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
             try
            {
                if (result == DialogResult.OK)
                {
                    string[] files = Directory.GetDirectories(folderBrowserDialog1.SelectedPath);
                    txtpath.Text = files[0];
                }
            }
            catch  {  }
        }
    }
}
