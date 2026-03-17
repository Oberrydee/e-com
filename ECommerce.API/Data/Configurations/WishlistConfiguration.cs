using ECommerce.API.Modules.Products.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.API.Data.Configurations;

public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        builder.ToTable("Wishlists");

        builder.HasKey(wishlist => wishlist.Id);

        builder.Property(wishlist => wishlist.UserId)
            .IsRequired();

        builder.Property(wishlist => wishlist.Label)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(wishlist => wishlist.CreatedAt)
            .IsRequired();

        builder.Property(wishlist => wishlist.UpdatedAt)
            .IsRequired();

        builder.HasOne(wishlist => wishlist.User)
            .WithMany(user => user.Wishlists)
            .HasForeignKey(wishlist => wishlist.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
