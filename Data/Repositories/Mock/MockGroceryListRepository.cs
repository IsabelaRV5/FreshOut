using Data.Entities;
using Data.Repositories.Database;
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
    /// Mock implementation of IGroceryListRepository
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos 
    /// Created: 2026-04-19  
    /// </remarks>
    public class MockGroceryListRepository : IGroceryListRepository
    {
        private List<GroceryList> _groceryLists;
        private MockUserRepository _userRepository;
        private MockGroceryItemRepository _groceryItemRepository;
        private int _nextId;

        /// <summary>
        /// Constructor that initializes the mock repository
        /// </summary>
        public MockGroceryListRepository()
        {
            _groceryLists = new List<GroceryList>
            {

                new GroceryList
                {
                    GroceryListId = 1,
                    Name = "Healthy",
                    UserId = 1,
                    User = _userRepository.GetUserById(1),
                    Items = _groceryItemRepository.GetItemsByGroceryList(1)
                },

                new GroceryList
                {
                    GroceryListId = 2,
                    Name = "Priority list",
                    UserId = 2,
                    User = _userRepository.GetUserById(2),
                    Items = _groceryItemRepository.GetItemsByGroceryList(2)
                }
            };


            _nextId = _groceryLists.Max(g => g.GroceryListId) + 1;
        }
        /// <summary>
        /// Gets a grocery list by id
        /// </summary>
        /// <param name="listId">The list id</param>
        public GroceryList GetGroceryListById(int listId)
        {
            return _groceryLists.FirstOrDefault(l => l.GroceryListId == listId);
        }
        /// <summary>
        /// Gets all grocery lists for a user
        /// </summary>
        /// <param name="userId">The user id</param>
        public List<GroceryList> GetUserGroceryLists(int userId)
        {
            return _groceryLists.Where(l => l.UserId == userId).ToList();
        }
        /// <summary>
        /// Adds a new grocery list
        /// </summary>
        /// <param name="list">The grocery list to be added</param>
        public void AddGroceryList(GroceryList list)
        {
            if (list != null)
            {
                list.GroceryListId = _nextId++;
                list.User = _userRepository.GetUserById(list.UserId)!;
                _groceryLists.Add(list);
            }
        }
        /// <summary>
        /// Removes a grocery list by id
        /// </summary>
        /// <param name="listId">The id of the list to be removed</param>
        public void RemoveGroceryList(int listId)
        {
            GroceryList list = _groceryLists.FirstOrDefault(l => l.GroceryListId == listId);

            if (list != null)
            {
                _groceryLists.Remove(list);
            }
        }
        /// <summary>
        /// Updates an existing grocery list
        /// </summary>
        /// <param name="list">The updated list</param>
        public void UpdateGroceryList(GroceryList list)
        {
            if (list != null)
            {
                GroceryList existing = _groceryLists.FirstOrDefault(l => l.GroceryListId == list.GroceryListId);

                if (existing != null)
                    existing.Name = list.Name;
            }
        }

        /// <summary>
        /// Deletes all items from an existing grocery list
        /// </summary>
        /// <param name="listId">The list from which items will be removed</param>
        public void DeleteAllFromGroceryList(int listId)
        {
            List<GroceryItem> items = _groceryItemRepository.GetItemsByGroceryList(listId);
            foreach (GroceryItem item in items)
            {
                _groceryItemRepository.RemoveItemFromGroceryList(listId, item.FoodItemId);
            }
        }
    }
}
