using IP.JobsAPI.Models;
using IP.JobsAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.JobsAPI.Services
{
    public class GlobalService
    {
        private SqlConnection myconn;
        
        public GlobalService()
        {
            DBService dsc = DBService.GetSqlInstance();
            myconn = dsc.GetDBConnection();
        }
        public int CheckDataIntegrity(string tableName, string fieldName,  int fieldValue,string ignoreTableList) 
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();
          
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_CheckDataIntegrity";
            sqlCmd.Parameters.Add(new SqlParameter("@tableName", tableName));
            sqlCmd.Parameters.Add(new SqlParameter("@fieldName", fieldName));
            sqlCmd.Parameters.Add(new SqlParameter("@fieldValue", fieldValue));
            sqlCmd.Parameters.Add(new SqlParameter("@ignoreTableList", ignoreTableList));
            sqlCmd.Connection = myconn;
            int counter = Convert.ToInt16(sqlCmd.ExecuteScalar());
            
            if (myconn.State != ConnectionState.Closed)
                myconn.Close();


            return counter;
        }
        public DataTable GetQueryResult(string query)
        {
            DataTable dtDetails = new DataTable();
            SqlDataAdapter da;
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_GetQueryResult";
            sqlCmd.Parameters.Add(new SqlParameter("@sqlstr", query));
            sqlCmd.Connection = myconn;
            da = new SqlDataAdapter(sqlCmd);
            dtDetails.Clear();
            da.Fill(dtDetails);

            if (myconn.State != ConnectionState.Closed)
                myconn.Close();
            return dtDetails;
        }

    }
}