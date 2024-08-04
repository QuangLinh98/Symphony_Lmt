using Course_Overview.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StudentClassController : Controller
    {
        private readonly DatabaseContext _context;

        public StudentClassController(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var classStudents = await _context.ClassStudents
                .Include(cs => cs.Class)
                .Include(cs => cs.Student)
                .ToListAsync();

            return View(classStudents);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSelected(int[] selectedStudents)
        {
            if (selectedStudents != null && selectedStudents.Length > 0)
            {
                // Lấy tất cả các ClassStudent liên quan đến các sinh viên đã chọn
                var classStudentsToRemove = await _context.ClassStudents
                    .Where(cs => selectedStudents.Contains(cs.StudentID))
                    .ToListAsync();

                // Xóa tất cả ClassStudent liên quan
                _context.ClassStudents.RemoveRange(classStudentsToRemove);

                // Xóa các sinh viên còn lại từ bảng Students
                var studentsToRemove = await _context.Students
                    .Where(s => selectedStudents.Contains(s.StudentID))
                    .ToListAsync();

                _context.Students.RemoveRange(studentsToRemove);

                // Lưu các thay đổi
                await _context.SaveChangesAsync();
            }

            // Chuyển hướng người dùng đến trang danh sách sinh viên
            return RedirectToAction("Index");
        }

    }
}