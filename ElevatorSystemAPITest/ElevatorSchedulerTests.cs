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
        /// <summary>
        /// requestElevator should assign an elevator to an idle elevator when available.
        /// </summary>
        [Fact]
        public void RequestElevator_ShouldAssignToIdle()
        {
            var mockService = new Mock<IElevatorService>();

            var elevator = new Elevator
            {
                Id = 1,
                CurrentFloor = 1,
                Direction = Direction.Idle
            };

            mockService.Setup(s => s.GetAllElevators())
                .Returns(new List<Elevator> { elevator });

            var scheduler = new ElevatorScheduler(mockService.Object);
            var request = new ElevatorRequest(5, Direction.Up);

            scheduler.RequestElevator(request);

            mockService.Verify(s => s.AddStop(1, 5), Times.Once);
        }
    }
}
