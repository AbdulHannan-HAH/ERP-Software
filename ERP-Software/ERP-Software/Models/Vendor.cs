using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.Models
{
    namespace ERP_Software.Models
    {
        public class Vendor
        {
            public int VendorID { get; set; }
            public string VendorName { get; set; }
            public string Contact { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string TaxNumber { get; set; }
            public string Address { get; set; }
            public int COAAccountID { get; set; }
            public string COAAccountTitle { get; set; } // For display only
        }
    }

}
