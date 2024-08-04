using Course_Overview.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Controllers
{
    public class CoreController : Controller
    {
        private readonly DatabaseContext _context;

        public CoreController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin/EX_StudentExamResult
        public IActionResult Index()
        {
            // Lấy ID của sinh viên đang đăng nhập từ session hoặc từ đối tượng người dùng hiện tại
            var studentId = HttpContext.Session.GetInt32("StudentID");

            if (studentId == null)
            {
                // Nếu không có ID của sinh viên đang đăng nhập, chuyển hướng đến trang đăng nhập hoặc hiển thị thông báo lỗi
                return RedirectToAction("Login", "Account");
            }

            // Truy vấn danh sách điểm và bài thi của sinh viên đang đăng nhập
            var studentScores = _context.EX_StudentExamResults
                .Include(r => r.Student) // Bao gồm thông tin sinh viên
                .Include(r => r.Exam) // Bao gồm thông tin kỳ thi
                .Where(r => r.StudentID == studentId) // Chỉ lấy dữ liệu của sinh viên hiện tại
                .GroupBy(r => new { r.StudentID, r.ExamID, r.Student.Name, r.Student.DateOfBirth, r.Exam.ExamName })
                .Select(g => new
                {
                    StudentID = g.Key.StudentID,
                    Name = g.Key.Name,
                    DateOfBirth = g.Key.DateOfBirth,
                    ExamID = g.Key.ExamID,
                    ExamName = g.Key.ExamName,
                    Score = g.Count(r => r.IsCorrect) // Tổng số câu trả lời đúng
                }).ToList();

            return View(studentScores);
        }

    }
}
