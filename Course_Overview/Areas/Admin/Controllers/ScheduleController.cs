using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using LModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Course_Overview.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly DatabaseContext _dbContext;

        public ScheduleController(IScheduleRepository scheduleRepository,DatabaseContext dbContext)
        {
            _scheduleRepository = scheduleRepository;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var schedules = await _scheduleRepository.GetAllScheduleAsync();
            return View(schedules);
        }

        public async Task<IActionResult>Detail(int id)
        {
            var schedule = await _scheduleRepository.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Courses = new SelectList(_dbContext.Courses, "CourseID", "CourseName", "CourseID");
            ViewBag.Classes = new SelectList(_dbContext.Classes, "ClassID", "ClassName", "ClassID");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>Create(Schedule schedule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _scheduleRepository.AddScheduleAsync(schedule);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.Courses = new SelectList(_dbContext.Courses, "CourseID", "CourseName", "CourseID");
            ViewBag.Classes = new SelectList(_dbContext.Classes, "ClassID", "ClassName", "ClassID");
            return View(schedule);
        }

        public async Task<IActionResult>Update(int id)
        {
            var ExistingSchedule = await _scheduleRepository.GetScheduleByIdAsync(id);
            if (ExistingSchedule == null)
            {
                return NotFound();
            }
            ViewBag.Courses = new SelectList(_dbContext.Courses, "CourseID", "CourseName", "CourseID");
            ViewBag.Classes = new SelectList(_dbContext.Classes, "ClassID", "ClassName", "ClassID");
            return View(ExistingSchedule);
        }

        [HttpPost]
        public async Task<IActionResult>Update(Schedule schedule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _scheduleRepository.UpdateScheduleAsync(schedule);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.Courses = new SelectList(_dbContext.Courses, "CourseID", "CourseName", "CourseID");
            ViewBag.Classes = new SelectList(_dbContext.Classes, "ClassID", "ClassName", "ClassID");
            return View(schedule);
        }

        public async Task<IActionResult>Delete(int id)
        {
            var ExistingSchedule = await _scheduleRepository.GetScheduleByIdAsync(id);
            if (ExistingSchedule == null)
            {
                return NotFound();
            }
            await _scheduleRepository.DeleteScheduleAsync(id);
            return RedirectToAction("Index");
        }
    }
}
