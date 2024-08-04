using Course_Overview.Areas.Admin.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Course_Overview.Controllers
{
	public class ContactController : Controller
	{
		private readonly IContactRepository _contactRepository;
		public ContactController(IContactRepository contactRepository)
		{
			_contactRepository = contactRepository;
		}

		public async Task<IActionResult> Index()
		{
			var contacts = await _contactRepository.GetAllContact();
			return View(contacts);
		}
	}
}
