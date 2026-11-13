using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data.Entities;
using Data.Repositories.Interfaces;
using GUI.Presentation;
using Microsoft.UI.Xaml;
using Uno.Extensions.Navigation;
using Windows.Gaming.Input.ForceFeedback;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GUI.Presentation.EditItem;

/// <summary>
/// this class is the ViewModel for the EditItemPage
/// it handles user input for editing/updating a food item and navigation to the kitchen/grocery page
/// </summary>
/// <remarks>
/// author: Isabela Resende Vasconcelos & Xena Tat
/// date: 18.05.2026
/// </remarks>
public partial class EditItemViewModel : ObservableObject
{

    private readonly ILocationItemRepository _locationRepo;
    private readonly IGroceryItemRepository _groceryRepo;

    /// <summary>
    /// selected item
    /// </summary>
    [ObservableProperty]
    private FoodItem _selectedItem;

    /// <summary>
    /// gets or sets the food name entered by the user
    /// </summary>
    [ObservableProperty]
    private string? _foodName;

    /// <summary>
    /// gets or sets the food quantity entered by the user
    /// </summary>
    [ObservableProperty]
    private int _quantity;

    /// <summary>
    /// gets or sets the food category entered by the user
    /// </summary>
    [ObservableProperty]
    private string? _selectedCategory;

    /// <summary>
    /// gets or sets the food expiry date entered by the user
    /// </summary>
    [ObservableProperty]
    private DateTimeOffset? _selectedExpDate;

    /// <summary>
    /// gets or sets the date picker visibility
    /// </summary>
    [ObservableProperty]
    private Visibility expDateVisibility = Visibility.Collapsed;

    /* ----- Messages to indicate validation of input ----- */

    /// <summary>
    /// gets or sets the visibility for success message
    /// </summary>
    [ObservableProperty]
    private Visibility successMessage = Visibility.Collapsed;

    /// <summary>
    /// gets or sets the visibility for error message
    /// </summary>
    [ObservableProperty]
    private Visibility errorMessage = Visibility.Collapsed;

    /// <summary>
    /// get boolean value for PointerMode (Whether clickable elements can be interacted or not)
    /// </summary>
    [ObservableProperty]
    public bool _pointerMode = true;

    private readonly INavigator _navigator;

    public EditItemViewModel(ILocationItemRepository locationRepo, IGroceryItemRepository groceryRepo, INavigator navigator, FoodItem selectedItem)
    {
        _navigator = navigator;
        _selectedItem = selectedItem;
        ExpDateVisibility = ExpDateVisibility = selectedItem is LocationItem ? Visibility.Visible : Visibility.Collapsed;
        _locationRepo = locationRepo;
        _groceryRepo = groceryRepo;
        EditFoodItem = new AsyncRelayCommand(EditItem);
        Quantity = _selectedItem.Quantity;
    }

    /// <summary>
    /// list of food categories
    /// </summary>
    public List<string> Categories { get; } = new()
    {
        "🥕 Vegetables",
        "🍉 Fruits",
        "🥛 Dairy",
        "🥩 Meat",
        "🍞 Carbohydrates",
        "🌾 Grains",
        "🍪 Snacks",
        "🧋 Beverages",
        "🌭 Condiments",
        "🍝 Sauces",
        "🧂 Seasonings",
        "🍯 Spreads",
        "🍧 Frozen",
        "🥫 Canned/Preserved",
        "🫚 Spices",
        "🌿 Herbs",
        "🍵 Tea",
        "🥜 Nuts & Seeds",
        "🐟 Seafood",
        "🍷 Liquor",
        "🥡 Leftovers"
    };

    /// <summary>
    /// gets the command that edits the food item
    /// </summary>
    public ICommand EditFoodItem { get; }

    /// <summary>
    /// edits the food item
    /// </summary>
    /// <returns>a task representing the asynchronous operation</returns>
    private async Task EditItem()
    {
        // If Item is from Location list
        if (_selectedItem is LocationItem)
        {
            LocationItem locationItem = (LocationItem)_selectedItem;
            DateOnly expDate = locationItem.ExpirationDate;
            if (Quantity <= 0)
            {
                SuccessMessage = Visibility.Collapsed;
                ErrorMessage = Visibility.Visible;
            }
            else
            {
                if (SelectedExpDate != null)
                {
                    DateTime selectedTime = SelectedExpDate?.Date ?? DateTime.Now;
                    expDate = DateOnly.FromDateTime(selectedTime);
                }
                LocationItem updatedLocationItem = new LocationItem() { FoodItemId = _selectedItem.FoodItemId, User = locationItem.User, Location = locationItem.Location, Name = FoodName, Quantity = Quantity, Type = SelectedCategory, ExpirationDate = expDate, LocationListId = locationItem.LocationListId };
                _locationRepo.UpdateLocationItem(updatedLocationItem);
                ErrorMessage = Visibility.Collapsed;
                SuccessMessage = Visibility.Visible;
            }
        }
        // If Item is from Grocery list
        else {
            if (Quantity <= 0)
            {
                SuccessMessage = Visibility.Collapsed;
                ErrorMessage = Visibility.Visible;
            }
            else
            {
                GroceryItem groceryItem = (GroceryItem)_selectedItem;
                GroceryItem updatedGroceryItem = new GroceryItem() { FoodItemId = _selectedItem.FoodItemId, User = groceryItem.User, GroceryList = groceryItem.GroceryList, Name = FoodName, Quantity = Quantity, Type = SelectedCategory, GroceryListId = groceryItem.GroceryListId };
                _groceryRepo.UpdateGroceryItem(updatedGroceryItem);
                ErrorMessage = Visibility.Collapsed;
                SuccessMessage = Visibility.Visible;
            }
        }
    }
}
