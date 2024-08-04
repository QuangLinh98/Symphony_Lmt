using Course_Overview.Areas.Admin.Repository;
using LModels;
using Microsoft.AspNetCore.Mvc;

namespace Course_Overview.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class RoleController : Controller
	{
		private readonly IRoleRepository _roleRepository;
		private readonly IRolePermissionrepository _rolePermissionRepository;
		public RoleController(IRoleRepository roleRepository, IRolePermissionrepository rolePermissionRepository)
		{
			_roleRepository = roleRepository;
			_rolePermissionRepository = rolePermissionRepository;
		}

		public async Task<IActionResult> Index()
		{
			var roles = await _roleRepository.GetAllRoleAsync();
			return View(roles);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult>Create(Role role)
		{
			try
			{
                if (ModelState.IsValid)
                {
					await _roleRepository.AddRoleAsync(role);
					return RedirectToAction("Index");
                }
            }				
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View(role);
		}

		public async Task<IActionResult> Update(int id)
		{
			var ExistingRole = await _roleRepository.GetOneRoleAsync(id);
            if (ExistingRole == null)
            {
				return NotFound();
            }
			return View(ExistingRole);
        }

		[HttpPost]
		public async Task<IActionResult>Update(Role role)
		{
			try
			{
                if (ModelState.IsValid)
                {
					await _roleRepository.UpdateRoleAsync(role);
					return RedirectToAction("Index");
				}
            }
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			return View(role);
		}

		public async Task<IActionResult>Delete(int id)
		{
			var ExistingRole = await _roleRepository.GetOneRoleAsync(id);
            if (ExistingRole == null)
            {
				return NotFound();
            }
			await _roleRepository.DeleteRoleAsync(id);
			return RedirectToAction("Index");
        }

		[HttpPost]
		public async Task<IActionResult>AssignPermissions(int roleId, List<int> permissionsId)
		{
			await _rolePermissionRepository.AssignPermissionsToRoleAsync(roleId, permissionsId);
			return RedirectToAction("Index");
		}
	}
}
