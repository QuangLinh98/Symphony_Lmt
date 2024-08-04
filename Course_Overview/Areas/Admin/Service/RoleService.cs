using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Service
{
	public class RoleService : IRoleRepository
	{
		private readonly DatabaseContext _databaseContext;
		public RoleService(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
		}
		public async Task AddRoleAsync(Role role)
		{
			await _databaseContext.Roles.AddAsync(role);
			await _databaseContext.SaveChangesAsync();
		}

		public async Task DeleteRoleAsync(int id)
		{
			var role = await _databaseContext.Roles.FindAsync(id);
			if (role != null)
			{
				_databaseContext.Roles.Remove(role);
				await _databaseContext.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Role>> GetAllRoleAsync()
		{
			return await _databaseContext.Roles.ToListAsync();
		}

		public async Task<Role> GetOneRoleAsync(int id)
		{
			return await _databaseContext.Roles.FindAsync(id);
		}

		public async Task UpdateRoleAsync(Role role)
		{
			_databaseContext.Roles.Update(role);
			await _databaseContext.SaveChangesAsync();
		}
	}
}
