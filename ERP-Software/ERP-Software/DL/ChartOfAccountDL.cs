using ERP_Software.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace ERP_Software.DL
{
    public class ChartOfAccountDL
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

        public static List<ChartOfAccount> GetAllAccounts()
        {
            List<ChartOfAccount> list = new List<ChartOfAccount>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"SELECT c.*, p.AccountTitle AS ParentAccountTitle
                                 FROM ChartOfAccounts c
                                 LEFT JOIN ChartOfAccounts p ON c.ParentAccountID = p.AccountID";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new ChartOfAccount
                    {
                        AccountID = (int)reader["AccountID"],
                        AccountCode = reader["AccountCode"].ToString(),
                        AccountTitle = reader["AccountTitle"].ToString(),
                        ParentAccountID = reader["ParentAccountID"] == DBNull.Value ? null : (int?)reader["ParentAccountID"],
                        AccountType = reader["AccountType"].ToString(),
                        Description = reader["Description"].ToString(),
                        IsActive = (bool)reader["IsActive"],
                        ParentAccountTitle = reader["ParentAccountTitle"]?.ToString()
                    });
                }
            }
            return list;
        }

        public static string AddAccount(ChartOfAccount acc)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"INSERT INTO ChartOfAccounts
                                 (AccountCode, AccountTitle, ParentAccountID, AccountType, Description, IsActive)
                                 VALUES (@Code, @Title, @Parent, @Type, @Desc, 1)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Code", acc.AccountCode);
                cmd.Parameters.AddWithValue("@Title", acc.AccountTitle);
                cmd.Parameters.AddWithValue("@Parent", (object?)acc.ParentAccountID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Type", acc.AccountType);
                cmd.Parameters.AddWithValue("@Desc", acc.Description ?? "");

                int rows = cmd.ExecuteNonQuery();
                return rows > 0 ? "✅ Account added successfully!" : "❌ Failed to add account.";
            }
        }

        public static List<ChartOfAccount> GetParentAccounts()
        {
            List<ChartOfAccount> list = new();
            using SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string query = "SELECT AccountID, AccountTitle FROM ChartOfAccounts WHERE ParentAccountID IS NULL AND IsActive = 1";
            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new ChartOfAccount
                {
                    AccountID = (int)dr["AccountID"],
                    AccountTitle = dr["AccountTitle"].ToString()
                });
            }
            return list;
        }

        public static List<ChartOfAccount> GetChildAccounts(int parentId)
        {
            List<ChartOfAccount> list = new();
            using SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string query = "SELECT AccountID, AccountTitle FROM ChartOfAccounts WHERE ParentAccountID = @pid AND IsActive = 1";
            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@pid", parentId);
            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new ChartOfAccount
                {
                    AccountID = (int)dr["AccountID"],
                    AccountTitle = dr["AccountTitle"].ToString()
                });
            }
            return list;
        }

    }
}
