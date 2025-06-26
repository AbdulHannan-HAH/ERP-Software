using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP_Software.Utils
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Add = new RoutedUICommand("Add", "Add", typeof(CustomCommands));
        public static readonly RoutedUICommand Update = new RoutedUICommand("Update", "Update", typeof(CustomCommands));
        public static readonly RoutedUICommand Delete = new RoutedUICommand("Delete", "Delete", typeof(CustomCommands));
    }
}

