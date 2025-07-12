using ElevatorSystemAPI.Application.Interfaces;
using ElevatorSystemAPI.Domain.Entities;
using ElevatorSystemAPI.Domain.Enums;
using ElevatorSystemAPI.Domain.ValueObjects;

namespace ElevatorSystemAPI.Application.Services
{
    /// <summary>
    /// ElevatorScheduler is responsible for managing elevator requests and simulating the elevator system.
    /// </summary>
    public class ElevatorScheduler : IElevatorScheduler
    {
        private readonly IElevatorService _elevatorService;

        public ElevatorScheduler(IElevatorService elevatorService)
        {
            _elevatorService = elevatorService;
        }

        /// <summary>
        /// Registers a new elevator request and assigns an elevator to handle it.
        /// </summary>
        /// <param name="request"></param>
        public int RequestElevator(ElevatorRequest request)
        {
            var elevators = _elevatorService.GetAllElevators();

            // Very naive: pick the first idle or closest elevator
            var selected = elevators
                .Where(e => e.Direction == Direction.Idle)
                .OrderBy(e => Math.Abs(e.CurrentFloor - request.RequestedFloor))
                .FirstOrDefault();

            selected ??= elevators
                .OrderBy(e => Math.Abs(e.CurrentFloor - request.RequestedFloor))
                .First();

            _elevatorService.AddStop(selected.Id, request.RequestedFloor);

            Console.WriteLine($"[Request] Floor {request.RequestedFloor} requesting {request.Direction}, Elevator {selected.Id} assigned");

            return selected.Id;
        }

        /// <summary>
        /// Simulates the elevator system by moving elevators and handling stops.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task SimulateAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var elevators = _elevatorService.GetAllElevators();

                foreach (var elevator in elevators)
                {
                    if (!elevator.IsMoving && elevator.StopsQueue.Count == 0)
                        continue;

                    _elevatorService.UpdateDirection(elevator.Id);

                    // Simulate movement
                    if (elevator.Direction != Direction.Idle)
                    {
                        await Task.Delay(Constants.TravelTimeSeconds * 1000, cancellationToken);
                        _elevatorService.MoveElevator(elevator.Id);
                        Console.WriteLine($"[Move] Elevator {elevator.Id} moved to floor {elevator.CurrentFloor}");
                    }

                    if (elevator.IsAtNextStop())
                    {
                        Console.WriteLine($"[Stop] Elevator {elevator.Id} stopped at floor {elevator.CurrentFloor}");
                        await Task.Delay(Constants.LoadUnloadTimeSeconds * 1000, cancellationToken);
                        _elevatorService.HandleArrival(elevator.Id);
                    }
                }
            }
        }
    }
}
