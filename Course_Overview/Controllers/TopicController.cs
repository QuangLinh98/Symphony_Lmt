using Course_Overview.Areas.Admin.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Course_Overview.Controllers
{
	public class TopicController : Controller
	{
		private readonly ITopicRepository _topicRepository;
		public TopicController(ITopicRepository topicRepository)
		{
			_topicRepository = topicRepository;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult>TopicDetail(int id)
		{
			var topicDetail = await _topicRepository.GetOneTopic(id);
            if (topicDetail == null)
            {
				return NotFound();
            }

			return View(topicDetail);
        }
	}
}
