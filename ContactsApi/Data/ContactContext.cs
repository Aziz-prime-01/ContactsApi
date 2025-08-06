using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Data;

public class ContactContext(DbContextOptions<ContactContext> options) : DbContext(options)
{
    public DbSet<Entities.Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}