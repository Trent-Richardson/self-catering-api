using Enums;
using Xunit;
using Shouldly;
using SelfCatering;
using System;
using System.Linq;

namespace Services
{
    /// <summary>
    /// update booking - success - updated
    /// booking not found - fail
    /// booking conflict - fail
    /// </summary>
    public class DemoInMemoryReservationStoreTestsUpdateReservation : DemoInMemoryReservationStoreTestsBase
    {
        [Fact]
        public void UpdateBooking_BookingFound_Success()
        {
            var result =  reservationStore.UpdateReservation(new UpdateReservation{ Id = 1, InTime = new DateTime(3000, 1, 1), OutTime = new DateTime(3000, 2, 1) });
            result.ShouldBe(EnumReservationResult.Success);
            var updated = reservationStore.GetReservations().First(x => x.Id == 1);
            updated.InTime.Year.ShouldBe(3000);
            updated.OutTime.Year.ShouldBe(3000);
        }

        [Fact]
        public void UpdateBooking_BookingNotFound_Fail()
        {
            var result = reservationStore.UpdateReservation(new UpdateReservation{ Id = 99, InTime = new DateTime(3000, 1, 1), OutTime = new DateTime(3000, 2, 1) });
            result.ShouldBe(EnumReservationResult.NotFound);
        }

        [Fact]
        public void UpdateBooking_BookingInvalid_Fail()
        {
            var result =  reservationStore.UpdateReservation(new UpdateReservation{ Id = 1, InTime = new DateTime(3000, 1, 1), OutTime = new DateTime(2999, 2, 1) });
            result.ShouldBe(EnumReservationResult.BookingInvalid);
        }

        [Fact]
        public void UpdateBooking_BookingConflict_Fail()
        {
            // conflicts with booking id = 2
            var result = reservationStore.UpdateReservation(new UpdateReservation{ Id = 1, InTime = new DateTime(1990,1,1), OutTime = new DateTime(1992,1,1) });
            result.ShouldBe(EnumReservationResult.BookingConflict);
        }
    }
}