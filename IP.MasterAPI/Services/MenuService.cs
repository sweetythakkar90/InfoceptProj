using IP.MasterAPI.Models;
using IP.MasterAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IP.MasterAPI.Services
{
    public class MenuService : IMenuRepository
    {
        private SqlConnection myconn;
        private GlobalServiceMethods gs;
        public MenuService()
        {
            DBService dsc = DBService.GetSqlInstance();
            gs = new GlobalServiceMethods();
            myconn = dsc.GetDBConnection();
        }
        public List<Menu> GetMenuDetails(int roleId, char mode)
        {
            SqlDataReader reader = null;
            if (myconn.State != ConnectionState.Open)
                myconn.Open();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = "SP_GetMenuDetails";
            sqlCmd.Parameters.Add(new SqlParameter("@roleId", roleId));
            sqlCmd.Parameters.Add(new SqlParameter("@mode", mode));
            sqlCmd.Connection = myconn;
            reader = sqlCmd.ExecuteReader();
            List<Menu> menu = new List<Menu>();
           
                while (reader.Read())
                {
                    menu.Add(new Menu()
                    {
                        Id = Convert.ToInt32(reader.GetValue(0)),
                        menuName = reader.GetValue(1).ToString(),
                        parentId = Convert.ToInt32(reader.GetValue(2)),
                        parentMenu = reader.GetValue(3).ToString(),
                        controllerName = reader.GetValue(4).ToString(),
                        actionName = reader.GetValue(5).ToString(),
                        roleId = Convert.ToInt32(reader.GetValue(6)),
                        roles = reader.GetValue(7).ToString(),
                        userRights = reader.GetValue(8).ToString()

                    });
                }
             
            if (myconn.State != ConnectionState.Closed)
                myconn.Close();


            return menu;
        }
       
    }
}