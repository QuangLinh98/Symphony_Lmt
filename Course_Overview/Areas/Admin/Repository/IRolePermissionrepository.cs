namespace Course_Overview.Areas.Admin.Repository
{
	public interface IRolePermissionrepository
	{
		Task AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds);
	}
}
