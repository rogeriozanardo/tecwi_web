using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class PedidoItemConfiguration : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.ID).UseIdentityColumn();
            entityTypeBuilder.HasKey(x => x.ID);

            entityTypeBuilder.HasOne(x => x.Pedido)
              .WithMany(x => x.PedidoItem)
              .HasForeignKey(x => x.IDPedido)
              .OnDelete(DeleteBehavior.NoAction);

            entityTypeBuilder.Property(x => x.cdempresa).HasMaxLength(5);
            entityTypeBuilder.Property(x => x.cdfilial).HasMaxLength(2);
            entityTypeBuilder.Property(x => x.nummovimento).IsRequired();
            entityTypeBuilder.Property(x => x.seq).IsRequired();
            entityTypeBuilder.Property(x => x.cdproduto).HasMaxLength(25);
            entityTypeBuilder.Property(x => x.tpregistro).HasMaxLength(1);
            entityTypeBuilder.Property(x => x.qtdsolicitada).HasPrecision(19,8);
            entityTypeBuilder.Property(x => x.qtdprocessada).HasPrecision(19,8);
            entityTypeBuilder.Property(x => x.VlVenda).HasPrecision(19,10);
            entityTypeBuilder.Property(x => x.vlcalculado).HasPrecision(19, 10);
            entityTypeBuilder.Property(x => x.uporigem).HasMaxLength(10);
        }
    }
}
