using Course_Overview.Areas.Admin.Attributes;
using Course_Overview.Data;
using Course_Overview.Helper;
using LModels.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : BaseController
	{
        private readonly DatabaseContext _dbContext;
        public SliderController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {           
            var sliders = await _dbContext.Sliders.ToListAsync();
            return View(sliders);
        }


        public IActionResult Create()
        {
            /*var userPermissions = HttpContext.Items["UserPermissions"] as List<string>;
            if (userPermissions == null || !userPermissions.Contains("SLIDER_CREATE"))
            {
                return Forbid();
            }*/
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(Slider slider, IFormFile imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   /* if (imageFile != null && imageFile.Length > 0)
                    {
                        var subFolder = "SliderImages";
                        var saveImagePath = await UploadFile.SaveImage(subFolder, imageFile);
                        slider.ImagePath = saveImagePath;
                    }*/
                    await _dbContext.AddAsync(slider);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(slider);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string actionType)
        {
            var sliderExisting = await _dbContext.Sliders.FindAsync(id);
            if (sliderExisting == null)
            {
                return NotFound();
            }
            if (actionType == "Disactivate")
            {
                sliderExisting.Status = false;
            }
            else if (actionType == "Activate")
            {
                sliderExisting.Status = true;
            }
            else
            {
                return BadRequest("Invalid action type");
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }


        public async Task<IActionResult> Update(int id)
        {
            var sliderExisting = await _dbContext.Sliders.FindAsync(id);
            if (sliderExisting == null)
            {
                return NotFound();
            }

            return View(sliderExisting);
        }

        [HttpPost]

        public async Task<IActionResult> Update(Slider slider)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (slider.ImageFile != null)
                    {
                        var imagePath = await UploadFile.SaveImage("SliderImages", slider.ImageFile);
                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            if (!string.IsNullOrEmpty(slider.ImagePath))
                            {
                                UploadFile.DeleteImage(slider.ImagePath);
                            }

                            //Cập nhật ảnh mới 
                            slider.ImagePath = imagePath;
                        }
                    }
                    _dbContext.Sliders.Update(slider);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(slider);
        }


        public async Task<IActionResult>Delete(int id)
        {
            try
            {
                var slider = await _dbContext.Sliders.FindAsync(id);
                if (slider == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(slider.ImagePath))
                {
                    UploadFile.DeleteImage(slider.ImagePath);
                }

                _dbContext.Sliders.Remove(slider);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
           
        }
    }
}
