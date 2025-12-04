using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Spendly_FF.Repositories
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(Transaction transaction);
        Task<Transaction> GetByIdAsync(int id);
        Task<List<Transaction>> GetAllAsync();
        Task<List<Transaction>> GetTransactionsForMonthAsync(int year, int month);
    }
}
