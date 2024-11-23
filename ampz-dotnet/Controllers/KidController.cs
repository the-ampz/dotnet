using ampz_dotnet.Builders;
using ampz_dotnet.Data;
using ampz_dotnet.Model;
using ampz_dotnet.Requests;
using ampz_dotnet.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace ampz_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Controller de Crianças")]
    public class KidController : ControllerBase
    {
        private readonly AppDbContext _context;

        public KidController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("user/{userId}")]
        [SwaggerOperation(Summary = "Lista de crianças vinculadas a um usuário específico")]
        public async Task<ActionResult<IEnumerable<KidSimplifiedResponse>>> GetKidsByUserId(int userId)
        {
            var kids = await _context.Kids
                .Where(k => k.UserId == userId)
                .Select(k => KidSimplifiedResponse.From(k))
                .ToListAsync();

            if (!kids.Any())
            {
                return NotFound($"Nenhuma criança encontrada para o usuário com o ID {userId}");
            }

            return Ok(kids);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza os dados de uma criança com base no ID")]
        public async Task<IActionResult> UpdateKid(int id, UpdateKidRequest request)
        {
            var kid = await _context.Kids.FindAsync(id);

            if (kid == null)
            {
                return NotFound($"Nenhuma criança encontrada com o ID {id}");
            }

            kid.UpdateInformation(request);
            _context.Kids.Update(kid);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui uma criança do sistema com base no ID")]
        public async Task<IActionResult> DeleteKid(int id)
        {
            var kid = await _context.Kids.FindAsync(id);

            if (kid == null)
            {
                return NotFound($"Nenhuma criança encontrada com o ID {id}.");
            }

            _context.Kids.Remove(kid);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
