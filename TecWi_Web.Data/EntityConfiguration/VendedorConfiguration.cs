using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class VendedorConfiguration : IEntityTypeConfiguration<Vendedor>
    {
        public void Configure(EntityTypeBuilder<Vendedor> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.IdVendedor);
            entityTypeBuilder.Property(x => x.IdVendedor).UseIdentityColumn();
            entityTypeBuilder.Property(x => x.CdVendedor).IsRequired().HasMaxLength(10);
            entityTypeBuilder.Property(x => x.Apelido).HasMaxLength(20);
            entityTypeBuilder.Property(x => x.Nome).HasMaxLength(40);
        }
    }
}
