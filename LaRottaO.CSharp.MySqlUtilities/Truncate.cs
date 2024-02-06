using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace LaRottaO.CSharp.MySqlUtilities
{
    public class Truncate
    {
        public async Task<Tuple<Boolean, String, int>> truncate(string argConnString, String argQuery, int argTimeOutMs)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(argConnString);

            try
            {
                mySqlConnection.Open();

                MySqlCommand mySqlCommand = new MySqlCommand(argQuery, mySqlConnection);

                if (argTimeOutMs != -1)
                {
                    mySqlCommand.CommandTimeout = argTimeOutMs;
                }

                int truncateResult = await mySqlCommand.ExecuteNonQueryAsync();

                return new Tuple<Boolean, String, int>(true, Constants.MYSQL_SUCCESS, truncateResult);
            }
            catch (Exception ex)
            {
                return new Tuple<Boolean, String, int>(false, Constants.MYSQL_ERROR + " " + ex.ToString(), 0);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }
    }
}