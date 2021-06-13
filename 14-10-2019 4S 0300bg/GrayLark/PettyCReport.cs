using CrystalDecisions.Shared;
using GrayLark.bin.Debug.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrayLark
{
    public partial class PettyCReport : Form
    {
        public PettyCReport(String User, String Date1, String Date2, String Total, String pb)
        {
            InitializeComponent();
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
            con.Open();
            SqlDataAdapter adap = new SqlDataAdapter();
            SqlCommand sqlcmd = new SqlCommand("ProdRep3", con);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@User", User);
            sqlcmd.Parameters.AddWithValue("@DATE11", Date1);
            sqlcmd.Parameters.AddWithValue("@DATE22", Date2);

            adap.SelectCommand = sqlcmd;
            DataSet ds = new DataSet();
            adap.Fill(ds);
            con.Close();
            LatestPettyCash pe = new LatestPettyCash();
            //
          
            //
            pe.Database.Tables["PettyView"].SetDataSource(ds.Tables[0]);

            pe.Database.Tables["tbl_IssuePettyDate"].SetDataSource(ds.Tables[1]);

            decimal tt = 0; String a1 = "select sum(Issue_Amount) from tbl_IssuePettyDate where Petty_User='" + User + "' and Date between '" + Date1 + "' and '" + Date2 + "'"; DataTable d1 = clsDataLayer.RetreiveQuery(a1);
            if (d1.Rows.Count > 0)
            {
                String hs = d1.Rows[0][0].ToString();
                if (hs.Equals(""))
                {
                    tt = 0;
                }
                else
                {
                    tt = Convert.ToDecimal(d1.Rows[0][0].ToString());
                }
            }
            ParameterFields pfield = new ParameterFields();
            ParameterField ptitle = new ParameterField();
            ParameterDiscreteValue pvalue = new ParameterDiscreteValue();
            ptitle.ParameterFieldName = "Date_From";
            pvalue.Value = Date1;
            ptitle.CurrentValues.Add(pvalue);

            ParameterField ptitle1 = new ParameterField();
            ParameterDiscreteValue pvalue1 = new ParameterDiscreteValue();
            ptitle1.ParameterFieldName = "Date_To";
            pvalue1.Value = Date2;
            ptitle1.CurrentValues.Add(pvalue1);

            ParameterField ptitle3 = new ParameterField();
            ParameterDiscreteValue pvalue3 = new ParameterDiscreteValue();
            ptitle3.ParameterFieldName = "Total";
            pvalue3.Value = tt;
            ptitle3.CurrentValues.Add(pvalue3);

            ParameterField ptitle4 = new ParameterField();
            ParameterDiscreteValue pvalue4 = new ParameterDiscreteValue();
            ptitle4.ParameterFieldName = "Pof";
            pvalue4.Value = User;
            ptitle4.CurrentValues.Add(pvalue4);

            //
            ParameterField a5 = new ParameterField();
            ParameterDiscreteValue b1 = new ParameterDiscreteValue();
            a5.ParameterFieldName = "Ptotal";
            b1.Value = Total;
            a5.CurrentValues.Add(b1);

            ParameterField r1 = new ParameterField();
            ParameterDiscreteValue r2 = new ParameterDiscreteValue();
            r1.ParameterFieldName = "pb";
            r2.Value = pb;
            r1.CurrentValues.Add(r2);

            //ParameterField a3 = new ParameterField();
            //ParameterDiscreteValue b3 = new ParameterDiscreteValue();
            //a3.ParameterFieldName = "cp";
            //b3.Value = cp;
            //a3.CurrentValues.Add(b3);
            //
            //pfield.Add(a3);
            pfield.Add(a5); pfield.Add(r1); 
          
            pfield.Add(ptitle); pfield.Add(ptitle1); pfield.Add(ptitle3); pfield.Add(ptitle4);
            crystalReportViewer1.ParameterFieldInfo = pfield;
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = pe;
            crystalReportViewer1.Refresh();
        }

        private void PettyCReport_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
