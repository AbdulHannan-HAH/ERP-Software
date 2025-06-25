using ERP_Software.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

public static class LedgerReportDL
{
    private static string cs = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

    public static List<LedgerEntry> GetLedgerEntries(int accountId)
    {
        var list = new List<LedgerEntry>();
        decimal balance = 0;

        using var con = new SqlConnection(cs);
        string q = @"
        SELECT je.EntryDate, je.Description, jed.Debit, jed.Credit
        FROM JournalEntryDetails jed
        JOIN JournalEntries je ON jed.EntryID = je.EntryID
        WHERE jed.AccountID = @id
        ORDER BY je.EntryDate";

        using var cmd = new SqlCommand(q, con);
        cmd.Parameters.AddWithValue("@id", accountId);
        con.Open();
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            decimal debit = Convert.ToDecimal(r["Debit"]);
            decimal credit = Convert.ToDecimal(r["Credit"]);
            balance += (debit - credit);

            list.Add(new LedgerEntry
            {
                EntryDate = (DateTime)r["EntryDate"],
                Description = r["Description"].ToString(),
                Debit = debit,
                Credit = credit,
                RunningBalance = balance
            });
        }

        return list;
    }
}
