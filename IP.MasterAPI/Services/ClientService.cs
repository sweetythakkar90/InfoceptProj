using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.MasterAPI.Services
{
    public class ClientService : IClientRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;

        private StatusTypeService stypeService;

        public ClientService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            stypeService = new StatusTypeService();
            myconn = dsc.GetDBConnection();
        }

        public List<Client> GetClientDetailsAsync(int Id)
        {
            try
            {

                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_ClientGet";
                sqlCmd.Parameters.Add(new SqlParameter("@Id", Id));
                

                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<Client> lst = new List<Client>();
                if (!reader.HasRows)
                {
                    lst.Add(new Client());
                    lst[0].statusType = stypeService.GetStatusTypeDetailsAsync(0);

                }
                else
                {
                    while (reader.Read())
                    {

                        lst.Add(new Client()
                        {
                            Id = Convert.ToInt32(reader.GetValue(0)),
                            clientName = reader.GetValue(1).ToString(),
                            companyId = Convert.ToInt32(reader.GetValue(2)),
                            companyName = reader.GetValue(3).ToString(),
                            email = reader.GetValue(4).ToString(),
                            address = reader.GetValue(5).ToString(),
                            phoneNumber = reader.GetValue(6).ToString(),
                            createdDate = Convert.ToDateTime(reader.GetValue(7)),
                            modifiedDate = reader.GetValue(8) == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader.GetValue(8)),
                            statusId = Convert.ToInt32(reader.GetValue(9)),
                            statusName = reader.GetValue(10).ToString(),
                            userId = Convert.ToInt32(reader.GetValue(11)),
                          

                    });
                    }
                    lst[0].statusType = stypeService.GetStatusTypeDetailsAsync(0);
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
        public void InsertClientDetailsAsync(Client client)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            client.createdDate = DateTime.Now;
            client.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ClientInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", client.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@companyId", client.companyId));
            sqlCmd.Parameters.Add(new SqlParameter("@clientName", client.clientName));
            sqlCmd.Parameters.Add(new SqlParameter("@email", client.email == null ? "" : client.email));
            sqlCmd.Parameters.Add(new SqlParameter("@address", client.address == null ? "" : client.address));
            sqlCmd.Parameters.Add(new SqlParameter("@phoneNumber", client.phoneNumber == null ? "" : client.phoneNumber));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", client.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", client.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", client.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", client.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                client.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

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
        public List<Client> UpdateClientDetailsAsync(Client client)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();
            client.createdDate = DateTime.Now;
            client.modifiedDate = DateTime.Now;


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ClientInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", client.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@companyId", client.companyId));
            sqlCmd.Parameters.Add(new SqlParameter("@clientName", client.clientName));
            sqlCmd.Parameters.Add(new SqlParameter("@email", client.email == null ? "" : client.email));
            sqlCmd.Parameters.Add(new SqlParameter("@address", client.address == null ? "" : client.address));
            sqlCmd.Parameters.Add(new SqlParameter("@phoneNumber", client.phoneNumber == null ? "" : client.phoneNumber));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", client.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", client.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", client.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", client.userId));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();

                //  UpdateClientRatesDetailsAsync(clientRates, tran);
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
            return GetClientDetailsAsync(0);
        }

        public List<Client> DeleteClientDetailsAsync(int Id)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ClientDelete";
            sqlCmd.Parameters.Add(new SqlParameter("@Id", Id));

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
            return GetClientDetailsAsync(0);
        }
    }
}