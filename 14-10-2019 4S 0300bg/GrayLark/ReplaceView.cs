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
    public partial class ReplaceView : Form
    {
        public ReplaceView(Object obj)
        {
            InitializeComponent();
            crystalReportViewer1.ReportSource = obj;
         crystalReportViewer1.Refresh();
        }
    }
}
