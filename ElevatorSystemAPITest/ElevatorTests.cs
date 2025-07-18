using ElevatorSystemAPI.Domain.Entities;
using ElevatorSystemAPI.Domain.Enums;
using Xunit;
using Assert = Xunit.Assert;

namespace ElevatorSystemAPITest
{
    public class ElevatorTests
    {
        // Test: AddStop should add a new floor to the queue if it doesn't already exist
        [Fact]
        public void AddStop_ShouldAddNewFloor()
        {
            var elevator = new Elevator();
            elevator.AddStop(5); // Add floor 5 to the queue

            Assert.True(elevator.StopsQueue.Contains(5)); // Assert it's added
        }

        // AddStop should not add the same floor twice (no duplicates)
        [Fact]
        public void AddStop_ShouldNotAddDuplicate()
        {
            var elevator = new Elevator();
            elevator.AddStop(5);
            elevator.AddStop(5); // Attempt to add duplicate floor

            Assert.Single(elevator.StopsQueue); // Assert only one entry
        }

        // UpdateDirection sets the correct direction based on target floor
        [Theory]
        [InlineData(2, 5, Direction.Up)]   // If current is 2 and target is 5, direction should be Up
        [InlineData(7, 3, Direction.Down)] // If current is 7 and target is 3, direction should be Down
        public void UpdateDirection_ShouldSetCorrectDirection(int current, int target, Direction expected)
        {
            var elevator = new Elevator { CurrentFloor = current };
            elevator.AddStop(target);      // Add the target floor
            elevator.UpdateDirection();    // Determine direction

            Assert.Equal(expected, elevator.Direction); // Validate direction
        }

        // MoveOneFloor increments floor when going Up
        [Fact]
        public void MoveOneFloor_Up_ShouldIncrementFloor()
        {
            var elevator = new Elevator { CurrentFloor = 1, Direction = Direction.Up };
            elevator.MoveOneFloor(); // Should go from 1 to 2

            Assert.Equal(2, elevator.CurrentFloor);
        }

        // MoveOneFloor decrements floor when going Down
        [Fact]
        public void MoveOneFloor_Down_ShouldDecrementFloor()
        {
            var elevator = new Elevator { CurrentFloor = 3, Direction = Direction.Down };
            elevator.MoveOneFloor(); // Should go from 3 to 2

            Assert.Equal(2, elevator.CurrentFloor);
        }

        // ArriveAtFloor should remove current floor from the queue
        [Fact]
        public void ArriveAtFloor_ShouldRemoveStop()
        {
            var elevator = new Elevator { CurrentFloor = 5 };
            elevator.AddStop(5);           // Add stop at floor 5
            elevator.ArriveAtFloor();      // Simulate arriving at floor

            Assert.Empty(elevator.StopsQueue); // Stop should be removed
        }
    }
}
