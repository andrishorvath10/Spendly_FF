using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spendly_FF.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime DateUtc { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public string Notes { get; set; }
        public string ReceiptImagePath { get; set; } // Blokk fotójának útvonala
        public DateTime CreatedAtUtc { get; set; }
        // Navigációs Property:
        public Category Category { get; set; }
    }
}
