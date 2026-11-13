using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository responsible for managing GroceryItems CRUD operations
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos  
    /// Created: 2026-04-18  
    /// </remarks>
    public interface IGroceryItemRepository
    {
        /// <summary>
        /// Gets all items in a grocery list
        /// </summary>
        /// <param name="listId">The list id</param>
        public List<GroceryItem> GetItemsByGroceryList(int listId);
        /// <summary>
        /// Adds an item to a grocery list
        /// </summary>
        /// <param name="listId">The list id</param>
        /// <param name="item">The item to add</param>
        public void AddItemToGroceryList(int listId, GroceryItem item);
        /// <summary>
        /// Updates a grocery item
        /// </summary>
        /// <param name="item">The updated item</param>
        public void UpdateGroceryItem(GroceryItem item);
        /// <summary>
        /// Removes an item from a grocery list
        /// </summary>
        /// <param name="listId">The list id</param>
        /// <param name="itemId">The item id</param>
        public void RemoveItemFromGroceryList(int listId, int itemId);
    }
}
