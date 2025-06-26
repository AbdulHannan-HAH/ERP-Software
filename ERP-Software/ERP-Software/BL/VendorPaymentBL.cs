using ERP_Software.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_Software.DL;

namespace ERP_Software.BL
{
    public static class VendorPaymentBL
    {
        public static string AddPaymentWithJournal(VendorPayment payment, string description)
        {
            return VendorPaymentDL.AddPaymentWithJournal(payment, description);
        }

        public static List<VendorInvoice> GetUnpaidInvoicesByVendor(int vendorId)
        {
            return VendorPaymentDL.GetUnpaidInvoicesByVendor(vendorId);
        }
    }
}
