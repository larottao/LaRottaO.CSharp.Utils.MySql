using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaRottaO.CSharp.MySqlUtilities
{
    public class ExecuteStoredProcedure
    {
        public Task<Tuple<Boolean, String, List<Dictionary<String, String>>>> executeStoredProcedure(String argConnString, String argStoredProcedureName, Dictionary<String, String> argInputParametersList, List<String> argOutputParametersList, int argTimeOutMs)
        {
            return Task.Run(() =>
            {
                MySqlConnection MmSqlConnection = new MySqlConnection(argConnString);
                MySqlCommand mySqlCommand = new MySqlCommand();

                try
                {
                    if (argTimeOutMs != -1)
                    {
                        mySqlCommand.CommandTimeout = argTimeOutMs;
                    }

                    mySqlCommand.Connection = MmSqlConnection;
                    mySqlCommand.CommandText = argStoredProcedureName;
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    mySqlCommand.CommandTimeout = Convert.ToInt32(argTimeOutMs);

                    MmSqlConnection.Open();
                    mySqlCommand.Connection = MmSqlConnection;

                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    foreach (KeyValuePair<string, string> inputParameter in argInputParametersList)
                    {
                        mySqlCommand.Parameters.Add(new MySqlParameter(inputParameter.Key, inputParameter.Value));
                    }

                    List<Dictionary<String, String>> totalOutputRows = new List<Dictionary<String, String>>();

                    Dictionary<String, String> singleOutputRow = new Dictionary<String, String>();

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            foreach (String outputParameter in argOutputParametersList)
                            {
                                singleOutputRow.Add(outputParameter, mySqlDataReader[outputParameter].ToString());
                            }

                            totalOutputRows.Add(singleOutputRow);
                        }
                    }

                    return new Tuple<Boolean, String, List<Dictionary<String, String>>>(true, Constants.MYSQL_SUCCESS, totalOutputRows);
                }
                catch (Exception ex)
                {
                    return new Tuple<Boolean, String, List<Dictionary<String, String>>>(false, Constants.MYSQL_ERROR + ex.Message, new List<Dictionary<String, String>>());
                }
                finally
                {
                    MmSqlConnection.Close();
                }
            });
        }
    }
}