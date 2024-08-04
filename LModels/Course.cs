

using Microsoft.AspNetCore.Http;

namespace LModels
{
	public class Course
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CourseID { get; set; }

		[Required(ErrorMessage = "Course Name is required")]
		public string CourseName { get; set; }

		[Required]
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }

		[Required]
		public int Duration { get; set; }                      //Thời gian đào tạo

        [Required]
        public string Level { get; set; }                      //Cấp độ khóa học 

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Fee { get; set; }
		public string EntryRequirements { get; set; }      //Mô tả yêu cầu đầu vào cho khóa học 
		public string StudyMaterials { get; set; }        //Liệt kê các tài liệu cần cho khóa hoc 
		public string LabFacilities { get; set; }         //Thông tin về cơ sở vật chất của trung tâm

		public string? ImagePath { get; set; }

		[NotMapped]
		public IFormFile? ImageFile { get; set; }

		public ICollection<Topic>? Topics { get; set; }

		public ICollection<Schedule>? Schedules { get; set; }
		public ICollection<RegistrationCourse>? RegistrationCourses { get; set; }


    }
}
