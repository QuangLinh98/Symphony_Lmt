using System.Security.Claims;
using Course_Overview.Data;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Areas.Admin.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;
        public PermissionMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                    var userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var userPermissions = await GetUserPermissionsFromDatabase(_context, userId);

                    context.Items["UserPermissions"] = userPermissions;
                }
            }

            await _next(context);
        }

        private async Task<List<string>> GetUserPermissionsFromDatabase(DatabaseContext _context, string userId)
        {
            // Lấy quyền của người dùng từ cơ sở dữ liệu
            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == int.Parse(userId))
                .Select(ur => ur.RoleId)
                .ToListAsync();

            var userPermissions = await _context.RolePermissions
                .Where(rp => userRoles.Contains(rp.RoleId))
                .Select(rp => rp.Permission.PermissionCode)
                .ToListAsync();

            return userPermissions;
        }
    }
}
