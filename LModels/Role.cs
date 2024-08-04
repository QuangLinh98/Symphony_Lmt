using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LModels
{
	public class Role
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int RoleId { get; set; }

		[Required]
		[MaxLength(50)] 
		public string RoleName { get; set; }

		[Required]
		[MaxLength(20)]
		public string RoleCode { get; set; }

		public ICollection<UserRole>? UserRoles { get; set; } = new List<UserRole>();
		public ICollection<RolePermission>? RolePermissions { get; set; } = new List<RolePermission>();
	}
}
