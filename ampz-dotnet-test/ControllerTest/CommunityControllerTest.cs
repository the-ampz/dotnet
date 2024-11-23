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

public class CommunityControllerTests
{
    private AppDbContext _context;
    private CommunityController _controller;

    public CommunityControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_Community")
            .Options;

        _context = new AppDbContext(options);
        _controller = new CommunityController(_context);
    }

    private async Task ResetDatabase()
    {
        _context.Communities.RemoveRange(_context.Communities);
        _context.Kids.RemoveRange(_context.Kids);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task CreateCommunity_ShouldReturnCreatedCommunity()
    {
        await ResetDatabase();

        var request = new CreateCommunityRequest
        {
            Name = "Test Community",
            Description = "A test description"
        };

        var result = await _controller.CreateCommunity(request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<CommunityDetailedResponse>(okResult.Value);

        Assert.Equal(request.Name, response.Name);
        Assert.Equal(request.Description, response.Description);
        Assert.Single(_context.Communities);
    }

    [Fact]
    public async Task GetAllCommunities_ShouldReturnListOfCommunities()
    {
        await ResetDatabase();

        var community = new Community
        {
            Name = "Test Community",
            Description = "A test description"
        };

        _context.Communities.Add(community);
        await _context.SaveChangesAsync();

        var result = await _controller.GetAllCommunities();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var communities = Assert.IsAssignableFrom<IEnumerable<CommunitySimplifiedResponse>>(okResult.Value);

        Assert.Single(communities);
        Assert.Equal(community.Name, communities.First().Name);
    }

    [Fact]
    public async Task AddKidToCommunity_ShouldAddKidAndReturnOk()
    {
        await ResetDatabase();

        var community = new Community
        {
            Name = "Test Community",
            Description = "A test description",
            Kids = new List<Kid>()
        };

        var kid = new Kid
        {
            Name = "Test Kid",
            Birthdate = DateOnly.FromDateTime(new DateTime(2010, 1, 1)),
            TotalScore = 0,
            TotalEnergySaved = 0.0m
        };

        _context.Communities.Add(community);
        _context.Kids.Add(kid);
        await _context.SaveChangesAsync();

        var result = await _controller.AddKidToCommunity(community.Id, kid.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var updatedCommunity = await _context.Communities.Include(c => c.Kids).FirstOrDefaultAsync(c => c.Id == community.Id);

        Assert.NotNull(updatedCommunity);
        Assert.Contains(updatedCommunity.Kids, k => k.Id == kid.Id);
        Assert.Equal($"Criança com Id {kid.Id} adicionada à comunidade com Id {community.Id}.", okResult.Value);
    }

    [Fact]
    public async Task AddKidToCommunity_ShouldReturnNotFoundForInvalidCommunity()
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

        var result = await _controller.AddKidToCommunity(999, kid.Id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Comunidade com Id 999 não encontrada.", notFoundResult.Value);
    }

    [Fact]
    public async Task AddKidToCommunity_ShouldReturnNotFoundForInvalidKid()
    {
        await ResetDatabase();

        var community = new Community
        {
            Name = "Test Community",
            Description = "A test description",
            Kids = new List<Kid>()
        };

        _context.Communities.Add(community);
        await _context.SaveChangesAsync();

        var result = await _controller.AddKidToCommunity(community.Id, 999);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Criança com Id 999 não encontrada.", notFoundResult.Value);
    }
}
