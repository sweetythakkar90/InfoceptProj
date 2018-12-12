using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.MasterAPI.Services
{
    public class CompanyService : ICompanyRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public CompanyService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<Company> GetCompanyDetailsAsync(int companyID) 
        {
            try
            {
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_CompanyGet";
                sqlCmd.Parameters.Add(new SqlParameter("@companyID", companyID));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<Company> lst = new List<Company>();
                while (reader.Read())
                {
                    lst.Add(new Company
                    {
                        companyID = Convert.ToInt32(reader.GetValue(0)),
                        companyName = reader.GetValue(1).ToString(),
                        email = reader.GetValue(2).ToString(),
                        phoneNumber = reader.GetValue(3).ToString(),
                        createdDate = Convert.ToDateTime(reader.GetValue(4)),
                        modifiedDate = reader.GetValue(5) == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(5)),
                        statusID = Convert.ToInt32(reader.GetValue(6))
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

        public void InsertCompanyDetailsAsync(Company comp)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            comp.createdDate = DateTime.Now;
            comp.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_CompanyInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@companyID", comp.companyID));
            sqlCmd.Parameters.Add(new SqlParameter("@companyName", comp.companyName));
            sqlCmd.Parameters.Add(new SqlParameter("@email", comp.email));
            sqlCmd.Parameters.Add(new SqlParameter("@phoneNumber", comp.phoneNumber));
            sqlCmd.Parameters.Add(new SqlParameter("@statusID", comp.statusID));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", comp.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", comp.modifiedDate));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                comp.companyID = Convert.ToInt32(sqlCmd.ExecuteScalar());

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

        public List<Company> UpdateCompanyDetailsAsync(Company comp)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            comp.createdDate = DateTime.Now;
            comp.modifiedDate = DateTime.Now;


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_CompanyInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@companyID", comp.companyID));
            sqlCmd.Parameters.Add(new SqlParameter("@companyName", comp.companyName));
            sqlCmd.Parameters.Add(new SqlParameter("@email", comp.email));
            sqlCmd.Parameters.Add(new SqlParameter("@phoneNumber", comp.phoneNumber));
            sqlCmd.Parameters.Add(new SqlParameter("@statusID", comp.statusID));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", comp.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", comp.modifiedDate));

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
            return GetCompanyDetailsAsync(0);
        }

        public List<Company> DeleteCompanyDetailsAsync(int companyID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_CompanyDelete";
            sqlCmd.Parameters.Add(new SqlParameter("@companyID", companyID));
            
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
            return GetCompanyDetailsAsync(0);
        }
    }
}