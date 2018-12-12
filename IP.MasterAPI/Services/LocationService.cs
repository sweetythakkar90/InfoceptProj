using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.MasterAPI.Services
{
    public class LocationService : ILocationRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public LocationService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<Location> GetLocationDetailsAsync(int ID)
        {
            try
            {
                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_LocationGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<Location> lst = new List<Location>();
                while (reader.Read())
                {

                    lst.Add(new Location()
                    {
                        ID = Convert.ToInt32(reader.GetValue(0)),
                        name = reader.GetValue(1).ToString(),
                        streetNo = reader.GetValue(2).ToString(),
                        street = reader.GetValue(3).ToString(),
                        suburb = reader.GetValue(4).ToString(),
                        state = reader.GetValue(5).ToString(),
                        pincode = reader.GetValue(6).ToString(),
                        createdDate = Convert.ToDateTime(reader.GetValue(7)),
                        modifiedDate = reader.GetValue(8) == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(8)),
                        userId = Convert.ToInt32(reader.GetValue(9))


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

        public void InsertLocationDetailsAsync(Location loc)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            loc.createdDate = DateTime.Now;
            loc.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_LocationInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@ID", loc.ID));
            sqlCmd.Parameters.Add(new SqlParameter("@name", loc.name));
            sqlCmd.Parameters.Add(new SqlParameter("@streetNo", loc.streetNo == null ? "" : loc.streetNo));
            sqlCmd.Parameters.Add(new SqlParameter("@street", loc.street == null ? "" : loc.street));
            sqlCmd.Parameters.Add(new SqlParameter("@suburb", loc.suburb == null ? "" : loc.suburb));
            sqlCmd.Parameters.Add(new SqlParameter("@state", loc.state == null ? "" : loc.state));
            sqlCmd.Parameters.Add(new SqlParameter("@pincode", loc.pincode == null ? "" : loc.pincode));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", loc.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", loc.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", loc.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                loc.ID = Convert.ToInt32(sqlCmd.ExecuteScalar());

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
        public List<Location> UpdateLocationDetailsAsync(Location loc)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            loc.createdDate = DateTime.Now;
            loc.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_LocationInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@ID", loc.ID));
            sqlCmd.Parameters.Add(new SqlParameter("@name", loc.name));
            sqlCmd.Parameters.Add(new SqlParameter("@streetNo", loc.streetNo == null ? "" : loc.streetNo));
            sqlCmd.Parameters.Add(new SqlParameter("@street", loc.street == null ? "" : loc.street));
            sqlCmd.Parameters.Add(new SqlParameter("@suburb", loc.suburb == null ? "" : loc.suburb));
            sqlCmd.Parameters.Add(new SqlParameter("@state", loc.state == null ? "" : loc.state));
            sqlCmd.Parameters.Add(new SqlParameter("@pincode", loc.pincode == null ? "" : loc.pincode));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", loc.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", loc.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", loc.userId));
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
            return GetLocationDetailsAsync(0);
        }

        public List<Location> DeleteLocationDetailsAsync(int ID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_LocationDelete";
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
            return GetLocationDetailsAsync(0);
        }
    }
}