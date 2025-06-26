using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.Models
{
    public class Item
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int MinLevel { get; set; }

        public int StoreID { get; set; }
        public string StoreType { get; set; } // optional for DataGrid display

        public int ClassID { get; set; }
        public int QuantityInStock { get; set; }

        public string ClassName { get; set; } // optional for DataGrid display
    }
}
