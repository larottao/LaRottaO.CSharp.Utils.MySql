using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaRottaO.CSharp.MySqlUtilities
{
    public class SelectSingleColumn
    {
        public Task<Tuple<Boolean, String, List<String>>> selectSingleColumn(string argConnString, String argQuery, int argRequiredColumnIndex, int argTimeoutMs)
        {
            return Task.Run(() =>
            {
                MySqlConnection mySqlConnection = new MySqlConnection(argConnString);

                MySqlDataReader mySqlDataReader = null;

                try
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand(argQuery, mySqlConnection);

                    if (argTimeoutMs != -1)
                    {
                        mySqlCommand.CommandTimeout = argTimeoutMs;
                    }

                    mySqlDataReader = mySqlCommand.ExecuteReader();

                    List<String> outputList = new List<String>();

                    while (mySqlDataReader.Read())
                    {
                        outputList.Add(mySqlDataReader.GetValue(argRequiredColumnIndex).ToString());
                    }

                    if (outputList.Count == 0)
                    {
                        return new Tuple<Boolean, String, List<String>>(true, Constants.MYSQL_NO_RESULTS, outputList);
                    }

                    return new Tuple<Boolean, String, List<String>>(true, Constants.MYSQL_SUCCESS, outputList);
                }
                catch (Exception ex)
                {
                    return new Tuple<Boolean, String, List<String>>(false, Constants.MYSQL_ERROR + " " + ex.ToString(), null);
                }
                finally
                {
                    if (mySqlDataReader != null)
                    {
                        mySqlDataReader.Close();
                    }
                    if (mySqlConnection != null)
                    {
                        mySqlConnection.Close();
                    }
                }
            });
        }
    }
}