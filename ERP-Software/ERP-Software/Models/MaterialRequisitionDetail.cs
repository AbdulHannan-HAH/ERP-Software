using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.Models
{
    public class MaterialRequisitionDetail
    {
        public int MRDetailID { get; set; }
        public int MRID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
    }

}
