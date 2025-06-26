using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ERP_Software.Models
{
    public class MaterialRequisition
    {
        public int MRID { get; set; }
        public DateTime MRDate { get; set; }
        public string RequestedBy { get; set; }
        public int CostCenterID { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public List<MaterialRequisitionDetail> Details { get; set; }
    }

}
