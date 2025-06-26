using ERP_Software.DL;
using ERP_Software.Models;
using System;
using System.Collections.Generic;

namespace ERP_Software.BL
{
    public class ItemBL
    {
        public static List<Item> GetAllItems() => ItemDL.GetAllItems();

        public static string AddItem(Item item)
        {
            if (string.IsNullOrWhiteSpace(item.Name)) return "❌ Name is required";
            if (item.StoreID <= 0) return "❌ Select a valid store";
            if (item.ClassID <= 0) return "❌ Select a valid class";
            if (item.MinLevel < 0) return "❌ Min level must be >= 0";
            return ItemDL.AddItem(item);
        }

        public static string DeleteItem(int id) => ItemDL.DeleteItem(id);

        //public static bool IsStockAvailable(int itemId, int requestedQty)
        //{
        //    var item = ItemDL.GetItemById(itemId);
        //    return item != null && item.QuantityInStock >= requestedQty;
        //}


    }
}
