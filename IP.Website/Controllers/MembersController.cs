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
    public class MembersController : Controller
    {
        // GET: SORType
        string Baseurl = "http://ipmasterapi-dev.ap-southeast-2.elasticbeanstalk.com/";
        //string Baseurl = "http://localhost:57225/";
        public ActionResult Index()
        {
            try
            {
                List<MembersModel> obj = new List<MembersModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.GetAsync("api/members/get");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var response = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Members list  
                        obj = JsonConvert.DeserializeObject<List<MembersModel>>(response);

                        var responseTask1 = client.GetAsync("api/statusType/get");
                        responseTask1.Wait();

                        var result1 = responseTask1.Result;

                        if (result1.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var response1 = result1.Content.ReadAsStringAsync().Result;
                            var res = JsonConvert.DeserializeObject<List<StatusTypeModel>>(response1);
                            //Deserializing the response recieved from web api and storing into the StatusType list  
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
        public ActionResult GetMembersSkillSets(int ID)
        {
            try
            {
                List<MembersSkillSetMappingModel> obj = new List<MembersSkillSetMappingModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    string tableName = "tblMembers";
                    string fieldName = "memberId";
                    var responseTask1 = client.GetAsync("api/Global/CheckDataIntegrity/" + tableName + "/" + fieldName + "/" + ID);
                    responseTask1.Wait();
                    var result1 = responseTask1.Result;

                    if (result1.IsSuccessStatusCode)
                    {
                        var statusTypeResponse = result1.Content.ReadAsStringAsync().Result;
                        if (Convert.ToInt32(statusTypeResponse) == 0)
                        {
                            //HTTP GET
                            var responseTask = client.GetAsync("api/members/GetMember/" + ID);
                            responseTask.Wait();

                            var result = responseTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                //Storing the response details recieved from web api   
                                var response = result.Content.ReadAsStringAsync().Result;

                                //Deserializing the response recieved from web api and storing into the SORType list  
                                obj = JsonConvert.DeserializeObject<List<MembersSkillSetMappingModel>>(response);
                            }
                        }
                        else
                        {
                            ViewBag.Message = "Data already in use";
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
        public async Task<ActionResult> Insert(MembersModel member)
        {
            try
            {
            
                MembersModel membersInfo = new MembersModel();
                var sSelect = Request.Form["skillSelect"].Split(',');

                foreach (var item in sSelect)
                {
                    member.skillSelect.Add(new MembersSkillSetMappingModel {memberId = 0,skillSetId= Convert.ToInt32(item)   });
                }
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();

                    var obj = JsonConvert.SerializeObject(member);

                    // Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllComapnies using HttpClient 
                    HttpResponseMessage Res = await client.PostAsync("api/members/insert", new StringContent(obj, Encoding.UTF8, "application/json"));

                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var membersResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Company list  
                        membersInfo = JsonConvert.DeserializeObject<MembersModel>(membersResponse);
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

        public ActionResult Update(MembersModel members)
        {
            try
            {
                List<MembersModel> membersInfo = new List<MembersModel>();
                var sSelect = Request.Form["skillSelect"].Split(',');

                foreach (var item in sSelect)
                {
                    members.skillSelect.Add(new MembersSkillSetMappingModel { memberId = members.Id, skillSetId = Convert.ToInt32(item) });
                }
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP GET
                    var responseTask = client.PutAsJsonAsync("api/members/update", members);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var membersResponse = result.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Sub SORType  list  
                        membersInfo = JsonConvert.DeserializeObject<List<MembersModel>>(membersResponse);

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
                    string tableName = "tblMembers";
                    string fieldName = "memberId";
                    var responseTask1 = client.GetAsync("api/Global/CheckDataIntegrity/" + tableName + "/" + fieldName + "/" + ID);
                    responseTask1.Wait();
                    var result1 = responseTask1.Result;

                    if (result1.IsSuccessStatusCode)
                    {
                        var statusTypeResponse = result1.Content.ReadAsStringAsync().Result;
                        if (Convert.ToInt32(statusTypeResponse) == 0)
                        {
                            //HTTP GET
                            var responseTask = client.DeleteAsync("api/members/delete/" + ID);
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
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogHandler.LogData(ex);

                throw ex;
            }
        }
    }
}