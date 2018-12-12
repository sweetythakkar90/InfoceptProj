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
    public class JobDocumentsController : Controller
    {
        // GET: SORType
        string Baseurl = "http://ipjobsapi-dev.ap-southeast-2.elasticbeanstalk.com/";
        //string Baseurl = "http://localhost:50087/";
        public ActionResult Index()
        {
            try
            {
                List<JobDocumentsModel> obj = new List<JobDocumentsModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.GetAsync("api/JobDocuments/get");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var response = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the SORType list  
                        obj = JsonConvert.DeserializeObject<List<JobDocumentsModel>>(response);
                        var responseTask1 = client.GetAsync("api/statusType/get");
                        responseTask1.Wait();

                        var result1 = responseTask1.Result;

                        if (result1.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var response1 = result1.Content.ReadAsStringAsync().Result;
                            var res = JsonConvert.DeserializeObject<List<StatusTypeModel>>(response1);
                            //Deserializing the response recieved from web api and storing into the SORType list  
                            ViewBag.StatusList = new SelectList(res, "ID", "name");

                        }
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

        public async Task<ActionResult> Insert(JobDocumentsModel cm)
        {
            try
            {
              
             
                JobDocumentsModel JobDocumentsInfo = new JobDocumentsModel();
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();

                    var obj = JsonConvert.SerializeObject(cm);

                    // Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllComapnies using HttpClient 
                    HttpResponseMessage Res = await client.PostAsync("api/JobDocuments/insert", new StringContent(obj, Encoding.UTF8, "application/json"));

                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var JobDocumentsResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Company list  
                        JobDocumentsInfo = JsonConvert.DeserializeObject<JobDocumentsModel>(JobDocumentsResponse);
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

        public ActionResult Update(JobDocumentsModel cm)
        {
            try
            {
                List<JobDocumentsModel> JobDocumentsInfo = new List<JobDocumentsModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.PutAsJsonAsync("api/JobDocuments/update", cm);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var JobDocumentsResponse = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Sub SORType  list  
                        JobDocumentsInfo = JsonConvert.DeserializeObject<List<JobDocumentsModel>>(JobDocumentsResponse);

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
                    var responseTask = client.DeleteAsync("api/JobDocuments/delete/" + ID);
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