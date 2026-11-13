using Data;
using Data.Entities;
using Data.Repositories.Database;
using Data.Repositories.Interfaces;
using GUI;
using GUI.Presentation;
using GUI.Presentation.EditItem;
using GUI.Presentation.AddFoodItem;
using GUI.Presentation.GroceryLists;
using GUI.Presentation.Kitchen;
using GUI.Presentation.Login;
using GUI.Presentation.Main;
using GUI.Presentation.Registration;
using Logic;
using Uno.Resizetizer;

namespace FreshOut;

public partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object. This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    protected Window? MainWindow { get; private set; }
    protected IHost? Host { get; private set; }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            // Add navigation support for toolkit controls such as TabBar and NavigationView
            .UseToolkitNavigation()
            .Configure(host => host
#if DEBUG
                // Switch to Development environment when running in DEBUG
                .UseEnvironment(Environments.Development)
#endif
                .UseLogging(configure: (context, logBuilder) =>
                {
                    // Configure log levels for different categories of logging
                    logBuilder
                        .SetMinimumLevel(
                            context.HostingEnvironment.IsDevelopment() ?
                                LogLevel.Information :
                                LogLevel.Warning)

                        // Default filters for core Uno Platform namespaces
                        .CoreLogLevel(LogLevel.Warning);

                    // Uno Platform namespace filter groups
                    // Uncomment individual methods to see more detailed logging
                    //// Generic Xaml events
                    //logBuilder.XamlLogLevel(LogLevel.Debug);
                    //// Layout specific messages
                    //logBuilder.XamlLayoutLogLevel(LogLevel.Debug);
                    //// Storage messages
                    //logBuilder.StorageLogLevel(LogLevel.Debug);
                    //// Binding related messages
                    //logBuilder.XamlBindingLogLevel(LogLevel.Debug);
                    //// Binder memory references tracking
                    //logBuilder.BinderMemoryReferenceLogLevel(LogLevel.Debug);
                    //// DevServer and HotReload related
                    //logBuilder.HotReloadCoreLogLevel(LogLevel.Information);
                    //// Debug JS interop
                    //logBuilder.WebAssemblyLogLevel(LogLevel.Debug);

                }, enableUnoLogging: true)
                .UseConfiguration(configure: configBuilder =>
                    configBuilder
                        .EmbeddedSource<App>()
                        .Section<AppConfig>()
                )
                // Enable localization (see appsettings.json for supported languages)
                .UseLocalization()
                .UseHttp((context, services) =>
                {
#if DEBUG
                    // DelegatingHandler will be automatically injected
                    services.AddTransient<DelegatingHandler, DebugHttpHandler>();
#endif

                })
                .ConfigureServices((context, services) =>
                {
                    services.AddScoped<IFoodItemRepository, DBFoodItemRepository>();
                    services.AddScoped<IGroceryItemRepository, DBGroceryItemRepository>();
                    services.AddScoped<IGroceryListRepository, DBGroceryListRepository>();
                    services.AddScoped<ILocationItemRepository, DBLocationItemRepository>();
                    services.AddScoped<ILocationListRepository, DBLocationListRepository>();
                    services.AddScoped<IUserRepository, DBUserRepository>();
                    services.AddDbContext<FreshOutContext>();

                })
                .UseNavigation(RegisterRoutes)
            );
        MainWindow = builder.Window;

#if DEBUG
        MainWindow.UseStudio();
#endif
        MainWindow.SetWindowIcon();

        Host = await builder.NavigateAsync<Shell>();
    }

    private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
    {
        views.Register(
            new ViewMap(ViewModel: typeof(ShellViewModel)),
            new ViewMap<LoginPage, LoginViewModel>(),
            new ViewMap<RegistrationPage, RegistrationViewModel>(),
            new DataViewMap<MainPage, MainViewModel, User>(),
            new DataViewMap<KitchenPage, KitchenViewModel, User>(),
            new DataViewMap<GroceryListsPage, GroceryListsViewModel, User>(),
            new DataViewMap<EditItemPage, EditItemViewModel, FoodItem>(),
            new DataViewMap<AddLocationItemPage, AddLocationItemViewModel, LocationList>(),
            new DataViewMap<AddGroceryItemPage, AddGroceryItemViewModel, GroceryList>()
        );

        routes.Register(
            new RouteMap("", View: views.FindByViewModel<ShellViewModel>(),
                Nested:
                [
                    new ("Login", View: views.FindByViewModel<LoginViewModel>(), IsDefault:true),
                    new ("Registration", View: views.FindByViewModel<RegistrationViewModel>()),
                    new ("Main", View: views.FindByViewModel<MainViewModel>()),
                    new ("Kitchen", View: views.FindByViewModel<KitchenViewModel>()),
                    new ("Grocery Lists", View: views.FindByViewModel<GroceryListsViewModel>()),
                    new ("Edit Item", View: views.FindByViewModel<EditItemViewModel>()),
                    new ("Add Location Item", View: views.FindByViewModel<AddLocationItemViewModel>()),
                    new ("Add Grocery Item", View: views.FindByViewModel<AddGroceryItemViewModel>())
                ]
            )
        );
    }
}
