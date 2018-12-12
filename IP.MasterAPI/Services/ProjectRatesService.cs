using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.MasterAPI.Services
{
    public class ProjectRatesService : IProjectRatesRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public ProjectRatesService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<ProjectRates> GetProjectRatesDetailsAsync(int ID, int projID)
        {
            try
            {
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_ProjectRatesGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));
                sqlCmd.Parameters.Add(new SqlParameter("@projID", projID));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<ProjectRates> lst = new List<ProjectRates>();
                while (reader.Read())
                {
                    lst.Add(new ProjectRates()
                    {
                        Id = Convert.ToInt32(reader.GetValue(0)),
                        ProjId = Convert.ToInt32(reader.GetValue(1)),
                        SORTypeId = Convert.ToInt32(reader.GetValue(2)),
                        SORTypeName = reader.GetValue(3).ToString(),
                        subSORTypeId = Convert.ToInt32(reader.GetValue(4)),
                        subSORTypeName = reader.GetValue(5).ToString(),
                        SORCode = reader.GetValue(6).ToString(),
                        description = reader.GetValue(7).ToString(),
                        unitOfMeasure = reader.GetValue(8).ToString(),
                        unit = Convert.ToInt32(reader.GetValue(9)),
                        unitPrice = Convert.ToDecimal(reader.GetValue(10)),
                        cost = Convert.ToDecimal(reader.GetValue(11)),
                        statusId = Convert.ToInt32(reader.GetValue(12)),
                        statusName= reader.GetValue(13).ToString(),
                        expiryDate = Convert.ToDateTime(reader.GetValue(14)),
                        createdDate = Convert.ToDateTime(reader.GetValue(15)),
                        modifiedDate = reader.GetValue(16) == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(16)),
                        userId = Convert.ToInt32(reader.GetValue(17))


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
        public void InsertProjectRatesDetailsAsync(ProjectRates projRates)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            projRates.createdDate = DateTime.Now;
            projRates.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ProjectRatesInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", projRates.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@ProjId", projRates.ProjId));
            sqlCmd.Parameters.Add(new SqlParameter("@SORTypeId", projRates.SORTypeId));
            sqlCmd.Parameters.Add(new SqlParameter("@subSORTypeId", projRates.subSORTypeId));
            sqlCmd.Parameters.Add(new SqlParameter("@SORCode", projRates.SORCode));
            sqlCmd.Parameters.Add(new SqlParameter("@description", projRates.description));
            sqlCmd.Parameters.Add(new SqlParameter("@unitOfMeasure", projRates.unitOfMeasure));
            sqlCmd.Parameters.Add(new SqlParameter("@unit", projRates.unit));
            sqlCmd.Parameters.Add(new SqlParameter("@unitPrice", projRates.unitPrice));
            sqlCmd.Parameters.Add(new SqlParameter("@cost", projRates.cost));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", projRates.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@expiryDate", projRates.expiryDate));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", projRates.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", projRates.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", projRates.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                projRates.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

               // InsertProjectRatesRateDetailsAsync(ProjectRatesRates, ProjectRates.ProjectRatesID, tran);
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
        public void UpdateProjectRatesDetailsAsync(ProjectRates projRates)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();
            projRates.createdDate = DateTime.Now;
            projRates.modifiedDate = DateTime.Now;


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ProjectRatesInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", projRates.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@ProjId", projRates.ProjId));
            sqlCmd.Parameters.Add(new SqlParameter("@SORTypeId", projRates.SORTypeId));
            sqlCmd.Parameters.Add(new SqlParameter("@subSORTypeId", projRates.subSORTypeId));
            sqlCmd.Parameters.Add(new SqlParameter("@SORCode", projRates.SORCode));
            sqlCmd.Parameters.Add(new SqlParameter("@description", projRates.description));
            sqlCmd.Parameters.Add(new SqlParameter("@unitOfMeasure", projRates.unitOfMeasure));
            sqlCmd.Parameters.Add(new SqlParameter("@unit", projRates.unit));
            sqlCmd.Parameters.Add(new SqlParameter("@unitPrice", projRates.unitPrice));
            sqlCmd.Parameters.Add(new SqlParameter("@cost", projRates.cost));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", projRates.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@expiryDate", projRates.expiryDate));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", projRates.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", projRates.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", projRates.userId));
            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();

              //  UpdateProjectRatesRatesDetailsAsync(ProjectRatesRates, tran);
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

        public void DeleteProjectRatesDetailsAsync(int ID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ProjectRatesDelete";
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