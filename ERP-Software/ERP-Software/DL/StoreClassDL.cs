using System.Collections.Generic;
using System.Data.SqlClient;
using ERP_Software.Models;
using System.Configuration;
using Microsoft.Data.SqlClient;


namespace ERP_Software.DL
{
    public class StoreClassDL
    {
        private static string conStr = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

        public static void AddClass(StoreClass c)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string query = "INSERT INTO StoreClasses (ClassName, StoreTypeID) VALUES (@name, @type)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", c.ClassName);
                cmd.Parameters.AddWithValue("@type", c.StoreTypeID);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<StoreClass> GetAllClasses()
        {
            List<StoreClass> list = new List<StoreClass>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string query = @"SELECT sc.ClassID, sc.ClassName, sc.StoreTypeID, ps.store_name
                                 FROM StoreClasses sc
                                 JOIN procure_stores ps ON sc.StoreTypeID = ps.name_id";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new StoreClass
                    {
                        ClassID = (int)reader["ClassID"],
                        ClassName = reader["ClassName"].ToString(),
                        StoreTypeID = (int)reader["StoreTypeID"],
                        StoreTypeName = reader["store_name"].ToString()
                    });
                }
            }
            return list;
        }

        public static void DeleteClass(int id)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string query = "DELETE FROM StoreClasses WHERE ClassID = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
