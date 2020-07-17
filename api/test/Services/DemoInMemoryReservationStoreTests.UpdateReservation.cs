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
            InitializeTestData();
            var result =  reservationStore.UpdateReservation(new UpdateReservation{ Id = 1, InTime = new DateTime(3000, 1, 1), OutTime = new DateTime(3000, 1, 2) });
            result.ShouldBe(EnumReservationResult.Success);
            var updated = reservationStore.GetReservations().First(x => x.Id == 1);
            updated.InTime.Year.ShouldBe(3000);
            updated.OutTime.Day.ShouldBe(2);
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
            InitializeTestData();
            var result =  reservationStore.UpdateReservation(new UpdateReservation{ Id = 1, InTime = new DateTime(3000, 1, 1), OutTime = new DateTime(2999, 2, 1) });
            result.ShouldBe(EnumReservationResult.BookingInvalid);
        }

        [Fact]
        public void UpdateBooking_BookingConflict_Fail()
        {
            InitializeTestData();
            // conflicts with booking id = 2
            var result = reservationStore.UpdateReservation(new UpdateReservation{ Id = 1, InTime = new DateTime(1990,12,31), OutTime = new DateTime(1991,1,5) });
            result.ShouldBe(EnumReservationResult.BookingConflict);
        }
    }
}