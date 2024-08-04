using Course_Overview.Areas.Admin.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Course_Overview.Views.Shared.Components
{
	public class CourseCategoryViewComponent:ViewComponent
	{
		private readonly ICourserRepository _courserRepository;
		public CourseCategoryViewComponent(ICourserRepository courserRepository)
		{
			_courserRepository = courserRepository;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var courses = await _courserRepository.GetAllCourse();
			return View(courses);
		}
	}
}
