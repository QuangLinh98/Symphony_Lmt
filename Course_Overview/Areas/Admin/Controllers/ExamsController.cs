using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Course_Overview.Data;
using Course_Overview.ViewModel;
using LModels;
using X.PagedList.Extensions;
using Microsoft.EntityFrameworkCore;
using LModels.ExModel;

namespace Course_Overview.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExamsController : Controller
    {
        private readonly DatabaseContext _dbContext;

        public ExamsController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index()
        {
            var subjects = _dbContext.EX_Subjects.ToList();
            return View(subjects);
        }
        public ActionResult PartialList(EX_ExamsVM model)
        {
            try
            {
                var _Exam = _dbContext.EX_Exams.Where(x => x.SubjectID == model.SubjectID).ToList();
                if (!string.IsNullOrEmpty(model.ExamName))
                {
                    _Exam = _Exam.Where(x => x.ExamName.Contains(model.ExamName)).ToList();
                }

                var pagedData = _Exam.ToPagedList(model.Page, 10);
                return PartialView(pagedData);

            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi)
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult ViewAndEdit(int ExamId)
        {
            var _Exams = _dbContext.EX_Exams.Where(x => x.ExamID == ExamId).FirstOrDefault();
            var _ExamQuestions = _dbContext.EX_ExamQuestions
                                .Where(x => x.ExamID == ExamId)
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
        public ActionResult RenderExams(int ExamId)
        {
            var _Exams = _dbContext.EX_Exams.Where(x => x.ExamID == ExamId).FirstOrDefault();
            var _ExamQuestions = _dbContext.EX_ExamQuestions
                                .Where(x => x.ExamID == ExamId)
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
        public ActionResult RenderQuestion(int SubjectId, int LessionId, int Page, string QuestionText)
        {
            // Lấy danh sách các câu hỏi từ EX_Subjects bao gồm các thông tin liên quan
            var questions = _dbContext.EX_Subjects
                .Where(s => s.SubjectID == SubjectId)
                .Include(s => s.Lessons)
                    .ThenInclude(l => l.Questions)
                        .ThenInclude(q => q.Answers)
                .SelectMany(s => s.Lessons)
                .SelectMany(l => l.Questions)
                .ToList();

            // Lọc theo LessionId nếu cần thiết
            if (LessionId != -1)
            {
                questions = questions.Where(x => x.LessonID == LessionId).ToList();
            }

            // Lấy danh sách các QuestionID đã có trong EX_ExamQuestions
            var examQuestionIds = _dbContext.EX_ExamQuestions
                .Select(eq => eq.QuestionID)
                .ToList();

            // Loại bỏ các câu hỏi đã có trong EX_ExamQuestions
            questions = questions
                .Where(q => !examQuestionIds.Contains(q.QuestionID))
                .ToList();

            //where QuestionText

            if (!string.IsNullOrEmpty(QuestionText))
            {
                questions = questions.Where(x => x.QuestionText.Contains(QuestionText)).ToList();
            }
            var pagedData = questions.ToPagedList(Page, 1);
            ViewBag.Page = Page;
            return PartialView(pagedData);

        }
        //{ QuestionId: QuestionId, ExamID: ExamID }
        public ActionResult SelectToExam(int QuestionId, int ExamID)
        {
            try
            {
                var Ex = _dbContext.EX_Exams.SingleOrDefault(a => a.ExamID == ExamID);
                var Exquestion = new EX_ExamQuestion();
                Exquestion.QuestionID = QuestionId;
                Exquestion.ExamID = ExamID;
                _dbContext.EX_ExamQuestions.Add(Exquestion);
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Succes" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "fail" });
            }
        }
        public ActionResult DeleteExamsQustion(int QuestionID, int ExamID)
        {
            try
            {
                var Ex = _dbContext.EX_ExamQuestions.SingleOrDefault(a => a.ExamID == ExamID && a.QuestionID == QuestionID);
                _dbContext.EX_ExamQuestions.Remove(Ex);
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Succes" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "fail" });
            }
        }

        public ActionResult RenderModalAdd(int SubjectId)
        {
            ViewBag.SubjectId = SubjectId;
            return View();
        }
        //SubjectIdAdd: SubjectIdAdd, ExamNameAdd: ExamNameAdd, ExamDateAdd: ExamDateAdd, TotalMinsAdd: TotalMinsAdd },
        public ActionResult AddExams(int SubjectIdAdd, string ExamNameAdd, DateTime ExamDateAdd, int TotalMinsAdd)
        {
            try
            {
                var exam = new EX_Exam();
                exam.SubjectID = SubjectIdAdd;
                exam.ExamName = ExamNameAdd;
                exam.ExamDate = ExamDateAdd;
                exam.TotalMins = TotalMinsAdd;
                exam.Status = 0;
                _dbContext.EX_Exams.Add(exam);
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Succes" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "fail" });
            }
        }
        public ActionResult Cancel(int ExamId)
        {
            try
            {
                var Ex = _dbContext.EX_Exams.Where(x=>x.ExamID == ExamId).FirstOrDefault();
                Ex.Status = (Ex.Status == -1) ? 0 : -1;
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Succes" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "fail" });
            }
        }
        public ActionResult AssignExamForClass(int ExamId)
        {
            var _ClassExams = _dbContext.ClassExams.Where(x=>x.ExamID == ExamId).ToList();
            var _Classes = _dbContext.Classes.ToList();
            var EX_Exams = _dbContext.EX_Exams.Where(_x => _x.ExamID == ExamId).FirstOrDefault();
            ViewBag._ClassExams = _ClassExams;
            ViewBag._Classes = _Classes;
            ViewBag.EX_Exams = EX_Exams;
            return View();
        }
        public ActionResult ListClass(int ExamId, string ClassName)
        {
            var _ClassExams = _dbContext.ClassExams.Where(x => x.ExamID == ExamId).ToList();
            var _Classes = _dbContext.Classes.ToList();
            var EX_Exams = _dbContext.EX_Exams.Where(_x => _x.ExamID == ExamId).FirstOrDefault();
            if (!string.IsNullOrEmpty(ClassName))
            {
                _Classes = _Classes.Where(x => x.ClassName.Contains(ClassName)).ToList();
            }
            ViewBag._ClassExams = _ClassExams;
            ViewBag._Classes = _Classes;
            ViewBag.EX_Exams = EX_Exams;
            return View();
        }
        public ActionResult changeStatus(int ExamID, int ClassID)
        {
            try
            {
                var ExamClass = _dbContext.ClassExams.Where(x => x.ExamID == ExamID && x.ClassID == ClassID).FirstOrDefault();
                if(ExamClass != null)
                {
                    _dbContext.ClassExams.Remove(ExamClass);
                }
                else
                {
                    var newClassExam = new ClassExam();
                    newClassExam.ExamID = ExamID;
                    newClassExam.ClassID = ClassID;
                    _dbContext.ClassExams.Add(newClassExam);
                }
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Succes" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "fail" });
            }
        }
    
        public ActionResult SaveEdit(string ExamsNameEdit, DateTime ExamDateEdit, int TotalMinsEdit, int ExamID)
        {
            try
            {
               var ex = _dbContext.EX_Exams.Where(x => x.ExamID == ExamID).FirstOrDefault();
                ex.ExamName = ExamsNameEdit;
                ex.ExamDate = ExamDateEdit;
                ex.TotalMins = TotalMinsEdit;
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Succes" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "fail" });
            }
        }
        public ActionResult Run( int ExamID)
        {
            try
            {
                var ex = _dbContext.EX_Exams.Where(x => x.ExamID == ExamID).FirstOrDefault();
                if(ex.ExamDate < DateTime.Now)
                {
                    return Json(new { success = false, message = "Vui Long Edit Lai Gio" });
                }
                ex.TimeEnd = ex.ExamDate.AddMinutes(ex.TotalMins);
                ex.Status = 1;
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Succes" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "fail" });
            }
        }
    } 
}
