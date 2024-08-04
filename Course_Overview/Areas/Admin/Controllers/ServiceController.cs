using Course_Overview.Data;
using LModels.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ServiceController : BaseController
	{
		private readonly DatabaseContext _dbContext;
		public ServiceController(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<IActionResult> Index()
		{
			var services = await _dbContext.Services.ToListAsync();
			return View(services);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Services service)
		{
            if (ModelState.IsValid)
            {
				await _dbContext.Services.AddAsync(service);
				await _dbContext.SaveChangesAsync();
				return RedirectToAction("Index");
            }
			return View(service);
        }

		public async Task<IActionResult> Update(int id)
		{
			var serviceExisting = await _dbContext.Services.FindAsync(id);
			if (serviceExisting == null)
			{
				return NotFound();
			}
			return View(serviceExisting);
		}

		[HttpPost]
		public async Task<IActionResult>Update(Services services)
		{
			try
			{
                if (ModelState.IsValid)
                {
					_dbContext.Services.Update(services);
					await _dbContext.SaveChangesAsync();
					return RedirectToAction("Index");
                }
            }
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View(services);
		}

		public async Task<IActionResult>Delete(int id)
		{
			var service = await _dbContext.Services.FindAsync(id);
            if (service == null)
            {
				return NotFound();
            }
			_dbContext.Services.Remove(service);
			await _dbContext.SaveChangesAsync();
			return RedirectToAction("Index");
        }
	}
}
