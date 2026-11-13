using Data.Entities;
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
    /// Mock implementation of ILocationItemRepository
    /// </summary>
    /// <remarks>
    /// Author: Isabela Resende Vasconcelos 
    /// Created: 2026-04-19  
    /// </remarks>
    public class MockLocationItemRepository : ILocationItemRepository
    {


        private List<LocationItem> _locationItems;
        private MockUserRepository _userRepository;
        private MockLocationListRepository _locationListRepository;
        private MockFoodItemRepository _foodItemRepository;
        private int _nextId;

        /// <summary>
        /// Constructor that initializes the mock repository
        /// </summary>
        public MockLocationItemRepository()
        {
            _locationItems = new List<LocationItem>
            {

                new LocationItem { FoodItemId = 5, Name = "Broccoli", Type = "Vegetable", Quantity = 3, UserId = 1, User = _userRepository.GetUserById(1), LocationListId = 1, Location = _locationListRepository.GetLocationListById(1), ExpirationDate = new DateOnly(2026, 10, 10)},
                new LocationItem { FoodItemId = 6, Name = "Chicken", Type = "Meat", Quantity = 5, UserId = 1, User = _userRepository.GetUserById(1), LocationListId = 1, Location = _locationListRepository.GetLocationListById(1), ExpirationDate = new DateOnly(2026, 6, 5)},
                new LocationItem { FoodItemId = 7, Name = "Rice", Type = "Grain", Quantity = 2, UserId = 2, User = _userRepository.GetUserById(2), LocationListId = 2, Location = _locationListRepository.GetLocationListById(2), ExpirationDate = new DateOnly(2027, 2, 1)},
                new LocationItem { FoodItemId = 8, Name = "Spaghetti", Type = "Pasta", Quantity = 5, UserId = 2, User = _userRepository.GetUserById(2), LocationListId = 2, Location = _locationListRepository.GetLocationListById(2), ExpirationDate = new DateOnly(2026, 7, 8)}
            };

            _nextId = _foodItemRepository.GetFoodItems().Max(f => f.FoodItemId) + 1;
        }
        /// <summary>
        /// Gets items by location id
        /// </summary>
        /// <param name="locationId">The location id</param>
        public List<LocationItem> GetItemsByLocation(int locationId)
        {
            return _locationItems.Where(i => i.LocationListId == locationId).ToList();
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
                item.FoodItemId = _nextId++;
                item.LocationListId = locationId;

                _locationItems.Add(item);
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
                LocationItem existing = _locationItems.FirstOrDefault(i => i.FoodItemId == item.FoodItemId);

                if (existing != null)
                {
                    existing.Name = item.Name;
                    existing.Type = item.Type;
                    existing.Quantity = item.Quantity;
                    existing.ExpirationDate = item.ExpirationDate;
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
            LocationItem item = _locationItems.FirstOrDefault(i => i.FoodItemId == itemId && i.LocationListId == locationId);

            if (item != null)
            {
                _locationItems.Remove(item);
            }
        }
    }
}