using ChiuMartSAIS2.Classes;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChiuMartSAIS2.Reports
{
    public partial class frmTokenReport : Form
    {
        public string orno = "";

        public frmTokenReport()
        {
            InitializeComponent();
        }

        private void frmTokenReport_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
            int or = Int32.Parse(orno);

            //this.vw_ORReportTableAdapter.Fill(this.chiumart_data.vw_ORReport, or);
            //  this.
            dbHelper objdbhelp = new dbHelper();


            DataTable dt = objdbhelp.getorReport(orno);
            DataTable dt1 = objdbhelp.getDetails();

            reportViewer1.Visible = true;
            //reportViewer1.LocalReport.ReportPath = "Report1.rdlc";
            reportViewer1.LocalReport.DataSources.Clear();
            ReportParameter p1 = new ReportParameter("orno", orno);
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));

            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", dt1));

            //this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1 });
            this.reportViewer1.RefreshReport();
           
            // reportViewer1.LocalReport.SetParameters(orno);
           // reportViewer1.PrintDialog();// = true;
        }
        private void AutoPrint()
        {
            AutoPrintCls autoprintme = new AutoPrintCls(reportViewer1.LocalReport);
            autoprintme.Print();
        }

        private void reportViewer1_RenderingComplete(object sender, RenderingCompleteEventArgs e)
        {
            AutoPrint();
        }

        //


    }
}
