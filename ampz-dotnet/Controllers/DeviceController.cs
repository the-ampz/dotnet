using ampz_dotnet.Builders;
using ampz_dotnet.Data;
using ampz_dotnet.Requests;
using ampz_dotnet.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ampz_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Controller de Dispositivos")]
    public class DeviceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DeviceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("kid/{kidId}")]
        [SwaggerOperation(Summary = "Lista de dispositivos associados a uma criança específica")]
        public async Task<ActionResult<IEnumerable<DeviceSimplifiedResponse>>> GetDevicesByKidId(int kidId)
        {
            var devices = await _context.Devices
                .Where(d => d.KidId == kidId)
                .Select(d => DeviceSimplifiedResponse.From(d))
                .ToListAsync();

            if (!devices.Any())
            {
                return NotFound($"Nenhum dispositivo encontrado para a criança com o ID {kidId}");
            }

            return Ok(devices);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza as informações de um dispositivo com base no ID")]
        public async Task<IActionResult> UpdateDevice(int id, UpdateDeviceRequest request)
        {
            var device = await _context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound($"Nenhum dispositivo encontrado com o ID {id}");
            }

            device.UpdateInformation(request);
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove um dispositivo do sistema com base no ID")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var device = await _context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound($"Nenhum dispositivo encontrado com o ID {id}");
            }

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
