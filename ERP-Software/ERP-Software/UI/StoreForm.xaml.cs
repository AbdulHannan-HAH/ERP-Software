using ERP_Software.BL;
using ERP_Software.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Software.UI
{
    public partial class StoreForm : UserControl
    {
        private int selectedStoreId = -1;
        private static readonly string conStr = ConfigurationManager.ConnectionStrings["ERPConnection"].ConnectionString;

        public StoreForm()
        {
            InitializeComponent();
            LoadStoreTypes();
            LoadCOAParents();
            LoadStores();
        }

        private void LoadStoreTypes()
        {
            var list = new List<ProcStoreCatg>();
            using var con = new SqlConnection(conStr);
            con.Open();
            using var cmd = new SqlCommand("SELECT name_id, store_name FROM procure_stores", con);
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
                list.Add(new ProcStoreCatg
                {
                    storeid = dr.GetInt32(0),
                    storename = dr.GetString(1)
                });
            cmbType.ItemsSource = list;
        }

        private void LoadCOAParents()
        {
            cmbParentCOA.ItemsSource = ChartOfAccountBL.GetParentAccounts();
        }

        private void cmbParentCOA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbParentCOA.SelectedValue != null)
            {
                int pid = (int)cmbParentCOA.SelectedValue;
                cmbChildCOA.ItemsSource = ChartOfAccountBL.GetChildAccounts(pid);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStoreName.Text)
             || cmbType.SelectedValue == null
             || cmbChildCOA.SelectedValue == null)
            {
                MessageBox.Show("Please fill all fields and select a COA child.");
                return;
            }

            var st = new Stores
            {
                StoreName = txtStoreName.Text.Trim(),
                Type = cmbType.SelectedValue.ToString(),
                Location = txtLocation.Text.Trim(),
                COAAccountID = (int)cmbChildCOA.SelectedValue
            };

            var res = StoreBL.AddStore(st);
            MessageBox.Show(res);
            Clear();
            LoadStores();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedStoreId <= 0) { MessageBox.Show("Select a store first."); return; }

            var st = new Stores
            {
                StoreID = selectedStoreId,
                StoreName = txtStoreName.Text.Trim(),
                Type = cmbType.SelectedValue.ToString(),
                Location = txtLocation.Text.Trim(),
                COAAccountID = (int)cmbChildCOA.SelectedValue
            };
            var res = StoreBL.UpdateStore(st);
            MessageBox.Show(res);
            Clear();
            LoadStores();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedStoreId <= 0) { MessageBox.Show("Select to delete."); return; }
            var res = StoreBL.DeleteStore(selectedStoreId);
            MessageBox.Show(res);
            Clear();
            LoadStores();
        }

        private void dgStores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgStores.SelectedItem is Stores st)
            {
                selectedStoreId = st.StoreID;
                txtStoreName.Text = st.StoreName;
                txtLocation.Text = st.Location;
                cmbType.SelectedValue = st.Type;
                cmbParentCOA.SelectedValue = st.COAParentAccountID;
                cmbChildCOA.SelectedValue = st.COAAccountID;
                txtCOAInfo.Text = st.COAAccountTitle;
            }
        }

        private void Clear()
        {
            txtStoreName.Clear();
            txtLocation.Clear();
            cmbType.SelectedIndex = -1;
            cmbParentCOA.SelectedIndex = -1;
            cmbChildCOA.SelectedIndex = -1;
            txtCOAInfo.Text = "Select a child account";
            selectedStoreId = -1;
        }

        private void LoadStores()
        {
            dgStores.ItemsSource = StoreBL.GetAllStoresWithCOA();
        }
    }
}
