using System.Security.Claims;
using System.Text.RegularExpressions;
using Course_Overview.Areas.Admin.Repository;
using Course_Overview.Areas.Admin.Service;
using Course_Overview.Areas.Admin.Viewmodels;
using Course_Overview.Data;
using Course_Overview.Helper;
using Course_Overview.Service;
using LModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Course_Overview.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;      //Sử dụng Identity để Hash Password
        private readonly LoginService _loginService;
        private readonly EmailService _emailService;
        private readonly IPermissionRepository _permissionRepository;
        private readonly DatabaseContext _context;
        public UserController(IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            LoginService loginService,
            EmailService emailService,
            DatabaseContext context,
            IPermissionRepository permissionRepository
            )
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _loginService = loginService;
            _emailService = emailService;
            _context = context;
            _permissionRepository = permissionRepository;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUser();
            return View(users);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        //Phương thức Create User
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!ValidatePassword(model.Password, out string errorMessage))
                    {
                        TempData["ErrorMessage"] = "Password invalid format. Please try again";
                        return View(model);
                    }
                    var user = new User()
                    {
                        Email = model.Email,
                        Password = _passwordHasher.HashPassword(new User { Email = model.Email }, model.Password),
                        Name = model.Name,
                        Address = model.Address,
                        Phone = model.Phone,
                        FailedAttempts = 0,
                        EmailConfirmed = false,
                        IsNewUser = true
                    };

                    //Tạo ra 1 mã thông báo xác nhận email
                    var token = Guid.NewGuid().ToString();
                    user.EmailConfirmationToken = token;
                    user.EmailConfirmed = false;
                    user.IsNewUser = true;    // điều kiện này để user cần xác thực email trươc khi login


                    await _userRepository.AddUser(user);
                    //Gui thong bao ma xac nhan toi email
                    var callbackUrl = Url.Action("ConfirmEmail", "Auth", new { token }, protocol: HttpContext.Request.Scheme);
                    await _emailService.SendMail(user.Email, "Confirm your email", $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                    TempData["SuccessMessage"] = "Registration successful. Please check your email to confirm your account.";
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        //Phương thức delete
        [HttpPost]
        public async Task<IActionResult> Delete(string email)
        {
            try
            {
                var userExisting = await _userRepository.GetUserByEmailAsync(email);
                if (userExisting == null)
                {
                    return NotFound();
                }
                await _userRepository.DeleteUser(email);
                return RedirectToAction("Index", "Auth");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        //Password Securiry
        private bool ValidatePassword(string password, out string errorMessage)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");
            var hasSpecialChar = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasMinimum8Chars.IsMatch(password))
            {
                errorMessage = "Password should be at least 8 characters long.";
                return false;
            }
            else if (!hasUpperChar.IsMatch(password))
            {
                errorMessage = "Password should contain at least one uppercase letter.";
                return false;
            }
            else if (!hasLowerChar.IsMatch(password))
            {
                errorMessage = "Password should contain at least one lowercase letter.";
                return false;
            }
            else if (!hasNumber.IsMatch(password))
            {
                errorMessage = "Password should contain at least one numeric value.";
                return false;
            }
            else if (!hasSpecialChar.IsMatch(password))
            {
                errorMessage = "Password should contain at least one special character.";
                return false;
            }
            else
            {
                errorMessage = string.Empty;
                return true;
            }
        }

        //Phương thức này sẽ lấy thông tin người dùng và các quyền hiện có, sau đó tạo một PermissionAssignmentViewModel để hiển thị trên form.
        public async Task<IActionResult> AssignPermissions(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy tất cả các quyền
            var permissions = await _permissionRepository.GetAllPermissionAsync();

            // Lấy các quyền đã gán cho người dùng
            var grantedPermissions = await _context.UserPermissions
               .Where(up => up.UserId == id)
               .Select(up => up.PermissionId)
               .ToListAsync();

            // Tạo ViewModel
            var viewModel = new PermissionAssignmentViewModel
            {
                UserId = id,
                UserName = user.Name,
                RolePermissions = permissions.Select(p => new RolePermission
                {
                    PermissionId = p.PermissionId,
                    Permission = p
                }).ToList(),
                GrantedPermissions = grantedPermissions  // Danh sách các ID quyền đã được cấp
            };

            return View(viewModel);
        }

        [HttpPost]
        //Phương thức này sẽ nhận thông tin từ form và cập nhật quyền cho người dùng trong cơ sở dữ liệu.
        public async Task<IActionResult> AssignPermissions(PermissionAssignmentViewModel model)
        {
            if (ModelState.IsValid) // Kiểm tra ModelState
            {
                // Xóa tất cả các quyền hiện có của người dùng
                var existingPermissions = _context.UserPermissions
                    .Where(up => up.UserId == model.UserId)
                    .ToList();

                _context.UserPermissions.RemoveRange(existingPermissions); // Xóa các quyền hiện có

                // Gán quyền mới từ form
                if (model.GrantedPermissions != null)
                {
                    foreach (var permissionId in model.GrantedPermissions)
                    {
                        var userPermission = new UserPermission
                        {
                            UserId = model.UserId,
                            PermissionId = permissionId
                        };

                        await _context.UserPermissions.AddAsync(userPermission); // Thêm quyền mới vào cơ sở dữ liệu
                    }
                }

                await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

                // Xóa quyền cũ trong session
                HttpContext.Session.Remove("userPermissions"); // Xóa quyền cũ trước khi thêm quyền mới

                // Xóa claims cũ trước khi thêm claims mới
                var claimsIdentity = User.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    // Xóa tất cả claims quyền cũ
                    var existingClaims = claimsIdentity.FindAll("Permission").ToList();
                    foreach (var claim in existingClaims)
                    {
                        claimsIdentity.RemoveClaim(claim);
                    }
                }

                // Cập nhật quyền vào session
                await UpdateUserPermissionsInSession(model.UserId); // Gọi phương thức cập nhật quyền trong session

                // Cập nhật claims cho người dùng
                var user = await _userRepository.GetUserByIdAsync(model.UserId);
                await UpdateClaimsForUser(user);

                // Kiểm tra claims sau khi cập nhật
                var userClaims = User.Claims.ToList();
                foreach (var claim in userClaims)
                {
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }

                TempData["AssignSuccess"] = "Permissions assigned successfully."; // Thông báo thành công
                return RedirectToAction("Index", "User"); // Chuyển hướng về trang danh sách người dùng
            }

            TempData["ErrorMessage"] = "An error occurred while assigning permissions."; // Thông báo lỗi
            return View(model); // Trả lại view nếu có lỗi
        }

        //cập nhật quyền 
        private async Task UpdateUserPermissionsInSession(int userId)
        {
            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            var permissions = await _context.RolePermissions
                .Where(rp => userRoles.Contains(rp.RoleId))
                .Select(rp => rp.Permission.PermissionCode)
                .ToListAsync();

            SessionHelper.SetSession(HttpContext, "userPermissions", JsonConvert.SerializeObject(permissions));
        }



        //Xóa quyền
        [HttpPost]
        public async Task<IActionResult> RemovePermission(int userId, int permissionId)
        {
            var userPermission = await _context.UserPermissions
                .FirstOrDefaultAsync(up => up.UserId == userId && up.PermissionId == permissionId);

            if (userPermission != null)
            {
                _context.UserPermissions.Remove(userPermission);
                await _context.SaveChangesAsync();

                // Cập nhật quyền vào session
                var userRoles = await _context.UserRoles
                    .Where(ur => ur.UserId == userId)
                    .Select(ur => ur.RoleId)
                    .ToListAsync();

                var permissions = await _context.RolePermissions
                    .Where(rp => userRoles.Contains(rp.RoleId))
                    .Select(rp => rp.Permission.PermissionCode)
                    .ToListAsync();

                // Lưu quyền mới vào session
                SessionHelper.SetSession(HttpContext, "userPermissions", JsonConvert.SerializeObject(permissions));

                TempData["SuccessMessage"] = "Permission removed successfully.";
            }

            return RedirectToAction("Index", "User");
        }

        // Hàm cập nhật claims
        private async Task UpdateClaimsForUser(User user)
        {
            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == user.ID)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            var userPermissions = await _context.RolePermissions
                .Where(rp => userRoles.Contains(rp.RoleId))
                .Select(rp => rp.Permission.PermissionCode)
                .ToListAsync();

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };

            foreach (var permission in userPermissions)
            {
                userClaims.Add(new Claim("Permission", permission));
            }

            var identity = new ClaimsIdentity(userClaims, "Custom");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignOutAsync(); // Đăng xuất
            await HttpContext.SignInAsync(principal); // Đăng nhập lại với claims mới
        }

    }
}