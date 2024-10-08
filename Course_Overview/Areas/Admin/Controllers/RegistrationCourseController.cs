﻿using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Data;
using Course_Overview.Service;
using LModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class RegistrationCourseController : Controller
	{
        private readonly IStudentRepository _studentRepository;
        private readonly DatabaseContext _dbContext;
        private readonly EmailService _emailService;
        public RegistrationCourseController(DatabaseContext dbContext, EmailService emailService, IStudentRepository studentRepository)
		{
			_dbContext = dbContext;
			_emailService = emailService;
			_studentRepository = studentRepository;
		}
		public async Task<IActionResult> Index()
		{
			var registrations = await _dbContext.RegistrationCourses
							   .Include(cr => cr.Course)
							   .Include(cr => cr.Student)
							   .ToListAsync();
			return View(registrations);
		}

		//Phương thức xác nhận đăng ký 
		[HttpPost]
		public async Task<IActionResult> AcceptRegistration( int id)
		{
			var registration = await _dbContext.RegistrationCourses
                                               .Include(rc => rc.Student)
                                               .Include(rc => rc.Course)
                                               .FirstOrDefaultAsync(rc => rc.RegistrationCourseID == id);
			if (registration == null)
			{
				return NotFound();
			}
			registration.Status = "Accepted";
			await _dbContext.SaveChangesAsync();

			//Tạo ra 1 mã thông báo xác nhận email
			var student = registration.Student;
            var token = Guid.NewGuid().ToString();
			student.EmailConfirmationToken = token;
			student.EmailConfirmed = false;

            _dbContext.Students.Update(student);
            await _dbContext.SaveChangesAsync();

            //Gui thong bao ma xac nhan toi email
            var callbackUrl = Url.Action(
				"ConfirmEmail",
                "RegistrationCourse",
				new { token }, protocol: HttpContext.Request.Scheme);

            var studentName = student.Name;
            var courseName = registration.Course.CourseName;

            await _emailService.SendMail(student.Email, "Exam schedule announcement", $" Dear {studentName}, Symphony Center has confirmed your registration for the {courseName} course. Please click on the link to proceed to the placement test: <a href='{callbackUrl}'>link</a>");
            TempData["SuccessMessage"] = "Send mail to user successful.";

            return RedirectToAction("Index");
		}

        //Phương thưc xác nhận Email
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "Invalid token.";
                return RedirectToAction("Index");
            }

            try
            {
                var student = await _studentRepository.GetStudentByEmailConfirmationTokenAsync(token);
                if (student != null)
                {
					student.EmailConfirmed = true;
					student.EmailConfirmationToken = null;
                    await _studentRepository.UpdateStudent(student);
                    TempData["SuccessMessage"] = "Email confirmed successfully. Please proceed to the exams.";
                    return RedirectToAction("Index", "Exams", new { area = ""});
                }

                TempData["ErrorMessage"] = "Invalid token.";
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = "An error occurred while confirming your email. Please try again later.";
            }
            return RedirectToAction("Index", "Exams", new { area = "" });
        }

        //Phương thức từ chối đăng ký 
        [HttpPost]
        public async Task<IActionResult> RejectRegistration(int id)
        {
            var registration = await _dbContext.RegistrationCourses.FindAsync(id);
            if (registration == null)
            {
                return NotFound();
            }
            registration.Status = "Rejected";
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // 5. Xóa đăng ký
        public async Task<IActionResult> Delete(int id)
        {
            var registration = await _dbContext.RegistrationCourses.FindAsync(id);
            if (registration != null)
            {
                _dbContext.RegistrationCourses.Remove(registration);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}
