using Spendly_FF.Repositories;
namespace Spendly_FF.Services
{
    public interface IExportService
    {
        Task<string> ExportMonthlyReport(int year, int month);
        Task ShareFileAsync(string filePath);
    }
    public class ExportService : IExportService
    {
        private readonly ITransactionRepository _repo;
        public ExportService(ITransactionRepository repo) => _repo = repo;

        public async Task<string> ExportMonthlyReport(int year, int month)
        {
            // ... Logika a CSV tartalom generálására ...
            var data = await _repo.GetTransactionsForMonthAsync(year, month);
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Dátum,Összeg,Kategória,Megjegyzés");
            foreach (var t in data)
            {
                sb.AppendLine($"{t.DateUtc:yyyy-MM-dd},{t.Amount},{t.Category.Name},{t.Notes.Replace(",", "")}");
            }
            string csvContent = sb.ToString();

            // Fájl mentése az ideiglenes könyvtárba
            string fileName = $"Spendly_Report_{year}_{month}.csv";
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            await File.WriteAllTextAsync(filePath, csvContent);

            return filePath;
        }

        public async Task ShareFileAsync(string filePath)
        {
             await Share.Default.RequestAsync(new ShareFileRequest // Megosztási kérés indítása [cite: 100]
            {
                Title = "Havi kiadás riport",
                File = new ShareFile(filePath) // ShareFileRequest fájl megosztásához [cite: 102]
            });
        }
    }
}
