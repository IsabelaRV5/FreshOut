using System.Collections.ObjectModel;
using Data.Entities;
using Logic;

namespace GUI.Presentation.Kitchen;

public sealed partial class KitchenPage : Page
{
    private KitchenViewModel ViewModel => DataContext as KitchenViewModel;
    public KitchenPage()
    {
        this.InitializeComponent();
    }

    // Source: https://learn.microsoft.com/en-us/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.controls.primitives.popup?view=windows-app-sdk-2.0
    // Handles the Click event on the Cancel Button inside the Popup control and 
    // closes the Popup. 
    private void CloseAddPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (AddListPopup.IsOpen) { AddListPopup.IsOpen = false; ViewModel.PointerMode = true; }
    }

    private void CloseEditPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (EditListPopup.IsOpen) { EditListPopup.IsOpen = false; ViewModel.PointerMode = true; }
    }

    private void CloseConfirmDeleteListPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (DeleteListPopup.IsOpen) { DeleteListPopup.IsOpen = false; ViewModel.PointerMode = true; }
    }

    // Handles Click event on the Add Button inside the Popup control and
    // Adds the Location to the LocationList if valid and closes the Popup
    // Or displays an error if invalid
    private void AddPopupClicked(object sender, RoutedEventArgs e)
    { 
        if (AddListPopup.IsOpen)
        {
            ObservableCollection<LocationList> updated = ViewModel.LocationLists;
            if (updated.Any(x => x.Name.Equals(ViewModel.NewLocationName)))
            {
                ViewModel.ErrorMessage = "Location already exist in your Kitchen!";
                ViewModel.AddLocationError = Visibility.Visible;
            }
            else if (ViewModel.NewLocationName == null || ViewModel.NewLocationName.Length < 1)
            {
                ViewModel.ErrorMessage = "Location name cannot be empty!";
                ViewModel.AddLocationError = Visibility.Visible;
            }
            else
            {
                LocationList newLocation = new LocationList() { Name = ViewModel.NewLocationName, User = ViewModel._user };
                updated.Add(newLocation);
                ViewModel._locationRepo.AddLocationList(newLocation);
                ViewModel.LocationLists = updated;
                AddListPopup.IsOpen = false;
                ViewModel.PointerMode = true;
            }
        }
    }

    // Handles Click event on the Change Button inside the Popup control and
    // Change the Location name of selected list if valid and closes the Popup
    // Or displays an error if invalid
    private void ChangePopupClicked(object sender, RoutedEventArgs e)
    {
        if (EditListPopup.IsOpen)
        {
            ObservableCollection<LocationList> updated = ViewModel.LocationLists;
            if (updated.Any(x => x.Name.Equals(ViewModel.NewLocationName)))
            {
                ViewModel.ErrorMessage = "Location already exist in your Kitchen!";
                ViewModel.AddLocationError = Visibility.Visible;
            }
            else if (ViewModel.NewLocationName == null || ViewModel.NewLocationName.Length < 1)
            {
                ViewModel.ErrorMessage = "Location name cannot be empty!";
                ViewModel.AddLocationError = Visibility.Visible;
            }
            else
            {
                LocationList selected = ViewModel.SelectedLocation;
                selected.Name = ViewModel.NewLocationName;
                ViewModel._locationRepo.UpdateLocationList(selected);

                for(int i = 0; i < updated.Count; i++)
                {
                    if (ViewModel.SelectedLocation == updated[i])
                    {
                        updated[i] = selected;
                    }
                }

                ViewModel.SelectedLocation = selected;
                ViewModel.LocationLists = new ObservableCollection<LocationList>(updated);

                EditListPopup.IsOpen = false;
                ViewModel.PointerMode = true;
            }
        }
    }

    // Handles Click event on the Yes Button inside the Popup control and
    // Deletes the selected Location from LocationLists and closes the Popup
    private void DeleteListPopupClicked(object sender, RoutedEventArgs e)
    {
        if (DeleteListPopup.IsOpen)
        {
            if (ViewModel.SelectedLocation != null)
            {
                ViewModel._locationRepo.RemoveLocationList(ViewModel.SelectedLocation.LocationListId);
                ViewModel.LocationLists = new ObservableCollection<LocationList>(ViewModel._locationRepo.GetUserLocationLists(ViewModel._user.UserId));

                if (ViewModel.LocationLists.Count > 0)
                {
                    ViewModel.SelectedLocation = ViewModel.LocationLists[0];
                    ViewModel.Items = new ObservableCollection<LocationItem>(ViewModel.LocationLists[0].Items);
                }
                else
                {
                    ViewModel.SelectedLocation = null;
                    ViewModel.Items = null;
                }
            }

            DeleteListPopup.IsOpen = false;
            ViewModel.PointerMode = true;
        }
    }

    // Handles the Click event on the Button on the page and opens the Popup. 
    private void ShowAddPopupOffsetClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!AddListPopup.IsOpen) { AddListPopup.IsOpen = true; ViewModel.PointerMode = false; ViewModel.AddLocationError = Visibility.Collapsed; ViewModel.NewLocationName = ""; }
    }

    // Handles the Click event on the Button on the page and opens the Popup. 
    private void ShowEditPopupOffsetClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!EditListPopup.IsOpen) { EditListPopup.IsOpen = true; ViewModel.PointerMode = false; ViewModel.AddLocationError = Visibility.Collapsed; ViewModel.NewLocationName = ""; }
    }

    // Handles the Click event on the Button on the page and opens the Popup. 
    private void ShowConfirmDeleteListPopupOffsetClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!DeleteListPopup.IsOpen) { DeleteListPopup.IsOpen = true; ViewModel.PointerMode = false; ViewModel.AddLocationError = Visibility.Collapsed; ViewModel.NewLocationName = ""; }
    }
}

