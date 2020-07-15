using System;
using Constants;
using Enums;
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
        public IActionResult BookReservations([FromBody] ReservationCreate reservation) 
        {
            var (result, id) = _reservationStore.BookReservation(reservation);
            return result switch 
            {
                EnumReservationResult.Success => Ok($"Success - reservation id: {id}"),
                EnumReservationResult.MaxBookingExceeded => BadRequest("Failed to make booking - max number of bookings exceeded."),
                EnumReservationResult.BookingConflict => BadRequest("Failed to make booking - booking conflict detected."),
                EnumReservationResult.BookingInvalid => BadRequest("Failed to make booking - booking request invalid."),
                _ => BadRequest("Failed to make booking.")
            };
        }

        [HttpDelete]
        [Route("reservation/{id}")]
        public IActionResult CancelBooking([FromRoute] int id)
        {
            var result = _reservationStore.CancelReservation(id);
            return result switch 
            {
                EnumReservationResult.Success => Ok($"Success - booking with id: {id} has been cancelled."),
                EnumReservationResult.NotFound => BadRequest("No booking found."),
                _ => BadRequest("Failed to cancel booking.")
            };
        }

        [HttpPut]
        [Route("reservation/{id}")]
        public IActionResult UpdateBooking([FromBody] UpdateReservation updateDetails)
        {
            var result = _reservationStore.UpdateReservation(updateDetails);
            return result switch
            {
                EnumReservationResult.Success => Ok($"Success - updated booking with id: {updateDetails.Id}"),
                EnumReservationResult.NotFound => BadRequest("Failed to update booking - booking not found."),
                EnumReservationResult.BookingConflict => BadRequest("Failed to update booking - booking conflict detected."),
                _ => BadRequest("Failed to update booking.")
            };
        }

        [HttpPatch]
        [Authorize("review-policy")]
        [Route("reservation/{id}")]
        public IActionResult ReviewBooking([FromRoute] int id, [FromBody] string review) 
        {
            var result = _reservationStore.AddReview(id, review);
            return result switch 
            {
                EnumReservationResult.Success => Ok("Success - review posted."),
                EnumReservationResult.MaxReviewLengthExceeded => BadRequest($"Review length exceeded max length: {ConstantsReservation.MAX_REVIEW_LENGTH}"),
                EnumReservationResult.NotFound => BadRequest("Booking not found."),
                _ => BadRequest("Failed to post review.")
            };
        }
    }
}
