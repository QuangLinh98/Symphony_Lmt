using Course_Overview.Areas.Admin.Repository;
using LModels.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Course_Overview.Controllers
{
	public class OurTeamController : Controller
	{
		private readonly ITeacherRepository _teacherRepository;

		public OurTeamController(ITeacherRepository teacherRepository)
		{
			_teacherRepository = teacherRepository;
		}

		public async Task<IActionResult> Index()
		{
			var teacherList = await _teacherRepository.GetAllTeacher();
			var viewModel = new HomeIndexViewModel
			{
				Teachers = teacherList,
			};
			return View(viewModel);
		}
	}
}
