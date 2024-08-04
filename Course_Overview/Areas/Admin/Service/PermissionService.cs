using System.Data;
using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Service
{
	public class PermissionService : IPermissionRepository
	{
		private readonly DatabaseContext _databaseContext;
		public PermissionService(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
		}

		public async Task AddPermissionAsync(Permission permission)
		{
			await _databaseContext.Permissions.AddAsync(permission);
			await _databaseContext.SaveChangesAsync();
		}

		public async Task DeletePermissionAsync(int id)
		{
			var permission = await _databaseContext.Permissions.FindAsync(id);
			if (permission != null)
			{
				_databaseContext.Permissions.Remove(permission);
				await _databaseContext.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Permission>> GetAllPermissionAsync()
		{
			return await _databaseContext.Permissions.ToListAsync();
		}

		public async Task<Permission> GetOnePermissionAsync(int id)
		{
			return await _databaseContext.Permissions.FindAsync(id);
		}

		public async Task UpdatePermissionAsync(Permission permission)
		{
			_databaseContext.Permissions.Update(permission);
			await _databaseContext.SaveChangesAsync();
		}
	}
}
