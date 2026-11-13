using System.Collections.ObjectModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using Data.Entities;
using Data.Repositories.Database;
using Data.Repositories.Interfaces;
using GUI.Presentation;
using GUI.Presentation.AddFoodItem;
using GUI.Presentation.EditItem;
using Logic;
using Microsoft.UI;
using Uno.Extensions.Navigation;

namespace GUI.Presentation.GroceryLists;

/// <summary>
/// this class is the ViewModel for the GroceryListsPage
/// it provides navigation to the AddGroceryItemPage, the ability to remove all food items, the ability to create new grocery lists, and to search within each grocery list
/// </summary>
/// <remarks>
/// author: Jiha Poitevin & Isabela Resende Vasconcelos & Xena Tat
/// date: 18.05.2026
/// </remarks>
public partial class GroceryListsViewModel : ObservableObject
{
    private readonly INavigator _navigator;
    public readonly IGroceryListRepository _groceryListRepo;
    private readonly IGroceryItemRepository _groceryItemRepo;

    /// <summary>
    /// initializes a new instance of the GroceryListsViewModel class
    /// </summary>
    /// <param name="navigator">the navigation service used to switch between view models</param>
    /// <param name="groceryListRepo">the grocery list repository</param>
    /// <param name="groceryItemRepo">the grocery items repository</param>
    /// <param name="user">the logged in user</param>
    /// 
    public GroceryListsViewModel(INavigator navigator, IGroceryListRepository groceryListRepo, IGroceryItemRepository groceryItemRepo, User user)
    {
        _navigator = navigator;
        _groceryListRepo = groceryListRepo;
        _groceryItemRepo = groceryItemRepo;
        _user = user;

        Title = "Grocery Lists";
        GoToAddFoodItem = new AsyncRelayCommand(GoToAddFoodItemView);
        DeleteOne = new AsyncRelayCommand<object>(RemoveOne);
        DeleteAllChecked = new AsyncRelayCommand(RemoveAllChecked);
        CheckItem = new AsyncRelayCommand<object>(CheckCurrentItem);
        GoToEditItem = new AsyncRelayCommand<object>(GoToEditItemView);
        _ = LoadAsync();

        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        string month = today.ToString("MMMM");
        string year = today.Year.ToString();
        string day = today.Day.ToString();

        CurrentDate = $"{month} {day} {year}";

        SearchCommand = new RelayCommand(Search);
        ResetListCommand = new RelayCommand(ResetList);
        SortByNameCommand = new RelayCommand(SortByName);
        SortByQuantityCommand = new RelayCommand(SortByQuantity);

        GroceryLists = new ObservableCollection<GroceryList>(_groceryListRepo.GetUserGroceryLists(_user.UserId));
        if (GroceryLists.Count > 0)
        {
            SelectedGroceryList = GroceryLists[0];
            Items = new ObservableCollection<GroceryItem>(SelectedGroceryList.Items);
        }
    }


    /* ----- PROPERTIES ----- */

    /// <summary>
    /// logged in user
    /// </summary>
    public User _user;

    /// <summary>
    /// gets the title of the grocery list page
    /// </summary>
    public string? Title { get; }

    /// <summary>
    /// gets the grocery lists the user has from the db
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<GroceryList> _groceryLists = new();

    /// <summary>
    /// gets the first grocery list
    /// </summary>
    [ObservableProperty]
    private GroceryList? _selectedGroceryList;

    /// <summary>
    /// get food items written down in the grocery list 
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<GroceryItem> _items = new();

    /// <summary>
    /// get user's input keyword from search bar
    /// </summary>
    [ObservableProperty]
    private string _searchBox;

    /// <summary>
    /// get current date
    /// </summary>
    [ObservableProperty]
    private string _currentDate;

    /// <summary>
    /// get input name of Grocery list from Popup
    /// </summary>
    [ObservableProperty]
    public string _newGroceryListName;


    /* ----- Error management ----- */

    /// <summary>
    /// get visibility of AddGroceryList error in Popup
    /// </summary>
    [ObservableProperty]
    public Visibility _addGroceryListError = Visibility.Collapsed;

    /// <summary>
    /// get message of error in Popup
    /// </summary>
    [ObservableProperty]
    public string _errorMessage;


    /* ----- Loading user's Grocery list data ----- */

    /// <summary>
    /// When selected grocery list is changed, display Items according to that grocery list
    /// If no grocery list can be selected, hide Ingredient controls and show placeholder header
    /// </summary>
    /// <param name="value">Currently selected grocery list</param>
    partial void OnSelectedGroceryListChanged(GroceryList? value)
    {
        if (value != null)
        {
            Items = new ObservableCollection<GroceryItem>(value.Items);
            IngredientControls = Visibility.Visible;
            PlaceholderLocationHeader = Visibility.Collapsed;
        }
        else
        {
            IngredientControls = Visibility.Collapsed;
            PlaceholderLocationHeader = Visibility.Visible;
        }
    }

    /// <summary>
    /// sets grocery lists from db
    /// </summary>
    /// <returns>a task representing the asynchronous operation</returns>
    public async Task LoadAsync()
    {
        if (SelectedGroceryList != null)
            Items = new ObservableCollection<GroceryItem>(SelectedGroceryList.Items);
    }

    /* ----- Controls for location Items ----- */

    /// <summary>
    /// get Visibility for Ingredient Control buttons (Add ingredient, Remove all spoilage)
    /// </summary>
    [ObservableProperty]
    public Visibility _ingredientControls = Visibility.Collapsed;

    /// <summary>
    /// get Visibility for placeholder header ("Select a location")
    /// </summary>
    [ObservableProperty]
    public Visibility _placeholderLocationHeader = Visibility.Visible;

    /// <summary>
    /// gets the command to delete one item
    /// </summary>
    public ICommand DeleteOne { get; }

    /// <summary>
    /// gets the command to delete all checked item
    /// </summary>
    public ICommand DeleteAllChecked { get; }

    /// <summary>
    /// gets the command to check item
    /// </summary>
    public ICommand CheckItem { get; }

    /// <summary>
    /// get boolean value for PointerMode (Whether clickable elements can be interacted or not)
    /// </summary>
    [ObservableProperty]
    public bool _pointerMode = true;

    /// <summary>
    /// delete all checked items from grocery list
    /// </summary>
    /// <returns>a task representing the asynchronous operation</returns>
    private async Task RemoveAllChecked()
    {
        if (SelectedGroceryList != null)
        {
            foreach (GroceryItem item in Items)
            {
                if (item.IsChecked)
                {
                    _groceryItemRepo.RemoveItemFromGroceryList(SelectedGroceryList.GroceryListId, item.FoodItemId);
                    Items = new ObservableCollection<GroceryItem>(SelectedGroceryList.Items);
                }
            }
        }
    }

    /// <summary>
    /// delete selected item from grocery list
    /// </summary>
    /// References:
    /// https://learn.microsoft.com/en-sg/answers/questions/1622390/get-a-value-of-the-row-or-item-in-listview-using-a
    /// <returns>a task representing the asynchronous operation</returns>
    private async Task RemoveOne(object o)
    {
        var item = o as GroceryItem;
        if (SelectedGroceryList != null)
        {
            _groceryItemRepo.RemoveItemFromGroceryList(SelectedGroceryList.GroceryListId, item.FoodItemId);
            Items = new ObservableCollection<GroceryItem>(SelectedGroceryList.Items);
        }
    }

    /// <summary>
    /// check selected item from grocery list
    /// </summary>
    /// References:
    /// https://learn.microsoft.com/en-sg/answers/questions/1622390/get-a-value-of-the-row-or-item-in-listview-using-a
    /// <returns>a task representing the asynchronous operation</returns>
    private async Task CheckCurrentItem(object o)
    {
        var item = o as GroceryItem;
        if (SelectedGroceryList != null)
        {
            item.IsChecked = !item.IsChecked;
            _groceryItemRepo.UpdateGroceryItem(item);
            Items = new ObservableCollection<GroceryItem>(SelectedGroceryList.Items);
        }
    }

    /* ----- Navigation ----- */

    /// <summary>
    /// gets the command that navigates to the add grocery item view
    /// </summary>
    public ICommand GoToAddFoodItem { get; }

    /// <summary>
    /// gets the command to go to edit item page
    /// </summary>
    public ICommand GoToEditItem { get; }

    /// <summary>
    /// navigates to the add grocery item view
    /// </summary>
    /// <returns>a task representing the asynchronous operation</returns>
    private async Task GoToAddFoodItemView()
    {
        await _navigator.NavigateViewModelAsync<AddGroceryItemViewModel>(this, data: SelectedGroceryList);
    }

    /// <summary>
    /// navigates to the edit item view
    /// </summary>
    /// <returns>a task representing the asynchronous operation</returns>
    private async Task GoToEditItemView(object o)
    {
        var item = o as FoodItem;
        await _navigator.NavigateViewModelAsync<EditItemViewModel>(this, data: item);
    }

    /* ----- Items sorting and searching ----- */

    /// <summary>
    /// gets command that resets LocationItems list
    /// </summary>
    public ICommand SearchCommand { get; }

    /// <summary>
    /// gets command that resets the LocationItems list
    /// </summary>
    public ICommand ResetListCommand { get; }

    /// <summary>
    /// gets command that sorts FoodItems by Name
    /// </summary>
    public ICommand SortByNameCommand { get; }

    /// <summary>
    /// gets command that sorts FoodItems by Quantity
    /// </summary>
    public ICommand SortByQuantityCommand { get; }


    /// <summary>
    /// Sorts FoodItems by Name
    /// </summary>
    public void SortByName()
    {
        ISortMethod<FoodItem> sortMethod = new SortByName();
        List<FoodItem> original = CreateFoodItemsList_FromGroceryItemsList(Items);
        List<FoodItem> foodItems = new List<FoodItem>(original);

        foodItems = sortMethod.SortAscending(foodItems);
        if (original.SequenceEqual(foodItems))
        {
            foodItems = sortMethod.SortDescending(foodItems);
        }

        List<GroceryItem> sort = CreateGroceryItemList_FromFoodItemList(foodItems);

        ObservableCollection<GroceryItem> updated = new ObservableCollection<GroceryItem>(sort);
        Items = updated;
    }

    /// <summary>
    /// Sorts FoodItems by Quantity
    /// </summary>
    public void SortByQuantity()
    {
        ISortMethod<FoodItem> sortMethod = new SortByQuantity();
        List<FoodItem> original = CreateFoodItemsList_FromGroceryItemsList(Items);
        List<FoodItem> foodItems = new List<FoodItem>(original);

        foodItems = sortMethod.SortAscending(foodItems);
        if (original.SequenceEqual(foodItems))
        {
            foodItems = sortMethod.SortDescending(foodItems);
        }

        List<GroceryItem> sort = CreateGroceryItemList_FromFoodItemList(foodItems);

        ObservableCollection<GroceryItem> updated = new ObservableCollection<GroceryItem>(sort);
        Items = updated;
    }

    /// <summary>
    /// Helper Method for Sort methods
    /// Creates a FoodItems list from a GroceryItems ObservableCollection
    /// </summary>
    public List<FoodItem> CreateFoodItemsList_FromGroceryItemsList(ObservableCollection<GroceryItem> groceryItems)
    {
        List<FoodItem> foodItems = new List<FoodItem>();
        foreach (GroceryItem foodItem in groceryItems)
        {
            foodItems.Add(foodItem);
        }
        return foodItems;
    }

    /// <summary>
    /// Helper Method for Sort methods
    /// Creates a GroceryItem list from a FoodItem list
    /// </summary>
    public List<GroceryItem> CreateGroceryItemList_FromFoodItemList(List<FoodItem> foodItems)
    {
        List<GroceryItem> groceryItems = new List<GroceryItem>();
        foreach (GroceryItem groceryItem in foodItems)
        {
            groceryItems.Add(groceryItem);
        }
        return groceryItems;
    }

    /// <summary>
    /// Searches in GroceryItems list by keyword
    /// </summary>
    public void Search()
    {
        SearchLogic search = new SearchLogic();
        if (SelectedGroceryList != null)
            Items = search.SearchGrocery(new ObservableCollection<GroceryItem>(SelectedGroceryList.Items), SearchBox);
    }

    /// <summary>
    /// Resets GroceryItems list
    /// </summary>
    public void ResetList()
    {
        Items = new ObservableCollection<GroceryItem>(SelectedGroceryList.Items);
    }
}
