using ampz_dotnet.Model;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace ampz_dotnet.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Kid> Kids { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Community> Communities { get; set; }
    }
}
