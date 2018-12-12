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
using System.Collections;

namespace IP.Website.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        // GET: SORType
      string Baseurl = "http://ipjobsapi-dev.ap-southeast-2.elasticbeanstalk.com/";
      //  string Baseurl = "http://localhost:50087/";
        string MasterURL = "http://ipmasterapi-dev.ap-southeast-2.elasticbeanstalk.com/";
        //string MasterURL = "http://localhost:57225/";
        public ActionResult Index(int id = 0)
        {
            try
            {

                List<JobsModel> obj = new List<JobsModel>();
                var masters = new HttpClient();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    masters.BaseAddress = new Uri(MasterURL);
                    //HTTP GET
                    var responseTask = client.GetAsync("api/Jobs/get/" + id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var response = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the SORType list  
                        obj = JsonConvert.DeserializeObject<List<JobsModel>>(response);
                        ViewBag.ProjectList = new SelectList(obj[0].project, "Id", "projName");
                        ViewBag.ProjId = id;
                        ViewBag.TeamList = new SelectList(obj[0].team, "Id", "teamName");
                        ViewBag.SubContractorList = new SelectList(obj[0].subcontractor, "Id", "subconName");
                        ViewBag.JobStatusList = new SelectList(obj[0].jobstatus, "Id", "name");
                        ViewBag.ProjectJobTypeList = new SelectList(obj[0].projectjobtypes, "Id", "description");
                        ViewBag.ProjectRatesList = new SelectList(obj[0].projectRates, "Id", "SORCode");
                        ViewBag.MemberList = new SelectList(obj[0].members, "Id", "firstName");
                        ViewBag.JobRatesList = new SelectList(obj[0].jobRates, "Id", "SORCode");
                      
                        ArrayList resPriority = new ArrayList();
                        resPriority.Add("High");
                        resPriority.Add("Medium");
                        resPriority.Add("Low");

                        ViewBag.PriorityList = new SelectList(resPriority);
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

        public async Task<ActionResult> Insert(JobsModel proj)
        {
            try
            {
              
                
                JobsModel JobsInfo = new JobsModel();
              
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();

                    var obj = JsonConvert.SerializeObject(proj);

                    // Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllComapnies using HttpJobs 
                    HttpResponseMessage Res = await client.PostAsync("api/Jobs/insert", new StringContent(obj, Encoding.UTF8, "application/json"));

                    //Checking the response is successful or not which is sent using HttpJobs
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var JobsResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Company list  
                        JobsInfo = JsonConvert.DeserializeObject<JobsModel>(JobsResponse);
                    }

                    //returning the company list to view  
                    int id = proj.projId;
                    return RedirectToAction("Index",id);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }

        public ActionResult Update(JobsModel proj)
        {
            try
            {
                List<JobsModel> JobsInfo = new List<JobsModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.PutAsJsonAsync("api/Jobs/update", proj);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var JobsResponse = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Sub SORType  list  
                        JobsInfo = JsonConvert.DeserializeObject<List<JobsModel>>(JobsResponse);

                    }
                }
                int id = proj.projId;
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
                    //HTTP GET
                    var responseTask = client.DeleteAsync("api/Jobs/delete/" + ID);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");

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