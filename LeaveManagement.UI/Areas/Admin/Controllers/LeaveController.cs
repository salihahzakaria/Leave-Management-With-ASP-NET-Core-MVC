using LeaveManagement.Core.Domain.IdentityEntities;
using LeaveManagement.Core.DTO;
using LeaveManagement.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    }
}
