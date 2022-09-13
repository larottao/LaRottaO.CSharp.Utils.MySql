using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            try
            {
                mySqlConnection.Open();

                foreach (String query in argQueries)
                {
                    if (argShowDebug)
                    {
                        Console.WriteLine(query);
                    }

                    MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection);

                    if (argTimeoutMs != -1)
                    {
                        mySqlCommand.CommandTimeout = argTimeoutMs;
                    }

                    resultsList.Add(new Tuple<Boolean, String>(true, query + " " + mySqlCommand.ExecuteNonQuery()));
                }

                return resultsList;
            }
            catch (Exception ex)
            {
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