using Course_Overview.Data;
using LModels;

namespace Course_Overview.Utility
{
	public static class DataSeeder
	{
		public static async Task SeedRolePermissions(DatabaseContext context)
		{
            if (!context.RolePermissions.Any())
            {
				var adminRole = context.Roles.FirstOrDefault(r => r.RoleCode == "ADMIN");    // Seeder dữ liệu ban đầu lad Admin   
				var allPermissions = context.Permissions.ToList();                                  // Gán Admin cho tất cả các quyền có thể truy cập vào các action
                 
                if (adminRole != null)
                {
                    foreach (var permission in allPermissions)
                    {
                        var rolePermission = new RolePermission
                        {
                            RoleId = adminRole.RoleId,
                            PermissionId = permission.PermissionId,
                        };
                        await context.RolePermissions.AddAsync(rolePermission);
                    }
                    await context.SaveChangesAsync();
                }
            }
        }
	}
}
