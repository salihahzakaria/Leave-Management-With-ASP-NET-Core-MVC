using LeaveManagement.Core.Domain.IdentityEntities;
using LeaveManagement.Core.DTO;
using LeaveManagement.Core.Enums;
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

        [Route("[action]/{leaveId}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid leaveID)
        {
            LeaveResponse? leaveResponse = await _leaveService.GetLeaveByLeaveID(leaveID);

            if (leaveResponse == null)
            {
                return RedirectToAction("Index", "Leave", new
                {
                    area = "Admin"
                });
            }

            LeaveUpdateRequest leaveUpdateRequest = leaveResponse.ToLeaveUpdateRequest();

            List<LeaveTypeResponse> leaveTypes = await _leaveTypeService.GetAllLeavesType();
            ViewBag.LeaveTypes = leaveTypes.Select(temp => new SelectListItem()
            {
                Text = temp.Name,
                Value = temp.Id.ToString()
            });

            List<ApplicationUser> users = await _userManager.Users.ToListAsync();
            ViewBag.Users = users.Select(temp => new SelectListItem()
            {
                Text = temp.EmployeeName,
                Value = temp.Id.ToString()
            });

            List<StatusOptions> statuses = Enum.GetValues(typeof(StatusOptions)).Cast<StatusOptions>().ToList();
            ViewBag.Statuses = statuses.Select(temp => new SelectListItem()
            {
                Text = temp.ToString(),
                Value = temp.ToString()
            });

            ApplicationUser approver = await _userManager.GetUserAsync(User);
            ViewBag.ApproverID = approver.Id;
            ViewBag.Approver = approver.EmployeeName;

            return View(leaveUpdateRequest);
        }

        [Route("[action]/{leaveId}")]
        [HttpPost]
        public async Task<IActionResult> Edit(LeaveUpdateRequest leaveUpdateRequest)
        {
            LeaveResponse? leaveResponse = await _leaveService.GetLeaveByLeaveID(leaveUpdateRequest.Id);

            if (leaveResponse == null)
            {
                return RedirectToAction("Index", "Leaves");
            }

            if (!ModelState.IsValid)
            {
                List<LeaveTypeResponse> leaveTypes = await _leaveTypeService.GetAllLeavesType();
                ViewBag.LeaveTypes = leaveTypes.Select(temp => new SelectListItem()
                {
                    Text = temp.Name,
                    Value = temp.Id.ToString()
                });

                List<ApplicationUser> users = await _userManager.Users.ToListAsync();
                ViewBag.Users = users.Select(temp => new SelectListItem()
                {
                    Text = temp.EmployeeName,
                    Value = temp.Id.ToString()
                });

                ApplicationUser approver = await _userManager.GetUserAsync(User);
                ViewBag.ApproverID = approver.Id;
                ViewBag.Approver = approver.EmployeeName;

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View(leaveResponse.ToLeaveUpdateRequest());
            }

            LeaveResponse updatedLeave = await _leaveService.UpdateLeave(leaveUpdateRequest);

            return RedirectToAction("Index", "Leave", new
            {
                area = "Admin"
            });
        }
    }
}
