using ERP_Software.BL;
using ERP_Software.Models;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Software.UI
{
    public partial class ChartOfAccountForm : UserControl
    {
        public ChartOfAccountForm()
        {
            InitializeComponent();
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            dgAccounts.ItemsSource = ChartOfAccountBL.GetAllAccounts();
            cmbParent.ItemsSource = ChartOfAccountBL.GetAllAccounts();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ChartOfAccount acc = new ChartOfAccount
            {
                AccountCode = txtCode.Text.Trim(),
                AccountTitle = txtTitle.Text.Trim(),
                AccountType = (cmbType.SelectedItem as ComboBoxItem)?.Content.ToString(),
                ParentAccountID = cmbParent.SelectedValue != null ? (int?)cmbParent.SelectedValue : null,
                Description = txtDescription.Text.Trim()
            };

            string result = ChartOfAccountBL.AddAccount(acc);
            MessageBox.Show(result);

            if (result.StartsWith("✅"))
            {
                LoadAccounts();
                ClearForm();
            }
        }

        private void ClearForm()
        {
            txtCode.Clear();
            txtTitle.Clear();
            txtDescription.Clear();
            cmbParent.SelectedIndex = -1;
            cmbType.SelectedIndex = -1;
        }
    }
}
