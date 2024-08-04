using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LModels
{
	public class RegistrationCourse
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int RegistrationCourseID { get; set; }
		public string Status { get; set; }

		public int CourseID { get; set;}
		public Course? Course { get; set;}

		[ForeignKey("User")]
		public int ID { get; set;}
		public User? User { get; set;}

		public DateTime RegistrationDate { get; set; } // Thời gian đăng ký
	}
}
