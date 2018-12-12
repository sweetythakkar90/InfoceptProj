using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.MasterAPI.Services
{
    public class ProjectService : IProjectRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        private ProjectMembersService pm;
        private ProjectSubContractorsService ps;
        private ProjectJobTypesService pj;
        private ProjectRatesService pr;

        private LocationService location;
        private ClientService client;
        private SORTypeService sortype;
        private SubSORTypeService subsortype;
        private TeamService team;
        private SubContractorService subcontractor;
        private StatusTypeService statusType;

        public ProjectService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            pm = new ProjectMembersService();
            ps= new ProjectSubContractorsService();
            pj= new ProjectJobTypesService();
            pr = new ProjectRatesService();

             location = new LocationService();
            client = new ClientService();
            sortype = new SORTypeService();
            subsortype = new SubSORTypeService();
            team = new TeamService();
            subcontractor = new SubContractorService();
            statusType = new StatusTypeService();

            myconn = dsc.GetDBConnection();
        }
        public List<Project> GetProjectDetailsAsync(int ID, int clientID)
        {
            try
            {
                DataTable dtProject = new DataTable();
                if (myconn.State != ConnectionState.Open)
                    myconn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "SP_ProjectGet";
                sqlCmd.Parameters.Add(new SqlParameter("@ID", ID));
                sqlCmd.Parameters.Add(new SqlParameter("@clientID", clientID));
                sqlCmd.Connection = myconn;
                SqlDataAdapter daProject = new SqlDataAdapter(sqlCmd);
                dtProject.Clear();
                daProject.Fill(dtProject);
                List<Project> lst = new List<Project>();
                if (dtProject.Rows.Count == 0)
                {
                    lst.Add(new Project());
                    lst[0].members = pm.GetProjectMembersDetailsAsync(0, 0);
                    lst[0].subContractors = ps.GetProjectSubContractorsDetailsAsync(0, 0);

                    lst[0].location = location.GetLocationDetailsAsync(0);
                    lst[0].client = client.GetClientDetailsAsync(0);
                    lst[0].sortype = sortype.GetSORTypeDetailsAsync(0);
                    lst[0].subsortype = subsortype.GetSubSORTypeDetailsAsync(0);
                    lst[0].team = team.GetTeamDetailsAsync(0, 'I');
                    lst[0].subcontractor = subcontractor.GetSubContractorDetailsAsync(0, 'I');
                    lst[0].statusType = statusType.GetStatusTypeDetailsAsync(0);
                }
                else
                {
                    foreach (DataRow item in dtProject.Rows)
                    {
                        lst.Add(new Project()
                        {

                            Id = Convert.ToInt32(item[0]),
                            projName = item[1].ToString(),
                            locId = Convert.ToInt32(item[2]),
                            locName = item[3].ToString(),
                            clientId = Convert.ToInt32(item[4]),
                            clientName = item[5].ToString(),
                            startDate = Convert.ToDateTime(item[6]),
                            endDate = Convert.ToDateTime(item[7]),
                            statusId = Convert.ToInt32(item[8]),
                            statusName = item[9].ToString(),
                            createdDate = Convert.ToDateTime(item[10]),
                            modifiedDate = item[11] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[11]),
                            userId = Convert.ToInt32(item[12]),
                            projRates = pr.GetProjectRatesDetailsAsync(0, Convert.ToInt32(item[0])),
                            projMembers = pm.GetProjectMembersDetailsAsync(0, Convert.ToInt32(item[0])),
                            projSubContractors = ps.GetProjectSubContractorsDetailsAsync(0, Convert.ToInt32(item[0])),
                            projJobTypes = pj.GetProjectJobTypesDetailsAsync(0, Convert.ToInt32(item[0]))

                        });

                    }
                    lst[0].members = pm.GetProjectMembersDetailsAsync(0, 0);
                    lst[0].subContractors = ps.GetProjectSubContractorsDetailsAsync(0, 0);

                    lst[0].location = location.GetLocationDetailsAsync(0);
                    lst[0].client = client.GetClientDetailsAsync(0);
                    lst[0].sortype = sortype.GetSORTypeDetailsAsync(0);
                    lst[0].subsortype = subsortype.GetSubSORTypeDetailsAsync(0);
                    lst[0].team = team.GetTeamDetailsAsync(0, 'I');
                    lst[0].subcontractor = subcontractor.GetSubContractorDetailsAsync(0, 'I');
                    lst[0].statusType = statusType.GetStatusTypeDetailsAsync(0);
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
        //private List<ProjectRates> GetProjectRatesDetailsAsync(int id,int projID)
        //{
        //    DataTable dtRates = new DataTable();
        //    SqlCommand sqlCmd = new SqlCommand();
        //    sqlCmd.CommandType = CommandType.StoredProcedure;
        //    sqlCmd.CommandText = "SP_ProjectRatesGet";
        //    sqlCmd.Parameters.Add(new SqlParameter("@id", id));
        //    sqlCmd.Parameters.Add(new SqlParameter("@projId", projID));

        //    sqlCmd.Connection = myconn;
        //    SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
        //    dtRates.Clear();
        //    da.Fill(dtRates);

        //    List<ProjectRates> lst = new List<ProjectRates>();
        //    foreach (DataRow item in dtRates.Rows)
        //    {
        //        lst.Add(new ProjectRates()
        //        {
        //            Id = Convert.ToInt32(item[0]),
        //            ProjId = Convert.ToInt32(item[1]),
        //            SORTypeId = Convert.ToInt32(item[2]),
        //            SORTypeName = item[3].ToString(),
        //            subSORTypeId = Convert.ToInt32(item[4]),
        //            subSORTypeName = item[5].ToString(),
        //            SORCode = item[6].ToString(),
        //            description = item[7].ToString(),
        //            unitOfMeasure = item[8].ToString(),
        //            unit = Convert.ToInt32(item[9]),
        //            unitPrice = Convert.ToDecimal(item[10]),
        //            cost = Convert.ToDecimal(item[11]),
        //            expiryDate = Convert.ToDateTime(item[12]),
        //            createdDate = Convert.ToDateTime(item[13]),
        //            modifiedDate = item[14] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[14]),
        //            userId = Convert.ToInt32(item[15])


        //        });
        //    }

        //    return lst;
        //}
        //private List<ProjectMembers> GetProjectMembersDetailsAsync(int id, int projID)
        //{
        //    DataTable dtMembers = new DataTable();
        //    SqlCommand sqlCmd = new SqlCommand();
        //    sqlCmd.CommandType = CommandType.StoredProcedure;
        //    sqlCmd.CommandText = "SP_ProjectMembersGet";
        //    sqlCmd.Parameters.Add(new SqlParameter("@id", id));
        //    sqlCmd.Parameters.Add(new SqlParameter("@projId", projID));

        //    sqlCmd.Connection = myconn;
        //    SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
        //    dtMembers.Clear();
        //    da.Fill(dtMembers);

        //    List<ProjectMembers> lst = new List<ProjectMembers>();
        //    foreach (DataRow item in dtMembers.Rows)
        //    {
        //        lst.Add(new ProjectMembers()
        //        {
        //            Id = Convert.ToInt32(item[0]),
        //            projId = Convert.ToInt32(item[1]),
        //            memberId = Convert.ToInt32(item[2]),
        //            memberName = item[3].ToString(),
        //            statusId = Cpmitem[4].ToString(),
        //            statusName = item[5].ToString(),
        //            createdDate = Convert.ToDateTime(item[4]),
        //            modifiedDate = item[5] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[5]),
        //            userId = Convert.ToInt32(item[6])


        //        });
        //    }

        //    return lst;
        //}
        //private List<ProjectSubContractors> GetProjectSubContractorsDetailsAsync(int id, int projID)
        //{
        //    DataTable dtMembers = new DataTable();
        //    SqlCommand sqlCmd = new SqlCommand();
        //    sqlCmd.CommandType = CommandType.StoredProcedure;
        //    sqlCmd.CommandText = "SP_ProjectSubContractorsGet";
        //    sqlCmd.Parameters.Add(new SqlParameter("@id", id));
        //    sqlCmd.Parameters.Add(new SqlParameter("@projId", projID));

        //    sqlCmd.Connection = myconn;
        //    SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
        //    dtMembers.Clear();
        //    da.Fill(dtMembers);

        //    List<ProjectSubContractors> lst = new List<ProjectSubContractors>();
        //    foreach (DataRow item in dtMembers.Rows)
        //    {
        //        lst.Add(new ProjectSubContractors()
        //        {
        //            Id = Convert.ToInt32(item[0]),
        //            projId = Convert.ToInt32(item[1]),
        //            subcontractorId = Convert.ToInt32(item[2]),
        //            subconName = item[3].ToString(),
        //            createdDate = Convert.ToDateTime(item[4]),
        //            modifiedDate = item[5] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[5]),
        //            userId = Convert.ToInt32(item[6])


        //        });
        //    }

        //    return lst;
        //}
        //private List<ProjectJobTypes> GetProjectJobTypesDetailsAsync(int id, int projID)
        //{
        //    DataTable dtMembers = new DataTable();
        //    SqlCommand sqlCmd = new SqlCommand();
        //    sqlCmd.CommandType = CommandType.StoredProcedure;
        //    sqlCmd.CommandText = "SP_ProjectJobTypesGet";
        //    sqlCmd.Parameters.Add(new SqlParameter("@id", id));
        //    sqlCmd.Parameters.Add(new SqlParameter("@projId", projID));

        //    sqlCmd.Connection = myconn;
        //    SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
        //    dtMembers.Clear();
        //    da.Fill(dtMembers);

        //    List<ProjectJobTypes> lst = new List<ProjectJobTypes>();
        //    foreach (DataRow item in dtMembers.Rows)
        //    {
        //        lst.Add(new ProjectJobTypes()
        //        {
        //            Id = Convert.ToInt32(item[0]),
        //            projId = Convert.ToInt32(item[1]),
        //            description = item[2].ToString(),
        //            createdDate = Convert.ToDateTime(item[3]),
        //            modifiedDate = item[4] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(item[4]),
        //            userId = Convert.ToInt32(item[5])


        //        });
        //    }

        //    return lst;
        //}

        public void InsertProjectDetailsAsync(Project proj)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            proj.createdDate = DateTime.Now;
            proj.modifiedDate = DateTime.Now;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ProjectInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "I"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", proj.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@projName", proj.projName));
            sqlCmd.Parameters.Add(new SqlParameter("@locId", proj.locId));
            sqlCmd.Parameters.Add(new SqlParameter("@clientId", proj.clientId));
            sqlCmd.Parameters.Add(new SqlParameter("@startDate", proj.startDate));
            sqlCmd.Parameters.Add(new SqlParameter("@endDate", proj.endDate));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", proj.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", proj.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", proj.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", proj.userId));


            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                proj.Id = Convert.ToInt32(sqlCmd.ExecuteScalar());

                // InsertProjectRateDetailsAsync(ProjectRates, Project.ProjectID, tran);
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
        public List<Project> UpdateProjectDetailsAsync(Project proj)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();
            proj.createdDate = DateTime.Now;
            proj.modifiedDate = DateTime.Now;


            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ProjectInsertUpdate";
            sqlCmd.Parameters.Add(new SqlParameter("@mode", "U"));
            sqlCmd.Parameters.Add(new SqlParameter("@Id", proj.Id));
            sqlCmd.Parameters.Add(new SqlParameter("@projName", proj.projName));
            sqlCmd.Parameters.Add(new SqlParameter("@locId", proj.locId));
            sqlCmd.Parameters.Add(new SqlParameter("@clientId", proj.clientId));
            sqlCmd.Parameters.Add(new SqlParameter("@startDate", proj.startDate));
            sqlCmd.Parameters.Add(new SqlParameter("@endDate", proj.endDate));
            sqlCmd.Parameters.Add(new SqlParameter("@statusId", proj.statusId));
            sqlCmd.Parameters.Add(new SqlParameter("@createdDate", proj.createdDate));
            sqlCmd.Parameters.Add(new SqlParameter("@modifiedDate", proj.modifiedDate));
            sqlCmd.Parameters.Add(new SqlParameter("@userId", proj.userId));
            try
            {
                sqlCmd.Connection = myconn;
                sqlCmd.Transaction = tran;

                sqlCmd.ExecuteNonQuery();

                //  UpdateProjectRatesDetailsAsync(ProjectRates, tran);
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
            return GetProjectDetailsAsync(0, proj.clientId);
        }
        public void DeleteProjectDetailsAsync(int ID)
        {
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlTransaction tran = myconn.BeginTransaction();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_ProjectDelete";
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
    }
}
