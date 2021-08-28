using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.IdUsuario);
            entityTypeBuilder.Property(x => x.Nome).IsRequired();
            entityTypeBuilder.Property(x => x.Login).IsRequired();
            entityTypeBuilder.Property(x => x.Email).IsRequired();
            entityTypeBuilder.Property(x => x.SenhaHash).IsRequired();
            entityTypeBuilder.Property(x => x.SenhaSalt).IsRequired();
        }
    }
}
