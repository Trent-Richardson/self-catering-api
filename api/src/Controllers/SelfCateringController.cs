using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SelfCatering.Controllers
{
    [ApiController]
    [Authorize("self-catering-policy")]
    [Route("api/[controller]")]
    public class SelfCateringController : ControllerBase
    {
        private readonly ILogger<SelfCateringController> _logger;
        private IReservationStore _reservationStore;

        public SelfCateringController(ILogger<SelfCateringController> logger, IReservationStore reservationStore)
        {
            _logger = logger;
            _reservationStore = reservationStore;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("reservation/list")]
        public IActionResult GetReservations() 
        {
            var reservations = _reservationStore.GetReservations();
            return Ok(reservations);
        }

        [HttpPost]
        [Route("reservation")]
        public IActionResult BookReservations([FromBody] Reservation reservation) 
        {
            var id = _reservationStore.BookReservation(reservation);
            if(id == null)
                return BadRequest("Sorry, unable to make booking."); 
            return Ok($"Reservation id: {id}");
        }

        [HttpDelete]
        [Route("reservation/{id}")]
        public IActionResult CancelBooking([FromRoute] int id)
        {
            var result = _reservationStore.CancelReservation(id);
            if(result)
                return Ok($"Booking with id: {id} has been cancelled.");
            else
                return BadRequest("No booking found.");
        }

        [HttpPut]
        [Route("reservation/{id}")]
        public IActionResult UpdateBooking([FromBody] UpdateReservation updateDetails)
        {
            var result = _reservationStore.UpdateReservation(updateDetails);
            if(result)
                return Ok($"Updated booking with id: {updateDetails.Id}");
            else
                return BadRequest("Failed to update booking.");
        }

        [HttpPatch]
        [Authorize("review-policy")]
        [Route("reservation/{id}")]
        public IActionResult ReviewBooking([FromRoute] int id, [FromBody] string review) 
        {
            var result = _reservationStore.AddReview(id, review);
            if(result)
                return Ok("Review posted.");
            else
                return BadRequest("Failed to post review.");
        }
    }
}
