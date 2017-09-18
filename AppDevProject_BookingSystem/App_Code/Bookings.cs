using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace AppDevProject_BookingSystem
{
    /// <summary>
    /// Class contains methods for checking availability tables and managing bookings (add, update, delete, get info)
    /// (c) Developed by Denis Klyucherov
    /// </summary>
    public class Bookings
    {
        private string connStr = Properties.Settings.Default.MyConnection;
        private SqlDataReader dataReader;
        private DataTable _dataTable;
        private SqlConnection conn;
        private SqlCommand cmd;

        private string _message;
        public byte restaurantId;
        public int customerId;
        public DateTime bookingDateTime; // '23/06/2017 14:00'
        public string partySize;
        public int idChosenTable;
        public string nameChosenTable;
        public string occasion;
        public string notes;

        public string Message { get { return _message; } }
        public DataTable Datatable { get { return _dataTable; } }

        /// <summary>
        /// Creation Time list for bookings
        /// </summary>
        public List<string> ListTimeBooking()
        {
            List<string> timeList = new List<string>();
            for (int time = 10; time < 23; time++)
                timeList.Add(time.ToString() + ":00");
            return timeList;
        }

        /// <summary>
        /// Creation Party Size list for bookings
        /// </summary>
        public List<string> ListPartySize()
        {
            List<string> partySizeList = new List<string>();
            for (int group = 1; group <= 9; group++)
                partySizeList.Add(group.ToString());
            return partySizeList;
        }

        /// <summary>
        /// Creation Occasion list for bookings
        /// </summary>
        public List<string> OccasionList()
        {
            List<string> occasionList = new List<string>();
            string[] occasion = new string[] { "Anniversary", "Birthday", "Honeymoon", "Wedding", "Meeting", "Other" };
            foreach (string item in occasion)
                occasionList.Add(item);
            return occasionList;
        }

        /// <summary>
        /// Checking if field Occasion is correct (in the Web Application user can leave the field empty)
        /// </summary>
        /// <returns></returns>
        public bool CheckBookingInfo()
        {
            if (string.IsNullOrEmpty(occasion))
            {
                _message = "Occasion is incorrect";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Method for getting tableName by its Id
        /// Use in CheckingAvailability method
        /// </summary>
        /// <param name="dtSourceTable"></param>
        /// <param name="tableId"></param>
        /// <returns>queryTableName.First</returns>
        public string GetTableNameById(DataTable dtSourceTable, int tableId)
        {
            var queryTableName =
                from table in dtSourceTable.AsEnumerable()
                where table.Field<int>("id") == tableId
                select table.Field<string>("name");

            return queryTableName.First();
        }

        /// <summary>
        /// Getting booking information
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        public void LoadBookingInfo(string date, string time)
        {
            // Request data at certain date (as long as date keeps in the table Bookings as 'DATETIME' we need to get rid of time and leave just date - converting to VARCHAR like '20/06/2017')
            if (!string.IsNullOrEmpty(date) && string.IsNullOrEmpty(time)) // Passing just data without time
                date = " AND CONVERT(VARCHAR(10), CONVERT(DATE, bookingDate), 103) = '" + date + "'";
            else if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(time)) // Passing date and time
                date = " AND CONVERT(VARCHAR(10), CONVERT(DATETIME, bookingDate), 103) = '" + date + "' AND CONVERT(VARCHAR(5), CONVERT(DATETIME, bookingDate), 108) = '" + time + "'";

            using (conn = new SqlConnection(connStr))
            {
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT b.id as 'Booking ID', c.id as 'Customer ID', t.id as 'Table ID', b.bookingDate as 'Booking Date', b.partySize as 'Party Size', t.name as 'Table Name', c.title as 'Title', c.firstName as 'First Name', c.lastName as 'Last Name', c.Email, c.Phone, b.Occasion, b.Notes " +
                                        "FROM Customers c, Bookings b, Tables t " +
                                        "WHERE b.customer_id = c.id AND b.table_id = t.id" + date +
                                        " ORDER BY b.bookingDate";

                    try
                    {
                        conn.Open();
                        dataReader = cmd.ExecuteReader();
                        _dataTable = new DataTable();
                        if (dataReader.HasRows)
                            _dataTable.Load(dataReader);
                        else
                            _message = "You do not have any booking";
                    }
                    catch (SqlException e)
                    {
                        _message = e.Message;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method for checking availability tables
        /// INPUT DATA: bookingDate, partySize
        /// OUTPUT DATA: idChosenTable, nameChosenTable, message
        /// </summary>
        public Tuple<int, string, string> CheckingAvailability(string bkDate, string prtSize)
        {
            // Check if Party Size is not empty
            if (string.IsNullOrWhiteSpace(prtSize))
                return Tuple.Create(0, "", "Date or Party Size cannot be empty");

            using (conn = new SqlConnection(connStr))
            {
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    // Check if table is available (it's Id is not in the table Bookings) and Party Size less or equal number of seats and also more or equal minimal number of seats, which set up by emloyee
                    cmd.CommandText = "SELECT id, name, numSeats FROM Tables WHERE " +
                                            "id NOT IN (SELECT table_id FROM Bookings WHERE bookingDate = CONVERT(datetime, @bookingDate, 103)) " +
                                            "AND @partySize BETWEEN minNumBookingSeats AND numSeats";

                    cmd.Parameters.AddWithValue("@bookingDate", bkDate);
                    cmd.Parameters.AddWithValue("@partySize", prtSize);

                    try
                    {
                        conn.Open();
                        dataReader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        if (dataReader.HasRows)
                        {
                            _message = "Table is available for booking";
                            dt.Load(dataReader);

                            // If there is a just one table available for booking, return it ID
                            if (dt.Rows.Count == 1)
                            {
                                idChosenTable = Convert.ToInt32(dt.Rows[0]["id"]);
                                nameChosenTable = dt.Rows[0]["name"].ToString();
                                return Tuple.Create(idChosenTable, nameChosenTable, _message);
                            }

                            // If there are more than one table for booking, firstly we choose a table(s) with minimal number of seats
                            // For that we find out the minimal number of seats among table(s) in dataTable
                            byte minNumSeats = byte.MaxValue;
                            foreach (DataRow dataRow in dt.Rows)
                            {
                                byte numSeats = dataRow.Field<byte>("numSeats");
                                minNumSeats = Math.Min(minNumSeats, numSeats);
                            }

                            // After we found the minimal number of seats, we get array of ID(s) of table(s), because it can be more than one table with thу same minimal number of seats
                            var queryIdTablesWithMinNumSeats =
                                 from table in dt.AsEnumerable()
                                 where table.Field<byte>("numSeats") == minNumSeats
                                 select table.Field<int>("id");

                            // Determing how many tables with the minimal number of seats
                            int countSeats = queryIdTablesWithMinNumSeats.Count();

                            // If there is a just one table, return it ID
                            if (countSeats == 1)
                            {
                                idChosenTable = queryIdTablesWithMinNumSeats.First();
                                nameChosenTable = GetTableNameById(dt, idChosenTable);
                                return Tuple.Create(idChosenTable, nameChosenTable, _message);
                            }

                            // If there is more than one table with minimal number of seats, we randomly choose one of them
                            // Create array for these tables for using it in Random class
                            int[] idArray = new int[countSeats];
                            int i = 0;
                            foreach (var id in queryIdTablesWithMinNumSeats)
                            {
                                idArray[i] = id;
                                i++;
                            }

                            // Ramdomly choose the table
                            Random randomTable = new Random();
                            int index = randomTable.Next(0, countSeats);
                            idChosenTable = idArray[index];
                            nameChosenTable = GetTableNameById(dt, idChosenTable);

                            return Tuple.Create(idChosenTable, nameChosenTable, _message);
                        }
                        else
                            return Tuple.Create(0, "", "There is no any table for booking at this time");
                    }
                    catch (SqlException e)
                    {
                        return Tuple.Create(0, "", e.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method for adding bookings
        /// If booking is already exist - return false and do not adding
        /// </summary>
        /// <returns></returns>
        public bool AddBooking()
        {
            using (conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    cmd = new SqlCommand("select count(*) from Bookings where table_id = @table_id and bookingDate = @bookingDate", conn);

                    cmd.Parameters.AddWithValue("@table_id", idChosenTable);
                    cmd.Parameters.AddWithValue("@bookingDate", bookingDateTime);

                    if ((int)cmd.ExecuteScalar() > 0)
                    {
                        _message = "Selected table is already booked at this time. Choose different one.";
                        conn.Close();
                        return false;
                    }

                    cmd = new SqlCommand("INSERT INTO Bookings (restaurant_id, customer_id, table_id, bookingDate, partySize, occasion, notes) " +
                                         "VALUES (@restaurant_id, @cudtomer_id, @table_id, CONVERT(DATETIME, @bookingDate, 103), @partySize, @occasion, @notes)", conn);

                    cmd.Parameters.AddWithValue("@restaurant_id", restaurantId);
                    cmd.Parameters.AddWithValue("@cudtomer_id", customerId);
                    cmd.Parameters.AddWithValue("@table_id", idChosenTable);
                    cmd.Parameters.AddWithValue("@bookingDate", bookingDateTime);
                    cmd.Parameters.AddWithValue("@partySize", partySize);
                    cmd.Parameters.AddWithValue("@occasion", occasion);
                    cmd.Parameters.AddWithValue("@notes", notes);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        _message = "Booking has been added!";
                        return true;
                    }
                    else
                    {
                        _message = "Booking has NOT been added!";
                        return false;
                    }
                }
                catch (SqlException e)
                {
                    _message = e.Message;
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Updating booking information
        /// Checking if the table name is already exist in the database at selected date and time. If exist - do not update
        /// </summary>
        /// <param name="date"></param>
        public bool UpdateBooking(int bookingId)
        {
            using (conn = new SqlConnection(connStr))
            {
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "declare @varBookingDate DATETIME;" +
                                      "set @varBookingDate = CONVERT(DATETIME, @bookingDate, 103);" +
                                      "UPDATE Bookings SET table_id = @tableID, bookingDate = @varBookingDate, partySize = @partySize, occasion = @occasion, notes = @notes WHERE id = @bookingId " +
                                      "AND @tableID not in (select table_id from Bookings where bookingDate = @varBookingDate and id <> @bookingId)";

                    cmd.Parameters.AddWithValue("@bookingId", bookingId);
                    cmd.Parameters.AddWithValue("@tableID", idChosenTable);
                    cmd.Parameters.AddWithValue("@bookingDate", bookingDateTime);
                    cmd.Parameters.AddWithValue("@partySize", partySize);
                    cmd.Parameters.AddWithValue("@occasion", occasion);
                    cmd.Parameters.AddWithValue("@notes", notes);

                    try
                    {
                        conn.Open();
                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            _message = "Booking table has been updated succesfully!";
                            return true;
                        }
                        else
                        {
                            _message = "Selected table is already booked at this time. Choose different one.";
                            return false;
                        }
                    }
                    catch (SqlException e)
                    {
                        _message = e.Message;
                        return false;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method for deleting bookings
        /// </summary>
        /// <param name="bookingId"></param>
        public void DeleteBooking(int bookingId)
        {
            using (conn = new SqlConnection(connStr))
            {
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM Bookings WHERE id = @bookingId";

                    cmd.Parameters.AddWithValue("@bookingId", bookingId);

                    try
                    {
                        conn.Open();
                        if (cmd.ExecuteNonQuery() == 1)
                            _message = "Booking has been deleted succesfully";
                    }
                    catch (SqlException e)
                    {
                        _message = e.Message;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}