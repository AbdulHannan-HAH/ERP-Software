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
    /// Interaction logic for AdminUserCreation.xaml
    /// </summary>
    public partial class AdminUserCreation : UserControl
    {
        private int selectedUserId = -1;

        public AdminUserCreation()
        {
            InitializeComponent();
            LoadRoles();
            LoadUsers();

        }
        private void LoadRoles()
        {
            cmbRoles.ItemsSource = UserRolesBL.GetRoles();
        }
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            int roleId = cmbRoles.SelectedValue != null ? (int)cmbRoles.SelectedValue : -1;

            if (roleId == -1)
            {
                MessageBox.Show("Please select a role.");
                return;
            }

            string createdBy = "Admin"; 
            string message = UserBL.CreateAdminUser(username, email, roleId, createdBy);
            MessageBox.Show(message);

            txtUsername.Clear();
            txtEmail.Clear();
            cmbRoles.SelectedIndex = -1;
        }
        private void LoadUsers()
        {
            dgUsers.ItemsSource = null;
            dgUsers.ItemsSource = UserBL.GetAllUsers();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem is User user)
            {
                var result = MessageBox.Show("Delete user?", "Confirm", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show(UserBL.DeleteUser(user.UserID));
                    LoadUsers();
                }
            }
        }

        private void dgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUsers.SelectedItem is User user)
            {
                selectedUserId = user.UserID;

                // 👇 Fill the PasswordBox with the selected user's password
                txtNewPassword.Password = user.PasswordHash;
            }
        }


        private void btnUpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUserId == -1)
            {
                MessageBox.Show("Please select a user.");
                return;
            }

            string newPass = txtNewPassword.Password.Trim();
            MessageBox.Show(UserBL.UpdatePassword(selectedUserId, newPass));

            txtNewPassword.Clear();
            selectedUserId = -1;
            dgUsers.SelectedItem = null;
        }
        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtUsernamePlaceholder.Visibility = string.IsNullOrWhiteSpace(txtUsername.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtEmailPlaceholder.Visibility = string.IsNullOrWhiteSpace(txtEmail.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }

    }
}
