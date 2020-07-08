using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SelfCatering.Controllers
{
    [ApiController]
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
    }
}
