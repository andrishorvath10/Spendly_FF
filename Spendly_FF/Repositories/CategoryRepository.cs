using Microsoft.EntityFrameworkCore;
using Spendly_FF.Data;
using Spendly_FF.Models;
using Spendly_FF.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly SpendlyDbContext _context;

    // Konstruktor injektálás: a DI adja át a DbContext példányt
    public CategoryRepository(SpendlyDbContext context)
    {
        _context = context;
        // Az EF Core-ban a DbContext-et kell kezelni, nem a Connectiont
    }

    // READ: Aktív kategóriák lekérdezése (a TransactionEditor-nak kell)
    public async Task<List<Category>> GetAllActiveAsync()
    {
        // Csak az aktív (nem archivált) kategóriákat adja vissza
        return await _context.Categories.Where(c => !c.IsArchived).ToListAsync();
    }

    // READ: Egy kategória lekérdezése ID alapján
    public async Task<Category> GetByIdAsync(int id)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }

    // CREATE: Új kategória hozzáadása
    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    // UPDATE: Meglévő kategória módosítása
    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    // DELETE: Kategória törlése
    public async Task DeleteAsync(Category category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        // Javaslat: Éles alkalmazásban a Category-t nem törölni kell, hanem archiválni (IsArchived = true)
        // A kód, ami az archiválást implementálja:
        /*
        category.IsArchived = true;
        await UpdateAsync(category);
        */
    }
}