using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace IP.JobsAPI.Services
{
    public sealed class DBService
    {
        private static DBService dsc;
        private static string connectionStr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
        private static SqlConnection conn;
        private DBService() { }

        public static DBService GetSqlInstance()
        {
            if (dsc == null)
                dsc = new DBService();
            return dsc;
        }

        public SqlConnection GetDBConnection()
        {
            try
            {
                conn = new SqlConnection(connectionStr);
                return conn;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }




    }
}