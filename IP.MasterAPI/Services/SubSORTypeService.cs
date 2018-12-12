using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.MasterAPI.Services
{
    public class SubSORTypeService : ISubSORTypeRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public SubSORTypeService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<SubSORType> GetSubSORTypeDetailsAsync(int ID)
        {
            try
            {
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_SubSORTypeGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<SubSORType> lst = new List<SubSORType>();
                while (reader.Read())
                {

                    lst.Add(new SubSORType()
                    {
                        ID = Convert.ToInt32(reader.GetValue(0)),
                        name = reader.GetValue(1).ToString(),
                        SORTypeID = Convert.ToInt32(reader.GetValue(2)),
                        SORTypeName = reader.GetValue(3).ToString(),
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

        public void InsertSubSORTypeDetailsAsync(SubSORType SubSORType)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SubSORType.createdDate = DateTime.Now;
            SubSORType.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SubSORTypeInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", SubSORType.ID));
            sqlCmd.Parameters.Add(new SqlParameter("@name", SubSORType.name));
            sqlCmd.Parameters.Add(new SqlParameter("@SORTypeID", SubSORType.SORTypeID));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", SubSORType.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", SubSORType.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", SubSORType.userId));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                SubSORType.ID = Convert.ToInt32(sqlCmd.ExecuteScalar());


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

        public List<SubSORType> UpdateSubSORTypeDetailsAsync(SubSORType SubSORType)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SubSORType.createdDate = DateTime.Now;
            SubSORType.modifiedDate = DateTime.Now;


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SubSORTypeInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", SubSORType.ID));
            sqlCmd.Parameters.Add(new SqlParameter("@name", SubSORType.name));
            sqlCmd.Parameters.Add(new SqlParameter("@SORTypeID", SubSORType.SORTypeID));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", SubSORType.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", SubSORType.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", SubSORType.userId));
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
            return GetSubSORTypeDetailsAsync(0);
        }

        public List<SubSORType> DeleteSubSORTypeDetailsAsync(int ID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SubSORTypeDelete";
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
            return GetSubSORTypeDetailsAsync(0);
        }
    }
}