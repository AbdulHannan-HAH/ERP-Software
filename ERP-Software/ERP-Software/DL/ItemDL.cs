using System.Collections.Generic;
using System.Data.SqlClient;
using ERP_Software.Models;
using System.Configuration;
using Microsoft.Data.SqlClient;


namespace ERP_Software.DL
{
    public class ItemDL
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

        public static List<Item> GetAllItems()
        {
            List<Item> items = new List<Item>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"
                   SELECT i.ItemID, i.Name, i.Unit, i.MinLevel, i.QuantityInStock,
       i.StoreID, i.ClassID, s.Type AS StoreType, c.ClassName
FROM Items i
JOIN Stores s ON i.StoreID = s.StoreID
JOIN StoreClasses c ON i.ClassID = c.ClassID
";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    items.Add(new Item
                    {
                        ItemID = (int)reader["ItemID"],
                        Name = reader["Name"].ToString(),
                        Unit = reader["Unit"].ToString(),
                        MinLevel = (int)reader["MinLevel"],
                        StoreID = (int)reader["StoreID"],
                        StoreType = reader["StoreType"].ToString(),
                        ClassID = (int)reader["ClassID"],
                        ClassName = reader["ClassName"].ToString(),
                        QuantityInStock = (int)reader["QuantityInStock"]

                    });
                }
            }
            return items;
        }

        public static string AddItem(Item item)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "INSERT INTO Items (Name, Unit, MinLevel, StoreID, ClassID) VALUES (@Name, @Unit, @MinLevel, @StoreID, @ClassID)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Unit", item.Unit);
                cmd.Parameters.AddWithValue("@MinLevel", item.MinLevel);
                cmd.Parameters.AddWithValue("@StoreID", item.StoreID);
                cmd.Parameters.AddWithValue("@ClassID", item.ClassID);

                int rows = cmd.ExecuteNonQuery();
                return rows > 0 ? "✅ Item added successfully" : "❌ Failed to add item";
            }
        }

        public static string DeleteItem(int itemID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Items WHERE ItemID = @id", con);
                cmd.Parameters.AddWithValue("@id", itemID);
                int rows = cmd.ExecuteNonQuery();
                return rows > 0 ? "🗑️ Item deleted successfully" : "❌ Failed to delete item";
            }
        }

        //public static Item GetItemById(int itemId)
        //{
        //    Item item = null;
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        con.Open();
        //        string query = @"
        //    SELECT i.ItemID, i.Name, i.Unit, i.MinLevel, i.QuantityInStock,
        //           i.StoreID, i.ClassID, s.Type AS StoreType, c.ClassName
        //    FROM Items i
        //    JOIN Stores s ON i.StoreID = s.StoreID
        //    JOIN StoreClasses c ON i.ClassID = c.ClassID
        //    WHERE i.ItemID = @id";

        //        SqlCommand cmd = new SqlCommand(query, con);
        //        cmd.Parameters.AddWithValue("@id", itemId);
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            item = new Item
        //            {
        //                ItemID = (int)reader["ItemID"],
        //                Name = reader["Name"].ToString(),
        //                Unit = reader["Unit"].ToString(),
        //                MinLevel = (int)reader["MinLevel"],
        //                QuantityInStock = (int)reader["QuantityInStock"],
        //                StoreID = (int)reader["StoreID"],
        //                StoreType = reader["StoreType"].ToString(),
        //                ClassID = (int)reader["ClassID"],
        //                ClassName = reader["ClassName"].ToString()
        //            };
        //        }
        //    }
        //    return item;
        //}
        public static int GetCurrentStock(int itemId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT QuantityInStock FROM Items WHERE ItemID = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", itemId);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }
        public static void DecreaseItemStock(int itemId, int issuedQty)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE Items SET QuantityInStock = QuantityInStock - @qty WHERE ItemID = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@qty", issuedQty);
                cmd.Parameters.AddWithValue("@id", itemId);
                cmd.ExecuteNonQuery();
            }
        }



    }
}
