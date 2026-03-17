using ECommerce.API.Modules.Products.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.API.Data.Configurations;

public class ProductInCartConfiguration : IEntityTypeConfiguration<ProductInCart>
{
    public void Configure(EntityTypeBuilder<ProductInCart> builder)
    {
        builder.ToTable("ProductsInCart");

        builder.HasKey(productInCart => productInCart.Id);

        builder.HasIndex(productInCart => new { productInCart.CartId, productInCart.ProductId })
            .IsUnique();

        builder.Property(productInCart => productInCart.CartId)
            .IsRequired();

        builder.Property(productInCart => productInCart.ProductId)
            .IsRequired();

        builder.Property(productInCart => productInCart.Quantity)
            .IsRequired();

        builder.Property(productInCart => productInCart.CreatedAt)
            .IsRequired();

        builder.Property(productInCart => productInCart.UpdatedAt)
            .IsRequired();

        builder.HasOne(productInCart => productInCart.Cart)
            .WithMany(cart => cart.ProductsInCart)
            .HasForeignKey(productInCart => productInCart.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(productInCart => productInCart.Product)
            .WithMany(product => product.ProductsInCart)
            .HasForeignKey(productInCart => productInCart.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
