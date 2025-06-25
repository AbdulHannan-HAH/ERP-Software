using ERP_Software.DL;
using ERP_Software.Models;
using System.Collections.Generic;

namespace ERP_Software.BL
{
    public class StoreClassBL
    {
        public static string AddClass(StoreClass c)
        {
            if (string.IsNullOrWhiteSpace(c.ClassName))
                return "❌ Class name cannot be empty.";
            if (c.StoreTypeID <= 0)
                return "❌ Please select a valid store type.";

            StoreClassDL.AddClass(c);
            return "✅ Class added successfully.";
        }

        public static List<StoreClass> GetAllClasses()
        {
            return StoreClassDL.GetAllClasses();
        }

        public static string DeleteClass(int id)
        {
            if (id <= 0)
                return "❌ Invalid Class ID.";

            StoreClassDL.DeleteClass(id);
            return "✅ Class deleted successfully.";
        }
    }
}
