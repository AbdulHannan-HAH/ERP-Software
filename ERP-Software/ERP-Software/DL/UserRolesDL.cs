using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using ERP_Software.Models;
using Microsoft.Data.SqlClient;

namespace ERP_Software.DL
{
    class UserRolesDL
    {
        private static string conStr = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

        public static void InsertRole(string roleName)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "INSERT INTO userRoles (role) VALUES (@RoleName)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@RoleName", roleName);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static bool RoleExists(string roleName)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT COUNT(*) FROM userRoles WHERE role = @RoleName";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@RoleName", roleName);
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public static List<Roles> GetAllRoles()
        {
            List<Roles> roles = new List<Roles>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT * FROM userRoles";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    roles.Add(new Roles
                    {
                        RoleID = (int)reader["role_id"],
                        RoleName = reader["role"].ToString()
                    });
                }
            }
            return roles;
        }

        public static void UpdateRole(int roleId, string newRoleName)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "UPDATE userRoles SET role = @NewRoleName WHERE role_id = @RoleID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@NewRoleName", newRoleName);
                cmd.Parameters.AddWithValue("@RoleID", roleId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteRole(int roleId)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "DELETE FROM userRoles WHERE role_id = @RoleID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@RoleID", roleId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
