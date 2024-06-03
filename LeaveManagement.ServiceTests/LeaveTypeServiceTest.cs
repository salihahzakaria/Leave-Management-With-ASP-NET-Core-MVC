using AutoFixture;
using FluentAssertions;
using LeaveManagement.Core.Domain.Entities;
using LeaveManagement.Core.Domain.RepositoryContracts;
using LeaveManagement.Core.DTO;
using LeaveManagement.Core.ServiceContracts;
using LeaveManagement.Core.Services;
using Moq;
using Xunit.Abstractions;

namespace LeaveManagement.ServiceTests
{
    public class LeaveTypeServiceTest
    {
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly Mock<ILeaveTypeRepository> _leaveTypeRepositoryMock;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IFixture _fixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public LeaveTypeServiceTest(ITestOutputHelper testOutputHelper)
        {
            _leaveTypeRepositoryMock = new Mock<ILeaveTypeRepository>();
            _leaveTypeRepository = _leaveTypeRepositoryMock.Object;
            _leaveTypeService = new LeaveTypeService(_leaveTypeRepository);
            _fixture = new Fixture();
            _testOutputHelper = testOutputHelper;
        }

        #region AddLeaveType
        [Fact]
        public async void AddLeaveType_NullLeaveType_ToBeArgumentNullException()
        {
            //Arrange
            LeaveTypeAddRequest leaveTypeAddRequest = null;

            //Act
            Func<Task> action = async () =>
            {
                await _leaveTypeService.AddLeaveType(leaveTypeAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async void AddLeaveType_LeaveTypeNameIsNull_ToBeArgumentException()
        {
            //Arrange
            LeaveTypeAddRequest? leaveTypeAddRequest = _fixture
                .Build<LeaveTypeAddRequest>()
                .With(temp => temp.Name, null as string)
                .Create();

            LeaveType leaveType = leaveTypeAddRequest.ToLeaveType();

            _leaveTypeRepositoryMock
                .Setup(temp => temp
                .AddLeaveType(It.IsAny<LeaveType>()))
                .ReturnsAsync(leaveType);

            //Act
            Func<Task> action = async () =>
            {
                await _leaveTypeService.AddLeaveType(leaveTypeAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void AddLeaveType_LeaveTypeNameIsDuplicate_ToBeArgumentException()
        {
            //Arrange
            LeaveTypeAddRequest? leaveTypeAddRequest = _fixture
                .Build<LeaveTypeAddRequest>()
                .With(temp => temp.Name, "Annual Leave")
                .Create();

            LeaveType leaveType = leaveTypeAddRequest.ToLeaveType();
            LeaveTypeResponse leaveTypeResponse = leaveType.ToLeaveTypeResponse();

            _leaveTypeRepositoryMock
                .Setup(temp => temp
                .AddLeaveType(It.IsAny<LeaveType>()))
                .ReturnsAsync(leaveType);

            _leaveTypeRepositoryMock
                .Setup(temp => temp
                .GetLeaveTypeByName(leaveType.Name))
                .ReturnsAsync(leaveType);

            //Act
            Func<Task> action = async () =>
            {
                await _leaveTypeService.AddLeaveType(leaveTypeAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void AddLeaveType_FullLeaveTypeDetails_ToBeSuccessful()
        {
            //Arrange
            LeaveTypeAddRequest? leaveTypeAddRequest = _fixture.Create<LeaveTypeAddRequest>();
            LeaveType leaveType = leaveTypeAddRequest.ToLeaveType();
            LeaveTypeResponse leaveTypeResponseExpected = leaveType.ToLeaveTypeResponse();

            _leaveTypeRepositoryMock
                .Setup(temp => temp
                .AddLeaveType(It.IsAny<LeaveType>()))
                .ReturnsAsync(leaveType);

            //Act
             LeaveTypeResponse leaveTypeResponseFromAdd = await _leaveTypeService.AddLeaveType(leaveTypeAddRequest);
            leaveTypeResponseExpected.Id = leaveTypeResponseFromAdd.Id;

            //Assert
            leaveTypeResponseFromAdd.Id.Should().NotBe(Guid.Empty);
            leaveTypeResponseFromAdd.Should().Be(leaveTypeResponseExpected);
        }
        #endregion
    }
}