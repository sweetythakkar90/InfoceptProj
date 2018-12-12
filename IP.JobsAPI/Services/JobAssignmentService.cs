using IP.JobsAPI.Models;
using IP.JobsAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.JobsAPI.Services
{
    public class JobAssignmentService : IJobAssignmentRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public JobAssignmentService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<JobAssignment> GetJobAssignmentDetailsAsync(int ID, int jobId)
        {
            try
            {
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_JobAssignmentGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));
                sqlCmd.Parameters.Add(new SqlParameter("@jobId", jobId));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<JobAssignment> lst = new List<JobAssignment>();
                while (reader.Read())
                {
                    lst.Add(new JobAssignment()
                    {
                        Id = Convert.ToInt32(reader.GetValue(0)),
                        jobID = Convert.ToInt32(reader.GetValue(1)),
                        teamID = Convert.ToInt32(reader.GetValue(2)),
                        teamName = reader.GetValue(3).ToString(),
                        subcontractorID = Convert.ToInt32(reader.GetValue(4)),
                        subConName = reader.GetValue(5).ToString(),
                        jobAssignDate = Convert.ToDateTime(reader.GetValue(6)),
                        startTime = Convert.ToDateTime(reader.GetValue(7)),
                        endTime = Convert.ToDateTime(reader.GetValue(8)),
                        notes = reader.GetValue(9).ToString(),
                        createdDate = Convert.ToDateTime(reader.GetValue(10)),
                        modifiedDate = reader.GetValue(11) == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(11)),
                        userId = Convert.ToInt32(reader.GetValue(12))


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
        public void InsertJobAssignmentDetailsAsync(JobAssignment jobAssign)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            jobAssign.createdDate = DateTime.Now;
            jobAssign.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobAssignmentInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", jobAssign.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@jobID", jobAssign.jobID));
            sqlCmd.Parameters.Add(new SqlParameter("@teamID", jobAssign.teamID));
            sqlCmd.Parameters.Add(new SqlParameter("@subcontractorID", jobAssign.subcontractorID));
            sqlCmd.Parameters.Add(new SqlParameter("@jobAssignDate", jobAssign.jobAssignDate));
            sqlCmd.Parameters.Add(new SqlParameter("@startTime", jobAssign.startTime));
            sqlCmd.Parameters.Add(new SqlParameter("@endTime", jobAssign.endTime));
            sqlCmd.Parameters.Add(new SqlParameter("@notes", jobAssign.notes));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", jobAssign.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", jobAssign.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", jobAssign.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                jobAssign.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

               // InsertJobAssignmentRateDetailsAsync(JobAssignmentRates, JobAssignment.JobAssignmentID, tran);
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
        public void UpdateJobAssignmentDetailsAsync(JobAssignment jobAssign)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();
            jobAssign.createdDate = DateTime.Now;
            jobAssign.modifiedDate = DateTime.Now;


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobAssignmentInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", jobAssign.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@jobID", jobAssign.jobID));
            sqlCmd.Parameters.Add(new SqlParameter("@teamID", jobAssign.teamID));
            sqlCmd.Parameters.Add(new SqlParameter("@subcontractorID", jobAssign.subcontractorID));
            sqlCmd.Parameters.Add(new SqlParameter("@jobAssignDate", jobAssign.jobAssignDate));
            sqlCmd.Parameters.Add(new SqlParameter("@startTime", jobAssign.startTime));
            sqlCmd.Parameters.Add(new SqlParameter("@endTime", jobAssign.endTime));
            sqlCmd.Parameters.Add(new SqlParameter("@notes", jobAssign.notes));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", jobAssign.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", jobAssign.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", jobAssign.userId));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();

              //  UpdateJobAssignmentRatesDetailsAsync(JobAssignmentRates, tran);
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

        public void DeleteJobAssignmentDetailsAsync(int ID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobAssignmentDelete";
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