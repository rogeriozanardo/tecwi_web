using Microsoft.EntityFrameworkCore;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<UsuarioAplicacao> UsuarioAplicacao { get; set; }
        public DbSet<PagarReceber> PagarReceber { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
    }
}