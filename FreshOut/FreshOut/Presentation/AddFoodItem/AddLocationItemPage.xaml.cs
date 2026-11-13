using GUI.Presentation;
using GUI.Presentation.AddFoodItem;

namespace GUI.Presentation;
public sealed partial class AddLocationItemPage : Page
{
    private AddLocationItemViewModel ViewModel => DataContext as AddLocationItemViewModel;

    public AddLocationItemPage() 
    {
        this.InitializeComponent();
    }

    // Handles the Click event on the Cancel Button inside the Popup control and 
    // closes the Popup. 
    private void ClosePopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; ViewModel.PointerMode = true; }
    }

    // Handles the Click event on the Button on the page and opens the Popup. 
    private void ShowPopupOffsetClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!StandardPopup.IsOpen) { StandardPopup.IsOpen = true; ViewModel.PointerMode = false; }
    }
}

