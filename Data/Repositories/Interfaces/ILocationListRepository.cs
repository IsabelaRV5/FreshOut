using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository responsible for managing LocationList CRUD operations.
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos
    /// Created: 2026-04-18  
    /// </remarks>>
    public interface ILocationListRepository
    {
        /// <summary>
        /// Get all location lists for a user
        /// </summary>
        /// <param name="userId">The user id</param>
        public List<LocationList> GetUserLocationLists(int userId);
        /// <summary>
        /// Gets a location list by id
        /// </summary>
        /// <param name="listId">The list id</param>
        public LocationList GetLocationListById(int listId);
        /// <summary>
        /// Adds a new location list
        /// </summary>
        /// <param name="location">The location list to be added</param>
        public void AddLocationList(LocationList location);
        /// <summary>
        /// Removes a location list by its id
        /// </summary>
        /// <param name="listId">The list id</param>
        public void RemoveLocationList(int listId);
        /// <summary>
        /// Updates a location list
        /// </summary>
        /// <param name="location">The updated list</param>
        public void UpdateLocationList(LocationList location);

        /// <summary>
        /// Deletes all items from an existing location list
        /// </summary>
        /// <param name="listId">The list from which items will be removed</param>
        public void DeleteSpoilageFromLocationList(int listId);
    }
}
