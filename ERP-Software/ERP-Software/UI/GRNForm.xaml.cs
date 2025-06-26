using ERP_Software.BL;
using ERP_Software.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Software.UI
{
    public partial class GRNForm : UserControl
    {
        private List<GRNItem> grnItems = new();

        public GRNForm()
        {
            InitializeComponent();
            LoadApprovedPOs();
            dpGRNDate.SelectedDate = DateTime.Now;
        }

        private void LoadApprovedPOs()
        {
            cmbPOs.ItemsSource = PurchaseOrderBL.GetAllApprovedPOs();
        }

        private void cmbPOs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPOs.SelectedValue == null) return;

            int poid = (int)cmbPOs.SelectedValue;
            var poItems = PurchaseOrderBL.GetPOItems(poid);

            grnItems = poItems.Select(p => new GRNItem
            {
                POID = p.POID,
                ItemID = p.ItemID,
                ItemName = p.ItemName,
                QuantityReceived = p.Quantity, // default same as ordered
                Rate = p.Rate
            }).ToList();

            dgGRNItems.ItemsSource = null;
            dgGRNItems.ItemsSource = grnItems;
        }

        private void btnSaveGRN_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPOs.SelectedValue == null || grnItems.Count == 0)
            {
                MessageBox.Show("❌ Please select a PO and verify items.");
                return;
            }

            GRN grn = new GRN
            {
                POID = (int)cmbPOs.SelectedValue,
                GRNDate = dpGRNDate.SelectedDate ?? DateTime.Now,
                ReceivedBy = txtReceivedBy.Text.Trim(),
                Remarks = txtRemarks.Text.Trim(),
                Status = "Received",
                Items = grnItems
            };

            string result = GRNBL.AddGRN(grn);
            MessageBox.Show(result);

            if (result.StartsWith("✅"))
            {
                cmbPOs.SelectedIndex = -1;
                dgGRNItems.ItemsSource = null;
                grnItems.Clear();
                txtReceivedBy.Clear();
                txtRemarks.Clear();
                dpGRNDate.SelectedDate = DateTime.Now;
            }
        }

        private void txtReceivedBy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
