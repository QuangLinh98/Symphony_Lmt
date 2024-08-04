using System.Text.Json;
using System.Text.Json.Serialization;
using Course_Overview.Areas.Admin.Attributes;
using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Helper;
using LModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Extensions;

namespace Course_Overview.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class TopicController : BaseController
	{
		private readonly ITopicRepository _topicRepository;
		private readonly ICourserRepository _courseRepository;
		public TopicController(ITopicRepository topicRepository , ICourserRepository courserRepository)
		{
			_topicRepository = topicRepository;
			_courseRepository = courserRepository;
		}

		public async Task<IActionResult> Index(int? page)
		{
            return View();
		}

		public async Task<IActionResult>GetTopic()
		{
			var topics = await _topicRepository.GetAllTopic();
			var options = new JsonSerializerOptions
			{
				ReferenceHandler = ReferenceHandler.IgnoreCycles,
				WriteIndented = true
			};
			return Json(new { data = topics }, options);
		}

        public async Task<IActionResult> Create()
		{
			var coures = await _courseRepository.GetAllCourse();
			ViewBag.Courses = new SelectList(coures, "CourseID", "CourseName", "CourseID");
			return View();
		}

		[HttpPost]

        public async Task<IActionResult> Create(Topic topic, IFormFile imageFile)
		{
			try
			{
                if (ModelState.IsValid)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
						string subFolder = "TopicImage";
						string saveImagePath = await UploadFile.SaveImage(subFolder, imageFile);
						topic.ImagePath = saveImagePath;
                    }
					await _topicRepository.AddTopic(topic);
					return RedirectToAction("Index");
                }
            }
			catch(Exception ex) 
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View(topic);
		}


        public async Task<IActionResult> Update(int id)
		{
			var coures = await _courseRepository.GetAllCourse();
			ViewBag.Courses = new SelectList(coures, "CourseID", "CourseName", "CourseID");

			var topicExisting = await _topicRepository.GetOneTopic(id);
			if (topicExisting == null)
			{
				return NotFound();
			}
			
			return View(topicExisting);
		}

		[HttpPost]

        public async Task<IActionResult> Update(Topic topic)
		{
			try
			{
                if (ModelState.IsValid)
                {
                    if (topic.ImageFile != null)
                    {
						// Xử lý lưu hình ảnh mới vào thư mục TopicImage
						var imagePath = await UploadFile.SaveImage("TopicImage", topic.ImageFile);

                        if (!string.IsNullOrEmpty(imagePath))
                        {
							// Xóa hình ảnh cũ
							if (!string.IsNullOrEmpty(topic.ImagePath))
							{
								UploadFile.DeleteImage( topic.ImagePath);
							}
							// Cập nhật đường dẫn mới vào model
							topic.ImagePath = imagePath;
						}                 
                    }

					await _topicRepository.UpdateTopic(topic);
					return RedirectToAction("Index");
				}
            }
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View(topic);
		}


        public async Task<IActionResult>Delete(int id)
		{
			try
			{
				var topicExisting = await _topicRepository.GetOneTopic(id);
				if (topicExisting == null)
				{
					return NotFound();
				}
				else
				{
					if (!string.IsNullOrEmpty(topicExisting.ImagePath))
					{
						UploadFile.DeleteImage(topicExisting.ImagePath);
					}
					await _topicRepository.DeleteTopic(id);
                    return Ok(new { success = true, message = "Topic deleted successfully!" });
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
