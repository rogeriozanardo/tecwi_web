using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    class ContatoCobrancaLancamentoConfiguration : IEntityTypeConfiguration<ContatoCobrancaLancamento>
    {
        public void Configure(EntityTypeBuilder<ContatoCobrancaLancamento> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => new { idContato = x.IdContato, numlancto = x.Numlancto, sq = x.Sq, cdfilial = x.Cdfilial });

            entityTypeBuilder.HasOne(x => x.PagarReceber)
                .WithMany(x => x.ContatoCobrancaLancamento)
                .HasForeignKey(x => new { numlancto = x.Numlancto, sq = x.Sq, cdfilial = x.Cdfilial })
                .OnDelete(DeleteBehavior.NoAction);

            entityTypeBuilder.HasOne(x => x.ContatoCobranca)
                .WithMany(x => x.ContatoCobrancaLancamento)
                .HasForeignKey(x => x.IdContato)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
