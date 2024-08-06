using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using LModels.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Course_Overview.Controllers
{
	public class CourseController : Controller
	{
		private readonly ICourserRepository _courserRepository;
		private readonly ITopicRepository _topicRepository;
		private readonly DatabaseContext _dbContext;
		public CourseController(ICourserRepository courserRepository,
								DatabaseContext dbContext,
								ITopicRepository topicRepository
		)
		{
			_courserRepository = courserRepository;
			_dbContext = dbContext;
			_topicRepository = topicRepository;
		}

		public async Task<IActionResult> Index(string searchQuery, string duration , string fee , string level)
		{
			var course = await _courserRepository.GetAllCourse();

			//Search by name
			if (!string.IsNullOrEmpty(searchQuery))
			{
				course = course.Where(c => c.CourseName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
			}

			//Search by duration
			if (!string.IsNullOrEmpty(duration) && int.TryParse(duration, out int parsedDuration))
            {
				course = course.Where(c => c.Duration == parsedDuration).ToList();
            }

            //Search by Fee
            if (!string.IsNullOrEmpty(fee) && decimal.TryParse(fee, out decimal parsedFee))
            {
				course = course.Where(c => c.Fee == parsedFee).ToList();
            }

            //Search by Level
            if (!string.IsNullOrEmpty(level))
            {
                course = course.Where(c => c.Level == level).ToList();
            }
            return View(course);
		}

        public async Task<IActionResult> CourseDetail(int id)
        {
            try
            {
                var course = await _courserRepository.GetOneCourse(id);
                if (course == null)
                {
                    return NotFound();
                }

                var courses = await _courserRepository.GetAllCourse();
                if (courses == null)
                {
                    return NotFound();
                }


				// Lấy danh sách topic liên quan đến khóa học hiện tại
				var topics = await _topicRepository.GetTopicsByCourseId(id); 
				if (topics == null)
				{
					return NotFound();
				}

				var sliders = await _dbContext.Sliders.ToListAsync();
				int SliderId = 1;  // Đặt chỉ định cho = 1
				var viewModel = new CourseDetailViewModel
				{
					Course = course,
					Courses = courses.ToList(),
					Topics = topics.ToList(),
					Sliders = sliders.ToList(),
					SpecificSliderId = 3
				};
               

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }


		//Phương thức đăng ký khóa học 
		[HttpPost]
		public async Task<IActionResult> RegisterCourse(RegistrationCourse register)
		{
			// Lấy StudentId từ session
			var studentId = HttpContext.Session.GetInt32("StudentID");
			if (!studentId.HasValue)
			{
				return BadRequest("User is not logged in.");
			}

			// Kiểm tra xem StudentId có tồn tại trong bảng Students không
			var studentExists = await _dbContext.Students.AnyAsync(s => s.StudentID == studentId.Value);
			if (!studentExists)
			{
				return BadRequest("Student does not exist.");
			}

			// Kiểm tra xem CourseId có tồn tại trong bảng Courses không
			var courseExists = await _dbContext.Courses.AnyAsync(c => c.CourseID == register.CourseID);
			if (!courseExists)
			{
				return BadRequest("Course does not exist.");
			}

			// Gán StudentID từ thông tin người dùng đã đăng nhập
			register.StudentID = studentId.Value; // Gán StudentId
			register.RegistrationDate = DateTime.Now;
			register.Status = "Pending"; // Trạng thái ban đầu là Pending

			// Lưu đăng ký vào cơ sở dữ liệu
			_dbContext.RegistrationCourses.Add(register);
			await _dbContext.SaveChangesAsync();

			TempData["SuccessMessage"] = "Registration successful. We will respond via your email, please check your email. Thank you.";
			return RedirectToAction("Index", "Course");
		}
	}
}
