using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaRottaO.CSharp.MySqlUtilities
{
    public class Truncate
    {
        public Task<Tuple<Boolean, String, int>> truncate(string argConnString, String argQuery, int argTimeOutMs)
        {
            return Task.Run(() =>
            {
                MySqlConnection mySqlConnection = new MySqlConnection(argConnString);

                MySqlDataReader mySqlDataReader = null;

                try
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand(argQuery, mySqlConnection);

                    if (argTimeOutMs != -1)
                    {
                        mySqlCommand.CommandTimeout = argTimeOutMs;
                    }

                    int truncateResult = mySqlCommand.ExecuteNonQuery();

                    return new Tuple<Boolean, String, int>(true, Constants.MYSQL_SUCCESS, truncateResult);
                }
                catch (Exception ex)
                {
                    return new Tuple<Boolean, String, int>(false, Constants.MYSQL_ERROR + " " + ex.ToString(), 0);
                }
                finally
                {
                    if (mySqlDataReader != null)
                    {
                        mySqlDataReader.Close();
                        mySqlConnection.Close();
                    }
                }
            });
        }
    }
}