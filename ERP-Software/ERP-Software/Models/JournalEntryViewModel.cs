using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.Models
{
    public class JournalEntryViewModel
    {
        public string AccountTitle { get; set; }
        public string Description { get; set; }
        public string EntryDate { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
