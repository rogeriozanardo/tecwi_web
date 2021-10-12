using Microsoft.EntityFrameworkCore;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DataContext() : base()
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
        public DbSet<ClienteContato> ClienteContato { get; set; }
        public DbSet<ContatoCobranca> ContatoCobranca { get; set; }
        public DbSet<ContatoCobrancaLancamento> ContatoCobrancaLancamento { get; set; }
        public DbSet<LogOperacao> LogOperacao { get; set; }
    }
}