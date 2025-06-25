using ERP_Software.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_Software.Models;

namespace ERP_Software.BL
{
    public static class JournalEntryBL
    {
        public static string PostEntry(JournalEntry entry)
        {
            if (entry.Details == null || !entry.Details.Any())
                return "❌ Entry must have details.";

            decimal debit = entry.Details.Sum(d => d.Debit);
            decimal credit = entry.Details.Sum(d => d.Credit);
            if (debit != credit)
                return "❌ Debit and Credit must be equal.";

            entry.Amount = debit;
            return JournalEntryDL.AddJournalEntry(entry);
        }
        public static List<JournalEntryViewModel> GetFullLedger()
        {
            return JournalEntryDL.GetLedgerDetails();
        }

    }
    }
