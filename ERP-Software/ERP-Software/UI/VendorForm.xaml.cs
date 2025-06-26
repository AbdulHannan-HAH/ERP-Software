using ERP_Software.BL;
using ERP_Software.DL;
using ERP_Software.Models;
using ERP_Software.Models.ERP_Software.Models;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Software.UI
{
    public partial class VendorForm : UserControl
    {
        public VendorForm()
        {
            InitializeComponent();
            LoadCOA();
            LoadVendors();
        }

        private void LoadCOA()
        {
            cmbCOA.ItemsSource = ChartOfAccountBL.GetAllAccounts(); // Should return list of { AccountID, AccountTitle }
        }

        private void LoadVendors()
        {
            dgVendors.ItemsSource = VendorBL.GetAllVendors();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Vendor v = new Vendor
            {
                VendorName = txtName.Text.Trim(),
                Contact = txtContact.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                TaxNumber = txtTax.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                COAAccountID = (int)(cmbCOA.SelectedValue ?? 0)
            };

            string result = VendorBL.AddVendor(v);
            MessageBox.Show(result);

            if (result.StartsWith("✅"))
            {
                LoadVendors();
                ClearForm();
            }
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtContact.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtTax.Clear();
            txtAddress.Clear();
            cmbCOA.SelectedIndex = -1;
        }
    }
}
