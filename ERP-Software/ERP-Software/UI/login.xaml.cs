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

namespace ERP_Software.UI
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        public login()
        {
            InitializeComponent();
        }

        private void btnSignin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = checkbox1.IsChecked == true
                ? txtVisiblePassword.Text.Trim()
                : txtPassword.Password.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            var user = UserBL.AuthenticateUser(username, password);
            if (user != null)
            {
                MessageBox.Show($"Welcome {user.Username}! Role: {user.RoleName}");

                // Navigate to respective dashboard
                if (user.RoleName == "Admin")
                {
                    //new AdminDashboard(user).Show(); // Pass user if needed
                }
                else if (user.RoleName == "Coach")
                {
                    //new CoachDashboard(user).Show();
                }
                else if (user.RoleName == "Player")
                {
                    //new PlayerDashboard(user).Show();
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void checkbox1_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (checkbox1.IsChecked == true)
            {
                txtVisiblePassword.Text = txtPassword.Password;
                txtVisiblePassword.Visibility = Visibility.Visible;
                txtPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtPassword.Password = txtVisiblePassword.Text;
                txtVisiblePassword.Visibility = Visibility.Collapsed;
                txtPassword.Visibility = Visibility.Visible;
            }
        }
    }
}
