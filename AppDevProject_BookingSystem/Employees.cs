using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AppDevProject_BookingSystem
{
    class Employees
    {
        private string connStr = Properties.Settings.Default.MyConnection;
        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataReader dataReader;
        private DataTable dataTable;
        public DataTable DataTable { get { return dataTable; } }

        private static string emailPattern = @"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$";
        private static string passswordPattern = @"^[a-zA-Z0-9_-]{4,50}$";

        Regex emailRgx = new Regex(emailPattern, RegexOptions.IgnoreCase);
        Regex passwordRgx = new Regex(passswordPattern);

        public byte restaurantId;
        public string email;
        public string password;
        public string firstName;
        public string lastName;
        public string accessLevel;
        private string _message;
        public string Message { get { return _message; } }

        /// <summary>
        /// Creation Access level list
        /// </summary>
        //public List<string> OccasionList()
        //{
        //    List<string> accessLevelList = new List<string>();
        //    string[] levels = new string[] { "Administrator", "Manager", "Employee" };
        //    foreach (string item in levels)
        //        accessLevelList.Add(item);
        //    return accessLevelList;
        //}

        // Get First and Last name by email (use it in ConfigSystem to find out a logged-in user)
        public void GetEmployeeFirstLastNameByEmail(string email)
        {
            using (conn = new SqlConnection(connStr))
            {
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT firstName, lastName from Employees where email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

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
        /// Checking if fields firstName, lastName, Email and Password are correct
        /// </summary>
        /// <returns></returns>
        public string CheckEmployeeInfo()
        {
            Match emailMatch = emailRgx.Match(email);
            Match passwordMatch = passwordRgx.Match(password);

            if (string.IsNullOrEmpty(firstName.Trim()))
            {
                _message = "First name is incorrect";
                return "wrongFirstName";
            }

            else if (string.IsNullOrEmpty(lastName.Trim()))
            {
                _message = "Last name is incorrect";
                return "wrongLastName";
            }

            else if (!emailMatch.Success)
            {
                _message = "Email is incorrect";
                return "wrongEmail";
            }

            else if (!passwordMatch.Success)
            {
                _message = "Password does not meet security requirements";
                return "wrongPassword";
            }

            return "";
        }

        /// <summary>
        /// Getting Employees information
        /// </summary>
        public void ShowEmployees()
        {
            using (conn = new SqlConnection(connStr))
            {
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT e.id as 'ID', email as 'Email', firstName as 'First name', lastName as 'Last name', password, accessLevel as 'Access level' from Employees e, Permissions p where e.permission_id = p.id";

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
        /// Getting Employees access levels
        /// </summary>
        public void GetAccessLevels()
        {
            using (conn = new SqlConnection(connStr))
            {
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT accessLevel from Permissions";

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
        /// Adding an employee
        /// If the employee is already exist - return false and do not adding
        /// </summary>
        public bool AddEmployee()
        {
            using (conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    cmd = new SqlCommand("select count(*) from Employees where email = @email", conn);
                    cmd.Parameters.AddWithValue("@email", email);

                    if ((int)cmd.ExecuteScalar() > 0)
                    {
                        _message = "Email is already exist! Choose different one.";
                        conn.Close();
                        return false;
                    }

                    string addEmployee = "declare @permissionId tinyint;" +
                                         "set @permissionId = (select id from Permissions where accessLevel = @accessLevel);" +
                                         "INSERT INTO Employees(restaurant_id, permission_id, email, password, firstName, lastName) " +
                                         "VALUES (@restaurantId, @permissionId, @email, @password, @firstName, @lastName)";

                    cmd = new SqlCommand(addEmployee, conn);
                    cmd.Parameters.AddWithValue("@accessLevel", accessLevel);
                    cmd.Parameters.AddWithValue("@restaurantId", restaurantId);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@firstName", firstName);
                    cmd.Parameters.AddWithValue("@lastName", lastName);


                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        _message = "Employee has been added!";
                        return true;
                    }
                    else
                    {
                        _message = "Employee has NOT been added!";
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
        /// Updating employees information
        /// Checking if an email is already exist in the database. If exist - do not update
        /// </summary>
        /// <param name="employeeId"></param>
        public bool UpdateEmployee(int employeeId)
        {
            using (conn = new SqlConnection(connStr))
            {
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "declare @permissionId tinyint;" +
                                      "set @permissionId = (select id from Permissions where accessLevel = @accessLevel);" +
                                      "update Employees set permission_id = @permissionId, email = @email, password = @password, firstName = @firstName, lastName = @lastName where id = @id " +
                                      "AND @email not in (select email from Employees where email = @email and id <> @id);";
                    cmd.Parameters.AddWithValue("@accessLevel", accessLevel);
                    cmd.Parameters.AddWithValue("@id", employeeId);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@firstName", firstName);
                    cmd.Parameters.AddWithValue("@lastName", lastName);

                    try
                    {
                        conn.Open();
                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            _message = "Employee has been updated succesfully!";
                            return true;
                        }
                        else
                        {
                            _message = "Email is already exist! Choose different one.";
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
        /// Deleting employee
        /// </summary>
        /// <param name="employeeId"></param>
        public void DeleteEmployee(int employeeId)
        {
            using (conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string deleteEmployee = "delete from Employees where id = @id";
                    cmd = new SqlCommand(deleteEmployee, conn);
                    cmd.Parameters.AddWithValue("@id", employeeId);

                    if (cmd.ExecuteNonQuery() == 1)
                        _message = "Employee has been deleted succesfully!";
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
