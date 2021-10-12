using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class ClienteContatoConfiguration : IEntityTypeConfiguration<ClienteContato>
    {
        public void Configure(EntityTypeBuilder<ClienteContato> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.IdClienteContato);
            entityTypeBuilder.Property(x => x.IdClienteContato).ValueGeneratedNever();
            entityTypeBuilder.HasOne(x => x.Cliente)
                .WithMany(x => x.ClienteContato)
                .HasForeignKey(x => x.Cdclifor);
            entityTypeBuilder.Property(x => x.Nome).HasMaxLength(400);
            entityTypeBuilder.Property(x => x.Email).HasMaxLength(400);
            entityTypeBuilder.Property(x => x.Telefone).HasMaxLength(20);
        }
    }
}
