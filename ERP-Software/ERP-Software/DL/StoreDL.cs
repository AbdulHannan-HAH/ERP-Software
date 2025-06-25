using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ERP_Software.Models;

namespace ERP_Software.DL
{
    class StoreDL
    {
        private static string conStr = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

        public static List<Stores> GetAllStores()
        {
            List<Stores> stores = new List<Stores>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT * FROM Stores";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    stores.Add(new Stores
                    {
                        StoreID = (int)dr["StoreID"],
                        StoreName = dr["StoreName"].ToString(),
                        Type = dr["Type"].ToString(),
                        Location = dr["Location"].ToString()
                    });
                }
                dr.Close();
            }
            return stores;
        }
        public static bool StoreExists(string storeName)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT COUNT(*) FROM Stores WHERE StoreName = @storeName";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@storeName", storeName);
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public static void AddStore(Stores store)
        {
            string accountCode = GenerateNextAccountCode();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // Step 1: Insert into Chart of Accounts (COA)
                    string coaQuery = @"
                INSERT INTO ChartOfAccounts (AccountTitle, ParentAccountID, AccountType, Description, AccountCode) 
                VALUES (@title, @parentId, @type, @desc, @accCode); 
                SELECT SCOPE_IDENTITY();";

                    SqlCommand coaCmd = new SqlCommand(coaQuery, con, transaction);
                    coaCmd.Parameters.AddWithValue("@title", $"Store - {store.StoreName}");
                    coaCmd.Parameters.AddWithValue("@parentId", 8); // Adjust this parent ID as per your ChartOfAccounts hierarchy
                    coaCmd.Parameters.AddWithValue("@type", "Asset");
                    coaCmd.Parameters.AddWithValue("@desc", $"Auto-created account for {store.StoreName}");
                    coaCmd.Parameters.AddWithValue("@accCode", accountCode);

                    int coaID = Convert.ToInt32(coaCmd.ExecuteScalar());

                    // Step 2: Insert into Stores table
                    string storeQuery = @"
                INSERT INTO Stores (StoreName, Type, Location, COAAccountID) 
                VALUES (@name, @type, @location, @coaID)";

                    SqlCommand storeCmd = new SqlCommand(storeQuery, con, transaction);
                    storeCmd.Parameters.AddWithValue("@name", store.StoreName);
                    storeCmd.Parameters.AddWithValue("@type", store.Type);
                    storeCmd.Parameters.AddWithValue("@location", store.Location);
                    storeCmd.Parameters.AddWithValue("@coaID", coaID);

                    storeCmd.ExecuteNonQuery();

                    // ✅ Commit transaction if all succeeded
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("❌ Failed to add store with COA account.\n" + ex.Message);
                }
            }
        }


        public static void UpdateStore(Stores store)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // Step 1: Update store
                    string storeQuery = "UPDATE Stores SET StoreName=@name, Location=@location WHERE StoreID=@id";
                    SqlCommand storeCmd = new SqlCommand(storeQuery, con, transaction);
                    storeCmd.Parameters.AddWithValue("@name", store.StoreName);
                    storeCmd.Parameters.AddWithValue("@location", store.Location);
                    storeCmd.Parameters.AddWithValue("@id", store.StoreID);
                    storeCmd.ExecuteNonQuery();

                    // Step 2: Get COA ID
                    string getCOAQuery = "SELECT COAAccountID FROM Stores WHERE StoreID = @id";
                    SqlCommand getCmd = new SqlCommand(getCOAQuery, con, transaction);
                    getCmd.Parameters.AddWithValue("@id", store.StoreID);
                    object result = getCmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int coaID = Convert.ToInt32(result);

                        // Step 3: Update COA title
                        string updateCOAQuery = "UPDATE ChartOfAccounts SET AccountTitle = @title WHERE AccountID = @id";
                        SqlCommand coaCmd = new SqlCommand(updateCOAQuery, con, transaction);
                        coaCmd.Parameters.AddWithValue("@title", $"Store - {store.StoreName}");
                        coaCmd.Parameters.AddWithValue("@id", coaID);
                        coaCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("❌ Failed to update store and COA.\n" + ex.Message);
                }
            }
        }

        public static string GenerateNextAccountCode()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT AccountCode FROM ChartOfAccounts WHERE AccountCode LIKE 'STR-%'";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                int maxNumber = 0;

                while (reader.Read())
                {
                    string code = reader["AccountCode"].ToString();
                    if (code.Length > 4 && int.TryParse(code.Substring(4), out int num))
                    {
                        if (num > maxNumber)
                            maxNumber = num;
                    }
                }

                reader.Close();
                return $"STR-{(maxNumber + 1).ToString("D4")}";
            }
        }


        public static void DeleteStore(int id)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // Step 1: Get COA ID first
                    int coaID = 0;
                    string getCOAQuery = "SELECT COAAccountID FROM Stores WHERE StoreID = @id";
                    SqlCommand getCmd = new SqlCommand(getCOAQuery, con, transaction);
                    getCmd.Parameters.AddWithValue("@id", id);
                    object result = getCmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        coaID = Convert.ToInt32(result);
                    }

                    // Step 2: Delete store
                    string deleteStoreQuery = "DELETE FROM Stores WHERE StoreID = @id";
                    SqlCommand storeCmd = new SqlCommand(deleteStoreQuery, con, transaction);
                    storeCmd.Parameters.AddWithValue("@id", id);
                    storeCmd.ExecuteNonQuery();

                    // Step 3: Delete COA account (optional - only if it's not linked elsewhere)
                    if (coaID > 0)
                    {
                        string deleteCOAQuery = "DELETE FROM ChartOfAccounts WHERE AccountID = @coaId";
                        SqlCommand coaCmd = new SqlCommand(deleteCOAQuery, con, transaction);
                        coaCmd.Parameters.AddWithValue("@coaId", coaID);
                        coaCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("❌ Failed to delete store and COA.\n" + ex.Message);
                }
            }
        }
        public static List<Stores> GetAllStoresWithCOA()
        {
            List<Stores> stores = new List<Stores>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
            SELECT s.*, c.AccountTitle AS COAAccountTitle
            FROM Stores s
            LEFT JOIN ChartOfAccounts c ON s.COAAccountID = c.AccountID";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    stores.Add(new Stores
                    {
                        StoreID = (int)dr["StoreID"],
                        StoreName = dr["StoreName"].ToString(),
                        Type = dr["Type"].ToString(),
                        Location = dr["Location"].ToString(),
                        COAAccountID = (int)dr["COAAccountID"],
                        COAAccountTitle = dr["COAAccountTitle"].ToString()
                    });
                }
                dr.Close();
            }
            return stores;
        }


    }
}
