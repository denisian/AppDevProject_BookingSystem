using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDevProject_BookingSystem
{
    public static class Globals
    {
        // Use for dataGrdViewBooking in ConfigSystem.cs and then in ManageBooking.cs to know which event triggered opening ManageBooking Form
        // If it was clicking on dataGrdViewBooking - open ManageBooking Form with substitution all booking parameters
        // Otherwise, if it was Booking button pressing - open ManageBooking Form with empty parameters
        public static bool ConfigSystemClickEventFlag { get; set; }

        // Use to exchange tableName between ManageBookings.cs and ManageTables.cs to pass selected table name
        public static string TableNameSelected { get; set; }

        // Share list of Table Settings between TablesSettings and ManageTables classes
        public static List<string> listTableSelectedSettings { get; set; }

        // Use in ManageTables.cs and then in TableSettings.cs to know which event triggered opening TableSettings Form (Add or Edit)
        // If it was clicking on the Add Table button - open TableSettings Form with default parameters
        // Otherwise, if it was Edit Table button pressing - open TableSettings Form with substitution Table parameters
        public static bool TableSettingsClickEventFlag { get; set; }

        // Monitor whether there were changes in the settings of the tables (in TableSettings()) to call Retrieveing data from ConfigSystem() class
        public static bool tableSettingsChanged;
    }
}
