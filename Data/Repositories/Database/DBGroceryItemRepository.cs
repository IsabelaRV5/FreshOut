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

namespace Data.Repositories.Database
{
    /// <summary>
    /// DB implementation of IGroceryItemRepository
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos 
    /// Created: 2026-04-26 
    /// </remarks>
    public class DBGroceryItemRepository : IGroceryItemRepository
    {

        private FreshOutContext _context;

        /// <summary>
        /// Constructor that initializes the DB repository
        /// </summary>
        public DBGroceryItemRepository(FreshOutContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all items in a grocery list
        /// </summary>
        /// <param name="listId">The list id</param>
        public List<GroceryItem> GetItemsByGroceryList(int listId)
        {
            return _context.GroceryItems.Where(g => g.GroceryListId == listId).ToList();
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
                bool existing = _context.GroceryLists.Any(l => l.GroceryListId == listId);
                if (existing)
                {
                    item.GroceryListId = listId;
                    _context.GroceryItems.Add(item);
                    _context.SaveChanges();
                }
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
                GroceryItem existing = _context.GroceryItems.FirstOrDefault(i => i.FoodItemId == item.FoodItemId);

                if (existing != null)
                {
                    string newName = item.Name;
                    string newType = item.Type;
                    int newQuantity = item.Quantity;

                    if (item.Name == null || item.Name.Length == 0)
                        newName = existing.Name;

                    if (item.Type == null)
                        newType = existing.Type;

                    if (item.Quantity == 0)
                        newQuantity = existing.Quantity;

                    existing.Name = newName;
                    existing.Type = newType;
                    existing.Quantity = newQuantity;
                    _context.SaveChanges();

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
            GroceryItem item = _context.GroceryItems.FirstOrDefault(i => i.FoodItemId == itemId && i.GroceryListId == listId);

            if (item != null)
            {
                _context.GroceryItems.Remove(item);
                _context.SaveChanges();
            }
        }
    }
}