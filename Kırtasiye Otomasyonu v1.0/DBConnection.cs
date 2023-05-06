using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.IO;




namespace Kırtasiye_Otomasyonu_v1._0
{
    public class DBConnection
    {
        private static string appConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.config");
        private static ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = appConfigPath };
        private static Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        string connectionString = ConfigurationManager.ConnectionStrings["localhost_KirtasiyeDB_ConnectionGeneral"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["localhost_KirtasiyeDB_ConnectionGeneral"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            return conn;
        }

        public static void OpenConnection(SqlConnection conn)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
        }
    }
}
