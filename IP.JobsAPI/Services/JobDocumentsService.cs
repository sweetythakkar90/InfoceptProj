using IP.JobsAPI.Models;
using IP.JobsAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.JobsAPI.Services
{
    public class JobDocumentsService : IJobDocumentsRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public JobDocumentsService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<JobDocuments> GetJobDocumentsDetailsAsync(int ID, int jobId)
        {
            try
            {
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_JobDocumentsGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));
                sqlCmd.Parameters.Add(new SqlParameter("@jobId", jobId));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<JobDocuments> lst = new List<JobDocuments>();
                while (reader.Read())
                {
                    lst.Add(new JobDocuments()
                    {
                        Id = Convert.ToInt32(reader.GetValue(0)),
                        jobID = Convert.ToInt32(reader.GetValue(1)),
                        documentPath = reader.GetValue(2).ToString(),
                        documentType = reader.GetValue(3).ToString(),
                        createdDate = Convert.ToDateTime(reader.GetValue(4)),
                        modifiedDate = reader.GetValue(5) == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(5)),
                        userId = Convert.ToInt32(reader.GetValue(6))


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
        public void InsertJobDocumentsDetailsAsync(JobDocuments jobAssign)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            jobAssign.createdDate = DateTime.Now;
            jobAssign.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobDocumentsInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", jobAssign.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@jobID", jobAssign.jobID));
            sqlCmd.Parameters.Add(new SqlParameter("@documentPath", jobAssign.documentPath));
            sqlCmd.Parameters.Add(new SqlParameter("@documentType", jobAssign.documentType));
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
        public void UpdateJobDocumentsDetailsAsync(JobDocuments jobAssign)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();
            jobAssign.createdDate = DateTime.Now;
            jobAssign.modifiedDate = DateTime.Now;


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobDocumentsInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", jobAssign.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@jobID", jobAssign.jobID));
            sqlCmd.Parameters.Add(new SqlParameter("@documentPath", jobAssign.documentPath));
            sqlCmd.Parameters.Add(new SqlParameter("@documentType", jobAssign.documentType));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", jobAssign.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", jobAssign.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", jobAssign.userId));

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

        public void DeleteJobDocumentsDetailsAsync(int ID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobDocumentsDelete";
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