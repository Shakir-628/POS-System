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
    public partial class dlgSupplier : Form
    {
        //variable declarations
        private string _action;
        private int _crit;

        public string supplierName;
        public string supplierAddress;
        public string supplierContact;
        public string supplierContactPerson;
        public DateTime supplierDate;
        public string supplierTotalBalance;
        public string supplierRemainingBalance;

        public dlgSupplier(string action, int crit)
        {
            InitializeComponent();

            _action = action;
            _crit = crit;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dlgSupplier_Load(object sender, EventArgs e)
        {
            txtSupplierAddress.Text = supplierAddress;
            txtSupplierContact.Text = supplierContact;
            txtSupplierContactPerson.Text = supplierContactPerson;
            txtSupplierName.Text = supplierName;
            txtTotal.Text = supplierTotalBalance;
            txtRemBalance.Text = supplierRemainingBalance;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_action == "add")
            {
                if (checkEmpty() == false)
                {
                    MessageBox.Show("Please fill out all the required fields", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    DialogResult = DialogResult.OK;
                }
            }
            else
            {
                if (checkEmpty() == false)
                {
                    MessageBox.Show("Please fill out all the required fields", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }

        /// <summary>
        /// checks if the fields are properly inputted
        /// </summary>
        /// <returns></returns>
        private bool checkEmpty()
        {
            if (txtSupplierName.Text == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// This will give out the supplier value to the parent form
        /// </summary>
        /// <param name="supplier">supplier variable to handle the given/setted supplier</param>
        public void getSupplier(out int supplierId, out string supplierName, out string supplierAddress, out string supplierContact, out string supplierContactPerson, out DateTime supplierDate, out string supplierTotalBalance, out string supplierRemainingBalance)
        {
            // Set the supplier id
            supplierId = _crit;

            // Set the supplier name
            supplierName = txtSupplierName.Text;
            supplierAddress = txtSupplierAddress.Text;
            // Set the supplier contact
            supplierContact = txtSupplierContact.Text;
            supplierContactPerson = txtSupplierContactPerson.Text;
            supplierDate = Convert.ToDateTime(txtDate.Text);
            supplierTotalBalance = txtTotal.Text;
            supplierRemainingBalance = txtRemBalance.Text;

        }

    }
}
