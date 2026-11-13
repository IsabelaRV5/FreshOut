using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Database
{
    /// <summary>
    /// DB implementation of ILocationListRepository
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos 
    /// Created: 2026-04-26 
    /// </remarks>
    public class DBLocationListRepository : ILocationListRepository
    {

        private FreshOutContext _context;

        /// <summary>
        /// Constructor that initializes the DB repository
        /// </summary>
        public DBLocationListRepository(FreshOutContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all location lists for a user
        /// </summary>
        /// <param name="userId">The user id</param>
        public List<LocationList> GetUserLocationLists(int userId)
        {
            return _context.LocationLists.Include(l => l.Items).Where(l => l.UserId == userId).ToList();
        }

        /// <summary>
        /// Gets a location list by id
        /// </summary>
        /// <param name="listId">The list id</param>
        public LocationList GetLocationListById(int listId)
        {
            return _context.LocationLists.FirstOrDefault(l => l.LocationListId == listId);
        }

        /// <summary>
        /// Adds a new location list
        /// </summary>
        /// <param name="location">The location list to be added</param>
        public void AddLocationList(LocationList location)
        {
            if (location != null)
            {
                _context.LocationLists.Add(location);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Removes a location list by its id
        /// </summary>
        /// <param name="listId">The list id</param>
        public void RemoveLocationList(int listId)
        {
            LocationList list = _context.LocationLists.Find(listId);

            if (list != null)
            {
                _context.LocationLists.Remove(list);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Updates a location list
        /// </summary>
        /// <param name="location">The updated list</param>
        public void UpdateLocationList(LocationList location)
        {
            if (location != null)
            {

                LocationList existing = _context.LocationLists.FirstOrDefault(l => l.LocationListId == location.LocationListId);

                if (existing != null)
                {
                    existing.Name = location.Name;
                    _context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Deletes all items from an existing location list
        /// </summary>
        /// <param name="listId">The list from which items will be removed</param>
        public void DeleteSpoilageFromLocationList(int listId)
        {
            LocationList list = _context.LocationLists.Include(l => l.Items).FirstOrDefault(l => l.LocationListId == listId);

            if (list != null)
            {
                List<LocationItem> items = list.Items;
                foreach (LocationItem item in items)
                    if (item.ExpirationDate < DateOnly.FromDateTime(DateTime.Now))
                        _context.LocationItems.Remove(item);
                _context.SaveChanges();
            }
        }
    }
}