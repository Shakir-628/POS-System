
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
    public partial class dlgSalesReturnReport : Form
    {
        public dlgSalesReturnReport()
        {
            InitializeComponent();
            conf = new Classes.Configuration();
            populateEndShiftByType();
        }


        private Classes.Configuration conf;
        public double Total = 0;


        private void populateEndShiftByType()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conf.connectionstring))
                {
                    con.Open();
                    string sql = "";


                    sql = "SELECT r.productid, p.productName,r.productPrice,r.qty,r.productPrice*r.qty AS total, r.created_time FROM refund r inner JOIN products p ON p.productid = r.productid WHERE r.status = 'Completed' ORDER BY r.created_time ASC";

                    SqlCommand sqlCmd = new SqlCommand(sql, con);

                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    Total = 0;
                    listView1.Items.Clear();
                    while (reader.Read())
                    {
                        listView1.Items.Add(reader["productid"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["productName"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["productPrice"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["qty"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["total"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["created_time"].ToString());
                        Total += double.Parse(reader["total"].ToString());
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorCode = string.Format("Error Code : {0}", ex.ToString());
                MessageBox.Show(this, "Can't connect to database" + errorCode, errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void filterDate()
        {
            using (SqlConnection con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    con.Open();
                    string sql = "";


                    sql = "SELECT r.productid, p.productName,r.productPrice,r.qty,r.productPrice*r.qty AS total, r.created_time FROM refund r inner JOIN products p ON p.productid = r.productid WHERE r.status = 'Completed' and DATE_FORMAT(created_time,'%Y-%m-%d') BETWEEN @from AND @to ORDER BY created_time ASC";

                    SqlCommand sqlCmd = new SqlCommand(sql, con);
                    sqlCmd.Parameters.AddWithValue("from", dtpStart.Value.ToString("yyyy-MM-dd"));
                    sqlCmd.Parameters.AddWithValue("to", dtpEnd.Value.ToString("yyyy-MM-dd"));

                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    Total = 0;
                    listView1.Items.Clear();
                    while (reader.Read())
                    {
                        listView1.Items.Add(reader["productid"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["productName"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["productPrice"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["qty"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["total"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["created_time"].ToString());
                        Total += double.Parse(reader["total"].ToString());
                    }
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.ToString());
                    MessageBox.Show(this, "Can't connect to database" + errorCode, errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InitializeListView()
        {
            // Set up the inital values for the ListView and populate it. 
            this.listView1 = new ListView();
            this.listView1.Dock = DockStyle.Top;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Size = new System.Drawing.Size(292, 130);
            this.listView1.View = View.Details;
            this.listView1.FullRowSelect = true;

            PopulateListview();
        }

        private void PopulateListview()
        {
            ColumnHeader columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "Product Id";
            columnHeader1.TextAlign = HorizontalAlignment.Left;
            columnHeader1.Width = 0;

            ColumnHeader columnHeader2 = new ColumnHeader();
            columnHeader2.Text = "Product Name";
            columnHeader2.TextAlign = HorizontalAlignment.Center;
            columnHeader2.Width = 190;

            ColumnHeader columnHeader3 = new ColumnHeader();
            columnHeader3.Text = "Product Price";
            columnHeader3.TextAlign = HorizontalAlignment.Center;
            columnHeader3.Width = 80;

            ColumnHeader columnHeader4 = new ColumnHeader();
            columnHeader4.Text = "QTY";
            columnHeader4.TextAlign = HorizontalAlignment.Center;
            columnHeader4.Width = 150;

            ColumnHeader columnHeader5 = new ColumnHeader();
            columnHeader5.Text = "Total";
            columnHeader5.TextAlign = HorizontalAlignment.Center;
            columnHeader5.Width = 150;

            ColumnHeader columnHeader6 = new ColumnHeader();
            columnHeader6.Text = "Date";
            columnHeader6.TextAlign = HorizontalAlignment.Center;
            columnHeader6.Width = 185;

            this.listView1.Columns.Add(columnHeader1);
            this.listView1.Columns.Add(columnHeader2);
            this.listView1.Columns.Add(columnHeader3);
            this.listView1.Columns.Add(columnHeader4);
            this.listView1.Columns.Add(columnHeader5);
            this.listView1.Columns.Add(columnHeader6);
        }

        private void dlgEndShiftDayReport_Load(object sender, EventArgs e)
        {
            populateEndShiftByType();
            lblQuantity.Text = Total.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            filterDate();
            lblQuantity.Text = Total.ToString();
        }
    }
}
