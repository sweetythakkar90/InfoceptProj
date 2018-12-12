using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.MasterAPI.Services
{
    public class ProjectJobTypesService : IProjectJobTypesRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public ProjectJobTypesService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<ProjectJobTypes> GetProjectJobTypesDetailsAsync(int ID, int projID)
        {
            try
            {
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_ProjectJobTypesGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));
                sqlCmd.Parameters.Add(new SqlParameter("@projID", projID));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<ProjectJobTypes> lst = new List<ProjectJobTypes>();
                while (reader.Read())
                {

                    lst.Add(new ProjectJobTypes()
                    {
                        Id = Convert.ToInt32(reader.GetValue(0)),
                        projId = Convert.ToInt32(reader.GetValue(1)),
                        description = reader.GetValue(2).ToString(),
                        createdDate = Convert.ToDateTime(reader.GetValue(3)),
                        modifiedDate = reader.GetValue(4) == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(4)),
                        userId = Convert.ToInt32(reader.GetValue(5))
                        

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
        public void InsertProjectJobTypesDetailsAsync(ProjectJobTypes projJobTypes)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            projJobTypes.createdDate = DateTime.Now;
            projJobTypes.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ProjectJobTypesInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@ID", projJobTypes.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@projId", projJobTypes.projId));
            sqlCmd.Parameters.Add(new SqlParameter("@description", projJobTypes.description));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", projJobTypes.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", projJobTypes.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", projJobTypes.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                projJobTypes.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

               // InsertProjectJobTypesRateDetailsAsync(ProjectJobTypesRates, ProjectJobTypes.ProjectJobTypesID, tran);
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
        public void UpdateProjectJobTypesDetailsAsync(ProjectJobTypes projJobTypes)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();
            projJobTypes.createdDate = DateTime.Now;
            projJobTypes.modifiedDate = DateTime.Now;


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ProjectJobTypesInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@ID", projJobTypes.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@projId", projJobTypes.projId));
            sqlCmd.Parameters.Add(new SqlParameter("@description", projJobTypes.description));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", projJobTypes.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", projJobTypes.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", projJobTypes.userId));
            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();

              //  UpdateProjectJobTypesRatesDetailsAsync(ProjectJobTypesRates, tran);
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

        public void DeleteProjectJobTypesDetailsAsync(int ID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ProjectJobTypesDelete";
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