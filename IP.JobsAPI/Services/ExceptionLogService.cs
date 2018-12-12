using IP.JobsAPI.Models;
using IP.JobsAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.JobsAPI.Services
{
    public class ExceptionLogService : IExceptionLogRepository
    {
        private SqlConnection myconn;
        
        public ExceptionLogService()
        {
            DBService dsc = DBService.GetSqlInstance();
            myconn = dsc.GetDBConnection();
        }

        public List<ExceptionLog> GetExceptionLogDetailsAsync(int ExceptionLogID) 
        {
            SqlDataReader reader = null;
            if (myconn.State != ConnectionState.Open)
                myconn.Open();
          
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ExceptionLogGet";
            sqlCmd.Parameters.Add(new SqlParameter("@ExceptionLogID", ExceptionLogID));

            sqlCmd.Connection = myconn;
            reader = sqlCmd.ExecuteReader();
            List<ExceptionLog> lst = new List<ExceptionLog>();
            while (reader.Read())
            {
                lst.Add(new ExceptionLog
                {
                    ID = Convert.ToInt32(reader.GetValue(0)),
                    UserID = Convert.ToInt32(reader.GetValue(1)),
                    ApplicationName = reader.GetValue(2).ToString(),
                    MachineName = reader.GetValue(3).ToString(),
                    ExceptionClassName = reader.GetValue(4).ToString(),
                    ExceptionMethodName = reader.GetValue(5).ToString(),
                    ExceptionMessage = reader.GetValue(6).ToString(),
                    ExceptionStackTrace = reader.GetValue(7).ToString(),
                    ServerName = reader.GetValue(8).ToString(),
                    ExceptionType = reader.GetValue(9).ToString(),
                    Url = reader.GetValue(10).ToString(),
                    ExceptionLoggingTime = reader.GetValue(11) == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(11))

                    
                });

            }

            if (myconn.State != ConnectionState.Closed)
                myconn.Close();


            return lst;
        }

        public void InsertExceptionLogDetailsAsync(ExceptionLog eLog)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ExceptionLogInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@ID", eLog.ID));
            sqlCmd.Parameters.Add(new SqlParameter("@UserID", eLog.UserID));
            sqlCmd.Parameters.Add(new SqlParameter("@ApplicationName", eLog.ApplicationName));
            sqlCmd.Parameters.Add(new SqlParameter("@MachineName", eLog.MachineName));
            sqlCmd.Parameters.Add(new SqlParameter("@ExceptionClassName", eLog.ExceptionClassName));
            sqlCmd.Parameters.Add(new SqlParameter("@ExceptionMethodName", eLog.ExceptionMethodName));
            sqlCmd.Parameters.Add(new SqlParameter("@ExceptionMessage", eLog.ExceptionMessage));
            sqlCmd.Parameters.Add(new SqlParameter("@ExceptionStackTrace", eLog.ExceptionStackTrace));
            sqlCmd.Parameters.Add(new SqlParameter("@ServerName", eLog.ServerName));
            sqlCmd.Parameters.Add(new SqlParameter("@ExceptionType", eLog.ExceptionType));
            sqlCmd.Parameters.Add(new SqlParameter("@Url", eLog.Url));
            sqlCmd.Parameters.Add(new SqlParameter("@ExceptionLoggingTime", eLog.ExceptionLoggingTime));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                eLog.ID = Convert.ToInt32(sqlCmd.ExecuteScalar());

                tran.Commit();
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                if (myconn.State != ConnectionState.Closed)
                    myconn.Close();
            }
            
        }

        public List<ExceptionLog> UpdateExceptionLogDetailsAsync(ExceptionLog eLog)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ExceptionLogInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@ID", eLog.ID));
            sqlCmd.Parameters.Add(new SqlParameter("@UserID", eLog.UserID));
            sqlCmd.Parameters.Add(new SqlParameter("@ApplicationName", eLog.ApplicationName));
            sqlCmd.Parameters.Add(new SqlParameter("@MachineName", eLog.MachineName));
            sqlCmd.Parameters.Add(new SqlParameter("@ExceptionClassName", eLog.ExceptionClassName));
            sqlCmd.Parameters.Add(new SqlParameter("@ExceptionMethodName", eLog.ExceptionMethodName));
            sqlCmd.Parameters.Add(new SqlParameter("@ExceptionMessage", eLog.ExceptionMessage));
            sqlCmd.Parameters.Add(new SqlParameter("@ExceptionStackTrace", eLog.ExceptionStackTrace));
            sqlCmd.Parameters.Add(new SqlParameter("@ServerName", eLog.ServerName));
            sqlCmd.Parameters.Add(new SqlParameter("@ExceptionType", eLog.ExceptionType));
            sqlCmd.Parameters.Add(new SqlParameter("@Url", eLog.Url));
            sqlCmd.Parameters.Add(new SqlParameter("@ExceptionLoggingTime", eLog.ExceptionLoggingTime));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();

                tran.Commit();
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                if (myconn.State != ConnectionState.Closed)
                    myconn.Close();
            }
            return GetExceptionLogDetailsAsync(0);
        }

        public List<ExceptionLog> DeleteExceptionLogDetailsAsync(int ExceptionLogID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ExceptionLogDelete";
            sqlCmd.Parameters.Add(new SqlParameter("@ExceptionLogID", ExceptionLogID));
            
            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();

                tran.Commit();
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                if (myconn.State != ConnectionState.Closed)
                    myconn.Close();
            }
            return GetExceptionLogDetailsAsync(0);
        }
    }
}