using ElevatorSystemAPI.Domain.Entities;
using ElevatorSystemAPI.Domain.Enums;
using Xunit;
using Assert = Xunit.Assert;

namespace ElevatorSystemAPITest
{
    public class ElevatorTests
    {
        [Fact]
        public void AddStop_ShouldAddNewFloor()
        {
            var elevator = new Elevator();
            elevator.AddStop(5);

            Assert.True(elevator.StopsQueue.Contains(5));
        }

        [Fact]
        public void AddStop_ShouldNotAddDuplicate()
        {
            var elevator = new Elevator();
            elevator.AddStop(5);
            elevator.AddStop(5);

            Assert.Single(elevator.StopsQueue);
        }

        [Theory]
        [InlineData(2, 5, Direction.Up)]
        [InlineData(7, 3, Direction.Down)]
        public void UpdateDirection_ShouldSetCorrectDirection(int current, int target, Direction expected)
        {
            var elevator = new Elevator { CurrentFloor = current };
            elevator.AddStop(target);
            elevator.UpdateDirection();

            Assert.Equal(expected, elevator.Direction);
        }

        [Fact]
        public void MoveOneFloor_Up_ShouldIncrementFloor()
        {
            var elevator = new Elevator { CurrentFloor = 1, Direction = Direction.Up };
            elevator.MoveOneFloor();

            Assert.Equal(2, elevator.CurrentFloor);
        }

        [Fact]
        public void MoveOneFloor_Down_ShouldDecrementFloor()
        {
            var elevator = new Elevator { CurrentFloor = 3, Direction = Direction.Down };
            elevator.MoveOneFloor();

            Assert.Equal(2, elevator.CurrentFloor);
        }

        [Fact]
        public void ArriveAtFloor_ShouldRemoveStop()
        {
            var elevator = new Elevator { CurrentFloor = 5 };
            elevator.AddStop(5);
            elevator.ArriveAtFloor();

            Assert.Empty(elevator.StopsQueue);
        }
    }
}
