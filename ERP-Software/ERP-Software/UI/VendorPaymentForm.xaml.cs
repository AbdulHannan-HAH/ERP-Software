using ERP_Software.BL;
using ERP_Software.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Software.UI
{
    public partial class VendorPaymentForm : UserControl
    {
        private List<VendorInvoice> unpaidInvoices = new();

        public VendorPaymentForm()
        {
            InitializeComponent();
            dpPaymentDate.SelectedDate = DateTime.Now;
            LoadVendors();
        }

        private void LoadVendors()
        {
            cmbVendors.ItemsSource = VendorBL.GetAllVendors();
        }

        private void cmbVendors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbVendors.SelectedValue == null) return;

            int vendorId = (int)cmbVendors.SelectedValue;
            unpaidInvoices = VendorPaymentBL.GetUnpaidInvoicesByVendor(vendorId);
            cmbInvoices.ItemsSource = unpaidInvoices;
        }

        private void cmbInvoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbInvoices.SelectedItem is VendorInvoice invoice)
            {
                txtAmount.Text = invoice.TotalAmount.ToString("0.00");
            }
        }

        private void btnMakePayment_Click(object sender, RoutedEventArgs e)
        {
            if (cmbVendors.SelectedValue == null || cmbInvoices.SelectedValue == null || string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                MessageBox.Show("❌ Please select all required fields.");
                return;
            }

            VendorPayment payment = new VendorPayment
            {
                VendorID = (int)cmbVendors.SelectedValue,
                InvoiceID = (int)cmbInvoices.SelectedValue,
                PaymentDate = dpPaymentDate.SelectedDate ?? DateTime.Now,
                AmountPaid = decimal.Parse(txtAmount.Text),
                PaymentMethod = (cmbMethod.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Remarks = txtRemarks.Text
            };

            string result = VendorPaymentBL.AddPaymentWithJournal(payment, $"Payment to vendor for invoice #{payment.InvoiceID}");
            MessageBox.Show(result);

            if (result.StartsWith("✅"))
            {
                cmbInvoices.SelectedIndex = -1;
                txtAmount.Clear();
                txtRemarks.Clear();
                cmbMethod.SelectedIndex = -1;
                dpPaymentDate.SelectedDate = DateTime.Now;
                unpaidInvoices = VendorPaymentBL.GetUnpaidInvoicesByVendor(payment.VendorID);
                cmbInvoices.ItemsSource = unpaidInvoices;
            }
        }
    }
}
