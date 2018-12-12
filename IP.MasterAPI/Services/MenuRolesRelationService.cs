using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.MasterAPI.Services
{
    public class MenuRolesRelationService : IMenuRolesRelationRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public MenuRolesRelationService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<MenuRolesRelation> GetMenuRolesRelationDetailsAsync(int ID, int roleId)
        {
            try
            {
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_MenuRolesRelationGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));
                sqlCmd.Parameters.Add(new SqlParameter("@roleId", roleId));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<MenuRolesRelation> lst = new List<MenuRolesRelation>();
                while (reader.Read())
                {
                    lst.Add(new MenuRolesRelation()
                    {
                        Id = Convert.ToInt32(reader.GetValue(0)),
                        menuId = Convert.ToInt32(reader.GetValue(1)),
                        menuName = reader.GetValue(2).ToString(),
                        parentId = Convert.ToInt32(reader.GetValue(3)),
                        parentMenuName = reader.GetValue(4).ToString(),
                        roleId = Convert.ToInt32(reader.GetValue(5)),
                        roles = reader.GetValue(6).ToString(),
                        userRights = reader.GetValue(7).ToString(),
                        createdDate = Convert.ToDateTime(reader.GetValue(8)),
                        modifiedDate = reader.GetValue(9) == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(9)),
                        userId = Convert.ToInt32(reader.GetValue(10))
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
        public void InsertMenuRolesRelationDetailsAsync(MenuRolesRelation relation)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            relation.createdDate = DateTime.Now;
            relation.modifiedDate = DateTime.Now;
            relation.userRights = "";

            SqlCommand sqlCmd = new SqlCommand();

            DeleteMenuRolesRelationDetailsAsync(relation.roleId, tran);



            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_MenuRolesRelationInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", relation.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@menuId", relation.menuId));
            sqlCmd.Parameters.Add(new SqlParameter("@roleId", relation.roleId));
            sqlCmd.Parameters.Add(new SqlParameter("@userRights", relation.userRights));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", relation.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", relation.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", relation.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                relation.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

               // InsertMenuRolesRelationRateDetailsAsync(MenuRolesRelationRates, MenuRolesRelation.MenuRolesRelationID, tran);
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
        public void UpdateMenuRolesRelationDetailsAsync(MenuRolesRelation relation)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();
            relation.createdDate = DateTime.Now;
            relation.modifiedDate = DateTime.Now;


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_MenuRolesRelationInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", relation.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@menuId", relation.menuId));
            sqlCmd.Parameters.Add(new SqlParameter("@roleId", relation.roleId));
            sqlCmd.Parameters.Add(new SqlParameter("@userRights", relation.userRights));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", relation.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", relation.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", relation.userId));
            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();

              //  UpdateMenuRolesRelationRatesDetailsAsync(MenuRolesRelationRates, tran);
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

        public void DeleteMenuRolesRelationDetailsAsync(int ID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_MenuRolesRelationDelete";
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
        public void DeleteMenuRolesRelationDetailsAsync(int roleId, SqlTransaction tran)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_MenuRolesRelationBasedOnRolesDelete";
            sqlCmd.Parameters.Add(new SqlParameter("@roleId", roleId));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();

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