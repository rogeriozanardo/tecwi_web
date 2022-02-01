using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class PedidoMercoCampConfiguration : IEntityTypeConfiguration<PedidoMercoCamp>
    {
        public void Configure(EntityTypeBuilder<PedidoMercoCamp> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.IdPedidoMercoCamp).UseIdentityColumn();
            entityTypeBuilder.HasKey(x => x.IdPedidoMercoCamp);

            entityTypeBuilder.Property(x => x.Peso).HasPrecision(19,10);
        }
    }
}
