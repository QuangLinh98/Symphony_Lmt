using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Service
{
	public class CourseService : ICourserRepository
	{
		private readonly DatabaseContext _dbContext;
		public CourseService(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task AddCourse(Course course)
		{
			await _dbContext.Courses.AddAsync(course);
			await _dbContext.SaveChangesAsync();

		}

		public async Task DeleteCourse(int id)
		{
			var courseExisting = await GetOneCourse(id);
			if (courseExisting != null)
			{
				_dbContext.Courses.Remove(courseExisting);
				await _dbContext.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Course>> GetAllCourse()
		{
			var courses = await _dbContext.Courses.Include(c => c.Topics).ToListAsync();
			return courses;
		}

		public async Task<Course> GetOneCourse(int id)
		{
			var courseExisting = await _dbContext.Courses.FindAsync(id);
			return courseExisting;
		}

		public async Task UpdateCourse(Course course)
		{
			_dbContext.Courses.Update(course);
			await _dbContext.SaveChangesAsync();
		}
	}
}