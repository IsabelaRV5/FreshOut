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
    /// Mock implementation of IFoodItemRepository
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos 
    /// Created: 2026-04-19  
    /// </remarks>
    public class MockFoodItemRepository : IFoodItemRepository
    {

        private List<FoodItem> _foodItems;
        private int _nextId;
        private MockUserRepository _userRepository;
        private MockGroceryItemRepository _groceryItemRepository;
        private MockLocationItemRepository _locationItemRepository;

        /// <summary>
        /// Constructor that initializes the mock repository
        /// </summary>
        public MockFoodItemRepository()
        {
            _foodItems = new List<FoodItem>()
            {
                _groceryItemRepository.GetItemsByGroceryList(1)[0],
                _groceryItemRepository.GetItemsByGroceryList(1)[1],
                _groceryItemRepository.GetItemsByGroceryList(2)[0],
                _groceryItemRepository.GetItemsByGroceryList(2)[1],
                _locationItemRepository.GetItemsByLocation(1)[0],
                _locationItemRepository.GetItemsByLocation(1)[1],
                _locationItemRepository.GetItemsByLocation(2)[0],
                _locationItemRepository.GetItemsByLocation(2)[1]
            };

            _nextId = _foodItems.Max(f => f.FoodItemId) + 1;
        }

        /// <summary>
        /// Gets all food items from the database
        /// </summary>
        /// <returns>A list of all foodItems</returns>
        public List<FoodItem> GetFoodItems() 
        { 
            return _foodItems; 
        }
        /// <summary>
        /// Gets a food item by id
        /// </summary>
        /// <param name="itemId">The food item id</param>
        /// <returns>The food item if found or null if not found</returns>
        public FoodItem GetItemById(int itemId)
        {
            return _foodItems.FirstOrDefault(i => i.FoodItemId == itemId);
        }
        /// <summary>
        /// Gets all food items for a specific user
        /// </summary>
        /// <param name="userId">The user id</param>
        public List<FoodItem> GetUserFoodItems(int userId)
        {
            return _foodItems.Where(f => f.UserId == userId).ToList();
        }
        /// <summary>
        /// Adds a new food item
        /// </summary>
        /// <param name="item">The food item to add</param>
        public void AddFoodItem(FoodItem item)
        {
            if (item != null)
            {
                item.FoodItemId = _nextId++;
                item.User = _userRepository.GetUserById(item.UserId)!;
                _foodItems.Add(item);
            }
        }
        /// <summary>
        /// Removes a food item by id
        /// </summary>
        /// <param name="itemId">The id of the food item to be removed</param>
        public void RemoveFoodItem(int itemId)
        {
            FoodItem item = _foodItems.FirstOrDefault(i => i.FoodItemId == itemId);

            if (item != null)
            {
                _foodItems.Remove(item);
            }
        }
        /// <summary>
        /// Updates an existing food item
        /// </summary>
        /// <param name="item">The updated food item</param>
        public void UpdateFoodItem(FoodItem item)
        {
            if (item != null)
            {
                FoodItem existing = _foodItems.FirstOrDefault(i => i.FoodItemId == item.FoodItemId);

                if (existing != null)
                {
                    existing.Name = item.Name;
                    existing.Type = item.Type;
                    existing.Quantity = item.Quantity;
                }
            }
        }
    }
}