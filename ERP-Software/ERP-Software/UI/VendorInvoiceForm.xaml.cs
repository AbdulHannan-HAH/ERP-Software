using ERP_Software.BL;
using ERP_Software.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Software.UI
{
    public partial class VendorInvoiceForm : UserControl
    {
        public VendorInvoiceForm()
        {
            InitializeComponent();
            LoadGRNs();
            dpInvoiceDate.SelectedDate = DateTime.Now;
        }

        private void LoadGRNs()
        {
            cmbGRNs.ItemsSource = GRNBL.GetGRNsWithoutInvoice(); // Only GRNs not yet invoiced
        }

        private void btnSaveInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (cmbGRNs.SelectedValue == null || string.IsNullOrWhiteSpace(txtInvoiceNo.Text) || !decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("❌ Please fill all fields correctly.");
                return;
            }

            var grn = (GRN)cmbGRNs.SelectedItem;

            VendorInvoice invoice = new VendorInvoice
            {
                GRNID = grn.GRNID,
                VendorID = grn.VendorID, // assume GRN model includes VendorID
                InvoiceNumber = txtInvoiceNo.Text.Trim(),
                InvoiceDate = dpInvoiceDate.SelectedDate ?? DateTime.Now,
                TotalAmount = amount,
                Status = "Unpaid"
            };

            string result = VendorInvoiceBL.AddInvoiceWithJournal(invoice, txtDescription.Text.Trim());
            MessageBox.Show(result);

            if (result.StartsWith("✅"))
            {
                cmbGRNs.SelectedIndex = -1;
                txtInvoiceNo.Clear();
                txtAmount.Clear();
                txtDescription.Clear();
                dpInvoiceDate.SelectedDate = DateTime.Now;
            }
        }
        private int selectedVendorID = 0;

        private void cmbGRNs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbGRNs.SelectedItem is GRN selectedGRN)
            {
                selectedVendorID = selectedGRN.VendorID;

                decimal amount = GRNBL.GetGRNTotalAmount(selectedGRN.GRNID);
                txtAmount.Text = amount.ToString("N2");
            }
        }


    }
}
