using Course_Overview.Areas.Admin.Repository;
using LModels;
using Microsoft.AspNetCore.Mvc;

namespace Course_Overview.Controllers
{
	public class ScheduleController : Controller
	{
		private readonly IScheduleRepository _scheduleRepository;

		public ScheduleController(IScheduleRepository scheduleRepository)
		{
			_scheduleRepository = scheduleRepository;
		}

		public async Task<IActionResult> MySchedule()
		{
			var studentID = HttpContext.Session.GetInt32("StudentID");
			var schedules = await _scheduleRepository.GetAllScheduleAsync();

			// Lọc lịch học của sinh viên đang đăng nhập
			var mySchedules = schedules.Where(s => s.StudentID == studentID);

			return View(mySchedules);
		}
	}
}
