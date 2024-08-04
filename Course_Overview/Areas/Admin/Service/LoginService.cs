using Course_Overview.Areas.Admin.Repository;
using LModels;

namespace Course_Overview.Areas.Admin.Service
{
	public class LoginService
	{
		private readonly IUserRepository _userRepository; // Thêm repository để cập nhật dữ liệu


		//Biến này lưu trữ số lần đăng nhập thất bại cho mỗi email. Key là email của người dùng và value là số lần đăng nhập thất bại.
		private readonly Dictionary<string, int> _logintempts = new Dictionary<string, int>();

		private readonly int _maxTempts = 3;    // Giới hạn số lần đăng nhâpj thất bại trước khi tài khoản bị khoá tạm thời 

		private readonly TimeSpan _lockoutDuration = TimeSpan.FromMinutes(1);      //SetTimeOut khóa tài khoản tạm thời 

		public LoginService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		// 1. Phương thức này kiểm tra xem người dùng có được phép thử đăng nhập nữa hay không. 
		public async Task<bool> CanAtTemptLogin(User user)
		{
            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow)
            {
                return false;
            }

			var failedAttempts = await _userRepository.GetFailedAttemptsAsync(user.Email);
			return failedAttempts < _maxTempts;
		}


		//2. Phương thức này ghi nhận một lần đăng nhập thất bại cho email của người dùng.
		public async Task RecordfailedAtTempt(User user)
        {
			// Tăng số lần đăng nhập thất bại từ cơ sở dữ liệu
			var failedAttempts = await _userRepository.GetFailedAttemptsAsync(user.Email) + 1;

			// Cập nhật số lần đăng nhập thất bại vào đối tượng user
			user.FailedAttempts = failedAttempts;

			if (failedAttempts >= _maxTempts)
			{
				// Nếu số lần đăng nhập thất bại vượt quá ngưỡng, khóa tài khoản
				user.LockoutEnd = DateTime.UtcNow.Add(_lockoutDuration);
			}

			// Cập nhật thông tin người dùng vào cơ sở dữ liệu
			await _userRepository.UpdateUser(user);
		}
		
        //3.  Phương thức này đặt lại số lần đăng nhập thất bại của người dùng về 0.
        public async Task ResetAtTempts(User user)
        {
			// Reset số lần đăng nhập thất bại
			user.FailedAttempts = 0;

			// Mở khóa tài khoản
			user.LockoutEnd = null;

			// Cập nhật thông tin người dùng vào cơ sở dữ liệu
			await _userRepository.UpdateUser(user);
		}

        //4. Phương thức kiểm tra và reset tài khoản sau khi hết thời gian khóa tạm thời 
        public async Task<bool> CheckAndResetLockout(User user)
        {
            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value <= DateTime.UtcNow)
            {
                await ResetAtTempts(user);
                return true;
            }
            return false;
        }


        //5.lấy số lần đăng nhập thất bại của người dùng dựa trên địa chỉ email của họ
        public int GetFailedAttempts(string email)
		{
			// Kiểm tra xem email có tồn tại trong dictionary _logintempts không
			if (_logintempts.TryGetValue(email, out int attempts))
			{
				// Nếu email tồn tại, trả về số lần đăng nhập thất bại
				return attempts;
			}
			// Nếu email không tồn tại trong dictionary, trả về 0
			return 0;
		}

		public int GetMaxAttempts()
		{
			return _maxTempts;
		}
		
		//6.Chức năng của nó là trả về khoảng thời gian khóa tài khoản người dùng sau khi họ vượt quá số lần đăng nhập thất bại tối đa.
		public TimeSpan GetLockoutDuration()
		{
			return _lockoutDuration;
		}
	}
}
