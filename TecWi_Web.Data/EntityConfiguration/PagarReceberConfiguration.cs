using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class PagarReceberConfiguration : IEntityTypeConfiguration<PagarReceber>
    {
        public void Configure(EntityTypeBuilder<PagarReceber> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => new { numlancto = x.Numlancto, sq = x.Sq, cdfilial = x.Cdfilial });
            entityTypeBuilder.HasOne(x => x.Cliente)
                .WithMany(x => x.PagarReceber)
                .HasForeignKey(x => x.Cdclifor)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);
            entityTypeBuilder.Property(x => x.Valorr)
                .HasPrecision(18, 2);
            entityTypeBuilder.Ignore(x => x.Inscrifed);
            entityTypeBuilder.Ignore(x => x.Fantasia);
            entityTypeBuilder.Ignore(x => x.Razao);
            entityTypeBuilder.Ignore(x => x.DDD);
            entityTypeBuilder.Ignore(x => x.Fone1);
            entityTypeBuilder.Ignore(x => x.Fone2);
            entityTypeBuilder.Ignore(x => x.Email);
            entityTypeBuilder.Ignore(x => x.Cidade);
            entityTypeBuilder.Ignore(x => x.Cdclifor);
        }
    }
}