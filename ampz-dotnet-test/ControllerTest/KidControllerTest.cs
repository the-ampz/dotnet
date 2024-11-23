using Xunit;
using Microsoft.EntityFrameworkCore;
using ampz_dotnet.Controllers;
using ampz_dotnet.Data;
using ampz_dotnet.Model;
using ampz_dotnet.Requests;
using ampz_dotnet.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class KidControllerTests
{
    private AppDbContext _context;
    private KidController _controller;

    public KidControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_Kid")
            .Options;

        _context = new AppDbContext(options);
        _controller = new KidController(_context);
    }

    private async Task ResetDatabase()
    {
        _context.Kids.RemoveRange(_context.Kids);
        _context.Users.RemoveRange(_context.Users);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetKidsByUserId_ShouldReturnKids()
    {
        await ResetDatabase();

        var user = new User
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "password",
            City = "Test City",
            State = "Test State"
        };

        var kid = new Kid
        {
            Name = "Test Kid",
            Birthdate = DateOnly.FromDateTime(new DateTime(2010, 1, 1)),
            TotalScore = 0,
            TotalEnergySaved = 0.0m,
            User = user
        };

        _context.Users.Add(user);
        _context.Kids.Add(kid);
        await _context.SaveChangesAsync();

        var result = await _controller.GetKidsByUserId(user.Id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var kids = Assert.IsAssignableFrom<IEnumerable<KidSimplifiedResponse>>(okResult.Value);

        Assert.Single(kids);
        Assert.Equal(kid.Name, kids.First().Name);
    }


    [Fact]
    public async Task GetKidsByUserId_ShouldReturnNotFoundForInvalidUser()
    {
        await ResetDatabase();

        var result = await _controller.GetKidsByUserId(999);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Nenhuma criança encontrada para o usuário com o ID 999", notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateKid_ShouldReturnNoContent()
    {
        await ResetDatabase();

        var kid = new Kid
        {
            Name = "Test Kid",
            Birthdate = DateOnly.FromDateTime(new DateTime(2010, 1, 1)),
            TotalScore = 0,
            TotalEnergySaved = 0.0m
        };

        _context.Kids.Add(kid);
        await _context.SaveChangesAsync();

        var request = new UpdateKidRequest
        {
            Name = "Updated Kid",
            Birthdate = DateOnly.FromDateTime(new DateTime(2011, 1, 1))
        };

        var result = await _controller.UpdateKid(kid.Id, request);

        Assert.IsType<NoContentResult>(result);

        var updatedKid = await _context.Kids.FindAsync(kid.Id);

        Assert.Equal(request.Name, updatedKid.Name);
        Assert.Equal(request.Birthdate, updatedKid.Birthdate);
    }

    [Fact]
    public async Task UpdateKid_ShouldReturnNotFoundForInvalidKid()
    {
        await ResetDatabase();

        var request = new UpdateKidRequest
        {
            Name = "Updated Kid",
            Birthdate = DateOnly.FromDateTime(new DateTime(2011, 1, 1))
        };

        var result = await _controller.UpdateKid(999, request);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Nenhuma criança encontrada com o ID 999", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteKid_ShouldReturnNoContent()
    {
        await ResetDatabase();

        var kid = new Kid
        {
            Name = "Test Kid",
            Birthdate = DateOnly.FromDateTime(new DateTime(2010, 1, 1)),
            TotalScore = 0,
            TotalEnergySaved = 0.0m
        };

        _context.Kids.Add(kid);
        await _context.SaveChangesAsync();

        var result = await _controller.DeleteKid(kid.Id);

        Assert.IsType<NoContentResult>(result);

        var deletedKid = await _context.Kids.FindAsync(kid.Id);
        Assert.Null(deletedKid);
    }

    [Fact]
    public async Task DeleteKid_ShouldReturnNotFoundForInvalidKid()
    {
        await ResetDatabase();

        var result = await _controller.DeleteKid(999);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Nenhuma criança encontrada com o ID 999.", notFoundResult.Value);
    }
}
