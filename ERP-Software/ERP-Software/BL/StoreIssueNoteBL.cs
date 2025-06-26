using System.Collections.Generic;
using ERP_Software.Models;
using ERP_Software.DL;

namespace ERP_Software.BL
{
    public class StoreIssueNoteBL
    {
        public static string AddSIN(StoreIssueNote sin) => StoreIssueNoteDL.AddSIN(sin);
        public static List<StoreIssueNote> GetAllSINs() => StoreIssueNoteDL.GetAllSINs();
    }
}
