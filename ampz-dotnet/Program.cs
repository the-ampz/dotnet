using ampz_dotnet.Data;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Back-end da Ampz desenvolvido em ASP.NET - Comprovando o funcionamento das pipelines CI/CD",
        Description = "API em C# que permite gerenciar dispositivos, usuários e comunidades. O Ampz combina tecnologia IoT e educação infantil para incentivar hábitos sustentáveis, utilizando dispositivos instalados nos quartos das crianças para monitorar o consumo de energia em tempo real. De forma lúdica e interativa, transforma o aprendizado sobre economia de energia em uma experiência divertida, com desafios e recompensas que ensinam a importância de práticas sustentáveis.",
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger(c =>
{
    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
    {
        httpReq.HttpContext.Response.Headers.Append("Content-Type", "application/json; charset=utf-8");
    });
});

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();