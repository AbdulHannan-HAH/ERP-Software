using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.Models
{
    public  class Stores
    {
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public int COAAccountID { get; set; }
        public string COAAccountTitle { get; set; }
        public int? COAParentAccountID { get; set; } // This one is causing the error


    }
}
