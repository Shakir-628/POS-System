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
    public partial class dlgClientListReport : Form
    {
        private Classes.Configuration conf;
        private string status = "active";

        public dlgClientListReport()
        {
            InitializeComponent();

            conf = new Classes.Configuration();
        }

        private void populateClient()
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    //string sqlQuery = "SELECT * FROM client WHERE status = @status ORDER BY clientName ASC";
                    string sqlQuery = @"SELECT c.clientid,c.`clientName`,c.`clientContact`,c.`clientAddress`,c.`created_date`,c.`updated_date`,c.status,
SUM(t.`unitPrice` - t.paidBalance ) AS amount FROM CLIENT AS c 
 LEFT  JOIN
TRANSACTION AS t ON c.`clientId` = t.`clientId`
 WHERE 
c.status = @status and
(t.`paidBalance` <> 0 OR t.`paidBalance` IS NULL )
GROUP BY c.clientid,c.`clientName`,c.`clientContact`,c.`clientAddress`,c.`created_date`,c.`updated_date`,c.status
 ORDER BY c.clientName ASC ";


                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("status", this.status);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    listView1.Items.Clear();
                    string amount = "0";
                    while (reader.Read())
                    {
                        amount = "0";
                        listView1.Items.Add(reader["clientId"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["clientName"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["clientContact"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["clientAddress"].ToString());
                        // converts the transdate to datetime
                        DateTime aDate;
                        DateTime.TryParse(reader["created_date"].ToString(), out aDate);
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(aDate.ToString("MMMM dd, yyyy"));

                        // converts the transdate to datetime
                        DateTime uDate;
                        DateTime.TryParse(reader["updated_date"].ToString(), out uDate);
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(uDate.ToString("MMMM dd, yyyy"));
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["status"].ToString());
                        if (reader["amount"] != null)
                        {
                            amount = reader["amount"].ToString();
                        }
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(amount);
                    }

                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void searchClient(string critera)
        {
            Classes.functions objfunc = new Classes.functions();
            if (!objfunc.checkspecial(critera))
            {
                return;
            }
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    //string sqlQuery = "SELECT * FROM client WHERE (clientName LIKE @crit or clientContact = @crit ) AND status = @status ORDER BY clientName ASC";
                    string sqlQuery = @"SELECT c.clientid,c.`clientName`,c.`clientContact`,c.`clientAddress`,c.`created_date`,c.`updated_date`,c.status,
SUM(t.`unitPrice` - t.paidBalance ) AS amount FROM CLIENT AS c 
 LEFT  JOIN
TRANSACTION AS t ON c.`clientId` = t.`clientId`
 WHERE 
(c.clientName LIKE '%" + critera + @"%'  or c.clientContact = @crit ) AND
c.status = @status and
(t.`paidBalance` <> 0 OR t.`paidBalance` IS NULL )
GROUP BY c.clientid,c.`clientName`,c.`clientContact`,c.`clientAddress`,c.`created_date`,c.`updated_date`,c.status
 ORDER BY c.clientName ASC ";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    // SQL Query Parameters
                    sqlCmd.Parameters.AddWithValue("crit", critera);
                    sqlCmd.Parameters.AddWithValue("status", this.status);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    listView1.Items.Clear();
                    string amount = "0";
                    while (reader.Read())
                    {
                         amount = "0";
                        listView1.Items.Add(reader["clientId"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["clientName"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["clientContact"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["clientAddress"].ToString());
                        // converts the transdate to datetime
                        DateTime aDate;
                        DateTime.TryParse(reader["created_date"].ToString(), out aDate);
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(aDate.ToString("MMMM dd, yyyy"));

                        // converts the transdate to datetime
                        DateTime uDate;
                        DateTime.TryParse(reader["updated_date"].ToString(), out uDate);
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(uDate.ToString("MMMM dd, yyyy"));
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["status"].ToString());
                        if (reader["amount"] != null)
                        {
                            amount = reader["amount"].ToString();
                        }
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(amount);
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

        private void dlgClientListReport_Load(object sender, EventArgs e)
        {
            populateClient();
        }

        private void rboActive_CheckedChanged(object sender, EventArgs e)
        {
            status = "active";
            populateClient();
        }

        private void rboInactive_CheckedChanged(object sender, EventArgs e)
        {
            status = "inactive";
            populateClient();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchClient(txtSearch.Text);
        }

        private void btnOverview_Click(object sender, EventArgs e)
        {
            dlgIndividualLog log = new dlgIndividualLog();
            log.logType = "client";
            log.relationId = listView1.SelectedItems[0].Text;
            log.ShowDialog();
        }
    }
}
