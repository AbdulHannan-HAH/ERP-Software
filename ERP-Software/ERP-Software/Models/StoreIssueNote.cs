namespace ERP_Software.Models
{
    public class StoreIssueNote
    {
        public int SINID { get; set; }

        // ✅ Change from int to string

        public int ItemID { get; set; }
        public int IssuedQty { get; set; }
        public int CostCenterID { get; set; }
        public string IssuedBy { get; set; }
        public string Remarks { get; set; }


        // Optional Display-only Fields
        public string ItemName { get; set; }
        public string CostCenterName { get; set; }
        public string MRID { get; set; } // Assuming MRID is entered as text
        public DateTime IssueDate { get; set; } // Use DateTime for correct data handling


    }

}