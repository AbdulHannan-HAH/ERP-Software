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
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void ManageUsers_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            AdminUserCreation users = new AdminUserCreation(); 
            MainContentGrid.Children.Add(users);
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {

        }

       

        private void ManageRoles_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            UserRoles roles = new UserRoles(); 
            MainContentGrid.Children.Add(roles);
        }

        private void StoreClasses_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            StoreClassForm roles = new StoreClassForm(); 
            MainContentGrid.Children.Add(roles);
        }

        private void AddStores_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            StoreForm hah = new StoreForm();
            MainContentGrid.Children.Add(hah);
        }

        private void CostCenters_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            CostCenterForm hah = new CostCenterForm();
            MainContentGrid.Children.Add(hah);
        }

        private void SIN_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            SINForm hah = new SINForm();
            MainContentGrid.Children.Add(hah);
        }

        private void PurchaseRequisition_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            PurchaseRequisitionForm hah = new PurchaseRequisitionForm();
            MainContentGrid.Children.Add(hah);
        }

        private void PO_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            PurchaseOrderForm hah = new PurchaseOrderForm();
            MainContentGrid.Children.Add(hah);
        }

        private void GRN_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            GRNForm hah = new GRNForm();
            MainContentGrid.Children.Add(hah);
        }

        private void MaterialRequisition_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            MaterialRequisitionForm hah = new MaterialRequisitionForm();
            MainContentGrid.Children.Add(hah);
        }

        private void ManageItems_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            ItemForm hah = new ItemForm(); 
            MainContentGrid.Children.Add(hah);
        }

        private void AddStoreCategories_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            StoreNames hah = new StoreNames();
            MainContentGrid.Children.Add(hah);
        }

        private void VendorItems_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            VendorItemForm hah = new VendorItemForm();
            MainContentGrid.Children.Add(hah);
        }

        private void ManageVendors_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            VendorForm hah = new VendorForm();
            MainContentGrid.Children.Add(hah);
        }

        private void VendorInvoice_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            VendorInvoiceForm hah = new VendorInvoiceForm();
            MainContentGrid.Children.Add(hah);
        }

        private void VendorPayment_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            VendorPaymentForm hah = new VendorPaymentForm();
            MainContentGrid.Children.Add(hah);
        }

        private void COA_Click(object sender, RoutedEventArgs e)
        {
            MainContentGrid.Children.Clear();
            ChartOfAccountForm hah = new ChartOfAccountForm();
            MainContentGrid.Children.Add(hah);
        }
    }
}
