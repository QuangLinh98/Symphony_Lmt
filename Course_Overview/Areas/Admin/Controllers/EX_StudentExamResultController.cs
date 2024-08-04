using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Course_Overview.Data;
using LModels.ExModel;

namespace Course_Overview.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EX_StudentExamResultController : Controller
    {
        private readonly DatabaseContext _context;

        public EX_StudentExamResultController(DatabaseContext context)
        {
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
        public IActionResult AddStudentsToClass(int classId, int[] selectedItems)
        {
            if (selectedItems == null || selectedItems.Length == 0 || classId == 0)
            {
                return BadRequest("Invalid input.");
            }

            // Kiểm tra số lượng học sinh hiện tại trong lớp học
            var currentCount = _context.ClassStudents
                .Count(x => x.ClassID == classId);

            var selectedCount = selectedItems.Length;

            if (currentCount + selectedCount > 40)
            {
                return BadRequest("The class cannot have more than 40 students.");
            }

            // Xóa sinh viên khỏi lớp học hiện tại (nếu có)
            var existingEntries = _context.ClassStudents.Where(x => selectedItems.Contains(x.StudentID))
                .ToList();

            _context.ClassStudents.RemoveRange(existingEntries);
            _context.SaveChanges(); // Lưu thay đổi để xóa các mục hiện tại

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

            _context.SaveChanges(); // Lưu thay đổi để thêm các mục mới

            return Json(new { success = true });
        }




        public IActionResult GetClasses()
        {
            var classes = _context.Classes.ToList();

            return Json(classes);
        }

    }

}
