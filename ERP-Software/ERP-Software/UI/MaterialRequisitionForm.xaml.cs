using ERP_Software.BL;
using ERP_Software.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ERP_Software.UI
{
    /// <summary>
    /// Interaction logic for MaterialRequisitionForm.xaml
    /// </summary>
    public partial class MaterialRequisitionForm : UserControl
    {
        private List<MaterialRequisitionDetail> requisitionDetails = new();

        public MaterialRequisitionForm()
        {
            InitializeComponent();
            LoadCostCenters();
        }
        private void LoadCostCenters()
        {
            cmbCostCenter.ItemsSource = CostCenterBL.GetAllCostCenters();
        }
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            int itemId = Convert.ToInt32(Microsoft.VisualBasic.Interaction.InputBox("Enter Item ID:"));
            int qty = Convert.ToInt32(Microsoft.VisualBasic.Interaction.InputBox("Enter Quantity:"));

            requisitionDetails.Add(new MaterialRequisitionDetail
            {
                ItemID = itemId,
                Quantity = qty
            });

            dgItems.ItemsSource = null;
            dgItems.ItemsSource = requisitionDetails;
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            var mr = new MaterialRequisition
            {
                MRDate = DateTime.Now,
                RequestedBy = txtRequestedBy.Text,
                CostCenterID = (int)cmbCostCenter.SelectedValue,
                Remarks = txtRemarks.Text,
                Status = "Pending",
                Details = requisitionDetails
            };

            if (MaterialRequisitionBL.AddRequisition(mr))
            {
                MessageBox.Show("✅ MR Saved!");
                requisitionDetails.Clear();
                dgItems.ItemsSource = null;
            }
            else
            {
                MessageBox.Show("❌ Failed to save MR.");
            }
        }
    }
}
