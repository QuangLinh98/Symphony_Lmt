
using LModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Course_Overview.Data;
using Microsoft.EntityFrameworkCore;
using Course_Overview.Helper;
using LModels.ExModel;
using System.Net.Mail;
using System.Net;
using Course_Overview.Mail;
using Microsoft.Extensions.Options;
using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Areas.Admin.Viewmodels;
using Course_Overview.Service;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Course_Overview.Areas.Admin.Service;
using Course_Overview.ViewModel;
using Newtonsoft.Json;

namespace Course_Overview.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IStudentRepository _studentRepository;
        private readonly IPasswordHasher<Student> _passwordHasher;      //Sử dụng Identity để Hash Password
        private readonly StudentLoginService _studentLoginService;
        private readonly EmailService _emailService;
        private readonly EmailSetting _emailSettings;

        public AccountController(IOptions<EmailSetting> emailSettings,
                                  DatabaseContext context,
                                  IStudentRepository studentRepository,
                                  StudentLoginService studentLoginService,
                                  EmailService emailService,
                                  IPasswordHasher<Student> passwordHasher
                                  )
        {
            _emailSettings = emailSettings.Value;
            _studentRepository = studentRepository;
            _studentLoginService = studentLoginService;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy dữ liệu từ Session
            var studentID = HttpContext.Session.GetInt32("StudentID");
            var studentName = HttpContext.Session.GetString("StudentName");
            var studentEmail = HttpContext.Session.GetString("StudentEmail");
            var studentPhone = HttpContext.Session.GetString("StudentPhone");
            var studentAddress = HttpContext.Session.GetString("StudentAddress");

            // Kiểm tra nếu không có dữ liệu trong Session
            if (studentID == null || string.IsNullOrEmpty(studentName) || string.IsNullOrEmpty(studentEmail) || string.IsNullOrEmpty(studentPhone) || string.IsNullOrEmpty(studentAddress))
            {
                return RedirectToAction("Login", "Account");
            }

            // Tạo một ViewModel để truyền dữ liệu sang View
            var viewModel = new EditStudentViewModel
            {
                StudentID = studentID.Value,
                Name = studentName,
                Email = studentEmail,
                Phone = studentPhone,
                Address = studentAddress

            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(EditStudentViewModel model)
        {
            // Kiểm tra tính hợp lệ của model
            if (!ModelState.IsValid)
            {
                // Lấy dữ liệu từ Session
                var studentId = HttpContext.Session.GetInt32("StudentID");
                if (studentId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Lấy thông tin sinh viên từ cơ sở dữ liệu
                var student = await _studentRepository.GetStudentById(studentId.Value);
                if (student == null)
                {
                    return NotFound();
                }

                // Tạo lại ViewModel từ thông tin sinh viên
                var viewModel = new EditStudentViewModel
                {
                    StudentID = studentId.Value,
                    Name = student.Name,
                    Email = student.Email,
                    Phone = student.Phone,
                    Address = student.Address,
  
                };

                // Trả về view với dữ liệu đã lấy từ cơ sở dữ liệu
                return View(viewModel);
            }

            // Lấy dữ liệu từ Session
            var studentID = HttpContext.Session.GetInt32("StudentID");
            if (studentID == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy thông tin sinh viên từ cơ sở dữ liệu
            var studentToUpdate = await _studentRepository.GetStudentById(studentID.Value);
            if (studentToUpdate == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin sinh viên
            studentToUpdate.Name = model.Name;
            studentToUpdate.Email = model.Email;
            studentToUpdate.Phone = model.Phone;
            studentToUpdate.Address = model.Address;

			// Kiểm tra xem trường ImagePath có null hay không
			/*if (!string.IsNullOrEmpty(studentToUpdate.ImagePath))
			{
				HttpContext.Session.SetString("StudentImage", studentToUpdate.ImagePath);
			}*/
			//studentToUpdate.ImagePath = model.ProfileImageUrl;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.Update(studentToUpdate);
            await _context.SaveChangesAsync();

            // Cập nhật lại dữ liệu trong Session
            HttpContext.Session.SetString("StudentName", studentToUpdate.Name);
            HttpContext.Session.SetString("StudentEmail", studentToUpdate.Email);
            HttpContext.Session.SetString("StudentPhone", studentToUpdate.Phone);
            HttpContext.Session.SetString("StudentAddress", studentToUpdate.Address);


            // Thông báo cập nhật thành công
            TempData["SuccessMessage"] = "Your information has been updated successfully.";

            return RedirectToAction("Index"); // Chuyển hướng về trang index
        }


        // Đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Student student, IFormFile imageFile, string otp)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra OTP
                var sessionOtp = HttpContext.Session.GetString("GeneratedOtp");
                if (otp != sessionOtp)
                {
                    ViewBag.OtpError = "Invalid OTP.";
                    return View(student);
                }

                // Kiểm tra xem email đã tồn tại chưa
                var emailExisting = await _context.Students.AnyAsync(s => s.Email == student.Email);
                if (emailExisting)
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View(student);
                }

                // Kiểm tra xem số điện thoại đã tồn tại chưa
                var phoneExisting = await _context.Students.AnyAsync(s => s.Phone == student.Phone);
                if (phoneExisting)
                {
                    ModelState.AddModelError("Phone", "Phone already exists.");
                    return View(student);
                }

                // Xử lý ảnh nếu có
                if (imageFile != null && imageFile.Length > 0)
                {
                    var subFolder = "StudentImages";
                    var saveImagePath = await UploadFile.SaveImage(subFolder, imageFile);
                    student.ImagePath = saveImagePath;
                }

                student.Password = _passwordHasher.HashPassword(student, student.Password);

                // Thêm sinh viên vào cơ sở dữ liệu
                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                // Thêm liên kết sinh viên và lớp học
                var student1 = new ClassStudent
                {
                    StudentID = student.StudentID,
                    ClassID = 1 // Đảm bảo rằng lớp học với ID = 1 tồn tại trong cơ sở dữ liệu
                };
                _context.ClassStudents.Add(student1);
                await _context.SaveChangesAsync();

                // Gửi email thông báo
                var fromAddress = new MailAddress(_emailSettings.FromEmail, "Symphony");
                var toAddress = new MailAddress(student.Email);
                string fromPassword = _emailSettings.FromPassword;
                const string subject = "Entrance Exam Notification";
                string body = @"
                    <p>Congratulations on your registration! You need to take the entrance exam.</p>
                    <p>Please visit the following link to access the exam: <a href='https://localhost:7210/Account/Login'>Take the Exam</a></p>
                    <p>Best regards,<br>Symphony</p>";

                var smtp = new SmtpClient
                {
                    Host = _emailSettings.Host,
                    Port = _emailSettings.Port,
                    EnableSsl = _emailSettings.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
                TempData["SuccessMessage"] = "Registration successful. Please check your email to confirm your account.";

                return RedirectToAction("Login");
            }

            ViewBag.OtpError = "Invalid OTP.";
            return View(student);
        }


        // Đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["ErrorMessage"] = "Email and Password are required.";
                return RedirectToAction("Login");
            }

            // Kiểm tra thông tin đăng nhập
            var student = await _studentRepository.GetStudentByEmail(email);

            //kiểm tra email đã đươcj xác thực hay chưa 
            if (student.IsNewUser && !student.EmailConfirmed)
            {
                TempData["ErrorMessage"] = "Please confirm your email before logging in.";
                return RedirectToAction("Login");
            }

            // Xác thực mật khẩu
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(student, student.Password, password);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                await _studentLoginService.RecordfailedAtTempt(student); // Ghi lại số lần đăng nhập thất bại
                TempData["ErrorMessage"] = "Email or password invalid.";
                return RedirectToAction("Login");
            }

            if (student != null)
            {
                HttpContext.Session.SetInt32("StudentID", student.StudentID);
                HttpContext.Session.SetString("StudentName", student.Name);
                HttpContext.Session.SetString("StudentEmail", student.Email);
                HttpContext.Session.SetString("StudentPhone", student.Phone);
                HttpContext.Session.SetString("StudentAddress", student.Address);		

				return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        // Đăng xuất
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("StudentID");
            return RedirectToAction("Index", "Home");
        }

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

                var student = await _studentRepository.GetStudentByEmail(email);
                if (student != null)
                {
                    var token = Guid.NewGuid().ToString();  //Tạo mã Token duy nhất
                    student.ResetPasswordToken = token;
                    student.ResetPasswordTokenExpiration = DateTime.UtcNow.AddHours(1);   // Mã token hơpj lệ trong 1 giờ
                    await _studentRepository.UpdateStudent(student);

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
                var student = await _studentRepository.GetStudentByResetPasswordTokenAsync(model.Token);
                if (student == null || student.ResetPasswordTokenExpiration < DateTime.UtcNow)
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

                student.Password = _passwordHasher.HashPassword(student, model.Password);
                student.ResetPasswordToken = null;
                student.ResetPasswordTokenExpiration = null;
                student.IsNewUser = false;    //Điều kiện này cho phép user khi reset password không cần xác thực email
                await _studentRepository.UpdateStudent(student);

                TempData["SuccessMessage"] = "Password reset successfully. You can now log in.";
                return RedirectToAction("Login");
            }

            return View(model);
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student, IFormFile imageFile)
        {
            if (id != student.StudentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingStudent = await _context.Students.FindAsync(id);
                    if (existingStudent == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật thuộc tính của thực thể đã tải từ cơ sở dữ liệu
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var subFolder = "StudentImages";
                        var saveImagePath = await UploadFile.SaveImage(subFolder, imageFile);
                        existingStudent.ImagePath = saveImagePath;
                    }

                    // Cập nhật các thuộc tính khác
                    existingStudent.Name = student.Name;
                    existingStudent.Email = student.Email;
                    existingStudent.Password = student.Password;
                    existingStudent.Phone = student.Phone;
                    existingStudent.Address = student.Address;
                    existingStudent.DateOfBirth = student.DateOfBirth;
                    existingStudent.IdentityCard = student.IdentityCard;

                    _context.Update(existingStudent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "ClassStudent");
            }
            return View(student);
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }

        [HttpPost]
        public IActionResult SendOtp(string email)
        {
            // Generate OTP
            var random = new Random();
            var generatedOtp = random.Next(100000, 999999).ToString();

            // Store OTP in session
            HttpContext.Session.SetString("GeneratedOtp", generatedOtp);

            // Send OTP via email
            var fromAddress = new MailAddress(_emailSettings.FromEmail, "Symphony");
            var toAddress = new MailAddress(email);
            string fromPassword = _emailSettings.FromPassword;
            const string subject = "Your OTP Code";
            string body = $"Your OTP code is {generatedOtp}";

            var smtp = new SmtpClient
            {
                Host = _emailSettings.Host,
                Port = _emailSettings.Port,
                EnableSsl = _emailSettings.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

            return Json(new { success = true });
        }
    }
}




