using IP.JobsAPI.Models;
using IP.JobsAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using IP.MasterAPI.Services;
namespace IP.JobsAPI.Services
{
    public class JobsService : IJobsRepository
    {
        private SqlConnection myconn;
        private JobAssignmentService jAssign;
        private JobRatesService jRates;
        private JobBOQVariationService jBOQVariation;
        private JobDocumentsService jDocuments;
        private JobComplaintsService jComplaints;
        private JobDefectsService jDefects;

        private ProjectService project;
        private TeamService team;
        private SubContractorService subcontractor;
        private JobStatusService jobstatus;
        private ProjectJobTypesService projectjobtypes;
        private ProjectRatesService projectRates;
        private MembersService members;
        private JobRatesService jobRates;

        public JobsService()
        {
            DBService dsc = DBService.GetSqlInstance();
            jAssign = new JobAssignmentService();
            jRates = new JobRatesService();
            jBOQVariation = new JobBOQVariationService();
            jDocuments = new JobDocumentsService();
            jComplaints = new JobComplaintsService();
            jDefects = new JobDefectsService();

            project = new ProjectService();
            team = new TeamService();
            subcontractor = new SubContractorService();
            jobstatus = new JobStatusService();
            projectjobtypes = new ProjectJobTypesService();
            projectRates = new ProjectRatesService();
            members = new MembersService();
            jobRates = new JobRatesService();

            myconn = dsc.GetDBConnection();
        }

        public List<Jobs> GetJobsDetailsAsync(int Id, int projId)
        {
            DataTable dtJobs = new DataTable();

            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobsGet";
            sqlCmd.Parameters.Add(new SqlParameter("@Id", Id));
            sqlCmd.Parameters.Add(new SqlParameter("@projId", projId));
            sqlCmd.Connection = myconn;
            SqlDataAdapter daJobs = new SqlDataAdapter(sqlCmd);
            dtJobs.Clear();
            daJobs.Fill(dtJobs);
            List<Jobs> lst = new List<Jobs>();
            if (dtJobs.Rows.Count == 0)
            {
                lst.Add(new Jobs());
                lst[0].project = project.GetProjectDetailsAsync(projId, 0);
                lst[0].team = team.GetTeamDetailsAsync(0, 'I');
                lst[0].subcontractor = subcontractor.GetSubContractorDetailsAsync(0, 'I');
                lst[0].jobstatus = jobstatus.GetJobStatusDetailsAsync(0);
                lst[0].projectjobtypes = projectjobtypes.GetProjectJobTypesDetailsAsync(0, projId);
                lst[0].projectRates = projectRates.GetProjectRatesDetailsAsync(0, projId);
                lst[0].members = members.GetMembersDetailsAsync(0, 'I');
                lst[0].jRates = jRates.GetJobRatesDetailsAsync(0, Id);
            }
            else
            {
                foreach (DataRow item in dtJobs.Rows)
                {

                    lst.Add(new Jobs()
                    {
                        Id = Convert.ToInt32(item[0]),
                        projId = Convert.ToInt32(item[1]),
                        projJobTypeId = Convert.ToInt32(item[2]),
                        jobDesc = item[3].ToString(),
                        projName = item[4].ToString(),
                        jobTypeName = item[5].ToString(),
                        locCode = item[6].ToString(),
                        completionDate = Convert.ToDateTime(item[7]).Date,
                        jobStatusId = Convert.ToInt32(item[8]),
                        jobStatusName = item[9].ToString(),
                        createdDate = Convert.ToDateTime(item[10]),
                        modifiedDate = item[11] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[11]),
                        userId = Convert.ToInt32(item[12]),
                        jobAssign = jAssign.GetJobAssignmentDetailsAsync(0, Convert.ToInt32(item[0])),
                        jobRates = jRates.GetJobRatesDetailsAsync(0, Convert.ToInt32(item[0])),
                        jobBOQVariation = jBOQVariation.GetJobBOQVariationDetailsAsync(0, Convert.ToInt32(item[0])),
                        jobDocuments = jDocuments.GetJobDocumentsDetailsAsync(0, Convert.ToInt32(item[0])),
                        jobComplaints = jComplaints.GetJobComplaintsDetailsAsync(0, Convert.ToInt32(item[0])),
                        jobDefects = jDefects.GetJobDefectsDetailsAsync(0, Convert.ToInt32(item[0]))
                    });

                }
                lst[0].project = project.GetProjectDetailsAsync(projId, 0);
                lst[0].team = team.GetTeamDetailsAsync(0, 'I');
                lst[0].subcontractor = subcontractor.GetSubContractorDetailsAsync(0, 'I');
                lst[0].jobstatus = jobstatus.GetJobStatusDetailsAsync(0);
                lst[0].projectjobtypes = projectjobtypes.GetProjectJobTypesDetailsAsync(0, projId);
                lst[0].projectRates = projectRates.GetProjectRatesDetailsAsync(0, projId);
                lst[0].members = members.GetMembersDetailsAsync(0, 'I');
                lst[0].jRates = jRates.GetJobRatesDetailsAsync(0, Id);
            }
            if (myconn.State != ConnectionState.Closed)
                myconn.Close();
            return lst;
        }

        public void InsertJobsDetailsAsync(Jobs job)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();
            job.createdDate = DateTime.Now;
            job.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobsInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", job.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@projId", job.projId));
            sqlCmd.Parameters.Add(new SqlParameter("@projJobTypeId", job.projJobTypeId));
            sqlCmd.Parameters.Add(new SqlParameter("@jobDesc", job.jobDesc));
            sqlCmd.Parameters.Add(new SqlParameter("@locCode", job.locCode));
            sqlCmd.Parameters.Add(new SqlParameter("@completionDate", job.completionDate));
            sqlCmd.Parameters.Add(new SqlParameter("@jobStatusId", job.jobStatusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", job.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", job.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", job.userId));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                job.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

                // InsertJobAssignmentDetailsAsync(JobAssignment, tran);
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

        public List<Jobs> UpdateJobsDetailsAsync(Jobs job)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();
            job.createdDate = DateTime.Now;
            job.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobsInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", job.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@projId", job.projId));
            sqlCmd.Parameters.Add(new SqlParameter("@projJobTypeId", job.projJobTypeId));
            sqlCmd.Parameters.Add(new SqlParameter("@jobDesc", job.jobDesc));
            sqlCmd.Parameters.Add(new SqlParameter("@locCode", job.locCode));
            sqlCmd.Parameters.Add(new SqlParameter("@completionDate", job.completionDate));
            sqlCmd.Parameters.Add(new SqlParameter("@jobStatusId", job.jobStatusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", job.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", job.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", job.userId));
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
            return GetJobsDetailsAsync(0, job.projId);
        }

        public void DeleteJobsDetailsAsync(int Id)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_JobsDelete";
            sqlCmd.Parameters.Add(new SqlParameter("@Id", Id));

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

        }


    }
}
