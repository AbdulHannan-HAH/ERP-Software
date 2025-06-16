using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using ERP_Software.Models;
using Microsoft.Data.SqlClient;

namespace ERP_Software.DL
{
    internal class UserDL
    {
        private static string conStr = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

        public static void InsertUser(User user)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"INSERT INTO Users (username, email, password_hash, role_id, created_by)
                                 VALUES (@Username, @Email, @PasswordHash, @RoleID, @CreatedBy)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                cmd.Parameters.AddWithValue("@RoleID", user.RoleID);
                cmd.Parameters.AddWithValue("@CreatedBy", user.CreatedBy);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static bool UsernameExists(string username)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT COUNT(*) FROM users WHERE username = @Username";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }
        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"SELECT u.user_id, u.username, u.email, u.password_hash, u.role_id, r.role AS RoleName
                         FROM Users u
                         JOIN userRoles r ON u.role_id = r.role_id";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        UserID = (int)reader["user_id"],
                        Username = reader["username"].ToString(),
                        PasswordHash = reader["password_hash"].ToString(),
                        Email = reader["email"].ToString(),
                        RoleID = (int)reader["role_id"],
                        RoleName = reader["RoleName"].ToString()
                    });
                }
            }
            return users;
        }


        public static void DeleteUser(int userId)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "DELETE FROM Users WHERE user_id = @UserID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserID", userId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdatePassword(int userId, string newHashedPassword)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "UPDATE Users SET password_hash = @Password WHERE user_id = @UserID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Password", newHashedPassword);
                cmd.Parameters.AddWithValue("@UserID", userId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
