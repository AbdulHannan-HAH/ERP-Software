using ERP_Software.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ERP_Software.DL
{
    public static class GRNDL
    {
        private static string cs = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

        public static string AddGRN(GRN grn)
        {
            using var con = new SqlConnection(cs);
            con.Open();

            SqlTransaction tran = con.BeginTransaction();
            try
            {
                // Insert into GRN master table
                string q1 = @"INSERT INTO GoodsReceiptNotes (POID, GRNDate, ReceivedBy, Remarks, Status)
                          OUTPUT INSERTED.GRNID
                          VALUES (@POID, @Date, @RecBy, @Remarks, @Status)";
                SqlCommand cmd1 = new(q1, con, tran);
                cmd1.Parameters.AddWithValue("@POID", grn.POID);
                cmd1.Parameters.AddWithValue("@Date", grn.GRNDate);
                cmd1.Parameters.AddWithValue("@RecBy", grn.ReceivedBy);
                cmd1.Parameters.AddWithValue("@Remarks", grn.Remarks);
                cmd1.Parameters.AddWithValue("@Status", grn.Status);
                int grnId = (int)cmd1.ExecuteScalar();

                // Insert each GRN item
                foreach (var item in grn.Items)
                {
                    string q2 = @"INSERT INTO GRNItems (GRNID, ItemID, QuantityReceived, Rate)
                              VALUES (@GRNID, @ItemID, @Qty, @Rate)";
                    SqlCommand cmd2 = new(q2, con, tran);
                    cmd2.Parameters.AddWithValue("@GRNID", grnId);
                    cmd2.Parameters.AddWithValue("@ItemID", item.ItemID);
                    cmd2.Parameters.AddWithValue("@Qty", item.QuantityReceived);
                    cmd2.Parameters.AddWithValue("@Rate", item.Rate);
                    cmd2.ExecuteNonQuery();

                    // Update stock
                    string q3 = "UPDATE Items SET QuantityInStock = QuantityInStock + @Qty WHERE ItemID = @ItemID";
                    SqlCommand cmd3 = new(q3, con, tran);
                    cmd3.Parameters.AddWithValue("@Qty", item.QuantityReceived);
                    cmd3.Parameters.AddWithValue("@ItemID", item.ItemID);
                    cmd3.ExecuteNonQuery();
                }

                tran.Commit();
                return "✅ GRN saved and stock updated.";
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return "❌ Error: " + ex.Message;
            }
        }
        public static List<GRN> GetGRNsWithoutInvoice()
        {
            List<GRN> list = new();
            using var con = new SqlConnection(cs);
            string q = @"SELECT g.GRNID, g.POID, g.GRNDate, g.ReceivedBy, g.Remarks, g.Status, p.VendorID
                 FROM GoodsReceiptNotes g
                 JOIN PurchaseOrders p ON g.POID = p.POID
                 WHERE g.GRNID NOT IN (SELECT GRNID FROM VendorInvoices)";
            using var cmd = new SqlCommand(q, con);
            con.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new GRN
                {
                    GRNID = (int)r["GRNID"],
                    POID = (int)r["POID"],
                    GRNDate = (DateTime)r["GRNDate"],
                    ReceivedBy = r["ReceivedBy"].ToString(),
                    Remarks = r["Remarks"].ToString(),
                    Status = r["Status"].ToString(),
                    VendorID = (int)r["VendorID"]  // ✅ Set the vendor ID
                });
            }
            return list;
        }

        public static decimal GetGRNTotalAmount(int grnId)
        {
            using var con = new SqlConnection(cs);
            string q = "SELECT SUM(QuantityReceived * Rate) FROM GRNItems WHERE GRNID = @id";
            using var cmd = new SqlCommand(q, con);
            cmd.Parameters.AddWithValue("@id", grnId);
            con.Open();
            var result = cmd.ExecuteScalar();
            return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
        }

    }
}
