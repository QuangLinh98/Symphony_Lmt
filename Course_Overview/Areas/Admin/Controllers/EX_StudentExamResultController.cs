using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Course_Overview.Data;
using LModels.ExModel;
using Course_Overview.Mail;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

namespace Course_Overview.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class EX_StudentExamResultController : Controller
	{
		private readonly DatabaseContext _context;
		private readonly EmailSetting _emailSettings;
		public EX_StudentExamResultController(IOptions<EmailSetting> emailSettings, DatabaseContext context)
		{
			_emailSettings = emailSettings.Value;
			_context = context;
		}

		// GET: Admin/EX_StudentExamResult
		public IActionResult Index()
		{
			// Truy vấn danh sách học sinh và điểm của họ theo từng kỳ thi, bao gồm thông tin lớp học
			var studentScores = _context.EX_StudentExamResults
				.Include(r => r.Student) // Bao gồm thông tin sinh viên
				.ThenInclude(s => s.ClassStudents) // Bao gồm thông tin lớp học qua ClassStudents
				.Include(r => r.Exam) // Bao gồm thông tin kỳ thi
				.Where(r => r.Student.ClassStudents.Any(cs => cs.ClassID == 1)) // Lọc sinh viên thuộc lớp với ClassID = 1
				.GroupBy(r => new
				{
					r.StudentID,
					r.ExamID,
					r.Student.Name,
					r.Student.DateOfBirth,
					r.Student.IdentityCard,
					r.Exam.ExamName,
					ClassName = r.Student.ClassStudents != null && r.Student.ClassStudents.Any()
						? r.Student.ClassStudents.Select(cs => cs.Class != null ? cs.Class.ClassName : null).FirstOrDefault()
						: null // Xử lý trường hợp ClassStudents là null hoặc không có lớp
				})
				.Select(g => new
				{
					StudentID = g.Key.StudentID,
					Name = g.Key.Name,
					DateOfBirth = g.Key.DateOfBirth,
					ExamID = g.Key.ExamID,
					ExamName = g.Key.ExamName,
					CodeStudent = g.Key.IdentityCard,
					ClassName = g.Key.ClassName, // Thêm thông tin lớp học
					Score = g.Count(r => r.IsCorrect)
				}).ToList();

			return View(studentScores);
		}

		[HttpPost]
		public async Task<IActionResult> AddStudentsToClass(int classId, int[] selectedItems)
		{
			if (selectedItems == null || selectedItems.Length == 0 || classId == 0)
			{
				return BadRequest("Invalid input.");
			}

			// Kiểm tra số lượng học sinh hiện tại trong lớp học
			var currentCount = _context.ClassStudents.Count(x => x.ClassID == classId);
			var selectedCount = selectedItems.Length;

			if (currentCount + selectedCount > 40)
			{
				return BadRequest("The class cannot have more than 40 students.");
			}

			// Xóa sinh viên khỏi lớp học hiện tại (nếu có)
			var existingEntries = _context.ClassStudents.Where(x => selectedItems.Contains(x.StudentID)).ToList();
			_context.ClassStudents.RemoveRange(existingEntries);
			await _context.SaveChangesAsync(); // Lưu thay đổi để xóa các mục hiện tại

			// Lấy thông tin lớp học và học phí
			var classInfo = await _context.Classes.Where(c => c.ClassID == classId).FirstOrDefaultAsync();

			if (classInfo == null)
			{
				return BadRequest("Class not found.");
			}

			// Thêm sinh viên vào lớp học mới
			foreach (var studentId in selectedItems)
			{
				var classStudent = new ClassStudent
				{
					ClassID = classId,
					StudentID = studentId,
					Status = false // Hoặc giá trị mặc định khác
				};
				_context.ClassStudents.Add(classStudent);
			}

			await _context.SaveChangesAsync(); // Lưu thay đổi để thêm các mục mới

			// Gửi email thông báo
			var students = await _context.Students.Where(s => selectedItems.Contains(s.StudentID)).ToListAsync();

			var fromAddress = new MailAddress(_emailSettings.FromEmail, "Symphony");
			string fromPassword = _emailSettings.FromPassword;
			var smtp = new SmtpClient
			{
				Host = _emailSettings.Host,
				Port = _emailSettings.Port,
				EnableSsl = _emailSettings.EnableSsl,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
			};

			try
			{
				foreach (var student in students)
				{
					var toAddress = new MailAddress(student.Email);
					const string subject = "Class Enrollment Notification";

					// Tạo liên kết thanh toán PayPal với số tiền là học phí của lớp và classId
					var paymentLink = GeneratePayPalPaymentLink(student.Email, classInfo.Fee);

					string body = $"Dear {student.Name},\n\n" +
						   $"You have been enrolled in the class '{classInfo.ClassName}'.\n" +
						   $"Start learn is {classInfo.StartDate}.\n" +
						   $"End learn is {classInfo.EndDate}.\n" +
						   $"The tuition fee for this class is {classInfo.Fee:C}.\n" +
						   $"Please complete the payment using the following link: {paymentLink}\n\n" +
						   "Best regards,\nSymphony";

					using (var message = new MailMessage(fromAddress, toAddress)
					{
						Subject = subject,
						Body = body
					})
					{
						await smtp.SendMailAsync(message);
					}
				}
			}
			catch (Exception ex)
			{
				// Log lỗi hoặc xử lý lỗi
				return BadRequest($"Error sending email: {ex.Message}");
			}

			return Json(new { success = true });
		}

		// Tạo liên kết thanh toán PayPal
		private string GeneratePayPalPaymentLink(string email, decimal amount)
		{
			var businessEmail = "n.luu.gia.bao@gmail.com";
			var sandbox = true; // Sử dụng môi trường Sandbox
			var clientId = "ARU1w7yizVfWsJ-YmMsAfG2GPcVdcBCQBWYUGXAryBBWdJAJuruFlwE01m2ee_V6lhQvSfBclhI1dO-l";
			var secret = "EAbYv6sCINxIijeLaZ25XF21mOmQ8hskT1KoDU3siA0T3F9a_oqXr98AO-8N_uUTOc6eMrwqBn531TSd";
			var returnUrl = $"https://localhost:7210/Home/PaymentSuccess?studentEmail={email}"; // URL để người dùng quay lại sau khi thanh toán
			var cancelUrl = "https://localhost:7210/Home/PaymentCancel"; // URL để người dùng quay lại nếu hủy thanh toán

			// URL cơ sở cho PayPal Sandbox
			var baseUrl = sandbox ? "https://www.sandbox.paypal.com" : "https://www.paypal.com";

			var paymentUrl = $"{baseUrl}/cgi-bin/webscr?cmd=_xclick&business={businessEmail}&amount={amount}&currency_code=USD&return={returnUrl}&cancel_return={cancelUrl}";

			return paymentUrl;
		}

		public IActionResult GetClasses()
		{
			var classes = _context.Classes.ToList();

			return Json(classes);
		}

	}

}
