using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository responsible for managing FoodItem CRUD operations.
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos 
    /// Created: 2026-04-18  
    /// </remarks>
    public interface IFoodItemRepository
    {
        /// <summary>
        /// Gets all food items from the database
        /// </summary>
        /// <returns>A list of all foodItems</returns>
        public List<FoodItem> GetFoodItems();
        /// <summary>
        /// Gets a food item by id
        /// </summary>
        /// <param name="itemId">The food item id</param>
        /// <returns>The food item if found or null if not found</returns>
        public FoodItem GetItemById(int itemId);
        /// <summary>
        /// Gets all food items for a specific user
        /// </summary>
        /// <param name="userId">The user id</param>
        public List<FoodItem> GetUserFoodItems(int userId);
        /// <summary>
        /// Adds a new food item
        /// </summary>
        /// <param name="item">The food item to add</param>
        public void AddFoodItem(FoodItem item);
        /// <summary>
        /// Removes a food item by id
        /// </summary>
        /// <param name="itemId">The id of the food item to be removed</param>
        public void RemoveFoodItem(int itemId);
        /// <summary>
        /// Updates an existing food item
        /// </summary>
        /// <param name="item">The updated food item</param>
        public void UpdateFoodItem(FoodItem item);
    }
}
