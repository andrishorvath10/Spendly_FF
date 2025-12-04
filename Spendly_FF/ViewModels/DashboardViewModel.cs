using System.Collections.ObjectModel;
using System.Transactions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendly_FF.Repositories;
using Spendly_FF.Services;
using System.Linq;

public partial class DashboardViewModel : ObservableObject
{
    private readonly ITransactionRepository _transactionRepo;
    private readonly IExportService _exportService;
    private readonly IConnectivityService _connectivityService;

    [ObservableProperty]
    public decimal _totalIncome;
    [ObservableProperty]
    private decimal _totalExpenses;
    [ObservableProperty]
    private string _networkStatus = "Hálózati állapot betöltése...";
    [ObservableProperty]
    private ObservableCollection<Transaction> _latestTransactions = new();

    private int _currentMonth = DateTime.Now.Month;
    private int _currentYear = DateTime.Now.Year;

    public DashboardViewModel(ITransactionRepository transactionRepo, IExportService exportService, IConnectivityService connectivityService)
    {
        _transactionRepo = transactionRepo;
        _exportService = exportService;
        _connectivityService = connectivityService;

        // 1. Feliratkozás a hálózati állapotváltozásokra
        _connectivityService.ConnectivityChanged += OnConnectivityChanged;

        // Kezdő hálózati állapot beállítása a konstruktorban
        UpdateNetworkStatus(_connectivityService.NetworkAccess);

        // Aszinkron adatbetöltés indítása
        Task.Run(LoadDataAsync);
    }

    [RelayCommand]
    public async Task LoadDataAsync()
    {
        // Először töltsük le a tranzakciókat
        var transactions = await _transactionRepo.GetTransactionsForMonthAsync(_currentYear, _currentMonth);

        // Számítások
        TotalIncome = transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
        TotalExpenses = transactions.Where(t => t.Amount < 0).Sum(t => Math.Abs(t.Amount));

        // Frissíti a legutóbbi tranzakciókat
        LatestTransactions.Clear();
        foreach (var t in transactions.OrderByDescending(t => t.DateUtc).Take(5))
        {
            LatestTransactions.Add(t);
        }

        // Megjegyzés: Ha a LoadDataAsync hívás IMessenger-en keresztül érkezik, 
        // érdemes MainThread.BeginInvokeOnMainThread()-et használni,
        // de mivel Task.Run-ból és Command-ból hívódik, a frissítések általában rendben vannak.
    }

    // --- Parancs: Havi Riport Megosztása (Kiegészítő funkció #2) ---
    [RelayCommand]
    private async Task ShareMonthlyReportAsync()
    {
        try
        {
            string filePath = await _exportService.ExportMonthlyReport(_currentYear, _currentMonth);
            await _exportService.ShareFileAsync(filePath);
        }
        catch (Exception ex)
        {
            // Hibakezelés (pl. DisplayAlert megjelenítése)
        }
    }

    // --- Parancs: Navigálás az új tranzakcióhoz ---
    [RelayCommand]
    private async Task GoToNewTransactionAsync()
    {
        await Shell.Current.GoToAsync("TransactionEditor");
    }

    // -----------------------------------------------------------------
    // Hálózati Kapcsolat Detektálás Logika (kiegészítés)
    // -----------------------------------------------------------------

    // Eseménykezelő a hálózati változásra
    private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        // Fontos: a UI-t csak a Main Thread-en lehet frissíteni. 
        // A ConnectivityChanged esemény más szálon is futhat.
        MainThread.BeginInvokeOnMainThread(() =>
        {
            UpdateNetworkStatus(e.NetworkAccess);
        });
    }

    private void UpdateNetworkStatus(NetworkAccess access)
    {
        if (access == NetworkAccess.Internet)
        {
            NetworkStatus = "Online (Internet elérés)";
        }
        else if (access == NetworkAccess.Local)
        {
            NetworkStatus = "Helyi hálózat (Nincs Internet)";
        }
        else
        {
            NetworkStatus = "Offline (Nincs kapcsolat)";
        }
    }

    // Megjegyzés: Disposable interface implementálása javasolt az esemény leiratkozásához
}