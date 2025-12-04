using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spendly_FF.Models;

namespace Spendly_FF.Repositories
{
    public interface ICategoryRepository
    {
        // READ
        Task<List<Category>> GetAllActiveAsync(); // Ezt már láttuk (aktív kategóriák)
        Task<Category> GetByIdAsync(int id);

        // CREATE
        Task AddAsync(Category category);

        // UPDATE
        Task UpdateAsync(Category category);

        // DELETE
        Task DeleteAsync(Category category);
    }
}
