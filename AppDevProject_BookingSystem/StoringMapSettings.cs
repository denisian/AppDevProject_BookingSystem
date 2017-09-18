using System;
using System.Data;
using System.Data.SqlClient;

namespace AppDevProject_BookingSystem
{
    class StoringMapSettings
    {
        private string connStr = Properties.Settings.Default.MyConnection;
        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataReader dataReader;

        private byte[] _tableMapImage;
        public byte[] TableMapImage { get { return _tableMapImage; } }

        private string _tableMapConfig;
        public string TableMapConfig { get { return _tableMapConfig; } }

        private string _message;
        public string Message { get { return _message; } }

        public bool SavingTableMap(byte restaurantId, byte[] tableMapImage, string tableMapConfig)
        {
            using (conn = new SqlConnection(connStr))
            {
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE Restaurant set tableMapImage = @tableMapImage, tableMapConfig = @tableMapConfig where id = @id";
                    cmd.Parameters.AddWithValue("@tableMapImage", tableMapImage);
                    cmd.Parameters.AddWithValue("@tableMapConfig", tableMapConfig);
                    cmd.Parameters.AddWithValue("@id", restaurantId);

                    try
                    {
                        conn.Open();
                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            _message = "Settings have been saved succesfully";
                            return true;
                        }
                        else
                        {
                            _message = "Something went wrong";
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

        public bool RetrievingTableMap(byte restaurantId)
        {

            using (conn = new SqlConnection(connStr))
            {
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT tableMapImage, tableMapConfig from Restaurant where id = @id";
                    cmd.Parameters.AddWithValue("@id", restaurantId);

                    try
                    {
                        conn.Open();
                        dataReader = cmd.ExecuteReader();
                        if (dataReader.Read())
                        {
                            if (!String.IsNullOrEmpty(dataReader.GetValue(0).ToString()))
                                _tableMapImage = (byte[])dataReader.GetValue(0);

                            if (!String.IsNullOrEmpty(dataReader.GetValue(1).ToString()))
                                _tableMapConfig = (string)dataReader.GetValue(1);
                            return true;
                        }
                        else
                        {
                            _message = "No settings in the database";
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
    }
}
