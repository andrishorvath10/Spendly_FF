using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spendly_FF.Models
{
    public class Category
    {
        public int Id { get; set; }

        // A kategória neve, pl. "Élelmiszer" 
        public string Name { get; set; }

       
        public string Type { get; set; }

        
        public string Icon { get; set; }

        
        public string ColorHex { get; set; }

         
        public bool IsArchived { get; set; }

        
        public List<Transaction> Transactions { get; set; }

        
        public List<Budget> Budgets { get; set; }
    }
}
