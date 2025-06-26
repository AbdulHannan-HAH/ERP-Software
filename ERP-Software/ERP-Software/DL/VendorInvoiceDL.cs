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
    public static class VendorInvoiceDL
    {
        private static string cs = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

        public static string AddInvoiceWithJournal(VendorInvoice invoice, string description)
        {
            using var con = new SqlConnection(cs);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                // Insert into VendorInvoices
                string q1 = @"INSERT INTO VendorInvoices (GRNID, VendorID, InvoiceNumber, InvoiceDate, TotalAmount, Status)
                      OUTPUT INSERTED.InvoiceID
                      VALUES (@grn, @vendor, @num, @date, @amount, @status)";
                SqlCommand cmd1 = new(q1, con, tran);
                cmd1.Parameters.AddWithValue("@grn", invoice.GRNID);
                cmd1.Parameters.AddWithValue("@vendor", invoice.VendorID);
                cmd1.Parameters.AddWithValue("@num", invoice.InvoiceNumber);
                cmd1.Parameters.AddWithValue("@date", invoice.InvoiceDate);
                cmd1.Parameters.AddWithValue("@amount", invoice.TotalAmount);
                cmd1.Parameters.AddWithValue("@status", invoice.Status);
                int invoiceId = (int)cmd1.ExecuteScalar();

                // Insert into JournalEntries
                string q2 = @"INSERT INTO JournalEntries (EntryDate, Description, Amount)
                      OUTPUT INSERTED.EntryID
                      VALUES (@date, @desc, @amount)";
                SqlCommand cmd2 = new(q2, con, tran);
                cmd2.Parameters.AddWithValue("@date", invoice.InvoiceDate);
                cmd2.Parameters.AddWithValue("@desc", description);
                cmd2.Parameters.AddWithValue("@amount", invoice.TotalAmount);
                int entryId = (int)cmd2.ExecuteScalar();

                // Fetch COA accounts
                int inventoryAccountId = 1; // example: 'Inventory'
                int vendorAccountId = GetVendorCOAAccountID(invoice.VendorID, con, tran);

                // Debit Inventory
                string q3 = @"INSERT INTO JournalEntryDetails (EntryID, AccountID, Debit, Credit)
                      VALUES (@eid, @aid, @debit, 0)";
                SqlCommand cmd3 = new(q3, con, tran);
                cmd3.Parameters.AddWithValue("@eid", entryId);
                cmd3.Parameters.AddWithValue("@aid", inventoryAccountId);
                cmd3.Parameters.AddWithValue("@debit", invoice.TotalAmount);
                cmd3.ExecuteNonQuery();

                // Credit Vendor
                string q4 = @"INSERT INTO JournalEntryDetails (EntryID, AccountID, Debit, Credit)
                      VALUES (@eid, @aid, 0, @credit)";
                SqlCommand cmd4 = new(q4, con, tran);
                cmd4.Parameters.AddWithValue("@eid", entryId);
                cmd4.Parameters.AddWithValue("@aid", vendorAccountId);
                cmd4.Parameters.AddWithValue("@credit", invoice.TotalAmount);
                cmd4.ExecuteNonQuery();

                tran.Commit();
                return "✅ Invoice and journal entry posted.";
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return "❌ Error: " + ex.Message;
            }
        }

        private static int GetVendorCOAAccountID(int vendorId, SqlConnection con, SqlTransaction tran)
        {
            string q = "SELECT COAAccountID FROM Vendors WHERE VendorID = @id";
            SqlCommand cmd = new(q, con, tran);
            cmd.Parameters.AddWithValue("@id", vendorId);
            return (int)cmd.ExecuteScalar();
        }

    }
}
