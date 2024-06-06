using LeaveManagement.Core.Domain.IdentityEntities;
using LeaveManagement.Core.DTO;
using LeaveManagement.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> users = _userManager.Users.ToList();

            Dictionary<Guid, List<string>> userRoles = new Dictionary<Guid, List<string>>();

            foreach (ApplicationUser user in users)
            {
                IEnumerable<string> roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(user.Id, roles.ToList());
            }

            ViewBag.UserRoles = userRoles;

            return View(users);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);

                return View(registerRequest);
            }

            ApplicationUser user = new ApplicationUser()
            {
                EmployeeName = registerRequest.Name,
                Email = registerRequest.Email,
                UserName = registerRequest.Email,
                PhoneNumber = registerRequest.Phone
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (result.Succeeded)
            {
                // Check status of radio button
                if (registerRequest.UserType == UserTypeOptions.Admin)
                {
                    // Create 'Admin' role
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {
                        ApplicationRole role = new ApplicationRole()
                        {
                            Name = UserTypeOptions.Admin.ToString()
                        };

                        await _roleManager.CreateAsync(role);
                    }

                    //Add the new user into 'Admin' role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Employee.ToString());
                }
                else if (registerRequest.UserType == UserTypeOptions.Employee)
                {
                    // Create 'Employee' role
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Employee.ToString()) is null)
                    {
                        ApplicationRole role = new ApplicationRole()
                        {
                            Name = UserTypeOptions.Employee.ToString()
                        };

                        await _roleManager.CreateAsync(role);
                    }

                    //Add the new user into 'Employee' role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Employee.ToString());
                }

                return RedirectToAction("Index", "Account", new
                {
                    area = "Admin"
                });
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }

                return View(registerRequest);
            }
        }
    }
}
