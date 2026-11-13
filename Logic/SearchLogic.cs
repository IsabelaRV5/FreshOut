using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;

namespace Logic
{
    /// <summary>
    /// this class is the logic for Searching FoodItems in Kitchen
    /// or Grocery list
    /// </summary>
    /// <remarks>
    /// author: Xena Tat
    /// date: 04.05.2026
    /// </remarks>
    public class SearchLogic
    {
        ///<summary>
        /// <c>SearchKitchen</c>
        /// Searches input word in a Location list
        /// and returns list of items that matches that has a matching
        /// name
        /// 
        /// </summary>
        /// <param name="foodItems"></param> Current displayed LocationItems list
        /// <param name="word"></param> User's input word
        /// <returns> Returns sorted list</returns>
        public ObservableCollection<LocationItem> SearchKitchen(ObservableCollection<LocationItem> foodItems, string word)
        {
            // If input word is empty, return original list
            if (word.Length == 0)
            {
                return foodItems;
            }

            // Otherwise, apply search
            ObservableCollection<LocationItem> updated = new ObservableCollection<LocationItem>();
            foreach(LocationItem item in foodItems)
            {
                // Checks if input is included in names of items (Unsensitive case)
                if (item.Name.ToLower().Contains(word.ToLower()))
                {
                    updated.Add(item);
                }
            }
            return updated;
        }

        ///<summary>
        /// <c>SearchGrocery</c>
        /// Searches input word in a Grocery list
        /// and returns list of items that matches that has a matching
        /// name
        /// 
        /// </summary>
        /// <param name="foodItems"></param> Current displayed GroceryItems list
        /// <param name="word"></param> User's input word
        /// <returns> Returns sorted list</returns>
        public ObservableCollection<GroceryItem> SearchGrocery(ObservableCollection<GroceryItem> foodItems, string word)
        {
            ObservableCollection<GroceryItem> updated = new ObservableCollection<GroceryItem>();
            foreach (GroceryItem item in foodItems)
            {
                if (item.Name.ToLower().Contains(word.ToLower()))
                {
                    updated.Add(item);
                }
            }
            return updated;
        }
    }
}
