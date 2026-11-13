using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Data.Repositories.Database
{
    /// <summary>
    /// DB implementation of IFoodItemRepository
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos 
    /// Created: 2026-04-21 
    /// </remarks>
    public class DBFoodItemRepository : IFoodItemRepository
    {

        private FreshOutContext _context;

        /// <summary>
        /// Constructor that initializes the DB repository
        /// </summary>
        public DBFoodItemRepository(FreshOutContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all food items from the database
        /// </summary>
        /// <returns>A list of all foodItems</returns>
        public List<FoodItem> GetFoodItems()
        {
            return _context.FoodItems.ToList();
        }
        /// <summary>
        /// Gets a food item by id
        /// </summary>
        /// <param name="itemId">The food item id</param>
        /// <returns>The food item if found or null if not found</returns>
        public FoodItem GetItemById(int itemId)
        {
            return _context.FoodItems.FirstOrDefault(i => i.FoodItemId == itemId);
        }
        /// <summary>
        /// Gets all food items for a specific user
        /// </summary>
        /// <param name="userId">The user id</param>
        public List<FoodItem> GetUserFoodItems(int userId)
        {
            return _context.FoodItems.Where(f => f.UserId == userId).ToList();
        }
        /// <summary>
        /// Adds a new food item
        /// </summary>
        /// <param name="item">The food item to add</param>
        public void AddFoodItem(FoodItem item)
        {
            if (item != null)
            {
                _context.FoodItems.Add(item);
                _context.SaveChanges();
            }
        }
        /// <summary>
        /// Removes a food item by id
        /// </summary>
        /// <param name="itemId">The id of the food item to be removed</param>
        public void RemoveFoodItem(int itemId)
        {
            FoodItem item = _context.FoodItems.FirstOrDefault(i => i.FoodItemId == itemId);

            if (item != null)
            {
                _context.FoodItems.Remove(item);
                _context.SaveChanges();
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
                FoodItem existing = _context.FoodItems.FirstOrDefault(i => i.FoodItemId == item.FoodItemId);

                if (existing != null)
                {
                    existing.Name = item.Name;
                    existing.Type = item.Type;
                    existing.Quantity = item.Quantity;
                    _context.SaveChanges();
                }
            }
        }
    }
}