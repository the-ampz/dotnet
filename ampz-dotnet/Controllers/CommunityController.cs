using ampz_dotnet.Model;
using ampz_dotnet.Builders;
using ampz_dotnet.Requests;
using ampz_dotnet.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using ampz_dotnet.Data;

namespace ampz_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Controller de Comunidades")]
    public class CommunityController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommunityController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cria uma nova comunidade")]
        public async Task<ActionResult<CommunityDetailedResponse>> CreateCommunity(CreateCommunityRequest request)
        {
            var community = new CommunityBuilder()
                .Name(request.Name)
                .Description(request.Description)
                .Build();

            _context.Communities.Add(community);
            await _context.SaveChangesAsync();

            var createdCommunity = CommunityDetailedResponse.From(community);

            return Ok(createdCommunity);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retorna uma lista com todas as comunidades")]
        public async Task<ActionResult<IEnumerable<CommunitySimplifiedResponse>>> GetAllCommunities()
        {
            var communities = await _context.Communities
                .Select(c => CommunitySimplifiedResponse.From(c))
                .ToListAsync();

            return Ok(communities);
        }

        [HttpPost("{communityId}/add-kid/{kidId}")]
        [SwaggerOperation(Summary = "Adiciona uma criança a uma comunidade")]
        public async Task<IActionResult> AddKidToCommunity(int communityId, int kidId)
        {
            var community = await _context.Communities.Include(c => c.Kids).FirstOrDefaultAsync(c => c.Id == communityId);
            if (community == null)
            {
                return NotFound($"Comunidade com Id {communityId} não encontrada.");
            }

            var kid = await _context.Kids.FirstOrDefaultAsync(k => k.Id == kidId);
            if (kid == null)
            {
                return NotFound($"Criança com Id {kidId} não encontrada.");
            }

            if (!community.Kids.Contains(kid))
            {
                community.Kids.Add(kid);
                await _context.SaveChangesAsync();
            }

            return Ok($"Criança com Id {kidId} adicionada à comunidade com Id {communityId}.");
        }

    }
}
