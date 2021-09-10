using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.EntityConfiguration
{
    public class ContatoCobrancaConfiguration : IEntityTypeConfiguration<ContatoCobranca>
    {
        public void Configure(EntityTypeBuilder<ContatoCobranca> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.IdContato);
        }
    }
}
