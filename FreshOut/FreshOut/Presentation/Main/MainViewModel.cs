using Data.Entities;
using GUI.Presentation;
using Windows.System;
using User = Data.Entities.User;
using GUI.Presentation.GroceryLists;
using GUI.Presentation.Kitchen;
using GUI.Presentation.Login;
using Data.Repositories.Interfaces;

namespace GUI.Presentation.Main;

/// <summary>
/// this class is the ViewModel for the MainPage
/// it provides navigation to core application sections such as the kitchen page, the grocery lists page, and the login page
/// </summary>
/// <remarks>
/// author: Jiha Poitevin
/// date: 05.05.2026
/// </remarks>
public partial class MainViewModel : ObservableObject
{
    private readonly INavigator _navigator;
    private User _user;

    /// <summary>
    /// initializes a new instance of the MainViewModel class
    /// </summary>
    /// <param name="navigator">the navigation service used to switch between view models</param>
    public MainViewModel(INavigator navigator, User user)
    {
        _navigator = navigator;
        _user = user;

        Title = "Main";
        Username = user.Username;
        ProfilePic = user.IconPath;
        GoToKitchen = new AsyncRelayCommand(GoToKitchenView);
        GoToGroceryLists = new AsyncRelayCommand(GoToGroceryListsView);
        GoToLogin = new AsyncRelayCommand(LogOut);
    }



    /* ----- PROPERTIES ----- */

    /// <summary>
    /// gets the title of the main page
    /// </summary>
    public string? Title { get; }

    /// <summary>
    /// gets user's username
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// gets the user's profile pic
    /// </summary>
    public string ProfilePic { get; }



    /* ----- NAVIGATION ----- */

    /// <summary>
    /// gets the command that navigates to the kitchen view
    /// </summary>
    public ICommand GoToKitchen { get; }

    /// <summary>
    /// gets the command that navigates to the grocery lists view
    /// </summary>
    public ICommand GoToGroceryLists { get; }

    /// <summary>
    /// gets the command that navigates to the login view
    /// </summary>
    public ICommand GoToLogin { get; }

    /// <summary>
    /// navigates to the kitchen view
    /// </summary>
    /// <returns>a task representing the asynchronous operation</returns>
    private async Task GoToKitchenView()
    {
        await _navigator.NavigateViewModelAsync<KitchenViewModel>(this, data: _user);
    }

    /// <summary>
    /// navigates to the grocery lists view
    /// </summary>
    /// <returns>a task representing the asynchronous operation</returns>
    private async Task GoToGroceryListsView()
    {
        await _navigator.NavigateViewModelAsync<GroceryListsViewModel>(this, data: _user);
    }

    /// <summary>
    /// logs out the current user and navigates to the login view 
    /// </summary>
    /// <returns>a task representing the asynchronous operation</returns>
    private async Task LogOut()
    {
        await _navigator.NavigateViewModelAsync<LoginViewModel>(this, qualifier: Qualifiers.ClearBackStack);
    }
}
