using Microsoft.AspNetCore.Mvc;

namespace Course_Overview.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class HomeController : BaseController
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
