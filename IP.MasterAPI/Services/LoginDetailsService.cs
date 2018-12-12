using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.MasterAPI.Services
{
    public class LoginDetailsService : ILoginDetailsRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public LoginDetailsService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<LoginDetails> GetLoginDetailsAsync(int ID) 
        {
            try
            {
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_LoginGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<LoginDetails> lst = new List<LoginDetails>();
                while (reader.Read())
                {
                    lst.Add(new LoginDetails
                    {
                        ID = Convert.ToInt32(reader.GetValue(0)),
                        userId = Convert.ToInt32(reader.GetValue(1).ToString()),
                        loginDate = Convert.ToDateTime(reader.GetValue(2)),
                        logoutDate = reader.GetValue(3) == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(3))

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

        public void InsertLoginDetailsAsync(LoginDetails ln)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_LoginInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@ID", ln.ID));
            sqlCmd.Parameters.Add(new SqlParameter("@UserId", ln.userId));
            sqlCmd.Parameters.Add(new SqlParameter("@LoginDate", ln.loginDate));
            sqlCmd.Parameters.Add(new SqlParameter("@LogoutDate", ln.logoutDate));
            
             
            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                ln.ID = Convert.ToInt32(sqlCmd.ExecuteScalar());

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

        public List<LoginDetails> UpdateLoginDetailsAsync(LoginDetails ln)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_LoginInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@ID", ln.ID));
            sqlCmd.Parameters.Add(new SqlParameter("@UserId", ln.userId));
            sqlCmd.Parameters.Add(new SqlParameter("@LoginDate", ln.loginDate));
            sqlCmd.Parameters.Add(new SqlParameter("@LogoutDate", ln.logoutDate));

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
            return GetLoginDetailsAsync(0);
        }

        public List<LoginDetails> DeleteLoginDetailsAsync(int ID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_LoginDelete";
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
            return GetLoginDetailsAsync(0);
        }
    }
}