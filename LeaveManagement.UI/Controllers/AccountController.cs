using LeaveManagement.Core.Domain.IdentityEntities;
using LeaveManagement.Core.DTO;
using LeaveManagement.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.UI.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
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
                // Check status of user (Employee by default)
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

                // Sign in
                await _signInManager.SignInAsync(user, true);

                return RedirectToAction("Index", "Leave", new
                {
                    area = "Employee"
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

        [Route("[action]")]
        [Route("/")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);

                return View(loginRequest);
            }

            var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, true, false);

            if (result.Succeeded)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(loginRequest.Email);

                if (user != null)
                {
                    // Check if user 'Admin'
                    if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
                    {
                        return RedirectToAction("Index", "Leave", new
                        {
                            Areas = "Admin"
                        });
                    }
                    // Check if user 'Employee'
                    else if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Employee.ToString()))
                    {
                        return RedirectToAction("Index", "Leave", new
                        {
                            Areas = "Employee"
                        });
                    }
                }

                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("Login", "Invalid email or password");
            return View(loginRequest);
        }
    }
}
