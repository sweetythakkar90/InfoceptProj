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
    public class ProjectSubContractorsController : Controller
    {
        // GET: StatusType
        string Baseurl = "http://ipmasterapi-dev.ap-southeast-2.elasticbeanstalk.com/";
        //string Baseurl = "http://localhost:57225/";
        public ActionResult Index()
        {
            try
            {
                List<ProjectSubContractorsModel> obj = new List<ProjectSubContractorsModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.GetAsync("api/ProjectSubContractors/get");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var response = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the SubContractor list  
                        obj = JsonConvert.DeserializeObject<List<ProjectSubContractorsModel>>(response);
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

        public async Task<ActionResult> Insert(ProjectSubContractorsModel pm)
        {
            try
            {


                ProjectSubContractorsModel ProjectSubContractorsInfo = new ProjectSubContractorsModel();
                List<ProjectSubContractorsModel> projectSubContractors = new List<ProjectSubContractorsModel>();
                var memSelect = Request.Form["subContractorsSelect"].Split(',');

                foreach (var item in memSelect)
                {
                    projectSubContractors.Add(new ProjectSubContractorsModel { projId = pm.projId, subcontractorId = Convert.ToInt32(item) });
                }

                foreach (ProjectSubContractorsModel p in projectSubContractors)
                {


                    using (var client = new HttpClient())
                    {
                        //Passing service base url
                        client.BaseAddress = new Uri(Baseurl);

                        client.DefaultRequestHeaders.Clear();

                        var obj = JsonConvert.SerializeObject(p);

                        // Define request data format
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        //Sending request to find web api REST service resource GetAllComapnies using HttpClient 
                        HttpResponseMessage Res = await client.PostAsync("api/ProjectSubContractors/insert", new StringContent(obj, Encoding.UTF8, "application/json"));

                        //Checking the response is successful or not which is sent using HttpClient
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api 
                            var ProjectSubContractorsResponse = Res.Content.ReadAsStringAsync().Result;

                            //Deserializing the response recieved from web api and storing into the Company list  
                            ProjectSubContractorsInfo = JsonConvert.DeserializeObject<ProjectSubContractorsModel>(ProjectSubContractorsResponse);
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

        public ActionResult Update(ProjectSubContractorsModel cm)
        {
            try
            {
                List<ProjectSubContractorsModel> ProjectSubContractorsInfo = new List<ProjectSubContractorsModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.PutAsJsonAsync("api/ProjectSubContractors/update", cm);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var ProjectSubContractorsResponse = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Sub StatusType  list  
                        ProjectSubContractorsInfo = JsonConvert.DeserializeObject<List<ProjectSubContractorsModel>>(ProjectSubContractorsResponse);

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
                    string tableName = "tblProjectSubContractors";
                    string fieldName = "ProjSubContractorsId";
                    var responseTask1 = client.GetAsync("api/Global/CheckDataIntegrity/" + tableName + "/" + fieldName + "/" + ID);
                    responseTask1.Wait();
                    var result1 = responseTask1.Result;

                    if (result1.IsSuccessStatusCode)
                    {
                        var statusTypeResponse = result1.Content.ReadAsStringAsync().Result;
                        if (Convert.ToInt32(statusTypeResponse) == 0)
                        {
                            //HTTP GET
                            var responseTask = client.DeleteAsync("api/ProjectSubContractors/delete/" + ID);
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