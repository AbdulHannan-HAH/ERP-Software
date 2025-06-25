using System.Collections.Generic;
using ERP_Software.Models;
using ERP_Software.DL;

namespace ERP_Software.BL
{
    public class CostCenterBL
    {
        public static List<CostCenter> GetAllCostCenters()
        {
            return CostCenterDL.GetAll();
        }

        public static bool AddCostCenter(CostCenter cc)
        {
            return CostCenterDL.Add(cc);
        }

        public static bool DeleteCostCenter(int id)
        {
            return CostCenterDL.Delete(id);
        }
    }
}
