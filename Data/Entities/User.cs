using Microsoft.EntityFrameworkCore;


namespace Data.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public required string Username {  get; set; }
        public required string PasswordHash { get; set; }
        public required string IconPath { get; set; }
        public List<FoodItem> FoodItems { get; set; } = new List<FoodItem>();
        public List<LocationList> FoodLocations { get; set; } = new List<LocationList>();   
        public List<GroceryList> GroceryLists { get; set; } = new List<GroceryList>();

    }
}
