using LeaveManagement.Core.DTO;
using LeaveManagement.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]")]
    public class LeaveTypeController : Controller
    {
        public readonly ILeaveTypeService _leaveTypeService;

        public LeaveTypeController(ILeaveTypeService leaveTypeService)
        {
            _leaveTypeService = leaveTypeService;
        }

        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            List<LeaveTypeResponse> leaveTypeList = await _leaveTypeService.GetAllLeavesType();
            return View(leaveTypeList);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create(LeaveTypeAddRequest leaveTypeAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return View(leaveTypeAddRequest);
            }

            LeaveTypeResponse leave_type_response = await _leaveTypeService.AddLeaveType(leaveTypeAddRequest);

            return RedirectToAction("Index", "LeaveType", new
            {
                area = "Admin"
            });
        }
    }
}
