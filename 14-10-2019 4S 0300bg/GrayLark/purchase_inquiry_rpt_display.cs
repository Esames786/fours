using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrayLark
{
    public partial class purchase_inquiry_rpt_display : Form
    {
        public purchase_inquiry_rpt_display(Object obj,String qm)
        {
            InitializeComponent();
            SendEmail(obj,qm); 
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        } 
        private void SendEmail(Object obj,String q)
        { 
                CrystalDecisions.CrystalReports.Engine.Database crDatabase;
                TableLogOnInfo crTableLogonInfo;
                Tables crTables;
                ConnectionInfo crConnectionInfo;
                ReportDocument cryRpt = new ReportDocument();
                string Apppath = Path.GetDirectoryName(Application.ExecutablePath);
                Apppath = Path.Combine(Apppath, "Report\\rptPurchaseInquiry.rpt");
                cryRpt.Load(Apppath); String v4 = "";
                String m = "select * from VU_PINQ where PIurCode ='" + q + "'";
                DataTable dr = clsDataLayer.RetreiveQuery(m);
                if (dr.Rows.Count > 0) { cryRpt.SetDataSource(dr); v4 = dr.Rows[0]["VendorName"].ToString(); } 

                crConnectionInfo = new ConnectionInfo();
                string[] con = File.ReadAllLines("ConnectionSetup.txt");
                crConnectionInfo.ServerName = con[0];
                crConnectionInfo.DatabaseName = con[1];
                crConnectionInfo.UserID = con[2];
                crConnectionInfo.Password = con[3];
                crDatabase = cryRpt.Database;
                crTables = crDatabase.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
                {
                    crTableLogonInfo = crTable.LogOnInfo;
                    crTableLogonInfo.ConnectionInfo = crConnectionInfo;
                    crTable.ApplyLogOnInfo(crTableLogonInfo);
                }
            crystalReportViewer1.ReportSource = cryRpt;
                crystalReportViewer1.Refresh();
                string path = Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf("Debug"));

                String name = "Inquiry " + DateTime.Now.ToString("dd-MM-yyyy") + q + DateTime.Now.ToString("MM_ss");
                path = Path.Combine(path, @"Debug\ " + name + ".pdf");
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                bool find = path.Contains(name); 
                cryRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);

               SendEmails(path, v4); 
        }



        private void SendEmails(String Path,String vendor)
        {
            try
            {
                String E1 = "";
                String fet = "select Email from Customer where FullName='" + vendor + "'"; DataTable dm = clsDataLayer.RetreiveQuery(fet);
                if (dm.Rows.Count > 0)
                {
                    E1 = dm.Rows[0][0].ToString();
                    // Credentials
                    var credentials = new NetworkCredential("salmanidrees012@gmail.com", "salman1234***");

                    // Mail message
                    var mail = new System.Net.Mail.MailMessage()
                    {
                        From = new MailAddress("salmanidrees012@gmail.com"),
                        Subject = "Purchase Inquiry " + DateTime.Now.ToString("dd-MM-yyyy HH:MM"),
                        Body = "Check this Attachement"
                    };


                    mail.To.Add(new MailAddress(E1));
                    Attachment attach = new Attachment(Path);
                    mail.Attachments.Add(attach);
                    // Smtp client
                    var client = new SmtpClient()
                    {
                        Port = 587,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Host = "smtp.gmail.com",
                        EnableSsl = true,
                        Credentials = credentials
                    };

                    // Send it...         
                    client.Send(mail);
                    MessageBox.Show("File Send Success!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else { MessageBox.Show("Add Vendor Email!", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in sending email: " + ex.Message);

                return;
            }
        }
        //



    }
}
