using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using ERP_Software.Models;
using Microsoft.Data.SqlClient;

namespace ERP_Software.DL
{
    public class StoreIssueNoteDL
    {
        private static string conStr = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

        public static string AddSIN(StoreIssueNote sin)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string query = "INSERT INTO StoreIssueNote (MRID, ItemID, IssuedQty, IssueDate, IssuedBy, Remarks, CostCenterID) " +
                               "VALUES (@MRID, @ItemID, @IssuedQty, @IssueDate, @IssuedBy, @Remarks, @CostCenterID)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@MRID", sin.MRID);
                cmd.Parameters.AddWithValue("@ItemID", sin.ItemID);
                cmd.Parameters.AddWithValue("@IssuedQty", sin.IssuedQty);
                cmd.Parameters.AddWithValue("@IssueDate", sin.IssueDate);
                cmd.Parameters.AddWithValue("@IssuedBy", sin.IssuedBy);
                cmd.Parameters.AddWithValue("@Remarks", sin.Remarks);
                cmd.Parameters.AddWithValue("@CostCenterID", sin.CostCenterID);

                return cmd.ExecuteNonQuery() > 0 ? "✅ Issue note saved." : "❌ Failed to save issue note.";
            }
        }

        public static List<StoreIssueNote> GetAllSINs()
        {
            List<StoreIssueNote> list = new List<StoreIssueNote>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string query = @"SELECT s.SINID, s.MRID, s.ItemID, s.IssuedQty, s.IssueDate, s.IssuedBy, s.Remarks, s.CostCenterID,
                        i.Name AS ItemName, c.Name AS CostCenterName
                        FROM StoreIssueNote s
                        JOIN Items i ON s.ItemID = i.ItemID
                        JOIN CostCenters c ON s.CostCenterID = c.CostCenterID";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new StoreIssueNote
                    {
                        SINID = (int)reader["SINID"],
                        MRID = reader["MRID"].ToString(), // Assuming MRID is string
                        ItemID = (int)reader["ItemID"],
                        IssuedQty = (int)reader["IssuedQty"],
                        IssueDate = Convert.ToDateTime(reader["IssueDate"]), // ✅ FIXED HERE
                        IssuedBy = reader["IssuedBy"].ToString(),
                        Remarks = reader["Remarks"].ToString(),
                        CostCenterID = (int)reader["CostCenterID"],
                        ItemName = reader["ItemName"].ToString(),
                        CostCenterName = reader["CostCenterName"].ToString()
                    });
                }
            }
            return list;
        }

    }
}