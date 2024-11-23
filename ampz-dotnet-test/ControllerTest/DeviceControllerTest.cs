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

public class DeviceControllerTests
{
    private AppDbContext _context;
    private DeviceController _controller;

    public DeviceControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_Device")
            .Options;

        _context = new AppDbContext(options);
        _controller = new DeviceController(_context);
    }

    private async Task ResetDatabase()
    {
        _context.Devices.RemoveRange(_context.Devices);
        _context.Kids.RemoveRange(_context.Kids);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetDevicesByKidId_ShouldReturnDevices()
    {
        await ResetDatabase();

        var kid = new Kid
        {
            Name = "Test Kid",
            Birthdate = DateOnly.FromDateTime(new DateTime(2010, 1, 1))
        };

        var device = new Device
        {
            Name = "Test Device",
            Type = "Smartphone",
            OperatingSystem = "Android",
            Kid = kid
        };

        _context.Kids.Add(kid);
        _context.Devices.Add(device);
        await _context.SaveChangesAsync();

        var result = await _controller.GetDevicesByKidId(kid.Id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var devices = Assert.IsAssignableFrom<IEnumerable<DeviceSimplifiedResponse>>(okResult.Value);

        Assert.Single(devices);
        Assert.Equal(device.Name, devices.First().Name);
    }

    [Fact]
    public async Task GetDevicesByKidId_ShouldReturnNotFoundForInvalidKid()
    {
        await ResetDatabase();

        var result = await _controller.GetDevicesByKidId(999);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Nenhum dispositivo encontrado para a criança com o ID 999", notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateDevice_ShouldReturnNoContent()
    {
        await ResetDatabase();

        var device = new Device
        {
            Name = "Test Device",
            Type = "Smartphone",
            OperatingSystem = "Android"
        };

        _context.Devices.Add(device);
        await _context.SaveChangesAsync();

        var request = new UpdateDeviceRequest
        {
            Name = "Updated Device",
            Type = "Tablet",
            OperatingSystem = "iOS"
        };

        var result = await _controller.UpdateDevice(device.Id, request);

        Assert.IsType<NoContentResult>(result);

        var updatedDevice = await _context.Devices.FindAsync(device.Id);

        Assert.Equal(request.Name, updatedDevice.Name);
        Assert.Equal(request.Type, updatedDevice.Type);
        Assert.Equal(request.OperatingSystem, updatedDevice.OperatingSystem);
    }

    [Fact]
    public async Task UpdateDevice_ShouldReturnNotFoundForInvalidDevice()
    {
        await ResetDatabase();

        var request = new UpdateDeviceRequest
        {
            Name = "Updated Device",
            Type = "Tablet",
            OperatingSystem = "iOS"
        };

        var result = await _controller.UpdateDevice(999, request);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Nenhum dispositivo encontrado com o ID 999", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteDevice_ShouldReturnNoContent()
    {
        await ResetDatabase();

        var device = new Device
        {
            Name = "Test Device",
            Type = "Smartphone",
            OperatingSystem = "Android"
        };

        _context.Devices.Add(device);
        await _context.SaveChangesAsync();

        var result = await _controller.DeleteDevice(device.Id);

        Assert.IsType<NoContentResult>(result);

        var deletedDevice = await _context.Devices.FindAsync(device.Id);
        Assert.Null(deletedDevice);
    }

    [Fact]
    public async Task DeleteDevice_ShouldReturnNotFoundForInvalidDevice()
    {
        await ResetDatabase();

        var result = await _controller.DeleteDevice(999);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Nenhum dispositivo encontrado com o ID 999", notFoundResult.Value);
    }
}
