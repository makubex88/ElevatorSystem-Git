using ElevatorSystemAPI.Domain.Entities;

namespace ElevatorSystemAPI.Application.Interfaces
{
    public interface IElevatorScheduler
    {
        int RequestElevator(ElevatorRequest request);
        Task SimulateAsync(CancellationToken cancellationToken);
    }
}
