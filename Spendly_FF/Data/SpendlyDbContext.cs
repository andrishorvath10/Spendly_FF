using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spendly_FF.Models;

namespace Spendly_FF.Data
{
    public class SpendlyDbContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Budget> Budgets { get; set; }

        private readonly string _databasePath;

        public SpendlyDbContext(string databasePath)
        {
            _databasePath = databasePath;
            Database.EnsureCreated(); // Létrehozza az adatbázist
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }
    }
}
