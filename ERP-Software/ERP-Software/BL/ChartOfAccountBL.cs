using ERP_Software.Models;
using ERP_Software.DL;
using System.Collections.Generic;

namespace ERP_Software.BL
{
    public class ChartOfAccountBL
    {
        public static List<ChartOfAccount> GetAllAccounts() => ChartOfAccountDL.GetAllAccounts();

        public static string AddAccount(ChartOfAccount acc)
        {
            if (string.IsNullOrWhiteSpace(acc.AccountCode))
                return "❌ Account Code is required.";
            if (string.IsNullOrWhiteSpace(acc.AccountTitle))
                return "❌ Account Title is required.";
            if (string.IsNullOrWhiteSpace(acc.AccountType))
                return "❌ Account Type is required.";

            return ChartOfAccountDL.AddAccount(acc);
        }
        public static List<ChartOfAccount> GetParentAccounts() => ChartOfAccountDL.GetParentAccounts();
        public static List<ChartOfAccount> GetChildAccounts(int parentId) => ChartOfAccountDL.GetChildAccounts(parentId);


    }
}
