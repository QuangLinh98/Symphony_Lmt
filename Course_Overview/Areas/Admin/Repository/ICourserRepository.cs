using LModels;

namespace Course_Overview.Areas.Admin.Repository
{
	public interface ICourserRepository
	{
		Task<IEnumerable<Course>> GetAllCourse();
		Task<Course> GetOneCourse(int id);
		Task AddCourse(Course course);
		Task UpdateCourse(Course course);
		Task DeleteCourse(int id);
	}
}
