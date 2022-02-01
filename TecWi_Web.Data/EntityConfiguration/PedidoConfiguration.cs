using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.ID).UseIdentityColumn();
            entityTypeBuilder.HasKey(x => x.ID);

            entityTypeBuilder.Property(x => x.cdempresa).HasMaxLength(5);
            entityTypeBuilder.Property(x => x.cdfilial).HasMaxLength(2);
            entityTypeBuilder.Property(x => x.nummovimento).IsRequired();
            entityTypeBuilder.Property(x => x.cdcliente).HasMaxLength(7);
            entityTypeBuilder.Property(x => x.nummovcliente).HasMaxLength(60);
            entityTypeBuilder.Property(x => x.stpendencia).HasMaxLength(1);
            entityTypeBuilder.Property(x => x.cdvendedor).HasMaxLength(10);
            entityTypeBuilder.Property(x => x.stpagafrete).HasMaxLength(1);
            entityTypeBuilder.Property(x => x.cdtransportadora).HasMaxLength(5);
            entityTypeBuilder.Property(x => x.cdtransportadoraredespacho).HasMaxLength(5);
            entityTypeBuilder.Property(x => x.stpagafreteredespacho).HasMaxLength(1);

           
        }
    }
}
