using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.Models
{
    public class PurchaseOrder
    {
        public int POID { get; set; }
        public int PRID { get; set; }
        public int VendorID { get; set; }
        public int StoreID { get; set; }
        public DateTime PODate { get; set; }
        public string Status { get; set; }
    }
}
