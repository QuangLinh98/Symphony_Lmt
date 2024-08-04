using Course_Overview.Data;
using LModels.ExModel;
using Microsoft.AspNetCore.Mvc;

namespace Course_Overview.Controllers
{
    public class PaymentController : Controller
    {
        private readonly DatabaseContext _context;

        public PaymentController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult ProcessPayment(int classId, int studentId, decimal fee)
        {
            if (studentId == 0 || classId == 0)
            {
                return BadRequest("Invalid input.");
            }

            var classStudent = _context.ClassStudents
                .FirstOrDefault(x => x.StudentID == studentId);

            if (classStudent != null)
            {
                // Update existing entry
                classStudent.ClassID = classId;
                classStudent.Status = true;
            }
            else
            {
                // Add new entry
                classStudent = new ClassStudent
                {
                    StudentID = studentId,
                    ClassID = classId,
                    Status = true
                };
                _context.ClassStudents.Add(classStudent);
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "ClassStudent");
        }

    }
}