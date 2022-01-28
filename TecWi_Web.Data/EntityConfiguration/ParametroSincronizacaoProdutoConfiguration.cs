using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class ParametroSincronizacaoProdutoConfiguration : IEntityTypeConfiguration<ParametroSincronizacaoProduto>
    {
        public void Configure(EntityTypeBuilder<ParametroSincronizacaoProduto> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.ID).UseIdentityColumn();
            entityTypeBuilder.HasKey(x => x.ID);
            entityTypeBuilder.Property(x => x.UltimoUpdateDate).IsRequired();
            entityTypeBuilder.Property(x => x.DataHoraSincronizacao).IsRequired();
            entityTypeBuilder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
        }
    }
}
