using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Maps
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .UseIdentityColumn();

            builder.Property(x => x.Name)
                .HasColumnName("Name");

            builder.Property(x => x.CodErp)
                .HasColumnName("CodErp");

            builder.Property(x => x.Price)
                .HasColumnName("Price");

            builder.Property(x => x.ValueCashBack)
                .HasColumnName("ValueCashBack");

            builder.HasMany(x => x.Purchases)
                .WithMany(p => p.Products);
        }
    }
}
