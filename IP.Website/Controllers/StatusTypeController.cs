using IP.Website.Exceptions;
using IP.Website.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace IP.Website.Controllers
{
    [Authorize]
    public class StatusTypeController : Controller
    {
        // GET: SORType
        string Baseurl = "http://ipmasterapi-dev.ap-southeast-2.elasticbeanstalk.com/";
        //string Baseurl = "http://localhost:57225/";
        public ActionResult Index()
        {
            try
            {


                List<StatusTypeModel> obj = new List<StatusTypeModel>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.GetAsync("api/statustype/get");
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var response = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the SORType list  
                        obj = JsonConvert.DeserializeObject<List<StatusTypeModel>>(response);
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
        public async Task<ActionResult> Insert(StatusTypeModel statusType)
        {
            try
            {
                if (Session["acct"] != null)
                {
                    statusType.userId = ((AccountModel)Session["acct"]).uId;
                }
                StatusTypeModel SORTypeInfo = new StatusTypeModel();
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();

                    var stype = JsonConvert.SerializeObject(statusType);

                    // Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllComapnies using HttpClient 
                    HttpResponseMessage Res = await client.PostAsync("api/statustype/insert", new StringContent(stype, Encoding.UTF8, "application/json"));

                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var statusTypeResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Company list  
                        SORTypeInfo = JsonConvert.DeserializeObject<StatusTypeModel>(statusTypeResponse);
                    }

                    //returning the company list to view  
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }


        public ActionResult Update(StatusTypeModel statusType)
        {
            try
            {
                if (Session["acct"] != null)
                {
                    statusType.userId = ((AccountModel)Session["acct"]).uId;
                }
                List<StatusTypeModel> statusTypeInfo = new List<StatusTypeModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.PutAsJsonAsync("api/statustype/update", statusType);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var statusTypeResponse = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Company list  
                        statusTypeInfo = JsonConvert.DeserializeObject<List<StatusTypeModel>>(statusTypeResponse);

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

        public ActionResult Delete(int ID)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);

                    string tableName = "tblStatus";
                    string fieldName = "statusId";
                    var responseTask1 = client.GetAsync("api/Global/CheckDataIntegrity/" + tableName + "/" + fieldName + "/" + ID);
                    responseTask1.Wait();
                    var result1 = responseTask1.Result;

                    if (result1.IsSuccessStatusCode)
                    {
                        var statusTypeResponse = result1.Content.ReadAsStringAsync().Result;
                        if (Convert.ToInt32(statusTypeResponse) == 0)
                        {
                            //HTTP GET
                            var responseTask = client.DeleteAsync("api/statustype/delete/" + ID);
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
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }


            return RedirectToAction("Index");
        }
    }
}
