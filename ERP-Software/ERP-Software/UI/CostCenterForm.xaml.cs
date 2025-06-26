using ERP_Software.BL;
using ERP_Software.Models;
using System.Windows;
using System;
using System.Windows.Controls;

namespace ERP_Software.UI
{
    public partial class CostCenterForm : UserControl
    {
        public CostCenterForm()
        {
            InitializeComponent();
            LoadCostCenters();
        }

        private void LoadCostCenters()
        {
            dgCostCenters.ItemsSource = CostCenterBL.GetAllCostCenters();
        }

        private void AddCostCenter_Click(object sender, RoutedEventArgs e)
        {
            var cc = new CostCenter
            {
                Name = txtName.Text.Trim(),
                Description = txtDesc.Text.Trim(),
                CreatedBy = "admin" 
            };

            if (CostCenterBL.AddCostCenter(cc))
            {
                MessageBox.Show("✅ Cost center added.");
                LoadCostCenters();
                txtName.Clear(); txtDesc.Clear();
            }
            else
            {
                MessageBox.Show("❌ Failed to add cost center.");
            }
        }
    }
}
