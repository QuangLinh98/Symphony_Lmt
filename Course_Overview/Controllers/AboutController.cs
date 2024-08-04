using Course_Overview.Areas.Admin.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Course_Overview.Controllers
{
	public class AboutController : Controller
	{
		private readonly IAboutRepository _aboutRepository;
		public AboutController (IAboutRepository aboutRepository)
		{
			_aboutRepository = aboutRepository;
		}

		public async Task<IActionResult> Index()
		{
			var abouts = await _aboutRepository.GetAllAbout();
			return View(abouts);
		}
	}
}
