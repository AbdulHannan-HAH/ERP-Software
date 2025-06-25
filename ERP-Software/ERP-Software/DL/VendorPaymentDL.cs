using ERP_Software.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ERP_Software.DL
{
    public static class VendorPaymentDL
    {
        private static string cs = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

        public static string AddPaymentWithJournal(VendorPayment payment, string description)
        {
            using var con = new SqlConnection(cs);
            con.Open();
            var tran = con.BeginTransaction();

            try
            {
                // Insert payment
                string insertPayment = @"INSERT INTO VendorPayments (VendorID, InvoiceID, PaymentDate, AmountPaid, PaymentMethod, Remarks)
                                         OUTPUT INSERTED.PaymentID
                                         VALUES (@vendor, @invoice, @date, @amount, @method, @remarks)";
                SqlCommand cmd = new(insertPayment, con, tran);
                cmd.Parameters.AddWithValue("@vendor", payment.VendorID);
                cmd.Parameters.AddWithValue("@invoice", payment.InvoiceID);
                cmd.Parameters.AddWithValue("@date", payment.PaymentDate);
                cmd.Parameters.AddWithValue("@amount", payment.AmountPaid);
                cmd.Parameters.AddWithValue("@method", payment.PaymentMethod);
                cmd.Parameters.AddWithValue("@remarks", payment.Remarks ?? "");
                int paymentId = (int)cmd.ExecuteScalar();

                // Insert journal entry
                string journalEntry = @"INSERT INTO JournalEntries (EntryDate, Description, Amount)
                                        OUTPUT INSERTED.EntryID
                                        VALUES (@date, @desc, @amount)";
                SqlCommand cmd2 = new(journalEntry, con, tran);
                cmd2.Parameters.AddWithValue("@date", payment.PaymentDate);
                cmd2.Parameters.AddWithValue("@desc", description);
                cmd2.Parameters.AddWithValue("@amount", payment.AmountPaid);
                int entryId = (int)cmd2.ExecuteScalar();

                // Accounts Payable (debit)
                SqlCommand debit = new("INSERT INTO JournalEntryDetails (EntryID, AccountID, Debit) VALUES (@eid, 201, @amt)", con, tran);
                debit.Parameters.AddWithValue("@eid", entryId);
                debit.Parameters.AddWithValue("@amt", payment.AmountPaid);
                debit.ExecuteNonQuery();

                // Bank or Cash (credit)
                SqlCommand credit = new("INSERT INTO JournalEntryDetails (EntryID, AccountID, Credit) VALUES (@eid, 101, @amt)", con, tran);
                credit.Parameters.AddWithValue("@eid", entryId);
                credit.Parameters.AddWithValue("@amt", payment.AmountPaid);
                credit.ExecuteNonQuery();

                // Update invoice to paid
                string updateInvoice = "UPDATE VendorInvoices SET Status='Paid' WHERE InvoiceID=@id";
                SqlCommand cmd3 = new(updateInvoice, con, tran);
                cmd3.Parameters.AddWithValue("@id", payment.InvoiceID);
                cmd3.ExecuteNonQuery();

                tran.Commit();
                return "✅ Payment recorded and journal entry created.";
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return "❌ Error: " + ex.Message;
            }
        }

        public static List<VendorInvoice> GetUnpaidInvoicesByVendor(int vendorId)
        {
            List<VendorInvoice> list = new();
            using var con = new SqlConnection(cs);
            string q = @"SELECT InvoiceID, InvoiceNumber, TotalAmount 
                         FROM VendorInvoices WHERE VendorID=@vid AND Status='Unpaid'";
            SqlCommand cmd = new(q, con);
            cmd.Parameters.AddWithValue("@vid", vendorId);
            con.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new VendorInvoice
                {
                    InvoiceID = (int)r["InvoiceID"],
                    InvoiceNumber = r["InvoiceNumber"].ToString(),
                    TotalAmount = (decimal)r["TotalAmount"]
                });
            }
            return list;
        }
    }
}
