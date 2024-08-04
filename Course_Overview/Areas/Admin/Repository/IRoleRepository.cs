using LModels;

namespace Course_Overview.Areas.Admin.Repository
{
	public interface IRoleRepository
	{
		Task<IEnumerable<Role>> GetAllRoleAsync();
		Task<Role> GetOneRoleAsync(int id);
		Task AddRoleAsync(Role role);
		Task UpdateRoleAsync(Role role);
		Task DeleteRoleAsync(int id);
	}
}
