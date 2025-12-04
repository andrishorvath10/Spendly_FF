using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spendly_FF.Models;
using Spendly_FF.Data;
using Microsoft.EntityFrameworkCore;

namespace Spendly_FF.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly SpendlyDbContext _context;
        public TransactionRepository(SpendlyDbContext context) => _context = context;

        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Transaction transaction)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<Transaction> GetByIdAsync(int id)
        {
            return await _context.Transactions.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.Include(t => t.Category).ToListAsync();
        }

        public async Task<List<Transaction>> GetTransactionsForMonthAsync(int year, int month)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.DateUtc.Year == year && t.DateUtc.Month == month)
                .ToListAsync();
        }
    }
}
