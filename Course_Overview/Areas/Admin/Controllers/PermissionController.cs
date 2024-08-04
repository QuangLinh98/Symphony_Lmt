using System.Data;
using Course_Overview.Areas.Admin.Repository;
using LModels;
using Microsoft.AspNetCore.Mvc;

namespace Course_Overview.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class PermissionController : Controller
	{
		private readonly IPermissionRepository _permissionRepository;
		public PermissionController(IPermissionRepository permissionRepository)
		{
			_permissionRepository = permissionRepository;
		}

		public async Task<IActionResult> Index()
		{
			var permissions = await _permissionRepository.GetAllPermissionAsync();
			return View(permissions);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Permission permission)
		{
			try
			{
                if (ModelState.IsValid)
                {
					await _permissionRepository.AddPermissionAsync(permission);
					return RedirectToAction("Index");
                }
            }
				catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View(permission);
		}

		public async Task<IActionResult> Update(int id)
		{
			var ExistingPermission = await _permissionRepository.GetOnePermissionAsync(id);
			if (ExistingPermission == null)
			{
				return NotFound();
			}
			return View(ExistingPermission);
		}

		[HttpPost]
		public async Task<IActionResult> Update(Permission permission)
		{
			try
			{
				if (ModelState.IsValid)
				{
					await _permissionRepository.UpdatePermissionAsync(permission);
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View(permission);
		}

		public async Task<IActionResult> Delete(int id)
		{
			var ExistingPermission = await _permissionRepository.GetOnePermissionAsync(id);
			if (ExistingPermission == null)
			{
				return NotFound();
			}
			await _permissionRepository.DeletePermissionAsync(id);
			return RedirectToAction("Index");
		}
	}
}
