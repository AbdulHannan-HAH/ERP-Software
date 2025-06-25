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
    public static class PurchaseOrderDL
    {
        private static string cs = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

        public static string AddPO(PurchaseOrder po, List<POItem> items)
        {
            using var con = new SqlConnection(cs);
            con.Open();
            using var tran = con.BeginTransaction();

            try
            {
                string insertPO = @"INSERT INTO PurchaseOrders (PRID, VendorID, StoreID, PODate, Status)
                                OUTPUT INSERTED.POID
                                VALUES (@prid, @vendor, @store, @date, @status)";
                using var cmd = new SqlCommand(insertPO, con, tran);
                cmd.Parameters.AddWithValue("@prid", po.PRID);
                cmd.Parameters.AddWithValue("@vendor", po.VendorID);
                cmd.Parameters.AddWithValue("@store", po.StoreID);
                cmd.Parameters.AddWithValue("@date", po.PODate);
                cmd.Parameters.AddWithValue("@status", po.Status);
                int poid = (int)cmd.ExecuteScalar();

                foreach (var item in items)
                {
                    string insertItem = @"INSERT INTO PurchaseOrderItems (POID, ItemID, Quantity, Rate)
                                      VALUES (@poid, @item, @qty, @rate)";
                    using var cmdItem = new SqlCommand(insertItem, con, tran);
                    cmdItem.Parameters.AddWithValue("@poid", poid);
                    cmdItem.Parameters.AddWithValue("@item", item.ItemID);
                    cmdItem.Parameters.AddWithValue("@qty", item.Quantity);
                    cmdItem.Parameters.AddWithValue("@rate", item.Rate);
                    cmdItem.ExecuteNonQuery();
                }

                tran.Commit();
                return "✅ PO added successfully";
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return $"❌ Error: {ex.Message}";
            }
        }

        public static List<PurchaseOrder> GetAllPOs()
        {
            var list = new List<PurchaseOrder>();
            using var con = new SqlConnection(cs);
            string query = "SELECT * FROM PurchaseOrders";
            using var cmd = new SqlCommand(query, con);
            con.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new PurchaseOrder
                {
                    POID = (int)r["POID"],
                    PRID = (int)r["PRID"],
                    VendorID = (int)r["VendorID"],
                    StoreID = (int)r["StoreID"],
                    PODate = (DateTime)r["PODate"],
                    Status = r["Status"].ToString()
                });
            }
            return list;
        }
        public static List<PurchaseOrder> GetApprovedPOs()
        {
            var list = new List<PurchaseOrder>();
            using var con = new SqlConnection(cs);
            string q = "SELECT POID, VendorID, StoreID, PODate FROM PurchaseOrders WHERE Status = 'Approved'";
            using var cmd = new SqlCommand(q, con);
            con.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new PurchaseOrder
                {
                    POID = (int)r["POID"],
                    VendorID = (int)r["VendorID"],
                    StoreID = (int)r["StoreID"],
                    PODate = (DateTime)r["PODate"]
                });
            }
            return list;
        }

        public static List<POItem> GetPOItems(int poid)
        {
            var list = new List<POItem>();
            using var con = new SqlConnection(cs);
            string q = @"SELECT poi.POItemID, poi.POID, poi.ItemID, i.Name AS ItemName, poi.Quantity, poi.Rate
                     FROM PurchaseOrderItems poi
                     JOIN Items i ON poi.ItemID = i.ItemID
                     WHERE poi.POID = @poid";
            using var cmd = new SqlCommand(q, con);
            cmd.Parameters.AddWithValue("@poid", poid);
            con.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new POItem
                {
                    POItemID = (int)r["POItemID"],
                    POID = (int)r["POID"],
                    ItemID = (int)r["ItemID"],
                    ItemName = r["ItemName"].ToString(),
                    Quantity = (int)r["Quantity"],
                    Rate = (decimal)r["Rate"]
                });
            }
            return list;
        }
    }

}
