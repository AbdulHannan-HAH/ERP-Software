using ERP_Software.Models;
using ERP_Software.BL;
using ERP_Software.DL;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Software.UI
{
    public partial class StoreClassForm : UserControl
    {
        public StoreClassForm()
        {
            InitializeComponent();
            LoadStoreTypes();
            LoadClasses();
        }

        private void LoadStoreTypes()
        {
            cmbStoreTypes.ItemsSource = ProcStoreCatgBL.GetStoreNames();  // You must create this method
        }

        private void LoadClasses()
        {
            dgClasses.ItemsSource = StoreClassBL.GetAllClasses();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            StoreClass c = new StoreClass
            {
                ClassName = txtClassName.Text.Trim(),
                StoreTypeID = cmbStoreTypes.SelectedValue != null ? (int)cmbStoreTypes.SelectedValue : -1
            };

            string result = StoreClassBL.AddClass(c);
            MessageBox.Show(result);

            if (result.StartsWith("✅"))
            {
                LoadClasses();
                txtClassName.Clear();
                cmbStoreTypes.SelectedIndex = -1;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgClasses.SelectedItem is StoreClass selected)
            {
                string result = StoreClassBL.DeleteClass(selected.ClassID);
                MessageBox.Show(result);
                LoadClasses();
            }
        }
    }
}
