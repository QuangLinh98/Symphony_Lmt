using System.ComponentModel.DataAnnotations;

namespace Course_Overview.Areas.Admin.Viewmodels
{
	public class UserCreateViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[StringLength(250, ErrorMessage = "Name is required")]
		public string Name { get; set; }

		[Required]
		[StringLength(350, ErrorMessage = "Address is required")]
		public string Address { get; set; }

		[Required]
		[Phone]
		[RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Phone number must start with 0 and be 10 to 11 digits long.")]
		public string Phone { get; set; }

	}
}
