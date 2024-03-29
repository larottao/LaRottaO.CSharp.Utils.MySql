﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace LaRottaO.CSharp.MySqlUtilities
{
    public class Select
    {
        public async Task<Tuple<Boolean, String, List<T>>> select<T>(string argConnString, String argQuery, int argTimeoutMs, Boolean showDebug = false) where T : new()
        {
            if (showDebug)
            {
                Console.WriteLine("Connection String is: " + argConnString);
                Console.WriteLine("Query is: " + argQuery);
            }

            MySqlConnection conn = new MySqlConnection(argConnString);

            DbDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(argQuery, conn);

                if (argTimeoutMs != -1)
                {
                    cmd.CommandTimeout = argTimeoutMs;
                }

                rdr = await cmd.ExecuteReaderAsync();

                //  Create a dictionary that contains each column name and a consecutive number. That number will be later  used to locate the column by its name.

                Dictionary<String, int> dictionaryColumnNameVsIndex = new Dictionary<String, int>();

                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    String nombreColumna = rdr.GetName(i);
                    dictionaryColumnNameVsIndex.Add(nombreColumna, i);
                }

                PropertyInfo[] properties = typeof(T).GetProperties();

                T destinationObject;

                List<T> listaSalida = new List<T>();

                while (rdr.Read())
                {
                    //  For each row obtained from the query execution, create a new instance of the Object

                    destinationObject = new T();

                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        foreach (PropertyInfo property in properties)
                        {
                            //  Check if the destination object contains a property with the same name.

                            if (dictionaryColumnNameVsIndex.ContainsKey(property.Name))
                            {
                                //  If it does, assign the value to said property.

                                PropertyInfo propertyToBeChanged = destinationObject.GetType().GetProperty(property.Name);

                                Object newValue = null;

                                try
                                {
                                    newValue = rdr[dictionaryColumnNameVsIndex[property.Name]];

                                    if (newValue == null || newValue.GetType().ToString() == "System.DBNull")
                                    {
                                        propertyToBeChanged.SetValue(destinationObject, null);
                                    }
                                    else
                                    {
                                        propertyToBeChanged.SetValue(destinationObject, newValue);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    propertyToBeChanged.SetValue(destinationObject, null);

                                    String errorMessageForConsole = "ERROR: Parsing the property: " + property.Name + " failed. " + ex.Message;

                                    Console.WriteLine(errorMessageForConsole);
                                    Debug.WriteLine(errorMessageForConsole);

                                    return new Tuple<Boolean, String, List<T>>(false, Constants.MYSQL_ERROR + errorMessageForConsole, new List<T>());
                                }
                            }
                        }
                    }

                    //  After all rows have been processed, return the object list

                    listaSalida.Add(destinationObject);
                }

                if (listaSalida.Count == 0)
                {
                    return new Tuple<Boolean, String, List<T>>(false, Constants.MYSQL_NO_RESULTS, listaSalida);
                }

                return new Tuple<Boolean, String, List<T>>(true, Constants.MYSQL_SUCCESS, listaSalida);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
                Debug.WriteLine(ex.Message + " " + ex.StackTrace);

                return new Tuple<Boolean, String, List<T>>(false, Constants.MYSQL_ERROR + " " + ex, new List<T>());
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}