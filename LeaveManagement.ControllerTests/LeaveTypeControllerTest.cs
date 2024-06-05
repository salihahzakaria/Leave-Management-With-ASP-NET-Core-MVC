using AutoFixture;
using FluentAssertions;
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
    }
}