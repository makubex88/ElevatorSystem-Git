using ElevatorSystemAPI.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

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
        /// ExecuteAsync is called by the host to start the background service.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SimulationBackgroundService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var elevatorService = scope.ServiceProvider.GetRequiredService<IElevatorService>();
                    var scheduler = scope.ServiceProvider.GetRequiredService<IElevatorScheduler>();

                    // Process elevator requests
                    foreach (var elevator in elevatorService.GetAllElevators())
                    {
                        if (elevator.StopsQueue.Count > 0)
                        {
                            _logger.LogInformation("Elevator {ElevatorId} currently at floor {CurrentFloor} with direction {Direction}.",
                                elevator.Id, elevator.CurrentFloor, elevator.Direction);

                            elevatorService.UpdateDirection(elevator.Id);
                            elevatorService.MoveElevator(elevator.Id);

                            if (elevator.StopsQueue.Contains(elevator.CurrentFloor))
                            {
                                elevatorService.HandleArrival(elevator.Id);
                                _logger.LogInformation("Elevator {ElevatorId} stopped at floor {CurrentFloor} to load/unload passengers.", elevator.Id, elevator.CurrentFloor);
                            }
                        }

                        // Push update via SignalR
                        await _hubContext.Clients.All.SendAsync("ReceiveElevatorUpdate", elevator);
                    }
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("SimulationBackgroundService stopping.");
        }
    }
}
