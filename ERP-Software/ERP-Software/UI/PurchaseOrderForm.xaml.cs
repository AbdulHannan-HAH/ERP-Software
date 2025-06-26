using ERP_Software.BL;
using ERP_Software.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Software.UI
{
    public partial class PurchaseOrderForm : UserControl
    {
        private List<PRItem> prItems = new();

        public PurchaseOrderForm()
        {
            InitializeComponent();
            LoadStores();
            LoadVendors();
            dpDate.SelectedDate = DateTime.Now;
        }

        private void LoadStores()
        {
            cmbStores.ItemsSource = StoreBL.GetAllStores();
        }

        private void LoadVendors()
        {
            cmbVendors.ItemsSource = VendorBL.GetAllVendors();
        }

        private void cmbStores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbStores.SelectedValue != null && cmbVendors.SelectedValue != null)
            {
                int storeId = (int)cmbStores.SelectedValue;
                int vendorId = (int)cmbVendors.SelectedValue;
                cmbPRs.ItemsSource = PurchaseRequisitionBL.GetPRs(vendorId, storeId);
            }
        }

        private void cmbPRs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPRs.SelectedValue != null)
            {
                int prid = (int)cmbPRs.SelectedValue;
                prItems = PurchaseRequisitionBL.GetPRItems(prid);
                dgItems.ItemsSource = null;
                dgItems.ItemsSource = prItems;
            }
        }

        private void btnSavePO_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStores.SelectedValue == null || cmbVendors.SelectedValue == null || cmbPRs.SelectedValue == null || prItems.Count == 0)
            {
                MessageBox.Show("❌ Store, Vendor, PR and Items are required");
                return;
            }

            PurchaseOrder po = new PurchaseOrder
            {
                StoreID = (int)cmbStores.SelectedValue,
                VendorID = (int)cmbVendors.SelectedValue,
                PRID = (int)cmbPRs.SelectedValue,
                PODate = dpDate.SelectedDate ?? DateTime.Now,
                Status = "Open"
            };

            // ✅ Convert PRItem to POItem
            List<POItem> poItems = new List<POItem>();
            foreach (var prItem in prItems)
            {
                poItems.Add(new POItem
                {
                    ItemID = prItem.ItemID,
                    Quantity = prItem.Quantity,
                    Rate = prItem.Rate ?? 0
                });
            }

            string result = PurchaseOrderBL.AddPO(po, poItems);
            MessageBox.Show(result);
            if (result.StartsWith("✅"))
            {
                dgItems.ItemsSource = null;
                cmbPRs.SelectedIndex = -1;
            }
        }

    }
}
