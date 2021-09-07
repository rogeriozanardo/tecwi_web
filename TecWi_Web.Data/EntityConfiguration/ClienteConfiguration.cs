using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Cdclifor);
            entityTypeBuilder.Property(x => x.Cdclifor).ValueGeneratedNever();
            entityTypeBuilder.HasOne(x => x.Usuario)
                .WithMany(x => x.Cliente)
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}