using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Course_Overview.Areas.Admin.Service
{
    public class StudentService : IStudentRepository
    {
        private readonly DatabaseContext _dbContext;

        public StudentService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddStudent(Student student)
        {
            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();
		}

        public async Task DeleteStudent(string email)
        {
			var student = await GetStudentByEmail(email);
			if (student != null)
			{
				_dbContext.Students.Remove(student);
				await _dbContext.SaveChangesAsync();
			}
		}

        public async Task<IEnumerable<Student>> GetAllStudent()
        {
            var students = await _dbContext.Students.ToListAsync();
            return students;
        }

		public async Task<int> GetFailedAttemptsAsync(string email)
		{
			var student = await _dbContext.Students.FirstOrDefaultAsync(u => u.Email == email);
			return student != null ? student.FailedAttempts : 0;
		}


		public async Task<Student> GetStudentByEmail(string email)
		{
			return await _dbContext.Students.FirstOrDefaultAsync(u => u.Email == email);
		}

		public async Task<Student> GetStudentByEmailConfirmationTokenAsync(string token)
		{
			return await _dbContext.Students.SingleOrDefaultAsync(u => u.EmailConfirmationToken == token);
		}

		public async Task<Student> GetStudentById(int id)
		{
			return await _dbContext.Students.FindAsync(id);
		}

		public async Task<Student> GetStudentByResetPasswordTokenAsync(string token)
		{
			return await _dbContext.Students.FirstOrDefaultAsync(u => u.ResetPasswordToken == token);
		}

		public async Task UpdateStudent(Student student)
        {
            _dbContext.Students.Update(student);
            await _dbContext.SaveChangesAsync();
        }
    }
}
