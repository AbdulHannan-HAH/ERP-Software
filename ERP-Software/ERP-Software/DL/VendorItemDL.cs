using ERP_Software.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

public static class VendorItemDL
{
    private static string conStr = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

    public static List<VendorItem> GetAllVendorItems()
    {
        List<VendorItem> list = new List<VendorItem>();
        using (SqlConnection con = new SqlConnection(conStr))
        {
            con.Open();
            string query = @"SELECT vi.VendorItemID, vi.VendorID, v.VendorName AS VendorName,
                                    vi.ItemID, i.Name AS ItemName, vi.LastPrice, vi.LeadTimeDays, vi.IsActive
                             FROM VendorItems vi
                             JOIN Vendors v ON vi.VendorID = v.VendorID
                             JOIN Items i ON vi.ItemID = i.ItemID";

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new VendorItem
                {
                    VendorItemID = (int)dr["VendorItemID"],
                    VendorID = (int)dr["VendorID"],
                    VendorName = dr["VendorName"].ToString(),
                    ItemID = (int)dr["ItemID"],
                    ItemName = dr["ItemName"].ToString(),
                    LastPrice = (decimal)dr["LastPrice"],
                    LeadTimeDays = (int)dr["LeadTimeDays"],
                    IsActive = (bool)dr["IsActive"]
                });
            }
        }
        return list;
    }

    public static string AddVendorItem(VendorItem vi)
    {
        using (SqlConnection con = new SqlConnection(conStr))
        {
            con.Open();
            string query = "INSERT INTO VendorItems (VendorID, ItemID, LastPrice, LeadTimeDays, IsActive) " +
                           "VALUES (@VendorID, @ItemID, @LastPrice, @LeadTimeDays, @IsActive)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@VendorID", vi.VendorID);
            cmd.Parameters.AddWithValue("@ItemID", vi.ItemID);
            cmd.Parameters.AddWithValue("@LastPrice", vi.LastPrice);
            cmd.Parameters.AddWithValue("@LeadTimeDays", vi.LeadTimeDays);
            cmd.Parameters.AddWithValue("@IsActive", vi.IsActive);

            return cmd.ExecuteNonQuery() > 0 ? "✅ Linked successfully" : "❌ Failed to link";
        }
    }
}
