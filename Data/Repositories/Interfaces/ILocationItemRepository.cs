using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository responsible for managing LocationItem CRUD operations.
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos
    /// Created: 2026-04-18  
    public interface ILocationItemRepository
    {
        /// <summary>
        /// Gets items by location id
        /// </summary>
        /// <param name="locationId">The location id</param>
        public List<LocationItem> GetItemsByLocation(int locationId);
        /// <summary>
        /// Adds an item to a location list
        /// </summary>
        /// <param name="locationId">The location id</param>
        /// <param name="item">The item to add</param>
        public void AddItemToLocation(int locationId, LocationItem item);
        /// <summary>
        /// Updates a location item
        /// </summary>
        /// <param name="item">The updated item</param>
        public void UpdateLocationItem(LocationItem item);
        /// <summary>
        /// Removes an item from a location
        /// </summary>
        /// <param name="locationId">The location id</param>
        /// <param name="itemId">The item id</param>
        public void RemoveItemFromLocation(int locationId, int itemId);
    }
}
