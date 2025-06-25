using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.Models
{
    public class GRN
    {
        public int GRNID { get; set; }
        public int POID { get; set; }
        public DateTime GRNDate { get; set; }
        public string ReceivedBy { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; } // e.g. Received, Pending
        public List<GRNItem> Items { get; set; }
        public int VendorID { get; set; }

    }
}
