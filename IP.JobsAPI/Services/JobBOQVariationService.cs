using IP.JobsAPI.Models;
using IP.JobsAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.JobsAPI.Services
{
    public class JobBOQVariationService : IJobBOQVariationRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public JobBOQVariationService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<JobBOQVariation> GetJobBOQVariationDetailsAsync(int ID, int jobId)
        {
            try
            {
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_JobBOQVariationGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));
                sqlCmd.Parameters.Add(new SqlParameter("@jobId", jobId));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<JobBOQVariation> lst = new List<JobBOQVariation>();
                while (reader.Read())
                {
                    lst.Add(new JobBOQVariation()
                    {
                        Id = Convert.ToInt32(reader.GetValue(0)),
                        jobID = Convert.ToInt32(reader.GetValue(1)),
                        jobRatesId = Convert.ToInt32(reader.GetValue(2)),
                        SORCode = reader.GetValue(3).ToString(),
                        BOQQuantity = Convert.ToDecimal(reader.GetValue(4)),
                        createdDate = Convert.ToDateTime(reader.GetValue(5)),
                        modifiedDate = reader.GetValue(6) == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(6)),
                        userId = Convert.ToInt32(reader.GetValue(7))


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
        public void InsertJobBOQVariationDetailsAsync(JobBOQVariation jbBOQVariation)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            jbBOQVariation.createdDate = DateTime.Now;
            jbBOQVariation.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobBOQVariationInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", jbBOQVariation.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@jobID", jbBOQVariation.jobID));
            sqlCmd.Parameters.Add(new SqlParameter("@jobRatesId", jbBOQVariation.jobRatesId));
            sqlCmd.Parameters.Add(new SqlParameter("@BOQQuantity", jbBOQVariation.BOQQuantity));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", jbBOQVariation.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", jbBOQVariation.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", jbBOQVariation.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                jbBOQVariation.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

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
        public void UpdateJobBOQVariationDetailsAsync(JobBOQVariation jbBOQVariation)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();
            jbBOQVariation.createdDate = DateTime.Now;
            jbBOQVariation.modifiedDate = DateTime.Now;


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobBOQVariationInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", jbBOQVariation.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@jobID", jbBOQVariation.jobID));
            sqlCmd.Parameters.Add(new SqlParameter("@jobRatesId", jbBOQVariation.jobRatesId));
            sqlCmd.Parameters.Add(new SqlParameter("@BOQQuantity", jbBOQVariation.BOQQuantity));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", jbBOQVariation.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", jbBOQVariation.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", jbBOQVariation.userId));

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

        public void DeleteJobBOQVariationDetailsAsync(int ID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobBOQVariationDelete";
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