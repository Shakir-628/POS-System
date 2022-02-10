using System;
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
    public partial class dlgClient : Form
    {
        //variable declarations
        private string _action;
        private int _crit;

        public string clientName;
        public string clientAddress;
        public string clientContact;
        public string CNIC;
        public string guranteeDetials;
        public string otherDetails;
        public string categoryType;

        public dlgClient(string action, int crit)
        {
            InitializeComponent();

            _action = action;
            _crit = crit;
        }

        /// <summary>
        /// This will give out the client value to the parent form
        /// </summary>
        /// <param name="client">client variable to handle the given/setted client</param>
        public void getClient(out int clientId, out string clientName, out string clientAddress, out string clientContact, out string CNIC, out string guranteeDetails, out string otherDetails, out string categoryType)
        {
            // Set the client id
            clientId = _crit;

            // Set the client name
            clientName = txtClientName.Text;
            // Set the client address
            clientAddress = txtClientAddress.Text;
            // set the client contact
            clientContact = txtContact.Text;

            CNIC = txtCNIC.Text;

            guranteeDetails = txtGuranteeDetails.Text;

            otherDetails = txtOtherDetails.Text;
            categoryType = cbCategoryType.SelectedText;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dlgClient_Load(object sender, EventArgs e)
        {
            txtClientName.Text = clientName;
            txtClientAddress.Text = clientAddress;
            txtContact.Text = clientContact;
            txtCNIC.Text = CNIC;
            txtGuranteeDetails.Text = guranteeDetials;
            txtOtherDetails.Text = otherDetails;
            cbCategoryType.SelectedItem = categoryType;



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
            if (txtClientName.Text == "")
            {
                return false;
            }
            else if (txtContact.Text == "")
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
