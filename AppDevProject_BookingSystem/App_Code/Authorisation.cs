using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AppDevProject_BookingSystem
{
    /// <summary>
    /// Class for authorisation to the main form for the restaurant staff
    /// (c) Developed by Denis Klyucherov and Yevgeniy Stenyushkin
    /// </summary>
    public class Authorisation
    {
        private string connStr = Properties.Settings.Default.MyConnection;
        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataReader dataReader;
        private DataTable dataTable;

        private static string emailPattern = @"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$";
        private static string passswordPattern = @"^[a-zA-Z0-9_-]{4,50}$";

        Regex emailRgx = new Regex(emailPattern, RegexOptions.IgnoreCase);
        Regex passwordRgx = new Regex(passswordPattern);

        private string _message;

        public string Message { get { return _message; } }

        /// <summary>
        /// INPUT: email and password
        /// OUTPUT: "" - authorisation failed
        ///         Administrator/Manager/Employee - return access level
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Login(string email, string password)
        {
            Match emailMatch = emailRgx.Match(email);
            Match passwordMatch = passwordRgx.Match(password);

            if (!emailMatch.Success && !passwordMatch.Success)
            {
                _message = "Email and Password are not correct";
                return "";
            }
            else if (!emailMatch.Success)
            {
                _message = "Email is incorrect";
                return "";
            }
            else if (!passwordMatch.Success)
            {
                _message = "Password does not meet security requirements";
                return "";
            }

            using (conn = new SqlConnection(connStr))
            {
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select accessLevel, password from Employees e, Permissions p where e.permission_id = p.id and email = @email AND NULLIF(password, '') is not null";
                    cmd.Parameters.AddWithValue("@email", email);

                    try
                    {
                        conn.Open();
                        dataReader = cmd.ExecuteReader();
                        dataTable = new DataTable();
                        if (dataReader.HasRows) // If password has been found
                        {
                            dataTable.Load(dataReader);
                            string pswrd = dataTable.Rows[0]["password"].ToString(); // Taking password
                            if (password == pswrd) // If match
                                return dataTable.Rows[0]["accessLevel"].ToString(); // Return access level
                            else
                            {
                                _message = "Password is incorrect";
                                return "";
                            }
                        }
                        else
                        {
                            _message = "Account is not registered";
                            return "";
                        }
                    }
                    catch (SqlException e)
                    {
                        _message = e.Message;
                        return "";
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
