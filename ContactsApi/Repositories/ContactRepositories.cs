using Microsoft.EntityFrameworkCore;
using Npgsql;
using ContactsApi.Data;
using ContactsApi.Exceptions;
using ContactsApi.Repositories.Abstraction;
namespace ContactsApi.Repositories;

public class ContactRepositories(ContactContext context) : IContactRepository
{
    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var effectedRows = await context.Contacts
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        if (effectedRows < 1)
            throw new CustomNotFoundException($"Contact with id {id} not found.");
    }

    public async ValueTask<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
        => await context.Contacts.AnyAsync(x => x.Email == email, cancellationToken);

    public async ValueTask<IEnumerable<Entities.Contact>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.Contacts.Where(x => x.State != Entities.ContactState.Deleted).ToListAsync(cancellationToken);

    public async ValueTask<Entities.Contact> GetSingleAsync(int id, CancellationToken cancellationToken = default)
        => await GetSingleOrDefaultAsync(id, cancellationToken)
            ?? throw new CustomNotFoundException("Contact not found");

    public async ValueTask<Entities.Contact?> GetSingleOrDefaultAsync(int id, CancellationToken cancellationToken = default)
        => await context.Contacts.FindAsync([ id ], cancellationToken);

    public async ValueTask<Entities.Contact> InsertAsync(Entities.Contact contact, CancellationToken cancellationToken = default)
    {
        try
        {
            contact.CreatedAt = DateTimeOffset.UtcNow;
            contact.UpdatedAt = DateTimeOffset.UtcNow;

            var entry = context.Contacts.Add(contact);
            await context.SaveChangesAsync(cancellationToken);

            return entry.Entity;
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" }) 
        {
            throw new CustomConflictException("Email must be unique");
        }
    }

    public async ValueTask<Entities.Contact> UpdateAsync(Entities.Contact contact, CancellationToken cancellationToken = default)
    {
        try
        {
            contact.UpdatedAt = DateTimeOffset.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" }) 
        {
            throw new CustomConflictException("Email must be unique");
        }

        return contact;
    }
}