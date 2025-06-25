using ERP_Software.Models;
using ERP_Software.BL;
using ERP_Software.DL;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Software.UI
{
    public partial class ItemForm : UserControl
    {
        public ItemForm()
        {
            InitializeComponent();
            LoadStoreTypes();
            LoadClasses();
            LoadItems();
        }

        private void LoadStoreTypes()
        {
            cmbStoreTypes.ItemsSource = StoreBL.GetAllStores(); // Must return list with StoreID & Type
        }

        private void LoadClasses()
        {
            cmbClasses.ItemsSource = StoreClassBL.GetAllClasses(); // Must return list with ClassID & ClassName
        }

        private void LoadItems()
        {
            dgItems.ItemsSource = ItemBL.GetAllItems(); // Must join with Store & Class to show names
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Item item = new Item
            {
                Name = txtName.Text.Trim(),
                Unit = txtUnit.Text.Trim(),
                MinLevel = int.TryParse(txtMinLevel.Text, out int level) ? level : 0,
                StoreID = cmbStoreTypes.SelectedValue != null ? (int)cmbStoreTypes.SelectedValue : -1,
                ClassID = cmbClasses.SelectedValue != null ? (int)cmbClasses.SelectedValue : -1
            };

            string result = ItemBL.AddItem(item);
            MessageBox.Show(result);

            if (result.StartsWith("✅"))
            {
                LoadItems();
                ClearForm();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgItems.SelectedItem is Item selectedItem)
            {
                string result = ItemBL.DeleteItem(selectedItem.ItemID);
                MessageBox.Show(result);
                LoadItems();
            }
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtUnit.Clear();
            txtMinLevel.Clear();
            cmbStoreTypes.SelectedIndex = -1;
            cmbClasses.SelectedIndex = -1;
        }
    }
}
