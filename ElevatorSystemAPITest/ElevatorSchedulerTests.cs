using ElevatorSystemAPI.Application.Interfaces;
using ElevatorSystemAPI.Application.Services;
using ElevatorSystemAPI.Domain.Entities;
using ElevatorSystemAPI.Domain.Enums;
using Moq;
using Xunit;

namespace ElevatorSystemAPITest
{
    public class ElevatorSchedulerTests
    {
        // Test to verify that an elevator request is assigned to an idle elevator
        public void RequestElevator_ShouldAssignToIdle()
        {
            // Arrange: Create a mock of the elevator service
            var mockService = new Mock<IElevatorService>();

            // Simulate an idle elevator at floor 1
            var elevator = new Elevator
            {
                Id = 1,
                CurrentFloor = 1,
                Direction = Direction.Idle
            };

            // Mock the service to return the idle elevator
            mockService.Setup(s => s.GetAllElevators())
                .Returns(new List<Elevator> { elevator });

            // Instantiate the scheduler with the mocked service
            var scheduler = new ElevatorScheduler(mockService.Object);

            // Create a request to go up from floor 5
            var request = new ElevatorRequest(5, Direction.Up);

            // Act: Make the request
            scheduler.RequestElevator(request);

            // Assert: Ensure the AddStop method was called with the correct parameters
            mockService.Verify(s => s.AddStop(1, 5), Times.Once);
        }
    }
}