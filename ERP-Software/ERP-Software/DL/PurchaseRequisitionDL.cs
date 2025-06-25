using ERP_Software.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

public static class PurchaseRequisitionDL
{
    private static string cs = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

    public static List<PurchaseRequisition> GetPRs(int vendorId, int storeId)
    {
        var list = new List<PurchaseRequisition>();
        using var con = new SqlConnection(cs);
        string q = "SELECT PRID, VendorID, StoreID, PRDate, Status FROM PurchaseRequisitions WHERE VendorID=@v AND StoreID=@s";
        using var cmd = new SqlCommand(q, con);
        cmd.Parameters.AddWithValue("@v", vendorId);
        cmd.Parameters.AddWithValue("@s", storeId);
        con.Open();
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new PurchaseRequisition
            {
                PRID = (int)r["PRID"],
                VendorID = (int)r["VendorID"],
                StoreID = (int)r["StoreID"],
                PRDate = (DateTime)r["PRDate"],
                Status = r["Status"].ToString()
            });
        }
        return list;
    }

    public static List<PRItem> GetPRItems(int prid)
    {
        var list = new List<PRItem>();
        using var con = new SqlConnection(cs);
        string q = @"SELECT pi.PRItemID, pi.PRID, pi.ItemID, i.Name AS ItemName, pi.Quantity, pi.Rate
                     FROM PRItems pi
                     JOIN Items i ON pi.ItemID=i.ItemID
                     WHERE PRID=@prid";
        using var cmd = new SqlCommand(q, con);
        cmd.Parameters.AddWithValue("@prid", prid);
        con.Open();
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new PRItem
            {
                PRItemID = (int)r["PRItemID"],
                PRID = (int)r["PRID"],
                ItemID = (int)r["ItemID"],
                ItemName = r["ItemName"].ToString(),
                Quantity = (int)r["Quantity"],
                Rate = r["Rate"] as decimal?
            });
        }
        return list;
    }
    public static string AddPR(PurchaseRequisition pr, List<PRItem> items)
    {
        using SqlConnection con = new SqlConnection(cs);
        con.Open();
        SqlTransaction tr = con.BeginTransaction();

        try
        {
            // Step 1: Insert into PurchaseRequisitions
            string insertPR = @"INSERT INTO PurchaseRequisitions (VendorID, StoreID, PRDate, Status)
                            VALUES (@VendorID, @StoreID, @PRDate, @Status);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand cmdPR = new SqlCommand(insertPR, con, tr);
            cmdPR.Parameters.AddWithValue("@VendorID", pr.VendorID);
            cmdPR.Parameters.AddWithValue("@StoreID", pr.StoreID);
            cmdPR.Parameters.AddWithValue("@PRDate", pr.PRDate);
            cmdPR.Parameters.AddWithValue("@Status", pr.Status);

            int prid = Convert.ToInt32(cmdPR.ExecuteScalar());

            // Step 2: Insert all PR items
            foreach (var item in items)
            {
                SqlCommand cmdItem = new SqlCommand(@"
                INSERT INTO PRItems (PRID, ItemID, Quantity, Rate)
                VALUES (@PRID, @ItemID, @Qty, @Rate);", con, tr);

                cmdItem.Parameters.AddWithValue("@PRID", prid);
                cmdItem.Parameters.AddWithValue("@ItemID", item.ItemID);
                cmdItem.Parameters.AddWithValue("@Qty", item.Quantity);
                cmdItem.Parameters.AddWithValue("@Rate", item.Rate ?? 0);
                cmdItem.ExecuteNonQuery();
            }

            tr.Commit();
            return "✅ PR created successfully.";
        }
        catch (Exception ex)
        {
            tr.Rollback();
            return $"❌ Failed to create PR: {ex.Message}";
        }
    }

}
