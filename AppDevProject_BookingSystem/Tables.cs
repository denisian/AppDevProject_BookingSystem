using System.Data;
using System.Data.SqlClient;

namespace AppDevProject_BookingSystem
{
    /// <summary>
    /// Class for managing tables in the Staff account (request, update, delete)
    /// </summary>
    public class Tables
    {
        private string connStr = Properties.Settings.Default.MyConnection;
        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataReader dataReader;
        private DataTable dataTable;
        public DataTable DataTable { get { return dataTable; } }

        public byte maxTableNumSeats { get { return 10; } } // Set up maximum of table number seats
        public byte maxTableMinNumBookingSeats { get { return 10; } } // set up maximum of minimal number seats

        public string name;
        public byte numSeats;
        public byte minNumBookingSeats;
        private string _message;
        public string Message { get { return _message; } }

        /// <summary>
        /// Checking if field Occasion is correct (in the Web Application user can leave the field empty)
        /// </summary>
        /// <returns></returns>
        public string CheckTableInfo()
        {
            if (string.IsNullOrEmpty(name))
            {
                _message = "Table name cannot be empty!";
                return "wrongName";
            }

            if (numSeats < minNumBookingSeats)
            {
                _message = "Seats number must be greater the minimal seats number!";
                return "wrongNumber";
            }

            return "";
        }

        public void ShowTables()
        {
            using (conn = new SqlConnection(connStr))
            {
                conn.Open();
                cmd = new SqlCommand("select id as 'Table ID', name as 'Table name', numSeats as 'Seats number', minNumBookingSeats as 'Minimal seats number' from Tables", conn);
                dataReader = cmd.ExecuteReader();
                dataTable = new DataTable();
                if (dataReader.HasRows)
                    dataTable.Load(dataReader);
                else
                    _message = "No data found";
                conn.Close();
            }
        }

        /// <summary>
        /// Getting Id, numSeats and minNumBookingSeats of a table using it's name
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public void GetTableParameters(string tableName)
        {
            using (conn = new SqlConnection(connStr))
            {
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT id, numSeats, minNumBookingSeats from Tables where name = '" + tableName + "'";

                    try
                    {
                        conn.Open();
                        dataReader = cmd.ExecuteReader();
                        dataTable = new DataTable();
                        if (dataReader.HasRows)
                            dataTable.Load(dataReader);
                        else
                            _message = "No data found";
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
        /// Adding a table.
        /// If table is already exist - return false and do not adding
        /// </summary>
        public bool AddTable()
        {
            using (conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    cmd = new SqlCommand("select count(*) from Tables where name = @name", conn);
                    cmd.Parameters.AddWithValue("@name", name);

                    if ((int)cmd.ExecuteScalar() > 0)
                    {
                        _message = "The Tables name is already exist! Choose different one.";
                        conn.Close();
                        return false;
                    }

                    string addTable = "INSERT INTO Tables (name, numSeats, minNumBookingSeats) " +
                                        "VALUES (@name, @numSeats, @minNumBookingSeats)";

                    cmd = new SqlCommand(addTable, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@numSeats", numSeats);
                    cmd.Parameters.AddWithValue("@minNumBookingSeats", minNumBookingSeats);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        _message = "Table has been added";
                        return true;
                    }
                    else
                    {
                        _message = "Table has NOT been added!";
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
        /// Updating information about tables
        /// Checking if the name is already exist in the database. If exist - do not update
        /// </summary>
        /// <param name="tableId"></param>
        public bool UpdateTable(int tableId)
        {
            using (conn = new SqlConnection(connStr))
            {
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update Tables set name = @name, numSeats = @numSeats, minNumBookingSeats = @minNumBookingSeats where id = @id " +
                                      "AND @name not in (select name from tables where name = @name and id <> @id)";
                    cmd.Parameters.AddWithValue("@id", tableId);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@numSeats", numSeats);
                    cmd.Parameters.AddWithValue("@minNumBookingSeats", minNumBookingSeats);

                    try
                    {
                        conn.Open();
                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            _message = "Table updated succesfully";
                            return true;
                        }
                        else
                        {
                            _message = "The Tables name is already exist! Choose different one.";
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
        /// Deleting tables
        /// </summary>
        /// <param name="tableId"></param>
        public void DeleteTable(int tableId)
        {
            using (conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string deleteTable = "delete from Tables where id = @id";
                    cmd = new SqlCommand(deleteTable, conn);
                    cmd.Parameters.AddWithValue("@id", tableId);

                    if (cmd.ExecuteNonQuery() == 1)
                        _message = "Table has been deleted succesfully";
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