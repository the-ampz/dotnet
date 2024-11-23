using ampz_dotnet.Builders;
using ampz_dotnet.Requests;
using ampz_dotnet.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ampz_dotnet.Data;
using Microsoft.EntityFrameworkCore;
using ampz_dotnet.Model;

namespace ampz_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Gerenciamento de Usuários")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cria um novo usuário com crianças e dispositivos associados")]
        public async Task<ActionResult<UserSimplifiedResponse>> CreateUser(CreateUserRequest request)
        {
            var user = new UserBuilder()
                .Name(request.Name)
                .Email(request.Email)
                .Password(BCrypt.Net.BCrypt.HashPassword(request.Password))
                .Birthdate(request.Birthdate)
                .City(request.City)
                .State(request.State)
                .Build();

            if (request.Kids != null && request.Kids.Any())
            {
                user.Kids = request.Kids.Select(kidRequest =>
                {
                    var kid = new Kid
                    {
                        Name = kidRequest.Name,
                        Birthdate = kidRequest.Birthdate,
                        TotalScore = 0,
                        TotalEnergySaved = 0.0m
                    };

                    if (kidRequest.Devices != null && kidRequest.Devices.Any())
                    {
                        kid.Devices = kidRequest.Devices.Select(deviceRequest => new Device
                        {
                            Name = deviceRequest.Name,
                            Type = deviceRequest.Type,
                            OperatingSystem = deviceRequest.OperatingSystem
                        }).ToList();
                    }

                    return kid;
                }).ToList();
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var createdUser = UserSimplifiedResponse.From(user);

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os usuários cadastrados")]
        public async Task<ActionResult<IEnumerable<UserSimplifiedResponse>>> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => UserSimplifiedResponse.From(u))
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtém os detalhes de um usuário pelo ID")]
        public async Task<ActionResult<UserDetailedResponse>> GetUserById(int id)
        {
            var user = await _context.Users
                .Include(u => u.Kids)
                .ThenInclude(k => k.Devices)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound($"Nenhum usuário encontrado com o ID {id}.");
            }

            return Ok(UserDetailedResponse.From(user));
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza as informações de um usuário pelo ID")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserRequest request)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound($"Nenhum usuário encontrado com o ID {id}");
            }

            user.UpdateInformation(request);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um usuário pelo ID")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Kids)
                .ThenInclude(k => k.Devices)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound($"Nenhum usuário encontrado com o ID {id}");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
