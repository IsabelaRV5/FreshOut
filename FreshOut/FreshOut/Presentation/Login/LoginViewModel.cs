using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data;
using Data.Entities;
using Data.Repositories.Database;
using Data.Repositories.Interfaces;
using GUI.Presentation;
using GUI.Presentation.Main;
using GUI.Presentation.Registration;
using Logic;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Uno.Extensions.Navigation;

namespace GUI.Presentation.Login;

/// <summary>
/// this class is the ViewModel for the LoginPage
/// it handles user input for authentication and navigation to the main page and to the registration page
/// </summary>
/// <remarks>
/// author: Jiha Poitevin & Xena Tat
/// date: 18.05.2026
/// </remarks>
public partial class LoginViewModel : ObservableValidator
{
    private readonly INavigator _navigator;
    private readonly IUserRepository _userRepo;
    private User _user;

    /// <summary>
    /// initializes a new instance of the LoginViewModel class
    /// </summary>
    /// <param name="navigator">the navigation service used to switch between view models</param>
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    public LoginViewModel(INavigator navigator, IUserRepository userRepo)
    {
        _navigator = navigator;
        _userRepo = userRepo;
        Title = "Login";
        LoginAndGoToMain = new RelayCommand(VerifyUser);
        GoToRegistration = new AsyncRelayCommand(GoToRegistrationView);
    }



    /* ----- PROPERTIES ----- */

    /// <summary>
    /// gets the title of the registration page
    /// </summary>
    public string? Title { get; }

    /// <summary>
    /// gets or sets the username entered by the user
    /// </summary>
    [ObservableProperty]
    [Required(ErrorMessage = "Username is required!")]
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    private string? _username;

    /// <summary>
    /// gets or sets the password entered by the user
    /// </summary>
    [ObservableProperty]
    [Required(ErrorMessage = "Password is required!")]
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    private string? _password;

    /// <summary>
    /// gets or sets the error message for invalid credentials
    /// </summary>
    [ObservableProperty]
    private Visibility _errorMessage = Visibility.Collapsed;


    /// <summary>
    /// gets the validation error message for empty inputs
    /// </summary>
    public string? _emptyInputs;
    public string? EmptyInputs
    {
        get => _emptyInputs;
        set => SetProperty(ref _emptyInputs, value);
    }



    /* ----- ERROR PROPERTIES FOR UI BINDING ----- */

    /// <summary>
    /// retrieves the first validation error message for a given property
    /// </summary>
    /// <param name="propertyName">the name of the property to check for validation errors</param>
    /// <returns>the first error message as a string, or null if no errors exist</returns>
    private string? GetFirstError(string propertyName) => GetErrors(propertyName).Cast<object>().FirstOrDefault()?.ToString();

    /// <summary>
    /// gets the validation error message for the username input
    /// </summary>
    public string? UsernameError => GetFirstError(nameof(Username));

    /// <summary>
    /// gets the validation error message for the password input
    /// </summary>
    public string? PasswordError => GetFirstError(nameof(Password));

    /// <summary>
    /// gets the validation error message for login failure
    /// </summary>
    private string? _loginError;
    public string? LoginError
    {
        get => _loginError;
        set => SetProperty(ref _loginError, value);
    }



    /* ----- LIVE VALIDATION ----- */

    /// <summary>
    /// triggers username validation
    /// </summary>
    /// <param name="value">user's username</param>
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    partial void OnUsernameChanged(string? value)
    {
        ValidateProperty(value, nameof(Username));
        ClearGeneralErrors();
        OnPropertyChanged(nameof(UsernameError));
    }

    /// <summary>
    /// triggers password validation
    /// </summary>
    /// <param name="value">user's password</param>
    [RequiresUnreferencedCode("Calls CommunityToolkit.Mvvm.ComponentModel.ObservableValidator.ValidateProperty(Object, String)")]
    partial void OnPasswordChanged(string? value)
    {
        ValidateProperty(value, nameof(Password));
        ClearGeneralErrors();
        OnPropertyChanged(nameof(PasswordError));
    }

    /// <summary>
    /// Clears all error messages
    /// </summary>
    private void ClearGeneralErrors()
    {
        EmptyInputs = null;
        LoginError = null;
    }



    /* ----- MAIN LOGIC ----- */

    /// <summary>
    /// validates credentials
    /// </summary>
    public void VerifyUser()
    {
        UserManager manager = new UserManager(_userRepo);
        User user = manager.ValidateCredentials(_username, _password);
        if (user != null)
        {
            _user = user;
            ErrorMessage = Visibility.Collapsed;
            Login();
        }
        ErrorMessage = Visibility.Visible;
    }


    /// <summary>
    /// gets the command that navigates to the main view after an authenticated login
    /// </summary>
    public ICommand LoginAndGoToMain { get; }

    /// <summary>
    /// attempts to execute a login, navigates to the main view and clears the navigation back stack if sucessful
    /// </summary>
    /// <returns>a task representing the asynchronous operation</returns>
    [RelayCommand(CanExecute = nameof(CanLogin))]
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    private async Task Login()
    {
        ClearGeneralErrors();
        ValidateAllProperties();
        OnPropertyChanged(nameof(UsernameError));
        OnPropertyChanged(nameof(PasswordError));

        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            EmptyInputs = "Please fill out all the required fields!";
            return;
        }

        if (HasErrors)
            return;

        await _navigator.NavigateViewModelAsync<MainViewModel>(this, qualifier: Qualifiers.ClearBackStack, data: _user);
    }

    /// <summary>
    /// boolean indicating whether there are errors or not in login page
    /// </summary>
    /// <returns>
    /// true/false based on if there's errors present
    /// --------------------------------------------------
    /// true - there are errors, therefore user cannot log in
    /// false - there are no errors, therefore user can log in
    /// </returns>
    private bool CanLogin() => !HasErrors;
    


    /* ----- NAVIGATION ----- */

    /// <summary>
    /// gets the command that navigates to the registration view
    /// </summary>
    public ICommand GoToRegistration { get; }

    /// <summary>
    /// navigates to the registration view and clears the navigation back stack
    /// </summary>
    /// <returns>a task representing the asynchronous operation</returns>
    private async Task GoToRegistrationView()
    {
        await _navigator.NavigateViewModelAsync<RegistrationViewModel>(this, qualifier: Qualifiers.ClearBackStack);
    }
}
