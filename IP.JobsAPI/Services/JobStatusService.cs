using IP.JobsAPI.Models;
using IP.JobsAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace IP.JobsAPI.Services
{
    public class JobStatusService : IJobStatusRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public JobStatusService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

       
       
        public List<JobStatus> GetJobStatusDetailsAsync(int Id)
        {
            try
            {

           
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_JobStatusGet";
                sqlCmd.Parameters.Add(new SqlParameter("@Id", Id));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<JobStatus> lst = new List<JobStatus>();
                while (reader.Read())
                {

                    lst.Add(new JobStatus()
                    {
                        ID = Convert.ToInt32(reader.GetValue(0)),
                        name = reader.GetValue(1).ToString(),
                        createdDate = Convert.ToDateTime(reader.GetValue(2)),
                        modifiedDate = reader.GetValue(3) == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(3)),
                        userId = Convert.ToInt32(reader.GetValue(4))
                    });
                }

                if (myconn.State != ConnectionState.Closed)
                    myconn.Close();
                return lst;
            }
            catch (Exception ex)
            {
                gs.LogData(ex);
                throw ex;
            }

        }

        public void InsertJobStatusDetailsAsync(JobStatus jobStatus)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            jobStatus.createdDate = DateTime.Now;
            jobStatus.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobStatusInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", jobStatus.ID));
            sqlCmd.Parameters.Add(new SqlParameter("@name", jobStatus.name));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", jobStatus.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", jobStatus.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", jobStatus.userId));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                jobStatus.ID = Convert.ToInt32(sqlCmd.ExecuteScalar());


                tran.Commit();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                gs.LogData(ex);
                throw ex;
            }
            finally
            {
                if (myconn.State != ConnectionState.Closed)
                    myconn.Close();
            }
        }

        public List<JobStatus> UpdateJobStatusDetailsAsync(JobStatus jobStatus)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            jobStatus.createdDate = DateTime.Now;
            jobStatus.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobStatusInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", jobStatus.ID));
            sqlCmd.Parameters.Add(new SqlParameter("@name", jobStatus.name));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", jobStatus.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", jobStatus.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", jobStatus.userId));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;
              
                sqlCmd.ExecuteNonQuery();

                tran.Commit();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                gs.LogData(ex);
                throw;
            }
            finally
            {
                if (myconn.State != ConnectionState.Closed)
                    myconn.Close();
            }
            return GetJobStatusDetailsAsync(0);
        }

        public List<JobStatus> DeleteJobStatusDetailsAsync(int Id)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobStatusDelete";
            sqlCmd.Parameters.Add(new SqlParameter("@ID", Id));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;
               
                sqlCmd.ExecuteNonQuery();


                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                gs.LogData(ex);
                throw ex;
            }
            finally
            {
                if (myconn.State != ConnectionState.Closed)
                    myconn.Close();
            }
            return GetJobStatusDetailsAsync(0);
        }
    }
}