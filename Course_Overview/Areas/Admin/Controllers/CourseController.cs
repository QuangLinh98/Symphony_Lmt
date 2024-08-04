using System.Text.Json;
using System.Text.Json.Serialization;
using Course_Overview.Areas.Admin.Attributes;
using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Areas.Admin.Service;
using Course_Overview.Helper;
using LModels;
using LModels.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace Course_Overview.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : BaseController
	{
        private readonly ICourserRepository _courseRepository;
        private readonly ITopicRepository _topicRepository;
        public CourseController(ICourserRepository courseRepository,ITopicRepository topicRepository)
        {
            _courseRepository = courseRepository ;
            _topicRepository = topicRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            // Lấy thông tin khóa học từ cơ sở dữ liệu bằng ID
            var course = await _courseRepository.GetOneCourse(id);

            // Nếu khóa học không tồn tại, trả về lỗi 404
            if (course == null)
            {
                return NotFound();
            }

            // Trả về view với dữ liệu khóa học
            return View(course);
        }

        public async Task<IActionResult> GetCourses()
        {
            var courses = await _courseRepository.GetAllCourse();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return Json(new { data = courses }, options);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Course course, IFormFile imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        string subFolder = "Courseimages";
                        string saveImagePath = await UploadFile.SaveImage(subFolder, imageFile);
                        course.ImagePath = saveImagePath;
                    }

                    await _courseRepository.AddCourse(course);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(course);
        }

        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var courseExisting = await _courseRepository.GetOneCourse(id);
                if (courseExisting == null)
                {
                    return NotFound("Not found");
                }
                return View(courseExisting);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (course.ImageFile != null)
                    {
                        var imagePath = await UploadFile.SaveImage("Courseimages", course.ImageFile);
                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            if (!string.IsNullOrEmpty(course.ImagePath))
                            {
                                UploadFile.DeleteImage(course.ImagePath);
                            }

                            //Cập nhật đường dân mới vào model
                            course.ImagePath = imagePath;
                        }
                    }
                    await _courseRepository.UpdateCourse(course);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(course);
        }
    }
}