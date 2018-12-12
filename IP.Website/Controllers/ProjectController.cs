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
    public class ProjectController : Controller
    {
        // GET: SORType
        string Baseurl = "http://ipmasterapi-dev.ap-southeast-2.elasticbeanstalk.com/";
        //string Baseurl = "http://localhost:57225/";
        public ActionResult Index(int id = 0)
        {
            try
            {

                List<ProjectModel> obj = new List<ProjectModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.GetAsync("api/Project/get/" + id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var response = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the SORType list  
                        obj = JsonConvert.DeserializeObject<List<ProjectModel>>(response);

                        ViewBag.LocationList = new SelectList(obj[0].location, "ID", "name");
                        ViewBag.ClientList = new SelectList(obj[0].client, "Id", "clientName");
                        ViewBag.ClientID = id;
                        ViewBag.SORTypeList = new SelectList(obj[0].sortype, "ID", "name");
                        ViewBag.SubSORTypeList = new SelectList(obj[0].subsortype, "ID", "name");
                        ViewBag.TeamList = new SelectList(obj[0].team, "teamID", "teamName");
                        ViewBag.SubContractorList = new SelectList(obj[0].subcontractor, "Id", "subconName");
                        ViewBag.StatusTypeList = new SelectList(obj[0].statusType, "Id", "name");
                        ViewBag.RatesStatusList = new SelectList(obj[0].statusType, "ID", "name");
                        ViewBag.MembersStatusList = new SelectList(obj[0].statusType, "ID", "name");
                        ViewBag.SCStatusList = new SelectList(obj[0].statusType, "ID", "name");
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

        public async Task<ActionResult> Insert(ProjectModel proj)
        {
            try
            {
              
                
                ProjectModel ProjectInfo = new ProjectModel();
              
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();

                    var obj = JsonConvert.SerializeObject(proj);

                    // Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllComapnies using HttpProject 
                    HttpResponseMessage Res = await client.PostAsync("api/Project/insert", new StringContent(obj, Encoding.UTF8, "application/json"));

                    //Checking the response is successful or not which is sent using HttpProject
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var ProjectResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Company list  
                        ProjectInfo = JsonConvert.DeserializeObject<ProjectModel>(ProjectResponse);
                    }

                    //returning the company list to view  
                    int id = proj.clientId;
                    return RedirectToAction("Index",id);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }

        public ActionResult Update(ProjectModel proj)
        {
            try
            {
                List<ProjectModel> ProjectInfo = new List<ProjectModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.PutAsJsonAsync("api/Project/update", proj);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var ProjectResponse = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Sub SORType  list  
                        ProjectInfo = JsonConvert.DeserializeObject<List<ProjectModel>>(ProjectResponse);

                    }
                }
                int id = proj.clientId;
                return RedirectToAction("Index", id);
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
                    string tableName = "tblProject";
                    string fieldName = "projId";
                    string ignoreTableList = "'tblProjectRates','tblProjectMembers','tblProjectSubContractors','tblProjectJobTypes'";

                    var responseTask1 = client.GetAsync("api/Global/CheckDataIntegrity/" + tableName + "/" + fieldName + "/" + ID + "/" + ignoreTableList);
                    responseTask1.Wait();
                    var result1 = responseTask1.Result;

                    if (result1.IsSuccessStatusCode)
                    {
                        var statusTypeResponse = result1.Content.ReadAsStringAsync().Result;
                        if (Convert.ToInt32(statusTypeResponse) == 0)
                        {
                            //HTTP GET
                            var responseTask = client.DeleteAsync("api/Project/delete/" + ID);
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
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }
    }
}