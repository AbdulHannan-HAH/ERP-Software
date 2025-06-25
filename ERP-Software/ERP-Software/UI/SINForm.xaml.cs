using ERP_Software.BL;
using ERP_Software.DL;
using ERP_Software.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Software.UI
{
    public partial class SINForm : UserControl
    {
        public SINForm()
        {
            try
            {
                InitializeComponent();
                LoadItems();
                LoadCostCenters();
                LoadSINs();
                dpDate.SelectedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading SINForm: " + ex.Message);
            }
        }
        private void LoadItems()
        {
            cmbItems.ItemsSource = ItemBL.GetAllItems();
        }

        private void LoadCostCenters()
        {
            cmbCostCenters.ItemsSource = CostCenterBL.GetAllCostCenters();
        }

        private void LoadSINs()
        {
            dgSINs.ItemsSource = StoreIssueNoteBL.GetAllSINs();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbItems.SelectedValue == null || cmbCostCenters.SelectedValue == null)
                {
                    MessageBox.Show("Please select item and cost center.");
                    return;
                }

                StoreIssueNote sin = new StoreIssueNote
                {
                    MRID = txtMRID.Text.Trim(), // ✅ MRID is string
                    ItemID = (int)cmbItems.SelectedValue,
                    IssuedQty = int.TryParse(txtQty.Text, out int qty) ? qty : 0,
                    CostCenterID = (int)cmbCostCenters.SelectedValue,
                    IssuedBy = txtIssuedBy.Text.Trim(),
                    Remarks = txtRemarks.Text.Trim(),
                    IssueDate = dpDate.SelectedDate ?? DateTime.Now // ✅ Now it's DateTime
                };

                string result = StoreIssueNoteBL.AddSIN(sin);

                if (result.StartsWith("✅"))
                {
                    int currentStock = ItemDL.GetCurrentStock(sin.ItemID);
                    if (currentStock < sin.IssuedQty)
                    {
                        MessageBox.Show("❌ Not enough stock available!");
                        return;
                    }

                    ItemDL.DecreaseItemStock(sin.ItemID, sin.IssuedQty);
                    MessageBox.Show("✅ SIN saved and stock updated!");
                    LoadSINs();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error: " + ex.Message);
            }
        }



        private void ClearForm()
        {
            txtMRID.Clear();
            txtQty.Clear();
            txtIssuedBy.Clear();
            txtRemarks.Clear();
            cmbItems.SelectedIndex = -1;
            cmbCostCenters.SelectedIndex = -1;
            dpDate.SelectedDate = DateTime.Now;
        }
    }
}
