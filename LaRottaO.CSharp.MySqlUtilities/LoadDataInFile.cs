using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaRottaO.CSharp.MySqlUtilities
{
    public class LoadDataInFile
    {
        public async Task<Tuple<Boolean, String, int>> loadDataInFile(String argConnString, StringBuilder argLoadDataQuery)

        {
            /*
             * USAGE:
            LOAD DATA LOCAL INFILE '" + path.Replace("\\", "/") + "'" + " ");
            INTO TABLE Portabilidades ");
            FIELDS TERMINATED BY '\t'
            ENCLOSED BY '|'");
            LINES TERMINATED BY '\r\n'
            IGNORE 1 LINES
            (MVNO, CLIENT, COMPANY)
            SET DATE_INSERT = NOW()
            */

            MySqlConnection conn = new MySqlConnection(argConnString + ";AllowLoadLocalInfile=true");

            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(argLoadDataQuery.ToString(), conn);

                int loaderResult = cmd.ExecuteNonQuery();

                return new Tuple<Boolean, String, int>(true, Constants.MYSQL_SUCCESS, loaderResult);
            }
            catch (Exception ex)
            {
                return new Tuple<Boolean, String, int>(false, Constants.MYSQL_ERROR + " " + ex.ToString(), 0);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                    conn.Close();
                }
            }
        }
    }
}