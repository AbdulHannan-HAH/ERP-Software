using ERP_Software.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.BL
{
    public static class VendorItemBL
    {
        public static List<VendorItem> GetAllVendorItems() => VendorItemDL.GetAllVendorItems();

        public static string AddVendorItem(VendorItem vi)
        {
            if (vi.VendorID <= 0) return "❌ Vendor required";
            if (vi.ItemID <= 0) return "❌ Item required";
            if (vi.LastPrice <= 0) return "❌ Enter valid price";
            return VendorItemDL.AddVendorItem(vi);
        }
    }

}
