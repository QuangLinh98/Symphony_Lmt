using LModels;

namespace Course_Overview.Areas.Admin.Repository
{
	public interface IPermissionRepository
	{
		Task<IEnumerable<Permission>> GetAllPermissionAsync();
		Task<Permission> GetOnePermissionAsync(int id);
		Task AddPermissionAsync(Permission permission);
		Task UpdatePermissionAsync(Permission permission);
		Task DeletePermissionAsync(int id);
	}
}
