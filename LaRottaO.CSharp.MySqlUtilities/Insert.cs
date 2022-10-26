using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LaRottaO.CSharp.MySqlUtilities
{
    public class Insert
    {
        public async Task<List<Tuple<Boolean, String>>> insert(string argConnString, List<String> argQueries, int argTimeoutMs, Boolean argShowDebug = false)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(argConnString);

            MySqlDataReader mySqlDataReader = null;

            List<Tuple<Boolean, String>> resultsList = new List<Tuple<Boolean, String>>();

            String actualQueryForDebugPurposes = "";

            try
            {
                mySqlConnection.Open();

                foreach (String queryIterator in argQueries)
                {
                    actualQueryForDebugPurposes = queryIterator;

                    MySqlCommand mySqlCommand = new MySqlCommand(queryIterator, mySqlConnection);

                    if (argTimeoutMs != -1)
                    {
                        mySqlCommand.CommandTimeout = argTimeoutMs;
                    }

                    resultsList.Add(new Tuple<Boolean, String>(true, queryIterator + " " + mySqlCommand.ExecuteNonQuery()));

                    if (argShowDebug)
                    {
                        Debug.WriteLine(actualQueryForDebugPurposes + " SUCCESS");
                        Console.WriteLine(actualQueryForDebugPurposes + " SUCCESS");
                    }
                }

                return resultsList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(actualQueryForDebugPurposes + " FAILED " + ex.Message.ToString());
                Debug.WriteLine(actualQueryForDebugPurposes + " FAILED " + ex.Message.ToString());

                resultsList.Add(new Tuple<Boolean, String>(false, Constants.MYSQL_ERROR + " " + ex.Message.ToString()));

                return resultsList;
            }
            finally
            {
                if (mySqlDataReader != null)
                {
                    mySqlDataReader.Close();
                }

                if (mySqlConnection != null)
                    mySqlConnection.Close();
            }
        }
    }
}