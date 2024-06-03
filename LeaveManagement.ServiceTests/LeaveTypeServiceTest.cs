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
        public async Task AddLeaveType_NullLeaveType_ToBeArgumentNullException()
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
        public async Task AddLeaveType_LeaveTypeNameIsNull_ToBeArgumentException()
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
        public async Task AddLeaveType_LeaveTypeNameIsDuplicate_ToBeArgumentException()
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
        public async Task AddLeaveType_FullLeaveTypeDetails_ToBeSuccessful()
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

        #region GetAllLeaveType 
        [Fact]
        public async Task GetAllLeaveType_EmptyList_ToBeEmpty()
        {
            //Arrange
            List<LeaveType> leaveTypes = new List<LeaveType>();

            _leaveTypeRepositoryMock
                .Setup(temp => temp.GetAllLeavesType()).ReturnsAsync(leaveTypes);

            //Act
            List<LeaveTypeResponse> leaveTypeResponseListActual = await _leaveTypeService.GetAllLeavesType();

            //Assert
            leaveTypeResponseListActual.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllLeaveType_AddFewLeaveType_ToBeSuccessful()
        {
            //Arrange
            _fixture.Behaviors
               .OfType<ThrowingRecursionBehavior>().ToList()
               .ForEach(temp => _fixture.Behaviors.Remove(temp));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            List<LeaveType> leaveTypes = new List<LeaveType>()
            {
                _fixture
                .Build<LeaveType>()
                .With(temp => temp.Name, "Annual Leave")
                .Create(),

                _fixture
                .Build<LeaveType>()
                .With(temp => temp.Name, "Emergency Leave")
                .Create(),

                _fixture
                .Build<LeaveType>()
                .With(temp => temp.Name, "Sick Leave")
                .Create(),
            };

            List<LeaveTypeResponse> leaveTypeResponsesListExpected = leaveTypes
                .Select(temp => temp
                .ToLeaveTypeResponse())
                .ToList();

            _leaveTypeRepositoryMock
                .Setup(temp => temp.GetAllLeavesType()).ReturnsAsync(leaveTypes);

            //print leaveTypeResponsesListExpected
            _testOutputHelper.WriteLine("Expected: ");
            foreach (LeaveTypeResponse leaveTypeResponsesExpected in leaveTypeResponsesListExpected)
            {
                _testOutputHelper.WriteLine(leaveTypeResponsesExpected.ToString());
            }

            //Act
            List<LeaveTypeResponse> leaveTypeResponseListActual = await _leaveTypeService.GetAllLeavesType();

            //print leaveTypeResponseListActual
            _testOutputHelper.WriteLine("Expected: ");
            foreach (LeaveTypeResponse leaveTypeResponseActual in leaveTypeResponseListActual)
            {
                _testOutputHelper.WriteLine(leaveTypeResponseActual.ToString());
            }

            //Assert
            leaveTypeResponseListActual.Should().BeEquivalentTo(leaveTypeResponsesListExpected);
        }
        #endregion

        #region GetLeaveTypeByLeaveTypeID
        [Fact]
        public async Task GetLeaveTypeByLeaveTypeID_NullLeaveTypeID_ToBeNull()
        {
            //Arrange
            Guid? leaveTypeID = null;

            //Act
            LeaveTypeResponse? leaveTypeResponseExpected = await _leaveTypeService.GetLeaveTypeByLeaveTypeID(leaveTypeID);

            //Assert
            leaveTypeResponseExpected.Should().BeNull();
        }

        [Fact]
        public async Task GetLeaveTypeByLeaveTypeID_ValidLeaveTypeID_ToBeSuccessful()
        {
            //Arrange
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(temp => _fixture.Behaviors.Remove(temp));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            LeaveType leaveType = _fixture
                .Build<LeaveType>()
                .With(temp => temp.Name, "Sick Leave")
                .Create();

            LeaveTypeResponse leaveTypeResponseExpected = leaveType.ToLeaveTypeResponse();

            _leaveTypeRepositoryMock
                .Setup(temp => temp
                .GetLeaveTypeByID(It.IsAny<Guid>()))
                .ReturnsAsync(leaveType);

            //print leaveTypeResponseExpected
            _testOutputHelper.WriteLine("Expected: ");
            _testOutputHelper.WriteLine(leaveTypeResponseExpected.ToString());

            //Act
            LeaveTypeResponse? leaveTypeResponseActual = await _leaveTypeService.GetLeaveTypeByLeaveTypeID(leaveType.Id);

            //print leaveTypeResponseActual
            _testOutputHelper.WriteLine("Actual: ");
            _testOutputHelper.WriteLine(leaveTypeResponseActual?.ToString());

            //Assert
            leaveTypeResponseActual.Should().Be(leaveTypeResponseExpected);
        }
        #endregion
    }
}