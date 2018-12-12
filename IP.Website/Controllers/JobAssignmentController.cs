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
    public class JobAssignmentController : Controller
    {
        // GET: SORType
        string Baseurl = "http://ipjobsapi-dev.ap-southeast-2.elasticbeanstalk.com/";
        //string Baseurl = "http://localhost:50087/";
        public ActionResult Index()
        {
            try
            {
                List<JobAssignmentModel> obj = new List<JobAssignmentModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.GetAsync("api/JobAssignment/get");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var response = result.Content.ReadAsStringAsync().Result;
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

        public async Task<ActionResult> Insert(JobAssignmentModel cm)
        {
            try
            {
              
             
                JobAssignmentModel JobAssignmentInfo = new JobAssignmentModel();
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();

                    var obj = JsonConvert.SerializeObject(cm);

                    // Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllComapnies using HttpClient 
                    HttpResponseMessage Res = await client.PostAsync("api/JobAssignment/insert", new StringContent(obj, Encoding.UTF8, "application/json"));

                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var JobAssignmentResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Company list  
                        JobAssignmentInfo = JsonConvert.DeserializeObject<JobAssignmentModel>(JobAssignmentResponse);
                    }

                    //returning the company list to view  
                    return RedirectToAction("Index","Jobs");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }

        public ActionResult Update(JobAssignmentModel cm)
        {
            try
            {
                List<JobAssignmentModel> JobAssignmentInfo = new List<JobAssignmentModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.PutAsJsonAsync("api/JobAssignment/update", cm);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var JobAssignmentResponse = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Sub SORType  list  
                        JobAssignmentInfo = JsonConvert.DeserializeObject<List<JobAssignmentModel>>(JobAssignmentResponse);

                    }
                }

                return RedirectToAction("Index", "Jobs");
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
                    var responseTask = client.DeleteAsync("api/JobAssignment/delete/" + ID);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");

                    }
                }
                return RedirectToAction("Index", "Jobs");
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }
    }
}