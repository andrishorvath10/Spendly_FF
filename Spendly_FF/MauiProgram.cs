

using Spendly_FF.Data;
using Spendly_FF.Repositories;
using Spendly_FF.Services;

namespace Spendly_FF
{

    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>();

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "spendly.db3");

            // DbContext és Repository rétegek (Singleton)
            builder.Services.AddSingleton<SpendlyDbContext>(s => new SpendlyDbContext(dbPath));
            builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();
            builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();

            // Service rétegek (Kamera, Exportálás)
            builder.Services.AddSingleton<IPhotoService, PhotoService>();
            builder.Services.AddSingleton<IExportService, ExportService>();
            builder.Services.AddSingleton<IConnectivityService, ConnectivityService>();

            // ViewModel-ek (Dashboard=Singleton, Editor/Lista=Transient)
            builder.Services.AddSingleton<DashboardViewModel>();
            builder.Services.AddTransient<TransactionListViewModel>();
            builder.Services.AddTransient<TransactionEditorViewModel>();

            // View-k (Page-ek)
            builder.Services.AddSingleton<DashboardPage>();
            builder.Services.AddTransient<TransactionListPage>();
            builder.Services.AddTransient<TransactionEditorPage>(); // Szerkesztő oldal (CRUD)

            return builder.Build();
        }
    }
}
