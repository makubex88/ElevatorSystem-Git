namespace ElevatorSystemAPI.Api.Models
{
    public class RequestElevatorDto
    {
        public int Floor { get; set; }
        public string Direction { get; set; } // "Up" or "Down"
    }
}
