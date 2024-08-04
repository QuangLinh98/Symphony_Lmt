using System.Text.Json.Serialization;
using System.Text.Json;
using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Course_Overview.Areas.Admin.Attributes;

namespace Course_Overview.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClassController : BaseController
	{
        private readonly IClassRepository _classRepository;
        private readonly ITeacherRepository _teacherRepository;

        public ClassController(IClassRepository classRepository, ITeacherRepository teacherRepository)
        {
            _classRepository = classRepository;
            _teacherRepository = teacherRepository;
        }


        public IActionResult Index()
        {          
            return View();
        }

        public async Task<IActionResult> GetClass()
        {
            var rooms = await _classRepository.GetAllClass();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return Json(new { data = rooms }, options);
        }


        public async Task<IActionResult> Create()
        {
            var teachers = await _teacherRepository.GetAllTeacher();
            ViewBag.Teacher = new SelectList(teachers, "TeacherID", "FullName", "TeacherID");
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create( Class room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _classRepository.AddClass(room);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(room);
        }


        public async Task<IActionResult>Update(int id)
        {
			var teachers = await _teacherRepository.GetAllTeacher();
			ViewBag.Teacher = new SelectList(teachers, "TeacherID", "FullName", "TeacherID");

			var roomExisting = await _classRepository.GetOneClass(id);
            return View(roomExisting);
        }

        [HttpPost]

        public async Task<IActionResult>Update(Class room)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Bad request!");
                }
                await _classRepository.UpdateClass(room);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);  
            }
			return View(room);
		}

        public async Task<IActionResult>Delete(int id)
        {
            var classExisting = await _classRepository.GetOneClass(id);
            if (classExisting == null)
            {
                return NotFound();
            }
            await _classRepository.DeleteClass(id);
            return RedirectToAction("Index");
        }
    }
}
