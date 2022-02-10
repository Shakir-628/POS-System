namespace ChiuMartSAIS2.Reports
{
    partial class frmTokenReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.vw_ORReportBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.chiumart_data = new ChiuMartSAIS2.chiumart_data();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.vwORReportBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vw_ORReportTableAdapter = new ChiuMartSAIS2.chiumart_dataTableAdapters.vw_ORReportTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.vw_ORReportBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chiumart_data)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vwORReportBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // vw_ORReportBindingSource
            // 
            this.vw_ORReportBindingSource.DataMember = "vw_ORReport";
            this.vw_ORReportBindingSource.DataSource = this.chiumart_data;
            // 
            // chiumart_data
            // 
            this.chiumart_data.DataSetName = "chiumart_data";
            this.chiumart_data.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.vw_ORReportBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "ChiuMartSAIS2.Reports.rptTokenReport.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(907, 492);
            this.reportViewer1.TabIndex = 0;
            this.reportViewer1.RenderingComplete += new Microsoft.Reporting.WinForms.RenderingCompleteEventHandler(this.reportViewer1_RenderingComplete);
            // 
            // vwORReportBindingSource
            // 
            this.vwORReportBindingSource.DataMember = "vw_ORReport";
            this.vwORReportBindingSource.DataSource = this.chiumart_data;
            // 
            // vw_ORReportTableAdapter
            // 
            this.vw_ORReportTableAdapter.ClearBeforeFill = true;
            // 
            // frmTokenReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 492);
            this.Controls.Add(this.reportViewer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTokenReport";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Invoice";
            this.Load += new System.EventHandler(this.frmTokenReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.vw_ORReportBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chiumart_data)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vwORReportBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource vwORReportBindingSource;
        private chiumart_data chiumart_data;
        private chiumart_dataTableAdapters.vw_ORReportTableAdapter vw_ORReportTableAdapter;
        private System.Windows.Forms.BindingSource vw_ORReportBindingSource;
    }
}