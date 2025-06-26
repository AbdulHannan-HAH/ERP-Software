using ERP_Software.DL;
using ERP_Software.Models;
using System;
using System.Collections.Generic;

namespace ERP_Software.BL
{
    class StoreBL
    {
        public static string AddStore(Stores store)
        {
            if (string.IsNullOrWhiteSpace(store.StoreName))
                return "❌ Store name cannot be empty.";

            if (StoreDL.StoreExists(store.StoreName))
                return "❌ Store name already exists.";

            try
            {
                StoreDL.AddStore(store);
                return "✅ Store and COA account added successfully.";
            }
            catch (Exception ex)
            {
                return $"❌ Failed to add store.\n{ex.Message}";
            }
        }

        public static string UpdateStore(Stores store)
        {
            if (store.StoreID <= 0)
                return "❌ Invalid Store ID.";

            if (string.IsNullOrWhiteSpace(store.StoreName))
                return "❌ Store name cannot be empty.";

            try
            {
                StoreDL.UpdateStore(store);
                return "✅ Store and COA account updated successfully.";
            }
            catch (Exception ex)
            {
                return $"❌ Failed to update store.\n{ex.Message}";
            }
        }

        public static string DeleteStore(int storeId)
        {
            if (storeId <= 0)
                return "❌ Invalid Store ID.";

            try
            {
                StoreDL.DeleteStore(storeId);
                return "✅ Store and COA account deleted successfully.";
            }
            catch (Exception ex)
            {
                return $"❌ Failed to delete store.\n{ex.Message}";
            }
        }

        public static List<Stores> GetAllStores()
        {
            return StoreDL.GetAllStores();
        }
        public static List<Stores> GetAllStoresWithCOA()
        {
            return StoreDL.GetAllStoresWithCOA();
        }

    }
}
