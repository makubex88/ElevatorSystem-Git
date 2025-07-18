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

        // Initializes the ElevatorService with 4 elevators for testing
        public ElevatorServiceTests()
        {
            // Create mock configuration with 4 elevators
            var options = Options.Create(new ElevatorSystemOptions
            {
                ElevatorCount = 4 // Simulate a system with 4 elevator instances
            });

            // Instantiate the service under test using the configured options
            _service = new ElevatorService(options);
        }

        // GetAllElevators should return exactly 4 elevators (initial system setup)
        [Fact]
        public void GetAllElevators_ShouldReturnCorrectCount()
        {
            var elevators = _service.GetAllElevators();

            Assert.Equal(4, elevators.Count); // System is expected to have 4 elevators
        }

        // AddStop should add a floor to the correct elevator's stop queue
        [Fact]
        public void AddStop_ShouldAddToCorrectElevator()
        {
            _service.AddStop(1, 6); // Add floor 6 to elevator with ID 1
            var elevator = _service.GetElevatorById(1);

            Assert.True(elevator.StopsQueue.Contains(6)); // Validate that stop was added
        }

        // UpdateDirection sets the correct movement direction for elevator
        [Fact]
        public void UpdateDirection_ShouldSetElevatorDirection()
        {
            _service.AddStop(1, 9);         // Add higher floor to queue
            _service.UpdateDirection(1);    // Trigger direction calculation

            var elevator = _service.GetElevatorById(1);
            Assert.Equal(Direction.Up, elevator.Direction); // Expect Up since 9 > 1
        }

        // MoveElevator should move one floor in the current direction
        [Fact]
        public void MoveElevator_ShouldMoveOneFloor()
        {
            _service.AddStop(1, 3);         // Destination is 3
            _service.UpdateDirection(1);    // Should be Up from 1
            _service.MoveElevator(1);       // Should move from 1 to 2

            var elevator = _service.GetElevatorById(1);
            Assert.Equal(2, elevator.CurrentFloor); // Verify moved one floor up
        }

        // ✅ Test: HandleArrival should clear the stop after reaching the destination
        [Fact]
        public void HandleArrival_ShouldClearStop()
        {
            _service.AddStop(1, 3);         // Target floor is 3
            _service.UpdateDirection(1);
            _service.MoveElevator(1);       // Move from 1 to 2
            _service.MoveElevator(1);       // Move from 2 to 3
            _service.HandleArrival(1);      // Simulate stop handling

            var elevator = _service.GetElevatorById(1);
            Assert.Empty(elevator.StopsQueue); // Stop should be removed after arrival
        }
    }
}
