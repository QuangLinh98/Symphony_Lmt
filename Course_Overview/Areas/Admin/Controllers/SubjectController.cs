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

namespace Course_Overview.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubjectController : Controller
    {
		private readonly DatabaseContext _dbContext;
		public SubjectController(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}
		public ActionResult Index()
		{
			return View();
		}
        private IQueryable<EX_Subject> ConditionWhere(EX_SubjectVM model)
        {
            var query = _dbContext.EX_Subjects.AsQueryable();
            //  query = query.Where(x => x.UserRequest == _user.UserName);
            if (!string.IsNullOrEmpty(model.SubjectName))
            {
                query = query.Where(x => x.SubjectName.Contains(model.SubjectName));
            }
            return query;
        }
        public ActionResult PartialList(EX_SubjectVM model)
        {
            try
            {

                var query = ConditionWhere(model);
                var result = query.ToList();
                var pagedData = result.ToPagedList(model.Page, 10);
                return PartialView(pagedData);

            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: log lỗi)
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult RenderModalAdd()
        {
            return View();
        }
        public ActionResult SubmitRequest(ModelSaveRequestSubmit ModelSaveRequestSubmit) 
        {
            try
            {
                var subject = new EX_Subject();
                subject.SubjectName = ModelSaveRequestSubmit.SubjectName;

                _dbContext.EX_Subjects.Add(subject);
                _dbContext.SaveChanges();
                foreach (var item in ModelSaveRequestSubmit.Lession)
                {
                    var lession = new EX_Lesson();
                    lession.LessonNumber = item.No;
                    lession.LessonName = item.LessonName;
                    lession.SubjectID = subject.SubjectID;
                    _dbContext.EX_Lessons.Add(lession);
                }
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Succes" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Fail" });
            }
      
        }
        public ActionResult Delete(int id)
        {
            try
            {
                // Tìm subject theo ID
                var subject = _dbContext.EX_Subjects.Find(id);

                // Kiểm tra nếu subject tồn tại
                if (subject == null)
                {
                    return Json(new { success = false, message = "Subject not found" });
                }

                // Tìm tất cả các lesson liên quan đến subject
                var lessons = _dbContext.EX_Lessons.Where(l => l.SubjectID == id).ToList();

                // Xóa tất cả các lesson
                _dbContext.EX_Lessons.RemoveRange(lessons);

                // Xóa subject
                _dbContext.EX_Subjects.Remove(subject);

                // Lưu thay đổi vào cơ sở dữ liệu
                _dbContext.SaveChanges();

                return Json(new { success = true, message = "Delete successful" });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi
                return Json(new { success = false, message = "Delete failed", error = ex.Message });
            }
        }
        public ActionResult RenderModalEdit(int id)
        {
            var subject = _dbContext.EX_Subjects.Where(x=>x.SubjectID == id).FirstOrDefault();
            var lession = _dbContext.EX_Lessons.Where(x => x.SubjectID == subject.SubjectID).ToList();
            foreach (var l in lession)
            {
                if (l.LessonName == null)
                {
                    l.LessonName = "";
                }
                if (l.LessonNumber == null)
                {
                    l.LessonNumber = "";
                }
            }
            subject.Lessons = lession;
            return View(subject);
        }
        public ActionResult Edit(ModelSaveRequestEdit modelSaveRequestEdit)
        {
            try
            {
                // Tìm subject theo SubjectID
                var subject = _dbContext.EX_Subjects
                    .FirstOrDefault(s => s.SubjectID == modelSaveRequestEdit.SubjectID);

                if (subject == null)
                {
                    return Json(new { success = false, message = "Subject not found" });
                }

                // Cập nhật hoặc thêm các lesson
                var currentLessons = _dbContext.EX_Lessons
                    .Where(l => l.SubjectID == modelSaveRequestEdit.SubjectID)
                    .ToList();

                var newLessonIds = modelSaveRequestEdit.Lession.Select(l => l.Id).ToList();

                // Cập nhật hoặc thêm các lesson từ modelSaveRequestEdit
                foreach (var lessonEdit in modelSaveRequestEdit.Lession)
                {
                    if (lessonEdit.Id > 0) // Nếu có ID thì cập nhật
                    {
                        var existingLesson = currentLessons.FirstOrDefault(l => l.LessonID == lessonEdit.Id);
                        if (existingLesson != null)
                        {
                            existingLesson.LessonNumber = lessonEdit.LessonNumber;
                            existingLesson.LessonName = lessonEdit.LessonName;
                        }
                    }
                    else // Nếu không có ID thì thêm mới
                    {
                        var newLesson = new EX_Lesson
                        {
                            LessonNumber = lessonEdit.LessonNumber,
                            LessonName = lessonEdit.LessonName,
                            SubjectID = modelSaveRequestEdit.SubjectID
                        };
                        _dbContext.EX_Lessons.Add(newLesson);
                    }
                }

                // Xóa các lesson không có trong danh sách
                var lessonsToDelete = currentLessons
                    .Where(l => !newLessonIds.Contains(l.LessonID))
                    .ToList();

                _dbContext.EX_Lessons.RemoveRange(lessonsToDelete);

                // Lưu các thay đổi vào cơ sở dữ liệu
                _dbContext.SaveChanges();

                return Json(new { success = true, message = "Edit successful" });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi
                return Json(new { success = false, message = "Edit failed", error = ex.Message });
            }
        }





    }
}
