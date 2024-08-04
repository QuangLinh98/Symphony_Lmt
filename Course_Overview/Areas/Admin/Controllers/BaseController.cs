using Course_Overview.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Course_Overview.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class BaseController : Controller
	{

		// Kiểm tra đăng nhập trong constructor
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);
			if (!IsUserLoggedIn())
			{
				context.Result = RedirectToAction("Login", "Auth");
			}
		}

		private bool IsUserLoggedIn()
		{
			// Sử dụng SessionHelper để kiểm tra trạng thái đăng nhập của User
			return SessionHelper.GetSession(HttpContext, "userSession") != null;
		}
	}
}
