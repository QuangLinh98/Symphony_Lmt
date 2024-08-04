using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LModels
{
	public class Permission
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int PermissionId { get; set; }

		[Required]
		[MaxLength(50)]
		public string PermissionName { get; set; }

		[Required]
		[MaxLength(50)]
		public string PermissionCode { get; set; }

		public ICollection<RolePermission>? RolePermissions { get; set; }
		public ICollection<UserPermission>? UserPermissions { get; set; }
	}
}
