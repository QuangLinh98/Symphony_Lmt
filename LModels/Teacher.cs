
using Microsoft.AspNetCore.Http;

namespace LModels
{
	public class Teacher
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeacherID { get; set; }

		[Required(ErrorMessage = "Full name is required")]
		public string FullName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[Phone]
		[RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Phone number must start with 0 and be 10 to 11 digits long.")]
		public string Phone { get; set; }

		[Required]
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }
		public string? ImagePath { get; set; }

		[NotMapped]
		public IFormFile? ImageFile { get; set; }
		public string TeacherCode { get; set; }

		//Tạo Contructor để thưc hiện Generate Teacher_Code 
		public Teacher()
		{
			TeacherCode = GenerateTeacherCode();
		}

		// Sử dụng biểu thức điều kiện đê thực hiện Generate
		private string GenerateTeacherCode()
		{
			DateTime now = DateTime.Now;
			return $"TC{now:yyyyMMddHHmmss}";
		}
		public ICollection<Class>? Classes { get; set; }
		public ICollection<LabSession>? LabSessions { get; set; }
	}
}
