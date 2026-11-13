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
    /// Mock implementation of ILocationListRepository
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos 
    /// Created: 2026-04-19  
    /// </remarks>
    public class MockLocationListRepository : ILocationListRepository
    {
        private List<LocationList> _locationLists;
        private MockUserRepository _userRepository;
        private MockLocationItemRepository _locationItemRepository;
        private int _nextId;

        /// <summary>
        /// Constructor that initializes the mock repository
        /// </summary>
        public MockLocationListRepository()
        {
            _locationLists = new List<LocationList>
            {

                new LocationList
                {
                    LocationListId = 1,
                    Name = "Freezer",
                    UserId = 1,
                    User = _userRepository.GetUserById(1),
                    Items = _locationItemRepository.GetItemsByLocation(1)
                },

                 new LocationList
                {
                    LocationListId = 2,
                    Name = "Fridge",
                    UserId = 2,
                    User = _userRepository.GetUserById(2),
                    Items = _locationItemRepository.GetItemsByLocation(2)
                }
            };


            _nextId = _locationLists.Max(g => g.LocationListId) + 1;
        }
        /// <summary>
        /// Get all location lists for a user
        /// </summary>
        /// <param name="userId">The user id</param>
        public List<LocationList> GetUserLocationLists(int userId)
        {
            return _locationLists.Where(l => l.UserId == userId).ToList();
        }
        /// <summary>
        /// Gets a location list by id
        /// </summary>
        /// <param name="listId">The list id</param>
        public LocationList GetLocationListById(int listId)
        {
            return _locationLists.FirstOrDefault(l => l.LocationListId == listId);
        }
        /// <summary>
        /// Adds a new location list
        /// </summary>
        /// <param name="location">The location list to be added</param>
        public void AddLocationList(LocationList location)
        {
            if (location != null)
            {
                location.LocationListId = _nextId++;
                location.User = _userRepository.GetUserById(location.UserId)!;
                _locationLists.Add(location);
            }
        }
        /// <summary>
        /// Removes a location list by its id
        /// </summary>
        /// <param name="listId">The list id</param>
        public void RemoveLocationList(int listId)
        {
            LocationList list = _locationLists.FirstOrDefault(l => l.LocationListId == listId);

            if (list != null)
            {
                _locationLists.Remove(list);
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
                LocationList existing = _locationLists.FirstOrDefault(l => l.LocationListId == location.LocationListId);

                if (existing != null)
                {
                    existing.Name = location.Name;
                    existing.Items = location.Items;
                }
            }
        }

        /// <summary>
        /// Deletes all spoilage from an existing location list
        /// </summary>
        /// <param name="listId">The list from which items will be removed</param>
        public void DeleteSpoilageFromLocationList(int listId)
        {
            List<LocationItem> items = _locationItemRepository.GetItemsByLocation(listId);
            foreach (LocationItem item in items) {
                if (item.ExpirationDate < DateOnly.FromDateTime(DateTime.Now))
                    _locationItemRepository.RemoveItemFromLocation(listId, item.FoodItemId);
            }
        }
    }
}