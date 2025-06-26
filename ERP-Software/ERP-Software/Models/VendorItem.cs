using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.Models
{
    public class VendorItem
    {
        public int VendorItemID { get; set; }
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal LastPrice { get; set; }
        public int LeadTimeDays { get; set; }
        public bool IsActive { get; set; }
    }

}
