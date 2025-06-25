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
    public partial class StoreNames : UserControl
    {
        int selectedRoleId = -1;
        public StoreNames()
        {
            InitializeComponent();
            LoadStores();
        }
        private void LoadStores()
        {
            dgStores.ItemsSource = null;
            dgStores.ItemsSource = ProcStoreCatgBL.GetStoreNames();
        }
        private void txtRoleName_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPlaceholder.Visibility = string.IsNullOrWhiteSpace(txtRoleName.Text) ? Visibility.Visible : Visibility.Hidden;
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string result = ProcStoreCatgBL.AddStore(txtRoleName.Text.Trim());
            MessageBox.Show(result);

            txtRoleName.Clear();
            selectedRoleId = -1;
            LoadStores();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRoleId == -1)
            {
                MessageBox.Show("Please select a store to update.");
                return;
            }

            string result = ProcStoreCatgBL.UpdateStoreName(selectedRoleId, txtRoleName.Text.Trim());
            MessageBox.Show(result);

            txtRoleName.Clear();
            selectedRoleId = -1;
            LoadStores();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRoleId == -1)
            {
                MessageBox.Show("Please select a store to delete.");
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this store?", "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                string message = ProcStoreCatgBL.DeleteStore(selectedRoleId);
                MessageBox.Show(message);
                txtRoleName.Clear();
                selectedRoleId = -1;
                LoadStores();
            }
        }

        private void dgStores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgStores.SelectedItem is ProcStoreCatg selectedRole)
            {
                txtRoleName.Text = selectedRole.storename;
                selectedRoleId = selectedRole.storeid;
            }
        }


        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnAdd_Click(sender, e);
        }

        private void UpdateCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnUpdate_Click(sender, e);
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnDelete_Click(sender, e);
        }

    }
}
