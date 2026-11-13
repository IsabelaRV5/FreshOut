using Data.Entities;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Mock
{
    /// <summary>
    /// Mock implementation of IGroceryItemRepository
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos 
    /// Created: 2026-04-19  
    /// </remarks>
    public class MockGroceryItemRepository : IGroceryItemRepository
    {

        private List<GroceryItem> _groceryItems;
        private MockUserRepository _userRepository;
        private MockGroceryListRepository _groceryListRepository;
        private MockFoodItemRepository _foodItemRepository;
        private int _nextId;

        /// <summary>
        /// Constructor that initializes the mock repository
        /// </summary>
        public MockGroceryItemRepository()
        {
            _groceryItems = new List<GroceryItem>
            {
                new GroceryItem { FoodItemId = 1, Name = "Apples", Type = "Fruit", Quantity = 5, UserId = 1, User = _userRepository.GetUserById(1), GroceryListId = 1, GroceryList = _groceryListRepository.GetGroceryListById(1) },
                new GroceryItem { FoodItemId = 2, Name = "Bananas", Type = "Fruit", Quantity = 3, UserId = 1, User = _userRepository.GetUserById(1), GroceryListId = 1, GroceryList = _groceryListRepository.GetGroceryListById(1)},
                new GroceryItem { FoodItemId = 3, Name = "Milk", Type = "Dairy", Quantity = 1, UserId = 2, User = _userRepository.GetUserById(2), GroceryListId = 2, GroceryList = _groceryListRepository.GetGroceryListById(2)},
                new GroceryItem { FoodItemId = 4, Name = "Yogurt", Type = "Dairy", Quantity = 2, UserId = 2, User = _userRepository.GetUserById(2), GroceryListId = 2, GroceryList = _groceryListRepository.GetGroceryListById(2)}
            };


            _nextId = _foodItemRepository.GetFoodItems().Max(f => f.FoodItemId) + 1;
        }
        /// <summary>
        /// Gets all items in a grocery list
        /// </summary>
        /// <param name="listId">The list id</param>
        public List<GroceryItem> GetItemsByGroceryList(int listId)
        { 
            return _groceryItems.Where(i => i.GroceryListId == listId).ToList();
        }

        /// <summary>
        /// Adds an item to a grocery list
        /// </summary>
        /// <param name="listId">The list id</param>
        /// <param name="item">The item to add</param>
        public void AddItemToGroceryList(int listId, GroceryItem item)
        {
            if (item != null)
            {
                item.FoodItemId = _nextId++;
                item.GroceryListId = listId;

                _groceryItems.Add(item);
            }
        }
        /// <summary>
        /// Updates a grocery item
        /// </summary>
        /// <param name="item">The updated item</param>
        public void UpdateGroceryItem(GroceryItem item)
        {
            if (item != null)
            {
                GroceryItem existing = _groceryItems.FirstOrDefault(i => i.FoodItemId == item.FoodItemId);

                if (existing != null)
                {
                    existing.Name = item.Name;
                    existing.Type = item.Type;
                    existing.Quantity = item.Quantity;
                }
            }
        }
        /// <summary>
        /// Removes an item from a grocery list
        /// </summary>
        /// <param name="listId">The list id</param>
        /// <param name="itemId">The item id</param>
        public void RemoveItemFromGroceryList(int listId, int itemId)
        {
            GroceryItem item = _groceryItems.FirstOrDefault(i => i.FoodItemId == itemId && i.GroceryListId == listId);

            if (item != null)
            {
                _groceryItems.Remove(item);
            }
        }

    }
}
