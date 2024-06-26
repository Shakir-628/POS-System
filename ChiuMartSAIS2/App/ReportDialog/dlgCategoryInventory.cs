﻿
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
    public partial class dlgCategoryInventory : Form
    {
        public dlgCategoryInventory()
        {
            InitializeComponent();
            conf = new Classes.Configuration();
        }

        private string status = "active";

        private Classes.Configuration conf;
     

        private void populateProduct()
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT c.categoryid,c.categoryname,SUM(p.productSafetyStock) as productSafetyStock FROM Category c INNER JOIN products p ON p.categoryid = c.categoryid WHERE c.status = @status GROUP BY  c.categoryname,c.categoryid";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("status", this.status);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    dgvProduct.Rows.Clear();

                    while (reader.Read())
                    {
                        string[] dataRow = new string[] { reader["categoryid"].ToString(), reader["categoryname"].ToString(), reader["productSafetyStock"].ToString() };
                        dgvProduct.Rows.Add(dataRow);
                    }
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string checkStatus(int physical, int actual)
        {
            string result = "";
            if (physical > actual)
            {
                result = "OVER - " + (physical - actual);
                return result;
            }
            else if (actual > physical)
            {
                result = "SHORT - " + (actual - physical);
                return result;
            }
            else
            {
                result = "TALLY";
                return result;
            }
        }

        private string populateStocks(string id)
        {
            string result = "";
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT productStock FROM products WHERE status = @status AND productId = @id ORDER BY productName ASC";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("status", this.status);
                    sqlCmd.Parameters.AddWithValue("id", id);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result = reader["productStock"].ToString();
                    }
                    return result;
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return result;
                }
            }
        }

        private void dlgCategoryInventory_Load(object sender, EventArgs e)
        {
            populateProduct();
        }

        private void dgvProduct_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvProduct.CurrentCell.ColumnIndex == 2)
            //{
            string id = dgvProduct.Rows[dgvProduct.CurrentRow.Index].Cells[0].Value.ToString();
            int physical = Int32.Parse(dgvProduct.Rows[dgvProduct.CurrentRow.Index].Cells[2].Value.ToString());
            dgvProduct.Rows[dgvProduct.CurrentRow.Index].Cells[3].Value = populateStocks(id);
            int actual = Int32.Parse(dgvProduct.Rows[dgvProduct.CurrentRow.Index].Cells[3].Value.ToString());

            dgvProduct.Rows[dgvProduct.CurrentRow.Index].Cells[4].Value = checkStatus(physical, actual);
            //}
        }

        private void dgvProduct_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvProduct.CurrentCell.ColumnIndex != 2)
            {
                e.Cancel = true;
            }
        }


        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
