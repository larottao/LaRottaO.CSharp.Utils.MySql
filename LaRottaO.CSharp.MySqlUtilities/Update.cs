using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaRottaO.CSharp.MySqlUtilities
{
    public class Update
    {
        public async Task<Tuple<Boolean, String, int>> update(string argConnectionString, String stringQuery, int argTimeOutMs)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(argConnectionString);

            MySqlDataReader mySqlDataReader = null;

            try
            {
                mySqlConnection.Open();

                MySqlCommand mySqlCommand = new MySqlCommand(stringQuery, mySqlConnection);

                if (argTimeOutMs != -1)
                {
                    mySqlCommand.CommandTimeout = argTimeOutMs;
                }

                int resultadoUpdate = mySqlCommand.ExecuteNonQuery();

                return new Tuple<Boolean, String, int>(true, Constants.MYSQL_SUCCESS, resultadoUpdate);
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
        }
    }
}