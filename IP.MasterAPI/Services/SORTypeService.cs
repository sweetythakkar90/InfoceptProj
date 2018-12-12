using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.MasterAPI.Services
{
    public class SORTypeService : ISORTypeRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public SORTypeService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<SORType> GetSORTypeDetailsAsync(int ID) 
        {
            try
            {
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_SORTypeGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<SORType> lst = new List<SORType>();
                while (reader.Read())
                {

                    lst.Add(new SORType()
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

        public void InsertSORTypeDetailsAsync(SORType SORType)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SORType.createdDate = DateTime.Now;
            SORType.modifiedDate = DateTime.Now;
            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SORTypeInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", SORType.ID));
            sqlCmd.Parameters.Add(new SqlParameter("@name", SORType.name));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", SORType.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", SORType.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", SORType.userId));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                SORType.ID = Convert.ToInt32(sqlCmd.ExecuteScalar());

        
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

        public List<SORType> UpdateSORTypeDetailsAsync(SORType SORType)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SORType.createdDate = DateTime.Now;
            SORType.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SORTypeInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", SORType.ID));
            sqlCmd.Parameters.Add(new SqlParameter("@name", SORType.name));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", SORType.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", SORType.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", SORType.userId));
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
            return GetSORTypeDetailsAsync(0);
        }

        public List<SORType> DeleteSORTypeDetailsAsync(int ID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SORTypeDelete";
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
            return GetSORTypeDetailsAsync(0);
        }
    }
}