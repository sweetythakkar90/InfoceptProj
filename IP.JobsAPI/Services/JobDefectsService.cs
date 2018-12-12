using IP.JobsAPI.Models;
using IP.JobsAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.JobsAPI.Services
{
    public class JobDefectsService : IJobDefectsRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public JobDefectsService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<JobDefects> GetJobDefectsDetailsAsync(int ID, int jobId)
        {
            try
            {
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_JobDefectsGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));
                sqlCmd.Parameters.Add(new SqlParameter("@jobId", jobId));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<JobDefects> lst = new List<JobDefects>();
                while (reader.Read())
                {
                    lst.Add(new JobDefects()
                    {
                        Id = Convert.ToInt32(reader.GetValue(0)),
                        jobID = Convert.ToInt32(reader.GetValue(1)),
                        defectDesc = reader.GetValue(2).ToString(),
                        memberId = Convert.ToInt32(reader.GetValue(3)),
                        memberName = reader.GetValue(4).ToString(),
                        subcontractorID = Convert.ToInt32(reader.GetValue(5)),
                        subConName = reader.GetValue(6).ToString(),
                        dueDate = Convert.ToDateTime(reader.GetValue(7)),
                        priority = reader.GetValue(8).ToString(),
                        createdDate = Convert.ToDateTime(reader.GetValue(9)),
                        modifiedDate = reader.GetValue(10) == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(10)),
                        userId = Convert.ToInt32(reader.GetValue(11))


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
        public void InsertJobDefectsDetailsAsync(JobDefects jobAssign)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            jobAssign.createdDate = DateTime.Now;
            jobAssign.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobDefectsInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", jobAssign.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@jobID", jobAssign.jobID));
            sqlCmd.Parameters.Add(new SqlParameter("@defectDesc", jobAssign.defectDesc));
            sqlCmd.Parameters.Add(new SqlParameter("@memberId", jobAssign.memberId));
            sqlCmd.Parameters.Add(new SqlParameter("@subcontractorID", jobAssign.subcontractorID));
            sqlCmd.Parameters.Add(new SqlParameter("@dueDate", jobAssign.dueDate));
            sqlCmd.Parameters.Add(new SqlParameter("@priority", jobAssign.priority));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", jobAssign.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", jobAssign.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", jobAssign.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                jobAssign.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

               // InsertJobDefectsRateDetailsAsync(JobDefectsRates, JobDefects.JobDefectsID, tran);
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
        public void UpdateJobDefectsDetailsAsync(JobDefects jobAssign)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();
            jobAssign.createdDate = DateTime.Now;
            jobAssign.modifiedDate = DateTime.Now;


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobDefectsInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", jobAssign.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@jobID", jobAssign.jobID));
            sqlCmd.Parameters.Add(new SqlParameter("@defectDesc", jobAssign.defectDesc));
            sqlCmd.Parameters.Add(new SqlParameter("@memberId", jobAssign.memberId));
            sqlCmd.Parameters.Add(new SqlParameter("@subcontractorID", jobAssign.subcontractorID));
            sqlCmd.Parameters.Add(new SqlParameter("@dueDate", jobAssign.dueDate));
            sqlCmd.Parameters.Add(new SqlParameter("@priority", jobAssign.priority));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", jobAssign.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", jobAssign.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", jobAssign.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();

              //  UpdateJobDefectsRatesDetailsAsync(JobDefectsRates, tran);
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

        public void DeleteJobDefectsDetailsAsync(int ID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobDefectsDelete";
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