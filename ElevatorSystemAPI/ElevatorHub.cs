using Microsoft.AspNetCore.SignalR;
using ElevatorSystemAPI.Domain.Entities;

namespace ElevatorSystemAPI
{
    public class ElevatorHub : Hub
    {
        public async Task SendElevatorUpdate(Elevator elevator)
        {
            await Clients.All.SendAsync("ReceiveElevatorUpdate", elevator);
        }
    }
}
