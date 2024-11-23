using Xunit;
using Microsoft.EntityFrameworkCore;
using ampz_dotnet.Controllers;
using ampz_dotnet.Data;
using ampz_dotnet.Model;
using ampz_dotnet.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ampz_dotnet.Requests;

public class UserControllerTests
{
    private AppDbContext _context;
    private UserController _controller;

    public UserControllerTests()
    {
        ConfigureTestDatabase();
    }

    private void ConfigureTestDatabase()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        _controller = new UserController(_context);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnCreatedUser()
    {
        await ResetDatabase();

        var user = new User
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "password",
            Birthdate = DateOnly.FromDateTime(new DateTime(2000, 1, 1)),
            City = "Test City",
            State = "Test State"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _controller.GetAllUsers();

        Assert.NotNull(result);
        Assert.Single(_context.Users);
    }

    private async Task ResetDatabase()
    {
        _context.Users.RemoveRange(_context.Users);
        _context.Kids.RemoveRange(_context.Kids);
        _context.Devices.RemoveRange(_context.Devices);

        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnListOfUsers()
    {
        await ResetDatabase();

        var user = new User
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "password",
            Birthdate = DateOnly.FromDateTime(new DateTime(2000, 1, 1)),
            City = "Test City",
            State = "Test State"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _controller.GetAllUsers();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var users = Assert.IsAssignableFrom<IEnumerable<UserSimplifiedResponse>>(okResult.Value);

        Assert.Single(users);
        Assert.Equal(user.Name, users.First().Name);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnNoContent()
    {
        await ResetDatabase();

        var user = new User
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "password",
            Birthdate = DateOnly.FromDateTime(new DateTime(2000, 1, 1)),
            City = "Test City",
            State = "Test State"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _controller.DeleteUser(user.Id);

        Assert.IsType<NoContentResult>(result);

        var deletedUser = await _context.Users.FindAsync(user.Id);
        Assert.Null(deletedUser);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnNoContent()
    {
        await ResetDatabase();

        var user = new User
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "password",
            Birthdate = DateOnly.FromDateTime(new DateTime(2000, 1, 1)),
            City = "Test City",
            State = "Test State"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var request = new UpdateUserRequest
        {
            Name = "Updated User",
            Email = "updated@example.com",
            City = "Updated City",
            State = "Updated State"
        };

        var result = await _controller.UpdateUser(user.Id, request);

        Assert.IsType<NoContentResult>(result);

        var updatedUser = await _context.Users.FindAsync(user.Id);
        Assert.Equal(request.Name, updatedUser.Name);
        Assert.Equal(request.Email, updatedUser.Email);
    }
}
