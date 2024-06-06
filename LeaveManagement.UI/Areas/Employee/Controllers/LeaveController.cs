using LeaveManagement.Core.Domain.IdentityEntities;
using LeaveManagement.Core.DTO;
using LeaveManagement.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagement.UI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Route("[area]/[controller]")]
    public class LeaveController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILeaveService _leaveService;

        public LeaveController(UserManager<ApplicationUser> userManager, 
            ILeaveTypeService leaveTypeService, 
            ILeaveService leaveService)
        {
            _userManager = userManager;
            _leaveTypeService = leaveTypeService;
            _leaveService = leaveService;
        }

        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            List<LeaveResponse> leaves = await _leaveService.GetLeaveByUserID(user.Id);

            if (leaves == null || !leaves.Any())
            {
                ViewBag.Message = "No leave records found.";
                leaves = new List<LeaveResponse>(); 
            }

            return View(leaves);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            ViewBag.UserID = user.Id;

            List<LeaveTypeResponse> leaveTypes = await _leaveTypeService.GetAllLeavesType();
            ViewBag.LeaveTypes = leaveTypes.Select(temp => new SelectListItem()
            {
                Text = temp.Name,
                Value = temp.Id.ToString()
            });

            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create(LeaveAddRequest leaveAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                ViewBag.UserID = user.Id;

                List<LeaveTypeResponse> leaveTypes = await _leaveTypeService.GetAllLeavesType();
                ViewBag.LeaveTypes = leaveTypes;

                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return View(leaveAddRequest);
            }

            LeaveResponse leaveResponse = await _leaveService.AddLeave(leaveAddRequest);

            return RedirectToAction("Index", "Leave", new
            {
                area = "Employee"
            });
        }
    }
}
