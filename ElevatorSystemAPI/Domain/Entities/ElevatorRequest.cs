using ElevatorSystemAPI.Domain.Enums;

namespace ElevatorSystemAPI.Domain.Entities
{
    public class ElevatorRequest
    {
        public int RequestedFloor { get; set; }
        public Direction Direction { get; set; }

        public ElevatorRequest(int requestedFloor, Direction direction)
        {
            RequestedFloor = requestedFloor;
            Direction = direction;
        }
    }
}
