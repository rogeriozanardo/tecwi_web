using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class PedidoItemMercoCampConfiguration : IEntityTypeConfiguration<PedidoItemMercoCamp>
    {
        public void Configure(EntityTypeBuilder<PedidoItemMercoCamp> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.IdPedidoItem).UseIdentityColumn();
            entityTypeBuilder.HasKey(x => x.IdPedidoItem);

            entityTypeBuilder.Property(x => x.Qtd).HasPrecision(19, 8);
            entityTypeBuilder.Property(x => x.QtdSeparado).HasPrecision(19, 8);

            entityTypeBuilder.HasOne(x => x.PedidoMercoCamp)
             .WithMany(x => x.PedidoItens)
             .HasForeignKey(x => x.PedidoMercoCampId)
             .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
