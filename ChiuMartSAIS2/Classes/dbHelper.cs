using ChiuMartSAIS2.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
            using (MySqlConnection Con = new MySqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT p.value FROM config as p WHERE p.status = '1' AND p.name = @name";

                    MySqlCommand sqlCmd = new MySqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("name", nam);

                    MySqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result = reader["value"].ToString();
                    }
                    return result;
                }
                catch (MySqlException ex)
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
            using (MySqlConnection Con = new MySqlConnection(conf.connectionstring))
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
                    string sqlQuery = @"SELECT transaction.orNo, products.productName,`change`.`change`,`change`.`paid`, units.unitDesc, IFNULL(client.clientName,'Walk in Customer') AS clientName, client.clientAddress, transaction.qty, transaction.unitPrice,products.retailPrice,IFNULL(products.retailPrice -  transaction.unitPrice,0) AS discount, transaction.transDate,client.clientContact
FROM category INNER JOIN
                         products ON category.categoryId = products.categoryId INNER JOIN
                         TRANSACTION ON products.productId = transaction.productId 
                         INNER JOIN units ON products.unitId = units.unitId LEFT OUTER JOIN
                         CLIENT ON transaction.clientId = client.clientId
                         RIGHT JOIN `change` ON `change`.`transId` = transaction.`transId`
WHERE (transaction.orNo = @orno)";




                    MySqlCommand sqlCmd = new MySqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("orno", orno);

                    MySqlDataReader reader = sqlCmd.ExecuteReader();
                    result.Load(reader);

                    return result;
                }
                catch (MySqlException ex)
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
            using (MySqlConnection Con = new MySqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = @"select * from
(
 (SELECT p.value as company_name FROM config as p WHERE p.status = '1' AND p.name ='company_name' )
 as y 
   join 
    (
SELECT p.value as company_address FROM config as p WHERE p.status = '1' AND p.name ='company_address'
    ) as j 
join 
    (
SELECT p.value as company_phone FROM config as p WHERE p.status = '1' AND p.name ='company_phone'
    ) as i 
join 
    (
SELECT p.value as company_policy FROM config as p WHERE p.status = '1' AND p.name ='company_policy'
    ) as k
join 
    (
SELECT p.value as developer_info FROM config as p WHERE p.status = '1' AND p.name ='developer_info'
    ) as l

)";

                    MySqlCommand sqlCmd = new MySqlCommand(sqlQuery, Con);
                    // sqlCmd.Parameters.AddWithValue("orno", orno);

                    MySqlDataReader reader = sqlCmd.ExecuteReader();
                    result.Load(reader);

                    return result;
                }
                catch (MySqlException ex)
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
            using (MySqlConnection Con = new MySqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT p.barcode FROM products as p WHERE   p.barcode = @barcode";

                    MySqlCommand sqlCmd = new MySqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("barcode", bar);

                    MySqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result = reader["barcode"].ToString();
                    }
                    if (result == "")
                    { return false; }
                    else { return true; }
                }
                catch (MySqlException ex)
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

            using (MySqlConnection Con = new MySqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "insert into `change`(`change`,paid,transId) values (@change,@paid,@transid)";

                    MySqlCommand sqlCmd = new MySqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("change", change);
                    sqlCmd.Parameters.AddWithValue("paid", paid);
                    sqlCmd.Parameters.AddWithValue("transid", transId);

                    sqlCmd.ExecuteNonQuery();
                    new dbHelper().backupinset(sqlCmd, "INSERT", "change");

                }
                catch (MySqlException ex)
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
            using (MySqlConnection Con = new MySqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    // string sqlQuery = "SELECT token ,expiryDate FROM company order by id ";
                    //string sqlQuery = "SELECT AES_DECRYPT(c.token,'ponka') as token ,AES_DECRYPT(c.expiryDate, 'ponka') as expiryDate FROM company as c order by c.id ";
                    string sqlQuery = "SELECT c.token,c.expiryDate FROM company as c order by c.id ";


                    MySqlCommand sqlCmd = new MySqlCommand(sqlQuery, Con);
                    //sqlCmd.Parameters.AddWithValue("barcode", bar);

                    MySqlDataReader reader = sqlCmd.ExecuteReader();

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
                catch (MySqlException ex)
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

            using (MySqlConnection Con = new MySqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    //string sqlQuery = "insert into `company`(`license`,expiryDate,token) values (@licenseText, AES_ENCRYPT(@expiryDate,'ponka'),AES_ENCRYPT(@token,'ponka')  )";
                    string sqlQuery = "insert into `company`(`license`,expiryDate,token) values (@licenseText, @expiryDate,@token)";

                    MySqlCommand sqlCmd = new MySqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("licenseText", licenseText);
                    sqlCmd.Parameters.AddWithValue("expiryDate", expiryDate);
                    sqlCmd.Parameters.AddWithValue("token", token);

                    sqlCmd.ExecuteNonQuery();
                    new dbHelper().backupinset(sqlCmd, "INSERT", "company");
                }
                catch (MySqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                    //  MessageBox.Show(this, "Can't connect to database", errorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void backupinset(MySqlCommand sqlCmdQry, string QryType, string tableName)
        {

            string QueryText = ToStringP(sqlCmdQry, sqlCmdQry.CommandText);


            conf = new Classes.Configuration();

            using (MySqlConnection Con = new MySqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    //string sqlQuery = "insert into `company`(`license`,expiryDate,token) values (@licenseText, AES_ENCRYPT(@expiryDate,'ponka'),AES_ENCRYPT(@token,'ponka')  )";
                    string sqlQuery = @"insert into `Backup` (
             `status`,
             `queryType`,
             `tableName`,            
             `query`)
VALUES(   0,   @queryType,@tableName,@query)";

                    MySqlCommand sqlCmd = new MySqlCommand(sqlQuery, Con);
                    sqlCmd.Parameters.AddWithValue("queryType", QryType);
                    sqlCmd.Parameters.AddWithValue("tableName", tableName);
                    sqlCmd.Parameters.AddWithValue("query", QueryText);

                    sqlCmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    string errorCode = string.Format("Error Code : {0}", ex.Number);
                }
            }
        }

        public string ToStringP(MySqlCommand parameters, string sqlQuery)
        {
            return parameters.Parameters.Cast<MySqlParameter>().Aggregate(sqlQuery, (current, p) => current.Replace("@" + p.ParameterName, "'" + p.Value.ToString() + "'"));
        }
        public bool syncLive()
        {

            conf = new Classes.Configuration();
            string qry = "";
            string id = "";
            using (MySqlConnection Con = new MySqlConnection(conf.connectionstring))
            {
                try
                {
                    Con.Open();
                    string sqlQuery = "SELECT c.query,c.id FROM backup as c where status = 0 order by c.id ";


                    MySqlCommand sqlCmd = new MySqlCommand(sqlQuery, Con);

                    MySqlDataReader reader = sqlCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        qry = reader["query"].ToString();
                        id = reader["id"].ToString();
                        ////////////////// //sent code
                        if (!functions.CheckForInternetConnection()) { return false; }
                        #region update code 


                        string sqlQueryupdate = "update `backup` set status =1 where id=@id";

                        MySqlCommand sqlCmdu = new MySqlCommand(sqlQueryupdate, Con);
                        sqlCmdu.Parameters.AddWithValue("id", id);

                        sqlCmdu.ExecuteNonQuery();
                        #endregion
                    }

                    if (qry == "")
                    { return false; }


                }
                catch (MySqlException ex)
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
