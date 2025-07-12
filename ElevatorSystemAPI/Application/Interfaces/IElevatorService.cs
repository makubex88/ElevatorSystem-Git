using ElevatorSystemAPI.Domain.Entities;

namespace ElevatorSystemAPI.Application.Interfaces
{
    public interface IElevatorService
    {
        List<Elevator> GetAllElevators();
        Elevator GetElevatorById(int elevatorId);
        void AddStop(int elevatorId, int floor);
        void MoveElevator(int elevatorId);
        void UpdateDirection(int elevatorId);
        void HandleArrival(int elevatorId);
    }
}
