using ERP_Software.Models;

public static class PurchaseRequisitionBL
{
    public static List<PurchaseRequisition> GetPRs(int vendorId, int storeId)
        => PurchaseRequisitionDL.GetPRs(vendorId, storeId);

    public static List<PRItem> GetPRItems(int prid)
        => PurchaseRequisitionDL.GetPRItems(prid);
    public static string AddPR(PurchaseRequisition pr, List<PRItem> items)
       => PurchaseRequisitionDL.AddPR(pr, items);
}
