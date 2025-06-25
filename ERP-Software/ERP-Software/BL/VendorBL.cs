using ERP_Software.DL;
using ERP_Software.Models;
using ERP_Software.Models.ERP_Software.Models;
using System.Collections.Generic;

namespace ERP_Software.BL
{
    public class VendorBL
    {
        public static List<Vendor> GetAllVendors() => VendorDL.GetAllVendors();

        public static string AddVendor(Vendor v)
        {
            if (string.IsNullOrWhiteSpace(v.VendorName)) return "❌ Vendor name required";
            if (v.COAAccountID <= 0) return "❌ COA account required";
            return VendorDL.AddVendor(v);
        }
    }
}
