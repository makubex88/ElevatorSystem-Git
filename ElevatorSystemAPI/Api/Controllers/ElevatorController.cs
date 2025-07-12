using ElevatorSystemAPI.Api.Models;
using ElevatorSystemAPI.Application.Interfaces;
using ElevatorSystemAPI.Domain.Entities;
using ElevatorSystemAPI.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElevatorSystemAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElevatorController : ControllerBase
    {
        private readonly IElevatorService _elevatorService;
        private readonly IElevatorScheduler _scheduler;

        public ElevatorController(IElevatorService elevatorService, IElevatorScheduler scheduler)
        {
            _elevatorService = elevatorService;
            _scheduler = scheduler;
        }

        /// <summary>
        /// Get All Elevator information.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllElevators()
        {
            var elevators = _elevatorService.GetAllElevators();
            return Ok(elevators);
        }

        /// <summary>
        /// Get elevator information by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetElevatorById(int id)
        {
            var elevator = _elevatorService.GetElevatorById(id);
            if (elevator == null)
                return NotFound();
            return Ok(elevator);
        }

        /// <summary>
        /// Request Elevator 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("request")]
        public IActionResult RequestElevator([FromBody] RequestElevatorDto dto)
        {
            if (!Enum.TryParse<Direction>(dto.Direction, true, out var direction))
                return BadRequest("Invalid direction.");

            var request = new ElevatorRequest(dto.Floor, direction);
            var assignedElevatorId = _scheduler.RequestElevator(request);

            return Ok(new { message = "Request accepted.", elevatorId = assignedElevatorId });
        }

        /// <summary>
        /// Steps Simulation
        /// </summary>  
        /// <returns></returns>
        [HttpPost("step")]
        public IActionResult SimulateStep()
        {
            foreach (var elevator in _elevatorService.GetAllElevators())
            {
                if (elevator.StopsQueue.Count > 0)
                {
                    _elevatorService.UpdateDirection(elevator.Id);
                    _elevatorService.MoveElevator(elevator.Id);

                    if (elevator.StopsQueue.Contains(elevator.CurrentFloor))
                        _elevatorService.HandleArrival(elevator.Id);
                }
            }

            return Ok(_elevatorService.GetAllElevators());
        }
    }
}
