using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class LogSincronizacaoProdutoMercoCampConfiguration : IEntityTypeConfiguration<LogSincronizacaoProdutoMercoCamp>
    {
        public void Configure(EntityTypeBuilder<LogSincronizacaoProdutoMercoCamp> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.ID).UseIdentityColumn();
            entityTypeBuilder.HasKey(x => x.ID);
            entityTypeBuilder.Property(x => x.InicioSincronizacao).IsRequired();
            entityTypeBuilder.Property(x => x.PeriodoInicialEnvio).IsRequired();
            entityTypeBuilder.Property(x => x.PeriodoFinalEnvio).IsRequired();
            entityTypeBuilder.Property(x => x.JsonEnvio).IsRequired();
        }
    }
}
