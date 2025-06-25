using ERP_Software.DL;
using ERP_Software.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.BL
{
    public static class GRNBL
    {
        public static string AddGRN(GRN grn)
        {
            if (grn.Items == null || grn.Items.Count == 0)
                return "❌ At least one item required.";

            return GRNDL.AddGRN(grn);
        }
        public static List<GRN> GetGRNsWithoutInvoice()
        {
            return GRNDL.GetGRNsWithoutInvoice();
        }
        public static decimal GetGRNTotalAmount(int grnId)
        {
            return GRNDL.GetGRNTotalAmount(grnId);
        }

    }
}
