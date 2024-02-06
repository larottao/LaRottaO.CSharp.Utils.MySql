using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace LaRottaO.CSharp.MySqlUtilities
{
    public class SelectSingleColumn
    {
        public async Task<Tuple<Boolean, String, List<String>>> selectSingleColumn(string argConnString, String argQuery, int argRequiredColumnIndex, int argTimeoutMs)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(argConnString);

            DbDataReader dbDataReader = null;

            try
            {
                mySqlConnection.Open();

                MySqlCommand mySqlCommand = new MySqlCommand(argQuery, mySqlConnection);

                if (argTimeoutMs != -1)
                {
                    mySqlCommand.CommandTimeout = argTimeoutMs;
                }

                dbDataReader = await mySqlCommand.ExecuteReaderAsync();

                List<String> outputList = new List<String>();

                while (dbDataReader.Read())
                {
                    outputList.Add(dbDataReader.GetValue(argRequiredColumnIndex).ToString());
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
                if (dbDataReader != null)
                {
                    dbDataReader.Close();
                }
                mySqlConnection.Close();
            }
        }
    }
}