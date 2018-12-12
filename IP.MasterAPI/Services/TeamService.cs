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
    public class TeamService : ITeamRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;

        public TeamService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }

        public List<Team> GetTeamDetailsAsync(int Id, char mode)
        {
            try
            {
                DataTable dt = new DataTable();
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_TeamGet";
                sqlCmd.Parameters.Add(new SqlParameter("@Id", Id));

                sqlCmd.Connection = myconn;
                SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                dt.Clear();
                da.Fill(dt);
                List<Team> lst = new List<Team>();
                if (mode == 'I')
                {
                    if (dt.Rows.Count == 0)
                    {
                        lst.Add(new Team());
                        lst[0].memberMapping = GetTeamMemberMappingDetailsAsync(0);
                    }
                    else
                    {
                        foreach (DataRow item in dt.Rows)
                        {

                            lst.Add(new Team
                            {

                                Id = Convert.ToInt32(item[0]),
                                teamName = item[1].ToString(),
                                statusId = Convert.ToInt32(item[2]),
                                statusName = item[3].ToString(),
                                createdDate = Convert.ToDateTime(item[4]),
                                modifiedDate = item[5] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[5]),
                                userId = Convert.ToInt32(item[6])
                            });
                        }
                        lst[0].memberMapping = GetTeamMemberMappingDetailsAsync(0);
                    }
                }
                else
                {
                    foreach (DataRow item in dt.Rows)
                    {

                        lst.Add(new Team
                        {


                            Id = Convert.ToInt32(item[0]),
                            teamName = item[1].ToString(),
                            statusId = Convert.ToInt32(item[2]),
                            statusName = item[3].ToString(),
                            createdDate = Convert.ToDateTime(item[4]),
                            modifiedDate = item[5] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[5]),
                            userId = Convert.ToInt32(item[6]),
                            memberMapping = GetTeamMemberMappingDetailsAsync(Convert.ToInt32(item[0]))

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
        private List<TeamMembersMapping> GetTeamMemberMappingDetailsAsync(int teamId)
        {
            DataTable dt = new DataTable();
            //if (myconn.State != ConnectionState.Open)
            //    myconn.Open();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_TeamMembersMappingGet";
            sqlCmd.Parameters.Add(new SqlParameter("@teamId", teamId));

            sqlCmd.Connection = myconn;
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
            dt.Clear();
            da.Fill(dt);

            List<TeamMembersMapping> lst = new List<TeamMembersMapping>();
            foreach (DataRow item in dt.Rows)
            {
                lst.Add(new TeamMembersMapping()
                {
                    Id = Convert.ToInt32(item[0]),
                    teamID = Convert.ToInt32(item[1]),
                    memberId= Convert.ToInt32(item[2]),
                    memberName= item[3].ToString()

                });
            }


            //if (myconn.State != ConnectionState.Closed)
            //    myconn.Close();
            return lst;
        }
        public void InsertTeamDetailsAsync(Team tm, List<TeamMembersMapping> membersMappings)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            tm.createdDate = DateTime.Now;
            tm.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_TeamInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", tm.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@teamName", tm.teamName));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", tm.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", tm.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", tm.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", tm.userId));
            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                tm.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if (membersMappings != null)
                {
                    foreach (TeamMembersMapping item in membersMappings)
                    {
                        InsertTeamMemberMappingDetailsAsync(item, tm.Id, tran);
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
        private void InsertTeamMemberMappingDetailsAsync(TeamMembersMapping membersMapping, int ID, SqlTransaction tran)
        {

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_TeamMembersMappingInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", membersMapping.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@teamID", ID));
            sqlCmd.Parameters.Add(new SqlParameter("@memberId", membersMapping.memberId));

            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;
                membersMapping.memberId = Convert.ToInt32(sqlCmd.ExecuteScalar());

            }
            catch (Exception)
            {
                throw;
            }

        }
        public List<Team> UpdateTeamDetailsAsync(Team tm, List<TeamMembersMapping> membersMappings)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            tm.createdDate = DateTime.Now;
            tm.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_TeamInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", tm.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@teamName", tm.teamName));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", tm.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", tm.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", tm.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", tm.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();
                if (membersMappings != null)
                {
                    foreach (TeamMembersMapping item in membersMappings)
                    {
                        UpdateTeamMemberMappingDetailsAsync(item, tm.Id, tran);
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
            return GetTeamDetailsAsync(0, 'I');
        }
        private void UpdateTeamMemberMappingDetailsAsync(TeamMembersMapping membersMapping,int ID,  SqlTransaction tran)
        {

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_TeamMembersMappingInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", membersMapping.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@teamID", ID));
            sqlCmd.Parameters.Add(new SqlParameter("@memberId", membersMapping.memberId));



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
        public List<Team> DeleteTeamDetailsAsync(int Id)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_TeamDelete";
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
            return GetTeamDetailsAsync(0, 'I');
        }

        public List<Team> DeleteTeamMemberMappingDetailsAsync(int Id)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_TeamMembersMappingDelete";
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
            return GetTeamDetailsAsync(0, 'I');
        }
    }
}