﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IP.Website.Models;
using IP.Website.Exceptions;
using System.Dynamic;

namespace IP.Website.Controllers
{
    [Authorize]
    public class MenuRolesRelationController : Controller
    {
        // GET: SORType
        string Baseurl = "http://ipmasterapi-dev.ap-southeast-2.elasticbeanstalk.com/";
        //string Baseurl = "http://localhost:57225/";
        public ActionResult Index(int roleId = 0)
        {
            try
            {
                List<MenuRolesRelationModel> obj = new List<MenuRolesRelationModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.GetAsync("api/MenuRolesRelation/get/" + roleId);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var response = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the SORType list  
                        obj = JsonConvert.DeserializeObject<List<MenuRolesRelationModel>>(response);
                    }

                }
                return View(obj);
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }

        public async Task<ActionResult> Insert(List<int> MenuSelect, int rId)
        {
            try
            {
                List<MenuRolesRelationModel> roleModel = new List<MenuRolesRelationModel>();

              //  var sSelect = Request.Form["MenuSelect"].Split(',');

                foreach (var item in MenuSelect)
                {
                    roleModel.Add(new MenuRolesRelationModel { Id = 0, menuId = Convert.ToInt32(item), roleId = rId });
                }
                MenuRolesRelationModel MenuRolesRelationInfo = new MenuRolesRelationModel();
                using (var client = new HttpClient())
                {
                    foreach (MenuRolesRelationModel cm in roleModel)
                    {

                        //Passing service base url
                        client.BaseAddress = new Uri(Baseurl);

                        client.DefaultRequestHeaders.Clear();

                        var obj = JsonConvert.SerializeObject(cm);

                        // Define request data format
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        //Sending request to find web api REST service resource GetAllComapnies using HttpClient 
                        HttpResponseMessage Res = await client.PostAsync("api/MenuRolesRelation/insert", new StringContent(obj, Encoding.UTF8, "application/json"));

                        //Checking the response is successful or not which is sent using HttpClient
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api 
                            var MenuRolesRelationResponse = Res.Content.ReadAsStringAsync().Result;

                            //Deserializing the response recieved from web api and storing into the Company list  
                            MenuRolesRelationInfo = JsonConvert.DeserializeObject<MenuRolesRelationModel>(MenuRolesRelationResponse);
                        }
                    }
                    //returning the company list to view  
                    return RedirectToAction("Index","Roles");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }

        public ActionResult Update(MenuRolesRelationModel cm)
        {
            try
            {
                List<MenuRolesRelationModel> MenuRolesRelationInfo = new List<MenuRolesRelationModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.PutAsJsonAsync("api/MenuRolesRelation/update", cm);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var MenuRolesRelationResponse = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Sub SORType  list  
                        MenuRolesRelationInfo = JsonConvert.DeserializeObject<List<MenuRolesRelationModel>>(MenuRolesRelationResponse);

                    }
                }

                return RedirectToAction("Index", "Roles");
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }

        public ActionResult Delete(int ID)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    string tableName = "tblMenuRolesRelation";
                    string fieldName = "MenuRolesId";
                    var responseTask1 = client.GetAsync("api/Global/CheckDataIntegrity/" + tableName + "/" + fieldName + "/" + ID);
                    responseTask1.Wait();
                    var result1 = responseTask1.Result;

                    if (result1.IsSuccessStatusCode)
                    {
                        var statusTypeResponse = result1.Content.ReadAsStringAsync().Result;
                        if (Convert.ToInt32(statusTypeResponse) == 0)
                        {
                            //HTTP GET
                            var responseTask = client.DeleteAsync("api/MenuRolesRelation/delete/" + ID);
                            responseTask.Wait();

                            var result = responseTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Index");

                            }
                        }
                        else
                        {
                            ViewBag.Message = "Data already in use";
                        }
                    }
                }
                return RedirectToAction("Index", "Roles");
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }
    }
}