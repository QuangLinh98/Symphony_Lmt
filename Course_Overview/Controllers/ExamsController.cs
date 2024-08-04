using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels.ExModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Controllers
{
	public class ExamsController : Controller
	{
	
		private readonly DatabaseContext _dbContext;

		public ExamsController(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}

        public ActionResult Index()
        {
            var studentId = HttpContext.Session.GetInt32("StudentID");

            // Lấy tất cả các lớp học mà học sinh đang theo học
            var classIds = _dbContext.ClassStudents
                .Where(cs => cs.StudentID == studentId)
                .Select(cs => cs.ClassID)
                .ToList();

            // Lấy tất cả các exam liên quan đến các lớp học này và đảm bảo không có exam trùng lặp
            var listExam = _dbContext.ClassExams
                .Where(e => classIds.Contains(e.ClassID))
                .Distinct() // Đảm bảo các bài kiểm tra không bị trùng lặp
                .ToList();

            // Lấy tất cả các bài kiểm tra từ bảng EX_Exams
            var distinctExamIds = listExam.Select(e => e.ExamID).Distinct().ToList();
            var exams = _dbContext.EX_Exams
                .Where(x => distinctExamIds.Contains(x.ExamID))
                .ToList();

            // Lấy các lớp học duy nhất từ các exam duy nhất
            var distinctClassIds = listExam.Select(e => e.ClassID).Distinct().ToList();
            var distinctClasses = _dbContext.Classes
                .Where(c => distinctClassIds.Contains(c.ClassID))
                .ToList();
            /*var OK = new List<ClassExam>();
            foreach (var item in distinctClasses)
            {
                var a = _dbContext.ClassExams.Where(x => x.ClassID == item.ClassID).ToList();
                foreach (var c in a)
                {
                    OK.Add(c);
                }
            }
            var distinctExamIDs = OK.Select(x => x.ExamID).Distinct().ToList();*/

            // Gửi dữ liệu đến View
            ViewBag.Exams = exams;
            ViewBag.Classes = distinctClasses;

            return View();
        }
        public ActionResult RenderExams(int ExamID)
        {
            var studentId = HttpContext.Session.GetInt32("StudentID");
            var _Exams = _dbContext.EX_Exams.Where(x => x.ExamID == ExamID).FirstOrDefault();
            var _ExamQuestions = _dbContext.EX_ExamQuestions
                                .Where(x => x.ExamID == ExamID)
                                .Include(x => x.Question) // Assuming the navigation property is named 'Question'
                                    .ThenInclude(q => q.Answers) // Assuming the navigation property in Question is named 'Answers'
                                .ToList();


            var _Subject = _dbContext.EX_Subjects.Where(x => x.SubjectID == _Exams.SubjectID).FirstOrDefault();
            var _Lession = _dbContext.EX_Lessons.Where(x => x.SubjectID == _Subject.SubjectID).ToList();
            ViewBag._Exams = _Exams;
            ViewBag._ExamQuestions = _ExamQuestions;
            ViewBag._Subject = _Subject;
            ViewBag._Lession = _Lession;
            return View();
          
        }
 

    }
}
