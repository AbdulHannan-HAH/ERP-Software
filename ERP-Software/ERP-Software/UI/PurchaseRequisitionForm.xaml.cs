using ERP_Software.BL;
using ERP_Software.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Software.UI
{
    public partial class PurchaseRequisitionForm : UserControl
    {
        private List<PRItem> prItems = new();

        public PurchaseRequisitionForm()

        {

            InitializeComponent();
            LoadVendors();
            LoadStores();
            LoadItems();
            dpPRDate.SelectedDate = DateTime.Now;
        }

        private void LoadVendors()
        {
            cmbVendors.ItemsSource = VendorBL.GetAllVendors();
        }

        private void LoadStores()
        {
            cmbStores.ItemsSource = StoreBL.GetAllStores();
        }

        private void LoadItems()
        {
            cmbItems.ItemsSource = ItemBL.GetAllItems();
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            if (cmbItems.SelectedValue == null || !int.TryParse(txtQty.Text, out int qty) || !decimal.TryParse(txtRate.Text, out decimal rate))
            {
                MessageBox.Show("❌ Invalid item, quantity or rate");
                return;
            }

            var item = (Item)cmbItems.SelectedItem;
            prItems.Add(new PRItem
            {
                ItemID = item.ItemID,
                ItemName = item.Name,
                Quantity = qty,
                Rate = rate
            });

            dgPRItems.ItemsSource = null;
            dgPRItems.ItemsSource = prItems;
            txtQty.Clear();
            txtRate.Clear();
        }

        private void btnSavePR_Click(object sender, RoutedEventArgs e)
        {
            if (cmbVendors.SelectedValue == null || cmbStores.SelectedValue == null || prItems.Count == 0)
            {
                MessageBox.Show("❌ Vendor, Store, and at least one item are required");
                return;
            }

            PurchaseRequisition pr = new PurchaseRequisition
            {
                VendorID = (int)cmbVendors.SelectedValue,
                StoreID = (int)cmbStores.SelectedValue,
                PRDate = dpPRDate.SelectedDate ?? DateTime.Now,
                Status = "Pending"
            };

            string result = PurchaseRequisitionBL.AddPR(pr, prItems);
            MessageBox.Show(result);
            if (result.StartsWith("✅"))
            {
                prItems.Clear();
                dgPRItems.ItemsSource = null;
            }
        }
    }
}
