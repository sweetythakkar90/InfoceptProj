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

namespace IP.Website.Controllers
{
    [Authorize]
    public class SubSORTypeController : Controller
    {
        // GET: SORType
        string Baseurl = "http://ipmasterapi-dev.ap-southeast-2.elasticbeanstalk.com/";
        //string Baseurl = "http://localhost:57225/";
        public ActionResult Index()
        {
            try
            {
                List<SubSORTypeModel> obj = new List<SubSORTypeModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.GetAsync("api/subsortype/get");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var response = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the SORType list  
                        obj = JsonConvert.DeserializeObject<List<SubSORTypeModel>>(response);

                        var responseTask1 = client.GetAsync("api/sortype/get");
                        responseTask1.Wait();

                        var result1 = responseTask1.Result;
                        if (result1.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var response1 = result1.Content.ReadAsStringAsync().Result;
                            var res = JsonConvert.DeserializeObject<List<SORTypeModel>>(response1);
                            //Deserializing the response recieved from web api and storing into the SORType list  
                            ViewBag.SORList = new SelectList(res, "ID", "name");

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
        public List<SubSORTypeModel> GetSubSORType(int SORTypeId)
        {
            try
            {
                List<SubSORTypeModel> obj = new List<SubSORTypeModel>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.GetAsync("api/subsortype/GetSpecificSubSORType/" + SORTypeId);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var response = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the SORType list  
                        obj = JsonConvert.DeserializeObject<List<SubSORTypeModel>>(response);
                    }

                }
                return obj;
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);
                throw ex;
            }
        }
        public async Task<ActionResult> Insert(SubSORTypeModel subSORType)
        {
            try
            {
                SubSORTypeModel SORTypeInfo = new SubSORTypeModel();
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();

                    var stype = JsonConvert.SerializeObject(subSORType);

                    // Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllComapnies using HttpClient 
                    HttpResponseMessage Res = await client.PostAsync("api/subsortype/insert", new StringContent(stype, Encoding.UTF8, "application/json"));

                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var SORTypeResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Company list  
                        SORTypeInfo = JsonConvert.DeserializeObject<SubSORTypeModel>(SORTypeResponse);
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

        public ActionResult Update(SubSORTypeModel subSORType)
        {
            try
            {
                List<SubSORTypeModel> subSORTypeInfo = new List<SubSORTypeModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.PutAsJsonAsync("api/subsortype/update", subSORType);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var subSORTypeResponse = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Sub SORType  list  
                        subSORTypeInfo = JsonConvert.DeserializeObject<List<SubSORTypeModel>>(subSORTypeResponse);

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

                    string tableName = "tblSubSORType";
                    string fieldName = "SubSORTypeID";
                    var responseTask1 = client.GetAsync("api/Global/CheckDataIntegrity/" + tableName + "/" + fieldName + "/" + ID);
                    responseTask1.Wait();
                    var result1 = responseTask1.Result;

                    if (result1.IsSuccessStatusCode)
                    {
                        var statusTypeResponse = result1.Content.ReadAsStringAsync().Result;
                        if (Convert.ToInt32(statusTypeResponse) == 0)
                        {
                            //HTTP GET
                            var responseTask = client.DeleteAsync("api/subsortype/delete/" + ID);
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