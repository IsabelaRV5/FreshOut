using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Logic;
using Data.Repositories.Interfaces;
using Data.Repositories.Mock;
using Data.Entities;

namespace FreshOutTests;

[TestClass]
public class UserManagerTest
{
    [TestMethod]
    public void ValidateCredentials_ValidCredentials_Valid()
    {
        //Arrange
        string inputUsername = "Alice";
        string inputPassword = "alice123";
        
        IUserRepository mockRepo = new MockUserRepository();
        UserManager userManager = new UserManager(mockRepo);

        //Act
        User findValidUser = userManager.ValidateCredentials(inputUsername, inputPassword);

        //Assert
        Assert.IsNotNull(findValidUser);
    }

    [TestMethod]
    public void ValidateCredentials_NonExistantUser_Invalid()
    {
        //Arrange
        string inputUsername = "UserDoesNotExist";
        string inputPassword = "alices123";

        IUserRepository mockRepo = new MockUserRepository();
        UserManager userManager = new UserManager(mockRepo);

        //Act
        User findValidUser = userManager.ValidateCredentials(inputUsername, inputPassword);

        //Assert
        Assert.IsNull(findValidUser);
    }

    [TestMethod]
    public void ValidateCredentials_WrongPassword_Invalid()
    {
        //Arrange
        string inputUsername = "Alice";
        string inputPassword = "password12345";

        IUserRepository mockRepo = new MockUserRepository();
        UserManager userManager = new UserManager(mockRepo);

        //Act
        User findValidUser = userManager.ValidateCredentials(inputUsername, inputPassword);

        //Assert
        Assert.IsNull(findValidUser);
    }

    [TestMethod]
    public void RegisterUser_ValidInput_Valid()
    {
        //Arrange
        string inputUsername = "Xena";
        string inputPassword = "xena123";
        string iconPath = "../static/GirlIcon.png";

        IUserRepository mockRepo = new MockUserRepository();
        UserManager userManager = new UserManager(mockRepo);

        // Act
        Boolean isAccountCreated = userManager.RegisterUser(inputUsername, inputPassword, iconPath);

        // Assert
        Assert.IsTrue(isAccountCreated);
    }

    [TestMethod]
    public void RegisterUser_UsernameInvalid_Invalid()
    {
        //Arrange

        //Alice is already taken
        string inputUsername = "Alice";
        string inputPassword = "alicePassword";
        string iconPath = "../static/GirlIcon.png";

        IUserRepository mockRepo = new MockUserRepository();
        UserManager userManager = new UserManager(mockRepo);

        // Act
        Boolean isAccountCreated = userManager.RegisterUser(inputUsername, inputPassword, iconPath);

        // Assert
        Assert.IsFalse(isAccountCreated);
    }
}
