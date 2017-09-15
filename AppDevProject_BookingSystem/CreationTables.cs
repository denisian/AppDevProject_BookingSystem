

namespace AppDevProject_BookingSystem
{
    public class CreationTables
    {
        //string crtTableCustomers = "CREATE TABLE [dbo].[Customers] (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, email VARCHAR(50), password VARCHAR(10), accountActivated BIT, typeAccount BIT, title VARCHAR(5), firstName VARCHAR(20) NOT NULL, lastName VARCHAR(20) NOT NULL, phone VARCHAR(15))";
        //string crtTablePermissons = "CREATE TABLE [dbo].[Permissions] (id TINYINT IDENTITY(1,1) NOT NULL PRIMARY KEY, accessLevel VARCHAR(20) NOT NULL)";
        //string crtTableEmployees = "CREATE TABLE [dbo].[Employees] (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, restaurant_id TINYINT FOREIGN KEY REFERENCES [Restaurant](id) NOT NULL, permission_id TINYINT FOREIGN KEY REFERENCES [Permissions](id) NOT NULL, email VARCHAR(50) NOT NULL, password VARCHAR(10) NOT NULL, firstName VARCHAR(20) NOT NULL, lastName VARCHAR(20) NOT NULL)";
        //string crtTableRestaurant = "CREATE TABLE [dbo].[Restaurant] (id TINYINT IDENTITY(1,1) NOT NULL PRIMARY KEY, name VARCHAR(20) NOT NULL, location VARCHAR(50), tableMapImage VARBINARY(max), tableMapConfig VARCHAR(max))";
        //string crtTableTables = "CREATE TABLE [dbo].[Tables] (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, name VARCHAR(10) NOT NULL, numSeats TINYINT NOT NULL, minNumBookingSeats TINYINT NOT NULL)";
        //string crtTableBookings = "CREATE TABLE [dbo].[Bookings] (id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, restaurant_id TINYINT FOREIGN KEY REFERENCES [Restaurant](id) ON UPDATE CASCADE NOT NULL, customer_id INT FOREIGN KEY REFERENCES [Customers](id) ON UPDATE CASCADE NOT NULL, table_id INT FOREIGN KEY REFERENCES [Tables](id) ON UPDATE CASCADE NOT NULL, bookingDate DATETIME NOT NULL, partySize TINYINT NOT NULL, occasion VARCHAR(20), notes VARCHAR(100))";
        //string crtTableUsersActivation = "CREATE TABLE [dbo].[UsersActivation] (user_id INT FOREIGN KEY REFERENCES [Customers](id) NOT NULL, activationCode UNIQUEIDENTIFIER NOT NULL)";
    }
}