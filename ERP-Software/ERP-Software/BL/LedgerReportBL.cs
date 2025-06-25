using ERP_Software.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.BL
{
    public static class LedgerReportBL
    {
        public static List<LedgerEntry> GetLedger(int accountId)
        {
            return LedgerReportDL.GetLedgerEntries(accountId);
        }
    }

}
