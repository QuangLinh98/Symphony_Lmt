using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using Course_Overview.Helper;
using LModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StudentController : BaseController
	{
        private readonly IStudentRepository _studentRepository;
        private readonly DatabaseContext _dbContext;

        public StudentController(IStudentRepository studentRepository, DatabaseContext dbContext)
        {
            _studentRepository = studentRepository;
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var students = await _studentRepository.GetAllStudent();
            return View(students);
        }

		public async Task<IActionResult> Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Student student, IFormFile imageFile)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var emailExisting = await _dbContext.Students.AnyAsync(s => s.Email == student.Email);
					if (emailExisting)
					{
						ModelState.AddModelError("Email", "Email already exists.");
						return View(student);
					}

					var phoneExisting = await _dbContext.Students.AnyAsync(s => s.Phone == student.Phone);
					if (phoneExisting)
					{
						ModelState.AddModelError("Phone", "Phone already exists.");
						return View(student);
					}

					if (imageFile != null && imageFile.Length > 0)
					{
						var subFolder = "StudentImages";
						var saveImagePath = await UploadFile.SaveImage(subFolder, imageFile);
						student.ImagePath = saveImagePath;
					}

					await _studentRepository.AddStudent(student);
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}

			return View(student);
		}

		public async Task<IActionResult>Update(int id)
		{
			var student = await _studentRepository.GetOneStudent(id);
            if (student == null)
            {
				return NotFound();
            }
			return View(student);
        }

		[HttpPost]
		public async Task<IActionResult> Update(Student student)
		{
			try
			{
                if (ModelState.IsValid)
                {
                    if (student.ImageFile != null)
                    {
						//Xử lưu lý hình ảnh mới vào thư mục
						var imagePath = await UploadFile.SaveImage("StudentImages", student.ImageFile);
                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            if (!string.IsNullOrEmpty(student.ImagePath))
                            {
								UploadFile.DeleteImage(student.ImagePath);
                            }

							student.ImagePath = imagePath;
                        }
                    }
					await _studentRepository.UpdateStudent(student);
					return RedirectToAction("Index");
                }
            }
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View(student);
		}

		public async Task<IActionResult>Delete(int id)
		{
			try
			{
				var studentExisting = await _studentRepository.GetOneStudent(id);
				if (studentExisting == null)
				{
					return NotFound();
				}
				else
				{
					if (!string.IsNullOrEmpty(studentExisting.ImagePath))
					{
						UploadFile.DeleteImage(studentExisting.ImagePath);
					}
					await _studentRepository.DeleteStudent(id);
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
