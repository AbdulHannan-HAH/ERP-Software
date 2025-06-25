using ERP_Software.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.BL
{
    public class MaterialRequisitionBL
    {
        public static bool AddRequisition(MaterialRequisition mr)
        {
            return MaterialRequisitionDL.AddMR(mr);
        }
    }

}
