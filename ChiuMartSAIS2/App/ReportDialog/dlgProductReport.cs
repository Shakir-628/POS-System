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
    public partial class dlgProductReport : Form
    {
        private string status = "active";

        private Classes.Configuration conf;

        public dlgProductReport()
        {
            InitializeComponent();

            conf = new Classes.Configuration();
        }

        private void populateProduct()
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT p.*, u.*, c.* FROM products as p INNER JOIN units as u ON p.unitId = u.unitId INNER JOIN category as c ON p.categoryId = c.categoryId WHERE p.status = @status ORDER BY p.productName ASC";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("status", this.status);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    listView1.Items.Clear();

                    while (reader.Read())
                    {
                        listView1.Items.Add(reader["productId"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["productName"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["unitDesc"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["productPrice"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["productStock"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["productSafetyStock"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["categoryName"].ToString());

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

        /// <summary>
        /// This function will search a specific product using a filter and a criteria
        /// </summary>
        private void searchProduct(string filter, string critera)
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "";
                    if (filter == "productName")
                    {
                        sqlQuery = "SELECT p.*, u.*, c.* FROM products as p INNER JOIN units as u ON p.unitId = u.unitId INNER JOIN category as c ON p.categoryId = c.categoryId WHERE p.productName LIKE @crit AND p.status = @status  ORDER BY p.productName ASC";
                    }
                    else if (filter == "categoryName")
                    {
                        sqlQuery = "SELECT p.*, u.*, c.* FROM products as p INNER JOIN units as u ON p.unitId = u.unitId INNER JOIN category as c ON p.categoryId = c.categoryId WHERE c.categoryName LIKE @crit AND p.status = @status  ORDER BY p.productName ASC";
                    }
                    else if (filter == "productId")
                    {
                        sqlQuery = "SELECT p.*, u.*, c.* FROM products as p INNER JOIN units as u ON p.unitId = u.unitId INNER JOIN category as c ON p.categoryId = c.categoryId WHERE p.productId LIKE @crit AND p.status = @status  ORDER BY p.productName ASC";
                    }

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    // SQL Query Parameters
                    sqlCmd.Parameters.AddWithValue("crit", "%" + critera + "%");
                    sqlCmd.Parameters.AddWithValue("status", this.status);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    listView1.Items.Clear();

                    while (reader.Read())
                    {
                        listView1.Items.Add(reader["productId"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["productName"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["unitDesc"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["productPrice"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["productStock"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["productSafetyStock"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["categoryName"].ToString());

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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string filter = "";

            if (rboProductName.Checked)
            {
                filter = "productName";
            }
            else if (rboCategory.Checked)
            {
                filter = "categoryName";
            }
            else if (rboProductId.Checked)
            {
                filter = "productId";
            }

            searchProduct(filter, txtSearch.Text);
        }

        private void rboActive_CheckedChanged(object sender, EventArgs e)
        {
            status = "active";
            populateProduct();
        }

        private void rboInactive_CheckedChanged(object sender, EventArgs e)
        {
            status = "inactive";
            populateProduct();
        }

        private void dlgProductReport_Load(object sender, EventArgs e)
        {
            populateProduct();
        }
    }
}
