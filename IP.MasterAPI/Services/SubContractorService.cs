using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;

namespace IP.MasterAPI.Services
{
    public class SubContractorService : ISubContractorRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;

        public SubContractorService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<SubContractor> GetSubContractorDetailsAsync(int Id, char mode)
        {
            try
            {
                DataTable dt = new DataTable();
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_SubContractorGet";
                sqlCmd.Parameters.Add(new SqlParameter("@Id", Id));

                sqlCmd.Connection = myconn;
                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                dt.Clear();
                da.Fill(dt);
                List<SubContractor> lst = new List<SubContractor>();
                if (mode == 'I')
                {
                    if (dt.Rows.Count == 0)
                    {
                        lst.Add(new SubContractor());
                            lst[0].skillSelect = GetSubContractorSkillSetsDetailsAsync(0);
                    }
                    else
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            lst.Add(new SubContractor
                            {
                                Id = Convert.ToInt32(item[0]),
                               subconName = item[1].ToString(),
                                email = item[2].ToString(),
                                address = item[3].ToString(),
                                phoneNumber = item[4].ToString(),
                                statusId = Convert.ToInt32(item[5]),
                                statusName = item[6].ToString(),
                                createdDate = Convert.ToDateTime(item[7]),
                                modifiedDate = item[8] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[8]),
                                userId = Convert.ToInt32(item[9])


                            });
                        }
                        lst[0].skillSelect = GetSubContractorSkillSetsDetailsAsync(0);
                    }
                   
                }
                else
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        lst.Add(new SubContractor
                        {
                            Id = Convert.ToInt32(item[0]),
                            subconName = item[1].ToString(),
                            email = item[2].ToString(),
                            address = item[3].ToString(),
                            phoneNumber = item[4].ToString(),
                            statusId = Convert.ToInt32(item[5]),
                            statusName = item[6].ToString(),
                            createdDate = Convert.ToDateTime(item[7]),
                            modifiedDate = item[8] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[8]),
                            userId = Convert.ToInt32(item[9])
                        });

                    }
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

        private List<SubContractorSkillSetMapping> GetSubContractorSkillSetsDetailsAsync(int subcontractorID)
        {

            DataTable dt = new DataTable();
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SubContractorSkillSetMappingGet";
            sqlCmd.Parameters.Add(new SqlParameter("@subcontractorID", subcontractorID));

            sqlCmd.Connection = myconn;
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
            dt.Clear();
            da.Fill(dt);
            List<SubContractorSkillSetMapping> lst = new List<SubContractorSkillSetMapping>();
            foreach (DataRow item in dt.Rows)
            {

                lst.Add(new SubContractorSkillSetMapping()
                {
                    Id = Convert.ToInt32(item[0]),
                    subcontractorID = Convert.ToInt32(item[1]),
                    skillSetId= Convert.ToInt32(item[2]),
                    skillSetName = item[3].ToString()
                });
            }

            if (myconn.State != ConnectionState.Closed)
                myconn.Close();
            return lst;
        }

        public void InsertSubContractorDetailsAsync(SubContractor sc, List<SubContractorSkillSetMapping> scSkillSets)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            sc.createdDate = DateTime.Now;
            sc.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SubContractorInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", sc.Id));
           sqlCmd.Parameters.Add(new SqlParameter("@subconName", sc.subconName));
            sqlCmd.Parameters.Add(new SqlParameter("@email", sc.email));
            sqlCmd.Parameters.Add(new SqlParameter("@address", sc.address));
            sqlCmd.Parameters.Add(new SqlParameter("@phoneNumber", sc.phoneNumber));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", sc.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", sc.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", sc.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", sc.userId));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sc.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if (scSkillSets != null)
                {
                    foreach (SubContractorSkillSetMapping item in scSkillSets)
                    {
                        InsertSubContractorSkillSetsDetailsAsync(item, sc.Id, tran);
                    }
                }
                //InsertSubContractorRatesAsync(scRate, sc.subcontractorID, tran);
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
        private void InsertSubContractorSkillSetsDetailsAsync(SubContractorSkillSetMapping scSkillSets, int ID, SqlTransaction tran)
        {

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SubContractorSkillSetMappingInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", scSkillSets.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@subcontractorID", ID));
            sqlCmd.Parameters.Add(new SqlParameter("@skillSetId", scSkillSets.skillSetId));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;
                scSkillSets.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

            }
            catch (Exception)
            {
                throw;
            }

        }
        public List<SubContractor> UpdateSubContractorDetailsAsync(SubContractor sc, List<SubContractorSkillSetMapping> scSkillSets)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            sc.createdDate = DateTime.Now;
            sc.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SubContractorInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", sc.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@subconName", sc.subconName));
            sqlCmd.Parameters.Add(new SqlParameter("@email", sc.email));
            sqlCmd.Parameters.Add(new SqlParameter("@address", sc.address));
            sqlCmd.Parameters.Add(new SqlParameter("@phoneNumber", sc.phoneNumber));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", sc.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", sc.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", sc.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", sc.userId));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();
                if (scSkillSets != null)
                {
                    foreach (SubContractorSkillSetMapping item in scSkillSets)
                    {
                        UpdateSubContractorSkillSetsDetailsAsync(item, sc.Id, tran);
                    }
                }

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
            return GetSubContractorDetailsAsync(0, 'I');
        }
      
        private void UpdateSubContractorSkillSetsDetailsAsync(SubContractorSkillSetMapping scSkillSets, int ID, SqlTransaction tran)
        {

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SubContractorSkillSetMappingInsertUpdate";
           
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
                sqlCmd.Parameters.Add(new SqlParameter("@Id", scSkillSets.Id));
                sqlCmd.Parameters.Add(new SqlParameter("@subcontractorID", ID));
                sqlCmd.Parameters.Add(new SqlParameter("@skillSetId", scSkillSets.skillSetId));

                try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;
                scSkillSets.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

            }
            catch (Exception)
            {
                throw;
            }

        }
        public List<SubContractor> DeleteSubContractorDetailsAsync(int subcontractorID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SubContractorDelete";
            sqlCmd.Parameters.Add(new SqlParameter("@Id", subcontractorID));

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
            return GetSubContractorDetailsAsync(0, 'I');
        }

        public List<SubContractor> DeleteSubContractorSkillSetsDetailsAsync(int subconSkillMappingID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SubContractorSkillSetMappingDelete";
            sqlCmd.Parameters.Add(new SqlParameter("@subconSkillMappingID", subconSkillMappingID));

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
            return GetSubContractorDetailsAsync(0, 'I');
        }

        public List<SubContractor> DeleteSubContractorRatesAsync(int subConRateID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_SubContractorRatesDelete";
            sqlCmd.Parameters.Add(new SqlParameter("@subConRateID", subConRateID));

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
            return GetSubContractorDetailsAsync(0, 'I');
        }
    }
}