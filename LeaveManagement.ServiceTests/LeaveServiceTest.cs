using AutoFixture;
using FluentAssertions;
using LeaveManagement.Core.Domain.Entities;
using LeaveManagement.Core.Domain.IdentityEntities;
using LeaveManagement.Core.Domain.RepositoryContracts;
using LeaveManagement.Core.DTO;
using LeaveManagement.Core.ServiceContracts;
using LeaveManagement.Core.Services;
using Moq;
using Xunit.Abstractions;

namespace LeaveManagement.ServiceTests
{
    public class LeaveServiceTest
    {
        private readonly ILeaveService _leaveService;
        private readonly Mock<ILeaveRepository> _leaveRepositoryMock;
        private readonly ILeaveRepository _leaveRepository;
        private readonly IFixture _fixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public LeaveServiceTest(ITestOutputHelper testOutputHelper)
        {
            _leaveRepositoryMock = new Mock<ILeaveRepository>();
            _leaveRepository = _leaveRepositoryMock.Object;
            _leaveService = new LeaveService(_leaveRepository);
            _fixture = new Fixture();
            _testOutputHelper = testOutputHelper;
        }

        #region AddLeave
        [Fact]
        public async Task AddLeave_NullLeave_ToBeArgumentNullException()
        {
            //Arrange
            LeaveAddRequest? leaveAddRequest = null;

            //Act
            Func<Task> action = async () =>
            {
                await _leaveService.AddLeave(leaveAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddLeave_NameIsNull_ToBeArgumentException()
        {
            //Arrange
            LeaveAddRequest leaveAddRequest = _fixture
                .Build<LeaveAddRequest>()
                .With(temp => temp.UserID, null as Guid?)
                .Create();

            //Act
            Func<Task> action = async () =>
            {
                await _leaveService.AddLeave(leaveAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddLeave_FullLeaveDetails_ToBeSuccessful()
        {
            //Arrange
            LeaveAddRequest? leaveAddRequest = _fixture.Create<LeaveAddRequest>();
            Leave leave = leaveAddRequest.ToLeave();
            LeaveResponse leaveResponseExpected = leave.ToLeaveResponse();

            _leaveRepositoryMock
                .Setup(temp => temp.AddLeave(It.IsAny<Leave>()))
                .ReturnsAsync(leave);

            //Act
            LeaveResponse leaveResponseActual = await _leaveService.AddLeave(leaveAddRequest);
            leaveResponseExpected.Id = leaveResponseActual.Id;

            //Assert
            leaveResponseActual.Id.Should().NotBe(Guid.Empty);
            leaveResponseActual.Should().Be(leaveResponseExpected);
        }
        #endregion

        #region GetAllLeave
        [Fact]
        public async Task GetAllLeave_EmptyList_ToBeEmpty()
        {
            //Arrange
            List<Leave> leaves = new List<Leave>();

            _leaveRepositoryMock
                .Setup(temp => temp.GetAllLeaves()).ReturnsAsync(leaves);

            //Act
            List<LeaveResponse> leaveResponsesListActual = await _leaveService.GetAllLeaves();

            //Assert
            leaveResponsesListActual.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllLeave_AddFewLeave_ToBeSuccessful()
        {
            //Arrange
            _fixture.Behaviors
               .OfType<ThrowingRecursionBehavior>().ToList()
               .ForEach(temp => _fixture.Behaviors.Remove(temp));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            List<Leave> leaves = new List<Leave>()
            {
                _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Approver, null as ApplicationUser)
                .Create(),

                _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Approver, null as ApplicationUser)
                .Create(),

                _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Approver, null as ApplicationUser)
                .Create()
            };

            List<LeaveResponse> leaveResponsesListExpected = leaves
                .Select(temp => temp
                .ToLeaveResponse())
                .ToList();

            _leaveRepositoryMock
                .Setup(temp => temp
                .GetAllLeaves())
                .ReturnsAsync(leaves);

            //print leaveResponsesListExpected
            _testOutputHelper.WriteLine("Expected: ");
            foreach (LeaveResponse leaveResponseExpected in leaveResponsesListExpected)
            {
                _testOutputHelper.WriteLine(leaveResponseExpected.ToString());
            }

            //Act
            List<LeaveResponse> leaveResponsesListActual = await _leaveService.GetAllLeaves();

            //print leaveResponsesListActual
            _testOutputHelper.WriteLine("Expected: ");
            foreach (LeaveResponse leaveResponseActual in leaveResponsesListActual)
            {
                _testOutputHelper.WriteLine(leaveResponseActual.ToString());
            }

            //Assert
            leaveResponsesListActual.Should().BeEquivalentTo(leaveResponsesListExpected);
        }
        #endregion

        #region GetLeaveByLeaveID
        [Fact]
        public async Task GetLeaveByLeaveID_NullLeaveID_ToBeNull()
        {
            //Arrange
            Guid? leaveID = null;

            //Act
            LeaveResponse? leaveResponseExpected = await _leaveService.GetLeaveByLeaveID(leaveID);

            //Assert
            leaveResponseExpected.Should().BeNull();
        }

        [Fact]
        public async Task GetLeaveByLeaveID_ValidLeaveID_ToBeSuccessful()
        {
            Leave leave = _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Approver, null as ApplicationUser)
                .Create();

            LeaveResponse leaveResponseExpected = leave.ToLeaveResponse();

            _leaveRepositoryMock
                .Setup(temp => temp
                .GetLeaveByLeaveID(It.IsAny<Guid>()))
                .ReturnsAsync(leave);

            //print leaveResponseExpected
            _testOutputHelper.WriteLine("Expected: ");
            _testOutputHelper.WriteLine(leaveResponseExpected.ToString());

            //Act
            LeaveResponse? leaveResponseActual = await _leaveService.GetLeaveByLeaveID(leave.Id);

            //print leaveResponseActual
            _testOutputHelper.WriteLine("Actual: ");
            _testOutputHelper.WriteLine(leaveResponseActual?.ToString());

            //Assert
            leaveResponseActual.Should().Be(leaveResponseExpected);
        }
        #endregion

        #region GetLeaveByUserID
        [Fact]
        public async Task GetLeaveByUserID_NullUserID_ToBeNull()
        {
            //Arrange
            Guid? userID = null;

            //Act
            List<LeaveResponse> leaveResponseExpected = await _leaveService.GetLeaveByUserID(userID);

            //Assert
            leaveResponseExpected.Should().BeNull();
        }

        [Fact]
        public async Task GetLeaveByUserID_ValidUserID_ToBeSuccessful()
        {
            //Arrange
            List<Leave> leaves = new List<Leave>()
            {
                _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Approver, null as ApplicationUser)
                .Create(),

                _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Approver, null as ApplicationUser)
                .Create(),

                _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Approver, null as ApplicationUser)
                .Create()
            };

            List<LeaveResponse> leaveResponsesListExpected = leaves
                .Select(temp => temp
                .ToLeaveResponse())
                .ToList();

            var userId = Guid.NewGuid();

            _leaveRepositoryMock
                .Setup(temp => temp
                .GetLeaveByUserID(userId))
                .ReturnsAsync(leaves);

            //print leaveResponsesListExpected
            _testOutputHelper.WriteLine("Expected: ");
            foreach (LeaveResponse leaveResponseExpected in leaveResponsesListExpected)
            {
                _testOutputHelper.WriteLine(leaveResponseExpected.ToString());
            }

            //Act
            List<LeaveResponse> leaveResponsesListActual = await _leaveService.GetLeaveByUserID(userId);

            //print leaveResponseActual
            _testOutputHelper.WriteLine("Actual: ");
            foreach (LeaveResponse leaveResponseActual in leaveResponsesListActual)
            {
                _testOutputHelper.WriteLine(leaveResponseActual.ToString());
            }
            //Assert
            leaveResponsesListActual.Should().BeEquivalentTo(leaveResponsesListExpected);
        }
        #endregion

        #region UpdateLeave
        [Fact]
        public async Task UpdateLeave_NullLeave_ToBeArgumentNullException()
        {
            //Arrange
            LeaveUpdateRequest? leaveUpdateRequest = null;

            //Act
            Func<Task> action = async () =>
            {
                await _leaveService.UpdateLeave(leaveUpdateRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateLeave_InvalidLeaveID_ToBeArgumentException()
        {
            //Arrange
            LeaveUpdateRequest? leaveUpdateRequest = _fixture.Build<LeaveUpdateRequest>().Create();
            Leave leave = leaveUpdateRequest.ToLeave();

            _leaveRepositoryMock
                .Setup(temp => temp
                .UpdateLeave(It.IsAny<Leave>()))
                .ReturnsAsync(leave);

            //Act
            Func<Task> action = async () =>
            {
                await _leaveService.UpdateLeave(leaveUpdateRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateLeave_NameIsNull_ToBeArgumentException()
        {
            //Arrange
            LeaveUpdateRequest leaveUpdateRequest = _fixture
                .Build<LeaveUpdateRequest>()
                .With(temp => temp.UserID, null as Guid?)
                .Create();

            Leave leave = leaveUpdateRequest.ToLeave();

            _leaveRepositoryMock
                .Setup(temp => temp
                .UpdateLeave(It.IsAny<Leave>()))
                .ReturnsAsync(leave);

            //Act
            Func<Task> action = async () =>
            {
                await _leaveService.UpdateLeave(leaveUpdateRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateLeave_FullLeaveDetails_ToBeSuccessful()
        {
            //Arrange
            _fixture.Behaviors
               .OfType<ThrowingRecursionBehavior>().ToList()
               .ForEach(temp => _fixture.Behaviors.Remove(temp));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            Leave leave = _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Approver, null as ApplicationUser)
                .With(temp => temp.Status, Core.Enums.StatusOptions.Approved.ToString())
                .Create();

            LeaveResponse leaveResponseExpected = leave.ToLeaveResponse();

            LeaveUpdateRequest leaveUpdateRequest = leaveResponseExpected.ToLeaveUpdateRequest();

            //print leaveResponseExpected
            _testOutputHelper.WriteLine("Expected: ");
            _testOutputHelper.WriteLine(leaveResponseExpected.ToString());

            _leaveRepositoryMock
                .Setup(temp => temp
                .GetLeaveByLeaveID(It.IsAny<Guid>()))
                .ReturnsAsync(leave);

            _leaveRepositoryMock
                .Setup(temp => temp
                .UpdateLeave(It.IsAny<Leave>()))
                .ReturnsAsync(leave);

            //Act
            LeaveResponse leaveResponseActual = await _leaveService.UpdateLeave(leaveUpdateRequest);

            //print leaveResponseActual
            _testOutputHelper.WriteLine("Actual: ");
            _testOutputHelper.WriteLine(leaveResponseActual.ToString());

            //Assert
            leaveResponseActual.Should().Be(leaveResponseExpected);
        }
        #endregion
    }
}
