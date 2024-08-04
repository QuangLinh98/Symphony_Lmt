using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Service
{
	public class TeacherService : ITeacherRepository
	{
		private readonly DatabaseContext _dbContext;
		public TeacherService(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task AddTeacher(Teacher teacher)
		{
			await _dbContext.Teachers.AddAsync(teacher);
			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteTeacher(int id)
		{
			var teacher = await GetOneTeacher(id);
			if (teacher != null)
			{
				_dbContext.Teachers.Remove(teacher);
				await _dbContext.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Teacher>> GetAllTeacher()
		{
			var teachers = await _dbContext.Teachers.ToListAsync();
			return teachers;
		}

		public async Task<Teacher> GetOneTeacher(int id)
		{
			var teacherExisting = await _dbContext.Teachers.FindAsync(id);
			return teacherExisting;
		}

		public async Task UpdateTeacher(Teacher teacher)
		{
			_dbContext.Teachers.Update(teacher);
			await _dbContext.SaveChangesAsync();
		}
	}
}
