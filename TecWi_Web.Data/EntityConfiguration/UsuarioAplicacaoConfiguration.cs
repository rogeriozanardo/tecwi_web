using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    class UsuarioAplicacaoConfiguration : IEntityTypeConfiguration<UsuarioAplicacao>
    {
        public void Configure(EntityTypeBuilder<UsuarioAplicacao> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => new {IdUsuario = x.IdUsuario, IdAplicacao = x.IdAplicacao, IdPerfil = x.IdPerfil});
            entityTypeBuilder.HasOne(x => x.Usuario)
                .WithMany(x => x.UsuarioAplicacao)
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.NoAction);
            entityTypeBuilder.Property(x => x.IdUsuario).IsRequired();
            entityTypeBuilder.Property(x => x.IdAplicacao).IsRequired();
            entityTypeBuilder.Property(x => x.IdPerfil).IsRequired();
        }
    }
}
