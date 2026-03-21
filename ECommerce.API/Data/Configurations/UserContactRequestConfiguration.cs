using ECommerce.API.Common.Enums;
using ECommerce.API.Modules.Contact.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.API.Data.Configurations;

public class UserContactRequestConfiguration : IEntityTypeConfiguration<UserContactRequest>
{
    public void Configure(EntityTypeBuilder<UserContactRequest> builder)
    {
        builder.ToTable("user_contact_request");

        builder.HasKey(contactRequest => contactRequest.Id);

        builder.Property(contactRequest => contactRequest.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(contactRequest => contactRequest.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(contactRequest => contactRequest.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(contactRequest => contactRequest.Message)
            .HasColumnName("message")
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(contactRequest => contactRequest.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(contactRequest => contactRequest.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .IsRequired()
            .HasDefaultValue(ContactRequestStatus.New);
    }
}
