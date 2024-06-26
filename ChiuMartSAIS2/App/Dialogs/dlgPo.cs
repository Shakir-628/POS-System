﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Text.RegularExpressions;
using ChiuMartSAIS2.Classes;
using System.Data.SqlClient;

namespace ChiuMartSAIS2.App.Dialogs
{
    public partial class dlgPo : Form
    {
        private Classes.Configuration conf;
        private AutoCompleteStringCollection prodSource = new AutoCompleteStringCollection();
        private AutoCompleteStringCollection unitSource = new AutoCompleteStringCollection();
        private List<String> qty = new List<string>();
        private List<String> productName = new List<string>();
        private List<String> units = new List<string>();
        private List<String> productPrice = new List<string>();
        private string poId;
        private string supplierName;
        private string supplierAddress;
        private string action;
        public dlgPo(List<String> _qty, List<String> _productName, List<String> _units, List<String> _productPrice, string _poId, string _supplierName, string _supplierAddress, string _action)
        {
            InitializeComponent();

            this.qty = _qty;
            this.productName = _productName;
            this.units = _units;
            this.productPrice = _productPrice;
            this.poId = _poId;
            this.supplierName = _supplierName;
            this.supplierAddress = _supplierAddress;
            this.action = _action;

            conf = new Classes.Configuration();
        }

        /// <summary>
        /// This function will get data from database and populate the textbox for supplier
        /// </summary>
        private void populateSupplierTextbox()
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT * FROM supplier WHERE status = 'active'";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string supplier = reader["supplierName"] + " - " + reader["supplierId"];
                        txtSupplier.AutoCompleteCustomSource.AddRange(new String[] { supplier });
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
        /// Get the product by product name
        /// </summary>
        /// <param name="prodname">Name of the product</param>
        /// <returns>Product Array</returns>
        private String[] getProductByName(string prodname)
        {
            string[] result = new string[6];
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT p.*, u.*, c.* FROM products as p INNER JOIN units as u ON p.unitId = u.unitId INNER JOIN category as c ON p.categoryId = c.categoryId WHERE p.status = 'active' AND p.productName = @prodname";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("prodname", prodname);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result[0] = reader["productId"].ToString();
                        result[1] = "1";
                        result[2] = reader["productName"].ToString();
                        result[3] = reader["unitDesc"].ToString();
                        result[4] = reader["supplierPrice"].ToString();

                        double total = double.Parse(result[1]) * double.Parse(result[4]);
                        result[5] = string.Format("{0:C}", total);
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

        /// <summary>
        /// This function will get data from database and populate the product textbox
        /// </summary>
        private void populateProductTextbox()
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT p.*, u.*, c.* FROM products as p INNER JOIN units as u ON p.unitId = u.unitId INNER JOIN category as c ON p.categoryId = c.categoryId WHERE p.status = 'active'";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string product = reader["productName"].ToString();
                        prodSource.AddRange(new String[] { product });
                    }

                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void updateTotalPrice()
        {
            try
            {
                double total = 0;
                for (int i = 0; i < (dgvCart.Rows.Count - 1); i++)
                {
                    total += double.Parse(dgvCart.Rows[i].Cells[5].Value.ToString(), System.Globalization.NumberStyles.Currency);
                }

                lblTotal.Text = string.Format("{0:C}", total);

            }
            catch (Exception)
            {

            }
        }

        private void updateTotalAmount()
        {
            try
            {
                double total = 0;
                for (int i = 0; i < (dgvCart.Rows.Count); i++)
                {
                    total += double.Parse(dgvCart.Rows[i].Cells[5].Value.ToString(), System.Globalization.NumberStyles.Currency);
                }

                lblTotal.Text = string.Format("{0:C}", total);

            }
            catch (Exception)
            {

            }
        }

        private bool checkEmpty()
        {
            if (txtSupplier.Text == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //get unit ID
        private string getUnitID(string crit)
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT unitId FROM units WHERE unitDesc = @crit";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    sqlCmd.Parameters.AddWithValue("crit", crit);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    string tmp = "";
                    while (reader.Read())
                    {
                        tmp = reader["unitId"].ToString();
                    }

                    return tmp;
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Message);
                    MessageBox.Show(this, "Error Retrieving unit id", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "";
                }

            }
        }

        //get product ID
        private string getProductID(string crit)
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT productId FROM products WHERE productName = @crit";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    sqlCmd.Parameters.AddWithValue("crit", crit);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    string tmp = "";
                    while (reader.Read())
                    {
                        tmp = reader["productId"].ToString();
                    }

                    return tmp;
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Message);
                    MessageBox.Show(this, "Error Retrieving product id", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "";
                }

            }
        }

        private String getSupplierAddress(string id)
        {
            string result = "";
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT supplierAddress FROM supplier WHERE supplierId = @supplierId";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("supplierId", id);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result = reader["supplierAddress"].ToString();
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

        /// <summary>
        /// GENERATE NEW PO NUMBER
        /// </summary>
        private string generatePO()
        {
            string lastPoNumber = "";

            string generatedPO = "";
            string currentdate = DateTime.Now.ToString("yyMMddHHmmss");
            // Get the last Po number
            try
            {
                using (SqlConnection con = new SqlConnection(conf.connectionstring))
                {
                    con.Open();
                    string sqlQuery = "SELECT top 1 * FROM po ORDER BY id DESC";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, con);
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        lastPoNumber = reader["poId"].ToString();
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorCode = string.Format("Error Code : {0}", ex.Number);
                MessageBox.Show(this, "Can't connect to database: " + ex.Message.ToString(), errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Check if there is no last OR number
            if (lastPoNumber == "")
            {
                // Create the first one
                generatedPO = currentdate + "1";
            }
            else
            {
                try
                {
                    string lastOr = lastPoNumber.Substring(12);
                    // convert the or number to int and increment it by 1
                    int orNum = Int32.Parse(lastOr) + 1;

                    generatedPO = currentdate + orNum.ToString();

                    if (double.Parse(generatedPO) >= 20000000000000)
                    { generatedPO = currentdate + "1"; }
                }
                catch (Exception)
                {
                    int orNum = 1;
                    generatedPO = currentdate + orNum.ToString();
                }
            }

            return generatedPO;
        }

        private string generatePOOld()
        {
            string lastPoNumber = "";

            string generatedPO = "";

            // Get the last Po number
            try
            {
                using (SqlConnection con = new SqlConnection(conf.connectionstring))
                {
                    con.Open();
                    string sqlQuery = "SELECT top 1 * FROM po ORDER BY id DESC";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, con);
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        lastPoNumber = reader["poId"].ToString();
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorCode = string.Format("Error Code : {0}", ex.Number);
                MessageBox.Show(this, "Can't connect to database: " + ex.Message.ToString(), errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Check if there is no last OR number
            if (lastPoNumber == "")
            {
                // Create the first one
                generatedPO = "0001";
            }
            else
            {
                // convert the or number to int and increment it by 1
                int orNum = Int32.Parse(lastPoNumber) + 1;

                generatedPO = orNum.ToString("0000");
            }

            return generatedPO;
        }

        private void insertPo(string poId, string productId, string supplierId, string qty, string unitId, string oldPrice)
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "INSERT INTO po (poId, productId, supplierId, status, qty, unitId, poStatus, oldPrice) VALUES (@poId, @productId, @supplierId, 'active', @qty, @unitId, 'Delivered', @oldPrice)";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    sqlCmd.Parameters.AddWithValue("poId", poId);
                    sqlCmd.Parameters.AddWithValue("productId", productId);
                    sqlCmd.Parameters.AddWithValue("supplierId", supplierId);
                    sqlCmd.Parameters.AddWithValue("qty", qty);
                    sqlCmd.Parameters.AddWithValue("unitId", unitId);
                    sqlCmd.Parameters.AddWithValue("oldPrice", oldPrice);

                    sqlCmd.ExecuteNonQuery();
                    new dbHelper().backupinset(sqlCmd, "INSERT", "po");

                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Adding new purchase order error", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Check the product queue table if the item does exist
        /// </summary>
        /// <param name="productId">Product Id of the product to search</param>
        /// <param name="price">The product price from supplier</param>
        /// <returns>Boolean</returns>
        private Boolean checkProductQueue(string productId, string price)
        {
            bool result = false;

            try
            {
                using (SqlConnection con = new SqlConnection(conf.connectionstring))
                {
                    con.Open();
                    string sqlQuery = "SELECT * FROM po_queue WHERE product_id = @productId AND supplier_price = @price";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, con);
                    sqlCmd.Parameters.AddWithValue("productId", productId);
                    sqlCmd.Parameters.AddWithValue("price", price);

                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorCode = string.Format("Error Code : {0}", ex.Number);
                MessageBox.Show(this, "Adding new purchase order error", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }

            return result;
        }

        /// <summary>
        /// This fuctio will update the product queue
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="price"></param>
        /// <param name="stock"></param>
        private void updateProductQueue(string productId, string price, string stock)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conf.connectionstring))
                {
                    con.Open();
                    string sqlQuery = "UPDATE po_queue SET stock = stock + @stock WHERE product_id = @productId AND supplier_price = @price";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, con);
                    sqlCmd.Parameters.AddWithValue("stock", stock);
                    sqlCmd.Parameters.AddWithValue("productId", productId);
                    sqlCmd.Parameters.AddWithValue("price", price);

                    sqlCmd.ExecuteNonQuery();
                    new dbHelper().backupinset(sqlCmd, "UPDATE", "po_queue");
                }
            }
            catch (SqlException ex)
            {
                string errorCode = string.Format("Error Code : {0}", ex.Number);
                MessageBox.Show(this, "Adding new purchase order error", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Insert a new product on the queue
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="price"></param>
        /// <param name="stock"></param>
        private void insertProductQueue(string productId, string price, string stock)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conf.connectionstring))
                {
                    con.Open();
                    string sqlQuery = "INSERT INTO po_queue(product_id, supplier_price, stock) VALUES(@productId, @price, @stock)";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, con);
                    sqlCmd.Parameters.AddWithValue("productId", productId);
                    sqlCmd.Parameters.AddWithValue("price", price);
                    sqlCmd.Parameters.AddWithValue("stock", stock);

                    sqlCmd.ExecuteNonQuery();
                    new dbHelper().backupinset(sqlCmd, "INSERT", "po_queue");
                }
            }
            catch (SqlException ex)
            {
                string errorCode = string.Format("Error Code : {0}", ex.Number);
                MessageBox.Show(this, "Adding new purchase order error", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void updateStocks(string qty, string crit, string newPrice)
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "UPDATE products SET productStock = productStock + @qty, supplierPrice = @newPrice WHERE productId = @crit";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    sqlCmd.Parameters.AddWithValue("qty", qty);
                    sqlCmd.Parameters.AddWithValue("newPrice", newPrice);
                    sqlCmd.Parameters.AddWithValue("crit", crit);

                    sqlCmd.ExecuteNonQuery();
                    new dbHelper().backupinset(sqlCmd, "UPDATE", "products");
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Updating stocks error", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// This will update the changes on the cart
        /// </summary>
        private void cartUpdateTotal()
        {
            //try
            // {
            // update for the total
            for (int i = 0; i < (dgvCart.Rows.Count - 1); i++)
            {
                if (dgvCart.Rows[i].Cells[0].Value != null)
                {
                    // update the total
                    double total = double.Parse(dgvCart.Rows[i].Cells[4].Value.ToString()) * double.Parse(dgvCart.Rows[i].Cells[1].Value.ToString());
                    dgvCart.Rows[i].Cells[5].Value = string.Format("{0:C}", total);
                }
            }

            // update the full total price of items on the cart
            updateTotalPrice();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}
        }

        private void dlgPo_Load(object sender, EventArgs e)
        {
            populateSupplierTextbox();
            populateProductTextbox();
            btnCheckout.Visible = true;

            // GENERATE NEW PO
            txtPoNo.Text = generatePO();

            if (action == "Edit")
            {
                dgvCart.Enabled = false;
                txtAddress.ReadOnly = true;
                txtSupplier.ReadOnly = true;
                txtPoNo.ReadOnly = true;

                btnCheckout.Visible = false;
                txtAddress.Text = supplierAddress;
                txtSupplier.Text = supplierName;
                txtPoNo.Text = poId;
                int ctr = 0;
                dgvCart.RowCount = qty.Count;
                foreach (string q in qty)
                {
                    dgvCart.Rows[ctr].Cells[1].Value = q;
                    ctr++;
                }
                ctr = 0;
                foreach (string item in productName)
                {
                    dgvCart.Rows[ctr].Cells[2].Value = item;
                    ctr++;
                }
                ctr = 0;
                foreach (string unit in units)
                {
                    dgvCart.Rows[ctr].Cells[3].Value = unit;
                    ctr++;
                }
                ctr = 0;
                foreach (string price in productPrice)
                {
                    dgvCart.Rows[ctr].Cells[4].Value = price;
                    ctr++;
                }
                for (int i = 0; i < (ctr); i++)
                {
                    double total = double.Parse(dgvCart.Rows[i].Cells[4].Value.ToString()) * double.Parse(dgvCart.Rows[i].Cells[1].Value.ToString());
                    dgvCart.Rows[i].Cells[5].Value = string.Format("{0:C}", total);
                }
                updateTotalAmount();
            }
            else
            {
                btnCheckout.Visible = true;
                action = "Add";
            }

        }

        private void dgvCart_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCart.CurrentCell.ColumnIndex == 2)
            {
                try
                {
                    string[] item = getProductByName(dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[2].Value.ToString());

                    dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[0].Value = item[0];
                    //dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[1].Value = 1;
                    dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[3].Value = item[3];
                    dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[4].Value = item[4];
                    dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[5].Value = item[5];

                    dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[1].Selected = true;
                    updateTotalPrice();
                    cartUpdateTotal();
                }
                catch (Exception)
                {
                    MessageBox.Show(this, "Please enter a product", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            else
            {
                cartUpdateTotal();
                updateTotalPrice();
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (checkEmpty() == false)
            {
                MessageBox.Show("Please fill out all the required fields", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                for (int i = 0; i < (dgvCart.Rows.Count - 1); i++)
                {
                    string prodId = getProductID(dgvCart.Rows[i].Cells[2].Value.ToString());
                    string unitId = getUnitID(dgvCart.Rows[i].Cells[3].Value.ToString());
                    string str = txtSupplier.Text;
                    string[] supplierId = str.Split(new string[] { " - " }, StringSplitOptions.None);
                    string qty = dgvCart.Rows[i].Cells[1].Value.ToString();
                    string newPrice = dgvCart.Rows[i].Cells[4].Value.ToString();

                    insertPo(txtPoNo.Text, prodId, supplierId[1], qty, unitId, newPrice);
                    updateStocks(qty, prodId, newPrice);

                    //Update queue as well
                    if (checkProductQueue(prodId, newPrice) == true)
                    {
                        updateProductQueue(prodId, newPrice, qty);
                    }
                    else
                    {
                        insertProductQueue(prodId, newPrice, qty);
                    }

                    // LOGS
                    Classes.ActionLogger.LogAction(qty, unitId, prodId, "transaction", prodId.ToString(), "", "", "", newPrice, supplierId[1]);
                }

                MessageBox.Show(this, "PO successfully added", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvCart.Enabled = false;
                btnCheckout.Enabled = false;


                //DialogResult = DialogResult.OK;
            }
        }

        private void btnNewTransaction_Click(object sender, EventArgs e)
        {
            dgvCart.Rows.Clear();
            label1.Text = " PO";
            lblTotal.Text = "0.0";
            txtSupplier.Text = "";
            txtAddress.Text = "";
            txtSupplier.ReadOnly = false;
            txtAddress.ReadOnly = false;
            dgvCart.Enabled = true;
            btnCheckout.Visible = true;
            btnCheckout.Enabled = true;

            // GENERATE NEW OR
            txtPoNo.Text = generatePO();
        }

        private void dgvCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvCart_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox prodName = e.Control as TextBox;
            if (dgvCart.CurrentCell.ColumnIndex == 2)
            {
                if (prodName != null)
                {
                    prodName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    prodName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    prodName.AutoCompleteCustomSource = prodSource;
                }
            }
            else
            {
                prodName.AutoCompleteCustomSource = null;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSupplier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtAddress.Focus();
                string str = txtSupplier.Text;
                string[] supplierId = new string[2];
                supplierId = str.Split(new string[] { " - " }, StringSplitOptions.None);

                try
                {
                    string address = getSupplierAddress(supplierId[1]);

                    txtAddress.Text = address;
                }
                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show(this, "Supplier doesn't exists, please choose from the suggestions", "System error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSupplier.Text = "";
                }
            }
        }

        private void txtAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dgvCart.Focus();
            }
        }

        private void txtSupplier_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }
    }
}
