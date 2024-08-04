using System.ComponentModel.DataAnnotations;

namespace Course_Overview.Areas.Admin.Viewmodels
{
	public class ResetPasswordViewModel
	{
		[Required]
		public string Token { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ComfirmPasswod { get; set; }
	}
}
