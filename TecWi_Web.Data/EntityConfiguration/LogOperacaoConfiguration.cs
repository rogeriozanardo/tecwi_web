using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    class LogOperacaoConfiguration : IEntityTypeConfiguration<LogOperacao>
    {
        public void Configure(EntityTypeBuilder<LogOperacao> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.IdLogOperacao);
            entityTypeBuilder.HasOne(x => x.Usuario)
                .WithMany(x => x.LogOperacao)
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}