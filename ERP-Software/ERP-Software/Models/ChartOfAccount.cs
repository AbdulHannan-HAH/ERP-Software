using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.Models
{
    public class ChartOfAccount
    {
        public int AccountID { get; set; }
        public string AccountCode { get; set; }
        public string AccountTitle { get; set; }
        public int? ParentAccountID { get; set; }
        public string AccountType { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        // For display only
        public string ParentAccountTitle { get; set; }
    }

}
