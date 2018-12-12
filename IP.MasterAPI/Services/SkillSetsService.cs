using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.MasterAPI.Services
{
    public class SkillSetsService : ISkillSetsRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public SkillSetsService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<SkillSets> GetSkillSetsDetailsAsync(int ID) 
        {
            try
            {
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_SkillSetsGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<SkillSets> lst = new List<SkillSets>();
                while (reader.Read())
                {

                    lst.Add(new SkillSets()
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

        public void InsertSkillSetsDetailsAsync(SkillSets SkillSets)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SkillSets.createdDate = DateTime.Now;
            SkillSets.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SkillSetsInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", SkillSets.ID));
            sqlCmd.Parameters.Add(new SqlParameter("@name", SkillSets.name));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", SkillSets.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", SkillSets.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", SkillSets.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                SkillSets.ID = Convert.ToInt32(sqlCmd.ExecuteScalar());

        
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

        public List<SkillSets> UpdateSkillSetsDetailsAsync(SkillSets SkillSets)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SkillSets.createdDate = DateTime.Now;
            SkillSets.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SkillSetsInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", SkillSets.ID));
            sqlCmd.Parameters.Add(new SqlParameter("@name", SkillSets.name));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", SkillSets.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", SkillSets.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", SkillSets.userId));

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
            return GetSkillSetsDetailsAsync(0);
        }

        public List<SkillSets> DeleteSkillSetsDetailsAsync(int ID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SkillSetsDelete";
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
            return GetSkillSetsDetailsAsync(0);
        }
    }
}