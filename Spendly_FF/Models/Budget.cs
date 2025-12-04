using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spendly_FF.Models
{
    public class Budget
    {
        public int Id { get; set; }

        
        public int Year { get; set; }

        
        public int Month { get; set; }

        
        public int? CategoryId { get; set; }

        
        public decimal LimitAmount { get; set; }

        // Navigációs Property: Kapcsolat a kategóriával (opcionális 1:1 kapcsolat)
        public Category Category { get; set; }
    }
}
