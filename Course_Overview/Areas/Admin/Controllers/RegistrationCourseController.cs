using Course_Overview.Areas.Admin.Repository;
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
        private readonly IUserRepository _userRepository;
        private readonly DatabaseContext _dbContext;
        private readonly EmailService _emailService;
        public RegistrationCourseController(DatabaseContext dbContext, EmailService emailService,IUserRepository userRepository)
		{
			_dbContext = dbContext;
			_emailService = emailService;
            _userRepository = userRepository;
		}
		public async Task<IActionResult> Index()
		{
			var registrations = await _dbContext.RegistrationCourses
							   .Include(cr => cr.Course)
							   .Include(cr => cr.User)
							   .ToListAsync();
			return View(registrations);
		}

		//Phương thức xác nhận đăng ký 
		[HttpPost]
		public async Task<IActionResult> AcceptRegistration( int id)
		{
			var registration = await _dbContext.RegistrationCourses
                                               .Include(rc => rc.User)
                                               .FirstOrDefaultAsync(rc => rc.RegistrationCourseID == id);
			if (registration == null)
			{
				return NotFound();
			}
			registration.Status = "Accepted";
			await _dbContext.SaveChangesAsync();

			//Tạo ra 1 mã thông báo xác nhận email
			var user = registration.User;
            var token = Guid.NewGuid().ToString();
            user.EmailConfirmationToken = token;
            user.EmailConfirmed = false;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            //Gui thong bao ma xac nhan toi email
            var callbackUrl = Url.Action(
				"ConfirmEmail",
                "RegistrationCourse",
				new { token }, protocol: HttpContext.Request.Scheme);

            await _emailService.SendMail(user.Email, "Exam schedule announcement", $" Dear Nguyen Quang Linh, Symphony Center has confirmed your registration for the NodeJS course. Please click on the link to proceed to the placement test: <a href='{callbackUrl}'>link</a>");
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
                var user = await _userRepository.GetUserByEmailConfirmationTokenAsync(token);
                if (user != null)
                {
                    user.EmailConfirmed = true;
                    user.EmailConfirmationToken = null;
                    await _userRepository.UpdateUser(user);
                    TempData["SuccessMessage"] = "Email confirmed successfully. You can start the exam now.";
                    return RedirectToAction("Login");
                }

                TempData["ErrorMessage"] = "Invalid token.";
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = "An error occurred while confirming your email. Please try again later.";
            }
            return RedirectToAction("Index");
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
