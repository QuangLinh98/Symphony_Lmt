using System.Diagnostics;
using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using Course_Overview.Models;
using LModels.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ICourserRepository _courseRepository;
		private readonly ITeacherRepository _teacherRepository;
		private readonly ITopicRepository _topicRepository;
		private readonly IFAQRepository _fAQRepository;
		private readonly IContactRepository _contactRepository;
		private readonly DatabaseContext _dbContext;

		public HomeController(ILogger<HomeController> logger,
			ICourserRepository courserRepository, 
			ITeacherRepository teacherRepository,
			ITopicRepository topicRepository,
			IFAQRepository fAQRepository,
			IContactRepository contactRepository,
			DatabaseContext dbContext
			)
		{
			_logger = logger;
			_courseRepository = courserRepository;
			_teacherRepository = teacherRepository;
			_dbContext = dbContext;
			_topicRepository = topicRepository;
			_fAQRepository = fAQRepository;
			_contactRepository = contactRepository;
		}

		public async Task<IActionResult> Index()
		{
			var  courseList = await _courseRepository.GetAllCourse();
			var  topicList = await _topicRepository.GetAllTopic();
			var  teacherList = await _teacherRepository.GetAllTeacher();
			var  sliderList = await _dbContext.Sliders.Where(s => s.Status).ToListAsync();	
			var  serviceList = await _dbContext.Services.ToListAsync();
			var fAQList = await _fAQRepository.GetAllFAQ();
			var contactList = await _contactRepository.GetAllContact();

			// Sử dụng ViewModel để gửi cả danh sách Course và Teacher đến View
			var viewModel = new HomeIndexViewModel
			{
				Courses = courseList,
				Topics = topicList,
				Teachers = teacherList,
				Sliders = sliderList,
				Services = serviceList,
				FAQs = fAQList,
				Contacts = contactList
			};
			return View(viewModel);
		}

        public async Task<IActionResult> CourseMenu()
        {
            var courses = await _courseRepository.GetAllCourse();
            return PartialView("_CourseMenu", courses);
        }

        public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

        [HttpGet]
        public IActionResult PaymentSuccess(string studentEmail)
        {
            if (string.IsNullOrEmpty(studentEmail))
            {
                return BadRequest("Invalid input.");
            }

            // Tìm sinh viên
            var student = _dbContext.Students.FirstOrDefault(s => s.Email == studentEmail);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            // Tìm thông tin lớp học của sinh viên
            var classStudent = _dbContext.ClassStudents
                                         .Include(cs => cs.Class) // Đảm bảo bao gồm thông tin lớp học
                                         .FirstOrDefault(cs => cs.StudentID == student.StudentID);

            if (classStudent == null || classStudent.Class == null)
            {
                return NotFound("Class information not found.");
            }

            // Kiểm tra xem thanh toán đã được thực hiện chưa
            if (classStudent.Status == true)
            {
                // Nếu thanh toán đã được thực hiện, chuyển hướng đến trang thông báo đã thanh toán
                return RedirectToAction("PaymentAlreadyProcessed");
            }

            // Cập nhật trạng thái thành true
            classStudent.Status = true;
            _dbContext.Update(classStudent);
            _dbContext.SaveChanges();

            // Cập nhật ViewBag
            ViewBag.ClassName = classStudent.Class.ClassName;

            // Tiếp tục xử lý và trả về view
            return View();
        }

        // Trang thông báo thanh toán đã được thực hiện
        public IActionResult PaymentAlreadyProcessed()
        {
            return View(); // Trang này sẽ thông báo cho người dùng rằng thanh toán đã được thực hiện
        }

        public IActionResult PaymentCancel()
        {
            return View();
        }
    }
}
