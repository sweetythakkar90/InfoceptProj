using Newtonsoft.Json;
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
    public class ProjectMembersController : Controller
    {
        // GET: SORType
        string Baseurl = "http://ipmasterapi-dev.ap-southeast-2.elasticbeanstalk.com/";
        //string Baseurl = "http://localhost:57225/";
        public ActionResult Index()
        {
            try
            {
                List<ProjectMembersModel> obj = new List<ProjectMembersModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.GetAsync("api/ProjectMembers/get");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var response = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Members list  
                        obj = JsonConvert.DeserializeObject<List<ProjectMembersModel>>(response);
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

        public async Task<ActionResult> Insert(ProjectMembersModel pm)
        {
            try
            {
              
             
                ProjectMembersModel ProjectMembersInfo = new ProjectMembersModel();
                List<ProjectMembersModel> projectMembers = new List<ProjectMembersModel>();
                var memSelect = Request.Form["memberSelect"].Split(',');

                foreach (var item in memSelect)
                {
                    projectMembers.Add(new ProjectMembersModel { projId = pm.projId, memberId = Convert.ToInt32(item) });
                }

                foreach(ProjectMembersModel p in projectMembers)
                {

                
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);
                        p.statusId = 1;
                    client.DefaultRequestHeaders.Clear();

                    var obj = JsonConvert.SerializeObject(p);

                    // Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllComapnies using HttpClient 
                    HttpResponseMessage Res = await client.PostAsync("api/ProjectMembers/insert", new StringContent(obj, Encoding.UTF8, "application/json"));

                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var ProjectMembersResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Company list  
                        ProjectMembersInfo = JsonConvert.DeserializeObject<ProjectMembersModel>(ProjectMembersResponse);
                    }

                    //returning the company list to view  
                 
                }
                }
                return RedirectToAction("Index", "Project");
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }

        public ActionResult Update(ProjectMembersModel cm)
        {
            try
            {
                List<ProjectMembersModel> ProjectMembersInfo = new List<ProjectMembersModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.PutAsJsonAsync("api/ProjectMembers/update", cm);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var ProjectMembersResponse = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Sub SORType  list  
                        ProjectMembersInfo = JsonConvert.DeserializeObject<List<ProjectMembersModel>>(ProjectMembersResponse);

                    }
                }

                return RedirectToAction("Index", "Project");
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
                    string tableName = "tblProjectMembers";
                    string fieldName = "ProjMembersId";
                    var responseTask1 = client.GetAsync("api/Global/CheckDataIntegrity/" + tableName + "/" + fieldName + "/" + ID);
                    responseTask1.Wait();
                    var result1 = responseTask1.Result;

                    if (result1.IsSuccessStatusCode)
                    {
                        var statusTypeResponse = result1.Content.ReadAsStringAsync().Result;
                        if (Convert.ToInt32(statusTypeResponse) == 0)
                        {
                            //HTTP GET
                            var responseTask = client.DeleteAsync("api/ProjectMembers/delete/" + ID);
                            responseTask.Wait();

                            var result = responseTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Index", "Project");

                            }
                        }
                        else
                        {
                            ViewBag.Message = "Data already in use";
                        }
                    }
                }
                return RedirectToAction("Index", "Project");
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }
    }
}