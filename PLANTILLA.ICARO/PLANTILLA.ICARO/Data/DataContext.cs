using Microsoft.EntityFrameworkCore;
using PLANTILLA.ICARO.Models;

namespace PLANTILLA.ICARO.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Usuarios { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}
