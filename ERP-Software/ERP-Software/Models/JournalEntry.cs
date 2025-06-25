using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.Models
{
    public class JournalEntry
    {
        public int EntryID { get; set; }
        public DateTime EntryDate { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public List<JournalEntryDetail> Details { get; set; }
    }
}
