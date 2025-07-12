using ElevatorSystemAPI.Domain.Enums;

namespace ElevatorSystemAPI.Application.DTOs
{
    public class ElevatorStatusDto
    {
        public int Id { get; set; }
        public int CurrentFloor { get; set; }
        public Direction Direction { get; set; }
        public bool IsMoving { get; set; }
        public List<int> StopsQueue { get; set; } = new();
    }
}
