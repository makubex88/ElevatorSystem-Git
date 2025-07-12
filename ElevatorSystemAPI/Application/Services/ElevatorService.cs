using ElevatorSystemAPI.Application.Interfaces;
using ElevatorSystemAPI.Domain.Entities;
using Microsoft.Extensions.Options;

namespace ElevatorSystemAPI.Application.Services
{
    /// <summary>
    /// ElevatorService provides methods to manage elevators, including adding stops, moving elevators, and handling arrivals.
    /// </summary>
    public class ElevatorService : IElevatorService
    {
        private readonly List<Elevator> _elevators;

        /// <summary>
        /// doesn't require an int directly. Instead, inject a configuration object.
        /// </summary>
        /// <param name="options"></param>
        public ElevatorService(IOptions<ElevatorSystemOptions> options)
        {
            var elevatorCount = options.Value.ElevatorCount;
            _elevators = new List<Elevator>();

            for (int i = 1; i <= elevatorCount; i++)
            {
                _elevators.Add(new Elevator { Id = i });
            }
        }

        /// <summary>
        /// Gets all elevators in the system.
        /// </summary>
        /// <returns></returns>
        public List<Elevator> GetAllElevators() => _elevators;

        /// <summary>
        /// Gets an elevator by its ID.
        /// </summary>
        /// <param name="elevatorId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public Elevator GetElevatorById(int elevatorId) =>
            _elevators.FirstOrDefault(e => e.Id == elevatorId)
            ?? throw new KeyNotFoundException("Elevator not found");

        public void AddStop(int elevatorId, int floor)
        {
            var elevator = GetElevatorById(elevatorId);
            elevator.AddStop(floor);
        }

        public void MoveElevator(int elevatorId)
        {
            var elevator = GetElevatorById(elevatorId);
            elevator.MoveOneFloor();
        }

        public void UpdateDirection(int elevatorId)
        {
            var elevator = GetElevatorById(elevatorId);
            elevator.UpdateDirection();
        }

        public void HandleArrival(int elevatorId)
        {
            var elevator = GetElevatorById(elevatorId);
            if (elevator.IsAtNextStop())
            {
                elevator.ArriveAtFloor();
                elevator.UpdateDirection();
            }
        }
    }
}
