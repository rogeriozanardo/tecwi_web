using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class EmpresaConfiguration : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.EmpresaId);
            entityTypeBuilder.Property(x => x.EmpresaId).IsRequired().ValueGeneratedNever(); 
            entityTypeBuilder.Property(x => x.CdCliente).IsRequired().ValueGeneratedNever();

            entityTypeBuilder.Property(x => x.Razao).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.Fantasia).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.Email).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.Cep).HasMaxLength(8);
            entityTypeBuilder.Property(x => x.Cidade).HasMaxLength(60);
            entityTypeBuilder.Property(x => x.Endereco).HasMaxLength(60);
            entityTypeBuilder.Property(x => x.Complemento).HasMaxLength(30);
            entityTypeBuilder.Property(x => x.Bairro).HasMaxLength(30);
            entityTypeBuilder.Property(x => x.DDD).HasMaxLength(5);
            entityTypeBuilder.Property(x => x.Fone1).HasMaxLength(15);
            entityTypeBuilder.Property(x => x.Fone2).HasMaxLength(15);
            entityTypeBuilder.Property(x => x.UF).HasMaxLength(2);
            entityTypeBuilder.Property(x => x.InscriEst).HasMaxLength(20);
            entityTypeBuilder.Property(x => x.InscriFed).HasMaxLength(14);
        }
    }
}
