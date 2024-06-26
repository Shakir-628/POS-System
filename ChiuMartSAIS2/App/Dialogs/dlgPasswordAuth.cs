﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace ChiuMartSAIS2.App.Dialogs
{

    public enum FormType
    {
        productMaintenance,
        categoryMaintenance,
        unitsMaintenance,
        suppliersMaintenance,
        clientsMaintenance,
        usersMaintenance,
        permissionsMaintenance,
        pointOfSales,
        inventoryMonitoring,
        purchaseOrder,
        chequeMonitoring,
        inventoryReport,
        salesReport,
        logsReport,
        userReport,
        clientReport,
        supplierReport,
        manualInventoryCheck,
        idleIntervalSettings,
        EndShiftMonitoring,
        SalesReturn
    }

    public partial class dlgPasswordAuth : Form
    {

        private Classes.Configuration conf;
        private Classes.StringHash stringHash;

        public bool idleMode = false;

        public FormType formToOpen;

        public dlgPasswordAuth()
        {
            InitializeComponent();

            conf = new Classes.Configuration();
            stringHash = new Classes.StringHash();
        }

        private void openForm()
        {
            switch (formToOpen)
            {
                case FormType.productMaintenance:
                    if (Classes.Authentication.Instance.products == 1)
                    {
                        App.frmProduct frm = new App.frmProduct();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.EndShiftMonitoring:
                    if (Classes.Authentication.Instance.endshiftMonitoring == 1)
                    {
                        App.frmEndShift frm = new App.frmEndShift();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.SalesReturn:
                    if (Classes.Authentication.Instance.SalesReturn == 1)
                    {
                        App.frmRefund frm = new App.frmRefund(null, null, null, null, "", "", "", "");
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.categoryMaintenance:
                    if (Classes.Authentication.Instance.categories == 1)
                    {
                        App.frmCategory frm = new App.frmCategory();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.unitsMaintenance:
                    if (Classes.Authentication.Instance.units == 1)
                    {
                        App.frmUnits frm = new App.frmUnits();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.suppliersMaintenance:
                    if (Classes.Authentication.Instance.suppliers == 1)
                    {
                        App.frmSupplier frm = new App.frmSupplier();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.clientsMaintenance:
                    if (Classes.Authentication.Instance.clients == 1)
                    {
                        App.frmClient frm = new App.frmClient();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.usersMaintenance:
                    if (Classes.Authentication.Instance.users == 1)
                    {
                        App.frmUsers frm = new App.frmUsers();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.permissionsMaintenance:
                    if (Classes.Authentication.Instance.permissions == 1)
                    {
                        App.frmPermissions frm = new App.frmPermissions();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.pointOfSales:
                    if (Classes.Authentication.Instance.pointOfSale == 1)
                    {
                        App.frmPOS frm = new App.frmPOS(null, null, null, null, "", "", "", "");
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.inventoryMonitoring:
                    if (Classes.Authentication.Instance.inventoryMonitoring == 1)
                    {
                        App.frmInventoryMonitoring frm = new App.frmInventoryMonitoring();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.purchaseOrder:
                    if (Classes.Authentication.Instance.purchaseOrder == 1)
                    {
                        App.frmPO frm = new App.frmPO();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.chequeMonitoring:
                    if (Classes.Authentication.Instance.chequeMonitoring == 1)
                    {
                        App.frmChequeMonitoring frm = new App.frmChequeMonitoring();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.inventoryReport:
                    if (Classes.Authentication.Instance.inventoryReport == 1)
                    {
                        App.ReportDialog.dlgProductReport frm = new App.ReportDialog.dlgProductReport();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.userReport:
                    if (Classes.Authentication.Instance.usersReport == 1)
                    {
                        App.ReportDialog.dlgUserReport frm = new App.ReportDialog.dlgUserReport();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.clientReport:
                    if (Classes.Authentication.Instance.clientReport == 1)
                    {
                        App.ReportDialog.dlgClientListReport frm = new App.ReportDialog.dlgClientListReport();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;

                case FormType.supplierReport:
                    if (Classes.Authentication.Instance.supplierReport == 1)
                    {
                        App.ReportDialog.dlgSupplierReport frm = new App.ReportDialog.dlgSupplierReport();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.logsReport:
                    if (Classes.Authentication.Instance.logsReport == 1)
                    {
                        App.ReportDialog.dlgAuditTrailReport frm = new App.ReportDialog.dlgAuditTrailReport();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.salesReport:
                    if (Classes.Authentication.Instance.salesReport == 1)
                    {
                        App.ReportMenuDialogs.dlgSalesReportMenu frm = new App.ReportMenuDialogs.dlgSalesReportMenu();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case FormType.manualInventoryCheck:
                    if (Classes.Authentication.Instance.manualInventoryReport == 1)
                    {
                        App.ReportDialog.dlgProductReport frm = new App.ReportDialog.dlgProductReport();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry but you don't have a permission to open this feature", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
               
            }
        }
        private void clickauth()
        { // Authenticate the user
            //Classes.Authentication.Instance.userLogin(stringHash.hashIt(txtPassword.Text));
            Classes.Authentication.Instance.userLoginByUsername();

            // Check the user permission
            if (txtPassword.Text == "pos")
            {
                // If superuser backdoor
                // Give full permission
                //Classes.Authentication.Instance.fullPermission();
            }

            if (this.idleMode)
            {
                DialogResult = DialogResult.OK;
                return;
            }

            openForm();
            //frmMain frm = new frmMain();
            // frm.ShowDialog();
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            clickauth();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dlgPasswordAuth_Load(object sender, EventArgs e)
        {
           // Classes.Authentication.Instance.resetPermission();
            clickauth();
        }

        private void dlgPasswordAuth_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
