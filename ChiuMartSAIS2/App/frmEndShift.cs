using ChiuMartSAIS2.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChiuMartSAIS2.App
{
    public partial class frmEndShift : Form
    {
        private Classes.Configuration conf;
        int cash_in_hand = 0;
        string sale_amount = "0";
        string opening_amount = "0";
        string refund_amount = "0";
        int res_5000 = 0;
        int res_1000 = 0;
        int res_500 = 0;
        int res_100 = 0;
        int res_50 = 0;
        int res_20 = 0;
        int res_10 = 0;
        int res_coin = 0;

        public frmEndShift()
        {
            InitializeComponent();
            conf = new Classes.Configuration();

            EndShiftInit();
            //  OpeningBalanceTotal();
            GetSaleAmount();
            GetRefundAmount();

        }

        public void EndShiftInit()
        {
            int val = 0;
            txt5000.Text = val.ToString();
            txt1000.Text = val.ToString();
            txt500.Text = val.ToString();
            txt100.Text = val.ToString();
            txt50.Text = val.ToString();
            txt20.Text = val.ToString();
            txt10.Text = val.ToString();
            txtCoins.Text = val.ToString();
            lbl5000.Text = val.ToString();
            lbl1000.Text = val.ToString();
            lbl500.Text = val.ToString();
            lbl100.Text = val.ToString();
            lbl50.Text = val.ToString();
            lbl20.Text = val.ToString();
            lbl10.Text = val.ToString();
            lblCoins.Text = val.ToString();
            txtTotalEndShift.Text = val.ToString();

        }

        public string EndShifTotalBalance()
        {
            Int32 TotalVal = int.Parse(lbl5000.Text) + int.Parse(lbl1000.Text) + int.Parse(lbl500.Text) + int.Parse(lbl100.Text) + int.Parse(lbl50.Text) + int.Parse(lbl20.Text) + int.Parse(lbl10.Text) + int.Parse(lblCoins.Text);
            return TotalVal.ToString();
        }

        public void OpeningBalanceTotal()
        {
            using (MySqlConnection Con = new MySqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT SUM(unitPrice) AS openingAmount FROM TRANSACTION WHERE CAST(transdate AS DATE) = DATE_ADD(CURDATE(), INTERVAL -1 DAY)";

                    MySqlCommand sqlCmd = new MySqlCommand(sqlQuery, Con);

                    MySqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        opening_amount = reader["openingAmount"].ToString();
                        if (!string.IsNullOrEmpty(opening_amount))
                        {
                            lblOA.Text = opening_amount;
                        }
                        else
                        {
                            lblOA.Text = "0";
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FileLogger.WriteLog(ex.Message);
                }
            }
        }
        public void GetSaleAmount()
        {
            using (MySqlConnection Con = new MySqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT COALESCE(SUM(UnitPrice*qty),0) AS sale_amount FROM TRANSACTION WHERE CAST(transdate AS DATE) = CURDATE()";

                    MySqlCommand sqlCmd = new MySqlCommand(sqlQuery, Con);

                    MySqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        sale_amount = reader["sale_amount"].ToString();
                        cash_in_hand = int.Parse(sale_amount);
                        if (!string.IsNullOrEmpty(sale_amount))
                        {
                            lblNS.Text = sale_amount;
                        }
                        else
                        {
                            lblNS.Text = "0";
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FileLogger.WriteLog(ex.Message);
                }
            }
        }

        public void GetRefundAmount()
        {
            using (MySqlConnection Con = new MySqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT COALESCE(SUM(productPrice*qty),0) AS refund_amount FROM refund WHERE CAST(created_time AS DATE) = CURDATE()";

                    MySqlCommand sqlCmd = new MySqlCommand(sqlQuery, Con);

                    MySqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        refund_amount = reader["refund_amount"].ToString();
                        cash_in_hand = cash_in_hand - int.Parse(refund_amount);
                        lblCH.Text = cash_in_hand.ToString();
                        if (!string.IsNullOrEmpty(refund_amount))
                        {
                            lblRA.Text = refund_amount;
                        }
                        else
                        {
                            lblRA.Text = "0";
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FileLogger.WriteLog(ex.Message);
                }
            }
        }
        private void frmEndShift_Load(object sender, EventArgs e)
        {

        }

        private void txt5000_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt5000.Text))
            {
                res_5000 = int.Parse(txt5000.Text) * 5000;
                lbl5000.Text = res_5000.ToString();
                lblCS.Text = txtTotalEndShift.Text = EndShifTotalBalance();
                cash_in_hand += res_5000;
                lblCH.Text = cash_in_hand.ToString();

            }
            else
            {
                lbl5000.Text = "0";
                cash_in_hand = cash_in_hand - res_5000;
                lblCH.Text = cash_in_hand.ToString();

            }
        }

        private void txt1000_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt1000.Text))
            {
                res_1000 = int.Parse(txt1000.Text) * 1000;
                lbl1000.Text = res_1000.ToString();
                lblCS.Text = txtTotalEndShift.Text = EndShifTotalBalance();
                cash_in_hand += res_1000;
                lblCH.Text = cash_in_hand.ToString();
            }
            else
            {
                lbl1000.Text = "0";

                cash_in_hand = cash_in_hand - res_1000;
                lblCH.Text = cash_in_hand.ToString();
            }
        }
        private void txt500_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt500.Text))
            {
                res_500 = int.Parse(txt500.Text) * 500;
                lbl500.Text = res_500.ToString();
                lblCS.Text = txtTotalEndShift.Text = EndShifTotalBalance();
                cash_in_hand += res_500;
                lblCH.Text = cash_in_hand.ToString();
            }
            else
            {
                lbl500.Text = "0";
                cash_in_hand = cash_in_hand - res_500;
                lblCH.Text = cash_in_hand.ToString();
            }
        }
        private void txt100_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt100.Text))
            {
                int res_100 = int.Parse(txt100.Text) * 100;
                lbl100.Text = res_100.ToString();
                lblCS.Text = txtTotalEndShift.Text = EndShifTotalBalance();
                cash_in_hand += res_100;
                lblCH.Text = cash_in_hand.ToString();
            }
            else
            {
                lbl100.Text = "0";
                cash_in_hand = cash_in_hand - res_100;
                lblCH.Text = cash_in_hand.ToString();
            }
        }
        private void txt50_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt50.Text))
            {
                int res_50 = int.Parse(txt50.Text) * 50;
                lbl50.Text = res_50.ToString();
                lblCS.Text = txtTotalEndShift.Text = EndShifTotalBalance();
                cash_in_hand += res_50;
                lblCH.Text = cash_in_hand.ToString();
            }
            else
            {
                lbl50.Text = "0";
                cash_in_hand = cash_in_hand - res_50;
                lblCH.Text = cash_in_hand.ToString();
            }
        }
        private void txt20_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt20.Text))
            {
                res_20 = int.Parse(txt20.Text) * 20;
                lbl20.Text = res_20.ToString();
                lblCS.Text = txtTotalEndShift.Text = EndShifTotalBalance();
                cash_in_hand += res_20;
                lblCH.Text = cash_in_hand.ToString();
            }
            else
            {
                lbl20.Text = "0";
                cash_in_hand = cash_in_hand - res_20;
                lblCH.Text = cash_in_hand.ToString();
            }
        }
        private void txt10_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt10.Text))
            {
                res_10 = int.Parse(txt10.Text) * 10;
                lbl10.Text = res_10.ToString();
                lblCS.Text = txtTotalEndShift.Text = EndShifTotalBalance();
                cash_in_hand += res_10;
                lblCH.Text = cash_in_hand.ToString();
            }
            else
            {
                lbl10.Text = "0";
                cash_in_hand = cash_in_hand - res_10;
                lblCH.Text = cash_in_hand.ToString();
            }
        }

        private void txtCoins_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCoins.Text))
            {
                res_coin = int.Parse(txtCoins.Text);
                lblCoins.Text = res_coin.ToString();
                lblCS.Text = txtTotalEndShift.Text = EndShifTotalBalance();
                cash_in_hand += res_coin;
                lblCH.Text = cash_in_hand.ToString();
            }
            else
            {
                lblCoins.Text = "0";
                cash_in_hand = cash_in_hand - res_coin;
                lblCH.Text = cash_in_hand.ToString();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            EndShiftInit();
        }


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            using (MySqlConnection Con = new MySqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "INSERT INTO EndShift (TotalAmount, OpeningAmount, NetSale, CashSale, RefundAmount, CashinHand,Status) VALUES " +
                        "(@TotalAmount, @OpeningAmount, @NetSale, @CashSale, @RefundAmount, @CashinHand, 'active')";
                    MySqlCommand sqlCmd = new MySqlCommand(sqlQuery, Con);

                    sqlCmd.Parameters.AddWithValue("TotalAmount", txtTotalEndShift.Text);
                    sqlCmd.Parameters.AddWithValue("OpeningAmount", lblOA.Text);
                    sqlCmd.Parameters.AddWithValue("NetSale", lblNS.Text);
                    sqlCmd.Parameters.AddWithValue("CashSale", lblCS.Text);
                    sqlCmd.Parameters.AddWithValue("RefundAmount", lblRA.Text);
                    sqlCmd.Parameters.AddWithValue("CashinHand", lblCH.Text);

                    sqlCmd.ExecuteNonQuery();
                    new dbHelper().backupinset(sqlCmd, "INSERT", "EndShift");
                    MessageBox.Show(this, "End Shift successfully added", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (MySqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "error", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lblOA_TextChanged(object sender, EventArgs e)
        {
            //if (e.KeyCode == System.Windows.Forms.Keys.Back)
            //{
            //}
            cash_in_hand = cash_in_hand - int.Parse(opening_amount);

            if (!string.IsNullOrEmpty(lblOA.Text))
            {
                opening_amount = lblOA.Text;
                cash_in_hand += int.Parse(opening_amount);
                lblCH.Text = cash_in_hand.ToString();
            }
            else
            {

            }

        }
    }
}
