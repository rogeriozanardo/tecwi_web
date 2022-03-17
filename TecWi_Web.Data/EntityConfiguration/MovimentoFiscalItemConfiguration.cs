using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class MovimentoFiscalItemConfiguration : IEntityTypeConfiguration<MovimentoFiscalItem>
    {
        public void Configure(EntityTypeBuilder<MovimentoFiscalItem> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.ID).UseIdentityColumn();
            entityTypeBuilder.HasKey(x => x.ID);

            entityTypeBuilder.HasOne(x => x.MovimentoFiscal)
              .WithMany(x => x.ItensMovimentoFiscal)
              .HasForeignKey(x => x.IDMovimentoFiscal)
              .OnDelete(DeleteBehavior.NoAction);

            entityTypeBuilder.Property(x => x.Sequencia).IsRequired();
            entityTypeBuilder.Property(x => x.IDMovimentoFiscal).IsRequired();
            entityTypeBuilder.Property(x => x.Qtd).HasPrecision(19, 8);
            entityTypeBuilder.Property(x => x.CdProduto).IsRequired();
            entityTypeBuilder.Property(x => x.CdProduto).HasMaxLength(25);
        }
    }
}
