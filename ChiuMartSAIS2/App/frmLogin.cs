using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChiuMartSAIS2.App
{
    public partial class frmLogin : Form
    {
        private Classes.StringHash stringHash;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            Classes.dbHelper objdb = new Classes.dbHelper();
            if (!objdb.checkLiecense())
            {
                frmLicense frm = new frmLicense();
                frm.ShowDialog();
                this.Close();
            }
        }
        private bool checkspecial(string name)
        {
            var regexItem = new Regex("^[a-zA-Z0-9_@ ]*$");

            if (regexItem.IsMatch(name)) { return true; }
            else { return false; }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                MessageBox.Show("User name cannot be empty");
                return;
            }
            if (txtpWord.Text == "")
            {
                MessageBox.Show("Passowrd cannot be empty");
                return;
            }
            if (txtUsername.Text.Length < 4)
            {
                MessageBox.Show("Incorrect Password");
                return;
            }
             if (!checkspecial(txtpWord.Text))
            {
                MessageBox.Show("Incorrect Password");
                return;
            }
            if (!checkspecial(txtUsername.Text))
            {
                MessageBox.Show("Incorrect User");
                return;
            }
            stringHash = new Classes.StringHash();
            if (Classes.Authentication.Instance.userLogin(txtUsername.Text, stringHash.hashIt(txtpWord.Text)))
            {
                frmMain frm = new frmMain();
                frm.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Login Failed");
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
