using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{ 
    /// <summary>
    /// Repository responsible for managing GroceryList CRUD operations.
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos 
    /// Created: 2026-04-18  
    /// </remarks>
    public interface IGroceryListRepository
    {
        /// <summary>
        /// Gets all grocery lists for a user
        /// </summary>
        /// <param name="userId">The user id</param>
        public List<GroceryList> GetUserGroceryLists(int userId);
        /// <summary>
        /// Gets a grocery list by id
        /// </summary>
        /// <param name="listId">The list id</param>
        public GroceryList GetGroceryListById(int listId);
        /// <summary>
        /// Adds a new grocery list
        /// </summary>
        /// <param name="list">The grocery list to be added</param>
        public void AddGroceryList(GroceryList list);
        /// <summary>
        /// Removes a grocery list by id
        /// </summary>
        /// <param name="listId">The id of the list to be removed</param>
        public void RemoveGroceryList(int listId);
        /// <summary>
        /// Updates an existing grocery list
        /// </summary>
        /// <param name="list">The updated list</param>
        public void UpdateGroceryList(GroceryList list);

        /// <summary>
        /// Deletes all items from an existing grocery list
        /// </summary>
        /// <param name="listId">The list from which items will be removed</param>
        public void DeleteAllFromGroceryList(int listId);
    }
}
