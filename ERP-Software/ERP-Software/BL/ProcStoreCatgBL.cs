using ERP_Software.DL;
using ERP_Software.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.BL
{
    internal class ProcStoreCatgBL
    {
        public static string AddStore(string storeName)
        {
            if (string.IsNullOrWhiteSpace(storeName))
                return "❌ Store name cannot be empty.";

            if (ProcStoreCatgDL.StoreExists(storeName))
                return "❌ Store name already exists.";

            ProcStoreCatgDL.InsertStoreName(storeName);
            return "✅ Store added successfully.";
        }

        public static List<ProcStoreCatg> GetStoreNames()
        {
            return ProcStoreCatgDL.GetAllStoreNames();
        }

        public static string UpdateStoreName(int storeId, string storeName)
        {
            if (storeId <= 0)
                return "❌ Invalid Store ID.";

            if (string.IsNullOrWhiteSpace(storeName))
                return "❌ New store name cannot be empty.";

            if (ProcStoreCatgDL.StoreExists(storeName))
                return "❌ This store name already exists.";

            ProcStoreCatgDL.UpdateStoreName(storeId, storeName);
            return "✅ Store updated successfully.";
        }

        public static string DeleteStore(int storeId)
        {
            if (storeId <= 0)
                return "❌ Invalid Store ID.";

            ProcStoreCatgDL.DeleteStore(storeId);
            return "✅ Store deleted successfully.";
        }
        public static string GetStoreNameById(int storeId)
        {
            var stores = ProcStoreCatgDL.GetAllStoreNames();
            var store = stores.FirstOrDefault(r => r.storeid == storeId);
            return store?.storename ?? "Unknown";
        }
    }
}
