using ChiuMartSAIS2.Classes;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiuMartSAIS2.Classes
{
    public class dbHelper
    {
        public enum configinfo
        {
            company_name,
            company_address
        }
        private Classes.Configuration conf;


        public string getConfigurationValue(string nam)
        {
            conf = new Classes.Configuration();
            string result = "";
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT p.value FROM config as p WHERE p.status = '1' AND p.name = @name";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("name", nam);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result = reader["value"].ToString();
                    }
                    return result;
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    //  MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return result;
                }

            }
        }
        public DataTable getorReport(string orno)
        {
            conf = new Classes.Configuration();
            DataTable result = new DataTable();
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    //                    string sqlQuery = @"SELECT transaction.orNo, products.productName, units.unitDesc, IFNULL(client.clientName,'Walk in Customer') as clientName, client.clientAddress, transaction.qty, transaction.unitPrice,products.retailPrice,IFNULL(products.retailPrice -  transaction.unitPrice,0) AS discount, transaction.transDate
                    //FROM            category INNER JOIN
                    //                         products ON category.categoryId = products.categoryId INNER JOIN
                    //                         transaction ON products.productId = transaction.productId INNER JOIN
                    //                         units ON products.unitId = units.unitId LEFT OUTER JOIN
                    //                         client ON transaction.clientId = client.clientId
                    //WHERE        (transaction.orNo = @orno)";
                    string sqlQuery = @"SELECT trans.orNo, products.productName,change.change,change.paid, units.unitDesc, IsNULL(client.clientName,'Walk in Customer') AS clientName, client.clientAddress, trans.qty, trans.unitPrice,products.retailPrice,ISNULL(products.retailPrice -  trans.unitPrice,0) AS discount, trans.transDate,client.clientContact
FROM category INNER JOIN
                         products ON category.categoryId = products.categoryId INNER JOIN
                         [TRANSACTION] trans ON products.productId = trans.productId 
                         INNER JOIN units ON products.unitId = units.unitId LEFT OUTER JOIN
                         CLIENT ON trans.clientId = client.clientId
                         RIGHT JOIN change ON change.transId = trans.transId
WHERE (trans.orNo = @orno)";




                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("orno", orno);

                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    result.Load(reader);

                    return result;
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    //  MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return result;
                }

            }
        }


        public DataTable getDetails()
        {
            conf = new Classes.Configuration();
            DataTable result = new DataTable();
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    //                    string sqlQuery = @"select * from
                    //(
                    // (SELECT p.value as company_name FROM config as p WHERE p.status = '1' AND p.name ='company_name' )
                    // as y 
                    //   join 
                    //    (
                    //SELECT p.value as company_address FROM config as p WHERE p.status = '1' AND p.name ='company_address'
                    //    ) as j 
                    //join 
                    //    (
                    //SELECT p.value as company_phone FROM config as p WHERE p.status = '1' AND p.name ='company_phone'
                    //    ) as i 
                    //join 
                    //    (
                    //SELECT p.value as company_policy FROM config as p WHERE p.status = '1' AND p.name ='company_policy'
                    //    ) as k
                    //join 
                    //    (
                    //SELECT p.value as developer_info FROM config as p WHERE p.status = '1' AND p.name ='developer_info'
                    //    ) as l

                    //)";
                    string sqlQuery = @"SELECT 
    MAX(CASE WHEN CAST(p.name AS VARCHAR(MAX)) = 'company_name' THEN CAST(p.value AS VARCHAR(MAX)) END) AS company_name,
    MAX(CASE WHEN CAST(p.name AS VARCHAR(MAX)) = 'company_address' THEN CAST(p.value AS VARCHAR(MAX)) END) AS company_address,
    MAX(CASE WHEN CAST(p.name AS VARCHAR(MAX)) = 'company_mobile' THEN CAST(p.value AS VARCHAR(MAX)) END) AS company_phone,
    MAX(CASE WHEN CAST(p.name AS VARCHAR(MAX)) = 'company_policy' THEN CAST(p.value AS VARCHAR(MAX)) END) AS company_policy,
    MAX(CASE WHEN CAST(p.name AS VARCHAR(MAX)) = 'company_website' THEN CAST(p.value AS VARCHAR(MAX)) END) AS developer_info
FROM config p
WHERE p.status = '1';";
                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    // sqlCmd.Parameters.AddWithValue("orno", orno);

                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    result.Load(reader);

                    return result;
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    //  MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return result;
                }

            }
        }

        public bool checkbarcode(string bar)
        {
            conf = new Classes.Configuration();
            string result = "";
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT p.barcode FROM products as p WHERE   p.barcode = @barcode";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("barcode", bar);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result = reader["barcode"].ToString();
                    }
                    if (result == "")
                    { return false; }
                    else { return true; }
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    //  MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
        }


        public void changeInsert(string change, string paid, string transId)
        {
            conf = new Classes.Configuration();

            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "insert into change(change,paid,transId) values (@change,@paid,@transid)";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("change", change);
                    sqlCmd.Parameters.AddWithValue("paid", paid);
                    sqlCmd.Parameters.AddWithValue("transid", transId);

                    sqlCmd.ExecuteNonQuery();
                    new dbHelper().backupinset(sqlCmd, "INSERT", "change");

                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    //  MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }

        public bool checkLiecense()
        {
            conf = new Classes.Configuration();
            string token = "";
            string dt = "";
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    // string sqlQuery = "SELECT token ,expiryDate FROM company order by id ";
                    //string sqlQuery = "SELECT AES_DECRYPT(c.token,'ponka') as token ,AES_DECRYPT(c.expiryDate, 'ponka') as expiryDate FROM company as c order by c.id ";
                    string sqlQuery = "SELECT c.token,c.expiryDate FROM company as c order by c.id ";


                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    //sqlCmd.Parameters.AddWithValue("barcode", bar);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        token = reader["token"].ToString();
                        dt = reader["expiryDate"].ToString();
                    }

                    if (token == "" || dt == "")
                    { return false; }

                    DateTime oDate = Convert.ToDateTime(dt);
                    //string tok = Classes.zipper.Decrypt(token, 19);
                    // if (Classes.functions.isMac(tok))
                    if (oDate >= DateTime.Now)
                    {

                        if (oDate >= DateTime.Now)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    return false;
                    //  MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void licenseInsert(string licenseText, string expiryDate, string token)
        {
            conf = new Classes.Configuration();

            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    //string sqlQuery = "insert into company(license,expiryDate,token) values (@licenseText, AES_ENCRYPT(@expiryDate,'ponka'),AES_ENCRYPT(@token,'ponka')  )";
                    string sqlQuery = "insert into company(license,expiryDate,token) values (@licenseText, @expiryDate,@token)";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("licenseText", licenseText);
                    sqlCmd.Parameters.AddWithValue("expiryDate", expiryDate);
                    sqlCmd.Parameters.AddWithValue("token", token);

                    sqlCmd.ExecuteNonQuery();
                    new dbHelper().backupinset(sqlCmd, "INSERT", "company");
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    //  MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void backupinset(SqlCommand sqlCmdQry, string QryType, string tableName)
        {

            string QueryText = ToStringP(sqlCmdQry, sqlCmdQry.CommandText);


            conf = new Classes.Configuration();

            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    //string sqlQuery = "insert into company(license,expiryDate,token) values (@licenseText, AES_ENCRYPT(@expiryDate,'ponka'),AES_ENCRYPT(@token,'ponka')  )";
                    string sqlQuery = @"insert into [Backup](
             status,
             queryType,
             tableName,            
             query)
VALUES(0,@queryType,@tableName,@query)";

                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("queryType", QryType);
                    sqlCmd.Parameters.AddWithValue("tableName", tableName);
                    sqlCmd.Parameters.AddWithValue("query", QueryText);

                    sqlCmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                }
            }
        }

        public string ToStringP(SqlCommand parameters, string sqlQuery)
        {
            return parameters.Parameters.Cast<SqlParameter>().Aggregate(sqlQuery, (current, p) => current.Replace("@" + p.ParameterName, "'" + p.Value.ToString() + "'"));
        }
        public bool syncLive()
        {

            conf = new Classes.Configuration();
            string qry = "";
            string id = "";
            using (SqlConnection Con = new SqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT c.query,c.id FROM [Backup] as c where status = 0 order by c.id ";


                    SqlCommand sqlCmd = new SqlCommand(sqlQuery, Con);

                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        qry = reader["query"].ToString();
                        id = reader["id"].ToString();
                        ////////////////// //sent code
                        if (!functions.CheckForInternetConnection()) { return false; }
                        #region update code 


                        string sqlQueryupdate = "update [Backup] set status = 1 where id=@id";

                        SqlCommand sqlCmdu = new SqlCommand(sqlQueryupdate, Con);
                        sqlCmdu.Parameters.AddWithValue("id", id);

                        sqlCmdu.ExecuteNonQuery();
                        #endregion
                    }

                    if (qry == "")
                    { return false; }


                }
                catch (SqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    return false;
                    //  MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return false;
        }

    }
}
