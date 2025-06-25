using ERP_Software.DL;
using ERP_Software.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.BL
{
    public static class PurchaseOrderBL
    {
        public static string AddPO(PurchaseOrder po, List<POItem> items)
            => PurchaseOrderDL.AddPO(po, items);

        public static List<PurchaseOrder> GetAllPOs()
            => PurchaseOrderDL.GetAllPOs();


        public static List<PurchaseOrder> GetAllApprovedPOs()
        {
            return PurchaseOrderDL.GetApprovedPOs();
        }

        public static List<POItem> GetPOItems(int poid)
        {
            return PurchaseOrderDL.GetPOItems(poid);
        }
    }
}
