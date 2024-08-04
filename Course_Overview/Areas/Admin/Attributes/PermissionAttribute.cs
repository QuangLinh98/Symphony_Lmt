using System.Security.Claims;
using Course_Overview.Data;
using Course_Overview.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Course_Overview.Areas.Admin.Attributes
{
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(string permission) : base(typeof(PermissionFilter))
        {
            Arguments = new object[] { permission };
        }

        //Kiểm tra quyền 
        public class PermissionFilter : IAuthorizationFilter
        {
            private readonly string _permission;

            public PermissionFilter(string permission)
            {
                _permission = permission;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var user = context.HttpContext.User;

                // Kiểm tra người dùng đã xác thực chưa
                if (!user.Identity.IsAuthenticated)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                // Kiểm tra quyền từ session
                var permissionsJson = SessionHelper.GetSession(context.HttpContext, "userPermissions");
                var permissions = string.IsNullOrEmpty(permissionsJson)
                    ? new List<string>()
                    : JsonConvert.DeserializeObject<List<string>>(permissionsJson);

                // Kiểm tra quyền trong session
                if (permissions.Contains(_permission))
                {
                    return; // Đã có quyền, cho phép truy cập
                }

                // Nếu không có quyền trong session, kiểm tra từ database
                // Sử dụng Task.Run để gọi phương thức bất đồng bộ
                var dbContext = context.HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;
                var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                // Lấy các quyền từ database
                var userRoles = dbContext.UserRoles
                    .Where(ur => ur.UserId == int.Parse(userId))
                    .Select(ur => ur.RoleId)
                    .ToList();

                var permissionsFromDb = dbContext.RolePermissions
                    .Where(rp => userRoles.Contains(rp.RoleId))
                    .Select(rp => rp.Permission.PermissionCode)
                    .ToList();

                // Kiểm tra quyền trong permissionsFromDb
                if (!permissionsFromDb.Contains(_permission))
                {
                    context.Result = new ForbidResult(); // Không có quyền
                }
            }


            // Phương thức không đồng bộ để kiểm tra quyền
            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                var user = context.HttpContext.User;

                // Kiểm tra người dùng đã xác thực chưa
                if (!user.Identity.IsAuthenticated)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var dbContext = context.HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;
                var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var userRoles = await dbContext.UserRoles
                    .Where(ur => ur.UserId == int.Parse(userId))
                    .Select(ur => ur.RoleId)
                    .ToListAsync();

                var permissions = await dbContext.RolePermissions
                    .Where(rp => userRoles.Contains(rp.RoleId))
                    .Select(rp => rp.Permission.PermissionCode)
                    .ToListAsync();

                // Kiểm tra quyền trong claims
                if (!permissions.Contains(_permission))
                {
                    context.Result = new ForbidResult();
                }
            }
        }

    }
}