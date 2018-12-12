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
using System.Web.Script.Serialization;

namespace IP.Website.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        // GET: SORType
        string Baseurl = "http://ipmasterapi-dev.ap-southeast-2.elasticbeanstalk.com/";
        //string Baseurl = "http://localhost:57225/";
        public ActionResult Index()
        {
            try
            {


                List<RolesModel> obj = new List<RolesModel>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.GetAsync("api/Roles/get");
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var response = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the SORType list  
                        obj = JsonConvert.DeserializeObject<List<RolesModel>>(response);
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
        public async Task<ActionResult> Insert(RolesModel roles)
        {
            try
            {
                if (Session["acct"] != null)
                {
                    roles.userId = ((AccountModel)Session["acct"]).uId;
                }
                RolesModel SORTypeInfo = new RolesModel();
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();

                    var stype = JsonConvert.SerializeObject(roles);

                    // Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllComapnies using HttpClient 
                    HttpResponseMessage Res = await client.PostAsync("api/Roles/insert", new StringContent(stype, Encoding.UTF8, "application/json"));

                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var RolesResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Company list  
                        SORTypeInfo = JsonConvert.DeserializeObject<RolesModel>(RolesResponse);
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

        public ActionResult Update(RolesModel roles)
        {
            try
            {
                if (Session["acct"] != null)
                {
                    roles.userId = ((AccountModel)Session["acct"]).uId;
                }
                List<RolesModel> RolesInfo = new List<RolesModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.PutAsJsonAsync("api/Roles/update", roles);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var RolesResponse = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Company list  
                        RolesInfo = JsonConvert.DeserializeObject<List<RolesModel>>(RolesResponse);

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

                    string tableName = "tblRoles";
                    string fieldName = "roleId";
                    var responseTask1 = client.GetAsync("api/Global/CheckDataIntegrity/" + tableName + "/" + fieldName + "/" + ID);
                    responseTask1.Wait();
                    var result1 = responseTask1.Result;

                    if (result1.IsSuccessStatusCode)
                    {
                        var RolesResponse = result1.Content.ReadAsStringAsync().Result;
                        if (Convert.ToInt32(RolesResponse) == 0)
                        {
                            //HTTP GET
                            var responseTask = client.DeleteAsync("api/Roles/delete/" + ID);
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
