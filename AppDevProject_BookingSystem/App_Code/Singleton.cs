using System.Collections.Generic;

namespace AppDevProject_BookingSystem
{
    /// <summary>
    /// Singleton pattern to share the same instance of Bookings and Tables classes between other classes
    /// For example, I use the same instance of the Bookings class to share the same Datatable between ConfigSystem and ManageBookings classes
    /// (c) Developed by Denis Klyucherov
    /// </summary>
    public class Singleton
    {
        private static Singleton instance;
        
        // Share instance of the Bookings class - booking
        public Bookings booking { get; set; }

        // Share list of Booking Details between ConfigSystem and ManageBookings classes
        public List<string> listBookingSelectedDetails { get; set; }

        // Constructor to initiate a new instance
        private Singleton()
        {
            booking = new Bookings();
        }

        public static Singleton GetInstance()
        {
            if (instance == null)
            {
                instance = new Singleton();
            }
            return instance;
        }
    }
}
