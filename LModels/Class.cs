

using LModels.ExModel;

namespace LModels
{
	public class Class
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ClassID { get; set; }

		[Required(ErrorMessage = "Class name is required")]
		public string ClassName { get; set; }
		public int TeacherID { get; set; }

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal Fee { get; set; }

		[Required]
		[RegularExpression("^(Basic|Advanced)$", ErrorMessage = "Invalid Segregated Class")]
		public string ClassType { get; set; }  //Phân loai lớp

		[Required]
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }

		public Teacher? Teacher { get; set; }
		public ICollection<Schedule>? Schedules { get; set; }
		public ICollection<ExamResult>? ExamResults { get; set; }
		public ICollection<ClassStudent>? ClassStudents { get; set; }

	}
}