using System.Text.Json.Serialization;
using System.Text.Json;
using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using Course_Overview.Helper;
using LModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class TeacherController : BaseController
	{
		private readonly ITeacherRepository _teacherRepository;
		private readonly DatabaseContext _dbContext;

		public TeacherController(ITeacherRepository teacherRepository, DatabaseContext dbContext)
		{
			_teacherRepository = teacherRepository;
			_dbContext = dbContext;
		}
		public IActionResult Index()
		{
			return View();
		}

        public async Task<IActionResult> GetTeacher()
        {
            var teachers = await _teacherRepository.GetAllTeacher();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return Json(new { data = teachers }, options);
        }

        public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Teacher teacher, IFormFile imageFile)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var emailExisting = await _dbContext.Teachers.AnyAsync(t => t.Email == teacher.Email);
					if (emailExisting)
					{
						ModelState.AddModelError("Email", "Email already exists.");
						return View(teacher);
					}

					var phoneExists = await _dbContext.Teachers.AnyAsync(t => t.Phone == teacher.Phone);
					if (phoneExists)
					{
						ModelState.AddModelError("Phone", "Phone number already exists.");
						return View(teacher);
					}

					if (imageFile != null && imageFile.Length > 0)
					{
						string subFolder = "TeacherImages";
						string imagePath = await UploadFile.SaveImage(subFolder, imageFile);
						teacher.ImagePath = imagePath;
					}
					await _teacherRepository.AddTeacher(teacher);
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View(teacher);
		}

		public async Task<IActionResult> Update(int id)
		{
			var teacher = await _teacherRepository.GetOneTeacher(id);
			return View(teacher);
		}

		[HttpPost]
		public async Task<IActionResult> Update(Teacher teacher)
		{
			try
			{
				if (ModelState.IsValid)
				{
					if (teacher.ImageFile != null)
					{
						//Xử lý hình ảnh mới vào thư mục 
						var imagePath = await UploadFile.SaveImage("TeacherImages", teacher.ImageFile);

						if (!string.IsNullOrEmpty(imagePath))
						{
							//Xóa hình cũ trong thư mục
							if (!string.IsNullOrEmpty(teacher.ImagePath))
							{
								UploadFile.DeleteImage(teacher.ImagePath);
							}

							//Xử lý cập nhật hình ảnh mới 
							teacher.ImagePath = imagePath;
						}
					}
					await _teacherRepository.UpdateTeacher(teacher);
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View(teacher);
		}

		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var tearcherExisting = await _teacherRepository.GetOneTeacher(id);
				if (tearcherExisting == null) 
				{
					return NotFound();
				}
				else
				{
                    if (!string.IsNullOrEmpty(tearcherExisting.ImagePath))
                    {
						UploadFile.DeleteImage(tearcherExisting.ImagePath);
                    }
					await _teacherRepository.DeleteTeacher(id);
					return RedirectToAction("Index");
                }
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View();
		}
	}
}
