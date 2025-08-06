using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Entities.Configuration;
public class ContactContext : IEntityTypeConfiguration<Contact>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.PhoneNumber);
        builder.Property(c => c.FirstName).HasMaxLength(30);
        builder.Property(c => c.LastName).HasMaxLength(30);
        builder.Property(c => c.PhoneNumber).IsRequired();
        builder.Property(c => c.Email).IsRequired();
    }
}