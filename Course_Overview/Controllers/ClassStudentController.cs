using Course_Overview.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Controllers
{
	public class ClassStudentController : Controller
	{
		private readonly DatabaseContext _context;

		public ClassStudentController(DatabaseContext context)
		{
			_context = context;
		}
        public async Task<IActionResult> Index()
        {
            // Lấy StudentID từ Session
            var studentId = HttpContext.Session.GetInt32("StudentID");

            if (studentId == null)
            {
                // Nếu không có StudentID trong session, chuyển hướng đến trang đăng nhập hoặc thông báo lỗi
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách ClassStudent chỉ cho sinh viên đã đăng nhập
            var classStudents = await _context.ClassStudents
                .Include(cs => cs.Class)
                .Include(cs => cs.Student)
                .Where(cs => cs.StudentID == studentId)
                .ToListAsync();

            return View(classStudents);
        }
    }
}
