using ERP_Software.BL;
using ERP_Software.Models;
using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace ERP_Software.UI
{
    public partial class VendorItemForm : UserControl
    {
        public VendorItemForm()
        {
            try
            {
                InitializeComponent();
                LoadVendors();
                LoadItems();
                LoadVendorItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading SINForm: " + ex.Message);
            }
        }

        private void LoadVendors()
        {
            cmbVendors.ItemsSource = VendorBL.GetAllVendors();
        }

        private void LoadItems()
        {
            cmbItems.ItemsSource = ItemBL.GetAllItems();
        }

        private void LoadVendorItems()
        {
            dgVendorItems.ItemsSource = VendorItemBL.GetAllVendorItems();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbVendors.SelectedValue == null || cmbItems.SelectedValue == null)
            {
                MessageBox.Show("❌ Please select both a vendor and an item.");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price))
            {
                MessageBox.Show("❌ Invalid price entered.");
                return;
            }

            if (!int.TryParse(txtLeadTime.Text.Trim(), out int leadTime))
            {
                MessageBox.Show("❌ Invalid lead time entered.");
                return;
            }

            VendorItem vi = new VendorItem
            {
                VendorID = (int)cmbVendors.SelectedValue,
                ItemID = (int)cmbItems.SelectedValue,
                LastPrice = price,
                LeadTimeDays = leadTime,
                IsActive = chkActive.IsChecked == true
            };

            string result = VendorItemBL.AddVendorItem(vi);
            MessageBox.Show(result);

            if (result.StartsWith("✅"))
            {
                ClearForm();
                LoadVendorItems();
            }
        }

        private void ClearForm()
        {
            cmbVendors.SelectedIndex = -1;
            cmbItems.SelectedIndex = -1;
            txtPrice.Clear();
            txtLeadTime.Clear();
            chkActive.IsChecked = false;
        }
    }
}
