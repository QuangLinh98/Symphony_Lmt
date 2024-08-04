using Course_Overview.Areas.Admin.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Course_Overview.Controllers
{
	public class FAQController : Controller
	{
		private readonly IFAQRepository _fAQRepository;
		public FAQController(IFAQRepository fAQRepository)
		{
			_fAQRepository = fAQRepository;
		}

		public async Task<IActionResult> Index()
		{
			var fAQs = await _fAQRepository.GetAllFAQ();
			return View(fAQs);
		}
	}
}
