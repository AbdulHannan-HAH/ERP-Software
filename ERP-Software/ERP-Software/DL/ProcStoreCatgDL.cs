using ERP_Software.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.DL
{
    internal class ProcStoreCatgDL
    {
        private static string conStr = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

        public static void InsertStoreName(string storeName)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "INSERT INTO procure_stores (store_name) VALUES (@storeName)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@storeName", storeName);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public static bool StoreExists(string storeName)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT COUNT(*) FROM procure_stores WHERE store_name = @storeName";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@storeName", storeName);
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }
        public static List<ProcStoreCatg> GetAllStoreNames()
        {
            List<ProcStoreCatg> roles = new List<ProcStoreCatg>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT * FROM procure_stores";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    roles.Add(new ProcStoreCatg
                    {
                        storeid = (int)reader["name_id"],
                        storename = reader["store_name"].ToString()
                    });
                }
            }
            return roles;
        }

        public static void UpdateStoreName(int storeid, string storename)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "UPDATE procure_stores SET store_name = @storename WHERE name_id = @storeid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@storename", storename);
                cmd.Parameters.AddWithValue("@storeid", storeid);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteStore(int storeId)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "DELETE FROM procure_stores WHERE name_id = @storeID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@storeID", storeId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
