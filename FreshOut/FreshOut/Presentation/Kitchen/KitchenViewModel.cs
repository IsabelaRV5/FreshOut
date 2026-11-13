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

namespace GUI.Presentation.Kitchen;

/// <summary>
/// this class is the ViewModel for the KitchenPage
/// it provides navigation to the AddLocationItemPage, the ability to remove all spoiled items, the ability to create new food locations, and to search within each food location
/// </summary>
/// <remarks>
/// author: Jiha Poitevin & Isabela Resende Vasconcelos & Xena Tat
/// date: 17.05.2026
/// </remarks>
public partial class KitchenViewModel : ObservableObject
{
    private readonly INavigator _navigator;
    public readonly ILocationListRepository _locationRepo;
    private readonly ILocationItemRepository _locationItemRepo;

    /// <summary>
    /// initializes a new instance of the KitchenViewModel class
    /// </summary>
    /// <param name="navigator">the navigation service used to switch between view models</param>
    /// <param name="locationRepo">the location list repository</param>
    /// <param name="locationItemRepo">the location items repository</param>
    /// <param name="user">the logged in user</param>
    public KitchenViewModel(INavigator navigator, ILocationListRepository locationRepo, ILocationItemRepository locationItemRepo, User user)
    {
        _navigator = navigator;
        _locationRepo = locationRepo;
        _locationItemRepo = locationItemRepo;
        _user = user;

        Title = "Kitchen";
        GoToAddFoodItem = new AsyncRelayCommand(GoToAddFoodItemView);
        DeleteAll = new AsyncRelayCommand(RemoveSpoilage);
        DeleteOne = new AsyncRelayCommand<object>(RemoveOne);
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
        SortByDateCommand = new RelayCommand(SortByDate);

        LocationLists = new ObservableCollection<LocationList>(_locationRepo.GetUserLocationLists(_user.UserId));
        if (LocationLists.Count > 0)
        {
            SelectedLocation = LocationLists[0];
            Items = new ObservableCollection<LocationItem>(SelectedLocation.Items);
        }
    }


    /* ----- PROPERTIES ----- */

    /// <summary>
    /// logged in user
    /// </summary>
    public User _user;

    /// <summary>
    /// gets the title of the kitchen page
    /// </summary>
    public string? Title { get; }

    /// <summary>
    /// gets the lists of locations the user has from the db
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<LocationList> _locationLists = new ();

    /// <summary>
    /// gets the first location (fridge)
    /// </summary>
    [ObservableProperty]
    private LocationList? _selectedLocation;

    /// <summary>
    /// get food items stored in the selected location 
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<LocationItem> _items = new();

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
    /// get input name of Location from Popup
    /// </summary>
    [ObservableProperty]
    public string _newLocationName;  


    /* ----- Error management ----- */

    /// <summary>
    /// get message of error in Popup
    /// </summary>
    [ObservableProperty]
    public string _errorMessage;

    /// <summary>
    /// get visibility of AddLocation error in Popup
    /// </summary>
    [ObservableProperty]
    public Visibility _addLocationError = Visibility.Collapsed;


    /* ----- Loading user's Kitchen data ----- */

    /// <summary>
    /// When selected location is changed, display Items according to that location
    /// If no location can be selected, hide Ingredient controls and show placeholder header
    /// </summary>
    /// <param name="value">Currently selected location</param>
    partial void OnSelectedLocationChanged(LocationList? value)
    {
        if (value != null)
        {
            Items = new ObservableCollection<LocationItem>(value.Items);
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
    /// sets location lists from db
    /// </summary>
    /// <returns>a task representing the asynchronous operation</returns>
    public async Task LoadAsync()
    {
        if (SelectedLocation != null)
            Items = new ObservableCollection<LocationItem>(SelectedLocation.Items);
    }


    /* ----- Controls for location Items ----- */

    /// <summary>
    /// get Visibility for Ingredient Control buttons (Add ingredient, Remove all spoilage)
    /// </summary>
    [ObservableProperty]
    public Visibility _ingredientControls = Visibility.Collapsed;

    /// <summary>
    /// gets the command to delete all spoiled items
    /// </summary>
    public ICommand DeleteAll { get; set; }

    /// <summary>
    /// gets the command to delete one item
    /// </summary>
    public ICommand DeleteOne { get; }

    /// <summary>
    /// get Visibility for placeholder header ("Select a location")
    /// </summary>
    [ObservableProperty]
    public Visibility _placeholderLocationHeader = Visibility.Visible;

    /// <summary>
    /// get boolean value for PointerMode (Whether clickable elements can be interacted or not)
    /// </summary>
    [ObservableProperty]
    public bool _pointerMode = true;

    /// <summary>
    /// delete all spoiled items from location
    /// </summary>
    /// <returns>a task representing the asynchronous operation</returns>
    private async Task RemoveSpoilage()
    {
        if (SelectedLocation != null)
        {
            _locationRepo.DeleteSpoilageFromLocationList(SelectedLocation.LocationListId);
            Items = new ObservableCollection<LocationItem>(SelectedLocation.Items);
        }
    }

    /// <summary>
    /// delete selected item from location
    /// </summary>
    /// References:
    /// https://learn.microsoft.com/en-sg/answers/questions/1622390/get-a-value-of-the-row-or-item-in-listview-using-a
    /// <returns>a task representing the asynchronous operation</returns>
    private async Task RemoveOne(object o)
    {
        var item = o as LocationItem;
        if (SelectedLocation != null)
        {
            _locationItemRepo.RemoveItemFromLocation(SelectedLocation.LocationListId, item.FoodItemId);
            Items = new ObservableCollection<LocationItem>(SelectedLocation.Items);
        }
    }

    /* ----- Navigation ----- */

    /// <summary>
    /// gets the command that navigates to the add ingredient view
    /// </summary>
    public ICommand GoToAddFoodItem { get; }


    /// <summary>
    /// gets the command to go to edit item page
    /// </summary>
    public ICommand GoToEditItem { get; }

    /// <summary>
    /// navigates to the add ingredient view
    /// </summary>
    /// <returns>a task representing the asynchronous operation</returns>
    private async Task GoToAddFoodItemView()
    {
        await _navigator.NavigateViewModelAsync<AddLocationItemViewModel>(this, data: SelectedLocation);
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
    /// gets command that sorts LocationItems by Date
    /// </summary>
    public ICommand SortByDateCommand { get; }


    /// <summary>
    /// Sorts FoodItems by Name
    /// </summary>
    public void SortByName()
    {
        ISortMethod<FoodItem> sortMethod = new SortByName();
        List<FoodItem> original = CreateFoodItemsList_FromLocationItemsList(Items);
        List<FoodItem> foodItems = new List<FoodItem>(original);

        foodItems = sortMethod.SortAscending(foodItems);
        if(original.SequenceEqual(foodItems))
        {
            foodItems = sortMethod.SortDescending(foodItems);
        }
        
        List<LocationItem> sort = CreateLocationItemList_FromFoodItemList(foodItems);

        ObservableCollection<LocationItem> updated = new ObservableCollection<LocationItem>(sort);
        Items = updated;
    }

    /// <summary>
    /// Sorts FoodItems by Quantity
    /// </summary>
    public void SortByQuantity()
    {
        ISortMethod<FoodItem> sortMethod = new SortByQuantity();
        List<FoodItem> original = CreateFoodItemsList_FromLocationItemsList(Items);
        List<FoodItem> foodItems = new List<FoodItem>(original);

        foodItems = sortMethod.SortAscending(foodItems);
        if (original.SequenceEqual(foodItems))
        {
            foodItems = sortMethod.SortDescending(foodItems);
        }

        List<LocationItem> sort = CreateLocationItemList_FromFoodItemList(foodItems);

        ObservableCollection<LocationItem> updated = new ObservableCollection<LocationItem>(sort);
        Items = updated;
    }


    /// <summary>
    /// Sorts LocationItems by Date
    /// </summary>
    public void SortByDate()
    {
        ISortMethod<LocationItem> sortMethod = new SortByDate();

        List<LocationItem> original = new List<LocationItem>(Items);
        List<LocationItem> foodItems = new List<LocationItem>(Items);

        foodItems = sortMethod.SortAscending(foodItems);
        if (original.SequenceEqual(foodItems))
        {
            foodItems = sortMethod.SortDescending(foodItems);
        }

        ObservableCollection<LocationItem> updated = new ObservableCollection<LocationItem>(foodItems);
        Items = updated;
    }

    /// <summary>
    /// Helper Method for Sort methods
    /// Creates a FoodItems list from a LocationItems ObservableCollection
    /// </summary>
    public List<FoodItem> CreateFoodItemsList_FromLocationItemsList(ObservableCollection<LocationItem> locationItems)
    {
        List<FoodItem> foodItems = new List<FoodItem>();
        foreach (LocationItem foodItem in locationItems)
        {
            foodItems.Add(foodItem);
        }
        return foodItems;
    }

    /// <summary>
    /// Helper Method for Sort methods
    /// Creates a LocationItem list from a FoodItem list
    /// </summary>
    public List<LocationItem> CreateLocationItemList_FromFoodItemList(List<FoodItem> foodItems)
    {
        List<LocationItem> locationItems = new List<LocationItem>();
        foreach (LocationItem locationItem in foodItems)
        {
            locationItems.Add(locationItem);
        }
        return locationItems;
    }

    /// <summary>
    /// Searches in LocationItems list by keyword
    /// </summary>
    public void Search()
    {
        SearchLogic search = new SearchLogic();
        if (SelectedLocation != null)
            Items = search.SearchKitchen(new ObservableCollection<LocationItem>(SelectedLocation.Items), SearchBox);
    }

    /// <summary>
    /// Resets LocationItems list
    /// </summary>
    public void ResetList()
    {
        Items = new ObservableCollection<LocationItem>(SelectedLocation.Items);
    }
}
