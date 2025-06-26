using ERP_Software.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

public class MaterialRequisitionDL
{
    private static string conStr = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

    public static bool AddMR(MaterialRequisition mr)
    {
        using (SqlConnection con = new SqlConnection(conStr))
        {
            con.Open();
            SqlTransaction tr = con.BeginTransaction();
            try
            {
                // Insert MR Header
                string query = @"INSERT INTO MaterialRequisition (MRDate, RequestedBy, CostCenterID, Status, Remarks)
                                 VALUES (@date, @by, @ccid, @status, @remarks);
                                 SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(query, con, tr);
                cmd.Parameters.AddWithValue("@date", mr.MRDate);
                cmd.Parameters.AddWithValue("@by", mr.RequestedBy);
                cmd.Parameters.AddWithValue("@ccid", mr.CostCenterID);
                cmd.Parameters.AddWithValue("@status", mr.Status);
                cmd.Parameters.AddWithValue("@remarks", mr.Remarks);
                int mrid = Convert.ToInt32(cmd.ExecuteScalar());

                // Insert Details
                foreach (var item in mr.Details)
                {
                    SqlCommand detailCmd = new SqlCommand(@"INSERT INTO MaterialRequisitionDetails (MRID, ItemID, Quantity)
                                                            VALUES (@mrid, @itemid, @qty)", con, tr);
                    detailCmd.Parameters.AddWithValue("@mrid", mrid);
                    detailCmd.Parameters.AddWithValue("@itemid", item.ItemID);
                    detailCmd.Parameters.AddWithValue("@qty", item.Quantity);
                    detailCmd.ExecuteNonQuery();
                }

                tr.Commit();
                return true;
            }
            catch
            {
                tr.Rollback();
                return false;
            }
        }
    }
}
