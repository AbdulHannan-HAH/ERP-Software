using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ERP_Software.Models;

namespace ERP_Software.DL
{
    public static class JournalEntryDL
    {
        private static string cs = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

        public static string AddJournalEntry(JournalEntry entry)
        {
            using var con = new SqlConnection(cs);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                string q1 = @"INSERT INTO JournalEntries (EntryDate, Description, Amount)
                              OUTPUT INSERTED.EntryID
                              VALUES (@date, @desc, @amt)";
                using var cmd1 = new SqlCommand(q1, con, tran);
                cmd1.Parameters.AddWithValue("@date", entry.EntryDate);
                cmd1.Parameters.AddWithValue("@desc", entry.Description ?? "");
                cmd1.Parameters.AddWithValue("@amt", entry.Amount);
                int entryId = (int)cmd1.ExecuteScalar();

                foreach (var d in entry.Details)
                {
                    string q2 = @"INSERT INTO JournalEntryDetails (EntryID, AccountID, Debit, Credit)
                                  VALUES (@eid, @aid, @debit, @credit)";
                    using var cmd2 = new SqlCommand(q2, con, tran);
                    cmd2.Parameters.AddWithValue("@eid", entryId);
                    cmd2.Parameters.AddWithValue("@aid", d.AccountID);
                    cmd2.Parameters.AddWithValue("@debit", d.Debit);
                    cmd2.Parameters.AddWithValue("@credit", d.Credit);
                    cmd2.ExecuteNonQuery();
                }

                tran.Commit();
                return "✅ Journal entry posted successfully.";
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return "❌ Error: " + ex.Message;
            }
        }
        public static List<JournalEntryViewModel> GetLedgerDetails()
        {
            var list = new List<JournalEntryViewModel>();
            using var con = new SqlConnection(cs);
            string q = @"SELECT je.EntryDate, je.Description, coa.AccountTitle,
                                jed.Debit, jed.Credit
                         FROM JournalEntries je
                         JOIN JournalEntryDetails jed ON je.EntryID = jed.EntryID
                         JOIN ChartOfAccounts coa ON jed.AccountID = coa.AccountID
                         ORDER BY je.EntryDate DESC";
            using var cmd = new SqlCommand(q, con);
            con.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new JournalEntryViewModel
                {
                    EntryDate = Convert.ToDateTime(r["EntryDate"]).ToString("yyyy-MM-dd"),
                    Description = r["Description"].ToString(),
                    AccountTitle = r["AccountTitle"].ToString(),
                    Debit = Convert.ToDecimal(r["Debit"]),
                    Credit = Convert.ToDecimal(r["Credit"])
                });
            }
            return list;
        }
    }
}
