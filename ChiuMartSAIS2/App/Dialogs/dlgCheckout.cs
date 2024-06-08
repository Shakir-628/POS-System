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
    public partial class dlgCheckout : Form
    {
        public string total = "";
        private string method = "";
        private string bank = "";
        private string branch = "";
        private string cheque = "";
        private string chequeName = "";
        private string chequeDate = "";
        private string balance = "";
        public string change = "";
        public string paid = "";
        private string _action;
        public bool isToken=false;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F2)
            {
                button1.PerformClick(); // checkout button
                return true;    // indicate that you handled this keystroke
            }
            if (keyData == Keys.Escape)
            {
                button2.PerformClick(); // checkout button
                return true;    // indicate that you handled this keystroke
            }

            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        public dlgCheckout(string action)
        {
            InitializeComponent();

            _action = action;
        }

        public void getProduct(out string method, out string bank, out string branch, out string chequeName, out string chequeDate, out string total, out string chequeNo)
        {
            // Set the payment method
            method = this.method;
            // Set the bank
            bank = this.bank;
            // Set the branch
            branch = this.branch;
            // Set the name
            chequeName = this.chequeName;
            // Set the number
            chequeNo = this.cheque;
            // Set the date
            chequeDate = this.chequeDate;
            // Set the total
            total = this.total;
        }

        public void getTotalPaid(out string balance)
        {
            // Set the payment method
            balance = this.balance;
        }

        private void dlgCheckout_Load(object sender, EventArgs e)
        {
            if (_action == "POS")
            {
                txtAmount.Text = total;
                txtReceivableTotal.Text = total;
                txtTotal.Text = total;
                txtCashRendered.Text = total;
            }
            else
            {
                if (total == "Completed")
                {
                    txtAmount.Text = "0.0";
                    txtTotal.Text = "0.0";
                }
                else
                {
                    txtAmount.Text = total;
                    txtTotal.Text = total;
                }
                tabControl1.TabPages.Remove(tabPage3);
            }

            button1.Focus();
        }

        private bool checkEmpty()
        {
            if (txtBank.Text == "")
            {
                return false;
            }
            else
            {
                if (txtBranch.Text == "")
                {
                    return false;
                }
                else
                {
                    if (txtName.Text == "")
                    {
                        return false;
                    }
                    else
                    {
                        if (txtChequeNumber.Text == "")
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
        }

        private void txtCashRendered_Click(object sender, EventArgs e)
        {
            txtCashRendered.SelectAll();
        }

        private void txtCashRendered_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Classes.functions ff = new Classes.functions();
            method = "Cash";
            balance = txtCashRendered.Text;
            this.paid = ff.RemoveCurrency(txtCashRendered.Text);
            this.change = ff.RemoveCurrency(txtChange.Text);
            DialogResult = DialogResult.OK;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (checkEmpty() == false)
            {
                MessageBox.Show("Please fill out all the required fields", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                method = "Cheque";
                bank = txtBank.Text;
                branch = txtBranch.Text;
                cheque = txtChequeNumber.Text;
                chequeName = txtName.Text;
                chequeDate = dtpDate.Text;
                DialogResult = DialogResult.OK;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            method = "Balance";
            DialogResult = DialogResult.OK;
        }

        private void txtCashRendered_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                Classes.functions ff = new Classes.functions();

                string cashrendered = ff.RemoveCurrencyCheckOut(txtCashRendered.Text);
                string amount = ff.RemoveCurrencyCheckOut(txtAmount.Text);
                txtChange.Text = ((double.Parse(cashrendered) - double.Parse(amount))).ToString();
                //this.change = txtChange.Text;
                //this.paid = cashrendered;
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message.ToString());
                txtChange.Text = "0.0";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Classes.functions ff = new Classes.functions();
            method = "Cash";
            balance = txtCashRendered.Text;
            this.paid = ff.RemoveCurrency(txtCashRendered.Text);
            this.change = ff.RemoveCurrency(txtChange.Text);
            isToken = true;
            DialogResult = DialogResult.OK;
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
