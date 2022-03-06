using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class MovimentoFiscalConfiguration : IEntityTypeConfiguration<MovimentoFiscal>
    {
        public void Configure(EntityTypeBuilder<MovimentoFiscal> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.ID).UseIdentityColumn();
            entityTypeBuilder.HasKey(x => x.ID);

            entityTypeBuilder.Property(x => x.NumeroNota).IsRequired();
            entityTypeBuilder.Property(x => x.NumMovimento).IsRequired();
            entityTypeBuilder.Property(x => x.QtdVolume).IsRequired();
            entityTypeBuilder.Property(x => x.QtdVolume).HasPrecision(19, 8);
            entityTypeBuilder.Property(x => x.Serie).HasMaxLength(5).IsRequired();
            entityTypeBuilder.Property(x => x.ValorTotal).HasPrecision(19, 8).IsRequired();
        }
    }
}
