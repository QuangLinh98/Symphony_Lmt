using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LModels
{
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		[StringLength(250, ErrorMessage = "Name is required.")]
		public string Name { get; set; }

		[Required]
		[StringLength(350, ErrorMessage = "Address is required")]
		public string Address { get; set; }

		[Required]
		[Phone]
		[RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Phone number must start with 0 and be 10 to 11 digits long.")]
		public string Phone { get; set; }
		public string? Status { get; set; }

		public int FailedAttempts { get; set; }   // Thuộc tính để lưu trữ số lần đăng nhập thất bại
		public DateTime? LockoutEnd { get; set; } // Thời gian khóa tài khoản kết thúc

		public bool EmailConfirmed { get; set; }     //cho biết địa chỉ email của người dùng đã được xác nhận hay chưa.
		public string? EmailConfirmationToken { get; set; }   //lưu trữ mã thông báo được sử dụng để xác nhận địa chỉ email của người dùng

		public string? ResetPasswordToken { get; set; }      // MÃ token gửi cho User để Reset Password
		public DateTime? ResetPasswordTokenExpiration { get; set; }
		public bool IsNewUser { get; set; }                 // Chức năng: Để set điều kiện user mới đăng ký phải xác thực email mới cho phép login và user cần resetPassword có thể login sau khi reset mà không cần xác thực   

		public ICollection<UserRole> UserRoles { get; set; }
		public ICollection<UserPermission> UserPermissions { get; set; }
		

	}
}
