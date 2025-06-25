using ERP_Software.BL;
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
using ERP_Software.Models;

namespace ERP_Software.UI
{
    /// <summary>
    /// Interaction logic for UserRoles.xaml
    /// </summary>
    public partial class UserRoles : UserControl
    {
        private int selectedRoleId = -1;
        public UserRoles()
        {
            InitializeComponent();
            LoadRoles();
        }
        private void LoadRoles()
        {
            dgRoles.ItemsSource = null;
            dgRoles.ItemsSource = UserRolesBL.GetRoles();
        }
        private void txtRoleName_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPlaceholder.Visibility = string.IsNullOrWhiteSpace(txtRoleName.Text) ? Visibility.Visible : Visibility.Hidden;
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRoleId == -1)
            {
                MessageBox.Show("Please select a role to delete.");
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this role?", "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                string message = UserRolesBL.DeleteRole(selectedRoleId);
                MessageBox.Show(message);
                txtRoleName.Clear();
                selectedRoleId = -1;
                LoadRoles();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string result = UserRolesBL.AddRole(txtRoleName.Text.Trim());
            MessageBox.Show(result);

            txtRoleName.Clear();
            selectedRoleId = -1;
            LoadRoles();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRoleId == -1)
            {
                MessageBox.Show("Please select a role to update.");
                return;
            }

            string result = UserRolesBL.UpdateRole(selectedRoleId, txtRoleName.Text.Trim());
            MessageBox.Show(result);

            txtRoleName.Clear();
            selectedRoleId = -1;
            LoadRoles();
        }
        private void dgRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgRoles.SelectedItem is Roles selectedRole)
            {
                txtRoleName.Text = selectedRole.RoleName;
                selectedRoleId = selectedRole.RoleID;
            }
        }
    }
}
