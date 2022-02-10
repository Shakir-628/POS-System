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
    public partial class frmLicense : Form
    {
        public frmLicense()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string password = "abc@123";
            Classes.functions ff = new Classes.functions();
            if (!string.IsNullOrEmpty(txtLicense.Text))
            {
                //if (!ff.checkspecial(txtLicense.Text))
                //{
                //    MessageBox.Show("Enter valid License key");
                //    return;
                //}

                string licenseToDecript = Classes.StringCipher.Decrypt(txtLicense.Text, password);
                Classes.dbHelper db = new Classes.dbHelper();

                string[] splitStr = licenseToDecript.Split(new string[] { "||" }, StringSplitOptions.None);
                string dt = splitStr[0];
                //string mac = splitStr[1];
                //if (Classes.functions.isMac(mac))
                //{
                    //string encmac = Classes.zipper.Encrypt(mac, 19);
                    db.licenseInsert(txtLicense.Text, dt, "1");

                    MessageBox.Show("Successfully Activated");
                    frmLogin frm = new frmLogin();
                    frm.Show();
                    this.Close();
                //}
               // else { MessageBox.Show("Enter valid License key"); }
            }
            else
            {

                MessageBox.Show("Enter valid License key");
            }
        }

        private void frmLicense_Load(object sender, EventArgs e)
        {
            string mc = Classes.functions.GetMacAddress();
            Random rd = new Random();
            int cy = rd.Next(1000, 100000);
            string cyt = Classes.functions.RandomString(6) + cy + "mmm" + mc;
            string enc = Classes.zipper.Encrypt(cyt, 13);
            lblLie.Text = enc;
            txtli.Text = enc;
        }
    }
}
