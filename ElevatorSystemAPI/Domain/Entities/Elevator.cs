using ElevatorSystemAPI.Domain.Enums;

namespace ElevatorSystemAPI.Domain.Entities
{
    public class Elevator
    {
        public int Id { get; set; }
        public int CurrentFloor { get; set; } = 1;
        public Direction Direction { get; set; } = Direction.Idle;
        public Queue<int> StopsQueue { get; private set; } = new();

        public bool IsMoving => Direction != Direction.Idle;

        public void AddStop(int floor)
        {
            if (!StopsQueue.Contains(floor))
                StopsQueue.Enqueue(floor);
        }

        public void MoveOneFloor()
        {
            if (Direction == Direction.Up) CurrentFloor++;
            else if (Direction == Direction.Down) CurrentFloor--;
        }

        public void UpdateDirection()
        {
            if (StopsQueue.Count == 0)
            {
                Direction = Direction.Idle;
                return;
            }

            var nextFloor = StopsQueue.Peek();
            Direction = nextFloor > CurrentFloor ? Direction.Up :
                        nextFloor < CurrentFloor ? Direction.Down :
                        Direction.Idle;
        }

        public bool IsAtNextStop()
        {
            return StopsQueue.Count > 0 && StopsQueue.Peek() == CurrentFloor;
        }

        public void ArriveAtFloor()
        {
            if (StopsQueue.Count > 0 && StopsQueue.Peek() == CurrentFloor)
                StopsQueue.Dequeue();
        }
    }
}
