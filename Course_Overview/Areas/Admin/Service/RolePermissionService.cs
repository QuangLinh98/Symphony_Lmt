using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;

namespace Course_Overview.Areas.Admin.Service
{
	public class RolePermissionService : IRolePermissionrepository
	{
		private readonly DatabaseContext _databaseContext;
		public RolePermissionService(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
		}
		public async Task AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds)
		{
			var existingRolePermission = _databaseContext.RolePermissions.Where(rp => rp.RoleId == roleId).ToList();
			_databaseContext.RolePermissions.RemoveRange(existingRolePermission);

            foreach (var permissionId in permissionIds)
            {
				var rolePermission = new RolePermission
				{
					RoleId = roleId,
					PermissionId = permissionId
				};
				await _databaseContext.RolePermissions.AddAsync(rolePermission);
            }
			await _databaseContext.SaveChangesAsync();
        }
	}
}
