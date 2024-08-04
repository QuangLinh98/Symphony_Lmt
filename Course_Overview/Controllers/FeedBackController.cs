using Microsoft.AspNetCore.Mvc;

namespace Course_Overview.Controllers
{
	public class FeedBackController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
