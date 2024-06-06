using LeaveManagement.Core.Domain.IdentityEntities;
using LeaveManagement.Core.DTO;
using LeaveManagement.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.UI.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
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
            List<LeaveResponse> leaves = await _leaveService.GetAllLeaves();

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
            List<ApplicationUser> users = await _userManager.Users.ToListAsync();
            ViewBag.Users = users.Select(temp => new SelectListItem()
            {
                Text = temp.EmployeeName,
                Value = temp.Id.ToString()
            });

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
                List<ApplicationUser> users = await _userManager.Users.ToListAsync();
                ViewBag.Users = users.Select(temp => new SelectListItem()
                {
                    Text = temp.EmployeeName,
                    Value = temp.Id.ToString()
                });

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
                area = "Admin"
            });
        }
    }
}
