using Microsoft.EntityFrameworkCore;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Context
{

//    add-migration InitialCreate -Context DataContext

//update-database -Context DataContext
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
        public DbSet<Produto> Produto { get; set; }
        public DbSet<ParametroSincronizacaoProduto> ParametroSincronizacaoProduto { get; set; }
        public DbSet<LogSincronizacaoProdutoMercoCamp> LogSincronizacaoProdutoMercoCamp { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<PedidoItem> PedidoItem { get; set; }
        public DbSet<PedidoMercoCamp> PedidoMercoCamp { get; set; }
        public DbSet<PedidoItemMercoCamp> PedidoItemMercoCamp { get; set; }
        public DbSet<Transportadora> Transportadora { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Vendedor> Vendedor { get; set; }
        public DbSet<MovimentoFiscal> MovimentoFiscal { get; set; }
        public DbSet<MovimentoFiscalItem> MovimentoFiscalItem { get; set; }
    }
}