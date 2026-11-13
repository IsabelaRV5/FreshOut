using Data.Entities;
using Logic;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace FreshOutTests;

[TestClass]
public sealed class SortingTest
{
    public User TestUser = new User() { Username = "Test", PasswordHash = "gfngnfdgfdg", IconPath= "../static/GirlIcon.png" };
    /// <summary>
    /// <c>SortFoodItems_ByNameAsc</c>
    /// Tests SortByName SortAscending method 
    /// </summary>
    [TestMethod]
    public void SortFoodItems_ByNameAsc()
    {
        //Arrange
        FoodItem potato = new LocationItem() { FoodItemId = 1, Name = "Potato", UserId = 1, User = TestUser, Type = "Vegetable", Quantity = 5, Location = new LocationList() { Name = "Test", User = TestUser } };
        FoodItem apple = new LocationItem() { FoodItemId = 2, Name = "Apple", UserId = 1, User = TestUser, Type = "Fruits", Quantity = 2, Location = new LocationList() { Name = "Test", User = TestUser } };
        FoodItem milk = new LocationItem() { FoodItemId = 3, Name = "Milk", UserId = 1, User = TestUser, Type = "Dairy", Quantity = 1, Location = new LocationList() { Name = "Test", User = TestUser } };
        FoodItem garlic = new LocationItem() { FoodItemId = 4, Name = "Garlic", UserId = 1, User = TestUser, Type = "Vegetable", Quantity = 10, Location = new LocationList() { Name = "Test", User = TestUser } };

        List<FoodItem> testList = new List<FoodItem>()
        {
            potato, apple, milk, garlic
        };

        // Act
        ISortMethod<FoodItem> sort = new SortByName();
        List<FoodItem> sorted = sort.SortAscending(testList);

        List<FoodItem> expected = new List<FoodItem>()
        {
            apple, garlic, milk, potato
        };

        //Assert
        CollectionAssert.AreEqual(expected, sorted);
    }

    /// <summary>
    /// <c>SortFoodItems_ByNameAsc</c>
    /// Tests SortByName SortDescending method 
    /// </summary>
    [TestMethod]
    public void SortFoodItems_ByNameDesc()
    {
        //Arrange
        FoodItem potato = new LocationItem() { FoodItemId = 1, Name = "Potato", UserId = 1, User = TestUser, Type = "Vegetable", Quantity = 5, Location = new LocationList() { Name = "Test", User = TestUser } };
        FoodItem apple = new LocationItem() { FoodItemId = 2, Name = "Apple", UserId = 1, User = TestUser, Type = "Fruits", Quantity = 2, Location = new LocationList() { Name = "Test", User = TestUser } };
        FoodItem milk = new LocationItem() { FoodItemId = 3, Name = "Milk", UserId = 1, User = TestUser, Type = "Dairy", Quantity = 1, Location = new LocationList() { Name = "Test", User = TestUser } };
        FoodItem garlic = new LocationItem() { FoodItemId = 4, Name = "Garlic", UserId = 1, User = TestUser, Type = "Vegetable", Quantity = 10, Location = new LocationList() { Name = "Test", User = TestUser } };

        List<FoodItem> testList = new List<FoodItem>()
        {
            potato, apple, milk, garlic
        };

        // Act
        ISortMethod<FoodItem> sort = new SortByName();
        List<FoodItem> sorted = sort.SortDescending(testList);

        List<FoodItem> expected = new List<FoodItem>()
        {
            potato, milk, garlic, apple
        };

        //Assert
        CollectionAssert.AreEqual(expected, sorted);
    }

    /// <summary>
    /// <c>SortFoodItems_ByDateAsc</c>
    /// Tests SortByDate SortAscending method 
    /// </summary>
    [TestMethod]
    public void SortFoodItems_ByDateAsc()
    {
        //Arrange
        LocationItem potato = new LocationItem() { FoodItemId = 1, Name = "Potato", UserId = 1, User = TestUser, Type = "Vegetable", Quantity = 5, Location = new LocationList() { Name = "Test", User = TestUser }, ExpirationDate = DateOnly.Parse("2026-04-17") };
        LocationItem apple = new LocationItem() { FoodItemId = 2, Name = "Apple", UserId = 1, User = TestUser, Type = "Fruits", Quantity = 2, Location = new LocationList() { Name = "Test", User = TestUser }, ExpirationDate = DateOnly.Parse("2026-04-11") };
        LocationItem milk = new LocationItem() { FoodItemId = 3, Name = "Milk", UserId = 1, User = TestUser, Type = "Dairy", Quantity = 1, Location = new LocationList() { Name = "Test", User = TestUser }, ExpirationDate = DateOnly.Parse("2026-04-22") };
        LocationItem garlic = new LocationItem() { FoodItemId = 4, Name = "Garlic", UserId = 1, User = TestUser, Type = "Vegetable", Quantity = 10, Location = new LocationList() { Name = "Test", User = TestUser }, ExpirationDate = DateOnly.Parse("2026-06-25") };

        List<LocationItem> testList = new List<LocationItem>()
        {
            potato, apple, milk, garlic
        };

        // Act
        ISortMethod<LocationItem> sort = new SortByDate();
        List<LocationItem> sorted = sort.SortAscending(testList);

        List<LocationItem> expected = new List<LocationItem>()
        {
            apple, potato, milk, garlic
        };

        //Assert
        CollectionAssert.AreEqual(expected, sorted);
    }

    /// <summary>
    /// <c>SortFoodItems_ByDateAsc</c>
    /// Tests SortByDate SortDescending method 
    /// </summary>
    [TestMethod]
    public void SortFoodItems_ByDateDesc()
    {
        //Arrange
        LocationItem potato = new LocationItem() { FoodItemId = 1, Name = "Potato", UserId = 1, User = TestUser, Type = "Vegetable", Quantity = 5, Location = new LocationList() { Name = "Test", User = TestUser }, ExpirationDate = DateOnly.Parse("2026-04-17") };
        LocationItem apple = new LocationItem() { FoodItemId = 2, Name = "Apple", UserId = 1, User = TestUser, Type = "Fruits", Quantity = 2, Location = new LocationList() { Name = "Test", User = TestUser }, ExpirationDate = DateOnly.Parse("2026-04-11") };
        LocationItem milk = new LocationItem() { FoodItemId = 3, Name = "Milk", UserId = 1, User = TestUser, Type = "Dairy", Quantity = 1, Location = new LocationList() { Name = "Test", User = TestUser }, ExpirationDate = DateOnly.Parse("2026-04-22") };
        LocationItem garlic = new LocationItem() { FoodItemId = 4, Name = "Garlic", UserId = 1, User = TestUser, Type = "Vegetable", Quantity = 10, Location = new LocationList() { Name = "Test", User = TestUser }, ExpirationDate = DateOnly.Parse("2026-06-25") };

        List<LocationItem> testList = new List<LocationItem>()
        {
            potato, apple, milk, garlic
        };

        // Act
        ISortMethod<LocationItem> sort = new SortByDate();
        List<LocationItem> sorted = sort.SortDescending(testList);

        List<LocationItem> expected = new List<LocationItem>()
        {
            garlic, milk, potato, apple
        };

        //Assert
        CollectionAssert.AreEqual(expected, sorted);
    }

    /// <summary>
    /// <c>SortFoodItems_ByQuantityAsc</c>
    /// Tests SortByQuantity SortAscending method 
    /// </summary>
    [TestMethod]
    public void SortFoodItems_ByQuantityAsc()
    {
        //Arrange
        FoodItem potato = new LocationItem() { FoodItemId = 1, Name = "Potato", UserId = 1, User = TestUser, Type = "Vegetable", Quantity = 5, Location = new LocationList() { Name = "Test", User = TestUser }, ExpirationDate = DateOnly.Parse("2026-04-17") };
        FoodItem apple = new LocationItem() { FoodItemId = 2, Name = "Apple", UserId = 1, User = TestUser, Type = "Fruits", Quantity = 2, Location = new LocationList() { Name = "Test", User = TestUser }, ExpirationDate = DateOnly.Parse("2026-04-11") };
        FoodItem milk = new LocationItem() { FoodItemId = 3, Name = "Milk", UserId = 1, User = TestUser, Type = "Dairy", Quantity = 1, Location = new LocationList() { Name = "Test", User = TestUser }, ExpirationDate = DateOnly.Parse("2026-04-22") };
        FoodItem garlic = new LocationItem() { FoodItemId = 4, Name = "Garlic", UserId = 1, User = TestUser, Type = "Vegetable", Quantity = 10, Location = new LocationList() { Name = "Test", User = TestUser }, ExpirationDate = DateOnly.Parse("2026-06-25") };

        List<FoodItem> testList = new List<FoodItem>()
        {
            potato, apple, milk, garlic
        };

        // Act
        ISortMethod<FoodItem> sort = new SortByQuantity();
        List<FoodItem> sorted = sort.SortAscending(testList);

        List<FoodItem> expected = new List<FoodItem>()
        {
            milk, apple, potato, garlic
        };

        //Assert
        CollectionAssert.AreEqual(expected, sorted);
    }

    /// <summary>
    /// <c>SortFoodItems_ByQuantityDesc</c>
    /// Tests SortByQuantity SortDescending method 
    /// </summary>
    [TestMethod]
    public void SortFoodItems_ByQuantityDesc()
    {
        //Arrange
        FoodItem potato = new LocationItem() { FoodItemId = 1, Name = "Potato", UserId = 1, User = TestUser, Type = "Vegetable", Quantity = 5, Location = new LocationList() { Name = "Test", User = TestUser }, ExpirationDate = DateOnly.Parse("2026-04-17") };
        FoodItem apple = new LocationItem() { FoodItemId = 2, Name = "Apple", UserId = 1, User = TestUser, Type = "Fruits", Quantity = 2, Location = new LocationList() { Name = "Test", User = TestUser }, ExpirationDate = DateOnly.Parse("2026-04-11") };
        FoodItem milk = new LocationItem() { FoodItemId = 3, Name = "Milk", UserId = 1, User = TestUser, Type = "Dairy", Quantity = 1, Location = new LocationList() { Name = "Test", User = TestUser }, ExpirationDate = DateOnly.Parse("2026-04-22") };
        FoodItem garlic = new LocationItem() { FoodItemId = 4, Name = "Garlic", UserId = 1, User = TestUser, Type = "Vegetable", Quantity = 10, Location = new LocationList() { Name = "Test", User = TestUser }, ExpirationDate = DateOnly.Parse("2026-06-25") };

        List<FoodItem> testList = new List<FoodItem>()
        {
            potato, apple, milk, garlic
        };

        // Act
        ISortMethod<FoodItem> sort = new SortByQuantity();
        List<FoodItem> sorted = sort.SortDescending(testList);

        List<FoodItem> expected = new List<FoodItem>()
        {
            garlic, potato, apple, milk
        };

        //Assert
        CollectionAssert.AreEqual(expected, sorted);
    }
}
