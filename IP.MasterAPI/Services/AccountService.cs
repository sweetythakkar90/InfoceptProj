using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.MasterAPI.Services
{
    public class AccountService : IAccountRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        private StatusTypeService stypeService;
        private MembersService mService;
        private SubContractorService scService;
        private MenuService mnuService;
        public AccountService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            stypeService = new StatusTypeService();
            mService = new MembersService();
            scService = new SubContractorService();
            mnuService = new MenuService();
            myconn = dsc.GetDBConnection();
        }
        public Account GetAuthenticateUser(string uName, string pwd)
        {
            SqlDataReader reader = null;
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_GetAuthenticateUser";
            sqlCmd.Parameters.Add(new SqlParameter("@userName", gs.CipherText(uName, "E")));
            sqlCmd.Parameters.Add(new SqlParameter("@password", gs.CipherText(pwd, "E")));
            sqlCmd.Connection = myconn;
            reader = sqlCmd.ExecuteReader();
            Account acct = new Account();
            while (reader.Read())
            {
                acct.Id = Convert.ToInt32(reader.GetValue(0));
                acct.uId = Convert.ToInt32(reader.GetValue(1));
                acct.userName = uName;
                acct.password = pwd;
                acct.userType = reader.GetValue(2).ToString();
                acct.roleId = Convert.ToInt32(reader.GetValue(3));
                acct.roles = reader.GetValue(4).ToString();
            }

            if (myconn.State != ConnectionState.Closed)
                myconn.Close();


            return acct;
        }
        public List<Roles> GetRoles(int Id)
        {
            try
            {

                SqlDataReader reader = null;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_RolesGet";
                sqlCmd.Parameters.Add(new SqlParameter("@Id", Id));


                sqlCmd.Connection = myconn;
                reader = sqlCmd.ExecuteReader();
                List<Roles> lst = new List<Roles>();

                while (reader.Read())
                {

                    lst.Add(new Roles()
                    {
                        Id = Convert.ToInt32(reader.GetValue(0)),
                        rolesName = reader.GetValue(1).ToString()
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
        public List<Account> GetUserDetailsAsync(int Id,int roleId)
        {
            try
            {

                DataTable dtUsers = new DataTable();
                SqlDataAdapter daUsers;
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_UserGet";
                sqlCmd.Parameters.Add(new SqlParameter("@Id", Id));


                sqlCmd.Connection = myconn;
                daUsers = new SqlDataAdapter(sqlCmd);
                dtUsers.Clear();
                daUsers.Fill(dtUsers);
                List<Account> lst = new List<Account>();
                if (dtUsers.Rows.Count ==0)
                {
                    lst.Add(new Account());
                    lst[0].statusTypeList = stypeService.GetStatusTypeDetailsAsync(0);
                    lst[0].membersList = mService.GetMembersDetailsAsync(0, 'U');
                    lst[0].scList = scService.GetSubContractorDetailsAsync(0, 'U');
                    lst[0].rolesList = GetRoles(0);
                    lst[0].menuList = mnuService.GetMenuDetails(roleId, 'S');
                }
                else
                {
                    foreach (DataRow item in dtUsers.Rows)
                    {

                        lst.Add(new Account()
                        {
                            Id = Convert.ToInt32(item[0]),
                            userName = gs.CipherText(item[1].ToString(), "D"),
                            password = gs.CipherText(item[2].ToString(), "D"),
                            memberId = Convert.ToInt32(item[3]),
                            memberName = item[4].ToString(),
                            subcontractorId = Convert.ToInt32(item[5]),
                            subcontractorName = item[6].ToString(),
                            roleId = Convert.ToInt32(item[7]),
                            roles = item[8].ToString(),
                            statusId = Convert.ToInt32(item[9]),
                            statusName = item[10].ToString(),
                            userType = item[11].ToString(),
                            createdDate = Convert.ToDateTime(item[12]),
                            modifiedDate = item[13] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[13]),
                            userId = Convert.ToInt32(item[14])
                        });
                    }
                    lst[0].statusTypeList = stypeService.GetStatusTypeDetailsAsync(0);
                    lst[0].membersList = mService.GetMembersDetailsAsync(0, 'U');
                    lst[0].scList = scService.GetSubContractorDetailsAsync(0, 'U');
                    lst[0].rolesList = GetRoles(0);
                    lst[0].menuList = mnuService.GetMenuDetails(roleId, 'S');
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
        public void InsertUserDetailsAsync(Account user)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            user.createdDate = DateTime.Now;
            user.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_UserInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", user.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@userName", gs.CipherText(user.userName, "E")));
            sqlCmd.Parameters.Add(new SqlParameter("@password", gs.CipherText(user.password, "E")));
            sqlCmd.Parameters.Add(new SqlParameter("@memberId", user.memberId));
            sqlCmd.Parameters.Add(new SqlParameter("@subcontractorId", user.subcontractorId));
            sqlCmd.Parameters.Add(new SqlParameter("@roleId", user.roleId));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", user.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", user.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", user.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", user.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                user.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

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
        public void UpdateUserDetailsAsync(Account user)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();
            user.createdDate = DateTime.Now;
            user.modifiedDate = DateTime.Now;


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_UserInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", user.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@userName", gs.CipherText(user.userName, "E")));
            sqlCmd.Parameters.Add(new SqlParameter("@password", gs.CipherText(user.password, "E")));
            sqlCmd.Parameters.Add(new SqlParameter("@memberId", user.memberId));
            sqlCmd.Parameters.Add(new SqlParameter("@subcontractorId", user.subcontractorId));
            sqlCmd.Parameters.Add(new SqlParameter("@roleId", user.roleId));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", user.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", user.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", user.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", user.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();

                //  UpdateUserRatesDetailsAsync(UserRates, tran);
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
        public void DeleteUserDetailsAsync(int Id)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_UserDelete";
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
        }
    }
}
