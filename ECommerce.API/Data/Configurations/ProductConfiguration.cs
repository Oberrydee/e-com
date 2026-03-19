using ECommerce.API.Modules.Products.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.API.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(p => p.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Price)
            .HasColumnType("numeric(18,2)");

        builder.Property(p => p.Quantity)
            .IsRequired();

        builder.Property(p => p.InternalReference)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(p => p.ShellId)
            .IsRequired();

        builder.Property(p => p.InventoryStatus)
            .HasConversion(
                status => ConvertInventoryStatusToString(status),
                value => ConvertStringToInventoryStatus(value))
            .HasMaxLength(32)
            .HasDefaultValue(Common.Enums.InventoryStatus.InStock);

        builder.Property(p => p.Rating)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.HasOne(p => p.File)
            .WithOne(f => f.Product)
            .HasForeignKey<ProductFile>(f => f.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static string ConvertInventoryStatusToString(Common.Enums.InventoryStatus status) => status switch
    {
        Common.Enums.InventoryStatus.InStock => "INSTOCK",
        Common.Enums.InventoryStatus.LowStock => "LOWSTOCK",
        Common.Enums.InventoryStatus.OutOfStock => "OUTOFSTOCK",
        _ => throw new InvalidOperationException("Unsupported inventory status.")
    };

    private static Common.Enums.InventoryStatus ConvertStringToInventoryStatus(string value) => value switch
    {
        "INSTOCK" => Common.Enums.InventoryStatus.InStock,
        "LOWSTOCK" => Common.Enums.InventoryStatus.LowStock,
        "OUTOFSTOCK" => Common.Enums.InventoryStatus.OutOfStock,
        _ => throw new InvalidOperationException("Unsupported inventory status.")
    };
}
