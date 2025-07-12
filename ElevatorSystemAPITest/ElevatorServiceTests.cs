using ElevatorSystemAPI.Application.Services;
using ElevatorSystemAPI.Domain.Enums;
using Microsoft.Extensions.Options;
using Xunit;
using Assert = Xunit.Assert;

namespace ElevatorSystemAPITest
{
    public class ElevatorServiceTests
    {
        private readonly ElevatorService _service;

        public ElevatorServiceTests()
        {
            var options = Options.Create(new ElevatorSystemOptions
            {
                ElevatorCount = 4
            });

            _service = new ElevatorService(options);
        }

        [Fact]
        public void GetAllElevators_ShouldReturnCorrectCount()
        {
            var elevators = _service.GetAllElevators();
            Assert.Equal(4, elevators.Count);
        }

        [Fact]
        public void AddStop_ShouldAddToCorrectElevator()
        {
            _service.AddStop(1, 6);
            var elevator = _service.GetElevatorById(1);

            Assert.True(elevator.StopsQueue.Contains(6));
        }

        [Fact]
        public void UpdateDirection_ShouldSetElevatorDirection()
        {
            _service.AddStop(1, 9);
            _service.UpdateDirection(1);

            var elevator = _service.GetElevatorById(1);
            Assert.Equal(Direction.Up, elevator.Direction);
        }

        [Fact]
        public void MoveElevator_ShouldMoveOneFloor()
        {
            _service.AddStop(1, 3);
            _service.UpdateDirection(1);
            _service.MoveElevator(1);

            var elevator = _service.GetElevatorById(1);
            Assert.Equal(2, elevator.CurrentFloor); // Started at 1
        }

        [Fact]
        public void HandleArrival_ShouldClearStop()
        {
            _service.AddStop(1, 3);
            _service.UpdateDirection(1);
            _service.MoveElevator(1);
            _service.MoveElevator(1);
            _service.HandleArrival(1);

            var elevator = _service.GetElevatorById(1);
            Assert.Empty(elevator.StopsQueue);
        }
    }
}
