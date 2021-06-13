using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.SqlServer.Management.Smo;
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
using Microsoft.SqlServer.Management.Smo;

namespace Reddot_Express_Inventory
{
    public partial class POrderView : Form
    {
        public POrderView(String p1, String p2, String p3, String p4, String p5, String v1, String v2, String v3, String v4, String v5, String from, String To, Object obj,String p6,String p7,String p8,String p9,String p10,String v6,String v7,String v8,String v9,String v10)
        {
            InitializeComponent();
           // SendEmail(p1,p2,p3,p4,p5,v1,v2,v3,v4,v5,from,To,obj);

            ParameterFields pfield = new ParameterFields();
            
            //
            ParameterField l11 = new ParameterField();
            ParameterDiscreteValue b11 = new ParameterDiscreteValue();
            l11.ParameterFieldName = "product6";
            b11.Value = p6;
            l11.CurrentValues.Add(b11);

            ParameterField l12 = new ParameterField();
            ParameterDiscreteValue b112 = new ParameterDiscreteValue();
            l12.ParameterFieldName = "product7";
            b112.Value = p7;
            l12.CurrentValues.Add(b112);

            ParameterField l13 = new ParameterField();
            ParameterDiscreteValue b113 = new ParameterDiscreteValue();
            l13.ParameterFieldName = "product8";
            b113.Value = p8;
            l13.CurrentValues.Add(b113);

            ParameterField l14 = new ParameterField();
            ParameterDiscreteValue b114 = new ParameterDiscreteValue();
            l14.ParameterFieldName = "product9";
            b114.Value = p9;
            l14.CurrentValues.Add(b114);

            ParameterField l15 = new ParameterField();
            ParameterDiscreteValue b115 = new ParameterDiscreteValue();
            l15.ParameterFieldName = "product10";
            b115.Value = p10;
            l15.CurrentValues.Add(b115);

            //
            ParameterField l16 = new ParameterField();
            ParameterDiscreteValue b116 = new ParameterDiscreteValue();
            l16.ParameterFieldName = "q6";
            b116.Value = v6;
            l16.CurrentValues.Add(b116);

            ParameterField l17 = new ParameterField();
            ParameterDiscreteValue b117 = new ParameterDiscreteValue();
            l17.ParameterFieldName = "q7";
            b117.Value = v7;
            l17.CurrentValues.Add(b117);

            ParameterField l18 = new ParameterField();
            ParameterDiscreteValue b118 = new ParameterDiscreteValue();
            l18.ParameterFieldName = "q8";
            b118.Value = v8;
            l18.CurrentValues.Add(b118);

            ParameterField l19 = new ParameterField();
            ParameterDiscreteValue b119 = new ParameterDiscreteValue();
            l19.ParameterFieldName = "q9";
            b119.Value = v9;
            l19.CurrentValues.Add(b119);

            ParameterField l20 = new ParameterField();
            ParameterDiscreteValue b1120 = new ParameterDiscreteValue();
            l20.ParameterFieldName = "q10";
            b1120.Value = v10;
            l20.CurrentValues.Add(b1120);
            //
            // 

   


            //
            
            
            ParameterField ptitle = new ParameterField();
            ParameterDiscreteValue pvalue = new ParameterDiscreteValue();
            ptitle.ParameterFieldName = "username";
            pvalue.Value = Login.UserID;
            ptitle.CurrentValues.Add(pvalue);

            //
            ParameterField d1 = new ParameterField();
            ParameterDiscreteValue c1 = new ParameterDiscreteValue();
            d1.ParameterFieldName = "from";
            c1.Value = from;
            d1.CurrentValues.Add(c1);

            ParameterField d2 = new ParameterField();
            ParameterDiscreteValue c2 = new ParameterDiscreteValue();
            d2.ParameterFieldName = "todate";
            c2.Value = To;
            d2.CurrentValues.Add(c2);
            //

            ParameterField l1 = new ParameterField();
            ParameterDiscreteValue b1 = new ParameterDiscreteValue();
            l1.ParameterFieldName = "product1";
            b1.Value = p1;
            l1.CurrentValues.Add(b1);

            ParameterField l2 = new ParameterField();
            ParameterDiscreteValue b2 = new ParameterDiscreteValue();
            l2.ParameterFieldName = "product2";
            b2.Value = p2;
            l2.CurrentValues.Add(b2);

            ParameterField l3 = new ParameterField();
            ParameterDiscreteValue b3 = new ParameterDiscreteValue();
            l3.ParameterFieldName = "product3";
            b3.Value = p3;
            l3.CurrentValues.Add(b3);

            ParameterField l4 = new ParameterField();
            ParameterDiscreteValue b4 = new ParameterDiscreteValue();
            l4.ParameterFieldName = "product4";
            b4.Value = p4;
            l4.CurrentValues.Add(b4);

            ParameterField l5 = new ParameterField();
            ParameterDiscreteValue b5 = new ParameterDiscreteValue();
            l5.ParameterFieldName = "product5";
            b5.Value = p5;
            l5.CurrentValues.Add(b5);

            //
            ParameterField l6 = new ParameterField();
            ParameterDiscreteValue b6 = new ParameterDiscreteValue();
            l6.ParameterFieldName = "q1";
            b6.Value = v1;
            l6.CurrentValues.Add(b6);

            ParameterField l7 = new ParameterField();
            ParameterDiscreteValue b7 = new ParameterDiscreteValue();
            l7.ParameterFieldName = "q2";
            b7.Value = v2;
            l7.CurrentValues.Add(b7);

            ParameterField l8 = new ParameterField();
            ParameterDiscreteValue b8 = new ParameterDiscreteValue();
            l8.ParameterFieldName = "q3";
            b8.Value = v3;
            l8.CurrentValues.Add(b8);

            ParameterField l9 = new ParameterField();
            ParameterDiscreteValue b9 = new ParameterDiscreteValue();
            l9.ParameterFieldName = "q4";
            b9.Value = v4;
            l9.CurrentValues.Add(b9);

            ParameterField l10 = new ParameterField();
            ParameterDiscreteValue b10 = new ParameterDiscreteValue();
            l10.ParameterFieldName = "q5";
            b10.Value = v5;
            l10.CurrentValues.Add(b10);
            //
            //
            pfield.Add(d1);
            pfield.Add(d2);

            pfield.Add(l1);
            pfield.Add(l2);
            pfield.Add(l3);
            pfield.Add(l4);
            pfield.Add(l5);
            pfield.Add(l6);
            pfield.Add(l7);
            pfield.Add(l8);
            pfield.Add(l9);
            pfield.Add(l10);
            //
            pfield.Add(l11);
            pfield.Add(l12);
            pfield.Add(l13);
            pfield.Add(l14);
            pfield.Add(l15);
            pfield.Add(l16);
            pfield.Add(l17);
            pfield.Add(l18);
            pfield.Add(l19);
            pfield.Add(l20);
            //
             
            pfield.Add(ptitle);
            crystalReportViewer1.ParameterFieldInfo = pfield;

            crystalReportViewer1.ReportSource = obj;
            crystalReportViewer1.Refresh();
        }


        private void SendEmail(String p1, String p2, String p3, String p4, String p5, String v1, String v2, String v3, String v4, String v5, String from, String To, Object obj)
        { 
                    //CrystalDecisions.CrystalReports.Engine.Database crDatabase;
                    //TableLogOnInfo crTableLogonInfo;
                    //Tables crTables;
                    //ConnectionInfo crConnectionInfo;
                    ////ReportDocument cryRpt = new ReportDocument();
                    ////string Apppath = Path.GetDirectoryName(Application.ExecutablePath);
                    ////Apppath = Path.Combine(Apppath, "Report\\ProductOrderLatest.rpt");
                    ////cryRpt.Load(Apppath);
                    //string[] con = File.ReadAllLines("ConnectionSetup.txt");
                    //crConnectionInfo = new ConnectionInfo();
                    //crConnectionInfo.ServerName = con[0];
                    //crConnectionInfo.DatabaseName = con[1];
                    //crConnectionInfo.UserID = con[2];
                    //crConnectionInfo.Password = con[3];
                    ////crDatabase = cryRpt.Database;
                    //crTables = crDatabase.Tables;
                    //foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
                    //{
                    //    crTableLogonInfo = crTable.LogOnInfo;
                    //    crTableLogonInfo.ConnectionInfo = crConnectionInfo;
                    //    crTable.ApplyLogOnInfo(crTableLogonInfo);
                    //}

#region parameter

                    ParameterFields pfield = new ParameterFields();
                    ParameterField ptitle = new ParameterField();
                    ParameterDiscreteValue pvalue = new ParameterDiscreteValue();
                    ptitle.ParameterFieldName = "username";
                    pvalue.Value = Login.UserID;
                    ptitle.CurrentValues.Add(pvalue);

                    //
                    ParameterField d1 = new ParameterField();
                    ParameterDiscreteValue c1 = new ParameterDiscreteValue();
                    d1.ParameterFieldName = "from";
                    c1.Value = from;
                    d1.CurrentValues.Add(c1);

                    ParameterField d2 = new ParameterField();
                    ParameterDiscreteValue c2 = new ParameterDiscreteValue();
                    d2.ParameterFieldName = "todate";
                    c2.Value = To;
                    d2.CurrentValues.Add(c2);
                    //

                    ParameterField l1 = new ParameterField();
                    ParameterDiscreteValue b1 = new ParameterDiscreteValue();
                    l1.ParameterFieldName = "product1";
                    b1.Value = p1;
                    l1.CurrentValues.Add(b1);

                    ParameterField l2 = new ParameterField();
                    ParameterDiscreteValue b2 = new ParameterDiscreteValue();
                    l2.ParameterFieldName = "product2";
                    b2.Value = p2;
                    l2.CurrentValues.Add(b2);

                    ParameterField l3 = new ParameterField();
                    ParameterDiscreteValue b3 = new ParameterDiscreteValue();
                    l3.ParameterFieldName = "product3";
                    b3.Value = p3;
                    l3.CurrentValues.Add(b3);

                    ParameterField l4 = new ParameterField();
                    ParameterDiscreteValue b4 = new ParameterDiscreteValue();
                    l4.ParameterFieldName = "product4";
                    b4.Value = p4;
                    l4.CurrentValues.Add(b4);

                    ParameterField l5 = new ParameterField();
                    ParameterDiscreteValue b5 = new ParameterDiscreteValue();
                    l5.ParameterFieldName = "product5";
                    b5.Value = p5;
                    l5.CurrentValues.Add(b5);

                    //
                    ParameterField l6 = new ParameterField();
                    ParameterDiscreteValue b6 = new ParameterDiscreteValue();
                    l6.ParameterFieldName = "q1";
                    b6.Value = v1;
                    l6.CurrentValues.Add(b6);

                    ParameterField l7 = new ParameterField();
                    ParameterDiscreteValue b7 = new ParameterDiscreteValue();
                    l7.ParameterFieldName = "q2";
                    b7.Value = v2;
                    l7.CurrentValues.Add(b7);

                    ParameterField l8 = new ParameterField();
                    ParameterDiscreteValue b8 = new ParameterDiscreteValue();
                    l8.ParameterFieldName = "q3";
                    b8.Value = v3;
                    l8.CurrentValues.Add(b8);

                    ParameterField l9 = new ParameterField();
                    ParameterDiscreteValue b9 = new ParameterDiscreteValue();
                    l9.ParameterFieldName = "q4";
                    b9.Value = v4;
                    l9.CurrentValues.Add(b9);

                    ParameterField l10 = new ParameterField();
                    ParameterDiscreteValue b10 = new ParameterDiscreteValue();
                    l10.ParameterFieldName = "q5";
                    b10.Value = v5;
                    l10.CurrentValues.Add(b10);
                    //
                    //
                    pfield.Add(d1);
                    pfield.Add(d2);

                    pfield.Add(l1);
                    pfield.Add(l2);
                    pfield.Add(l3);
                    pfield.Add(l4);
                    pfield.Add(l5);
                    pfield.Add(l6);
                    pfield.Add(l7);
                    pfield.Add(l8);
                    pfield.Add(l9);
                    pfield.Add(l10);
             
                    pfield.Add(ptitle);
                    crystalReportViewer1.ParameterFieldInfo = pfield;
#endregion parameter

                    //crystalReportViewer1.ReportSource = cryRpt;
                    //crystalReportViewer1.Refresh();

                    //string path = Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf("Debug"));
                    //path = Path.Combine(path, @"Debug\OrdersWeekly\"+DateTime.Now.ToString("dd-MMM-yyyy hh.mm")+".pdf");

                    //cryRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);

              
        }
    
    }
}
