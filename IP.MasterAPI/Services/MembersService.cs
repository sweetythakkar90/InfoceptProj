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
    public class MembersService : IMembersRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;

        public MembersService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<Members> GetMembersDetailsAsync(int membersID, char mode)
        {
            try
            {
                DataTable dt = new DataTable();
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_MembersGet";
                sqlCmd.Parameters.Add(new SqlParameter("@Id", membersID));

                sqlCmd.Connection = myconn;
                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                dt.Clear();
                da.Fill(dt);
                List<Members> lst = new List<Members>();
                if (mode == 'I')
                {
                    if (dt.Rows.Count == 0)
                    {
                        lst.Add(new Members());
                        lst[0].skillSelect = GetMembersSkillSetsDetailsAsync(0);
                    }
                    else
                    {
                        foreach (DataRow item in dt.Rows)
                        {

                            lst.Add(new Members
                            {
                                Id = Convert.ToInt32(item[0]),
                                firstName = item[1].ToString(),
                                lastName = item[2].ToString(),
                                email = item[3].ToString(),
                                phoneNumber = item[4].ToString(),
                                address = item[5].ToString(),
                                isTeamLead = Convert.ToBoolean(item[6]),
                                lastLoginDate = item[7] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[7]),
                                statusId = Convert.ToInt32(item[8]),
                                statusName = item[9].ToString(),
                                createdDate = Convert.ToDateTime(item[10]),
                                modifiedDate = item[11] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[11]),
                                userId = Convert.ToInt32(item[12])



                            });
                        }
                        lst[0].skillSelect = GetMembersSkillSetsDetailsAsync(0);
                    }
                    

                }
                else
                {
                    foreach (DataRow item in dt.Rows)
                    {

                        lst.Add(new Members
                        {

                            Id = Convert.ToInt32(item[0]),
                            firstName = item[1].ToString(),
                            lastName = item[2].ToString(),
                            email = item[3].ToString(),
                            phoneNumber = item[4].ToString(),
                            address = item[5].ToString(),
                            isTeamLead = Convert.ToBoolean(item[6]),
                            lastLoginDate = item[7] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[7]),
                            statusId = Convert.ToInt32(item[8]),
                            statusName = item[9].ToString(),
                            createdDate = Convert.ToDateTime(item[10]),
                            modifiedDate = item[11] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[11]),
                            userId = Convert.ToInt32(item[12]),
                            skillSelect = GetMembersSkillSetsDetailsAsync(Convert.ToInt32(item[0]))

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
        private List<MembersSkillSetMapping> GetMembersSkillSetsDetailsAsync(int memberId)
        {
            DataTable dt = new DataTable();
            //if (myconn.State != ConnectionState.Open)
            //    myconn.Open();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_MembersSkillSetMappingGet";
            sqlCmd.Parameters.Add(new SqlParameter("@memberId", memberId));

            sqlCmd.Connection = myconn;
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
            dt.Clear();
            da.Fill(dt);

            List<MembersSkillSetMapping> lst = new List<MembersSkillSetMapping>();
            foreach (DataRow item in dt.Rows)
            {
                lst.Add(new MembersSkillSetMapping()
                {
                    Id = Convert.ToInt32(item[0]),
                    memberId = Convert.ToInt32(item[1]),
                    skillSetId= Convert.ToInt32(item[2]),
                    skillSetName = item[3].ToString()

                });
            }


            //if (myconn.State != ConnectionState.Closed)
            //    myconn.Close();
            return lst;
        }
        public void InsertMembersDetailsAsync(Members mem, List<MembersSkillSetMapping> memberSkillSets)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            mem.createdDate = DateTime.Now;
            mem.modifiedDate = DateTime.Now;
            
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_MembersInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", mem.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@firstName", mem.firstName));
            sqlCmd.Parameters.Add(new SqlParameter("@lastName", mem.lastName));
            sqlCmd.Parameters.Add(new SqlParameter("@email", mem.email));
            sqlCmd.Parameters.Add(new SqlParameter("@phoneNumber", mem.phoneNumber));
            sqlCmd.Parameters.Add(new SqlParameter("@address", mem.address));
            sqlCmd.Parameters.Add(new SqlParameter("@isTeamLead", mem.isTeamLead));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", mem.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", mem.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", mem.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", mem.userId));
            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                mem.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if (memberSkillSets != null)
                {
                    foreach (MembersSkillSetMapping item in memberSkillSets)
                    {
                        InsertMembersSkillSetsDetailsAsync(item, mem.Id, tran);
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

        }
        private void InsertMembersSkillSetsDetailsAsync(MembersSkillSetMapping memberSkillSets, int ID, SqlTransaction tran)
        {

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_MembersSkillSetMappingInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", memberSkillSets.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@memberId", ID));
            sqlCmd.Parameters.Add(new SqlParameter("@skillSetId", memberSkillSets.skillSetId));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;
                memberSkillSets.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

            }
            catch (Exception)
            {
                throw;
            }

        }
        public List<Members> UpdateMembersDetailsAsync(Members mem, List<MembersSkillSetMapping> memberSkillSets)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            mem.createdDate = DateTime.Now;
            mem.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_MembersInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", mem.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@firstName", mem.firstName));
            sqlCmd.Parameters.Add(new SqlParameter("@lastName", mem.lastName));
            sqlCmd.Parameters.Add(new SqlParameter("@email", mem.email));
            sqlCmd.Parameters.Add(new SqlParameter("@phoneNumber", mem.phoneNumber));
            sqlCmd.Parameters.Add(new SqlParameter("@address", mem.address));
            sqlCmd.Parameters.Add(new SqlParameter("@isTeamLead", mem.isTeamLead));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", mem.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", mem.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", mem.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", mem.userId));
            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();
                if (memberSkillSets != null)
                {
                    foreach (MembersSkillSetMapping item in memberSkillSets)
                    {
                        UpdateMembersSkillSetsDetailsAsync(item, mem.Id, tran);
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
            return GetMembersDetailsAsync(0, 'I');
        }
        private void UpdateMembersSkillSetsDetailsAsync(MembersSkillSetMapping memberSkillSets, int ID, SqlTransaction tran)
        {

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_MembersSkillSetMappingInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", ID));
            sqlCmd.Parameters.Add(new SqlParameter("@memberId", memberSkillSets.memberId));
            sqlCmd.Parameters.Add(new SqlParameter("@skillSetId", memberSkillSets.skillSetId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();

            }
            catch (Exception)
            {
                throw;
            }

        }
        public List<Members> DeleteMembersDetailsAsync(int Id)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_MembersDelete";
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
            return GetMembersDetailsAsync(0, 'I');
        }

        public List<Members> DeleteMembersSkillSetsDetailsAsync(int Id)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_MembersSkillSetMappingDelete";
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
            return GetMembersDetailsAsync(0, 'I');
        }
    }
}