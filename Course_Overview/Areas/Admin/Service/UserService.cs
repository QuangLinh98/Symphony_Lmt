using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Service
{
	public class UserService : IUserRepository
	{
		private readonly DatabaseContext _dbContext;

		public UserService(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task AddUser(User user)
		{
			await _dbContext.Users.AddAsync(user);
			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteUser(string email)
		{
			var user = await GetUserByEmailAsync(email);
			if (user != null)
			{
				_dbContext.Users.Remove(user);
				await _dbContext.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<User>> GetAllUser()
		{
			var users = await _dbContext.Users.Where(u => u.Email != "Admin").ToListAsync();
			return users;
		}

		public async Task<int> GetFailedAttemptsAsync(string email)
		{
			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
			return user != null ? user.FailedAttempts : 0;
		}

		public async Task<User> GetUserByEmailAsync(string email)
		{
			return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
		}

		public async Task<User> GetUserByEmailConfirmationTokenAsync(string token)
		{
			return await _dbContext.Users.SingleOrDefaultAsync(u => u.EmailConfirmationToken == token);
		}

        public async Task<User> GetUserByIdAsync(int id)
        {
			return await _dbContext.Users.FindAsync(id);
        }

        public async Task<User> GetUserByResetPasswordTokenAsync(string token)
		{
			return await _dbContext.Users.FirstOrDefaultAsync(u => u.ResetPasswordToken == token);
		}

		public async Task UpdateUser(User user)
		{
			var existingUser = await _dbContext.Users.FindAsync(user.ID);
			if (existingUser != null)
			{
				_dbContext.Entry(existingUser).CurrentValues.SetValues(user);
				await _dbContext.SaveChangesAsync();
			}
		}

	}
}
