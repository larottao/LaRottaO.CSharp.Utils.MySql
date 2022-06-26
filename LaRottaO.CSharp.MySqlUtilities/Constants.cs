using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaRottaO.CSharp.MySqlUtilities
{
    public static class Constants
    {
        public static String MYSQL_SUCCESS { get; } = "SUCCESS";
        public static String MYSQL_NO_RESULTS { get; } = "RESULTS ARE EMPTY";
        public static String MYSQL_ERROR { get; } = "ERROR";

        //Example:
        //MYSQL_CONNECTION_STRING = "server='00.000.0.00';user='XXX';database='XXX';port='3308';password='XXX'"
    }
}