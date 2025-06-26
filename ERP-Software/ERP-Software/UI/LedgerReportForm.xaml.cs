using ERP_Software.BL;
using ERP_Software.DL;
using ERP_Software.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Software.UI
{
    public partial class LedgerReportForm : UserControl
    {
        public LedgerReportForm()
        {
            InitializeComponent();
            cmbAccounts.ItemsSource = ChartOfAccountBL.GetAllAccounts();
        }

        private void btnViewLedger_Click(object sender, RoutedEventArgs e)
        {
            if (cmbAccounts.SelectedValue == null)
            {
                MessageBox.Show("Please select an account.");
                return;
            }

            int accountId = (int)cmbAccounts.SelectedValue;
            var ledger = LedgerReportBL.GetLedger(accountId);
            dgLedger.ItemsSource = ledger;
        }
    }
}
