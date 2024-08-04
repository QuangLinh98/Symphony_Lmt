
using LModels.Client;

namespace LModels.ViewModels
{
	public class HomeIndexViewModel
	{
		public IEnumerable<Course> Courses { get; set; }
		public IEnumerable<Teacher> Teachers { get; set; }
		public IEnumerable<Slider> Sliders { get; set; }
		public IEnumerable<Services> Services { get; set; }
		public IEnumerable<Topic> Topics { get; set; }
		public IEnumerable<FAQ> FAQs { get; set; }
		public IEnumerable<Contact> Contacts { get; set; }
		public IEnumerable<AboutUs> AboutUs { get; set; }
	}
}
