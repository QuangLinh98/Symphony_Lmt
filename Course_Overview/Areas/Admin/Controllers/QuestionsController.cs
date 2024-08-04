using System.Text.Json.Serialization;
using System.Text.Json;
using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Course_Overview.ViewModel;
using LModels.ExModel;

using static System.Runtime.InteropServices.JavaScript.JSType;
using X.PagedList.Extensions;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Course_Overview.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuestionsController : Controller
    {
		private readonly DatabaseContext _dbContext;
		public QuestionsController(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}
		public ActionResult Index()
		{
			var subject = _dbContext.EX_Subjects.ToList();
			return View(subject);
		}
        private IQueryable<EX_Question> ConditionWhere(EX_QuestionVM model)
        {
            var query = _dbContext.EX_Questions.AsQueryable();
            //  query = query.Where(x => x.UserRequest == _user.UserName);
            if (!string.IsNullOrEmpty(model.QuestionText))
            {
                query = query.Where(x => x.QuestionText.Contains(model.QuestionText));
            }
            
            return query;
        }
        public ActionResult PartialList(EX_QuestionVM model)
        {
            try
            {
                var questions = _dbContext.EX_Subjects
                        .Where(s => s.SubjectID == model.SubjectID)
                        .Include(s => s.Lessons)
                            .ThenInclude(l => l.Questions)
                                .ThenInclude(q => q.Answers)
                        .SelectMany(s => s.Lessons)
                        .SelectMany(l => l.Questions)
                        .ToList();
                if (!string.IsNullOrEmpty(model.QuestionText))
                {
                    questions = questions.Where(x => x.QuestionText.Contains(model.QuestionText)).ToList();
                }
                var listLession = _dbContext.EX_Lessons.Where(x => x.SubjectID == model.SubjectID).ToList();
                if(model.LessionID >= 0)
                {
                    ViewBag.listLession = listLession;
                

                    if(model.LessionID >= 1)
                    {
                        ViewBag.listLessionSelect = listLession.Where(x => x.LessonID == model.LessionID).ToList();
                        questions = questions.Where(x => x.LessonID == model.LessionID).ToList();

                    }

                    ViewBag.questions = questions;
                    var pagedData = questions.ToPagedList(model.Page, 10);
                    return PartialView(pagedData);
                }
                else
                {
                    if (model.LessionID != -2 && model.LessionID != -1) // khác Lần đầu tiên và all
                    {
                        questions = questions.Where(x => x.LessonID == model.LessionID).ToList();
                    }
                    ViewBag.listLession = listLession;

                    ViewBag.listLessionSelect = null;
                    ViewBag.questions = questions;
                    var pagedData = questions.ToPagedList(model.Page, 10);
                    return PartialView(pagedData);
                }
              
    
                
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi)
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        //ChangeIsCorrect

        public ActionResult ChangeIsCorrect(int Id)
        {
            try
            {
                var answer = _dbContext.EX_Answers.SingleOrDefault(a => a.AnswerID == Id);
                if (answer == null)
                {
                    return Json(new { success = false, message = "Answer not found" });
                }
                answer.IsCorrect = !answer.IsCorrect;
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Succes" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "fail" });
            }
        }

        //ChangeAnswerText
        public ActionResult ChangeAnswerText(int Id, string AnswerText)
        {
            try
            {
                var answer = _dbContext.EX_Answers.Where(a => a.AnswerID == Id).FirstOrDefault();
                answer.AnswerText = AnswerText;
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Succes" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "fail" });
            }
        }
        //ChangeQuestionText

        public ActionResult ChangeQuestionText(int Id, string QuestionText)
        {
            try
            {
                var answer = _dbContext.EX_Questions.Where(a => a.QuestionID == Id).FirstOrDefault();
                answer.QuestionText = QuestionText;
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Succes" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "fail" });
            }
        }
        //AddAnswer

        public ActionResult AddAnswer(int questionID, string inputValue)
        {
            try
            {
                var Answer = new EX_Answer();
                Answer.QuestionID = questionID;
                Answer.AnswerText = inputValue;
                Answer.IsCorrect = false;
                _dbContext.EX_Answers.Add(Answer);
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Succes" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "fail" });
            }
        }
        //DeleteAnswer
        public ActionResult DeleteAnswer(int AnswerId)
        {
            try
            {
                var answer = _dbContext.EX_Answers.Where(a => a.AnswerID == AnswerId).FirstOrDefault();
                _dbContext.EX_Answers.Remove(answer);
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Succes" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "fail" });
            }
        }
        //DeleteQuestion 
        public ActionResult DeleteQuestion(int QuestionId)
        {
            try
            {
                var listAnswer = _dbContext.EX_Answers.Where(x => x.QuestionID == QuestionId).ToList();
                if (listAnswer.Count > 0)
                {
                    foreach (var answer in listAnswer)
                    {
                        _dbContext.EX_Answers.Remove(answer);
                    }
                }
                _dbContext.SaveChanges();

                var Question = _dbContext.EX_Questions.Where(a => a.QuestionID == QuestionId).FirstOrDefault();
                _dbContext.EX_Questions.Remove(Question);
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Succes" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "fail" });
            }
        }
        //{ SubjectId: SubjectId, LessionID: LessionID }
        public ActionResult RenderModalAdd(int SubjectId, int LessionID)
        {
           var subject = _dbContext.EX_Subjects.Where(x=>x.SubjectID == SubjectId).FirstOrDefault();
            ViewBag.Subject = subject;
            var Lession = _dbContext.EX_Lessons.Where(x => x.LessonID == LessionID).FirstOrDefault();
            ViewBag.Lession = Lession;
            return View();
        }
        public ActionResult SubmitRequest(ModelQuestionSave ModelQuestionSave)
        {
            try
            {
                var _Question = new EX_Question();
                _Question.QuestionText = ModelQuestionSave.QuestionText;
                _Question.LessonID = ModelQuestionSave.LessionIdAdd;
                _dbContext.EX_Questions.Add(_Question);
                _dbContext.SaveChanges();
                foreach (var item in ModelQuestionSave.AnswerSave)
                {
                    var _Answer = new EX_Answer();
                    _Answer.AnswerText = item.Answer;
                    _Answer.QuestionID = _Question.QuestionID;
                    _dbContext.EX_Answers.Add(_Answer);
                }
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
