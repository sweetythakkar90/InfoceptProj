using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace IP.MasterAPI.Services
{
    public class StatusTypeService : IStatusTypeRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public StatusTypeService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

       
       
        public List<StatusType> GetStatusTypeDetailsAsync(int StatusTypeID)
        {
            try
            {

           
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_StatusTypeGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", StatusTypeID));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<StatusType> lst = new List<StatusType>();
                while (reader.Read())
                {

                    lst.Add(new StatusType()
                    {
                        ID = Convert.ToInt32(reader.GetValue(0)),
                        name = reader.GetValue(1).ToString(),
                        createdDate = Convert.ToDateTime(reader.GetValue(2)),
                        modifiedDate = reader.GetValue(3) == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(3)),
                        userId = Convert.ToInt32(reader.GetValue(4))
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

        public void InsertStatusTypeDetailsAsync(StatusType statusType)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            statusType.createdDate = DateTime.Now;
            statusType.modifiedDate = DateTime.Now;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_StatusTypeInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", statusType.ID));
            sqlCmd.Parameters.Add(new SqlParameter("@name", statusType.name));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", statusType.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", statusType.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", statusType.userId));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                statusType.ID = Convert.ToInt32(sqlCmd.ExecuteScalar());


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

        public List<StatusType> UpdateStatusTypeDetailsAsync(StatusType statusType)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            statusType.createdDate = DateTime.Now;
            statusType.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_StatusTypeInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", statusType.ID));
            sqlCmd.Parameters.Add(new SqlParameter("@name", statusType.name));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", statusType.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", statusType.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", statusType.userId));

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
                throw;
            }
            finally
            {
                if (myconn.State != ConnectionState.Closed)
                    myconn.Close();
            }
            return GetStatusTypeDetailsAsync(0);
        }

        public List<StatusType> DeleteStatusTypeDetailsAsync(int statusTypeID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_StatusTypeDelete";
            sqlCmd.Parameters.Add(new SqlParameter("@ID", statusTypeID));

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
            return GetStatusTypeDetailsAsync(0);
        }
    }
}