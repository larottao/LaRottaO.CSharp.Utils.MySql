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
        public async Task<Tuple<Boolean, String, List<Dictionary<String, Object>>>> executeStoredProcedure(String argConnString, String argStoredProcedureName, Dictionary<String, object> argInputParametersList, List<String> argOutputParametersList, int argTimeOutMs)
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

                foreach (KeyValuePair<string, object> inputParameter in argInputParametersList)
                {
                    mySqlCommand.Parameters.Add(new MySqlParameter(inputParameter.Key, inputParameter.Value));
                }

                List<Dictionary<String, Object>> totalOutputRows = new List<Dictionary<String, Object>>();

                using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                {
                    while (mySqlDataReader.Read())
                    {
                        Dictionary<String, Object> singleOutputRow = new Dictionary<String, Object>();

                        foreach (String outputParameter in argOutputParametersList)
                        {
                            singleOutputRow.Add(outputParameter, mySqlDataReader[outputParameter].ToString());
                        }

                        totalOutputRows.Add(singleOutputRow);
                    }
                }
                if (totalOutputRows.Count == 0)
                {
                    return new Tuple<Boolean, String, List<Dictionary<String, Object>>>(true, Constants.MYSQL_NO_RESULTS, totalOutputRows);
                }

                return new Tuple<Boolean, String, List<Dictionary<String, Object>>>(true, Constants.MYSQL_SUCCESS, totalOutputRows);
            }
            catch (Exception ex)
            {
                return new Tuple<Boolean, String, List<Dictionary<String, Object>>>(false, Constants.MYSQL_ERROR + " " + ex.Message, new List<Dictionary<String, Object>>());
            }
            finally
            {
                MmSqlConnection.Close();
            }
        }
    }
}