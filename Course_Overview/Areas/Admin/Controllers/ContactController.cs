using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Helper;
using LModels;
using Microsoft.AspNetCore.Mvc;

namespace Course_Overview.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ContactController : BaseController
	{
		private readonly IContactRepository _contactRepository;
		public ContactController(IContactRepository contactRepository)
		{
			_contactRepository = contactRepository;
		}

		public async Task<IActionResult> Index()
		{
			var contact = await _contactRepository.GetAllContact();
			return View(contact);
		}

		public IActionResult Create()
		{
			return View();	
		}

		[HttpPost]
		public async Task<IActionResult> Create(Contact contact, IFormFile imageFile)
		{
			try
			{
                if (ModelState.IsValid)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
						string subFolder = "ContactImages";
						string saveImage = await UploadFile.SaveImage(subFolder, imageFile);
						contact.ImagePath = saveImage;
                    }

					await _contactRepository.AddContact(contact);
					return RedirectToAction("Index");
                }
            }
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View(contact);
		}

		public async Task<IActionResult>Update(int id)
		{
			var contactExisting = await _contactRepository.GetOneContact(id);
			if (contactExisting == null) 
			{
				return NotFound();
			}
			return View(contactExisting);
		}

		[HttpPost]
		public async Task<IActionResult>Update(Contact contact)
		{
			try
			{
                if (ModelState.IsValid)
                {
                    if (contact.ImageFile != null)
                    {
						var imagePath = await UploadFile.SaveImage("ContactImages", contact.ImageFile);
                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            if (!string.IsNullOrEmpty(contact.ImagePath))
                            {
								UploadFile.DeleteImage(contact.ImagePath);
                            }

							contact.ImagePath = imagePath;
                        }
                    }

					await _contactRepository.UpdateContact(contact);
					return RedirectToAction("Index");
                }
            }
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View(contact);
		}

		public async Task<IActionResult>Delete(int id)
		{
			var contactExisting = await _contactRepository.GetOneContact(id);
            if (contactExisting == null)
            {
				return NotFound();
            }
            else
            {
                if (!string.IsNullOrEmpty(contactExisting.ImagePath))
                {
					UploadFile.DeleteImage(contactExisting.ImagePath);
                }
				await _contactRepository.DeleteContact(id);
				return RedirectToAction("Index");
            }
        }
	}
}
