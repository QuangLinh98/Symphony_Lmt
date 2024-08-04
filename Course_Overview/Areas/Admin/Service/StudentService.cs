using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using Microsoft.EntityFrameworkCore;

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

        public async Task DeleteStudent(int id)
        {
            var student = await GetOneStudent(id);
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

        public async Task<Student> GetOneStudent(int id)
        {
            var student = await _dbContext.Students.FindAsync(id);
            return student;
        }

        public async Task UpdateStudent(Student student)
        {
            _dbContext.Students.Update(student);
            await _dbContext.SaveChangesAsync();
        }
    }
}
