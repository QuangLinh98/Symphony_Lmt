using LModels;

namespace Course_Overview.Areas.Admin.Viewmodels
{
	public class PermissionAssignmentViewModel
	{
		public int UserId { get; set; }
		public string? UserName { get; set; }
		public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public List<int> GrantedPermissions { get; set; } = new List<int>(); // Danh sách quyền đã được gán
    }
}
