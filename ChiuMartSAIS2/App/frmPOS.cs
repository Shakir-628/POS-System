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

namespace ChiuMartSAIS2.App
{
    public partial class frmPOS : Form
    {
        private Classes.Configuration conf;
        private AutoCompleteStringCollection prodSource = new AutoCompleteStringCollection();
        private AutoCompleteStringCollection unitSource = new AutoCompleteStringCollection();
        private List<String> qty = new List<string>();
        private List<String> productName = new List<string>();
        private List<String> units = new List<string>();
        private List<String> productPrice = new List<string>();
        private List<int> poStock = new List<int>();
        private List<String> poSupplierPrice = new List<string>();
        private string orNo;
        private string clientName;
        private string clientAddress;
        private string action;
        private string bank = "";
        private string branch = "";
        private string chequeNo = "";
        private string chequeName = "";
        private string chequeDate = "";
        private string total = "";
        private string yellowBasyoReturned;
        private string transparentBasyoReturned;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F2)
            {
                btnCheckout.PerformClick();
                return true;    // indicate that you handled this keystroke
            }
            if (keyData == Keys.Escape)
            {
                button5.PerformClick(); // checkout button
                return true;    // indicate that you handled this keystroke
            }
            if (keyData == Keys.F1)
            {
                btnNewTransaction.PerformClick(); // checkout button
                return true;    // indicate that you handled this keystroke
            }
            if (keyData == Keys.F12)
            {
                btnVoid.PerformClick(); // checkout button
                return true;    // indicate that you handled this keystroke
            }
            if (keyData == Keys.Delete)
            {
                buttondel.PerformClick(); // checkout button
                return true;
            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        public frmPOS(List<String> _qty, List<String> _productName, List<String> _units, List<String> _productPrice, string _poId, string _clientName, string _clientAddress, string _action)
        {
            InitializeComponent();
            action = _action;
            qty = _qty;
            productName = _productName;
            units = _units;
            productPrice = _productPrice;
            orNo = _poId;
            clientName = _clientName;
            clientAddress = _clientAddress;

            conf = new Classes.Configuration();

            txtBarcode.Focus();
        }

        /// <summary>
        /// This function will get data from database and populate the textbox for client
        /// </summary>
        private void populateClientTextbox()
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT * FROM client WHERE status = 'active'";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string client = reader["clientName"] + " - " + reader["clientId"] + " - " + reader["clientContact"];
                        txtClient.AutoCompleteCustomSource.AddRange(new String[] { client });

                    }
                    txtClient.AutoCompleteMode = AutoCompleteMode.None;
                    // txtClient.AutoCompleteSource = AutoCompleteSource.CustomSource;
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FileLogger.WriteLog(ex.Message);
                }
            }
        }

        private string insertTransaction(string orNo, string productId, string clientId, string qty, string unitId, string paymentMethod, string supplierPrice, string unitPrice, string yellowBasyoReturned, string transparentBasyoReturned)
        {
            string id = "";
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "INSERT INTO [transaction] (orNo, productId, clientId, paymentMethod, qty, unitId, transStatus, supplierPrice, unitPrice, yellowBasyoReturned, transparentBasyoReturned) VALUES (@orNo, @productId, @clientId, @paymentMethod, @qty, @unitId, 'Completed', @supplierPrice, @unitPrice, @yellowBasyoReturned, @transparentBasyoReturned); SELECT SCOPE_IDENTITY();";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    sqlCmd.Parameters.AddWithValue("orNo", orNo);
                    sqlCmd.Parameters.AddWithValue("productId", productId);
                    sqlCmd.Parameters.AddWithValue("clientId", clientId);
                    sqlCmd.Parameters.AddWithValue("qty", qty);
                    sqlCmd.Parameters.AddWithValue("unitId", unitId);
                    sqlCmd.Parameters.AddWithValue("paymentMethod", paymentMethod);
                    sqlCmd.Parameters.AddWithValue("supplierPrice", supplierPrice);
                    sqlCmd.Parameters.AddWithValue("unitPrice", unitPrice);
                    sqlCmd.Parameters.AddWithValue("yellowBasyoReturned", yellowBasyoReturned);
                    sqlCmd.Parameters.AddWithValue("transparentBasyoReturned", transparentBasyoReturned);

                    id = Convert.ToString(sqlCmd.ExecuteScalar());

                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Transaction error", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FileLogger.WriteLog(ex.Message);
                }
            }
            return id;
        }

        private void updateStocks(string qty, string crit, string newPrice)
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    //string sqlQuery = "UPDATE products SET productStock = productStock - @qty, productPrice = @newPrice WHERE productId = @crit";
                    string sqlQuery = "UPDATE products SET productSafetyStock = productSafetyStock - @qty, productPrice = @newPrice WHERE productId = @crit";
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
                    FileLogger.WriteLog(ex.Message);
                }
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
                    FileLogger.WriteLog(ex.Message);
                    return "";
                }

            }
        }

        private void getPoQueue(string crit)
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT * FROM po_queue WHERE product_id = @crit";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    sqlCmd.Parameters.AddWithValue("crit", crit);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    poStock.Clear();
                    poSupplierPrice.Clear();

                    while (reader.Read())
                    {
                        poStock.Add(Int32.Parse(reader["stock"].ToString()));
                        poSupplierPrice.Add(reader["supplier_price"].ToString());
                    }

                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Message);
                    MessageBox.Show(this, "Error Retrieving queue stocks", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FileLogger.WriteLog(ex.Message);
                }

            }
        }

        private void insertCheque(string bank, string branch, string chequeName, string chequeDate, string chequeNo, string amount)
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    DateTime chequeDateFinal;
                    DateTime.TryParse(chequeDate, out chequeDateFinal);

                    Con.Open();
                    string sqlQuery = "INSERT INTO cheque (chequeNo, chequeName, chequeBank, chequeBranch, chequeAmount, chequeDate, status) VALUES (@chequeNo, @chequeName, @chequeBank, @chequeBranch, @chequeAmount, @chequeDate, 'active')";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    sqlCmd.Parameters.AddWithValue("chequeNo", chequeNo);
                    sqlCmd.Parameters.AddWithValue("chequeName", chequeName);
                    sqlCmd.Parameters.AddWithValue("chequeBank", bank);
                    sqlCmd.Parameters.AddWithValue("chequeBranch", branch);
                    sqlCmd.Parameters.AddWithValue("chequeAmount", amount);
                    sqlCmd.Parameters.AddWithValue("chequeDate", chequeDateFinal.ToString("yyyy-MM-dd"));

                    sqlCmd.ExecuteNonQuery();
                    new dbHelper().backupinset(sqlCmd, "INSERT", "cheque");
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Transaction error", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FileLogger.WriteLog(ex.Message);
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
                    string sqlQuery = "SELECT productId FROM products WHERE productName = @crit or barcode = @crit ";
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
                    FileLogger.WriteLog(ex.Message);
                    return "";
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
                    FileLogger.WriteLog(ex.Message);
                }
            }
        }

        private void updateProductQueue(string productId, string price, string stock)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conf.connectionstring))
                {
                    con.Open();
                    string sqlQuery = "UPDATE po_queue SET stock = stock - @stock WHERE product_id = @productId AND supplier_price = @price";
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
                MessageBox.Show(this, "Adding new po error", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileLogger.WriteLog(ex.Message);
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
                    string sqlQuery = "SELECT p.*, u.*, c.* FROM products as p INNER JOIN units as u ON p.unitId = u.unitId INNER JOIN category as c ON p.categoryId = c.categoryId WHERE p.status = 'active' AND (p.productName = @prodname or p.barcode=@prodname)";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("prodname", prodname);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result[0] = reader["productId"].ToString();
                        result[1] = "1";
                        result[2] = reader["productName"].ToString();
                        result[3] = reader["unitDesc"].ToString();
                        result[4] = reader["productPrice"].ToString();

                        double total = double.Parse(result[1]) * double.Parse(result[4]);
                        result[5] = total.ToString();
                    }
                    return result;
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FileLogger.WriteLog(ex.Message);
                    return result;
                }
            }
        }

        private void clearUI()
        {
            dgvCart.Rows.Clear();
            lbl_Heading.Text = " POS";
            lblTotal.Text = "0.0";
            txtAddress.Text = "";
            txtClient.Text = "Walk-in Client";
            dgvCart.Enabled = true;
            txtAddress.ReadOnly = false;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            txtClient.ReadOnly = false;
            btnCheckout.Enabled = true;
            // GENERATE NEW OR
            txtOrNo.Text = generateOR();
            txtClient.Focus();
        }

        private String getProductSupplierPrice(string prodname)
        {
            string result = "";
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    //string sqlQuery = "SELECT supplier_price FROM products INNER JOIN po_queue ON products.productId = po_queue.product_id WHERE products.productName = @prodname AND stock > 0 ORDER BY po_queue.id ASC LIMIT 1";
                    string sqlQuery = "SELECT top 1 retailPrice FROM products  WHERE products.productName = @prodname";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("prodname", prodname);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result = reader["retailPrice"].ToString();
                    }
                    return result;
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FileLogger.WriteLog(ex.Message);
                    return result;
                }
            }
        }

        private String getProductProductPrice(string prodname)
        {
            string result = "";
            string client = "0";

            if (txtClient.Text == "")
            {
                return "0";
            }

            if (txtClient.Text == "Walk-in Client")
            {
                client = "0";
            }
            else
            {
                string[] clientId = txtClient.Text.Split(new string[] { " - " }, StringSplitOptions.None);
                client = clientId[1];
            }


            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT p.productPrice FROM products p INNER JOIN [transaction] t  ON p.productId = t.productId WHERE p.productName = @prodname AND t.clientId = @clientId";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("prodname", prodname);
                    sqlCmd.Parameters.AddWithValue("clientId", client);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result = reader["productPrice"].ToString();
                    }
                    return result;
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FileLogger.WriteLog(ex.Message);
                    return result;
                }
            }
        }

        private String getClientAddress(string id)
        {
            string result = "";
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT clientAddress FROM client WHERE clientId = @clientId";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("clientId", id);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result = reader["clientAddress"].ToString();
                    }
                    return result;
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FileLogger.WriteLog(ex.Message);
                    return result;
                }
            }
        }

        /// <summary>
        /// Check the product stock
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns>Product Stock</returns>
        private int checkProductStockById(int id)
        {
            int result = 0;
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT p.* FROM products as p WHERE p.status = 'active' AND p.productId = @id";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("id", id);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result = Int32.Parse(reader["productStock"].ToString());
                    }
                    return result;
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FileLogger.WriteLog(ex.Message);
                    return result;
                }
            }
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            dbHelper objDb = new Classes.dbHelper();

            lbl_Heading.Text = objDb.getConfigurationValue(dbHelper.configinfo.company_name.ToString());
            populateClientTextbox();
            populateProductTextbox();


            // Set status bar labels
            txtCashier.Text = Classes.Authentication.Instance.userFullName;
            lblUsername.Text = Classes.Authentication.Instance.username;
            lblDate.Text = DateTime.Today.ToLongDateString().ToString();
            //lblTime.Text = DateTime.Today.ToLocalTime().ToString();
            if (action == "birReport")
            {
                button8.Visible = false;
                btnNewTransaction.Visible = false;
                btnBasyo.Visible = false;
                btnCheckout.Text = "Print";
                btnVoid.Text = "Remove";
                populatePosUi();

            }
            else
            {
                // GENERATE NEW OR
                txtOrNo.Text = generateOR();
            }
            this.ActiveControl = txtBarcode;// .Focus();
        }

        private void txtClient_Click(object sender, EventArgs e)
        {
            txtClient.SelectAll();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Parse the product string
            if (txtAddress.Text != "")
            {
                string[] product = Regex.Split(txtAddress.Text, " - ");
                int id = Int32.Parse(product[1]);

                // update the full total price of items on the cart
                updateTotalPrice();
            }
            else
            {
                MessageBox.Show("You have to select a product first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void txtProduct_Click(object sender, EventArgs e)
        {
            txtAddress.SelectAll();
        }

        private void dgvCart_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // cartUpdateTotal();
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
                if (action == "birReport")
                {
                    if (dgvCart.Rows[i].Cells[4].Value != null)
                    {
                        double totalBIR = double.Parse(dgvCart.Rows[i].Cells[4].Value.ToString()) * double.Parse(dgvCart.Rows[i].Cells[1].Value.ToString());
                        dgvCart.Rows[i].Cells[5].Value = string.Format("{0:C}", totalBIR);
                    }
                }
                else
                {
                    // Check the update on quantity if there's enough stocks
                    // if not, set it to maximum stocks
                    if (dgvCart.Rows[i].Cells[0].Value != null)
                    {
                        int id = Int32.Parse(dgvCart.Rows[i].Cells[0].Value.ToString());
                        int stock = checkProductStockById(id);
                        int updatedStock = Int32.Parse(dgvCart.Rows[i].Cells[1].Value.ToString());

                        //if (stock < updatedStock && stock != 0)
                        //{
                        //    MessageBox.Show(this, "Insufficient Stocks\nYou only have " + stock + " left for this product", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    dgvCart.Rows[i].Cells[1].Value = 0;
                        //}
                        //else if (stock == 0)
                        //{
                        //    MessageBox.Show(this, "You do not have stocks for this product", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    dgvCart.Rows[i].Cells[1].Value = 0;
                        //    dgvCart.Rows.Remove(dgvCart.Rows[i]);
                        //}

                        // update the total
                        string tmpQty = dgvCart.Rows[i].Cells[4].Value.ToString() == null || dgvCart.Rows[i].Cells[4].Value.ToString() == "" ? "0" : dgvCart.Rows[i].Cells[4].Value.ToString();
                        double total = double.Parse(tmpQty) * double.Parse(dgvCart.Rows[i].Cells[1].Value.ToString());
                        dgvCart.Rows[i].Cells[5].Value = string.Format("{0:C}", total);

                    }
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

        private void updateTotalPrice()
        {
            try
            {
                double total = 0;
                for (int i = 0; i < (dgvCart.Rows.Count - 1); i++)
                {
                    total += double.Parse(dgvCart.Rows[i].Cells[5].Value.ToString(), System.Globalization.NumberStyles.Currency);
                }

                if (checkBox1.Checked == true)
                {
                    total -= double.Parse(txtTransBasyo.Text) * 100;
                }

                if (checkBox1.Checked == true)
                {
                    total -= double.Parse(txtYellowBasyo.Text) * 130;
                }

                lblTotal.Text = string.Format("{0:C}", total);

            }
            catch (Exception ex)
            {
                FileLogger.WriteLog(ex.Message);
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

                lblTotal.Text = total.ToString();

            }
            catch (Exception ex)
            {
                FileLogger.WriteLog(ex.Message);
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            string transId = string.Empty;

            if (action == "birReport")
            {
                if (dgvCart.Rows.Count <= 1)
                {
                    MessageBox.Show("There's no item to print", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                // Check if ability to checkout
                if (dgvCart.Rows.Count <= 1)
                {
                    MessageBox.Show("There's no item available for checkout because the cart is empty or the total is zero", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Open the checkout form
                Dialogs.dlgCheckout frm = new Dialogs.dlgCheckout("POS");
                // Set the variables
                frm.total = lblTotal.Text;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < (dgvCart.Rows.Count - 1); i++)
                    {
                        string paymentMethod = "";

                        frm.getProduct(out paymentMethod, out bank, out branch, out chequeName, out chequeDate, out total, out chequeNo);
                        if (paymentMethod == "Cheque")
                        {
                            insertCheque(bank, branch, chequeName, chequeDate, chequeNo, total);
                        }

                        // Check if the cell has a product, if not, continue the loop
                        if (dgvCart.Rows[i].Cells[3].Value.ToString() == "")
                        {
                            continue;
                        }

                        string prodId = getProductID(dgvCart.Rows[i].Cells[3].Value.ToString());
                        string unitId = getUnitID(dgvCart.Rows[i].Cells[2].Value.ToString());
                        string currentPrice = getProductProductPrice(dgvCart.Rows[i].Cells[3].Value.ToString());
                        string supplierPrice = getProductSupplierPrice(dgvCart.Rows[i].Cells[3].Value.ToString());

                        string str = txtClient.Text;
                        string[] clientId = new string[2];
                        if (str == "Walk-in Client")
                        {
                            clientId[1] = "0";
                        }
                        else
                        {
                            clientId = str.Split(new string[] { " - " }, StringSplitOptions.None);
                        }
                        string qty = dgvCart.Rows[i].Cells[1].Value.ToString();
                        string newPrice = dgvCart.Rows[i].Cells[4].Value.ToString();
                        int prodqty = Int32.Parse(qty);

                        //getPoQueue(prodId);
                        transId = insertTransaction(txtOrNo.Text, prodId, clientId[1], prodqty.ToString(), unitId, paymentMethod, supplierPrice, newPrice, txtYellowBasyo.Text, txtTransBasyo.Text);
                        //for (int j = 0; j < poStock.Count; j++)
                        //{
                        //    if (poStock[j] >= prodqty)
                        //    {
                        //        updateProductQueue(prodId, poSupplierPrice[j], prodqty.ToString());
                        //        transId = insertTransaction(txtOrNo.Text, prodId, clientId[1], prodqty.ToString(), unitId, paymentMethod, poSupplierPrice[j], newPrice, txtYellowBasyo.Text, txtTransBasyo.Text);
                        //        break;
                        //    }
                        //    else
                        //    {
                        //        updateProductQueue(prodId, poSupplierPrice[j], poStock[j].ToString());
                        //        if (poStock[j] != 0)
                        //        {
                        //            transId = insertTransaction(txtOrNo.Text, prodId, clientId[1], poStock[j].ToString(), unitId, paymentMethod, poSupplierPrice[j], newPrice, txtYellowBasyo.Text, txtTransBasyo.Text);
                        //        }
                        //        prodqty = prodqty - poStock[j];
                        //    }
                        //}
                        dbHelper dh = new dbHelper();


                        dh.changeInsert(frm.change, frm.paid, transId);
                        updateStocks(qty, prodId, newPrice);
                        // updateProductQueue(prodId, supplierPrice, qty);

                        // LOGS
                        Classes.ActionLogger.LogAction(qty, unitId, prodId, "transaction", prodId.ToString(), clientId[1], "", newPrice, "", "");
                    }



                    if (frm.isToken == true)
                    {
                        Reports.frmTokenReport rptT = new Reports.frmTokenReport();
                        rptT.orno = txtOrNo.Text;
                        rptT.ShowDialog();
                    }

                    Reports.frmOrReport rpt = new Reports.frmOrReport();
                    rpt.orno = txtOrNo.Text;
                    rpt.ShowDialog();

                    btnCheckout.Enabled = false;
                    insertNewOR();
                    //MessageBox.Show(this, "Transaction Complete", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearUI();
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Open the transaction history form
            Dialogs.dlgTransactionHistoy frm = new Dialogs.dlgTransactionHistoy();
            // Set the variables
            if (frm.ShowDialog() == DialogResult.OK)
            {
                frm.getTransaction(out qty, out productName, out units, out productPrice,
                out orNo, out clientName, out clientAddress, out action, out yellowBasyoReturned, out transparentBasyoReturned);
                if (action == "View")
                {

                    dgvCart.Enabled = false;
                    txtAddress.ReadOnly = true;
                    txtClient.ReadOnly = true;
                    btnCheckout.Enabled = false;
                    txtTransBasyo.Text = transparentBasyoReturned;
                    txtYellowBasyo.Text = yellowBasyoReturned;

                    lbl_Heading.Text = "View Transaction";

                    populatePosUi();
                }
                else
                {
                    lbl_Heading.Text = " POS";
                }
            }
        }

        private void populatePosUi()
        {
            txtAddress.Text = clientAddress;
            txtClient.Text = clientName;
            if (clientName == "")
            {
                txtClient.Text = "Walk-in Client";
            }
            txtOrNo.Text = orNo;
            int ctr = 0;
            if (action == "birReport")
            {
                dgvCart.RowCount = qty.Count + 1;
            }
            else
            {
                dgvCart.RowCount = qty.Count;
            }
            foreach (string q in qty)
            {
                dgvCart.Rows[ctr].Cells[1].Value = q;
                ctr++;
            }
            ctr = 0;
            foreach (string item in productName)
            {
                dgvCart.Rows[ctr].Cells[3].Value = item;
                ctr++;
            }
            ctr = 0;
            foreach (string unit in units)
            {
                dgvCart.Rows[ctr].Cells[2].Value = unit;
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

        /// <summary>
        /// GENERATE NEW OR NUMBER
        /// </summary>
        private string generateOR()
        {
            string lastOrNumber = "";
            DateTime today = DateTime.Today;
            int currentYear = today.Year;
            int currentMonth = today.Month + 1;
            string currentdate = DateTime.Now.ToString("yyMMddHHmmss");

            string generatedOR = "";

            // Get the last OR number
            try
            {
                using (SqlConnection con = new SqlConnection(conf.connectionstring))
                {
                    con.Open();
                    string sqlQuery = "SELECT top 1 * FROM [or] ORDER BY id DESC";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, con);
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        lastOrNumber = reader["ornumber"].ToString();
                    }
                }
            }
            catch (SqlException ex)
            {
                string errorCode = string.Format("Error Code : {0}", ex.Number);
                MessageBox.Show(this, "Can't connect to database: " + ex.Message.ToString(), errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileLogger.WriteLog(ex.Message);
            }

            // Check if there is no last OR number
            if (lastOrNumber == "")
            {
                // Create the first one
                //generatedOR = currentYear.ToString() + currentMonth.ToString("00")  + "0001";
                generatedOR = currentdate + "1";
            }
            else
            {

                try
                {
                    string lastOr = lastOrNumber.Substring(12);
                    int orNum = Int32.Parse(lastOr) + 1;

                    //generatedOR = currentYear.ToString() + currentMonth.ToString() + orNum.ToString("000000");
                    generatedOR = currentdate + orNum.ToString();
                    if (double.Parse(generatedOR) >= 20000000000000)
                    {
                        orNum = 1;
                        generatedOR = currentdate + orNum.ToString();
                    }
                }
                catch (Exception)
                {
                    int orNum = 1;
                    generatedOR = currentdate + orNum.ToString();
                }
            }

            return generatedOR;
        }

        private void insertNewOR()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conf.connectionstring))
                {
                    con.Open();
                    string sqlQuery = "INSERT INTO [or](ornumber) VALUES(@ornumber)";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, con);
                    sqlCmd.Parameters.AddWithValue("ornumber", txtOrNo.Text);

                    sqlCmd.ExecuteNonQuery();
                    new dbHelper().backupinset(sqlCmd, "INSERT", "OR");
                }
            }
            catch (SqlException ex)
            {
                string errorCode = string.Format("Error Code : {0}", ex.Number);
                MessageBox.Show(this, "Can't connect to database: " + ex.Message.ToString(), errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileLogger.WriteLog(ex.Message);
            }
        }

        private void dgvCart_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox prodName = e.Control as TextBox;
            if (dgvCart.CurrentCell.ColumnIndex == 3)
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

        private void dgvCart_EditModeChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("Done editing");
        }

        private Boolean checkProduct(string productName)
        {
            bool result = false;
            int count = 0;

            for (int i = 0; i < (dgvCart.Rows.Count - 1); i++)
            {
                if (dgvCart.Rows[i].Cells[3].Value.ToString() == productName)
                {
                    count++;
                }

                if (count > 1)
                {
                    result = true;
                }
            }

            return result;
        }

        private void dgvCart_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCart.CurrentCell.ColumnIndex == 3)
            {
                if (dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[3].Value == null)
                {
                    return;
                }
                // Check if the product is already on the cart
                if (checkProduct(dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[3].Value.ToString()))
                {
                    if (MessageBox.Show("This product is already exists on the CART. Do you want to add this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        dgvCart.Rows.Remove(dgvCart.Rows[dgvCart.CurrentRow.Index]);
                        return;
                    }
                }

                try
                {
                    string[] item = getProductByName(dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[3].Value.ToString());

                    dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[0].Value = item[0];
                    //dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[1].Value = 1;
                    dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[2].Value = item[3];

                    string price = getProductProductPrice(dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[3].Value.ToString());
                    if (price == "") { price = item[4]; }
                    dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[4].Value = price == "" ? "0" : price;
                    dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[5].Value = string.Format("{0:C}", item[5]);

                    if (action != "birReport")
                    {
                        for (int i = 0; i < (dgvCart.Rows.Count - 1); i++)
                        {
                            // Check the update on quantity if there's enough stocks
                            // if not, set it to maximum stocks
                            int id = Int32.Parse(dgvCart.Rows[i].Cells[0].Value.ToString());
                            int stock = checkProductStockById(id);
                            int updatedStock = Int32.Parse(dgvCart.Rows[i].Cells[1].Value.ToString());

                            //if (stock < updatedStock && stock != 0)
                            //{
                            //    MessageBox.Show(this, "Insufficient Stocks\nYou only have " + stock + " left for this product", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //    dgvCart.Rows[i].Cells[1].Value = 0;

                            //}
                            //else if (stock == 0)
                            //{
                            //    MessageBox.Show(this, "You do not have stocks for this product", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //    dgvCart.Rows[i].Cells[1].Value = 0;
                            //    dgvCart.Rows.Remove(dgvCart.Rows[i]);
                            //}
                        }
                    }
                    dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[1].Selected = true;
                    updateTotalPrice();
                    cartUpdateTotal();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Please enter a product", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MessageBox.Show(ex.Message.ToString());
                    FileLogger.WriteLog(ex.Message);
                }
            }
            else if (dgvCart.CurrentCell.ColumnIndex == 4)
            {
                if (action != "birReport")
                {
                    string currentPrice = getProductProductPrice(dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[3].Value.ToString());
                    double supplierPrice = double.Parse(getProductSupplierPrice(dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[3].Value.ToString()));
                    double updatedPrice = double.Parse(dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[4].Value.ToString());

                    if (updatedPrice < supplierPrice)
                    {
                        MessageBox.Show(this, "The price is less than the supplier price", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[4].Value = currentPrice;
                    }
                    else if (updatedPrice == supplierPrice)
                    {
                        MessageBox.Show(this, "The price is equal to the supplier price", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                cartUpdateTotal();
                updateTotalPrice();
            }
            else
            {
                cartUpdateTotal();
                updateTotalPrice();
            }
        }

        private void btnNewTransaction_Click(object sender, EventArgs e)
        {
            clearUI();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                MessageBox.Show("You entered");
            }
        }
        private void txtClient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                listBox1.Focus();

            }
            if (e.KeyCode == Keys.Enter)
            {
                txtAddress.Focus();
                string str = txtClient.Text;
                string[] clientId = new string[2];
                if (str == "Walk-in Client")
                {
                    clientId[1] = "0";
                }
                else
                {
                    clientId = str.Split(new string[] { " - " }, StringSplitOptions.None);
                }

                string address = "";

                try
                {
                    address = getClientAddress(clientId[1]);
                }
                catch (IndexOutOfRangeException ex)
                {
                    MessageBox.Show(this, "Client doesn't exists. Reverting to walk-in client", "System error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtClient.Text = "Walk-in Client";
                    FileLogger.WriteLog(ex.Message);
                }

                txtAddress.Text = address;

                int rowCount = dgvCart.Rows.Count;

                try
                {
                    for (int i = 0; i < rowCount; i++)
                    {
                        dgvCart.Rows.RemoveAt(i);
                        --rowCount;
                    }
                }
                catch (Exception ex)
                {
                    FileLogger.WriteLog(ex.Message);
                }

                cartUpdateTotal();
            }
        }
        private void txtAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dgvCart.Focus();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            /*
            if (Classes.GetLastUserInput.formStatusIdle == false)
            {
                if (Classes.GetLastUserInput.GetIdleTickCount() >= 300000)
                {
                    Classes.GetLastUserInput.formStatusIdle = true;
                    Dialogs.dlgIdleStatus idle = new Dialogs.dlgIdleStatus();
                    idle.ShowDialog();
                }
            }*/

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                txtTransBasyo.Enabled = true;
                txtTransBasyo.Focus();
                txtTransBasyo.SelectAll();
            }
            else
            {
                txtTransBasyo.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                txtYellowBasyo.Enabled = true;
                txtYellowBasyo.Focus();
                txtYellowBasyo.SelectAll();
            }
            else
            {
                txtYellowBasyo.Enabled = false;
            }
        }

        private void frmPOS_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void txtTransBasyo_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                updateTotalPrice();
            }
        }

        private void txtYellowBasyo_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                updateTotalPrice();
            }
        }

        private void btnVoid_Click(object sender, EventArgs e)
        {
            if (action == "birReport")
            {
                if (dgvCart.SelectedRows.Count != 0)
                {
                    int selectedIndex = dgvCart.CurrentCell.RowIndex;
                    if (selectedIndex > -1)
                    {
                        dgvCart.Rows.RemoveAt(selectedIndex);
                        dgvCart.Refresh(); // if needed
                        updateTotalAmount();
                    }
                }
            }
            else
            {
                clearUI();
            }
        }

        private void btnBasyo_Click(object sender, EventArgs e)
        {
            frmBasyo frm = new frmBasyo();
            frm.ShowDialog();
        }

        private void dgvCart_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (dgvCart.CurrentRow != null)
            {
                dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[1].Value = "1";
            }
        }

        private void txtBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            //if ((e.KeyCode & Keys.Enter) == Keys.Enter)
            if (e.KeyCode == Keys.Enter)
            {
                string strCurrentString = txtBarcode.Text.Trim().ToString();
                if (strCurrentString != "")
                {
                    fillrowsbytext();
                    //Do something with the barcode entered 
                    txtBarcode.Text = "";
                }
                txtBarcode.Focus();
            }
        }

        private void txtBarcode_Leave(object sender, EventArgs e)
        {
            fillrowsbytext();
        }
        private void addrowingrid() { }
        private void fillrowsbytext()
        {
            //if (txtBarcode.Text.Count() < 4) { return; }
            try
            {
                int cnt = dgvCart.Rows.Count - 1;
                string[] item = getProductByName(txtBarcode.Text.ToString());



                if (item != null && item.Count() >= 4 && item[2] != null)
                {
                    if (item[2] != "")
                    {
                        lblProduct.Text = item[2];

                        txtBarcode.Text = "";
                    }
                    bool go = qtyaddProduct(item[2]);
                    if (!go)
                    {
                        DataGridViewRow row = (DataGridViewRow)dgvCart.Rows[0].Clone();
                        row.Cells[0].Value = item[0];
                        row.Cells[1].Value = 1;
                        row.Cells[2].Value = item[3];
                        row.Cells[3].Value = item[2];
                        //string price = getProductProductPrice(item[2]);
                        //row.Cells[4].Value = price == "" ? "0" : price;
                        row.Cells[4].Value = item[4];
                        row.Cells[5].Value = string.Format("{0:C}", item[5]);



                        //dgvCart.Rows[cnt].Cells[4].Value = price == "" ? "0" : price;
                        //dgvCart.Rows[cnt].Cells[5].Value = string.Format("{0:C}", item[5]);

                        dgvCart.Rows.Add(row);




                        if (action != "birReport")
                        {
                            for (int i = 0; i < (dgvCart.Rows.Count - 1); i++)
                            {
                                // Check the update on quantity if there's enough stocks
                                // if not, set it to maximum stocks
                                int id = Int32.Parse(dgvCart.Rows[i].Cells[0].Value.ToString());
                                int stock = checkProductStockById(id);
                                int updatedStock = Int32.Parse(dgvCart.Rows[i].Cells[1].Value.ToString());

                                //if (stock < updatedStock && stock != 0)
                                //{
                                //    MessageBox.Show(this, "Insufficient Stocks\nYou only have " + stock + " left for this product", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //    dgvCart.Rows[i].Cells[1].Value = 0;

                                //}
                                //else if (stock == 0)
                                //{
                                //    MessageBox.Show(this, "You do not have stocks for this product", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //    dgvCart.Rows[i].Cells[1].Value = 0;
                                //    dgvCart.Rows.Remove(dgvCart.Rows[i]);
                                //}
                            }
                        }
                        // dgvCart.Rows[dgvCart.CurrentRow.Index].Cells[1].Selected = true;
                        updateTotalPrice();
                        cartUpdateTotal();
                        txtBarcode.Focus();
                    }
                }
                else
                {

                    MessageBox.Show("Product not found", "Message", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                FileLogger.WriteLog(ex.Message);
                //MessageBox.Show(this, "Please enter a product or barcode", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // MessageBox.Show(ex.Message.ToString());rc
            }
        }

        private Boolean qtyaddProduct(string productName)
        {
            bool result = false;
            int count = 0;

            for (int i = 0; i < (dgvCart.Rows.Count - 1); i++)
            {
                if (dgvCart.Rows[i].Cells[3].Value.ToString() == productName)
                {
                    try
                    {
                        dgvCart.Rows[i].Cells[1].Value = Convert.ToInt32(dgvCart.Rows[i].Cells[1].Value) + 1;

                        // dgvCart.Rows[i].Cells[5].Value = Convert.ToInt32(dgvCart.Rows[i].Cells[5].Value) + Convert.ToInt32(dgvCart.Rows[i].Cells[5].Value)

                        dgvCart_CellEndEdit(null, null);

                        count++;
                    }
                    catch (Exception e) { FileLogger.WriteLog(e.Message); }

                }

                if (count >= 1)
                {
                    result = true;
                }
            }

            return result;
        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void listBox1_Leave(object sender, EventArgs e)
        {
            hideResults();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtClient.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
            txtAddress.Focus();
            string str = txtClient.Text;
            string[] clientId = new string[2];
            if (str == "Walk-in Client")
            {
                clientId[1] = "0";
            }
            else
            {
                clientId = str.Split(new string[] { " - " }, StringSplitOptions.None);
            }

            string address = "";

            try
            {
                address = getClientAddress(clientId[1]);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(this, "Client doesn't exists. Reverting to walk-in client", "System error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtClient.Text = "Walk-in Client";
                FileLogger.WriteLog(ex.Message);
            }

            txtAddress.Text = address;

            hideResults();
        }
        void hideResults()
        {
            listBox1.Visible = false;
        }
        private void txtClient_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (txtClient.Text.Length == 0)
            {
                hideResults();
                return;
            }

            foreach (String s in txtClient.AutoCompleteCustomSource)
            {
                if (s.ToUpper().Contains(txtClient.Text.ToUpper()))
                {
                    // Console.WriteLine("Found text in: " + s);
                    listBox1.Items.Add(s);
                    listBox1.Visible = true;
                }
            }
        }


        private void buttondel_Click(object sender, EventArgs e)
        {

            Int32 selectedRowCount = dgvCart.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                for (int i = 0; i < selectedRowCount; i++)
                {
                    dgvCart.Rows.RemoveAt(dgvCart.SelectedRows[0].Index);


                }
                updateTotalPrice();
                cartUpdateTotal();
            }
        }
    }
}
