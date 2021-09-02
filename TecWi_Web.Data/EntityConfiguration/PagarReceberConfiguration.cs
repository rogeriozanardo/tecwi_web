using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class PagarReceberConfiguration : IEntityTypeConfiguration<PagarReceber>
    {
        public void Configure(EntityTypeBuilder<PagarReceber> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => new { numlancto = x.numlancto, sq = x.sq, cdfilial = x.cdfilial });
            entityTypeBuilder.HasOne(x => x.cliente)
                .WithMany(x => x.PagarReceber)
                .OnDelete(DeleteBehavior.Restrict);
            entityTypeBuilder.Property(x => x.valorr)
                .HasPrecision(18, 2);
            entityTypeBuilder.Ignore(x => x.inscrifed);
            entityTypeBuilder.Ignore(x => x.fantasia);
            entityTypeBuilder.Ignore(x => x.razao);
            entityTypeBuilder.Ignore(x => x.ddd);
            entityTypeBuilder.Ignore(x => x.fone1);
            entityTypeBuilder.Ignore(x => x.fone2);
            entityTypeBuilder.Ignore(x => x.email);
            entityTypeBuilder.Ignore(x => x.cidade);
        }
    }
}