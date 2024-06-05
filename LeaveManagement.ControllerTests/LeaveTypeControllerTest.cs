using AutoFixture;
using FluentAssertions;
using LeaveManagement.Core.Domain.Entities;
using LeaveManagement.Core.DTO;
using LeaveManagement.Core.ServiceContracts;
using LeaveManagement.UI.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit.Abstractions;

namespace LeaveManagement.ControllerTests
{
    public class LeaveTypeControllerTest
    {
        public readonly LeaveTypeController _leaveTypeController;
        public readonly Mock<ILeaveTypeService> _leaveTypeServiceMock;
        public readonly ILeaveTypeService _leaveTypeService;
        private readonly IFixture _fixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public LeaveTypeControllerTest(ITestOutputHelper testOutputHelper)
        {
            _leaveTypeServiceMock = new Mock<ILeaveTypeService>();
            _leaveTypeService = _leaveTypeServiceMock.Object;
            _leaveTypeController = new LeaveTypeController(_leaveTypeService);
            _fixture = new Fixture();
            _testOutputHelper = testOutputHelper;
        }

        #region Index
        [Fact]
        public async Task Index_ToReturnIndexViewWithLeaveTypeList_ToBeSuccessful()
        {
            //Arrange
            List<LeaveTypeResponse> leaveTypeList = _fixture.Create<List<LeaveTypeResponse>>();

            _leaveTypeServiceMock
                .Setup(temp => temp.GetAllLeavesType())
                .ReturnsAsync(leaveTypeList);

            //Act
            IActionResult result = await _leaveTypeController.Index();

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<LeaveTypeResponse>>();
            viewResult.ViewData.Model.Should().Be(leaveTypeList);
        }
        #endregion

        #region Create
        [Fact]
        public async Task Create_IfModelError_ToReturnCreateView()
        {
            //Arrange
            LeaveTypeAddRequest leaveTypeAddRequest = _fixture.Create<LeaveTypeAddRequest>();
            LeaveType leaveType = leaveTypeAddRequest.ToLeaveType();
            LeaveTypeResponse leaveTypeResponse = leaveType.ToLeaveTypeResponse();

            _leaveTypeServiceMock
                .Setup(temp => temp
                .AddLeaveType(It.IsAny<LeaveTypeAddRequest>()))
                .ReturnsAsync(leaveTypeResponse);

            //Act
            _leaveTypeController.ModelState.AddModelError("Name", "Name cannot be blank");
            IActionResult result = await _leaveTypeController.Create(leaveTypeAddRequest);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<LeaveTypeAddRequest>();
            viewResult.ViewData.Model.Should().Be(leaveTypeAddRequest);
        }

        [Fact]
        public async Task Create_IfNoModelError_ToReturnRedirectToIndexView()
        {
            //Arrange
            LeaveTypeAddRequest leaveTypeAddRequest = _fixture.Create<LeaveTypeAddRequest>();
            LeaveType leaveType = leaveTypeAddRequest.ToLeaveType();
            LeaveTypeResponse leaveTypeResponse = leaveType.ToLeaveTypeResponse();

            _leaveTypeServiceMock
                .Setup(temp => temp
                .AddLeaveType(It.IsAny<LeaveTypeAddRequest>()))
                .ReturnsAsync(leaveTypeResponse);

            //Act
            IActionResult result = await _leaveTypeController.Create(leaveTypeAddRequest);

            //Assert
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ActionName.Should().Be("Index");
        }
        #endregion
    }
}