using System.Text.Json;
using System.Text.Json.Serialization;
using Course_Overview.Areas.Admin.Repository;
using LModels;
using Microsoft.AspNetCore.Mvc;

namespace Course_Overview.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FAQController : BaseController
	{
        private readonly IFAQRepository _faqRepository;
        public FAQController(IFAQRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }

        public IActionResult Index()
        {       
            return View();
        }

        public async Task<IActionResult> GetFAQ()
        {
            var faqs = await _faqRepository.GetAllFAQ();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return Json(new { data = faqs }, options);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FAQ fAQ)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _faqRepository.AddFAQ(fAQ);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(fAQ);
        }

        public async Task<IActionResult> Update(int id)
        {
            var faqExisting = await _faqRepository.GetOneFAQ(id);
            if (faqExisting == null)
            {
                return NotFound();
            }
            return View(faqExisting);
        }

        [HttpPost]
        public async Task<IActionResult> Update(FAQ fAQ)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _faqRepository.UpdateFAQ(fAQ);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(fAQ);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _faqRepository.DeleteFAQ(id);
            return RedirectToAction("Index");
        }
    }
}
