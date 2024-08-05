using LModels;

namespace Course_Overview.Areas.Admin.Repository
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllStudent();
        Task<Student> GetStudentByEmail(string email);
        Task<Student> GetStudentById(int id);
        Task AddStudent(Student student);
        Task UpdateStudent(Student student);
        Task DeleteStudent(string email);

		// Lấy số lần đăng nhập thất bại cho email
		Task<int> GetFailedAttemptsAsync(string email);

		Task<Student> GetStudentByEmailConfirmationTokenAsync(string token);
		Task<Student> GetStudentByResetPasswordTokenAsync(string token);
	}
}
