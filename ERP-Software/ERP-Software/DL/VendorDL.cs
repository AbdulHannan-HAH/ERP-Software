using ERP_Software.Models;
using ERP_Software.Models.ERP_Software.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;


namespace ERP_Software.DL
{
    public class VendorDL
    {
        private static string conStr = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

        public static List<Vendor> GetAllVendors()
        {
            List<Vendor> list = new List<Vendor>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string query = @"SELECT v.*, c.AccountTitle 
                                 FROM Vendors v
                                 JOIN ChartOfAccounts c ON v.COAAccountID = c.AccountID";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Vendor
                    {
                        VendorID = (int)reader["VendorID"],
                        VendorName = reader["VendorName"].ToString(),
                        Contact = reader["Contact"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        TaxNumber = reader["TaxNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        COAAccountID = (int)reader["COAAccountID"],
                        COAAccountTitle = reader["AccountTitle"].ToString()
                    });
                }
            }
            return list;
        }

        public static string AddVendor(Vendor v)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string query = @"INSERT INTO Vendors 
                (VendorName, Contact, Email, Phone, TaxNumber, Address, COAAccountID)
                VALUES (@Name, @Contact, @Email, @Phone, @Tax, @Address, @AccountID)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", v.VendorName);
                cmd.Parameters.AddWithValue("@Contact", v.Contact ?? "");
                cmd.Parameters.AddWithValue("@Email", v.Email ?? "");
                cmd.Parameters.AddWithValue("@Phone", v.Phone ?? "");
                cmd.Parameters.AddWithValue("@Tax", v.TaxNumber ?? "");
                cmd.Parameters.AddWithValue("@Address", v.Address ?? "");
                cmd.Parameters.AddWithValue("@AccountID", v.COAAccountID);

                int rows = cmd.ExecuteNonQuery();
                return rows > 0 ? "✅ Vendor added successfully" : "❌ Failed to add vendor";
            }
        }
    }
}
