using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Service
{
	public class ContactService : IContactRepository
	{
		private readonly DatabaseContext _dbContext;
		public ContactService(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task AddContact(Contact contact)
		{
			await _dbContext.Contacts.AddAsync(contact);
			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteContact(int id)
		{
			var contact = await GetOneContact(id);
            if (contact != null)
            {
				_dbContext.Contacts.Remove(contact);
				await _dbContext.SaveChangesAsync();
			}        
		}

		public async Task<IEnumerable<Contact>> GetAllContact()
		{
			return await _dbContext.Contacts.ToListAsync();
		}

		public async Task<Contact> GetOneContact(int id)
		{
			var contactExisting = await _dbContext.Contacts.FindAsync(id);
			return contactExisting;
		}

		public async Task UpdateContact(Contact contact)
		{
			_dbContext.Contacts.Update(contact);
			await _dbContext.SaveChangesAsync();
		}
	}
}
