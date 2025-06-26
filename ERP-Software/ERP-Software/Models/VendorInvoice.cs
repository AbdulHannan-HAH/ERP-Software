using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.Models
{
    public class VendorInvoice
    {
        public int InvoiceID { get; set; }
        public int GRNID { get; set; }
        public int VendorID { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // Unpaid, Paid
    }

}
