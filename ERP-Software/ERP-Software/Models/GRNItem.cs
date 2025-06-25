using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.Models
{
    public class GRNItem
    {
        public int GRNItemID { get; set; }
        public int GRNID { get; set; }
        public int POID { get; set; } // 🔧 Add this line
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int QuantityReceived { get; set; }
        public decimal Rate { get; set; }
        public decimal Total => QuantityReceived * Rate;
    }

}
