using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Spendly_FF.Models;
using Spendly_FF.Repositories;
using Spendly_FF.Services;
using System.Collections.ObjectModel;

public partial class TransactionEditorViewModel : ObservableObject, IQueryAttributable
{
    private readonly ITransactionRepository _transactionRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IPhotoService _photoService;

    [ObservableProperty]
    private Transaction _currentTransaction = new Transaction();

    [ObservableProperty]
    private ObservableCollection<Category> _availableCategories = new();

    [ObservableProperty]
    private Category _selectedCategory;

    [ObservableProperty]
    private bool _isEditingExisting = false;

    private int _transactionId;

    public TransactionEditorViewModel(ITransactionRepository transactionRepo, IPhotoService photoService, ICategoryRepository categoryRepo)
    {
        _transactionRepo = transactionRepo;
        _photoService = photoService;
        _categoryRepo = categoryRepo;

        LoadCategoriesAsync();
    }

    private async Task LoadCategoriesAsync()
    {
        // Töltse be a kategóriákat a Picker/CollectionView számára
        var categories = await _categoryRepo.GetAllActiveAsync();
        AvailableCategories.Clear();
        foreach (var c in categories) AvailableCategories.Add(c);

        // Kezelje az új/szerkesztés eseteket a kategória kiválasztásánál
    }

    // --- Parancs: Tranzakció mentése (CRUD) ---
    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveTransactionAsync()
    {
        if (!CanSave()) return;

        try
        {
            CurrentTransaction.CategoryId = SelectedCategory.Id;

            if (IsEditingExisting)
            {
                await _transactionRepo.UpdateAsync(CurrentTransaction);
            }
            else
            {
                await _transactionRepo.AddAsync(CurrentTransaction);
            }

            // IMessenger használata: üzenetküldés a DashboardViewModel-nek a frissítéshez
            // WeakReferenceMessenger.Default.Send(new TransactionSavedMessage(true)); 

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            // Hibakezelés
        }
    }

    // Futtathatósági logika
    private bool CanSave() => CurrentTransaction.Amount != 0 && SelectedCategory != null;

    // --- Parancs: Blokk Fotózása (Kamera) ---
    [RelayCommand]
    private async Task TakeReceiptPhotoAsync()
    {
        var photoResult = await _photoService.CapturePhotoAsync();
        if (photoResult != null)
        {
            CurrentTransaction.ReceiptImagePath = photoResult.FullPath;
            OnPropertyChanged(nameof(CurrentTransaction)); // UI frissítés a kép megjelenítéséhez
        }
    }

    // Navigációs paraméter fogadása
    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out object idValue) && idValue is string idString && int.TryParse(idString, out int id))
        {
            _transactionId = id;
            IsEditingExisting = true;
            CurrentTransaction = await _transactionRepo.GetByIdAsync(_transactionId);
            // SelectedCategory beállítása a CurrentTransaction.Category alapján
            SelectedCategory = AvailableCategories.FirstOrDefault(c => c.Id == CurrentTransaction.CategoryId);
        }
    }
}