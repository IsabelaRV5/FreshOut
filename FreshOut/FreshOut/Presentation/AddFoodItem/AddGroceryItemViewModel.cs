using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data.Entities;
using Data.Repositories.Interfaces;
using GUI.Presentation;
using Microsoft.UI.Xaml;
using Uno.Extensions.Navigation;

namespace GUI.Presentation.AddFoodItem;

/// <summary>
/// this class is the ViewModel for the AddGroceryItemPage
/// it handles user input for adding a new food item and navigation to the grocery lists page
/// </summary>
/// <remarks>
/// author: Jiha Poitevin & Xena Tat
/// date: 17.05.2026
/// </remarks>
public partial class AddGroceryItemViewModel : ObservableObject
{
    private readonly INavigator _navigator;
    private readonly IGroceryItemRepository _groceryItemRepo;

    public AddGroceryItemViewModel(IGroceryItemRepository groceryItemRepo, GroceryList list, INavigator navigator)
    {
        _navigator = navigator;
        _groceryItemRepo = groceryItemRepo;
        _selectedList = list;
        AddNewIngredient = new AsyncRelayCommand(AddIngredient);
    }



    /* ----- PROPERTIES ----- */

    /// <summary>
    /// selected list
    /// </summary>
    [ObservableProperty]
    private GroceryList _selectedList;

    /// <summary>
    /// gets or sets the food name entered by the user
    /// </summary>
    [ObservableProperty]
    private string? _foodName;

    /// <summary>
    /// gets or sets the food quantity entered by the user
    /// </summary>
    [ObservableProperty]
    private int _quantity = 1;

    /// <summary>
    /// gets or sets the food category entered by the user
    /// </summary>
    [ObservableProperty]
    private string? _selectedCategory;

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
        "🥤 Beverages",
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



    /* ----- MAIN LOGIC ----- */

    /// <summary>
    /// gets the command that adds a new food item to the user location list
    /// </summary>
    public ICommand AddNewIngredient { get; }

    /// <summary>
    /// adds a new food item to the user location list
    /// </summary>
    /// <returns>a task representing the asynchronous operation</returns>
    private async Task AddIngredient()
    {
        // Checks inputs if valid
        if (Quantity <= 0 || FoodName == null || FoodName.Length < 1 || SelectedCategory == null)
        {
            SuccessMessage = Visibility.Collapsed;
            ErrorMessage = Visibility.Visible;
        }
        // If valid, insert to database
        else
        {
            GroceryItem newItem = new GroceryItem() { User = _selectedList.User, GroceryList = _selectedList, Name = FoodName, Quantity = Quantity, Type = SelectedCategory, GroceryListId = _selectedList.GroceryListId };
            _groceryItemRepo.AddItemToGroceryList(_selectedList.GroceryListId, newItem);
            ErrorMessage = Visibility.Collapsed;
            SuccessMessage = Visibility.Visible;
        }
    }
}
