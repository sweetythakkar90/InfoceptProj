using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.MasterAPI.Services
{
    public class ProjectSubContractorsService : IProjectSubContractorsRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public ProjectSubContractorsService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<ProjectSubContractors> GetProjectSubContractorsDetailsAsync(int ID, int projID)
        {
            try
            {
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_ProjectSubContractorsGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));
                sqlCmd.Parameters.Add(new SqlParameter("@projID", projID));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<ProjectSubContractors> lst = new List<ProjectSubContractors>();
                while (reader.Read())
                {

                    lst.Add(new ProjectSubContractors()
                    {
                        Id = Convert.ToInt32(reader.GetValue(0)),
                        projId = Convert.ToInt32(reader.GetValue(1)),
                        subcontractorId = Convert.ToInt32(reader.GetValue(2)),
                        subconName = reader.GetValue(3).ToString(),
                        statusId = Convert.ToInt32(reader.GetValue(4)),
                        statusName= reader.GetValue(5).ToString(),
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
        public void InsertProjectSubContractorsDetailsAsync(ProjectSubContractors projSubContractors)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            projSubContractors.createdDate = DateTime.Now;
            projSubContractors.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ProjectSubContractorsInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@ID", projSubContractors.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@projId", projSubContractors.projId));
            sqlCmd.Parameters.Add(new SqlParameter("@subcontractorId", projSubContractors.subcontractorId));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", projSubContractors.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", projSubContractors.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", projSubContractors.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", projSubContractors.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                projSubContractors.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

               // InsertProjectSubContractorsRateDetailsAsync(ProjectSubContractorsRates, ProjectSubContractors.ProjectSubContractorsID, tran);
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
        public void UpdateProjectSubContractorsDetailsAsync(ProjectSubContractors projSubContractors)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();
            projSubContractors.createdDate = DateTime.Now;
            projSubContractors.modifiedDate = DateTime.Now;


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ProjectSubContractorsInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@ID", projSubContractors.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@projId", projSubContractors.projId));
            sqlCmd.Parameters.Add(new SqlParameter("@subcontractorId", projSubContractors.subcontractorId));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", projSubContractors.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", projSubContractors.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", projSubContractors.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", projSubContractors.userId));
            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();

              //  UpdateProjectSubContractorsRatesDetailsAsync(ProjectSubContractorsRates, tran);
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

        public void DeleteProjectSubContractorsDetailsAsync(int ID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ProjectSubContractorsDelete";
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