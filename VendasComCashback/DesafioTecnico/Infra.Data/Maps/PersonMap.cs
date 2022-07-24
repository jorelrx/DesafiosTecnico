using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Maps
{
    public class PersonMap : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person>  builder)
        {
            builder.ToTable("People");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .UseIdentityColumn();
            builder.Property(x => x.Name)
                .HasColumnName("Name");

            builder.Property(x => x.Document)
                .HasColumnName("Document");

            builder.Property(x => x.Phone)
                .HasColumnName("Phone");

            builder.HasMany(x => x.Purchases)
                .WithOne(p => p.Person)
                .HasForeignKey(p => p.PersonId);

        }
    }
}
