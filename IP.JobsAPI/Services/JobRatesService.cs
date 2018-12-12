using IP.JobsAPI.Models;
using IP.JobsAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.JobsAPI.Services
{
    public class JobRatesService : IJobRatesRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public JobRatesService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<JobRates> GetJobRatesDetailsAsync(int ID, int jobId)
        {
            try
            {
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_JobRatesGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));
                sqlCmd.Parameters.Add(new SqlParameter("@jobId", jobId));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<JobRates> lst = new List<JobRates>();
                while (reader.Read())
                {
                    lst.Add(new JobRates()
                    {
                        Id = Convert.ToInt32(reader.GetValue(0)),
                        jobID = Convert.ToInt32(reader.GetValue(1)),
                        projRatesId = Convert.ToInt32(reader.GetValue(2)),
                        SORCode = reader.GetValue(3).ToString(),
                        propsedBOQQuantity = Convert.ToDecimal( reader.GetValue(4)),
                        actualBOQQuantity = Convert.ToInt32(reader.GetValue(5)),
                        createdDate = Convert.ToDateTime(reader.GetValue(6)),
                        modifiedDate = reader.GetValue(7) == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(7)),
                        userId = Convert.ToInt32(reader.GetValue(8))


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
        public void InsertJobRatesDetailsAsync(JobRates jobAssign)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            jobAssign.createdDate = DateTime.Now;
            jobAssign.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobRatesInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", jobAssign.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@jobID", jobAssign.jobID));
            sqlCmd.Parameters.Add(new SqlParameter("@projRatesId", jobAssign.projRatesId));
            sqlCmd.Parameters.Add(new SqlParameter("@propsedBOQQuantity", jobAssign.propsedBOQQuantity));
            sqlCmd.Parameters.Add(new SqlParameter("@actualBOQQuantity", jobAssign.actualBOQQuantity));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", jobAssign.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", jobAssign.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", jobAssign.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                jobAssign.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());
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
        public void UpdateJobRatesDetailsAsync(JobRates jobAssign)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();
            jobAssign.createdDate = DateTime.Now;
            jobAssign.modifiedDate = DateTime.Now;


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobRatesInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", jobAssign.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@jobID", jobAssign.jobID));
            sqlCmd.Parameters.Add(new SqlParameter("@projRatesId", jobAssign.projRatesId));
            sqlCmd.Parameters.Add(new SqlParameter("@propsedBOQQuantity", jobAssign.propsedBOQQuantity));
            sqlCmd.Parameters.Add(new SqlParameter("@actualBOQQuantity", jobAssign.actualBOQQuantity));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", jobAssign.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", jobAssign.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", jobAssign.userId));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();

              //  UpdateJobRatesRatesDetailsAsync(JobRatesRates, tran);
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

        public void DeleteJobRatesDetailsAsync(int ID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobRatesDelete";
            sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));

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
        }
    }
}