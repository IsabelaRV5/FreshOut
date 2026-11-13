using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;
using Data.Repositories.Database;
using Data.Repositories.Interfaces;
using GUI.Presentation;
using GUI.Presentation.Login;
using Logic;
using Uno.Extensions.Navigation;

namespace GUI.Presentation.Registration;

/// <summary>
/// this class is the ViewModel for the RegistrationPage
/// it handles user input for registration and navigation to the login page after a valid registration
/// </summary>
/// <remarks>
/// author: Jiha Poitevin
/// date: 27.04.2026
/// </remarks>
public partial class RegistrationViewModel : ObservableValidator
{
    private readonly INavigator _navigator;
    private readonly UserManager _userManager;
    private readonly IUserRepository _userRepo;
    private readonly ILocationListRepository _locationRepo;

    /// <summary>
    /// initializes a new instance of the RegistrationViewModel class
    /// </summary>
    /// <param name="navigator">the navigation service used to switch between view models</param>
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    public RegistrationViewModel(INavigator navigator, IUserRepository userRepo, ILocationListRepository locationRepo)
    {
        _navigator = navigator;
        _userRepo = userRepo;
        _locationRepo = locationRepo;
        _userManager = new UserManager(_userRepo);
        Title = "Registration";
        RegisterAndGoToLogin = new AsyncRelayCommand(Register);
        GoToLogin = new AsyncRelayCommand(GoToLoginView);
        SelectedProfilePic = "Green";

        ErrorsChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(UsernameError));
            OnPropertyChanged(nameof(PasswordError));
            OnPropertyChanged(nameof(PasswordConfirmError));
        };
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
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters!")]
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    private string? _username;

    /// <summary>
    /// gets or sets the profile pic chosen by the user
    /// </summary>
    [ObservableProperty]
    private string? _selectedProfilePic;

    /// <summary>
    /// gets or sets the password entered by the user
    /// </summary>
    [ObservableProperty]
    [Required(ErrorMessage = "Password is required!")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters!")]
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    private string? _password;

    /// <summary>
    /// gets or sets the password confirmation entered by the user
    /// </summary>
    [ObservableProperty]
    [Required(ErrorMessage = "Please confirm your password!")]
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    private string? _passwordConfirm;

    /// <summary>
    /// property used only for password comparison validation
    /// </summary>
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match!")]
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    public string? ConfirmPasswordValidation => PasswordConfirm;



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
    /// gets the validation error message for the password confirmation input, including both required and password match validation
    /// </summary>
    public string? PasswordConfirmError => GetFirstError(nameof(PasswordConfirm)) ?? GetFirstError(nameof(ConfirmPasswordValidation));

    /// <summary>
    /// gets the validation error message for empty inputs
    /// </summary>
    public string? _emptyInputs;
    public string? EmptyInputs
    {
        get => _emptyInputs;
        set => SetProperty(ref _emptyInputs, value);
    }

    /// <summary>
    /// gets the validation error message for registration failure
    /// </summary>
    private string? _registrationError;
    public string? RegistrationError
    {
        get => _registrationError;
        set => SetProperty(ref _registrationError, value);
    }



    /* ----- LIVE VALIDATION ----- */

    /// <summary>
    /// triggers validation of username live as the user types
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
    /// triggers revalidation of the password confirmation when the password changes
    /// </summary>
    /// <param name="value">user's password</param>
    [RequiresUnreferencedCode("Calls CommunityToolkit.Mvvm.ComponentModel.ObservableValidator.ValidateProperty(Object, String)")]
    partial void OnPasswordChanged(string? value)
    {
        ValidateProperty(value, nameof(Password));
        ValidateProperty(PasswordConfirm, nameof(PasswordConfirm));

        if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(PasswordConfirm))
            ValidateProperty(PasswordConfirm, nameof(ConfirmPasswordValidation));
        else
            ClearErrors(nameof(ConfirmPasswordValidation));

        ClearGeneralErrors();

        OnPropertyChanged(nameof(PasswordError));
        OnPropertyChanged(nameof(PasswordConfirmError));
    }

    /// <summary>
    /// triggers validation of password confirmation live as the user types
    /// </summary>
    /// <param name="value">user's password confirmation</param>
    [RequiresUnreferencedCode("Calls CommunityToolkit.Mvvm.ComponentModel.ObservableValidator.ValidateProperty(Object, String)")]
    partial void OnPasswordConfirmChanged(string? value)
    {
        ValidateProperty(value, nameof(PasswordConfirm));

        if (!string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(value))
            ValidateProperty(value, nameof(ConfirmPasswordValidation));
        else
            ClearErrors(nameof(ConfirmPasswordValidation));

        ClearGeneralErrors();

        OnPropertyChanged(nameof(PasswordConfirmError));
    }

    /// <summary>
    /// clears all error messages
    /// </summary>
    private void ClearGeneralErrors()
    {
        EmptyInputs = null;
        RegistrationError = null;
    }



    /* ----- MAIN LOGIC ----- */

    /// <summary>
    /// list of profile pics
    /// </summary>
    public List<string> ProfilePics { get; } = new()
    {
        "Green",
        "Blue",
        "Red",
        "Orange",
        "Purple",
        "Cyan"
    };

    /// <summary>
    /// gets the command that registers the user and navigates to the login page
    /// </summary>
    public ICommand RegisterAndGoToLogin { get; }

    /// <summary>
    /// attempts to execute the registration process and navigates to the login page if successful
    /// </summary>
    /// <returns>a task representing the asynchronous operation</returns>
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    private async Task Register()
    {
        try
        {
            ClearGeneralErrors();
            ValidateAllProperties();
            OnPropertyChanged(nameof(UsernameError));
            OnPropertyChanged(nameof(PasswordError));
            OnPropertyChanged(nameof(PasswordConfirmError));

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(PasswordConfirm))
            {
                EmptyInputs = "Please fill out all the required fields!";
                return;
            }

            if (HasErrors)
                return;

            bool success = _userManager.RegisterUser(Username!, Password!, $"/Assets/ProfilePics/{SelectedProfilePic}.png");
            if (!success)
            {
                RegistrationError = "Username already exists!";
                return;
            }

            // Add default Fridge location list to all registered users
            User user = _userRepo.GetUserByUsername(Username);
            LocationList fridge = new LocationList { Name = "Fridge", User = user, UserId = user.UserId };
            _locationRepo.AddLocationList(fridge);

            await _navigator.NavigateViewModelAsync<LoginViewModel>(this, qualifier: Qualifiers.ClearBackStack);
        }
        catch (Exception ex)
        {
            RegistrationError = $"Something went wrong: {ex.Message}";
        }
    }



    /* ----- NAVIGATION ----- */

    /// <summary>
    /// gets the command that navigates to the login view
    /// </summary>
    public ICommand GoToLogin { get; }

    /// <summary>
    /// navigates to the login view and clears the navigation back stack
    /// </summary>
    /// <returns>a task representing the asynchronous operation</returns>
    private async Task GoToLoginView()
    {
        await _navigator.NavigateViewModelAsync<LoginViewModel>(this, qualifier: Qualifiers.ClearBackStack);
    }
}
