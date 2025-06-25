namespace ERP_Software.Models
{
    public class StoreClass
    {
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public int StoreTypeID { get; set; }

        // Extra property for displaying store type name (optional)
        public string StoreTypeName { get; set; }
    }
}

