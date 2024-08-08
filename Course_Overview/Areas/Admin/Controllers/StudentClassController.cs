using Course_Overview.Data;
using LModels.ExModel;
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



        [HttpPost]
        public async Task<IActionResult> Edit(int studentID, int classID, bool TuitionFeeStatus)
        {
            if (ModelState.IsValid)
            {
                // Tìm ClassStudent hiện tại
                var classStudent = await _context.ClassStudents
                    .Include(cs => cs.Class)
                    .FirstOrDefaultAsync(cs => cs.StudentID == studentID);

                if (classStudent == null)
                {
                    return NotFound();
                }

                // Xóa sinh viên khỏi lớp học hiện tại (nếu có)
                if (classStudent.Class != null)
                {
                    // Tìm lớp học hiện tại
                    var oldClass = classStudent.Class;

                    // Xóa sinh viên khỏi lớp học hiện tại
                    _context.ClassStudents.Remove(classStudent);

                    // Cập nhật trạng thái lớp học cũ nếu cần thiết
                    // (Ví dụ: giảm số lượng sinh viên trong lớp học cũ nếu bạn đang theo dõi số lượng)
                    // oldClass.StudentCount--;
                    // _context.Classes.Update(oldClass);
                }

                // Tạo một đối tượng ClassStudent mới với ClassID mới
                var newClassStudent = new ClassStudent
                {
                    StudentID = studentID,
                    ClassID = classID,
                    Status = TuitionFeeStatus
                };

                // Thêm sinh viên vào lớp học mới
                _context.ClassStudents.Add(newClassStudent);

                // Cập nhật lớp học mới
                var newClass = await _context.Classes.FindAsync(classID);
                if (newClass == null)
                {
                    return NotFound();
                }

                // Cập nhật liên kết với lớp học mới
                // newClass.StudentCount++;
                // _context.Classes.Update(newClass);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }


        public IActionResult GetClasses()
        {
            var classes = _context.Classes.ToList();

            return Json(classes);
        }

    }
}