using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.Shared; 
using System.IO;

namespace GrayLark
{
    public partial class Return_Preview : Form
    {
        public Return_Preview(String Usr,String from,String to,Object obj)
        {
            InitializeComponent();
            this.BackColor = Color.SteelBlue;
            try
            {
            ParameterFields pfield = new ParameterFields();
            
            ParameterField USER = new ParameterField();
            ParameterDiscreteValue u = new ParameterDiscreteValue();
            USER.ParameterFieldName = "username";
            u.Value = Usr;
            USER.CurrentValues.Add(u);
            
            ParameterField FROM = new ParameterField();
            ParameterDiscreteValue f = new ParameterDiscreteValue();
            FROM.ParameterFieldName = "from";
            f.Value = from;
            FROM.CurrentValues.Add(f);
            
            ParameterField TO = new ParameterField();
            ParameterDiscreteValue t = new ParameterDiscreteValue();
            TO.ParameterFieldName = "to";
            t.Value = to;
            TO.CurrentValues.Add(t);
            
            pfield.Add(USER);
            pfield.Add(FROM);
            pfield.Add(TO);
            
            crystalReportViewer1.ParameterFieldInfo = pfield;
            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       
    }
}
