using ECommerce.API.Modules.Products.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.API.Data.Configurations;

public class ProductFileConfiguration : IEntityTypeConfiguration<ProductFile>
{
    public void Configure(EntityTypeBuilder<ProductFile> builder)
    {
        builder.ToTable("Files");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.FileName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(f => f.OriginalFileName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(f => f.ContentType)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(f => f.FilePath)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(f => f.BinaryContent)
            .IsRequired();

        builder.Property(f => f.SizeInBytes)
            .IsRequired();

        builder.Property(f => f.CreatedAt)
            .IsRequired();

        builder.Property(f => f.UpdatedAt)
            .IsRequired();

        builder.HasIndex(f => f.ProductId)
            .IsUnique();
    }
}
