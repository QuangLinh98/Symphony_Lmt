using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LModels.Client;

namespace LModels.ViewModels
{
	public class CourseDetailViewModel
	{
		public Course Course { get; set; }    // Mục đích hiển thị  chi tiết khóa học 
		public ICollection<Slider> Sliders { get; set; }
		public ICollection<Topic> Topics { get; set; }
		public ICollection<Course> Courses { get; set; }           // Mục đích hiển thị danh sách khóa hoc 
		public int SpecificSliderId { get; set; }   // Mục đích chỉ định cho Slider nào được hiển thị 
	}
}
