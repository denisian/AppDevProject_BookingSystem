using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AppDevProject_BookingSystem
{
    /// <summary>
    /// Class for managing customers in the Staff account, Registration and booking tables
    /// (c) Developed by Denis Klyucherov
    /// </summary>
    public class Customers
    {
        private string connStr = Properties.Settings.Default.MyConnection;
        private SqlConnection conn;
        private SqlCommand cmd;

        private static string emailPattern = @"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$";
        private static string phonePattern = @"^\d+$";

        Regex emailRgx = new Regex(emailPattern, RegexOptions.IgnoreCase);
        Regex phoneRgx = new Regex(phonePattern);

        private int _customerId = 0;
        public string email;
        public string password;
        public bool accountActivated;
        public bool typeAccount;
        public string title;
        public string firstName;
        public string lastName;
        public string phone;
        private string _message;

        public string Message { get { return _message; } }
        public int CustomerId { get { return _customerId; } }

        /// <summary>
        /// Checking if fields firstName, Phone and Email are correct
        /// </summary>
        /// <returns></returns>
        public string CheckCustomerInfo()
        {
            Match emailMatch = emailRgx.Match(email);
            Match phoneMatch = phoneRgx.Match(phone);

            if (string.IsNullOrEmpty(firstName.Trim()))
            {
                _message = "First Name is incorrect";
                return "wrongName";
            }

            else if (!phoneMatch.Success)
            {
                _message = "Phone is incorrect";
                return "wrongPhone";
            }

            else if (!string.IsNullOrEmpty(email.Trim()) && !emailMatch.Success)
            {
                _message = "Email is incorrect";
                return "wrongEmail";
            }

            return "";
        }

        /// <summary>
        /// Method for adding customers after booking a table
        /// OUTPUT: false - one of the fields is incorrect
        ///         true - customer has been added
        /// </summary>
        /// <returns></returns>
        public bool AddCustomer()
        {
            using (conn = new SqlConnection(connStr))
            {
                // Check if customer is already in the DB and has type 1 (non-guest)
                // Check if employee is already in the DB
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT " +
                                        "(select count(*) from Customers where email = @email AND typeAccount = 1) + " +
                                        "(select count(*) from Employees where email = @email) count";
                    cmd.Parameters.AddWithValue("@email", email);

                    try
                    {
                        conn.Open();
                        if ((int)cmd.ExecuteScalar() > 0)
                        {
                            _message = "E-mail is already exist";
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

                // Add customer
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO Customers (email, password, accountActivated, typeAccount, title, firstName, lastName, phone) " +
                                            "OUTPUT INSERTED.id " +
                                            "VALUES (@email, @password, @accountActivated, @typeAccount, @title, @firstName, @lastName, @phone)";

                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@accountActivated", accountActivated);
                    cmd.Parameters.AddWithValue("@typeAccount", typeAccount);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@firstName", firstName);
                    cmd.Parameters.AddWithValue("@lastName", lastName);
                    cmd.Parameters.AddWithValue("@phone", phone);

                    try
                    {
                        conn.Open();
                        _customerId = (int)cmd.ExecuteScalar();

                        if (_customerId != 0)
                        {
                            _message = "Customer has been added";
                            return true;
                        }
                        else
                        {
                            _message = "Customer has not been added!";
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
        /// Updating information about customer
        /// </summary>
        /// <param name="customerId"></param>
        public void UpdateCustomer(int customerId)
        {
            using (conn = new SqlConnection(connStr))
            {
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE Customers SET title = @title, firstName = @firstName, lastName = @lastName, email = @email, phone = @phone WHERE id = @customerId";

                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@firstName", firstName);
                    cmd.Parameters.AddWithValue("@lastName", lastName);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@phone", phone);

                    try
                    {
                        conn.Open();
                        if (cmd.ExecuteNonQuery() == 1)
                            _message = "Customer has been updated succesfully";
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
        /// Deleting customers
        /// </summary>
        /// <param name="customerId"></param>
        public void DeleteCustomer(int customerId)
        {
            using (conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string deleteCustomer = "delete from Customers where id = @id";
                    cmd = new SqlCommand(deleteCustomer, conn);
                    cmd.Parameters.AddWithValue("@id", customerId);

                    if (cmd.ExecuteNonQuery() == 1)
                        _message = "Customer has been deleted succesfully";
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
