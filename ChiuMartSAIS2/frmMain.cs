using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChiuMartSAIS2.Classes;

namespace ChiuMartSAIS2
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //TODO: SHOW CONFIRMATION DIALOG FIRST AND ASK THE USER BEFORE THEY EXIT THE PROGRAM
            this.Close();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.productMaintenance;
            //App.frmProduct frm = new App.frmProduct();
            frm.ShowDialog();
        }

        private void btnUnits_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.unitsMaintenance;
            //App.frmUnits frm = new App.frmUnits();
            frm.ShowDialog();
        }

        private void btnCategories_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.categoryMaintenance;
            //App.frmCategory frm = new App.frmCategory();
            frm.ShowDialog();
        }

        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.suppliersMaintenance;
            // App.frmSupplier frm = new App.frmSupplier();
            frm.ShowDialog();
        }

        private void btnClients_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.clientsMaintenance;
            //App.frmClient frm = new App.frmClient();
            frm.ShowDialog();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.usersMaintenance;
            //App.frmUsers frm = new App.frmUsers();
            frm.ShowDialog();
        }

        private void btnPermissions_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.permissionsMaintenance;
            //App.frmPermissions frm = new App.frmPermissions();
            frm.ShowDialog();
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            //App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            // frm.Owner = new frmMain();

            //frm.formToOpen = App.Dialogs.FormType.pointOfSales;

            App.frmPOS frm = new App.frmPOS(null, null, null, null, "", "", "", "");
            FileLogger.WriteLog("btnPOS_Click");
            //frm.ShowDialog();
            frm.Show();
        }

        private void btnInventoryMonitoring_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.inventoryMonitoring;
            //App.frmInventoryMonitoring frm = new App.frmInventoryMonitoring();
            frm.ShowDialog();
        }


        private void btnCalculator_Click(object sender, EventArgs e)
        {
            //This will open the built-in calculator on windows. 
            System.Diagnostics.Process.Start("calc");
        }

        private void btnTime_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("timedate.cpl");
        }

        private void btnNotes_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Windows\Sysnative\StikyNot.exe");
        }

        private void btnInventoryReport_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.inventoryReport;
            //  App.frmre frm = new App.frmInventoryMonitoring();
            //App.ReportDialog.dlgProductReport frm = new App.ReportDialog.dlgProductReport();
            frm.ShowDialog();
        }

        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.salesReport;
            // App.ReportDialog.dlgSalesReport frm = new App.ReportDialog.dlgSalesReport();
            frm.ShowDialog();
        }

        private void btnLogs_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.logsReport;
            //App.ReportDialog.dlgAuditTrailReport frm = new App.ReportDialog.dlgAuditTrailReport();
            frm.ShowDialog();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.clientReport;
            //App.ReportDialog.dlgClientListReport frm = new App.ReportDialog.dlgClientListReport();
            frm.ShowDialog();
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.supplierReport;
            //App.ReportDialog.dlgSupplierReport frm = new App.ReportDialog.dlgSupplierReport();
            frm.ShowDialog();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            //App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            //frm.formToOpen = App.Dialogs.FormType.chequeMonitoring;
            App.frmChequeMonitoring frm = new App.frmChequeMonitoring();
            frm.ShowDialog();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void btnPurchaseOrder_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.purchaseOrder;
            // App.frmPO frm = new App.frmPO();
            frm.ShowDialog();
        }

        private void btnUsersList_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.userReport;
            //App.ReportDialog.dlgUserReport frm = new App.ReportDialog.dlgUserReport();
            frm.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Idle Mode Logic
            if (Properties.Settings.Default.idleMode == true)
            {
                if (Classes.GetLastUserInput.formStatusIdle == false)
                {
                    if (Classes.GetLastUserInput.GetIdleTickCount() >= ChiuMartSAIS2.Properties.Settings.Default.idleInterval)
                    {
                        Classes.GetLastUserInput.formStatusIdle = true;
                        App.Dialogs.dlgIdleStatus idle = new App.Dialogs.dlgIdleStatus();
                        idle.ShowDialog();
                    }
                }
            }

        }

        private void btnIntervalSettings_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgIntervalSettings idleSettings = new App.Dialogs.dlgIntervalSettings();
            if (idleSettings.ShowDialog() == DialogResult.OK)
            {
                // DO NOTHING
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            App.ReportDialog.dlgManualInventory frm = new App.ReportDialog.dlgManualInventory();
            frm.ShowDialog();
        }

        private void btnSalesGeneration_Click(object sender, EventArgs e)
        {
            App.frmSalesGeneration frm = new App.frmSalesGeneration();
            frm.ShowDialog();
        }

        private void panel3_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N)
            {
                App.frmPOS frm = new App.frmPOS(null, null, null, null, "", "", "", "");
                frm.Show();
            }
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F1)
            {
                App.frmPOS frm = new App.frmPOS(null, null, null, null, "", "", "", "");
                frm.Show();
                return true;    // indicate that you handled this keystroke
            }

            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                Backup();
            }
            catch (Exception y)
            {
                FileLogger.WriteLog(y.Message);

            }

        }
        private void btnRestore_Click(object sender, EventArgs e)
        {
            //App.Dialogs.dlgRestoreDB frm = new App.Dialogs.dlgRestoreDB();
            //frm.ShowDialog();
            // string backupdrive = ChiuMartSAIS2.Properties.Settings.Default.backupdrive;
            var directory = new DirectoryInfo("C:\\posbackup");
            var file = (from f in directory.GetFiles()
                        orderby f.LastWriteTime descending
                        select f).First();

            Classes.Configuration conf = new Classes.Configuration();

            string decryptstring = Classes.functions.ReadEncryptedfile(file.FullName);

            using (MySqlConnection conn = new MySqlConnection(conf.connectionstring))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ImportFromString(decryptstring);
                        conn.Close();
                    }
                }
            }
            MessageBox.Show("Database successfully imported");

        }
        private void Backup()
        {
            Classes.Configuration conf = new Classes.Configuration();
            string file = "c:\\posBackup\\backup-" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".sql";
            var dir = Path.GetDirectoryName(file);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string backupstring = string.Empty;

            using (MySqlConnection conn = new MySqlConnection(conf.connectionstring))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        backupstring = mb.ExportToString();
                        conn.Close();
                    }
                }
            }
            Classes.functions.WriteEncryptedFile(file, backupstring);
            MessageBox.Show("Backup successfully generated in c drive");

            //StreamReader objReader = new StreamReader(file);
            //string sLine = "";
            //ArrayList arrText = new ArrayList();
            //while (sLine != null)
            //{
            //    sLine = objReader.ReadLine();
            //    if (sLine != null)
            //        arrText.Add(sLine);
            //}
            //objReader.Close();
            //string str = string.Empty;

            //foreach (string sOutput in arrText)
            //{
            //    str += sOutput;
            //}



        }


        private void btnConnection_Click(object sender, EventArgs e)
        {

        }

        private void button21_Click(object sender, EventArgs e)
        {
            new dbHelper().syncLive();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            new dbHelper().syncLive();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.EndShiftMonitoring;
            frm.ShowDialog();
        }

        private void btnRefund_Click(object sender, EventArgs e)
        {
            App.Dialogs.dlgPasswordAuth frm = new App.Dialogs.dlgPasswordAuth();
            frm.formToOpen = App.Dialogs.FormType.SalesReturn;
            frm.ShowDialog();
        }

        private void btnCategoryInventory_Click(object sender, EventArgs e)
        {
            App.ReportDialog.dlgCategoryInventory frm = new App.ReportDialog.dlgCategoryInventory();
            frm.ShowDialog();
        }

        private void btnEndShiftDay_Click(object sender, EventArgs e)
        {
            App.ReportDialog.dlgEndShiftDayReport frm = new App.ReportDialog.dlgEndShiftDayReport();
            frm.ShowDialog();
        }

      
        private void btnSalesReturnReport_Click(object sender, EventArgs e)
        {
            App.ReportDialog.dlgSalesReturnReport frm = new App.ReportDialog.dlgSalesReturnReport();
            frm.ShowDialog();
        }

        //public void SupplierNotify() 
        //{
        //    using (MySqlConnection Con = new MySqlConnection(conf.connectionstring))
        //    {
        //        try
        //        {
        //            Con.Open();
        //            string sqlQuery = "SELECT SUM(unitPrice) AS openingAmount FROM TRANSACTION WHERE CAST(transdate AS DATE) = DATE_ADD(CURDATE(), INTERVAL -1 DAY)";

        //            MySqlCommand sqlCmd = new MySqlCommand(sqlQuery, Con);

        //            MySqlDataReader reader = sqlCmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                string openingAmount = reader["openingAmount"].ToString();
        //                if (!string.IsNullOrEmpty(openingAmount))
        //                {
        //                    lblOA.Text = openingAmount;
        //                }
        //                else
        //                {
        //                    lblOA.Text = "0";
        //                }
        //            }
        //        }
        //        catch (MySqlException ex)
        //        {
        //            string errorCode = string.Format("Error Code : {0}", ex.Number);
        //            MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            FileLogger.WriteLog(ex.Message);
        //        }
        //    }
        //}

    }
}
