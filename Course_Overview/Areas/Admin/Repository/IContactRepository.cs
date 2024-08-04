using LModels;

namespace Course_Overview.Areas.Admin.Repository
{
	public interface IContactRepository
	{
		Task<IEnumerable<Contact>> GetAllContact();
		Task<Contact> GetOneContact(int id);
		Task AddContact(Contact contact);
		Task UpdateContact(Contact contact);
		Task DeleteContact(int id);
	}
}
