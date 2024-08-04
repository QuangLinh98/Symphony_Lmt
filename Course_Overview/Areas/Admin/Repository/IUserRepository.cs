using LModels;

namespace Course_Overview.Areas.Admin.Repository
{
	public interface IUserRepository
	{
		Task<IEnumerable<User>> GetAllUser();
		Task<User> GetUserByEmailAsync(string email);
		Task<User> GetUserByIdAsync(int  id);
		Task AddUser(User user);
		Task UpdateUser(User user);
		Task DeleteUser(string email);
		// Lấy số lần đăng nhập thất bại cho email
		Task<int> GetFailedAttemptsAsync(string email);

		Task<User> GetUserByEmailConfirmationTokenAsync(string token);
		Task<User> GetUserByResetPasswordTokenAsync(string token);
	}
}
