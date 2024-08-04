using Course_Overview.Areas.Admin.Service;
using LModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Course_Overview.Data;
using Microsoft.EntityFrameworkCore;
using Course_Overview.Helper;
using LModels.ExModel;
using Microsoft.AspNetCore.Identity;
using Course_Overview.Service;
using System.Text.RegularExpressions;
using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Areas.Admin.Service;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using Course_Overview.Areas.Admin.Viewmodels;
using Course_Overview.ViewModel;

namespace Course_Overview.Controllers
{
    public class AccountController : Controller
    {
		private readonly IUserRepository _userRepository;
		private readonly DatabaseContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
		private readonly LoginService _loginService;
		private readonly EmailService _emailService;
        public AccountController(DatabaseContext context,
                                  IPasswordHasher<User> passwordHasher,
                                  EmailService emailService,
                                  LoginService loginService,
                                  IUserRepository userRepository
                                  )
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _loginService = loginService;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userJson = SessionHelper.GetSession(HttpContext, "userSession");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login");
            }

            var user = JsonConvert.DeserializeObject<User>(userJson);
            var currentUser = await _userRepository.GetUserByIdAsync(user.ID);

            if (currentUser == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Login");
            }

            return View(new EditUserViewModel
            {
                Email = currentUser.Email, 
                Name = currentUser.Name,
                Address = currentUser.Address,
                Phone = currentUser.Phone
            });
        }

        // POST: Account/Index
        [HttpPost]
        public async Task<IActionResult> Index(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userJson = SessionHelper.GetSession(HttpContext, "userSession");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login");
            }

            var user = JsonConvert.DeserializeObject<User>(userJson);
            var currentUser = await _userRepository.GetUserByIdAsync(user.ID);
            if (currentUser == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Login");
            }

            // Cập nhật thông tin người dùng
            currentUser.Name = model.Name;
            currentUser.Address = model.Address;
            currentUser.Phone = model.Phone;

            // Kiểm tra mật khẩu mới
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword != model.ConfirmPassword)
                {
                    TempData["ErrorMessage"] = "Passwords do not match.";
                    return View(model);
                }

                if (!ValidatePassword(model.NewPassword, out string errorMessage))
                {
                    TempData["ErrorMessage"] = errorMessage;
                    return View(model);
                }

                // Cập nhật mật khẩu
                currentUser.Password = _passwordHasher.HashPassword(currentUser, model.NewPassword);
            }

            await _userRepository.UpdateUser(currentUser);
            userJson = JsonConvert.SerializeObject(currentUser);
            SessionHelper.SetSession(HttpContext, "userSession", userJson);

            TempData["SuccessMessage"] = "User information updated successfully.";
            return RedirectToAction("Index");
        }


        public IActionResult Login()
		{
            //Kiểm tra người dùng đã đăng nhập chưa , nếu đăng nhập rồi thì không thể đăng nhập tiếp 
			var userJson = SessionHelper.GetSession(HttpContext, "userSession");
			if (!string.IsNullOrEmpty(userJson))
			{
				return RedirectToAction("Index", "Course");
			}

			var message = TempData["ErrorMessage"] as string;
			ViewBag.ErrorMessage = message;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(string Email, string Password)
		{
			if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
			{
				TempData["ErrorMessage"] = "Email and Password are required.";
				return RedirectToAction("Login");
			}

			// Lấy người dùng từ repository
			var user = await _userRepository.GetUserByEmailAsync(Email);

			// Kiểm tra và reset tài khoản bị khóa tạm thời nếu thời gian khóa đã hết
			if (await _loginService.CheckAndResetLockout(user))
			{
				TempData["InfoMessage"] = "Your account lockout period has ended. Please try logging in again.";
			}

			// Kiểm tra xem người dùng có bị khóa tạm thời không
			if (user == null || !await _loginService.CanAtTemptLogin(user))
			{
				TempData["ErrorMessage"] = "Too many failed login attempts. Please try again later in 15 minutes!";
				return RedirectToAction("Login");
			}

			//kiểm tra email đã đươcj xác thực hay chưa 
			if (user.IsNewUser && !user.EmailConfirmed)
			{
				TempData["ErrorMessage"] = "Please confirm your email before logging in.";
				return RedirectToAction("Login");
			}

			// Xác thực mật khẩu
			var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, Password);
			if (passwordVerificationResult != PasswordVerificationResult.Success)
			{
				await _loginService.RecordfailedAtTempt(user); // Ghi lại số lần đăng nhập thất bại
				TempData["ErrorMessage"] = "Email or password invalid.";
				return RedirectToAction("Login");
			}

			// Đăng nhập thành công
			await _loginService.ResetAtTempts(user);   // Đăng nhập thành công thì reset lại số lần đăng nhập về 0

			// Lấy danh sách quyền của người dùng
			var userRoles = await _context.UserRoles
				.Where(ur => ur.UserId == user.ID)
				.Select(ur => ur.RoleId)
				.ToListAsync();

			var userPermissions = await _context.RolePermissions
				.Where(rp => userRoles.Contains(rp.RoleId))
				.Select(rp => rp.Permission.PermissionCode)
				.ToListAsync();


			// Khi đăng nhâp cần lưu trư thông tin user vào Session lên cần chuyển đổi thành JSON để làm việc vì Session chỉ hỗ trợ lưu trữ JSON  mà không lưu trữ đối tuọng
			var userJson = JsonConvert.SerializeObject(user);

			// Lưu thông tin user vào session
			SessionHelper.SetSession(HttpContext, "userSession", userJson);

			return RedirectToAction("Index", "Home", new { area = "" }); 

		}

		//Phương thức đăng ký tài khoản
		public async Task<IActionResult> Register(User user, string password)
		{
			if (string.IsNullOrEmpty(password))
			{
				ModelState.AddModelError("Password", "Password is required.");
				return View(user);
			}

			if (!ValidatePassword(password, out string errorMessage))
			{
				TempData["ErrorMessage"] = "Password invalid format. Please try again";
				return View(user);
			}

			user.Password = _passwordHasher.HashPassword(user, password);
			user.FailedAttempts = 0; // Khởi tạo số lần đăng nhập thất bại

			//Tạo ra 1 mã thông báo xác nhận email
			var token = Guid.NewGuid().ToString();
			user.EmailConfirmationToken = token;
			user.EmailConfirmed = false;
			user.IsNewUser = true;    // điều kiện này để user cần xác thực email trươc khi login

			await _userRepository.AddUser(user);

			//Gui thong bao ma xac nhan toi email
			var callbackUrl = Url.Action("ConfirmEmail", "Account", new { token }, protocol: HttpContext.Request.Scheme);
			await _emailService.SendMail(user.Email, "Confirm your email", $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
			TempData["SuccessMessage"] = "Registration successful. Please check your email to confirm your account.";

			return RedirectToAction("Login");
		}


        private bool ValidatePassword(string password, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Password cannot be empty.";
                return false;
            }
                
            // Kiểm tra độ dài tối thiểu
            if (password.Length < 8)
            {
                errorMessage = "Password must be at least 8 characters long.";
                return false;
            }             

            // Kiểm tra có ít nhất một chữ cái
            var hasLetter = new Regex(@"[a-zA-Z]+");
            if (!hasLetter.IsMatch(password))
            {
                errorMessage = "Password must contain at least one letter.";
                return false;
            }
                

            // Kiểm tra có ít nhất một số
            var hasDigit = new Regex(@"[0-9]+");
            if (!hasDigit.IsMatch(password))
            {
                errorMessage = "Password must contain at least one digit.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }


		//Phương thưc xác nhận Email
		public async Task<IActionResult> ConfirmEmail(string token)
		{
			if (string.IsNullOrEmpty(token))
			{
				TempData["ErrorMessage"] = "Invalid token.";
				return RedirectToAction("Index");
			}

			try
			{
				var user = await _userRepository.GetUserByEmailConfirmationTokenAsync(token);
				if (user != null)
				{
					user.EmailConfirmed = true;
					user.EmailConfirmationToken = null;
					await _userRepository.UpdateUser(user);
					TempData["SuccessMessage"] = "Email confirmed successfully. You can now log in.";
					return RedirectToAction("Login");
				}

				TempData["ErrorMessage"] = "Invalid token.";
			}
			catch (Exception ex)
			{

				TempData["ErrorMessage"] = "An error occurred while confirming your email. Please try again later.";
			}

			return RedirectToAction("Index");
		}


		//Phương thức Logout
		public IActionResult Logout()
		{
			SessionHelper.ClearSession(HttpContext);
			return RedirectToAction("Login", "Account");
		}

		
		//Phương thức View ForgotPassword
		public IActionResult ForgotPassword()
		{
			return View();
		}


		//Phương thức xử lý ForgotPassword
		[HttpPost]
		public async Task<IActionResult> ForgotPassword(string email)
		{
			try
			{
				if (string.IsNullOrEmpty(email))
				{
					ModelState.AddModelError("Email", "Email is required.");
					return View();
				}

				var user = await _userRepository.GetUserByEmailAsync(email);
				if (user != null)
				{
					var token = Guid.NewGuid().ToString();  //Tạo mã Token duy nhất
					user.ResetPasswordToken = token;
					user.ResetPasswordTokenExpiration = DateTime.UtcNow.AddHours(1);   // Mã token hơpj lệ trong 1 giờ
					await _userRepository.UpdateUser(user);

					var callbackUrl = Url.Action("ResetPassword", "Account", new { token }, protocol: Request.Scheme);
					await _emailService.SendMail(email, "Reset Password", $"Please reset your password by clicking <a href='{callbackUrl}'>here</a>.");
				}

				TempData["SuccessMessage"] = "If an account with that email address exists, a reset password link has been sent.";
				return RedirectToAction("Login");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View();
		}

		//Phương thức ResetPassword
		public IActionResult ResetPassword(string token)
		{
			if (string.IsNullOrEmpty(token))
			{
				TempData["ErrorMessage"] = "Invalid password reset token.";
				return RedirectToAction("Login");
			}

			var model = new ResetPasswordViewModel { Token = token };
			return View(model);
		}

		//Phương thức xử lý ResetPassword
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userRepository.GetUserByResetPasswordTokenAsync(model.Token);
				if (user == null || user.ResetPasswordTokenExpiration < DateTime.UtcNow)
				{
					TempData["ErrorMessage"] = "Invalid or expired password reset token.";
					return RedirectToAction("Login");
				}

				if (!ValidatePassword(model.Password, out string errorMessage))
				{
					TempData["ErrorMessage"] = "Password invalid format. Please try again";
					return View();
				}

				if (model.Password != model.ComfirmPasswod)
				{
					TempData["ErrorMessage"] = "Passwords do not match.";
					return View(model);
				}

				user.Password = _passwordHasher.HashPassword(user, model.Password);
				user.ResetPasswordToken = null;
				user.ResetPasswordTokenExpiration = null;
				user.IsNewUser = false;    //Điều kiện này cho phép user khi reset password không cần xác thực email
				await _userRepository.UpdateUser(user);

				TempData["SuccessMessage"] = "Password reset successfully. You can now log in.";
				return RedirectToAction("Login");
			}

			return View(model);
		}

       


    }
}