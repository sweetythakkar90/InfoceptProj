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
    public class RolesService : IRolesRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        private MenuRolesRelationService mrr;
        public RolesService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();

            mrr = new MenuRolesRelationService();
            myconn = dsc.GetDBConnection();
        }
        public List<Roles> GetRolesDetailsAsync(int RolesID)
        {
            try
            {


                DataTable dtRoles = new DataTable();
                SqlDataAdapter da;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_RolesGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", RolesID));

                sqlCmd.Connection = myconn;
                da = new SqlDataAdapter(sqlCmd);
                dtRoles.Clear();
                da.Fill(dtRoles);
                List<Roles> lst = new List<Roles>();

                foreach (DataRow item in dtRoles.Rows)
                {

                    lst.Add(new Roles()
                    {
                        Id = Convert.ToInt32(item[0]),
                        rolesName = item[1].ToString(),
                        createdDate = Convert.ToDateTime(item[2]),
                        modifiedDate = item[3] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[3]),
                        userId = Convert.ToInt32(item[4]),
                        menuRolesList = mrr.GetMenuRolesRelationDetailsAsync(0, Convert.ToInt32(item[0]))

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
        public void InsertRolesDetailsAsync(Roles roles)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            roles.createdDate = DateTime.Now;
            roles.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_RolesInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", roles.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@roles", roles.rolesName));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", roles.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", roles.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", roles.userId));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                roles.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());


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
        public List<Roles> UpdateRolesDetailsAsync(Roles roles)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            roles.createdDate = DateTime.Now;
            roles.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_RolesInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", roles.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@roles", roles.rolesName));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", roles.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", roles.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", roles.userId));

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
            return GetRolesDetailsAsync(0);
        }

        public List<Roles> DeleteRolesDetailsAsync(int RolesID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_RolesDelete";
            sqlCmd.Parameters.Add(new SqlParameter("@ID", RolesID));

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
            return GetRolesDetailsAsync(0);
        }
    }
}