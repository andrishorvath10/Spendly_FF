using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendly_FF.Models;
using Spendly_FF.Repositories;
using System.Collections.ObjectModel;
using System.Transactions;

public partial class TransactionListViewModel : ObservableObject
{
    private readonly ITransactionRepository _transactionRepo;
    private readonly ICategoryRepository _categoryRepo;

    // Eredeti lista (nem módosul szűréskor, csak a memóriában)
    private List<Transaction> _allTransactions = new List<Transaction>();

    // ObservableCollection: Ez kötődik a View CollectionView-hoz
    [ObservableProperty]
    private ObservableCollection<Transaction> _filteredTransactions = new();

    // Szűrési és keresési tulajdonságok (TwoWay binding a View-ban)
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FilteredTransactions))] // Ha ez változik, frissíti a FilteredTransactions-t
    private string _searchText = string.Empty;

    [ObservableProperty]
    private ObservableCollection<Category> _availableCategories = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FilteredTransactions))]
    private Category _selectedCategory;

    // ----------------------------------------------------
    // Konstruktor és Inicializálás
    // ----------------------------------------------------
    public TransactionListViewModel(ITransactionRepository transactionRepo, ICategoryRepository categoryRepo)
    {
        _transactionRepo = transactionRepo;
        _categoryRepo = categoryRepo;

        // A ViewModel betöltésekor azonnal elkezdjük az adatok betöltését
        Task.Run(LoadDataAsync);
    }

    // ----------------------------------------------------
    // Parancs: Adatok Betöltése (ReadAll)
    // ----------------------------------------------------
    [RelayCommand]
    public async Task LoadDataAsync()
    {
        // 1. Kategóriák betöltése a szűrő Picker-hez
        var categories = await _categoryRepo.GetAllActiveAsync();
        AvailableCategories.Clear();
        foreach (var c in categories) AvailableCategories.Add(c);

        // 2. Összes tranzakció betöltése az adatbázisból
        _allTransactions = await _transactionRepo.GetAllAsync();

        // 3. Szűrés és frissítés
        FilterAndApplyTransactions();
    }

    // ----------------------------------------------------
    // Parancs: Elem Kiválasztása (Navigálás Szerkesztésre)
    // ----------------------------------------------------
    [RelayCommand]
    private async Task SelectTransactionAsync(Transaction transaction)
    {
        if (transaction == null) return;

        // Navigálunk a TransactionEditorPage-re az ID paraméterrel
        await Shell.Current.GoToAsync($"TransactionEditor?id={transaction.Id}");
    }

    // ----------------------------------------------------
    // Privát metódus: Adatok szűrése és a View frissítése
    // ----------------------------------------------------
    private void FilterAndApplyTransactions()
    {
        IEnumerable<Transaction> transactions = _allTransactions;

        // 1. Keresés szöveges szűrővel
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            transactions = transactions.Where(t => t.Notes != null && t.Notes.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        // 2. Kategória szerinti szűrés
        if (SelectedCategory != null)
        {
            transactions = transactions.Where(t => t.CategoryId == SelectedCategory.Id);
        }

        // Végül frissítjük az ObservableCollection-t a View számára
        FilteredTransactions.Clear();
        foreach (var t in transactions.OrderByDescending(t => t.DateUtc))
        {
            FilteredTransactions.Add(t);
        }
    }

    // Ezt hívhatjuk meg a LoadDataAsync után, hogy meggyőződjünk a UI frissítéséről
    partial void OnSearchTextChanged(string value) => FilterAndApplyTransactions();
    partial void OnSelectedCategoryChanged(Category value) => FilterAndApplyTransactions();
}