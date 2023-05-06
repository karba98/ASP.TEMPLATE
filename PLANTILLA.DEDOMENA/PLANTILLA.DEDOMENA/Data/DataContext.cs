using Microsoft.EntityFrameworkCore;
using PLANTIILLA.DEDOMENA.Models;

namespace PLANTIILLA.DEDOMENA.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
       
        public DbSet<Empleo> Empleos { get; set; }
        public DbSet<EmpleoBR> EmpleosBR { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Fichero> Documentos { get; set; }
        public DbSet<User> Usuarios { get; set; }
    }
}
