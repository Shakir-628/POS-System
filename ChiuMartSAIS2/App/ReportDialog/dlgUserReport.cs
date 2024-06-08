using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace ChiuMartSAIS2.App.ReportDialog
{
    public partial class dlgUserReport : Form
    {
        private string status = "active";

        private Classes.Configuration conf;

        public dlgUserReport()
        {
            InitializeComponent();

            conf = new Classes.Configuration();
        }

        private void populateUsers()
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT u.*, p.role FROM [user] as u INNER JOIN permission as p ON u.permissionId = p.permissionId WHERE u.status = @status ORDER BY u.username ASC";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("status", this.status);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    listView1.Items.Clear();

                    while (reader.Read())
                    {
                        listView1.Items.Add(reader["userId"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["username"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["password"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["fullname"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["role"].ToString());

                        // converts the transdate to datetime
                        DateTime aDate;
                        DateTime.TryParse(reader["created_date"].ToString(), out aDate);
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(aDate.ToString("MMMM dd, yyyy"));

                        // converts the transdate to datetime
                        DateTime uDate;
                        DateTime.TryParse(reader["updated_date"].ToString(), out uDate);
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(uDate.ToString("MMMM dd, yyyy"));
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["status"].ToString());
                    }

                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void searchUser(string critera)
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "";

                    sqlQuery = "SELECT u.*, p.role FROM [user] as u INNER JOIN permission as p ON u.permissionId = p.permissionId WHERE u.username LIKE @crit AND u.status = @status ORDER BY u.username ASC";


                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    // SQL Query Parameters
                    sqlCmd.Parameters.AddWithValue("crit", "%" + critera + "%");
                    sqlCmd.Parameters.AddWithValue("status", this.status);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    listView1.Items.Clear();

                    while (reader.Read())
                    {
                        listView1.Items.Add(reader["userId"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["username"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["password"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["fullname"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["role"].ToString());

                        // converts the transdate to datetime
                        DateTime aDate;
                        DateTime.TryParse(reader["created_date"].ToString(), out aDate);
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(aDate.ToString("MMMM dd, yyyy"));

                        // converts the transdate to datetime
                        DateTime uDate;
                        DateTime.TryParse(reader["updated_date"].ToString(), out uDate);
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(uDate.ToString("MMMM dd, yyyy"));
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["status"].ToString());
                    }

                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rboActive_CheckedChanged(object sender, EventArgs e)
        {
            status = "active";
            populateUsers();
        }

        private void rboInactive_CheckedChanged(object sender, EventArgs e)
        {
            status = "inactive";
            populateUsers();
        }

        private void dlgUserReport_Load(object sender, EventArgs e)
        {
            populateUsers();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchUser(txtSearch.Text);
        }
    }
}
