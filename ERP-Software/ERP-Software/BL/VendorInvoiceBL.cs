using ERP_Software.DL;
using ERP_Software.Models;

public static class VendorInvoiceBL
{
    public static string AddInvoiceWithJournal(VendorInvoice invoice, string description)
    {
        return VendorInvoiceDL.AddInvoiceWithJournal(invoice, description);
    }
}
