using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Maps
{
    public class PurchaseMap : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.ToTable("Purchase");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .UseIdentityColumn();

            builder.Property(x => x.PersonId)
                .HasColumnName("PersonId");

            builder.Property(x => x.Date)
                .HasColumnName("DatePurchase");

            builder.HasOne(x => x.Person)
                .WithMany(p => p.Purchases)
                .HasForeignKey(p => p.PersonId);
        }
    }
}
