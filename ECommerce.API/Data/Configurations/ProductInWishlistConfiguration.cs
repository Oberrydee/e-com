using ECommerce.API.Modules.Products.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.API.Data.Configurations;

public class ProductInWishlistConfiguration : IEntityTypeConfiguration<ProductInWishlist>
{
    public void Configure(EntityTypeBuilder<ProductInWishlist> builder)
    {
        builder.ToTable("ProductsInWishlist");

        builder.HasKey(productInWishlist => productInWishlist.Id);

        builder.HasIndex(productInWishlist => new { productInWishlist.WishlistId, productInWishlist.ProductId })
            .IsUnique();

        builder.Property(productInWishlist => productInWishlist.WishlistId)
            .IsRequired();

        builder.Property(productInWishlist => productInWishlist.ProductId)
            .IsRequired();

        builder.Property(productInWishlist => productInWishlist.CreatedAt)
            .IsRequired();

        builder.Property(productInWishlist => productInWishlist.UpdatedAt)
            .IsRequired();

        builder.HasOne(productInWishlist => productInWishlist.Wishlist)
            .WithMany(wishlist => wishlist.ProductsInWishlist)
            .HasForeignKey(productInWishlist => productInWishlist.WishlistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(productInWishlist => productInWishlist.Product)
            .WithMany(product => product.ProductsInWishlist)
            .HasForeignKey(productInWishlist => productInWishlist.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
