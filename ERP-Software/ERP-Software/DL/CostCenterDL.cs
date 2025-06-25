using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using ERP_Software.Models;
using Microsoft.Data.SqlClient;


namespace ERP_Software.DL
{
    public class CostCenterDL
    {
        private static string conStr = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

        public static List<CostCenter> GetAll()
        {
            List<CostCenter> list = new List<CostCenter>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string query = "SELECT * FROM CostCenters WHERE IsActive = 1";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new CostCenter
                    {
                        CostCenterID = (int)dr["CostCenterID"],
                        Name = dr["Name"].ToString(),
                        Description = dr["Description"].ToString(),
                        IsActive = (bool)dr["IsActive"],
                        CreatedAt = Convert.ToDateTime(dr["CreatedAt"]),
                        CreatedBy = dr["CreatedBy"].ToString()
                    });
                }
            }
            return list;
        }

        public static bool Add(CostCenter cc)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "INSERT INTO CostCenters (Name, Description, CreatedBy) VALUES (@name, @desc, @createdBy)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", cc.Name);
                cmd.Parameters.AddWithValue("@desc", cc.Description);
                cmd.Parameters.AddWithValue("@createdBy", cc.CreatedBy);
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "UPDATE CostCenters SET IsActive = 0 WHERE CostCenterID = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
