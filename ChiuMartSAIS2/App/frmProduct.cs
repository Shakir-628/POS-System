﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using ChiuMartSAIS2.Classes;
using System.Data.SqlClient;

namespace ChiuMartSAIS2.App
{
    public partial class frmProduct : Form
    {
        public List<String> units = new List<string>();
        public List<String> category = new List<string>();

        private string productName;
        private string productCategory;
        private string productUnit;
        private double productStocks;
        private double productSafetyStock;
        private double productPrice;
        private double productId;
        private string barcode;
        private string retailPrice;
        private string status = "active";

        private Classes.Configuration conf;
        public frmProduct()
        {
            InitializeComponent();

            conf = new Classes.Configuration();
        }

        private void populateUnits()
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT * FROM units WHERE status = 'active'  ORDER BY unitDesc ASC";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    units.Clear();

                    while (reader.Read())
                    {
                        units.Add(reader["unitDesc"].ToString());
                    }

                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void populateCategory()
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT * FROM category WHERE status = 'active'  ORDER BY categoryName ASC";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    category.Clear();

                    while (reader.Read())
                    {
                        category.Add(reader["categoryName"].ToString());
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
        /// This function will get the products from the database and will populate
        /// the listview for products.
        /// </summary>
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
                        double discount = 0;
                        try
                        {
                            if (reader["retailPrice"] != null && reader["retailPrice"].ToString() != "")
                            {
                                discount = double.Parse(reader["retailPrice"].ToString()) - double.Parse(reader["productPrice"].ToString());
                            }
                        }
                        catch (Exception) { }
                        listView1.Items.Add(reader["productId"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["productName"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["unitDesc"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["productPrice"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["retailPrice"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(discount.ToString());

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
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["barcode"].ToString());


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
                        double discount = 0;
                        try
                        {
                            if (reader["retailPrice"] != null && reader["retailPrice"].ToString() != "")
                            {
                                discount = double.Parse(reader["retailPrice"].ToString()) - double.Parse(reader["productPrice"].ToString());
                            }
                        }
                        catch (Exception ) { }
                        listView1.Items.Add(reader["productId"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["productName"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["unitDesc"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["productPrice"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["retailPrice"].ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(discount.ToString());

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
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(reader["barcode"].ToString());
                    }

                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //get unit ID
        private double getUnitID(string crit)
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

                    double tmp = 0;
                    while (reader.Read())
                    {
                        tmp = Convert.ToDouble(reader["unitId"]);
                    }

                    return tmp;
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Message);
                    MessageBox.Show(this, "Error Retrieving unit id", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                }

            }
        }

        //get category ID
        private double getCategoryID(string crit)
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT categoryId FROM category WHERE categoryName = @crit";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    sqlCmd.Parameters.AddWithValue("crit", crit);

                    SqlDataReader reader = sqlCmd.ExecuteReader();



                    double tmp = 0;
                    while (reader.Read())
                    {
                        tmp = Convert.ToDouble(reader["categoryId"]);
                    }

                    return tmp;
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Message);
                    MessageBox.Show(this, "Error Retrieving category id", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                }

            }
        }

        private void insertProduct(double productPrice, double productStock, double productSafetyStock,
                    string productName, double unitId, double categoryId, string barcode, string retailPrice)
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "INSERT INTO products (productPrice, productStock, productSafetyStock, productName, unitId, categoryId, status,barcode , retailPrice) VALUES (@productPrice, @productStock, @productSafetyStock, @productName, @unitId, @categoryId, 'active', @barcode ,@retailPrice)";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    sqlCmd.Parameters.AddWithValue("productPrice", productPrice);
                    sqlCmd.Parameters.AddWithValue("productStock", productStock);
                    sqlCmd.Parameters.AddWithValue("productSafetyStock", productSafetyStock);
                    sqlCmd.Parameters.AddWithValue("productName", productName);
                    sqlCmd.Parameters.AddWithValue("unitId", unitId);
                    sqlCmd.Parameters.AddWithValue("categoryId", categoryId);
                    sqlCmd.Parameters.AddWithValue("barcode", barcode);
                    sqlCmd.Parameters.AddWithValue("retailPrice", retailPrice);


                    sqlCmd.ExecuteNonQuery();
                    new dbHelper().backupinset(sqlCmd, "INSERT", "products");
                    MessageBox.Show(this, "Product successfully added", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Adding new Product error", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void updateProduct(double productPrice, double productStock, double productSafetyStock,
                    string productName, double unitId, double categoryId, double criteria, string barcode, string retailPrice)
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "UPDATE products SET productPrice=@productPrice, productStock=@productStock, productSafetyStock=@productSafetyStock, productName=@productName, unitId=@unitId, categoryId=@categoryId ,barcode=@barcode ,retailPrice =@retailPrice WHERE productId=@criteria";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    sqlCmd.Parameters.AddWithValue("productPrice", productPrice);
                    sqlCmd.Parameters.AddWithValue("productStock", productStock);
                    sqlCmd.Parameters.AddWithValue("productSafetyStock", productSafetyStock);
                    sqlCmd.Parameters.AddWithValue("productName", productName);
                    sqlCmd.Parameters.AddWithValue("unitId", unitId);
                    sqlCmd.Parameters.AddWithValue("categoryId", categoryId);
                    sqlCmd.Parameters.AddWithValue("criteria", criteria);
                    sqlCmd.Parameters.AddWithValue("barcode", barcode);
                    sqlCmd.Parameters.AddWithValue("retailPrice", retailPrice);

                    sqlCmd.ExecuteNonQuery();
                    new dbHelper().backupinset(sqlCmd, "UPDATE", "products");
                    MessageBox.Show(this, "Product successfully updated", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Updating Product error", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void deleteProduct(double criteria)
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "UPDATE products SET status='inactive' WHERE productId=@criteria";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    sqlCmd.Parameters.AddWithValue("criteria", criteria);

                    sqlCmd.ExecuteNonQuery();
                    new dbHelper().backupinset(sqlCmd, "UPDATE", "products");
                    MessageBox.Show(this, "Product successfully deleted", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Deleting Product error", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void restoreProduct(double criteria)
        {
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "UPDATE products SET status='active' WHERE productId=@criteria";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    sqlCmd.Parameters.AddWithValue("criteria", criteria);

                    sqlCmd.ExecuteNonQuery();
                    new dbHelper().backupinset(sqlCmd, "UPDATE", "products");
                    MessageBox.Show(this, "Product successfully restored", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    MessageBox.Show(this, "Restoring Product error", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.dlgProducts frmProductsAdd = new Dialogs.dlgProducts("add", 0);
            populateUnits();
            populateCategory();
            frmProductsAdd.units = this.units;
            frmProductsAdd.category = this.category;
            if (frmProductsAdd.ShowDialog(this) == DialogResult.OK)
            {
                // If all validations were valid, we're going to get the category
                frmProductsAdd.getProduct(out productId, out productPrice, out productSafetyStock, out productStocks,
                    out productName, out productCategory, out productUnit, out barcode, out retailPrice);
                double unitId = getUnitID(productUnit);
                double categoryId = getCategoryID(productCategory);
                insertProduct(productPrice, productStocks, productSafetyStock,
                     productName, unitId, categoryId, barcode, retailPrice);
                populateProduct();
                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            Dialogs.dlgProducts frmProductsEdit = new Dialogs.dlgProducts("edit", productId);

            frmProductsEdit.productName = this.productName;
            frmProductsEdit.barcode = this.barcode;
            frmProductsEdit.productUnit = this.productUnit;
            frmProductsEdit.productPrice = this.productPrice;
            frmProductsEdit.productStocks = this.productStocks;
            frmProductsEdit.productSafetyStock = this.productSafetyStock;
            frmProductsEdit.productCategory = this.productCategory;
            frmProductsEdit.retailPrice = this.retailPrice;


            populateUnits();
            populateCategory();
            frmProductsEdit.units = this.units;
            frmProductsEdit.category = this.category;
            if (frmProductsEdit.ShowDialog(this) == DialogResult.OK)
            {
                // If all validations were valid, we're going to get the category
                frmProductsEdit.getProduct(out productId, out productPrice, out productSafetyStock, out productStocks,
                    out productName, out productCategory, out productUnit, out barcode, out retailPrice);
                double unitId = getUnitID(productUnit);
                double categoryId = getCategoryID(productCategory);
                updateProduct(productPrice, productStocks, productSafetyStock,
                     productName, unitId, categoryId, productId, barcode, retailPrice);
                populateProduct();
            }
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {
            if (Classes.Authentication.Instance.role != "Administrator")
            {
                btnEdit.Visible = false;
                btnDelete.Visible = false;
            }
            populateProduct();

        }

        private void listView1_Click(object sender, EventArgs e)
        {
            double id = double.Parse(listView1.SelectedItems[listView1.SelectedItems.Count - 1].Text);
            double stock = listView1.SelectedItems[listView1.SelectedItems.Count - 1].SubItems[6].Text == "" ? 0.0 : double.Parse(listView1.SelectedItems[listView1.SelectedItems.Count - 1].SubItems[6].Text);
            double price = listView1.SelectedItems[listView1.SelectedItems.Count - 1].SubItems[3].Text == "" ? 0.0 : double.Parse(listView1.SelectedItems[listView1.SelectedItems.Count - 1].SubItems[3].Text);
            double safety = listView1.SelectedItems[listView1.SelectedItems.Count - 1].SubItems[7].Text == "" ? 0.0 : double.Parse(listView1.SelectedItems[listView1.SelectedItems.Count - 1].SubItems[7].Text);

            productId = id;
            productStocks = stock;
            productPrice = price;
            productSafetyStock = safety;
            productUnit = listView1.SelectedItems[listView1.SelectedItems.Count - 1].SubItems[2].Text;
            productName = listView1.SelectedItems[listView1.SelectedItems.Count - 1].SubItems[1].Text;
            productCategory = listView1.SelectedItems[listView1.SelectedItems.Count - 1].SubItems[8].Text;
            barcode = listView1.SelectedItems[listView1.SelectedItems.Count - 1].SubItems[12].Text;
            retailPrice = listView1.SelectedItems[listView1.SelectedItems.Count - 1].SubItems[4].Text;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            if (btnDelete.Text == "&Delete")
            {
                if (MessageBox.Show(this, "Do you want to delete this product?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    deleteProduct(productId);
                    populateProduct();
                }
            }
            else
            {
                if (MessageBox.Show(this, "Do you want to restore this product?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    restoreProduct(productId);
                    populateProduct();
                }
            }

        }

        /// <summary>
        /// This function has a textchange event, it will call the search function
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event handler</param>
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
            btnDelete.Text = "&Delete";
            populateProduct();
        }

        private void rboInactive_CheckedChanged(object sender, EventArgs e)
        {
            status = "inactive";
            btnDelete.Text = "&Restore";
            populateProduct();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            Dialogs.dlgProducts frmProductsEdit = new Dialogs.dlgProducts("edit", productId);

            frmProductsEdit.productName = this.productName;
            frmProductsEdit.barcode = this.barcode;
            frmProductsEdit.productUnit = this.productUnit;
            frmProductsEdit.productPrice = this.productPrice;
            frmProductsEdit.productStocks = this.productStocks;
            frmProductsEdit.productSafetyStock = this.productSafetyStock;
            frmProductsEdit.productCategory = this.productCategory;
            frmProductsEdit.retailPrice = this.retailPrice;

            populateUnits();
            populateCategory();
            frmProductsEdit.units = this.units;
            frmProductsEdit.category = this.category;
            if (frmProductsEdit.ShowDialog(this) == DialogResult.OK)
            {
                // If all validations were valid, we're going to get the category
                frmProductsEdit.getProduct(out productId, out productPrice, out productSafetyStock, out productStocks,
                    out productName, out productCategory, out productUnit, out barcode, out retailPrice);
                double unitId = getUnitID(productUnit);
                double categoryId = getCategoryID(productCategory);
                updateProduct(productPrice, productStocks, productSafetyStock,
                     productName, unitId, categoryId, productId, barcode, retailPrice);
                populateProduct();
            }
        }

        private void frmProduct_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
