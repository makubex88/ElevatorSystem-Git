using ElevatorSystemAPI.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using System.Diagnostics;

namespace ElevatorSystemAPI.Application.Services
{
    public class SimulationBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SimulationBackgroundService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(10);
        private readonly IHubContext<ElevatorHub> _hubContext;

        /// <summary>
        /// Use a IServiceScopeFactory to Create a Scope
        /// clean way to resolve scoped services from a singleton background service.
        /// </summary>
        /// <param name="scopeFactory"></param>
        public SimulationBackgroundService(IServiceScopeFactory scopeFactory, ILogger<SimulationBackgroundService> logger, IHubContext<ElevatorHub> hubContext)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Background service to simulate elevator movement and handling
        /// ExecuteAsync is called by the host to start the background service.
        /// Moves elevators one floor per interval.
        /// Processes stops and arrival logic.
        /// Pushes updates to the frontend via SignalR.
        /// Uses dependency injection(IServiceScopeFactory) to resolve services cleanly.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SimulationBackgroundService started.");

            // Run continuously until cancellation is requested
            while (!stoppingToken.IsCancellationRequested)
            {
                // Create a scoped service provider to access scoped dependencies
                using (var scope = _scopeFactory.CreateScope())
                {
                    // Resolve elevator service and scheduler from DI
                    var elevatorService = scope.ServiceProvider.GetRequiredService<IElevatorService>();
                    var scheduler = scope.ServiceProvider.GetRequiredService<IElevatorScheduler>();

                    // Loop through all elevators in the system
                    foreach (var elevator in elevatorService.GetAllElevators())
                    {
                        // Process only if there are pending stops
                        if (elevator.StopsQueue.Count > 0)
                        {
                            // Log current state of the elevator
                            _logger.LogInformation("Elevator {ElevatorId} currently at floor {CurrentFloor} with direction {Direction}.",
                                elevator.Id, elevator.CurrentFloor, elevator.Direction);

                            // Update elevator direction (e.g., UP/DOWN/IDLE) based on target floors
                            elevatorService.UpdateDirection(elevator.Id);

                            // Move elevator one floor based on its direction
                            elevatorService.MoveElevator(elevator.Id);

                            // If elevator reached a target floor, handle arrival
                            if (elevator.StopsQueue.Contains(elevator.CurrentFloor))
                            {
                                // Remove stop and simulate load/unload delay
                                elevatorService.HandleArrival(elevator.Id);

                                _logger.LogInformation("Elevator {ElevatorId} stopped at floor {CurrentFloor} to load/unload passengers.",
                                    elevator.Id, elevator.CurrentFloor);
                            }
                        }

                        // Broadcast elevator state to connected clients (e.g., frontend dashboard) using SignalR
                        await _hubContext.Clients.All.SendAsync("ReceiveElevatorUpdate", elevator);
                    }
                }

                // Wait before next simulation tick (interval defined in options)
                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("SimulationBackgroundService stopping.");
        }
    }
}
