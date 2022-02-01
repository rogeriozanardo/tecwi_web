using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class TransportadoraConfiguration : IEntityTypeConfiguration<Transportadora>
    {
        public void Configure(EntityTypeBuilder<Transportadora> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.IdTransportadora);
            entityTypeBuilder.Property(x => x.IdTransportadora).UseIdentityColumn();
            entityTypeBuilder.Property(x => x.CdTransportadora).IsRequired().HasMaxLength(5);
            entityTypeBuilder.Property(x => x.Cidade).HasMaxLength(30);
            entityTypeBuilder.Property(x => x.Email).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.Fantasia).HasMaxLength(40);
            entityTypeBuilder.Property(x => x.Inscrifed).HasMaxLength(14);
            entityTypeBuilder.Property(x => x.Nome).HasMaxLength(40);
            entityTypeBuilder.Property(x => x.Tpinscricao).HasMaxLength(1);
        }
    }
}
