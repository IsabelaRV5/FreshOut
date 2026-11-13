using System.Collections.ObjectModel;
using Data.Entities;
using GUI.Presentation.GroceryLists;
using Windows.System;

namespace GUI.Presentation.GroceryLists;

public sealed partial class GroceryListsPage : Page
{
    private GroceryListsViewModel ViewModel => DataContext as GroceryListsViewModel;
    public GroceryListsPage()
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

    private void CloseConfirmDeleteAllPopupClicked(object sender, RoutedEventArgs e)
    {
        // if the Popup is open, then close it 
        if (DeleteAllPopup.IsOpen) { DeleteAllPopup.IsOpen = false; ViewModel.PointerMode = true; }
    }

    // Handles Click event on the Add Button inside the Popup control and
    // Adds the GroceryList to the GroceryLists if valid and closes the Popup
    // Or displays an error if invalid
    private void AddPopupClicked(object sender, RoutedEventArgs e)
    {
        if (AddListPopup.IsOpen)
        {
            ObservableCollection<GroceryList> updated = ViewModel.GroceryLists;
            if (!updated.Any(x => x.Name.Equals(ViewModel.NewGroceryListName)))
            {
                GroceryList newGroceryList = new GroceryList() { Name = ViewModel.NewGroceryListName, User = ViewModel._user };
                updated.Add(newGroceryList);
                ViewModel._groceryListRepo.AddGroceryList(newGroceryList);
                ViewModel.GroceryLists = updated;
                AddListPopup.IsOpen = false;
                ViewModel.PointerMode = true;
            }
            else
            {
                ViewModel.AddGroceryListError = Visibility.Visible;
            }
        }
    }

    // Handles Click event on the Change Button inside the Popup control and
    // Changes the GroceryList name if valid and closes the Popup
    // Or displays an error if invalid
    private void ChangePopupClicked(object sender, RoutedEventArgs e)
    {
        if (EditListPopup.IsOpen)
        {
            ObservableCollection<GroceryList> updated = ViewModel.GroceryLists;
            if (updated.Any(x => x.Name.Equals(ViewModel.NewGroceryListName)))
            {
                ViewModel.ErrorMessage = "Grocery list already exist!";
                ViewModel.AddGroceryListError = Visibility.Visible;
            }
            else if (ViewModel.NewGroceryListName == null || ViewModel.NewGroceryListName.Length < 1)
            {
                ViewModel.ErrorMessage = "Grocery list name cannot be empty!";
                ViewModel.AddGroceryListError = Visibility.Visible;
            }
            else
            {
                GroceryList selected = ViewModel.SelectedGroceryList;
                selected.Name = ViewModel.NewGroceryListName;
                ViewModel._groceryListRepo.UpdateGroceryList(selected);

                for (int i = 0; i < updated.Count; i++)
                {
                    if (ViewModel.SelectedGroceryList == updated[i])
                    {
                        updated[i] = selected;
                    }
                }

                ViewModel.SelectedGroceryList = selected;
                ViewModel.GroceryLists = new ObservableCollection<GroceryList>(updated);

                EditListPopup.IsOpen = false;
                ViewModel.PointerMode = true;
            }
        }
    }

    // Handles Click event on the Yes Button inside the Popup control and
    // Deletes the selected GroceryList from GroceryLists and closes the Popup
    private void DeleteListPopupClicked(object sender, RoutedEventArgs e)
    {
        if (DeleteListPopup.IsOpen)
        {
            if (ViewModel.SelectedGroceryList != null)
            {
                ViewModel._groceryListRepo.RemoveGroceryList(ViewModel.SelectedGroceryList.GroceryListId);
                ViewModel.GroceryLists = new ObservableCollection<GroceryList>(ViewModel._groceryListRepo.GetUserGroceryLists(ViewModel._user.UserId));

                if (ViewModel.GroceryLists.Count > 0)
                {
                    ViewModel.SelectedGroceryList = ViewModel.GroceryLists[0];
                    ViewModel.Items = new ObservableCollection<GroceryItem>(ViewModel.GroceryLists[0].Items);
                }
                else
                {
                    ViewModel.SelectedGroceryList = null;
                    ViewModel.Items = null;
                }
            }
            DeleteListPopup.IsOpen = false;
            ViewModel.PointerMode = true;
        }
    }

    // Handles Click event on the Yes Button inside the Popup control and
    // Deletes the selected GroceryList from GroceryLists and closes the Popup
    private void DeleteAllPopupClicked(object sender, RoutedEventArgs e)
    {
        if (DeleteAllPopup.IsOpen)
        {
            if (ViewModel.SelectedGroceryList != null)
            {
                ViewModel._groceryListRepo.DeleteAllFromGroceryList(ViewModel.SelectedGroceryList.GroceryListId);
                ViewModel.Items = new ObservableCollection<GroceryItem>(ViewModel.SelectedGroceryList.Items);
            }
            DeleteAllPopup.IsOpen = false;
            ViewModel.PointerMode = true;
        }
    }

    // Handles the Click event on the Button on the page and opens the Popup. 
    private void ShowAddPopupOffsetClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!AddListPopup.IsOpen) { AddListPopup.IsOpen = true; ViewModel.PointerMode = false; ViewModel.AddGroceryListError = Visibility.Collapsed; ViewModel.NewGroceryListName = ""; }
    }

    // Handles the Click event on the Button on the page and opens the Popup. 
    private void ShowEditPopupOffsetClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!EditListPopup.IsOpen) { EditListPopup.IsOpen = true; ViewModel.PointerMode = false; ViewModel.AddGroceryListError = Visibility.Collapsed; ViewModel.NewGroceryListName = ""; }
    }

    // Handles the Click event on the Button on the page and opens the Popup. 
    private void ShowConfirmDeleteListPopupOffsetClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!DeleteListPopup.IsOpen) { DeleteListPopup.IsOpen = true; ViewModel.PointerMode = false; ViewModel.AddGroceryListError = Visibility.Collapsed; ViewModel.NewGroceryListName = ""; }
    }

    // Handles the Click event on the Button on the page and opens the Popup. 
    private void ShowConfirmDeleteAllPopupOffsetClicked(object sender, RoutedEventArgs e)
    {
        // open the Popup if it isn't open already 
        if (!DeleteAllPopup.IsOpen) { DeleteAllPopup.IsOpen = true; ViewModel.PointerMode = false; ViewModel.AddGroceryListError = Visibility.Collapsed; ViewModel.NewGroceryListName = ""; }
    }
}

