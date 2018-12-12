using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.MasterAPI.Services
{
    public class ProjectMembersService : IProjectMembersRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public ProjectMembersService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<ProjectMembers> GetProjectMembersDetailsAsync(int ID, int projID)
        {
            try
            {
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_ProjectMembersGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));
                sqlCmd.Parameters.Add(new SqlParameter("@projID", projID));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<ProjectMembers> lst = new List<ProjectMembers>();
                while (reader.Read())
                {

                    lst.Add(new ProjectMembers()
                    {
                        Id = Convert.ToInt32(reader.GetValue(0)),
                        projId = Convert.ToInt32(reader.GetValue(1)),
                        memberId = Convert.ToInt32(reader.GetValue(2)),
                        memberName = reader.GetValue(3).ToString(),
                        statusId = Convert.ToInt32(reader.GetValue(4)),
                        statusName = reader.GetValue(5).ToString(),
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
        public void InsertProjectMembersDetailsAsync(ProjectMembers projMembers)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            projMembers.createdDate = DateTime.Now;
            projMembers.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ProjectMembersInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@ID", projMembers.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@projId", projMembers.projId));
            sqlCmd.Parameters.Add(new SqlParameter("@memberId", projMembers.memberId));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", projMembers.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", projMembers.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", projMembers.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", projMembers.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                projMembers.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

               // InsertProjectMembersRateDetailsAsync(ProjectMembersRates, ProjectMembers.ProjectMembersID, tran);
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
        public void UpdateProjectMembersDetailsAsync(ProjectMembers projMembers)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();
            projMembers.createdDate = DateTime.Now;
            projMembers.modifiedDate = DateTime.Now;


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ProjectMembersInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@ID", projMembers.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@projId", projMembers.projId));
            sqlCmd.Parameters.Add(new SqlParameter("@memberId", projMembers.memberId));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", projMembers.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", projMembers.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", projMembers.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", projMembers.userId));
            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();

              //  UpdateProjectMembersRatesDetailsAsync(ProjectMembersRates, tran);
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

        public void DeleteProjectMembersDetailsAsync(int ID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ProjectMembersDelete";
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