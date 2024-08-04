using Course_Overview.Areas.Admin.Repository;
using LModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Course_Overview.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutController : BaseController
	{
        private readonly IAboutRepository _aboutRepository;
        public AboutController(IAboutRepository aboutRepository)
        {
            _aboutRepository = aboutRepository;
        }    

        public async Task<IActionResult> Index()
        {
            var abouts = await _aboutRepository.GetAllAbout();
            return View(abouts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>Create(AboutUs aboutUs)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _aboutRepository.AddAbout(aboutUs);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(aboutUs);
        }

        public async Task<IActionResult>Update(int id)
        {
            var aboutExisting = await _aboutRepository.GetOneAbout(id);
            if (aboutExisting == null)
            {
                return NotFound();
            }
            return View(aboutExisting);
        }

        [HttpPost]
        public async Task<IActionResult>Update(AboutUs aboutUs)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _aboutRepository.UpdateAbout(aboutUs);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(aboutUs);
        }

        public async Task<IActionResult>Delete(int id)
        {
            var about = await _aboutRepository.GetOneAbout(id);
            if (about == null)
            {
                return NotFound();
            }
            await _aboutRepository.DeleteAbout(id);
            return RedirectToAction("Index");
        }
    }
}
