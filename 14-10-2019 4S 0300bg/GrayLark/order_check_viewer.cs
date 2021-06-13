using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyview
{
    public partial class order_check_viewer : Form
    {
        public order_check_viewer(Object obj)
        {
            InitializeComponent();
            crystalReportViewer1.ReportSource = obj; crystalReportViewer1.Refresh();

        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
