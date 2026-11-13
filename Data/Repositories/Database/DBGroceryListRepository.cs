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
    /// DB implementation of IGroceryListRepository
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos 
    /// Created: 2026-04-26 
    /// </remarks>
    public class DBGroceryListRepository : IGroceryListRepository
    {

        private FreshOutContext _context;

        /// <summary>
        /// Constructor that initializes the DB repository
        /// </summary>
        public DBGroceryListRepository(FreshOutContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all grocery lists for a user
        /// </summary>
        /// <param name="userId">The user id</param>
        public List<GroceryList> GetUserGroceryLists(int userId)
        {
            return _context.GroceryLists.Include(l => l.Items).Where(l => l.UserId == userId).ToList();
        }

        /// <summary>
        /// Gets a grocery list by id
        /// </summary>
        /// <param name="listId">The list id</param>
        public GroceryList GetGroceryListById(int listId)
        {
            return _context.GroceryLists.FirstOrDefault(l => l.GroceryListId == listId);
        }

        /// <summary>
        /// Adds a new grocery list
        /// </summary>
        /// <param name="list">The grocery list to be added</param>
        public void AddGroceryList(GroceryList list)
        {
            if (list != null)
            {
                _context.GroceryLists.Add(list);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Removes a grocery list by id
        /// </summary>
        /// <param name="listId">The id of the list to be removed</param>
        public void RemoveGroceryList(int listId)
        {
            GroceryList list = _context.GroceryLists.Find(listId);

            if (list != null)
            {
                _context.GroceryLists.Remove(list);
                _context.SaveChanges();
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
                GroceryList existing = _context.GroceryLists.FirstOrDefault(l => l.GroceryListId == list.GroceryListId);

                if (existing != null)
                {
                    existing.Name = list.Name;
                    _context.SaveChanges();
                }
            }
        }


        /// <summary>
        /// Deletes all items from an existing grocery list
        /// </summary>
        /// <param name="listId">The list from which items will be removed</param>
        public void DeleteAllFromGroceryList(int listId)
        {
            GroceryList list = _context.GroceryLists.Include(l => l.Items).FirstOrDefault(l => l.GroceryListId == listId);

            if (list != null)
            {
                _context.GroceryItems.RemoveRange(list.Items);
                _context.SaveChanges();
            }
        }
    }
}