

namespace LModels
{
	public class LabSession
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int SessionID { get; set; }
		public int StudentID { get; set; }
		public int TeacherID { get; set; }
		public int CourseID { get; set; }
		public bool OptedAtRegistration { get; set; }   //Đăng ký lúc đăng ký khóa học

		public DateTime StartTime { get; set; }     //Thời gian bắt đầu học 
		public DateTime EndTime { get; set; }     //Thời gian kết thúc học 

		//public Student? Students { get; set; }
		public Teacher? Teacher { get; set; }
		public Course? Course { get; set; }
	}
}
