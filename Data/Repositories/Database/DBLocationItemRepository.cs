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
    /// DB implementation of ILocationItemRepository
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos 
    /// Created: 2026-04-26 
    /// </remarks>
    public class DBLocationItemRepository : ILocationItemRepository
    {

        private FreshOutContext _context;

        /// <summary>
        /// Constructor that initializes the DB repository
        /// </summary>
        public DBLocationItemRepository(FreshOutContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Gets items by location id
        /// </summary>
        /// <param name="locationId">The location id</param>
        public List<LocationItem> GetItemsByLocation(int locationId)
        {
            return _context.LocationItems.Where(l => l.LocationListId == locationId).ToList();
        }

        /// <summary>
        /// Adds an item to a location list
        /// </summary>
        /// <param name="locationId">The location id</param>
        /// <param name="item">The item to add</param>
        public void AddItemToLocation(int locationId, LocationItem item)
        {
            if (item != null)
            {
                bool existing = _context.LocationLists.Any(l => l.LocationListId == locationId);
                if (existing)
                {
                    item.LocationListId = locationId;
                    _context.LocationItems.Add(item);
                    _context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Updates a location item
        /// </summary>
        /// <param name="item">The updated item</param>
        public void UpdateLocationItem(LocationItem item)
        {
            if (item != null)
            {
                LocationItem existing = _context.LocationItems.FirstOrDefault(i => i.FoodItemId == item.FoodItemId && i.LocationListId == item.LocationListId);

                if (existing != null)
                {
                    string newName = item.Name;
                    string newType = item.Type;
                    int newQuantity = item.Quantity;
                    DateOnly newExpDate = item.ExpirationDate;

                    if (item.Name == null || item.Name.Length == 0)
                        newName = existing.Name;

                    if (item.Type == null)
                        newType = existing.Type;

                    if (item.Quantity == 0)
                        newQuantity = existing.Quantity;

                    existing.Name = newName;
                    existing.Type = newType;
                    existing.Quantity = newQuantity;
                    existing.ExpirationDate = newExpDate;
                    _context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Removes an item from a location
        /// </summary>
        /// <param name="locationId">The location id</param>
        /// <param name="itemId">The item id</param>
        public void RemoveItemFromLocation(int locationId, int itemId)
        {
            LocationItem item = _context.LocationItems.FirstOrDefault(i => i.FoodItemId == itemId && i.LocationListId == locationId);

            if (item != null)
            {
                _context.LocationItems.Remove(item);
                _context.SaveChanges();
            }
        }
    }
}