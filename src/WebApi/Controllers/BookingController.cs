using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("seat-plan/{busScheduleId}")]
        public async Task<ActionResult<SeatPlanDto>> GetSeatPlan(Guid busScheduleId)
        {
            var seatPlan = await _bookingService.GetSeatPlanAsync(busScheduleId);
            if (seatPlan.Seats.Count == 0)
            {
                return NotFound("No schedule or seats found for this ID.");
            }
            return Ok(seatPlan);
        }

        [HttpPost("book-seat")]
        public async Task<ActionResult<BookSeatResultDto>> BookSeat([FromBody] BookSeatInputDto input)
        {
            var result = await _bookingService.BookSeatAsync(input);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
